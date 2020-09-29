Public Class MemberManagement_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = "SELECT Member_Code, Member_Name, Member_Type, Status FROM tblMember_Master WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvMember.DataSource = SQL.SQLDS
        gvMember.DataBind()
    End Sub

    Private Sub gvMember_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMember.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblMember_Master SET Status = @Status WHERE Member_Code = @Member_Code"
            SQL.FlushParams()
            SQL.AddParam("@Member_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='MemberManagement_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub gvMember_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMember.PageIndexChanging
        gvMember.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub
End Class