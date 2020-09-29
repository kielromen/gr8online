Public Class VendorManagement_View
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
        query = "SELECT * FROM tblVendor_Master WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvVendor.DataSource = SQL.SQLDS
        gvVendor.DataBind()
    End Sub


    Private Sub gvVendor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVendor.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblVendor_Master SET Status = @Status WHERE Vendor_Code = @Vendor_Code"
            SQL.FlushParams()
            SQL.AddParam("@Vendor_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='VendorManagement_View.aspx';</script>")
        End If
    End Sub
End Class