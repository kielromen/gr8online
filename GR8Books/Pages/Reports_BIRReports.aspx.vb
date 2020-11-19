Public Class Reports_BIRReports
    Inherits System.Web.UI.Page
    Dim Type As String = "BIR Reports"
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
        ddlPeriodType.Items.Add("Quarterly")
        ddlPeriodType.Items.Add("Monthly")
        ddlPeriodType.Items.Add("Date Range")
        ddlPeriodType.Items.Add("Daily")
        ddlMonth.SelectedIndex = Now.Month - 1
        ddlPeriodType.SelectedIndex = 2
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
        ElseIf ddlPeriodType.SelectedValue = "Quarterly" Then
            If ddlMonth.SelectedValue = "1st Quarter" Then
                dtpFromDate.Text = CDate("01-01-" & txtYear.Text).ToString("yyyy-MM-dd")
                dtpToDate.Text = CDate("03-31-" & txtYear.Text).ToString("yyyy-MM-dd")
            ElseIf ddlMonth.SelectedValue = "2nd Quarter" Then
                dtpFromDate.Text = CDate("04-01-" & txtYear.Text).ToString("yyyy-MM-dd")
                dtpToDate.Text = CDate("06-30-" & txtYear.Text).ToString("yyyy-MM-dd")
            ElseIf ddlMonth.SelectedValue = "3rd Quarter" Then
                dtpFromDate.Text = CDate("07-01-" & txtYear.Text).ToString("yyyy-MM-dd")
                dtpToDate.Text = CDate("09-30-" & txtYear.Text).ToString("yyyy-MM-dd")
            ElseIf ddlMonth.SelectedValue = "4th Quarter" Then
                dtpFromDate.Text = CDate("10-01-" & txtYear.Text).ToString("yyyy-MM-dd")
                dtpToDate.Text = CDate("12-31-" & txtYear.Text).ToString("yyyy-MM-dd")
            End If
            dtpFromDate.Attributes.Add("readonly", "true")
            dtpToDate.Attributes.Add("readonly", "true")
        End If
    End Sub

    Public Sub SelectReport()
        Dim ds As New DataTable()
        ds = Nothing
        gvFilter.DataSource = ds
        gvFilter.DataBind()
        panelFilter.Visible = False

        ddlReportType.Attributes.Remove("disabled")
        ddlPeriodType.Attributes.Remove("disabled")
        ddlMonth.Attributes.Remove("disabled")
        txtYear.Attributes.Remove("disabled")
        dtpFromDate.Attributes.Remove("disabled")
        dtpToDate.Attributes.Remove("disabled")

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
        ddlPeriodType.SelectedIndex = 1
        ddlMonth.SelectedIndex = Now.Month - 1

        If ddlReports.SelectedValue = "Summary Lists of Sales" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Items.Clear()
            ddlPeriodType.Items.Add("Quarterly")

            ddlMonth.Items.Clear()
            ddlMonth.Items.Add("1st Quarter")
            ddlMonth.Items.Add("2nd Quarter")
            ddlMonth.Items.Add("3rd Quarter")
            ddlMonth.Items.Add("4th Quarter")
        ElseIf ddlReports.SelectedValue = "Summary Lists of Purchases" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Items.Clear()
            ddlPeriodType.Items.Add("Quarterly")

            ddlMonth.Items.Clear()
            ddlMonth.Items.Add("1st Quarter")
            ddlMonth.Items.Add("2nd Quarter")
            ddlMonth.Items.Add("3rd Quarter")
            ddlMonth.Items.Add("4th Quarter")
        ElseIf ddlReports.SelectedValue = "Summary Alphalist of Withholding Tax" Then

        End If
        LoadPeriod()
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

        If ddlReports.SelectedValue = "Summary Lists of Sales" Then
            Session("@DateTo") = ddlMonth.SelectedValue
            Response.Write("<script>window.open('Reports.aspx?id=' + 'SLS', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Summary Lists of Purchases" Then
            Session("@DateTo") = ddlMonth.SelectedValue
            Response.Write("<script>window.open('Reports.aspx?id=' + 'SLP', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Summary Alphalist of Withholding Tax" Then
            Response.Write("<script>window.open('Reports.aspx?id=' + 'SAWT', '_blank');</script>")
        End If
    End Sub

    Private Sub gvFilter_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFilter.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)")
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)")
        End If
    End Sub

End Class