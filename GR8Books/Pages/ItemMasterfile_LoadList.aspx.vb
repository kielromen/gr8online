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
End Class