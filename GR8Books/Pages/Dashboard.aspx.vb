Public Class Dashboard1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                LoadUserCompany(Session("Company_Code"))
                alertWelcome.Visible = True
            End If
        End If
    End Sub

    Public Sub LoadUserCompany(ByVal CompanyCode As Integer)
        Dim query As String
        query = " SELECT * " &
                " FROM [Main].dbo.tblCompany_Information" &
                " WHERE Status = @Status AND Company_Code = @Company_Code"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Company_Code", CompanyCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Session("Company_Name") = SQL.SQLDR("Company_Name").ToString
            Session("TIN_No") = SQL.SQLDR("TIN_No").ToString
        End If
        lblUserCompany.Text = Session("Company_Name")
    End Sub


End Class