Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class DefaultAccounts_Loadlist
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
        query = " SELECT ID,  ModuleID, Description, tblDefaultAccount.AccountCode, AccountTitle, RefAccount, RefAccountTitle, Status, " &
                " 	   DateCreated, DateModified, WhoCreated, WhoModified " &
                " FROM tblDefaultAccount  " &
                " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS DefaultAccount " &
                " ON tblDefaultAccount.AccountCode = DefaultAccount.AccountCode " &
                " LEFT JOIN (SELECT AccountCode, AccountTitle AS RefAccountTitle FROM tblCOA) AS RefAccount " &
                " ON tblDefaultAccount.RefAccount = RefAccount.AccountCode " &
                " WHERE tblDefaultAccount.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvDefaultAccount.DataSource = SQL.SQLDS
        gvDefaultAccount.DataBind()
    End Sub

    Private Sub gvDefaultAccount_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDefaultAccount.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblDefaultAccount SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='DefaultAccount_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvDefaultAccount.Rows.Count > 0 Then
            'To Export all pages
            gvDefaultAccount.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("DefaultAccountList")
            For Each cell As TableCell In gvDefaultAccount.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvDefaultAccount.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=DefaultAccountList.xlsx")
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