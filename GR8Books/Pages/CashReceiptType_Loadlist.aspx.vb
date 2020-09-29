Public Class CashReceiptType_Loadlist
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
        query = " SELECT * FROM tblCollection_Type " &
                " LEFT JOIN " &
                " tblCOA ON tblCollection_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE tblCollection_Type.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvCollectionType.DataSource = SQL.SQLDS
        gvCollectionType.DataBind()
    End Sub

    Private Sub gvCollectionType_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCollectionType.PageIndexChanging
        gvCollectionType.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvCollectionType_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCollectionType.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCollection_Type SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='CollectionType_Loadlist.aspx';</script>")
        End If
    End Sub
End Class