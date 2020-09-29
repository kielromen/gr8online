Public Class CustomerManagement_View
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
        query = " SELECT * FROM tblCustomer_Master WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvCustomer.DataSource = SQL.SQLDS
        gvCustomer.DataBind()
    End Sub

    Private Sub gvCustomer_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCustomer.PageIndexChanging
        gvCustomer.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvCustomer_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCustomer.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCustomer_Master SET Status = @Status WHERE Customer_Code = @Customer_Code"
            SQL.FlushParams()
            SQL.AddParam("@Customer_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='CustomerManagement_View.aspx';</script>")
        End If
    End Sub
End Class