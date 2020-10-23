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
End Class