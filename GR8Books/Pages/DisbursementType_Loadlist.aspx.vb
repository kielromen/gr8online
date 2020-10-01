Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel

Public Class DisbursementType_Loadlist
    Inherits System.Web.UI.Page

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
        Dim query As String
        query = " SELECT * FROM tblDisbursement_Type " &
                " INNER JOIN " &
                " tblCOA ON tblDisbursement_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE tblDisbursement_Type.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvDisbursementType.DataSource = SQL.SQLDS
        gvDisbursementType.DataBind()
    End Sub

    Private Sub gvDisbursementType_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDisbursementType.PageIndexChanging
        gvDisbursementType.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvDisbursementType_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDisbursementType.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblDisbursement_Type SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='DisbursementType_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvDisbursementType.Rows.Count > 0 Then
            'To Export all pages
            gvDisbursementType.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("DisbursementType")
            For Each cell As TableCell In gvDisbursementType.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvDisbursementType.Rows
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    row.Cells(i).CssClass = "textmode"
                    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text.ToString.Replace("&nbsp;", "")
                Next
            Next
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=DisbursementType.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub
End Class