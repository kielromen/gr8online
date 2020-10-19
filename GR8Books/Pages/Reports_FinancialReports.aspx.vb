Public Class Reports_FinancialReports
    Inherits System.Web.UI.Page
    Dim Type As String = "Financial Reports"
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
        ddlPeriodType.SelectedIndex = 1

        ddlMonth.SelectedIndex = Now.Month - 1
        txtYear.Text = Now.Year

        txtSearch.Text = ""
        panelFilter.Visible = False
        LoadPeriod()
    End Sub

    Public Sub LoadPeriod()
        Dim period As String = (ddlMonth.SelectedIndex + 1).ToString & "-1-" & txtYear.Text.ToString
        If ddlPeriodType.SelectedValue = "Monthly" Then
            dtpFromDate.Text = CDate(Date.Parse(period)).ToString("yyyy-MM-dd")
            dtpToDate.text = CDate(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Remove("readonly")
            ddlMonth.Attributes.Remove("disabled")
            txtYear.Attributes.Remove("disabled")
        ElseIf ddlPeriodType.SelectedValue = "Yearly" Then
            dtpFromDate.Text = CDate(Date.Parse("1-1-" & txtYear.Text.ToString)).ToString("yyyy-MM-dd")
            dtpToDate.text = CDate(Date.Parse("12-31-" & txtYear.Text.ToString)).ToString("yyyy-MM-dd")
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
            dtpToDate.text = CDate(dtpFromDate.Text).ToString("yyyy-MM-dd")
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

    Public Sub LoadAccounts()
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  AccountCode AS ID, AccountTitle AS Filter " &
                " FROM    tblCOA " &
                " WHERE   Status = @Status AND AccountTitle LIKE '%' + @AccountTitle + '%' AND " &
                " Class = @Class ORDER BY AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", txtSearch.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Class", "Posting")
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

        ddlReportType.Attributes.Remove("disabled")
        ddlPeriodType.Attributes.Remove("disabled")
        ddlMonth.Attributes.Remove("disabled")
        txtYear.Attributes.Remove("disabled")
        dtpFromDate.Attributes.Remove("disabled")
        dtpToDate.Attributes.Remove("disabled")

        ddlPeriodType.Items.Clear()
        ddlPeriodType.Items.Add("Yearly")
        ddlPeriodType.Items.Add("Monthly")
        ddlPeriodType.Items.Add("Date Range")
        ddlPeriodType.Items.Add("Daily")
        ddlPeriodType.SelectedIndex = 1

        If ddlReports.SelectedValue = "Preliminary Trial Balance (TB generated before closing entries)" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Items.Clear()
            ddlPeriodType.Items.Add("As Of")
        ElseIf ddlReports.SelectedValue = "Post Closing Trial Balance (TB generated after PEC)" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Items.Clear()
            ddlPeriodType.Items.Add("As Of")
        ElseIf ddlReports.SelectedValue = "Trial Balance" Then

        ElseIf ddlReports.SelectedValue = "General Ledger" Then
            LoadAccounts()
        ElseIf ddlReports.SelectedValue = "Statement of Financial Position" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Attributes("disabled") = "disabled"
            ddlMonth.Attributes("disabled") = "disabled"
        ElseIf ddlReports.SelectedValue = "Statement of Operation" Then
            ddlReportType.Attributes("disabled") = "disabled"
            ddlPeriodType.Attributes("disabled") = "disabled"
            ddlMonth.Attributes("disabled") = "disabled"
        ElseIf ddlReports.SelectedValue = "Statement of Changes In Equity" Then
            ddlReportType.Attributes("disabled") = "disabled"
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
        Session("@DateTo") = IIf(ddlPeriodType.SelectedValue = "Daily" Or ddlPeriodType.SelectedValue = "As Of", CDate(dtpFromDate.Text), CDate(dtpToDate.Text))
        Session("@ReportType") = ddlReportType.SelectedValue
        If ddlReports.SelectedValue = "Preliminary Trial Balance (TB generated before closing entries)" Then
            GenerateTB(ddlReportType.SelectedValue, CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily" Or ddlPeriodType.SelectedValue = "As Of", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "Post Closing Trial Balance (TB generated after PEC)" Then
            GenerateTB(ddlReportType.SelectedValue, CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily" Or ddlPeriodType.SelectedValue = "As Of", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "Trial Balance" Then
            GenerateTB(ddlReportType.SelectedValue, CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "General Ledger" Then
            GenerateGL(ddlReportType.SelectedValue)
        ElseIf ddlReports.SelectedValue = "Statement of Financial Position" Then
            GenerateBS(CDate(dtpFromDate.Text), CDate(dtpToDate.Text))
        ElseIf ddlReports.SelectedValue = "Statement of Operation" Then
            GenerateIS(CDate(dtpFromDate.Text), CDate(dtpToDate.Text))
        ElseIf ddlReports.SelectedValue = "Statement of Changes In Equity" Then
            GenerateCE(CDate(dtpFromDate.Text), CDate(dtpToDate.Text))
        End If
    End Sub

    Private Sub gvFilter_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFilter.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)")
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)")
        End If
    End Sub

    Private Sub GenerateTB(ByVal Type As String, ByVal DateFrom As Date, ByVal DateTo As Date, Optional ByVal Filter As String = "")
        Dim insertSQL, deleteSQL As String
        deleteSQL = " DELETE FROM tblPRint_TB "
        SQL.ExecNonQuery(deleteSQL)
        If Type = "Detailed" And ddlReports.SelectedValue = "Trial Balance" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, CDDR, CDCR, SBDR, SBCR, PBDR, PBCR, JVDR, JVCR, TBDR, TBCR) " &
                    " SELECT  AccountCode, AccountTitle,  " &
                    " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " &
                    " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " &
                    " 		  SUM(CRDR) AS CRDR, " &
                    " 		  SUM(CRCR) AS CRCR, " &
                    " 		  SUM(CDDR) AS CDDR, " &
                    " 		  SUM(CDCR) AS CDCR, " &
                    " 		  SUM(SBDR) AS SBDR, " &
                    " 		  SUM(SBCR) AS SBCR, " &
                    " 		  SUM(PBDR) AS PBDR, " &
                    " 		  SUM(PBCR) AS PBCR, " &
                    " 		  SUM(JVDR) AS JVDR, " &
                    " 		  SUM(JVCR) AS JVCR, " &
                    " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " &
                    " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " &
                    " FROM " &
                    " ( " &
                    " 	SELECT tblCOA.AccountCode, tblCOA.AccountTitle,  " &
                    " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " &
                    " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Debit ELSE 0 END AS CRDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Credit ELSE 0 END AS CRCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Debit ELSE 0 END AS CDDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Credit ELSE 0 END AS CDCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Debit ELSE 0 END AS SBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Credit ELSE 0 END AS SBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Debit ELSE 0 END AS PBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Credit ELSE 0 END AS PBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " &
                    " 		   Debit AS TBDR, " &
                    " 		   Credit AS TBCR, View_GL.Status   " &
                    " 	FROM View_GL INNER JOIN tblCOA " &
                    " 	ON View_GL.AccntCode = tblCOA.AccountCode " &
                    " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "' AND '" & DateTo & "' " &
                    " ) AS A " &
                    " WHERE A.Status ='Saved'  " &
                    " GROUP BY AccountCode, AccountTitle "
            SQL.FlushParams()
            SQL.ExecNonQuery(insertSQL)
            Response.Write("<script>window.open('Reports.aspx?id=' + 'TBDETAILED', '_blank');</script>")
        ElseIf Type = "Summary" And ddlReports.SelectedValue = "Trial Balance" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, TBDR, TBCR) " &
                     " SELECT  AccountCode, AccountTitle,  " &
                     " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " &
                     " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " &
                     " 		  CASE WHEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) > SUM(CRCR + CDCR + JVCR + PBCR + SBCR) THEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) - SUM(CRCR + CDCR + JVCR + PBCR + SBCR) ELSE 0 END AS CRDR, " &
                     " 		  CASE WHEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) > SUM(CRDR + CDDR + JVDR + PBDR + SBDR) THEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) - SUM(CRDR + CDDR + JVDR + PBDR + SBDR) ELSE 0 END AS CRCR, " &
                     " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " &
                     " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " &
                     " FROM " &
                     " ( " &
                     " 	SELECT tblCOA.AccountCode, tblCOA.AccountTitle,  " &
                    " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " &
                    " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Debit ELSE 0 END AS CRDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Credit ELSE 0 END AS CRCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Debit ELSE 0 END AS CDDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Credit ELSE 0 END AS CDCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Debit ELSE 0 END AS SBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Credit ELSE 0 END AS SBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Debit ELSE 0 END AS PBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Credit ELSE 0 END AS PBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " &
                    " 		   Debit AS TBDR, " &
                    " 		   Credit AS TBCR, View_GL.Status " &
                    " 	FROM View_GL JOIN tblCOA " &
                    " 	ON View_GL.AccntCode = tblCOA.AccountCode  " &
                    " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "'And '" & DateTo & "'" &
                    " ) AS A " &
                    " WHERE A.Status ='Saved'  " &
                    " GROUP BY AccountCode, AccountTitle "
            SQL.ExecNonQuery(insertSQL)
            Response.Write("<script>window.open('Reports.aspx?id=' + 'TBSUMMARY', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Preliminary Trial Balance (TB generated before closing entries)" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, TBDR, TBCR) " &
                    " SELECT  AccountCode, AccountTitle,  " &
                    " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " &
                    " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " &
                    " 		  CASE WHEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) > SUM(CRCR + CDCR + JVCR + PBCR + SBCR) THEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) - SUM(CRCR + CDCR + JVCR + PBCR + SBCR) ELSE 0 END AS CRDR, " &
                    " 		  CASE WHEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) > SUM(CRDR + CDDR + JVDR + PBDR + SBDR) THEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) - SUM(CRDR + CDDR + JVDR + PBDR + SBDR) ELSE 0 END AS CRCR, " &
                    " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " &
                    " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " &
                    " FROM " &
                    " ( " &
                    " 	SELECT tblCOA.AccountCode, tblCOA.AccountTitle,  " &
                   " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " &
                   " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Debit ELSE 0 END AS CRDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Credit ELSE 0 END AS CRCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Debit ELSE 0 END AS CDDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Credit ELSE 0 END AS CDCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Debit ELSE 0 END AS SBDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Credit ELSE 0 END AS SBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Debit ELSE 0 END AS PBDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Credit ELSE 0 END AS PBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " &
                   " 		   Debit AS TBDR, " &
                   " 		   Credit AS TBCR, View_GL.Status " &
                   " 	FROM View_GL JOIN tblCOA " &
                   " 	ON View_GL.AccntCode = tblCOA.AccountCode  " &
                   " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "'And '" & DateTo & "'  AND RefType <> 'PEC' " &
                   " ) AS A " &
                   " WHERE A.Status ='Saved'  " &
                   " GROUP BY AccountCode, AccountTitle "
            SQL.ExecNonQuery(insertSQL)
            Response.Write("<script>window.open('Reports.aspx?id=' + 'TBBEFOREPEC', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Post Closing Trial Balance (TB generated after PEC)" Then
            insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, TBDR, TBCR) " &
                    " SELECT  AccountCode, AccountTitle,  " &
                    " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " &
                    " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " &
                    " 		  CASE WHEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) > SUM(CRCR + CDCR + JVCR + PBCR + SBCR) THEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) - SUM(CRCR + CDCR + JVCR + PBCR + SBCR) ELSE 0 END AS CRDR, " &
                    " 		  CASE WHEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) > SUM(CRDR + CDDR + JVDR + PBDR + SBDR) THEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) - SUM(CRDR + CDDR + JVDR + PBDR + SBDR) ELSE 0 END AS CRCR, " &
                    " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " &
                    " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " &
                    " FROM " &
                    " ( " &
                    " 	SELECT tblCOA.AccountCode, tblCOA.AccountTitle,  " &
                   " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " &
                   " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Debit ELSE 0 END AS CRDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Credit ELSE 0 END AS CRCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Debit ELSE 0 END AS CDDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Credit ELSE 0 END AS CDCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Debit ELSE 0 END AS SBDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Credit ELSE 0 END AS SBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Debit ELSE 0 END AS PBDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Credit ELSE 0 END AS PBCR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " &
                   " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " &
                   " 		   Debit AS TBDR, " &
                   " 		   Credit AS TBCR, View_GL.Status " &
                   " 	FROM View_GL JOIN tblCOA " &
                   " 	ON View_GL.AccntCode = tblCOA.AccountCode  " &
                   " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "'And '" & DateTo & "'" &
                   " ) AS A " &
                   " WHERE A.Status ='Saved'  " &
                   " GROUP BY AccountCode, AccountTitle "
            SQL.ExecNonQuery(insertSQL)
            Response.Write("<script>window.open('Reports.aspx?id=' + 'TBAFTERPEC', '_blank');</script>")
        End If
    End Sub

    Private Sub GenerateGL(ByVal Type As String)
        Dim query As String
        query = " DELETE FROM tblPrint_TB  "
        SQL.ExecNonQuery(query)
        Dim AccountCode As String = ""
        For Each row As GridViewRow In gvFilter.Rows
            If TryCast(row.FindControl("chkBox"), CheckBox).Checked Then
                AccountCode = row.Cells(1).Text
                query = " INSERT INTO tblPrint_TB(Code) " &
                        " VALUES (@Code) "
                SQL.FlushParams()
                SQL.AddParam("@Code", AccountCode)
                SQL.ExecNonQuery(query)
            End If
        Next
        If AccountCode = "" Then
            Response.Write("<script>alert('Please Select Filter!');</script>")
            Exit Sub
        End If
        If Type = "Detailed" Then
            Response.Write("<script>window.open('Reports.aspx?id=' + 'GENLGRD', '_blank');</script>")
        ElseIf Type = "Summary" Then
            Response.Write("<script>window.open('Reports.aspx?id=' + 'GENLGRS', '_blank');</script>")
        End If
    End Sub

    Private Sub GenerateBS(ByVal DateFrom As Date, ByVal DateTo As Date)
        Dim deleteSQl As String
        deleteSQl = " DELETE FROM rptBS "
        SQL.ExecNonQuery(deleteSQl)
        Dim dt As New DataTable
        Dim query As String
        Dim desc As String
        Dim groupID As Integer = 0
        Dim value As Decimal = 0
        Dim totalDesc(7) As String
        Dim totalCR(7) As Decimal
        Dim insertSQl As String
        Dim prevID As Integer = 0
        Dim recID As Integer = 1
        Dim incre As Integer = 1

        query = " SELECT  CASE WHEN AccountGroup ='Sub Account' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 		END  AS Descrition," & vbCrLf &
                "  CASE	WHEN AccountGroup = 'Account Type' THEN 'G1' WHEN AccountGroup = 'Account Sub Type' THEN 'G2' " & vbCrLf &
                " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'Sub Category' THEN 'G4' " & vbCrLf &
                " 		WHEN AccountGroup = 'Cost Center' THEN 'G5'	WHEN AccountGroup = 'Main Account' THEN 'G6' " & vbCrLf &
                " 		WHEN AccountGroup = 'Sub Account' THEN 'G7' " & vbCrLf &
                "  ELSE ''	END AS AccountGroup, " & vbCrLf &
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf &
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf &
                " 			ELSE 0 " & vbCrLf &
                " 	   END AS Amount, showTotal, contraAccount, OrderNo " & vbCrLf &
                " FROM tblCOA " & vbCrLf &
                " LEFT JOIN " & vbCrLf &
                " ( " & vbCrLf &
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & vbCrLf &
                " 	FROM      view_GL  " & vbCrLf &
                " 	WHERE     Status <> 'Cancelled' AND AppDate BETWEEN CAST('01-01-" & DateFrom.Year & "' AS DATE) AND '" & DateTo.Date & "' " & vbCrLf &
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf &
                " ) AS TB " & vbCrLf &
                " ON  tblCOA.AccountCode = TB.AccntCode " & vbCrLf &
                " WHERE  AccountType ='Balance Sheet' AND tblCOA.Status = 'Active' " & vbCrLf &
                " GROUP BY AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount " & vbCrLf &
                " HAVING  (AccountGroup <> 'Sub Account' OR " & vbCrLf &
                "        (AccountGroup = 'Sub Account'  AND " & vbCrLf &
                "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf &
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf &
                " 			ELSE 0 " & vbCrLf &
                " 	   END) <> 0)) " & vbCrLf &
                " UNION ALL " & vbCrLf &
                " ( " & vbCrLf &
                "	  SELECT CASE WHEN AccountGroup ='Sub Account' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 				 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 			END  AS Descrition," & vbCrLf &
                "  CASE	WHEN AccountGroup = 'Account Type' THEN 'G1' WHEN AccountGroup = 'Account Sub Type' THEN 'G2' " & vbCrLf &
                " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'Sub Category' THEN 'G4' " & vbCrLf &
                " 		WHEN AccountGroup = 'Cost Center' THEN 'G5'	WHEN AccountGroup = 'Main Account' THEN 'G6' " & vbCrLf &
                " 		WHEN AccountGroup = 'Sub Account' THEN 'G7' " & vbCrLf &
                "  ELSE ''	END AS AccountGroup, " & vbCrLf &
                "        Amount," & vbCrLf &
                "        showTotal, contraAccount, OrderNo" & vbCrLf &
                "        FROM tblCOA" & vbCrLf &
                "        LEFT JOIN" & vbCrLf &
                "	  (" & vbCrLf &
                "		SELECT (SELECT AccountCode FROM tblCOA WHERE AccountTitle LIKE 'NET%' AND AccountGroup = 'Sub Account') AS AccntCode, ISNULL(SUM(Credit - Debit),0) AS Amount FROM" & vbCrLf &
                "		View_GL WHERE Status <> 'Cancelled' AND AppDate BETWEEN CAST('01-01-" & DateFrom.Year & "' AS DATE) AND '" & DateTo.Date & "' AND AccntCode IN (SELECT AccountCode FROM tblCOA WHERE AccountType = 'Income Statement')" & vbCrLf &
                "	  ) AS Bal ON AccountCode = AccntCode" & vbCrLf &
                "	  WHERE AccountTitle LIKE 'NET%' AND AccountGroup = 'Sub Account'" & vbCrLf &
                " )" & vbCrLf &
                " ORDER BY OrderNo "
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                desc = row(0).ToString
                groupID = CInt(row(1).ToString.Replace("G", ""))
                value = row(2)
                If row(3) = True Then
                    totalDesc(groupID) = desc
                End If
                If row(4) = True Then
                    value = value * -1
                End If
                If groupID <> prevID Or groupID = 7 Then

                    If prevID > groupID Then
                        If prevID <> 7 Then
                            For i As Integer = incre - 1 To 0 Step -1
                                deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                                SQL.ExecNonQuery(deleteSQl)
                                recID -= 1
                            Next
                            incre = 0
                        Else
                            incre = 0
                        End If
                        For i As Integer = 6 To 1 Step -1
                            If groupID <= i Then
                                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                    If totalCR(i) <> 0 Then
                                        insertSQl = " INSERT INTO " &
                                               " rptBS(RecordID, Description, Amount, GroupID) " &
                                               " VALUES(@RecordID, @Description, @Amount, @GroupID)"
                                        SQL.FlushParams()
                                        SQL.AddParam("@RecordID", recID)
                                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                        SQL.AddParam("@Amount", totalCR(i))
                                        SQL.AddParam("@GroupID", i)
                                        SQL.ExecNonQuery(insertSQl)
                                        incre = 0
                                        totalDesc(i) = Nothing
                                        totalCR(i) = Nothing
                                        recID += 1
                                    End If

                                End If
                            End If
                        Next
                    End If
                    insertSQl = " INSERT INTO " &
                        " rptBS(RecordID, Description, Amount, GroupID) " &
                        " VALUES(@RecordID, @Description, @Amount,  @GroupID)"
                    SQL.FlushParams()
                    SQL.AddParam("@RecordID", recID)
                    SQL.AddParam("@Description", desc)
                    SQL.AddParam("@Amount", value)
                    SQL.AddParam("@GroupID", groupID)
                    SQL.ExecNonQuery(insertSQl)
                    prevID = groupID
                    recID += 1
                    incre += 1
                    If value <> 0 Then
                        For i As Integer = 1 To 7
                            If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                totalCR(i) += value
                            End If
                        Next
                    End If

                End If
            Next
            If prevID <> 7 Then
                For i As Integer = incre - 1 To 0 Step -1
                    deleteSQl = " DELETE FROM rptBS WHERE RecordID = '" & recID - 1 & "' "
                    SQL.ExecNonQuery(deleteSQl)
                    recID -= 1
                Next
            End If
            For i As Integer = 6 To 1 Step -1
                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                    If totalCR(i) <> 0 Then
                        insertSQl = " INSERT INTO " &
                               " rptBS(RecordID, Description, Amount, GroupID) " &
                               " VALUES(@RecordID, @Description, @Amount, @GroupID)"
                        SQL.FlushParams()
                        SQL.AddParam("@RecordID", recID)
                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                        SQL.AddParam("@Amount", totalCR(i))
                        SQL.AddParam("@GroupID", i)
                        SQL.ExecNonQuery(insertSQl)
                        incre = 0
                        totalDesc(i) = Nothing
                        totalCR(i) = Nothing
                        recID += 1
                    End If
                End If
            Next
        End If
        Response.Write("<script>window.open('Reports.aspx?id=' + 'FSFP', '_blank');</script>")
    End Sub

    Private Sub GenerateIS(ByVal DateFrom As Date, ByVal DateTo As Date)
        Dim deleteSQl As String
        deleteSQl = " DELETE FROM rptIS "
        SQL.ExecNonQuery(deleteSQl)
        Dim dt As New DataTable
        Dim query As String
        Dim desc As String
        Dim AccountCode As String
        Dim groupID As Integer = 0
        Dim value As Decimal = 0
        Dim valueMonth As Decimal = 0
        Dim totalDesc(7) As String
        Dim totalCR(7) As Decimal
        Dim insertSQl As String
        Dim prevID As Integer = 0
        Dim recID As Integer = 1
        Dim incre As Integer = 1

        query = " SELECT  CASE WHEN AccountGroup ='Sub Account' THEN AccountCode  + ' - ' + CASE WHEN ISNULL(ReportAlias,'') ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 			 ELSE CASE WHEN ReportAlias ='' THEN AccountTitle ELSE ReportAlias END " & vbCrLf &
                " 		END  AS Descrition," & vbCrLf &
                "  CASE	WHEN AccountGroup = 'Account Type' THEN 'G1' WHEN AccountGroup = 'Account Sub Type' THEN 'G2' " & vbCrLf &
                " 		WHEN AccountGroup = 'Category' THEN 'G3'	WHEN AccountGroup = 'Sub Category' THEN 'G4' " & vbCrLf &
                " 		WHEN AccountGroup = 'Cost Center' THEN 'G5'	WHEN AccountGroup = 'Main Account' THEN 'G6' " & vbCrLf &
                " 		WHEN AccountGroup = 'Sub Account' THEN 'G7' " & vbCrLf &
                "  ELSE ''	END AS AccountGroup, " & vbCrLf &
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf &
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf &
                " 			ELSE 0 " & vbCrLf &
                " 	   END AS Amount," & vbCrLf &
                "      showTotal, contraAccount, " & vbCrLf &
                " 	   CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(MonthDebit,0) - ISNULL(MonthCredit,0))  " & vbCrLf &
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(MonthCredit,0) - ISNULL(MonthDebit,0)) " & vbCrLf &
                " 			ELSE 0 " & vbCrLf &
                " 	   END AS MonthAmount," & vbCrLf &
                " 	   tblCOA.AccountCode" & vbCrLf &
                " FROM tblCOA " & vbCrLf &
                " LEFT JOIN " & vbCrLf &
                " ( " & vbCrLf &
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS Debit, SUM(Credit)  AS Credit " & vbCrLf &
                " 	FROM      view_GL  " & vbCrLf &
                " 	WHERE     AppDate BETWEEN CAST('01-01-" & DateFrom.Year & "' AS DATE) AND '" & DateTo.Date & "' " & vbCrLf &
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf &
                " ) AS TB " & vbCrLf &
                " ON  tblCOA.AccountCode = TB.AccntCode " & vbCrLf &
                " LEFT JOIN " & vbCrLf &
                " ( " & vbCrLf &
                " 	SELECT    AccntCode, AccntTitle, SUM(Debit) AS MonthDebit, SUM(Credit)  AS MonthCredit " & vbCrLf &
                " 	FROM      view_GL  " & vbCrLf &
                " 	WHERE     AppDate BETWEEN CAST('" & DateTo.Month & "-01-" & DateFrom.Year & "' AS DATE) AND '" & DateTo.Date & "' " & vbCrLf &
                " 	GROUP BY  AccntCode, AccntTitle " & vbCrLf &
                " ) AS MB " & vbCrLf &
                " ON  tblCOA.AccountCode = MB.AccntCode " & vbCrLf &
                " WHERE  AccountType ='Income Statement' " & vbCrLf &
                " GROUP BY AccountCode, AccountTitle, ReportAlias, AccountGroup, AccountNature, showTotal, OrderNo, contraAccount " & vbCrLf &
                " HAVING  (AccountGroup <> 'Sub Account' OR " & vbCrLf &
                "        (AccountGroup = 'Sub Account'  AND " & vbCrLf &
                "        (CASE WHEN AccountNature = 'Debit' THEN SUM(ISNULL(Debit,0) - ISNULL(Credit,0))  " & vbCrLf &
                " 	        WHEN AccountNature = 'Credit' THEN SUM(ISNULL(Credit,0) - ISNULL(Debit,0)) " & vbCrLf &
                " 			ELSE 0 " & vbCrLf &
                " 	   END) <> 0)) " & vbCrLf &
                " ORDER BY OrderNo "
        SQL.GetQuery(query)
        If SQL.SQLDS.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In SQL.SQLDS.Tables(0).Rows
                desc = row(0).ToString
                groupID = CInt(row(1).ToString.Replace("G", ""))
                value = row(2)
                valueMonth = row(5)
                If row(3) = True Then
                    totalDesc(groupID) = desc
                End If
                If row(4) = True Then
                    value = value * -1
                    valueMonth = valueMonth * -1
                End If
                AccountCode = row(6).ToString
                If groupID <> prevID Or groupID = 7 Then

                    If prevID > groupID Then
                        If prevID <> 7 Then
                            For i As Integer = incre - 1 To 0 Step -1
                                deleteSQl = " DELETE FROM rptIS WHERE RecordID = '" & recID - 1 & "' "
                                SQL.ExecNonQuery(deleteSQl)
                                recID -= 1
                            Next

                            incre = 0
                        Else
                            incre = 0
                        End If
                        For i As Integer = 7 To 1 Step -1
                            If groupID <= i Then
                                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                    If totalCR(i) <> 0 Then
                                        insertSQl = " INSERT INTO " &
                                               " rptIS(RecordID, Description, Amount, AmountMonth, GroupID, AccountCode) " &
                                               " VALUES(@RecordID, @Description, @Amount, @AmountMonth, @GroupID, @AccountCode)"
                                        SQL.FlushParams()
                                        SQL.AddParam("@RecordID", recID)
                                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                                        SQL.AddParam("@Amount", totalCR(i))
                                        SQL.AddParam("@AmountMonth", totalCR(i))
                                        SQL.AddParam("@GroupID", i)
                                        SQL.AddParam("@AccountCode", totalCR(i))
                                        SQL.ExecNonQuery(insertSQl)
                                        incre = 0
                                        totalDesc(i) = Nothing
                                        totalCR(i) = Nothing
                                        recID += 1
                                    End If

                                End If
                            End If

                        Next
                    End If
                    insertSQl = " INSERT INTO " &
                        " rptIS(RecordID, Description, Amount, AmountMonth, GroupID, AccountCode) " &
                        " VALUES(@RecordID, @Description, @Amount, @AmountMonth, @GroupID, @AccountCode)"
                    SQL.FlushParams()
                    SQL.AddParam("@RecordID", recID)
                    SQL.AddParam("@Description", desc)
                    SQL.AddParam("@Amount", value)
                    SQL.AddParam("@AmountMonth", valueMonth)
                    SQL.AddParam("@GroupID", groupID)
                    SQL.AddParam("@AccountCode", AccountCode)
                    SQL.ExecNonQuery(insertSQl)
                    prevID = groupID
                    recID += 1
                    incre += 1
                    If value <> 0 Then
                        For i As Integer = 1 To 7
                            If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                                totalCR(i) += value
                            End If
                        Next
                    End If

                End If
            Next
            If prevID <> 7 Then
                For i As Integer = incre - 1 To 0 Step -1
                    deleteSQl = " DELETE FROM rptIS WHERE RecordID = '" & recID - 1 & "' "
                    SQL.ExecNonQuery(deleteSQl)
                    recID -= 1
                Next
            End If
            For i As Integer = 7 To 1 Step -1
                If Not IsNothing(totalDesc(i)) AndAlso totalDesc(i) <> "" Then
                    If totalCR(i) <> 0 Then
                        insertSQl = " INSERT INTO " &
                               " rptIS(RecordID, Description, Amount, GroupID, AccountCode) " &
                               " VALUES(@RecordID, @Description, @Amount, @GroupID, @AccountCode)"
                        SQL.FlushParams()
                        SQL.AddParam("@RecordID", recID)
                        SQL.AddParam("@Description", "TOTAL " & totalDesc(i))
                        SQL.AddParam("@Amount", totalCR(i))
                        SQL.AddParam("@GroupID", i)
                        SQL.AddParam("@AccountCode", totalCR(i))
                        SQL.ExecNonQuery(insertSQl)
                        incre = 0
                        totalDesc(i) = Nothing
                        totalCR(i) = Nothing
                        recID += 1
                    End If
                End If
            Next
        End If
        Response.Write("<script>window.open('Reports.aspx?id=' + 'FSIS', '_blank');</script>")
    End Sub

    Private Sub GenerateCE(ByVal DateFrom As Date, ByVal DateTo As Date)
        Dim insertSQL, deleteSQL As String
        deleteSQL = " DELETE FROM tblPRint_TB "
        SQL.ExecNonQuery(deleteSQL)
        insertSQL = " INSERT INTO tblPRint_TB(Code, Title, BBDR, BBCR, CRDR, CRCR, TBDR, TBCR) " &
                     " SELECT  AccountCode, AccountTitle,  " &
                     " 		  CASE WHEN SUM(BBDR) > SUM(BBCR) THEN SUM(BBDR) - SUM(BBCR) ELSE 0 END AS BBDR, " &
                     " 		  CASE WHEN SUM(BBCR) > SUM(BBDR) THEN SUM(BBCR) - SUM(BBDR) ELSE 0 END AS BBDR, " &
                     " 		  CASE WHEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) > SUM(CRCR + CDCR + JVCR + PBCR + SBCR) THEN SUM(CRDR + CDDR + JVDR + PBDR + SBDR) - SUM(CRCR + CDCR + JVCR + PBCR + SBCR) ELSE 0 END AS CRDR, " &
                     " 		  CASE WHEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) > SUM(CRDR + CDDR + JVDR + PBDR + SBDR) THEN SUM(CRCR + CDCR + JVCR + PBCR + SBCR) - SUM(CRDR + CDDR + JVDR + PBDR + SBDR) ELSE 0 END AS CRCR, " &
                     " 		  CASE WHEN SUM(TBDR) > SUM(TBCR) THEN SUM(TBDR) - SUM(TBCR) ELSE 0 END AS TBDR, " &
                     " 		  CASE WHEN SUM(TBCR) > SUM(TBDR) THEN SUM(TBCR) - SUM(TBDR) ELSE 0 END AS TBCR " &
                     " FROM " &
                     " ( " &
                     " 	SELECT tblCOA.AccountCode, tblCOA.AccountTitle,  " &
                    " 		   CASE WHEN AppDate <'" & DateFrom & "' OR Book ='BB' THEN Debit ELSE 0 END AS BBDR, " &
                    " 		   CASE WHEN AppDate < '" & DateFrom & "' OR Book ='BB' THEN Credit ELSE 0 END AS BBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Debit ELSE 0 END AS CRDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Receipt' THEN Credit ELSE 0 END AS CRCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Debit ELSE 0 END AS CDDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Cash Disbursement' THEN Credit ELSE 0 END AS CDCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Debit ELSE 0 END AS SBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Sales Book' THEN Credit ELSE 0 END AS SBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Debit ELSE 0 END AS PBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='Purchase Book' THEN Credit ELSE 0 END AS PBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Debit ELSE 0 END AS JVDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND Book ='General Journal' THEN Credit ELSE 0 END AS JVCR, " &
                    " 		   Debit AS TBDR, " &
                    " 		   Credit AS TBCR, View_GL.Status " &
                    " 	FROM View_GL JOIN tblCOA " &
                    " 	ON View_GL.AccntCode = tblCOA.AccountCode  " &
                    " 	WHERE AppDate BETWEEN '01-01-" & DateFrom.Year & "'And '" & DateTo & "' AND  tblCOA.AccountCode LIKE '3%'" &
                    " ) AS A " &
                    " WHERE A.Status ='Saved'  " &
                    " GROUP BY AccountCode, AccountTitle "
        SQL.ExecNonQuery(insertSQL)
        Response.Write("<script>window.open('Reports.aspx?id=' + 'FSCE', '_blank');</script>")
    End Sub
End Class