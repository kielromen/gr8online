Public Class DisbursementType_Loadlist
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
        query = " SELECT * FROM tblDisbursement_Type " &
                " INNER JOIN " &
                " tblCOA ON tblDisbursement_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE tblDisbursement_Type.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvDisbursementType.DataSource = SQL.SQLDS
        gvDisbursementType.DataBind()
    End Sub

    Private Sub gvDisbursementType_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvDisbursementType.PageIndexChanging
        gvDisbursementType.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvDisbursementType_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDisbursementType.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblDisbursement_Type SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='DisbursementType_Loadlist.aspx';</script>")
        End If
    End Sub
End Class