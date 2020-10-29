Public Class Ledger
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Session("VCECode") = ""
                Session("AccntCode") = ""
            End If
        End If
    End Sub

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(dgvList, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim VCECode As String = HttpUtility.HtmlDecode(dgvList.SelectedRow.Cells(0).Text)
        Dim AccntCode As String = HttpUtility.HtmlDecode(dgvList.SelectedRow.Cells(2).Text)

        Dim url As String = Request.QueryString("Url")
        txtFilter.Text = ""
        Session("VCECode") = VCECode
        Session("AccntCode") = AccntCode
        Response.Write("<script>window.open('LedgerDetails.aspx', '_blank');</script>")
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
        txtFilter.Focus()
    End Sub

    Public Sub LoadList()
        Dim query As String = ""
        Dim filter As String = txtFilter.Text
        query = " SELECT VCECode, VCEName, AccntCode, AccntTitle, CONVERT(VARCHAR,CONVERT(MONEY,Debit),1) AS Debit, CONVERT(VARCHAR,CONVERT(MONEY,Credit),1) AS Credit" & vbCrLf &
                " FROM  dbo.View_SL " & vbCrLf &
                " WHERE VCECode <> '' AND (VCECode LIKE '%" & filter & "%' OR VCEName LIKE '%" & filter & "%') ORDER BY VCEName, AccntCode DESC"
        SQL.FlushParams()
        SQL.GetQuery(query)
        dgvList.DataSource = SQL.SQLDS
        dgvList.DataBind()
    End Sub
End Class