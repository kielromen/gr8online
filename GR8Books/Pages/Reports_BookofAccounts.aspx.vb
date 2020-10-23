Public Class Reports_BookofAccounts
    Inherits System.Web.UI.Page
    Dim Type As String = "Book of Accounts"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
            End If
        End If
    End Sub

    Public Sub Initialize()
        Session("@DateFrom") = ""
        Session("@DateTo") = ""
        Session("@ReportType") = ""
        Session("@Book") = ""
        Session("@Header") = ""
        Session("@FileType") = ""

        ddlReportType.Items.Clear()
        ddlReportType.Items.Add("Summary")
        ddlReportType.Items.Add("Detailed")

        ddlReports.Items.Clear()
        ddlReports.Items.Add("--Select Report--")
        ddlReports.DataSource = LoadtblDefault_Reports(Type).ToArray
        ddlReports.DataBind()

        ddlMonth.Items.Clear()
        ddlMonth.Items.Add("January")
        ddlMonth.Items.Add("February")
        ddlMonth.Items.Add("March")
        ddlMonth.Items.Add("April")
        ddlMonth.Items.Add("May")
        ddlMonth.Items.Add("June")
        ddlMonth.Items.Add("July")
        ddlMonth.Items.Add("August")
        ddlMonth.Items.Add("September")
        ddlMonth.Items.Add("October")
        ddlMonth.Items.Add("November")
        ddlMonth.Items.Add("December")
        ddlPeriodType.Items.Clear()
        ddlPeriodType.Items.Add("Yearly")
        ddlPeriodType.Items.Add("Monthly")
        ddlPeriodType.Items.Add("Date Range")
        ddlPeriodType.Items.Add("Daily")
        ddlMonth.SelectedIndex = Now.Month - 1
        ddlPeriodType.SelectedIndex = 1
        txtYear.Text = Now.Year

        panelFilter.Visible = False
        LoadPeriod()
    End Sub


    Public Sub LoadPeriod()
        Dim period As String = (ddlMonth.SelectedIndex + 1).ToString & "-1-" & txtYear.Text.ToString
        If ddlPeriodType.SelectedValue = "Monthly" Then
            dtpFromDate.Text = CDate(Date.Parse(period)).ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Remove("readonly")
            ddlMonth.Attributes.Remove("disabled")
            txtYear.Attributes.Remove("disabled")
        ElseIf ddlPeriodType.SelectedValue = "Yearly" Then
            dtpFromDate.Text = CDate(Date.Parse("1-1-" & txtYear.Text.ToString)).ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(Date.Parse("12-31-" & txtYear.Text.ToString)).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Add("readonly", "true")
            dtpToDate.Attributes.Add("readonly", "true")
            ddlMonth.Attributes("disabled") = "disabled"
            txtYear.Attributes.Remove("disabled")
        ElseIf ddlPeriodType.SelectedValue = "Date Range" Then
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Remove("readonly")
            ddlMonth.Attributes("disabled") = "disabled"
            txtYear.Attributes("disabled") = "disabled"
        ElseIf ddlPeriodType.SelectedValue = "Daily" Then
            dtpToDate.Text = CDate(dtpFromDate.Text).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Add("readonly", "true")
            ddlMonth.Attributes("disabled") = "disabled"
            txtYear.Attributes("disabled") = "disabled"
        ElseIf ddlPeriodType.SelectedValue = "As Of" Then
            dtpFromDate.Text = CDate(Date.Parse(Now.Date)).ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(dtpFromDate.Text).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Add("readonly", "true")
            ddlMonth.Attributes("disabled") = "disabled"
            txtYear.Attributes("disabled") = "disabled"
        End If
    End Sub

    Public Sub LoadModules()
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  TransType AS ID, Description AS Filter " &
                " FROM    tblTransactionSetup WHERE Type = @Type "
        SQL.FlushParams()
        SQL.AddParam("@Type", Session("@Book"))
        SQL.GetQuery(query)
        gvFilter.DataSource = SQL.SQLDS
        gvFilter.DataBind()
        panelFilter.Visible = True
    End Sub

    Public Sub SelectReport()
        Dim ds As New DataTable()
        ds = Nothing
        gvFilter.DataSource = ds
        gvFilter.DataBind()
        panelFilter.Visible = False
        ddlReportType.Attributes("disabled") = "disabled"
        If ddlReports.SelectedValue = "Cash Receipts Book" Then
            Session("@Book") = "Cash Receipt"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Book" Then
            Session("@Book") = "Cash Disbursement"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Purchase Book" Then
            Session("@Book") = "Purchase Book"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Sales Book" Then
            Session("@Book") = "Sales Book"
            LoadModules()
        End If
    End Sub

    Private Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click, btnExport.Click
        Page.Validate()
        Dim BT As Button = CType(sender, Button)
        Select Case BT.Text
            Case "Export to Excel"
                Session("@FileType") = "Excel"
            Case Else
                Session("@FileType") = ""
        End Select

        Session("@DateFrom") = CDate(dtpFromDate.Text)
        Session("@DateTo") = IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text))
        Session("@ReportType") = ddlReportType.SelectedValue
        Session("@Header") = ddlReports.SelectedValue

        Dim query As String
        query = " DELETE FROM tblPrint_BOA  "
        SQL.ExecNonQuery(query)
        Dim RefType As String = ""
        For Each row As GridViewRow In gvFilter.Rows
            If TryCast(row.FindControl("chkBox"), CheckBox).Checked Then
                RefType = row.Cells(1).Text
                query = " INSERT INTO tblPrint_BOA(RefType) " &
                        " VALUES (@RefType) "
                SQL.FlushParams()
                SQL.AddParam("@RefType", RefType)
                SQL.ExecNonQuery(query)
            End If
        Next
        If RefType = "" Then
            Response.Write("<script>alert('Please Select Filter !');</script>")
            Exit Sub
        End If

        If ddlReports.SelectedValue = "Cash Receipts Book" Then
            Session("@Book") = "Cash Receipt"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'CRBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Book" Then
            Session("@Book") = "Cash Disbursement Book"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'CDBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Purchase Book" Then
            Session("@Book") = "Purchase Book"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'PBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Sales Book - Looseleaf" Then
            Session("@Book") = "Sales Book"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'SBLL', '_blank');</script>")
        End If
    End Sub


    Private Sub gvFilter_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFilter.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)")
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)")
        End If
    End Sub


End Class