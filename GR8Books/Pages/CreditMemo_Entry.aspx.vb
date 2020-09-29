Public Class CreditMemo_Entry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                CreditMemo_Entry()
            End If
        End If
    End Sub
    Public Sub CreditMemo_Entry()
        Dim query As String
        query = "SELECT TransID, CM_Type, CreditAccount, AccountTitle FROM tblCM" &
                " LEFT JOIN" &
                " tblCOA ON" &
                " tblCOA.AccountCode = tblCM.CreditAccount " &
                " WHERE tblCM.Status = @Status"
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)

        gvVendor.DataSource = SQL.SQLDS
        gvVendor.DataBind()
    End Sub

    Private Sub gvVendor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVendor.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCM SET Status = @Status WHERE Code = @Code"
            SQL.FlushParams()
            SQL.AddParam("@Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='CreditMemo_Entry.aspx';</script>")
        End If
    End Sub

    Private Sub gvVendor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvVendor.PageIndexChanging
        gvVendor.PageIndex = e.NewPageIndex
        CreditMemo_Entry()
    End Sub
End Class