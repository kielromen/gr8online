Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class QueryGenerator
    Inherits System.Web.UI.Page

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
        hfFromAccountCode.Style.Add("visibility", "hidden")
        hfToAccountCode.Style.Add("visibility", "hidden")
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
        ddlFiscal.Items.Clear()
        ddlFiscal.Items.Add("MTD")
        ddlFiscal.Items.Add("YTD")
        ddlMonth.SelectedIndex = Now.Month - 1
        txtYear.Text = Now.Year
        LoadPeriod()
        rbCDB.Checked = True
        rbCRB.Checked = True
        rbGJ.Checked = True
        rbPB.Checked = True
        rbSB.Checked = True
        rbBB.Checked = True
        rbPEC.Checked = True
        rbSummary.Checked = True
        rbTransDate.Checked = True
    End Sub

    Public Sub LoadPeriod()
        Dim period As String = (ddlMonth.SelectedIndex + 1).ToString & "-1-" & txtYear.Text.ToString
        If ddlFiscal.SelectedValue = "MTD" Then
            dtpFrom.Value = CDate(Date.Parse(period)).ToString("yyyy-MM-dd")
            dtpTo.Value = CDate(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))).ToString("yyyy-MM-dd")
        ElseIf ddlFiscal.SelectedValue = "YTD" Then
            dtpFrom.Value = CDate(Date.Parse("1-1-" & txtYear.Text.ToString)).ToString("yyyy-MM-dd")
            dtpTo.Value = CDate(DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, Date.Parse(period)))).ToString("yyyy-MM-dd")
        End If
    End Sub

    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE '%' + @AccountTitle + '%' AND Status = @Status ORDER BY AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            AccountTitle.Add(String.Format("{0}--{1}", SQL.SQLDR("AccountTitle"), SQL.SQLDR("AccountCode")))
        End While
        Return AccountTitle.ToArray()
    End Function

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        If txtFromAccountTitle.Text = "" Or txtToAccountTitle.Text = "" Then
            Response.Write("<script>alert('Please select account code!');</script>")
        Else
            Dim ds As New DataTable()
            ds = Nothing
            gvListSummary.DataSource = ds
            gvListSummary.DataBind()
            gvListDetailed.DataSource = ds
            gvListDetailed.DataBind()
            If rbDetailed.Checked = True Then
                LoadDetailed()
            Else

                LoadSummary()
            End If
        End If
    End Sub

    Public Sub LoadSummary()
        Dim query As String
        If ddlFiscal.SelectedValue = "MTD" AndAlso rbBB.Checked = True Then
            query = "  SELECT   tblCOA.AccountCode, tblCOA.AccountTitle,   " & vbCrLf &
                    "           CASE WHEN SUM(Debit) > SUM(Credit) THEN  CONVERT(VARCHAR,CONVERT(MONEY,SUM(Debit) - SUM(Credit)),1) ELSE '0.00' END AS Debit,  " & vbCrLf &
                    "           CASE WHEN SUM(Credit) > SUM(Debit) THEN  CONVERT(VARCHAR,CONVERT(MONEY,SUM(Credit) - SUM(Debit)),1)  ELSE '0.00' END AS Credit,  " & vbCrLf &
                    "           tblCOA.AccountType  " & vbCrLf &
                    "  FROM      " & vbCrLf &
                    "  (      SELECT  JE_No, VCECode, RefTransID, RefType,  AccntCode, AccntTitle,   " & vbCrLf &
                    "  		        Debit, Credit, AppDate   " & vbCrLf &
                    "         FROM	view_GL WHERE status <> 'Cancelled'  ) AS JE   " & vbCrLf &
                    "  INNER JOIN tblCOA   " & vbCrLf &
                    "  ON	    JE.AccntCode = tblCOA.AccountCode   " & vbCrLf &
                    "  WHERE    JE.AppDate BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'   " & vbCrLf &
                    "  AND	    tblCOA.AccountCode  >= N'" & hfFromAccountCode.Text & "'  " & vbCrLf &
                    "  AND      tblCOA.AccountCode  <= N'" & hfToAccountCode.Text & "'  " & vbCrLf &
                    "  AND	    RefType IN ('" & IIf(rbCRB.Checked = True, "OR','AR','CR','PR','DEPOSIT", "") & "','" & IIf(rbPEC.Checked = True, "Period End Closing", "") & "','" & IIf(rbCDB.Checked = True, "CV','CHKV", "") & "','" & IIf(rbGJ.Checked = True, "JV", "") & "','" & IIf(rbPB.Checked = True, "PJ','APV", "") & "','" & IIf(rbSB.Checked = True, "SJ", "") & "','" & IIf(rbBB.Checked = True, "BB", "") & "')  " & vbCrLf &
                    "  GROUP BY tblCOA.AccountCode, tblCOA.AccountTitle, tblCOA.AccountType "
        Else
            query = "  SELECT   tblCOA.AccountCode, tblCOA.AccountTitle,   " & vbCrLf &
                    "           CASE WHEN SUM(Debit) > SUM(Credit) THEN CONVERT(VARCHAR,CONVERT(MONEY,SUM(Debit) - SUM(Credit)),1) ELSE '0.00'  END AS Debit,  " & vbCrLf &
                    "           CASE WHEN SUM(Credit) > SUM(Debit) THEN CONVERT(VARCHAR,CONVERT(MONEY,SUM(Credit) - SUM(Debit)),1) ELSE '0.00'  END AS Credit,  " & vbCrLf &
                    "           tblCOA.AccountType " & vbCrLf &
                    "  FROM     view_GL INNER JOIN tblCOA   " & vbCrLf &
                    "  ON	    view_GL.AccntCode = tblCOA.AccountCode   " &
                    "  WHERE    view_GL.status <> 'Cancelled' AND view_GL.AppDate BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'   " & vbCrLf &
                    "  AND	    tblCOA.AccountCode >= N'" & hfFromAccountCode.Text & "'  " & vbCrLf &
                    "  AND      tblCOA.AccountCode <= N'" & hfToAccountCode.Text & "'  " & vbCrLf &
                    "  AND	    RefType IN ('" & IIf(rbCRB.Checked = True, "OR','AR','CR','PR','DEPOSIT", "") & "','" & IIf(rbPEC.Checked = True, "Period End Closing", "") & "','" & IIf(rbCDB.Checked = True, "CV','CHKV", "") & "','" & IIf(rbGJ.Checked = True, "JV", "") & "','" & IIf(rbPB.Checked = True, "PJ','APV", "") & "','" & IIf(rbSB.Checked = True, "SJ", "") & "','" & IIf(rbBB.Checked = True, "BB", "") & "')  " & vbCrLf &
                    "  GROUP BY tblCOA.AccountCode, tblCOA.AccountTitle, tblCOA.AccountType "
        End If
        SQL.GetQuery(query)
        gvListSummary.Columns(1).FooterText = "Total"
        gvListSummary.Columns(1).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        gvListSummary.Columns(2).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        gvListSummary.Columns(3).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        totalDebit = 0
        totalCredit = 0
        gvListSummary.DataSource = SQL.SQLDS
        gvListSummary.ShowFooter = True
        gvListSummary.DataBind()


    End Sub

    Public Sub LoadDetailed()
        Dim query As String
        If ddlFiscal.SelectedValue = "MTD" AndAlso rbBB.Checked = True Then
            query = "  SELECT   tblCOA.AccountCode, tblCOA.AccountTitle,   " & vbCrLf &
                    "           CONVERT(VARCHAR,CONVERT(MONEY,Debit),1) AS Debit,CONVERT(VARCHAR,CONVERT(MONEY,Credit),1) AS Credit, VCECode, VCEName, Book, RefType, CASE WHEN RefType ='BB' THEN CAST(JE.JE_No AS nvarchar) ELSE CAST(ISNULL(JE.TransNo,RefTransID) AS nvarchar) END AS TransNo, RefTransID as RefTransID, REPLACE(CAST(Appdate as DATE),' 12:00:00 AM','') as Appdate,  " & vbCrLf &
                    "           tblCOA.AccountType, Particulars  " & vbCrLf &
                    "  FROM      " & vbCrLf &
                    "  (      SELECT  JE_No, VCECode, VCEName, TransNo, RefTransID, RefType,  AccntCode, AccntTitle,   " & vbCrLf &
                    "  		         Debit AS Debit, Credit AS Credit, AppDate, Book, Particulars   " & vbCrLf &
                    "         FROM	view_GL WHERE   view_GL.status <> 'Cancelled'  ) AS JE   " & vbCrLf &
                    "  INNER JOIN tblCOA   " & vbCrLf &
                    "  ON	    JE.AccntCode = tblCOA.AccountCode   " & vbCrLf &
                    "  WHERE    JE.AppDate BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'   " & vbCrLf &
                    "  AND	    tblCOA.AccountCode >= N'" & hfFromAccountCode.Text & "'  " & vbCrLf &
                    "  AND      tblCOA.AccountCode <= N'" & hfToAccountCode.Text & "'  " & vbCrLf &
                    "  AND	    RefType IN ('" & IIf(rbCRB.Checked = True, "OR','AR','CR','PR','DEPOSIT", "") & "','" & IIf(rbPEC.Checked = True, "Period End Closing", "") & "','" & IIf(rbCDB.Checked = True, "CV','CHKV", "") & "','" & IIf(rbGJ.Checked = True, "JV", "") & "','" & IIf(rbPB.Checked = True, "PJ','APV", "") & "','" & IIf(rbSB.Checked = True, "SJ", "") & "','" & IIf(rbBB.Checked = True, "BB", "") & "')  " & OrderBy()
        Else
            query = "  SELECT   tblCOA.AccountCode, tblCOA.AccountTitle,   " & vbCrLf &
                    "           CONVERT(VARCHAR,CONVERT(MONEY,Debit),1) AS Debit, CONVERT(VARCHAR,CONVERT(MONEY,Credit),1) AS Credit, VCECode, VCEName, Book, RefType, CASE WHEN RefType ='BB' THEN CAST(view_GL.JE_No AS nvarchar) ELSE CAST(ISNULL(TransNo,RefTransID) AS nvarchar) END AS TransNo, RefTransID as RefTransID, REPLACE(CAST(Appdate as DATE),' 12:00:00 AM','') as Appdate,  " & vbCrLf &
                    "           tblCOA.AccountType, Particulars " & vbCrLf &
                    "  FROM     view_GL INNER JOIN tblCOA   " & vbCrLf &
                    "  ON	    view_GL.AccntCode = tblCOA.AccountCode   " & vbCrLf &
                    "  WHERE    view_GL.status <> 'Cancelled' AND  view_GL.AppDate BETWEEN '" & dtpFrom.Value & "' AND '" & dtpTo.Value & "'   " & vbCrLf &
                    "  AND	    tblCOA.AccountCode >= N'" & hfFromAccountCode.Text & "'  " & vbCrLf &
                    "  AND      tblCOA.AccountCode <= N'" & hfToAccountCode.Text & "'  " & vbCrLf &
                    "  AND	    RefType IN ('" & IIf(rbCRB.Checked = True, "OR','AR','CR','PR','DEPOSIT", "") & "','" & IIf(rbPEC.Checked = True, "Period End Closing", "") & "','" & IIf(rbCDB.Checked = True, "CV','CHKV", "") & "','" & IIf(rbGJ.Checked = True, "JV", "") & "','" & IIf(rbPB.Checked = True, "PJ','APV", "") & "','" & IIf(rbSB.Checked = True, "SJ", "") & "','" & IIf(rbBB.Checked = True, "BB", "") & "')  " & OrderBy()
        End If
        SQL.GetQuery(query)
        gvListDetailed.Columns(2).FooterText = "Total"
        gvListDetailed.Columns(2).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        gvListDetailed.Columns(3).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        gvListDetailed.Columns(4).FooterStyle.HorizontalAlign = HorizontalAlign.Right
        totalDebit = 0
        totalCredit = 0
        gvListDetailed.DataSource = SQL.SQLDS
        gvListDetailed.DataBind()
    End Sub

    Private Function OrderBy() As String
        If rbTransDate.Checked = True Then
            Return " ORDER BY AppDate"
        ElseIf rbAccountCode.Checked = True Then
            Return " ORDER BY tblCOA.AccountCode "
        ElseIf rbAccountTitle.Checked = True Then
            Return " ORDER BY tblCOA.AccountTitle "
        ElseIf rbTransNo.Checked = True Then
            Return " ORDER BY RefTransID "
        Else
            Return ""
        End If
    End Function

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvListDetailed, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If rbDetailed.Checked = True Then
            Dim RefType As String = gvListDetailed.SelectedRow.Cells(5).Text
            Dim RefTransID As String = gvListDetailed.SelectedRow.Cells(11).Text
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
            End Select
        End If
    End Sub

    Dim totalDebit, totalCredit As Decimal
    Private Sub gvListSummary_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvListSummary.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = e.Row.DataItem
            totalDebit += Convert.ToDouble(row(2))
            totalCredit += Convert.ToDouble(row(3))
            gvListSummary.Columns(2).FooterText = totalDebit.ToString("N2")
            gvListSummary.Columns(3).FooterText = totalCredit.ToString("N2")
        End If
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs)
        If gvListSummary.Rows.Count > 0 Then
            Dim dt As New DataTable("Summary")
            For Each cell As TableCell In gvListSummary.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvListSummary.Rows
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    row.Cells(i).CssClass = "textmode"
                    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text.ToString.Replace("&nbsp;", "")
                Next
            Next
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=Summary.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If

        If gvListDetailed.Rows.Count > 0 Then
            Dim dt As New DataTable("Detailed")
            For Each cell As TableCell In gvListDetailed.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvListDetailed.Rows
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    dt.Rows(dt.Rows.Count - 1)(i) = row.Cells(i).Text.ToString.Replace("&nbsp;", "")
                Next
            Next
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=Detailed.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub


    Private Sub gvListDetailed_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvListDetailed.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = e.Row.DataItem
            totalDebit += Convert.ToDouble(row(2))
            totalCredit += Convert.ToDouble(row(3))
            gvListDetailed.Columns(3).FooterText = totalDebit.ToString("N2")
            gvListDetailed.Columns(4).FooterText = totalCredit.ToString("N2")
        End If
    End Sub
End Class