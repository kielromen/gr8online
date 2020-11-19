Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class BankRecon
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim BRNO As String
    Dim TransID As String
    Dim ModuleID As String = "BR"
    Dim ColumnPK As String = "BR_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblBR"
    Dim disableEvent As Boolean = False
    Dim Edit As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                If Session("ID") <> "" Then
                    LoadTransaction(Session("ID"))
                Else
                    btnSearch.Attributes.Remove("disabled")
                    btnNew.Attributes.Remove("disabled")
                    btnEdit.Attributes("disabled") = "disabled"
                    btnSave.Attributes("disabled") = "disabled"
                    btnCancel.Attributes("disabled") = "disabled"
                    btnPrev.Attributes("disabled") = "disabled"
                    btnNext.Attributes("disabled") = "disabled"
                    btnClose.Attributes("disabled") = "disabled"
                    btnPreview.Attributes("disabled") = "disabled"
                    EnableControl(False)
                    Session("TransID") = ""
                    dtpDoc_Date.Text = CDate(Date.Now).ToString("yyyy-MM-dd")
                End If
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtVariance.Attributes.Add("readonly", "readonly")
        txtAdjustedBankBalance.Attributes.Add("readonly", "readonly")
        txtAdjustedBookBalance.Attributes.Add("readonly", "readonly")
        txtDIT.Attributes.Add("readonly", "readonly")
        txtOC.Attributes.Add("readonly", "readonly")
        txtBookBalance.Attributes.Add("readonly", "readonly")
        txtAccountCode.Attributes.Add("readonly", "readonly")
        txtAccountTitle.Attributes.Add("readonly", "readonly")
        txtStatus.Attributes.Add("readonly", "readonly")

        txtTrans_Num.Text = ""

        txtAdjustedBankBalance.Text = "0.00"
        txtAdjustedBookBalance.Text = "0.00"
        txtBankBalance.Text = "0.00"
        txtBookBalance.Text = "0.00"
        txtDIT.Text = "0.00"
        txtOC.Text = "0.00"
        txtVariance.Text = "0.00"
        txtAccountCode.Text = ""
        txtAccountTitle.Text = ""
        txtStatus.Text = ""

        dtpDoc_Date.Text = Now.Date

        ddlBank.Items.Clear()
        ddlBank.Items.Add("--Select Bank--")
        ddlBank.DataSource = LoadBank().ToArray
        ddlBank.DataBind()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelBR.Enabled = Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        BRNO = ""
        Session("TransID") = ""
        txtStatus.Text = "Open"
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
        dtpDoc_Date.Attributes.Remove("readonly")

        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Text = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Public Sub RefreshData()
        LoadBankDetail(ddlBank.SelectedValue)
        LoadBookBalance(dtpDoc_Date.Text, txtAccountCode.Text)
        LoadDIT(dtpDoc_Date.Text, txtAccountCode.Text)
        LoadOC(dtpDoc_Date.Text, txtAccountCode.Text)
        LoadCleared(dtpDoc_Date.Text, txtAccountCode.Text)
        LoadAdjustedBookBalance()
        LoadAdjustedBankBalance()
        ComputeVariance()
    End Sub

    Private Sub LoadBankDetail(ByVal Bank As String)
        Dim query As String
        query = " SELECT tblBank.AccountCode, AccountTitle, tblBank.Status FROM tblBank " &
                " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA ON tblBank.AccountCode = tblCOA.AccountCode" &
                " WHERE tblBank.Status = 'Active' AND Bank = '" & Bank & "'"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Bank", Bank)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtAccountCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccountTitle.Text = SQL.SQLDR("AccountTitle").ToString
        End If
    End Sub

    Private Sub LoadBookBalance(ByVal AppDate As Date, ByVal AccountCode As String)
        Dim query As String
        query = " SELECT   ISNULL(SUM(Debit-Credit),0) AS BookBalance " & vbCrLf &
                " FROM     View_GL " & vbCrLf &
                " WHERE    View_GL.Status <> 'Cancelled' AND AppDate <=  @AppDate " & vbCrLf &
                " AND      AccntCode = @AccountCode "
        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtBookBalance.Text = CDec(SQL.SQLDR("BookBalance")).ToString("N2")
        Else
            txtBookBalance.Text = "0.00"
        End If
    End Sub

    Public Sub LoadDIT(ByVal AppDate As Date, ByVal AccountCode As String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ID, VCEName, Remarks, REPLACE(CAST(AppDate as DATE),' 12:00:00 AM','') as AppDate, RefTransID, RefType, TransNo,   CONVERT(VARCHAR,CONVERT(MONEY,SUM(Debit - Credit)),1) AS Amount  " &
                " FROM  View_GL " &
                " WHERE View_GL.Status <> 'Cancelled' AND AccntCode =  @AccountCode AND AppDate <= @AppDate" &
                " AND Book='Cash Receipt' " &
                " AND  (dateCleared IS NULL OR dateCleared  > @AppDate) " &
                " GROUP BY  ID, VCEName, Remarks, AppDate, RefTransID, RefType, TransNo" &
                " ORDER BY AppDate"
        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.GetQuery(query)
        gvDIT.DataSource = SQL.SQLDS
        gvDIT.DataBind()

        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.ReadQuery(query)
        txtDIT.Text = "0.00"
        While SQL.SQLDR.Read
            txtDIT.Text = CDec(txtDIT.Text) + CDec(SQL.SQLDR("Amount"))
        End While
        panelDIT.Visible = True
    End Sub

    Public Sub LoadOC(ByVal AppDate As Date, ByVal AccountCode As String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ID, VCEName, Remarks, CheckNo, REPLACE(CAST(AppDate as DATE),' 12:00:00 AM','') as AppDate, RefTransID, RefType, TransNo,   CONVERT(VARCHAR,CONVERT(MONEY,SUM(Credit - Debit)),1) AS Amount  " &
                " FROM  View_GL " &
                " WHERE View_GL.Status <> 'Cancelled' AND AccntCode =  @AccountCode AND AppDate <= @AppDate" &
                " AND Book='Cash Disbursement' " &
                " AND  (dateCleared IS NULL OR dateCleared  > @AppDate) " &
                " GROUP BY  ID, VCEName, Remarks, CheckNo, AppDate, RefTransID, RefType, TransNo" &
                " ORDER BY AppDate"
        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.GetQuery(query)
        gvOC.DataSource = SQL.SQLDS
        gvOC.DataBind()

        SQL.FlushParams()
        SQL.AddParam("@AppDate", AppDate)
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.ReadQuery(query)
        txtOC.Text = "0.00"
        While SQL.SQLDR.Read
            txtOC.Text = CDec(txtOC.Text) + CDec(SQL.SQLDR("Amount"))
        End While

        panelOC.Visible = True
    End Sub

    Public Sub LoadCleared(ByVal AppDate As Date, ByVal AccountCode As String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ID, REPLACE(CAST(AppDate as DATE),' 12:00:00 AM','') as AppDate, VCEName, Remarks, RefTransID, RefType, TransNo, CheckNo, " &
                " CASE WHEN Book = 'Cash Disbursement' THEN CONVERT(VARCHAR,CONVERT(MONEY,SUM(Credit - Debit)),1) ELSE CONVERT(VARCHAR,CONVERT(MONEY,SUM(Debit - Credit)),1) END AS Amount " &
                " FROM View_GL " &
                " WHERE View_GL.Status <> 'Cancelled' AND  dateCleared is not NULL AND AccntCode =  @AccountCode" &
                " GROUP BY  ID, Book,VCEName, Remarks, CheckNo, AppDate, RefTransID, RefType, TransNo" &
                " ORDER BY AppDate DESC"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.GetQuery(query)
        gvCleared.DataSource = SQL.SQLDS
        gvCleared.DataBind()
        panelCleared.Visible = True
    End Sub

    Private Sub LoadAdjustedBookBalance()
        Dim bookBal As Decimal
        If IsNumeric(txtBookBalance.Text) Then bookBal = CDec(txtBookBalance.Text) Else bookBal = 0
        txtAdjustedBookBalance.Text = CDec(bookBal).ToString("N2")
    End Sub

    Private Sub LoadAdjustedBankBalance()
        Dim DIT, CIB, OC, ADJ As Double
        If IsNumeric(txtBankBalance.Text) Then CIB = CDec(txtBankBalance.Text) Else CIB = 0
        If IsNumeric(txtDIT.Text) Then DIT = CDec(txtDIT.Text) Else DIT = 0
        If IsNumeric(txtOC.Text) Then OC = CDec(txtOC.Text) Else OC = 0
        ADJ = CIB + DIT - OC
        txtAdjustedBankBalance.Text = CDec(ADJ).ToString("N2")
    End Sub

    Private Sub ComputeVariance()
        Dim BankBal, BookBal, Variance As Decimal
        If IsNumeric(txtAdjustedBookBalance.Text) Then BookBal = CDec(txtAdjustedBookBalance.Text) Else BookBal = 0
        If IsNumeric(txtAdjustedBankBalance.Text) Then BankBal = CDec(txtAdjustedBankBalance.Text) Else BankBal = 0
        Variance = (BookBal - BankBal)
        txtVariance.Text = Variance.ToString("N2")
    End Sub

    Protected Sub txtBankBalance_TextChanged(sender As Object, e As EventArgs)
        LoadAdjustedBankBalance()
        ComputeVariance()
    End Sub

    Private Sub dtpDoc_Date_TextChanged(sender As Object, e As EventArgs) Handles dtpDoc_Date.TextChanged
        If CDate(dtpDoc_Date.Text) < CDate(GetMinDate(GetBankID(ddlBank.SelectedValue))) Then
            dtpDoc_Date.Text = CDate(GetMinDate(GetBankID(ddlBank.SelectedValue))).ToString("yyyy-MM-dd")
        End If

        RefreshData()
    End Sub

    Private Function GetMinDate(BankID As String) As Date
        Dim query As String
        query = "SELECT DATEADD(DAY,1,MAX(TransDate)) AS TransDate FROM tblBR WHERE Status <> 'Cancelled' AND BankID ='" & BankID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read And Not IsDBNull(SQL.SQLDR("TransDate")) Then
            Return SQL.SQLDR("TransDate")
        Else
            Return "1900-01-01"
        End If
    End Function

    Private Sub btnClearDIT_Click(sender As Object, e As EventArgs) Handles btnClearDIT.Click
        Dim withcheck As Boolean = False
        For Each row As GridViewRow In gvDIT.Rows
            If TryCast(row.FindControl("chkBox"), CheckBox).Checked Then
                withcheck = True
                Dim updateSQL As String
                updateSQL = " UPDATE tblJE_Details " & vbCrLf &
                            " SET dateCleared = @dateCleared " &
                            " WHERE  ID = @ID "
                SQL.FlushParams()
                SQL.AddParam("@ID", row.Cells(1).Text)
                SQL.AddParam("@dateCleared", dtpDoc_Date.Text)
                SQL.ExecNonQuery(updateSQL)
            End If
        Next
        If withcheck = False Then
            Response.Write("<script>alert('No selected transactions to be cleared.');</script>")
        End If
        RefreshData()


        Dim strClassnavClearedtab As String = navCLEAREDtab.Attributes("Class").Replace("active", "")
        navCLEAREDtab.Attributes.Remove("Class")
        navCLEAREDtab.Attributes.Add("Class", strClassnavClearedtab)

        Dim strClassnavClearedtabContent As String = navCLEARED.Attributes("Class").Replace("show active", "")
        navCLEARED.Attributes.Remove("Class")
        navCLEARED.Attributes.Add("Class", strClassnavClearedtabContent)

        Dim strClassnavOCtab As String = navOCtab.Attributes("Class").Replace("active", "")
        navOCtab.Attributes.Remove("Class")
        navOCtab.Attributes.Add("Class", strClassnavOCtab)

        Dim strClassnavOCtabContent As String = navOC.Attributes("Class").Replace("show active", "")
        navOC.Attributes.Remove("Class")
        navOC.Attributes.Add("Class", strClassnavOCtabContent)

        Dim strClassnavDITtab As String = navDITtab.Attributes("Class")
        navDITtab.Attributes.Remove("Class")
        navDITtab.Attributes.Add("Class", strClassnavDITtab & " active")

        Dim strClassnavDITtabcontent As String = navDIT.Attributes("Class")
        navDIT.Attributes.Remove("Class")
        navDIT.Attributes.Add("Class", strClassnavDITtabcontent & " show active")
    End Sub

    Private Sub btnClearOC_Click(sender As Object, e As EventArgs) Handles btnClearOC.Click
        Dim withcheck As Boolean = False
        For Each row As GridViewRow In gvOC.Rows
            If TryCast(row.FindControl("chkBox"), CheckBox).Checked Then
                withcheck = True
                Dim updateSQL As String
                updateSQL = " UPDATE tblJE_Details " & vbCrLf &
                            " SET dateCleared = @dateCleared " &
                            " WHERE  ID = @ID "
                SQL.FlushParams()
                SQL.AddParam("@ID", row.Cells(1).Text)
                SQL.AddParam("@dateCleared", dtpDoc_Date.Text)
                SQL.ExecNonQuery(updateSQL)
            End If
        Next
        If withcheck = False Then
            Response.Write("<script>alert('No selected transactions to be cleared.');</script>")
        End If
        RefreshData()

        Dim strClassnavDITtab As String = navDITtab.Attributes("Class").Replace("active", "")
        navDITtab.Attributes.Remove("Class")
        navDITtab.Attributes.Add("Class", strClassnavDITtab)

        Dim strClassnavDITtabcontent As String = navDIT.Attributes("Class").Replace("show active", "")
        navDIT.Attributes.Remove("Class")
        navDIT.Attributes.Add("Class", strClassnavDITtabcontent)

        Dim strClassnavClearedtab As String = navCLEAREDtab.Attributes("Class").Replace("active", "")
        navCLEAREDtab.Attributes.Remove("Class")
        navCLEAREDtab.Attributes.Add("Class", strClassnavClearedtab)

        Dim strClassnavClearedtabContent As String = navCLEARED.Attributes("Class").Replace("show active", "")
        navCLEARED.Attributes.Remove("Class")
        navCLEARED.Attributes.Add("Class", strClassnavClearedtabContent)

        Dim strClassnavOCtab As String = navOCtab.Attributes("Class")
        navOCtab.Attributes.Remove("Class")
        navOCtab.Attributes.Add("Class", strClassnavOCtab & " active")

        Dim strClassnavOCtabContent As String = navOC.Attributes("Class")
        navOC.Attributes.Remove("Class")
        navOC.Attributes.Add("Class", strClassnavOCtabContent & " show active")
    End Sub

    Private Sub btnUnclear_Click(sender As Object, e As EventArgs) Handles btnUnclear.Click
        Dim withcheck As Boolean = False
        For Each row As GridViewRow In gvCleared.Rows
            If TryCast(row.FindControl("chkBox"), CheckBox).Checked Then
                withcheck = True
                Dim updateSQL As String
                updateSQL = " UPDATE tblJE_Details " & vbCrLf &
                            " SET dateCleared = NULL " &
                            " WHERE  ID = @ID "
                SQL.FlushParams()
                SQL.AddParam("@ID", row.Cells(1).Text)
                SQL.ExecNonQuery(updateSQL)
            End If
        Next
        If withcheck = False Then
            Response.Write("<script>alert('No selected transactions to be uncleared.');</script>")
        End If
        RefreshData()

        Dim strClassnavDITtab As String = navDITtab.Attributes("Class").Replace("active", "")
        navDITtab.Attributes.Remove("Class")
        navDITtab.Attributes.Add("Class", strClassnavDITtab)

        Dim strClassnavDITtabcontent As String = navDIT.Attributes("Class").Replace("show active", "")
        navDIT.Attributes.Remove("Class")
        navDIT.Attributes.Add("Class", strClassnavDITtabcontent)

        Dim strClassnavOCtab As String = navOCtab.Attributes("Class").Replace("active", "")
        navOCtab.Attributes.Remove("Class")
        navOCtab.Attributes.Add("Class", strClassnavOCtab)

        Dim strClassnavOCtabContent As String = navOC.Attributes("Class").Replace("show active", "")
        navOC.Attributes.Remove("Class")
        navOC.Attributes.Add("Class", strClassnavOCtabContent)

        Dim strClassnavCLEAREDtab As String = navCLEAREDtab.Attributes("Class")
        navCLEAREDtab.Attributes.Remove("Class")
        navCLEAREDtab.Attributes.Add("Class", strClassnavCLEAREDtab & " active")

        Dim strClassnavCLEAREDtabContent As String = navCLEARED.Attributes("Class")
        navCLEARED.Attributes.Remove("Class")
        navCLEARED.Attributes.Add("Class", strClassnavCLEAREDtabContent & " show active")
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableControl(True)

        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
    End Sub

    Private Sub BankRecon_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
            Session("CopyFromID") = ""
            Session("CopyID") = ""
            Session("Type") = ""
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            Dim variance As Decimal
            If IsNumeric(txtVariance.Text) Then
                variance = txtVariance.Text
            Else
                variance = 0
            End If
            If variance <> 0 Then
                Response.Write("<script>alert('You cant save bank recon. if there is a variance.');</script>")
                Exit Sub
            End If
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    BRNO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    BRNO = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = BRNO
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    BRNO = txtTrans_Num.Text
                    Update()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    BRNO = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        BRNO = txtTrans_Num.Text
                        Update()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        BRNO = txtTrans_Num.Text
                        LoadTransaction(Session("TransID"))
                    Else
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " already exist!');</script>")
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblBR WHERE BR_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Save()
        Dim BankID = GetBankID(ddlBank.SelectedValue)
        Dim insertSQL As String
        activityStatus = True
        insertSQL = " INSERT INTO " &
                        " tblBR  (TransID, BR_No, BranchCode, BusinessCode, TransDate, BankID, BookEndBal, BookAdjBal, BankEndBal, BankAdjBal, " &
                        "         OC, DIT, Status, DateCreated, WhoCreated) " &
                        " VALUES (@TransID, @BR_No, @BranchCode, @BusinessCode, @TransDate,  @BankID, @BookEndBal, @BookAdjBal, @BankEndBal, @BankAdjBal, " &
                        "         @OC, @DIT, @Status, GETDATE(), @WhoCreated) "
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@BR_No", BRNO)
        SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@BusinessCode", "")
        SQL.AddParam("@TransDate", dtpDoc_Date.Text)
        SQL.AddParam("@BankID", BankID)
        If IsNumeric(txtBookBalance.Text) Then SQL.AddParam("@BookEndBal", CDec(txtBookBalance.Text)) Else SQL.AddParam("@BookEndBal", 0)
        If IsNumeric(txtAdjustedBookBalance.Text) Then SQL.AddParam("@BookAdjBal", CDec(txtAdjustedBookBalance.Text)) Else SQL.AddParam("@BookAdjBal", 0)
        If IsNumeric(txtBankBalance.Text) Then SQL.AddParam("@BankEndBal", CDec(txtBankBalance.Text)) Else SQL.AddParam("@BankEndBal", 0)
        If IsNumeric(txtAdjustedBankBalance.Text) Then SQL.AddParam("@BankAdjBal", CDec(txtAdjustedBankBalance.Text)) Else SQL.AddParam("@BankAdjBal", 0)
        If IsNumeric(txtOC.Text) Then SQL.AddParam("@OC", CDec(txtOC.Text)) Else SQL.AddParam("@OC", 0)
        If IsNumeric(txtDIT.Text) Then SQL.AddParam("@DIT", CDec(txtDIT.Text)) Else SQL.AddParam("@DIT", 0)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.ExecNonQuery(insertSQL)
    End Sub

    Public Sub Update()
        Dim BankID = GetBankID(ddlBank.SelectedValue)
        Dim updateSQL As String
        updateSQL = " UPDATE    tblBR  " &
                    " SET       TransID = @TransID, BR_No = @BR_No, BranchCode = @BranchCode, BusinessCode = @BusinessCode, TransDate = @TransDate, " &
                    "           BankID = @BankID, BookEndBal = @BookEndBal, BookAdjBal = @BookAdjBal, BankEndBal = @BankEndBal, BankAdjBal = @BankAdjBal, " &
                    "           OC = @OC, DIT = @DIT, DateModified = GETDATE(), WhoModified = @WhoModified " &
                    " WHERE     TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@BR_No", BRNO)
        SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@BusinessCode", "")
        SQL.AddParam("@TransDate", dtpDoc_Date.Text)
        SQL.AddParam("@BankID", BankID)
        If IsNumeric(txtBookBalance.Text) Then SQL.AddParam("@BookEndBal", CDec(txtBookBalance.Text)) Else SQL.AddParam("@BookEndBal", 0)
        If IsNumeric(txtAdjustedBookBalance.Text) Then SQL.AddParam("@BookAdjBal", CDec(txtAdjustedBookBalance.Text)) Else SQL.AddParam("@BookAdjBal", 0)
        If IsNumeric(txtBankBalance.Text) Then SQL.AddParam("@BankEndBal", CDec(txtBankBalance.Text)) Else SQL.AddParam("@BankEndBal", 0)
        If IsNumeric(txtAdjustedBankBalance.Text) Then SQL.AddParam("@BankAdjBal", CDec(txtAdjustedBankBalance.Text)) Else SQL.AddParam("@BankAdjBal", 0)
        If IsNumeric(txtOC.Text) Then SQL.AddParam("@OC", CDec(txtOC.Text)) Else SQL.AddParam("@OC", 0)
        If IsNumeric(txtDIT.Text) Then SQL.AddParam("@DIT", CDec(txtDIT.Text)) Else SQL.AddParam("@DIT", 0)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)
    End Sub

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT   TransID, BR_No, TransDate, BankID, BookEndBal, BookAdjBal, BankEndBal, BankAdjBal, OC, DIT, Status" &
                " FROM     tblBR " &
                " WHERE    TransId = @TransId " &
                " ORDER BY TransId "
        SQL.FlushParams()
        SQL.AddParam("@TransId", ID)
        SQL.ReadQuery(query)

        If SQL.SQLDR.Read Then
            disableEvent = True
            dtpDoc_Date.Attributes.Add("readonly", "readonly")
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            BRNO = SQL.SQLDR("BR_No").ToString
            Session("Transno") = SQL.SQLDR("BR_No").ToString
            txtTrans_Num.Text = BRNO
            txtStatus.Text = SQL.SQLDR("Status").ToString
            dtpDoc_Date.Text = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            txtBookBalance.Text = CDec(SQL.SQLDR("BookEndBal").ToString).ToString("N2")
            txtAdjustedBookBalance.Text = CDec(SQL.SQLDR("BookAdjBal").ToString).ToString("N2")
            txtBankBalance.Text = CDec(SQL.SQLDR("BankEndBal").ToString).ToString("N2")
            txtAdjustedBankBalance.Text = CDec(SQL.SQLDR("BankAdjBal").ToString).ToString("N2")
            txtOC.Text = CDec(SQL.SQLDR("OC").ToString).ToString("N2")
            txtDIT.Text = CDec(SQL.SQLDR("DIT").ToString).ToString("N2")
            disableEvent = False
            ddlBank.SelectedValue = GetBank(SQL.SQLDR("BankID").ToString)
            LoadBankDetail(ddlBank.SelectedValue)

            If txtStatus.Text = "Cancelled" Or txtStatus.Text = "Closed" Or txtStatus.Text = "Posted" Then
                btnEdit.Attributes("disabled") = "disabled"
                btnCancel.Attributes("disabled") = "disabled"
            Else
                btnEdit.Attributes.Remove("disabled")
                btnCancel.Attributes.Remove("disabled")
            End If

            btnClose.Attributes("disabled") = "disabled"
            btnSave.Attributes("disabled") = "disabled"
            btnNew.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
            btnSearch.Attributes.Remove("disabled")
            btnPreview.Attributes.Remove("disabled")

            EnableControl(False)

            LoadDIT(dtpDoc_Date.Text, txtAccountCode.Text)
            LoadOC(dtpDoc_Date.Text, txtAccountCode.Text)
            LoadCleared(dtpDoc_Date.Text, txtAccountCode.Text)
            LoadAdjustedBookBalance()
            LoadAdjustedBankBalance()
            ComputeVariance()
        Else
            Initialize()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If BRNO = "" Then
            Initialize()
            EnableControl(False)
            btnEdit.Attributes("disabled") = "disabled"
            btnCancel.Attributes("disabled") = "disabled"
            btnPrev.Attributes("disabled") = "disabled"
            btnNext.Attributes("disabled") = "disabled"
            btnPreview.Attributes("disabled") = "disabled"
        Else
            Initialize()
            LoadTransaction(TransID)
            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
            btnPreview.Attributes.Remove("disabled")
        End If
        btnSearch.Attributes.Remove("disabled")
        btnNew.Attributes.Remove("disabled")
        btnSave.Attributes("disabled") = "disabled"
        btnClose.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblBR  WHERE BR_No > '" & Session("Transno") & "' ORDER BY BR_No ASC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the end of record!');</script>")
            End If
        End If
    End Sub

    Private Sub btnPrev_Click(sender As Object, e As EventArgs) Handles btnPrev.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblBR  WHERE BR_No < '" & Session("Transno") & "' ORDER BY BR_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub

    Private Sub ddlBank_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBank.SelectedIndexChanged
        If disableEvent = False Then
            If Edit = False Then
                If ddlBank.SelectedIndex <> -1 Then
                    LoadBankDetail(ddlBank.SelectedValue)
                    dtpDoc_Date.Text = CDate(GetMinDate(GetBankID(ddlBank.SelectedValue))).ToString("yyyy-MM-dd")
                    If CDate(dtpDoc_Date.Text) <= Date.Today.Date Then
                        disableEvent = True
                        dtpDoc_Date.Text = CDate(Date.Today.Date).ToString("yyyy-MM-dd")
                        disableEvent = False
                    End If
                    RefreshData()
                End If
            Else
                If ddlBank.SelectedIndex <> -1 Then
                    LoadBankDetail(ddlBank.SelectedValue)
                    RefreshData()
                End If
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim query As String
        query = " UPDATE  " & DBTable & " SET Status ='Cancelled' WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID"))
        SQL.ExecNonQuery(query)

        LoadTransaction(Session("TransID"))
    End Sub
End Class