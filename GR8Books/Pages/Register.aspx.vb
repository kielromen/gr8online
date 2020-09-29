Imports System
Imports System.Web.UI.WebControls
Imports System.Net.Mail

Public Class Register
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = True Then
                Response.Redirect("Dashboard.aspx")
            End If
        End If
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click

    End Sub
End Class