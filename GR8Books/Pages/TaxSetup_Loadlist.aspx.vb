Public Class TaxSetup_Loadlist
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
        query = "  SELECT ID, TAX_Type, CONCAT(Percentage,' %') AS Percentage, Normal, tblTax_Setup.Name, Form, Basis, tblTax_Setup.AccountCode, tblCOA.AccountTitle FROM tblTax_Setup " &
                "  LEFT JOIN tblCOA ON tblTax_Setup.AccountCode = tblCOA.AccountCode " &
                "  WHERE tblTax_Setup.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvTaxSetUp.DataSource = SQL.SQLDS
        gvTaxSetUp.DataBind()
    End Sub

    Private Sub gvTaxSetUp_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvTaxSetUp.PageIndexChanging
        gvTaxSetUp.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvTaxSetUp_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTaxSetUp.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblTax_Setup SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='TaxSetup_Loadlist.aspx';</script>")
        End If
    End Sub
End Class