Public Class DebitMemo_Entry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                DebitMemo_Entry()
            End If
        End If
    End Sub
    Public Sub DebitMemo_Entry()
        Dim query As String
        query = "SELECT TransID, DM_Type, DebitAccount, AccountTitle FROM tblDM" &
                " LEFT JOIN" &
                " tblCOA ON" &
                " tblCOA.AccountCode = tblDM.DebitAccount " &
                " WHERE tblDM.Status = @Status"
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)

        gvVendor.DataSource = SQL.SQLDS
        gvVendor.DataBind()
    End Sub

    Private Sub gvVendor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVendor.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblDM SET Status = @Status WHERE Code = @Code"
            SQL.FlushParams()
            SQL.AddParam("@Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='DebitMemo_Entry.aspx';</script>")
        End If
    End Sub

    Private Sub gvVendor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvVendor.PageIndexChanging
        gvVendor.PageIndex = e.NewPageIndex
        DebitMemo_Entry()
    End Sub
End Class