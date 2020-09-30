Imports System.IO

Imports ClosedXML.Excel
Public Class ItemMasterfile_LoadList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = True Then
                If Session("UserRole") = "SystemAdmin" Then
                    LoadItemList()
                Else
                    Response.Redirect("Login.aspx")
                End If
            End If

        End If
    End Sub

    Public Sub LoadItemList()
        Dim query As String
        query = " SELECT * from tblItem_Master " &
                " WHERE tblItem_Master.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetDataTable(query)
        dgvItemList.DataSource = SQL.SQLDT
        dgvItemList.DataBind()
    End Sub

    Private Sub dgvItemList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvItemList.PageIndexChanging
        dgvItemList.PageIndex = e.NewPageIndex
        LoadItemList()
    End Sub

    Private Sub dgvItemList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvItemList.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblItem_Master  SET Status = @Status WHERE ItemCode = @ItemCode"
            SQL.FlushParams()
            SQL.AddParam("@ItemCode", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='ItemMasterfile_Loadlist.aspx';</script>")
        End If
    End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim imageUrl As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(dr("ItemPhoto"), Byte()))
                CType(e.Row.FindControl("ItemPhoto"), Image).ImageUrl = imageUrl
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvItemList.Rows.Count > 0 Then
            'To Export all pages
            dgvItemList.AllowPaging = False
            Me.LoadItemList()

            Dim dt As New DataTable("Itemlist")
            For Each cell As TableCell In dgvItemList.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In dgvItemList.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Itemlist.xlsx")
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