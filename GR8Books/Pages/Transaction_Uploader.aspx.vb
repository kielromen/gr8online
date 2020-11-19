Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class Transaction_Uploader
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim APVNO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "UB"
    Dim ColumnPK As String = "UB_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblJE_Upload"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Dim dt As New DataTable
                dt.Columns.Add("")
                gvUpload.DataSource = dt
                gvUpload.DataBind()
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Loadlist()
        If Session("ID") <> "" Then

            Dim query As String
            query = " SELECT REPLACE(CAST(AppDate as DATE),' 12:00:00 AM','') as AppDate, RefType, TransNo, AccntCode, AccntTitle, VCECode, VCEName, CONVERT(VARCHAR,CONVERT(MONEY,Debit),1) AS Debit, CONVERT(VARCHAR,CONVERT(MONEY,Credit),1) AS Credit,Particulars, RefNo, CostCenter FROM View_GL " &
                " WHERE View_GL.RefTransID = @RefTransID AND Upload = @Upload"
            SQL.FlushParams()
            SQL.AddParam("@RefTransID", Session("ID"))
            SQL.AddParam("@Upload", True)
            SQL.GetQuery(query)

            gvUploader.Columns(5).FooterText = "Total"
            gvUploader.Columns(5).FooterStyle.HorizontalAlign = HorizontalAlign.Right
            gvUploader.Columns(7).FooterStyle.HorizontalAlign = HorizontalAlign.Right
            gvUploader.Columns(8).FooterStyle.HorizontalAlign = HorizontalAlign.Right
            totalDebit1 = 0
            totalCredit1 = 0

            gvUploader.DataSource = SQL.SQLDS
            gvUploader.DataBind()
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvUploader.Rows.Count > 0 Then
            'To Export all pages
            gvUploader.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Uploaded")
            For Each cell As TableCell In gvUploader.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvUploader.Rows
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    row.Cells(i).CssClass = "textmode"
                    dt.Rows(dt.Rows.Count - 1)(i) = HttpUtility.HtmlDecode(row.Cells(i).Text.Trim)
                Next
            Next
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=Uploaded.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim i As String = gvUploader.Rows(0).Cells(0).Text

        Dim query As String
        query = " UPDATE  " & DBTable & " SET Status ='Cancelled' WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("ID"))
        SQL.ExecNonQuery(query)

        query = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND Upload = 1 "
        SQL.FlushParams()
        SQL.AddParam("@RefTransID", Session("ID"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnUploadSave_Click(sender As Object, e As EventArgs) Handles btnUploadSave.Click

        File_Upload.PostedFile.SaveAs(Server.MapPath("~/Uploads/Transaction.xlms"))

        Dim cn As System.Data.OleDb.OleDbConnection
        Dim cmd As System.Data.OleDb.OleDbCommand
        Dim dr As System.Data.OleDb.OleDbDataReader
        cn = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;" & "data source=" & Server.MapPath("~/Uploads/Transaction.xlms") & ";Extended Properties=Excel 12.0;")

        Try
            'SQL.BeginTransaction()

            cn.Open()
            cmd = cn.CreateCommand
            cmd.CommandText = "SELECT * FROM [Transaction$] ORDER BY Date, Doc_Type, Doc_No, Debit DESC, Credit "
            dr = cmd.ExecuteReader

            If dr.HasRows Then
                Dim TransID As String = ""
                TransID = SaveTransID()
                While dr.Read
                    'HEADER
                    If dr("Doc_Type").ToString <> "" Then


                        Dim JE_No As String = ""
                        Dim RefType As String = dr("Doc_Type").ToString
                        Dim RefTransID As String = dr("Doc_No").ToString
                        Dim AppDate As String = dr("Date").ToString
                        Dim Book As String = dr("Book").ToString

                        'DETAILS
                        Dim AccntCode As String = dr("AccntCode").ToString
                        Dim VCECode As String = dr("VCECode").ToString
                        Dim Debit As Decimal = CDec(IIf(IsNumeric(dr("Debit").ToString), dr("Debit").ToString, 0))
                        Dim Credit As Decimal = CDec(IIf(IsNumeric(dr("Credit").ToString), dr("Credit").ToString, 0))
                        Dim Particulars As String = dr("Particulars").ToString
                        Dim RefNo As String = dr("RefNo").ToString
                        Dim CostCenter As String = dr("CostCenter").ToString

                        If CheckDuplicateRef(RefType, RefTransID, TransID) Then
                            JE_No = GetJE_No(RefType, RefTransID, TransID)
                        Else
                            SaveJE_Header(TransID, RefType, RefTransID, AppDate, Book)
                            JE_No = GetJE_No(RefType, RefTransID, TransID)
                        End If

                        SaveJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo)
                    End If
                End While
            End If
            cn.Close()

            'SQL.Commit()

        Catch ex As Exception
            cn.Close()
            'SQL.Rollback()
            MsgBox(ex.Message)
            MsgBox("Sheet name should be Transaction!")
        Finally
            cn.Close()
        End Try

    End Sub

    Private Function CheckDuplicateRef(ByVal strRefType As String, ByVal strRefTransID As String, ByVal strTransID As String) As Boolean
        Dim selectSQL As String = ""
        selectSQL = " SELECT JE_No FROM tblJE_Header " & vbCrLf &
                    " WHERE RefType = @RefType AND  RefTransID = @RefTransID AND Remarks = @Remarks AND Upload = 1 "
        SQL.FlushParams()
        SQL.AddParam("@RefType", strRefType)
        SQL.AddParam("@Remarks", strRefTransID)
        SQL.AddParam("@RefTransID", strTransID)
        SQL.ReadQuery(selectSQL)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetJE_No(ByVal strRefType As String, ByVal strRefTransID As String, ByVal strTransID As String) As Integer
        Dim selectSQL As String = ""
        selectSQL = " SELECT JE_No FROM tblJE_Header " & vbCrLf &
                    " WHERE RefType = @RefType AND Remarks = @Remarks AND Upload = 1 AND RefTransID = @RefTransID  "
        SQL.FlushParams()
        SQL.AddParam("@RefType", strRefType)
        SQL.AddParam("@Remarks", strRefTransID)
        SQL.AddParam("@RefTransID", strTransID)
        SQL.ReadQuery(selectSQL)
        If SQL.SQLDR.Read Then
            Return CInt(SQL.SQLDR("JE_No").ToString)
        Else
            Return 0
        End If
    End Function


    Private Function SaveTransID() As Integer
        Dim selectSQL As String = " SELECT ISNULL(MAX(UB_No), 0) + 1 AS UB_No, ISNULL(MAX(TransID), 0) + 1 AS TransID FROM tblJE_Upload "
        SQL.FlushParams()
        SQL.ReadQuery(selectSQL)
        If SQL.SQLDR.Read Then
            Dim UB_No As Integer = CInt(SQL.SQLDR("UB_No").ToString)
            Dim TransID As Integer = CInt(SQL.SQLDR("TransID").ToString)
            Dim insertSQL As String = " INSERT INTO tblJE_Upload(TransID, UB_No, TransDate) " & vbCrLf &
                                      " VALUES(@TransID, @UB_No, GETDATE()) "
            SQL.FlushParams()
            SQL.AddParam("@TransID", TransID)
            SQL.AddParam("@UB_No", UB_No)
            SQL.ExecNonQuery(insertSQL)
            Return UB_No
        Else
            Return 1
        End If
    End Function

    Private Sub SaveJE_Header(ByVal TransID As String, ByVal RefType As String, ByVal RefTransID As String, ByVal AppDate As String, ByVal Book As String)

        Dim insertSQL As String = " INSERT INTO tblJE_Header(AppDate, RefType, RefTransID, Book, Remarks, Upload, DateCreated, WhoCreated) " & vbCrLf &
                                  " VALUES(@AppDate, @RefType, @RefTransID, @Book, @Remarks, @Upload, @DateCreated, @WhoCreated) "
        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@RefType", RefType)
        SQL.AddParam("@RefTransID", TransID)
        SQL.AddParam("@Book", Book)
        SQL.AddParam("@Remarks", RefTransID)
        SQL.AddParam("@Upload", True)
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)
    End Sub


    Private Sub SaveJE_Details(JE_No As String, AccntCode As String, VCECode As String, Debit As String, Credit As String, Particulars As String, RefNo As String)

        Dim insertSQL As String = " INSERT INTO tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo) " & vbCrLf &
                                  " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo) "
        SQL.FlushParams()
        SQL.AddParam("@JE_No", JE_No)
        SQL.AddParam("@AccntCode", AccntCode)
        SQL.AddParam("@VCECode", VCECode)
        SQL.AddParam("@Debit", Debit)
        SQL.AddParam("@Credit", Credit)
        SQL.AddParam("@Particulars", Particulars)
        SQL.AddParam("@RefNo", RefNo)
        SQL.ExecNonQuery(insertSQL)
    End Sub

    Dim totalDebit1, totalCredit1 As Decimal
    Private Sub gvUploader_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUploader.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = e.Row.DataItem
            totalDebit1 += Convert.ToDouble(row(7))
            totalCredit1 += Convert.ToDouble(row(8))
            gvUploader.Columns(7).FooterText = totalDebit1.ToString("N2")
            gvUploader.Columns(8).FooterText = totalCredit1.ToString("N2")
        End If
    End Sub


End Class