Public Class Branchcode_Entry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Branchcode_Entry()
            End If
        End If
    End Sub
    Public Sub Branchcode_Entry()
        Dim query As String
        query = "SELECT Branchcode, Description, Status FROM tblBranch"
        '" LEFT JOIN" &
        '" tblCOA ON" &
        '" tblCOA.AccountCode = tblDM.DebitAccount " &
        '" WHERE tblBranch.Status = @Status"
        SQL.AddParam("@branchcode", ID)
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)

        gvVendor.DataSource = SQL.SQLDS
        gvVendor.DataBind()
    End Sub
    Private Sub gvVendor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVendor.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblBranch SET Status = @Status WHERE Code = @Branchcode"
            SQL.FlushParams()
            SQL.AddParam("@Branchcode", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='Branchcode_Entry.aspx';</script>")
        End If
    End Sub
    Private Sub gvVendor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvVendor.PageIndexChanging
        gvVendor.PageIndex = e.NewPageIndex
        Branchcode_Entry()
    End Sub
End Class