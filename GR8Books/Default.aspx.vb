Imports System.Net
Imports System.Net.Mail
Imports System.Net.Configuration

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadCompany()
        End If
    End Sub

End Class