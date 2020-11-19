Imports System.Web.Services

Public Class Dashboard1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                LoadUserCompany(Session("Company_Code"))
                LoadList()
                alertWelcome.Visible = True
            End If
        End If
    End Sub

    Private Sub LoadList()
        ddlAccntTitleAPV.Items.Clear()
        ddlAccntTitleAPV.DataSource = LoadType("APV").ToArray
        ddlAccntTitleAPV.DataBind()
        ddlAccntTitleYearAPV.Items.Clear()
        ddlAccntTitleYearAPV.DataSource = LoadTypeYear("APV").ToArray
        ddlAccntTitleYearAPV.DataBind()
        ddlAccntTitleMonthGM.Items.Clear()
        For i As Integer = 1 To 12
            ddlAccntTitleMonthGM.Items.Add(MonthName(i))
        Next
        ddlAccntTitleMonthGM.SelectedIndex = Now.Month - 1
        ddlAccntTitleYearGM.Items.Clear()
        ddlAccntTitleYearGM.DataSource = LoadTypeYear("Sales", "COS").ToArray
        ddlAccntTitleYearGM.DataBind()
        ddlAccntTitleSales.Items.Clear()
        ddlAccntTitleSales.DataSource = LoadType("Sales").ToArray
        ddlAccntTitleSales.DataBind()
        ddlAccntTitleYearSales.Items.Clear()
        ddlAccntTitleYearSales.DataSource = LoadTypeYear("Sales").ToArray
        ddlAccntTitleYearSales.DataBind()
    End Sub

    Public Function LoadType(ByVal Type As String, Optional Type2 As String = "") As List(Of String)
        Dim list As New List(Of String)
        Dim query As String = ""
        query = " SELECT AccountTitle FROM tblCOA INNER JOIN tblDefaultAccount ON tblDefaultAccount.AccountCode = tblCOA.AccountCode AND ModuleID IN (@ModuleID, @ModuleID2) "
        SQL.FlushParams()
        SQL.AddParam("@ModuleID", Type)
        SQL.AddParam("@ModuleID2", Type2)
        SQL.ReadQuery(query)
        list.Add("All")
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("AccountTitle").ToString)
        End While
        Return list
    End Function


    Public Function LoadTypeYear(ByVal Type As String, Optional Type2 As String = "") As List(Of String)
        Dim list As New List(Of String)
        Dim query As String = ""
        query = " SELECT DISTINCT YEAR(AppDate) AS Year FROM View_GL WHERE AccntCode IN (SELECT AccountCode FROM tblDefaultAccount WHERE ModuleID IN (@ModuleID, @ModuleID2)) ORDER BY YEAR(AppDate) DESC "
        SQL.FlushParams()
        SQL.AddParam("@ModuleID", Type)
        SQL.AddParam("@ModuleID2", Type2)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Year").ToString)
        End While
        If list.Count = 0 Then
            list.Add(Now.Year)
        End If
        Return list
    End Function

    Public Sub LoadUserCompany(ByVal CompanyCode As Integer)
        Dim query As String
        query = " SELECT * " &
                " FROM [Main].dbo.tblCompany_Information" &
                " WHERE Status = @Status AND Company_Code = @Company_Code"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Company_Code", CompanyCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Session("Company_Name") = SQL.SQLDR("Company_Name").ToString
            Session("TIN_No") = SQL.SQLDR("TIN_No").ToString
        End If
        lblUserCompany.Text = Session("Company_Name")
    End Sub

    <WebMethod()>
    Public Shared Function GetChartType(ByVal AccountTitle As String, ByVal ModuleID As String) As List(Of Object)
        Dim chartData As New List(Of Object)()
        Dim query As String = ""
        query = " WITH Months AS " & vbCrLf &
                " ( " & vbCrLf &
                "     SELECT CAST(@Year + '-01-01' AS Date) AS Months " & vbCrLf &
                "     UNION ALL " & vbCrLf &
                "     SELECT DATEADD(MONTH, 1, Months) " & vbCrLf &
                "     FROM Months " & vbCrLf &
                "     WHERE DATEPART(MONTH, Months) < @Month " & vbCrLf &
                " ) " & vbCrLf &
                " SELECT DATENAME(MONTH, Months) AS Month FROM Months "
        SQL.FlushParams()
        SQL.AddParam("@Year", Now.Year)
        SQL.AddParam("@Month", 12)
        SQL.GetDataTable(query)
        Dim dtYears As DataTable = SQL.SQLDT
        Dim labels As New List(Of String)()
        For Each row As DataRow In dtYears.Rows
            labels.Add(row("Month"))
        Next
        chartData.Add(labels)

        query = " WITH Months AS " & vbCrLf &
                " ( " & vbCrLf &
                "     SELECT CAST(@Year + '-01-01' AS Date) AS Months " & vbCrLf &
                "     UNION ALL " & vbCrLf &
                "     SELECT DATEADD(MONTH, 1, Months) " & vbCrLf &
                "     FROM Months " & vbCrLf &
                "     WHERE DATEPART(MONTH, Months) < @Month " & vbCrLf &
                " ) " & vbCrLf &
                " SELECT DATENAME(MONTH, Months) AS Month, ISNULL(SUM(Balance) OVER(ORDER BY Months), 0) AS Balance FROM Months " & vbCrLf &
                " LEFT JOIN ( " & vbCrLf &
                "   SELECT DATENAME(MONTH, AppDate) AS Month, SUM(Credit - Debit) AS Balance " & vbCrLf &
                "   FROM view_GL WHERE YEAR(AppDate) = @Year AND MONTH(AppDate) <= @Month " & vbCrLf &
                "   AND Status <> 'Cancelled' " & vbCrLf &
                "   AND AccntCode IN (SELECT tblDefaultAccount.AccountCode FROM tblDefaultAccount INNER JOIN tblCOA ON tblDefaultAccount.AccountCode = tblCOA.AccountCode AND CASE WHEN @AccountTitle = 'All' THEN @AccountTitle ELSE AccountTitle END = @AccountTitle WHERE ModuleID = @ModuleID) " & vbCrLf &
                "   GROUP BY DATENAME(MONTH, AppDate) " & vbCrLf &
                " ) AS Balance ON DATENAME(MONTH, Months) = Month "

        SQL.FlushParams()
        SQL.AddParam("@Year", Now.Year)
        SQL.AddParam("@Month", 12)
        SQL.AddParam("@ModuleID", ModuleID)
        SQL.AddParam("@AccountTitle", AccountTitle)
        SQL.GetDataTable(query)
        Dim dtBalance As DataTable = SQL.SQLDT
        Dim series1 As New List(Of Decimal)()
        For Each row As DataRow In dtBalance.Rows
            series1.Add(CDec(row("Balance")))
        Next
        chartData.Add(series1)

        Return chartData
    End Function

    <WebMethod()>
    Public Shared Function GetChartTypeByMonth(ByVal AccountTitle As String, ByVal ModuleID As String) As List(Of Object)
        Dim chartData As New List(Of Object)()
        Dim query As String = ""
        query = " WITH Months AS " & vbCrLf &
                " ( " & vbCrLf &
                "     SELECT CAST(@Year + '-01-01' AS Date) AS Months " & vbCrLf &
                "     UNION ALL " & vbCrLf &
                "     SELECT DATEADD(MONTH, 1, Months) " & vbCrLf &
                "     FROM Months " & vbCrLf &
                "     WHERE DATEPART(MONTH, Months) < @Month " & vbCrLf &
                " ) " & vbCrLf &
                " SELECT DATENAME(MONTH, Months) AS Month FROM Months "
        SQL.FlushParams()
        SQL.AddParam("@Year", Now.Year)
        SQL.AddParam("@Month", 12)
        SQL.GetDataTable(query)
        Dim dtYears As DataTable = SQL.SQLDT
        Dim labels As New List(Of String)()
        For Each row As DataRow In dtYears.Rows
            labels.Add(row("Month"))
        Next
        chartData.Add(labels)

        query = " WITH Months AS " & vbCrLf &
                " ( " & vbCrLf &
                "     SELECT CAST(@Year + '-01-01' AS Date) AS Months " & vbCrLf &
                "     UNION ALL " & vbCrLf &
                "     SELECT DATEADD(MONTH, 1, Months) " & vbCrLf &
                "     FROM Months " & vbCrLf &
                "     WHERE DATEPART(MONTH, Months) < @Month " & vbCrLf &
                " ) " & vbCrLf &
                " SELECT DATENAME(MONTH, Months) AS Month, ISNULL(Balance, 0) AS Balance FROM Months " & vbCrLf &
                " LEFT JOIN ( " & vbCrLf &
                "   SELECT DATENAME(MONTH, AppDate) AS Month, SUM(Credit - Debit) AS Balance " & vbCrLf &
                "   FROM view_GL WHERE YEAR(AppDate) = @Year AND MONTH(AppDate) <= @Month " & vbCrLf &
                "   AND Status <> 'Cancelled' " & vbCrLf &
                "   AND AccntCode IN (SELECT tblDefaultAccount.AccountCode FROM tblDefaultAccount INNER JOIN tblCOA ON tblDefaultAccount.AccountCode = tblCOA.AccountCode AND CASE WHEN @AccountTitle = 'All' THEN @AccountTitle ELSE AccountTitle END = @AccountTitle WHERE ModuleID = @ModuleID) " & vbCrLf &
                "   GROUP BY DATENAME(MONTH, AppDate) " & vbCrLf &
                " ) AS Balance ON DATENAME(MONTH, Months) = Month "

        SQL.FlushParams()
        SQL.AddParam("@Year", Now.Year)
        SQL.AddParam("@Month", 12)
        SQL.AddParam("@ModuleID", ModuleID)
        SQL.AddParam("@AccountTitle", AccountTitle)
        SQL.GetDataTable(query)
        Dim dtBalance As DataTable = SQL.SQLDT
        Dim series1 As New List(Of Decimal)()
        For Each row As DataRow In dtBalance.Rows
            series1.Add(CDec(row("Balance")))
        Next
        chartData.Add(series1)

        Return chartData
    End Function

    <WebMethod()>
    Public Shared Function GetGM(ByVal strMonth As String, ByVal strYear As String) As List(Of Object)
        Dim chartData As New List(Of Object)()
        Dim query As String = ""
        Dim labels As New List(Of String)()
        labels.Add("Sales")
        labels.Add("COS")
        chartData.Add(labels)

        query = " SELECT Description, CASE WHEN AccountNature = 'Debit' THEN SUM(Debit - Credit) ELSE SUM(Credit - Debit) END AS Balance FROM View_GL " & vbCrLf &
                " INNER JOIN tblDefaultAccount ON AccountCode = AccntCode AND ModuleID IN ('Sales', 'COS') " & vbCrLf &
                " WHERE YEAR(AppDate) = @Year AND MONTH(AppDate) = @Month " & vbCrLf &
                " AND View_GL.Status <> 'Cancelled' " & vbCrLf &
                " GROUP BY Description, AccountNature ORDER BY CASE WHEN Description = 'Sales' THEN 1 ELSE 2 END "

        SQL.FlushParams()
        SQL.AddParam("@Year", strYear)
        SQL.AddParam("@Month", Month(strMonth & " 1, " & strYear))
        SQL.GetDataTable(query)
        Dim dtBalance As DataTable = SQL.SQLDT
        Dim series1 As New List(Of Decimal)()
        For Each row As DataRow In dtBalance.Rows
            series1.Add(CDec(row("Balance")))
        Next
        chartData.Add(series1)

        Return chartData
    End Function
End Class