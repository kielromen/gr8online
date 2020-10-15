Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class BankMasterfile_LoadList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                LoadBankList()
            End If
        End If
    End Sub

    Public Sub LoadBankList()
        Dim query As String
        query = " SELECT * FROM tblBank" &
                              " LEFT JOIN" &
                              " (SELECT AccountCode,AccountTitle FROM tblCOA) AS tblCOA  ON" &
                              " tblCOA.AccountCode = tblBank.AccountCode " &
                              " WHERE tblBank.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        dgvBankList.DataSource = SQL.SQLDS
        dgvBankList.DataBind()
    End Sub

    Private Sub dgvBankList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvBankList.PageIndexChanging
        dgvBankList.PageIndex = e.NewPageIndex
        LoadBankList()
    End Sub

    Private Sub dgvBankList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvBankList.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblBank SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='BankMasterfile_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvBankList.Rows.Count > 0 Then
            'To Export all pages
            dgvBankList.AllowPaging = False
            Me.LoadBankList()

            Dim dt As New DataTable("Banklist")
            For Each cell As TableCell In dgvBankList.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In dgvBankList.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Banklist.xlsx")
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