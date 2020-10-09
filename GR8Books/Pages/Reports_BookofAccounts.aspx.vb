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
                " FROM    tblTransactionSetup WHERE Book = @Book "
        SQL.FlushParams()
        SQL.AddParam("@Book", Session("@Book"))
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
        If ddlReports.SelectedValue = "Cash Receipts Journal" Then
            Session("@Book") = "Cash Receipt"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Journal" Then
            Session("@Book") = "Cash Disbursement"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "General Journal" Then
            Session("@Book") = "General Journal"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Purchase Journal" Then
            Session("@Book") = "Purchase Book"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Sales Journal" Then
            Session("@Book") = "Sales Book"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Cash Receipts Book - Looseleaf" Then
            Session("@Book") = "Cash Receipt"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Book - Looseleaf" Then
            Session("@Book") = "Cash Disbursement"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "General Journal - Looseleaf" Then
            Session("@Book") = "General Journal"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Purchase Book - Looseleaf" Then
            Session("@Book") = "Purchase Book"
            LoadModules()
        ElseIf ddlReports.SelectedValue = "Sales Book - Looseleaf" Then
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

        If ddlReports.SelectedValue = "Cash Receipts Journal" Then
            GenerateBOA(CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Journal" Then
            GenerateBOA(CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "General Journal" Then
            GenerateBOA(CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "Purchase Journal" Then
            GenerateBOA(CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        ElseIf ddlReports.SelectedValue = "Sales Journal" Then
            GenerateBOA(CDate(dtpFromDate.Text), IIf(ddlPeriodType.SelectedValue = "Daily", CDate(dtpFromDate.Text), CDate(dtpToDate.Text)))
        Else
            GenerateBOALL()
        End If
    End Sub

    Private Sub GenerateBOA(ByVal DateFrom As Date, ByVal DateTo As Date)
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

        If ddlReportType.SelectedValue = "Summary" Then
            Dim insertSQL, deleteSQL As String
            deleteSQL = " DELETE FROM tblPRint_TB "
            SQL.ExecNonQuery(deleteSQL)
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
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Cash Receipt' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Debit ELSE 0 END AS CRDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Cash Receipt' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Credit ELSE 0 END AS CRCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Cash Disbursement' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Debit ELSE 0 END AS CDDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Cash Disbursement' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Credit ELSE 0 END AS CDCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Sales Book' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Debit ELSE 0 END AS SBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Sales Book' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Credit ELSE 0 END AS SBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Purchase Book' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Debit ELSE 0 END AS PBDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='Purchase Book' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Credit ELSE 0 END AS PBCR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='General Journal' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Debit ELSE 0 END AS JVDR, " &
                    " 		   CASE WHEN AppDate >= '" & DateFrom & "' AND (Book ='General Journal' AND RefType IN (SELECT RefType FROM tblPrint_BOA)) THEN Credit ELSE 0 END AS JVCR, " &
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
            Response.Write("<script>window.open('Reports.aspx?id=' + 'BOASUM', '_blank');</script>")
        ElseIf ddlReportType.SelectedValue = "Detailed" Then
            Response.Write("<script>window.open('Reports.aspx?id=' + 'BOADET', '_blank');</script>")
        End If
    End Sub

    Private Sub gvFilter_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvFilter.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("onmouseover", "MouseEvents(this, event)")
            e.Row.Attributes.Add("onmouseout", "MouseEvents(this, event)")
        End If
    End Sub

    Private Sub GenerateBOALL()
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
        If ddlReports.SelectedValue = "Cash Receipts Book - Looseleaf" Then
            Session("@Book") = "Cash Receipt"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'CRBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Cash Disbursement Book - Looseleaf" Then
            Session("@Book") = "Cash Disbursement"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'CDBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "General Journal - Looseleaf" Then
            Session("@Book") = "General Journal"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'GJLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Purchase Book - Looseleaf" Then
            Session("@Book") = "Purchase Book"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'PBLL', '_blank');</script>")
        ElseIf ddlReports.SelectedValue = "Sales Book - Looseleaf" Then
            Session("@Book") = "Sales Book"
            Response.Write("<script>window.open('Reports.aspx?id=' + 'SBLL', '_blank');</script>")
        End If
    End Sub
End Class