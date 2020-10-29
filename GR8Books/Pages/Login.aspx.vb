Public Class Login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = True Then
                If Session("UserRole") = "SystemAdmin" Then
                    Response.Redirect("Dashboard.aspx")
                Else
                    Response.Redirect("Login.aspx")
                End If
            End If
        End If
    End Sub

    Protected Sub lnkRegister_Click(sender As Object, e As EventArgs) Handles lnkRegister.Click
        Response.Redirect("Register.aspx")
    End Sub

    Private Sub lnkForgot_Click(sender As Object, e As EventArgs) Handles lnkForgot.Click
        Response.Redirect("ForgotPassword.aspx")
    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Page.Validate()
        If (Page.IsValid) Then
            Dim query As String
            query = " SELECT  EmailAddress, Company_Code, Password, UserRole, MobileNumber, FirstName, MiddleName, LastName, Status, DateLastLogin" & vbCrLf &
                              " FROM    [Main].dbo.tblCompany_User  " & vbCrLf &
                              " WHERE   EmailAddress COLLATE Latin1_General_CS_AS = @EmailAddress " & vbCrLf &
                              "         AND Password COLLATE Latin1_General_CS_AS = @Password " & vbCrLf &
                              "         AND Status = @Status"
            SQL.AddParam("@EmailAddress", txtEMail.Value)
            SQL.AddParam("@Password", txtPassword.Value)
            SQL.AddParam("@Status", "Active")
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                Session("SessionExists") = True
                Session("EmailAddress") = SQL.SQLDR("EmailAddress").ToString
                Session("Company_Code") = SQL.SQLDR("Company_Code").ToString
                Session("UserRole") = SQL.SQLDR("UserRole").ToString
                Session("MobileNumber") = SQL.SQLDR("MobileNumber").ToString
                Session("FirstName") = SQL.SQLDR("FirstName").ToString
                Session("MiddleName") = SQL.SQLDR("MiddleName").ToString
                Session("LastName") = SQL.SQLDR("LastName").ToString

                Dim updateQuery As String
                updateQuery = " UPDATE [Main].dbo.tblCompany_User " & vbCrLf &
                              " SET DateLastLogin = GETDATE() WHERE EmailAddress = '" & Session("EmailAddress") & "' "
                SQL.ExecNonQuery(updateQuery)

                If Session("UserRole") = "SystemAdmin" Then
                    Response.Write("<script>window.location='Dashboard.aspx';</script>")
                Else
                    Response.Write("<script>window.location='Default.aspx';</script>")
                End If
            Else
                Response.Write("<script>alert('Username and password did not match');</script>")
                txtEMail.Value = ""
                txtEMail.Focus()
            End If
        End If
    End Sub
End Class