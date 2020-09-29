Public Class Collector_Loadlist
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
        query = " SELECT * FROM tblCollector " &
                " WHERE tblCollector.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvCollector.DataSource = SQL.SQLDS
        gvCollector.DataBind()
    End Sub

    Private Sub gvCollector_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCollector.PageIndexChanging
        gvCollector.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvCollector_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCollector.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCollector SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='Collector_Loadlist.aspx';</script>")
        End If
    End Sub
End Class