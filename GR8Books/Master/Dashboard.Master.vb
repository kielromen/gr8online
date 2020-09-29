Public Class Dashboard
    Inherits System.Web.UI.MasterPage

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        Session.Abandon()
        Response.Redirect("Login.aspx")
    End Sub


End Class