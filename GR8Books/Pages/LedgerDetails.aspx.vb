Public Class LedgerDetails
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                LoadList()
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtCode.Attributes.Add("readonly", "readonly")
        txtName.Attributes.Add("readonly", "readonly")
        txtAccount.Attributes.Add("readonly", "readonly")
        txtBalance.Attributes.Add("readonly", "readonly")
        txtCode.Text = ""
        txtName.Text = ""
        txtAccount.Text = ""
        txtBalance.Text = ""
    End Sub
    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(dgvList, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim RefType As String = dgvList.SelectedRow.Cells(2).Text
        Dim RefTransID As String = dgvList.SelectedRow.Cells(1).Text
        Session("ID") = ""
        Select Case RefType
            Case "JV"
                Dim url As String = "JournalVoucher.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "PJ"
                Dim url As String = "PurchaseJournal.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "SJ"
                Dim url As String = "SalesJournal.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "OR"
                Dim url As String = "OfficialReceipt.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "AR"
                Dim url As String = "AcknowledgementReceipt.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "PR"
                Dim url As String = "ProvisionalReceipt.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "CR"
                Dim url As String = "CollectionReceipt.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "CV"
                Dim url As String = "CheckVoucher.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
            Case "APV"
                Dim url As String = "AccountsPayableVoucher.aspx"
                Session("ID") = RefTransID
                Response.Write("<script>window.open('" & url & "', '_blank');</script>")
        End Select
    End Sub


    Public Sub LoadList()
        txtCode.Text = Session("VCECode")
        txtName.Text = GetVCEName(Session("VCECode"))
        txtAccount.Text = GetAccountTitle(Session("AccntCode"))
        txtBalance.Text = GetSLBalance(Session("VCECode"), Session("AccntCode"))
        Dim query As String = ""
        query = " SELECT  No,  REPLACE(CAST(AppDate as DATE),' 12:00:00 AM','') as AppDate, RefType, RefTransID,  TransNo, VCECode, VCEName, AccntCode, AccntTitle, " & vbCrLf &
                " CONVERT(VARCHAR,CONVERT(MONEY,Debit),1) AS Debit, CONVERT(VARCHAR,CONVERT(MONEY,Credit),1) AS Credit,CONVERT(VARCHAR,CONVERT(MONEY,Balance),1)  AS  Balance, RefNo, WhoCreated, LoginName" & vbCrLf &
                " FROM  dbo.View_Ledger " & vbCrLf &
                " WHERE VCECode = @VCECode AND AccntCode = @AccntCode"
        SQL.FlushParams()
        SQL.AddParam("@VCECode", Session("VCECode"))
        SQL.AddParam("@AccntCode", Session("AccntCode"))
        SQL.GetQuery(query)
        dgvList.DataSource = SQL.SQLDS
        dgvList.DataBind()
    End Sub
End Class