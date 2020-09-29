Public Class BankMasterfile_LoadList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                LoadBankList()
            End If
        End If
    End Sub

    Public Sub LoadBankList()
        Dim query As String
        query = " SELECT ID, Type,  Bank, Branch, tblBank.AccountCode, ISNULL(AccountTitle,'') as AccountTitle, AccountNumber, SeriesStart," &
                              " SeriesEnd, SeriesDigits FROM tblBank" &
                              " LEFT JOIN" &
                              " tblCOA ON" &
                              " tblCOA.AccountCode = tblBank.AccountCode " &
                              " WHERE tblBank.Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        dgvBankList.DataSource = SQL.SQLDS
        dgvBankList.DataBind()
    End Sub

    Private Sub dgvBankList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvBankList.PageIndexChanging
        dgvBankList.PageIndex = e.NewPageIndex
        LoadBankList()
    End Sub

    Private Sub dgvBankList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvBankList.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblBank SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='BankMasterfile_Loadlist.aspx';</script>")
        End If
    End Sub
End Class