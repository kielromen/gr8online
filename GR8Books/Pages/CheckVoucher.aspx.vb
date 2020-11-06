Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class CheckVoucher
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim CVNo As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "CV"
    Dim ColumnPK As String = "CV_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblDV"
    Dim bankID As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                If Session("ID") <> "" Then
                    LoadTransaction(Session("ID"))
                ElseIf Session("Type") <> "" And Session("CopyID") <> "" Then
                    For Each item In Session("CopyFromID")
                        LoadCopyFrom(item.Key, Session("Type"))
                    Next
                Else
                    btnSearch.Attributes.Remove("disabled")
                    btnNew.Attributes.Remove("disabled")
                    btnEdit.Attributes("disabled") = "disabled"
                    btnSave.Attributes("disabled") = "disabled"
                    btnCopyFrom.Attributes("disabled") = "disabled"
                    btnCancel.Attributes("disabled") = "disabled"
                    btnPrev.Attributes("disabled") = "disabled"
                    btnNext.Attributes("disabled") = "disabled"
                    btnClose.Attributes("disabled") = "disabled"
                    btnPreview.Attributes("disabled") = "disabled"
                    EnableControl(False)
                    Session("TransID") = ""
                    dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                    dtpBank_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                End If
            End If
        End If
    End Sub

    Public Sub LoadDatagrid()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("Debit"))
        dt.Columns.Add(New DataColumn("Credit"))
        dt.Columns.Add(New DataColumn("Particulars"))
        dt.Columns.Add(New DataColumn("Code"))
        dt.Columns.Add(New DataColumn("Name"))
        dt.Columns.Add(New DataColumn("CostCenter"))
        dt.Columns.Add(New DataColumn("RefID"))
        dt.Columns.Add(New DataColumn("VATType"))
        Dim dr As DataRow = dt.NewRow

        dr("chNo") = 1
        dr("AccntCode") = Nothing
        dr("AccntTitle") = Nothing
        dr("Particulars") = Nothing
        dr("Debit") = "0.00"
        dr("Credit") = "0.00"
        dr("Code") = Nothing
        dr("Name") = Nothing
        dr("CostCenter") = Nothing
        dr("RefID") = Nothing
        dr("VATType") = Nothing
        dt.Rows.Add(dr)

        ViewState("EntryTable") = dt

        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        SetDataTable()
    End Sub

    Protected Sub AddNewRow(sender As Object, e As EventArgs)
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")


                    dr = dt.NewRow
                    dr("chNo") = i + 2
                    dr("Debit") = "0.00"
                    dr("Credit") = "0.00"

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("Debit") = txtDebit.Text
                    dt.Rows(i)("Credit") = txtCredit.Text
                    dt.Rows(i)("Particulars") = txtParticulars.Text
                    dt.Rows(i)("Code") = txtCode.Text
                    dt.Rows(i)("Name") = txtName.Text

                    dt.Rows(i)("CostCenter") = ddlCostCenter.SelectedValue
                    dt.Rows(i)("RefID") = txtRefID.Text


                    rowIndex = i
                Next
                dt.Rows.Add(dr)
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "scrollDown", "setTimeout(function () { window.scrollTo(0,document.body.scrollHeight); }, 25);", True)
        End If
    End Sub

    Private Sub SetDataTable()
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")
                    Dim txtVATType As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATTYpe")

                    ddlCostCenter.Items.Clear()
                    ddlCostCenter.Items.Add("")
                    ddlCostCenter.DataSource = LoadCostCenter().ToArray
                    ddlCostCenter.DataBind()

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtAccntCode.Text = dt.Rows(i)("AccntCode").ToString
                    txtAccntTitle.Text = dt.Rows(i)("AccntTitle").ToString
                    txtDebit.Text = If(IsNumeric(dt.Rows(i)("Debit").ToString), CDec(dt.Rows(i)("Debit").ToString).ToString("N2"), "0.00")
                    txtCredit.Text = If(IsNumeric(dt.Rows(i)("Credit").ToString), CDec(dt.Rows(i)("Credit").ToString).ToString("N2"), "0.00")
                    txtParticulars.Text = dt.Rows(i)("Particulars").ToString
                    txtCode.Text = dt.Rows(i)("Code").ToString
                    txtName.Text = dt.Rows(i)("Name").ToString
                    ddlCostCenter.SelectedValue = dt.Rows(i)("CostCenter").ToString
                    txtRefID.Text = dt.Rows(i)("RefID").ToString
                    txtVATType.Text = dt.Rows(i)("VATType").ToString
                Next
            End If
        End If
    End Sub

    Private Sub dgvEntry_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvEntry.RowDeleting
        Dim rowIndex As Integer = e.RowIndex
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("Particulars") = txtParticulars.Text
                    dt.Rows(i)("Debit") = txtDebit.Text
                    dt.Rows(i)("Credit") = txtCredit.Text
                    dt.Rows(i)("Code") = txtCode.Text
                    dt.Rows(i)("Name") = txtName.Text
                    dt.Rows(i)("CostCenter") = ddlCostCenter.SelectedValue
                    dt.Rows(i)("RefID") = txtRefID.Text

                Next

                dt.Rows.RemoveAt(rowIndex)
                If dt.Rows.Count = 0 Then
                    dt.Rows.Add("", "", "", "0.00", "0.00")
                End If
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
        End If
    End Sub

    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE '%' + @AccountTitle + '%' AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            AccountTitle.Add(String.Format("{0}--{1}", SQL.SQLDR("AccountTitle"), SQL.SQLDR("AccountCode")))
        End While
        Return AccountTitle.ToArray()
    End Function

    <WebMethod()>
    Public Shared Function ListVCE(prefix As String) As String()
        Dim strName As New List(Of String)()
        Dim query As String
        query = "SELECT Code, Name FROM View_VCEMMaster " & vbCrLf &
                "WHERE Status = 'Active' AND Name LIKE '%' + @Name + '%' AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Name", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            strName.Add(String.Format("{0}--{1}", SQL.SQLDR("Name"), SQL.SQLDR("Code")))
        End While
        Return strName.ToArray()
    End Function

    Public Sub WithBank()
        Dim query As String
        query = " SELECT WithBank, tblDV_PaymentType. AccountCode, AccountTitle" & vbCrLf &
                " FROM   tblDV_PaymentType " & vbCrLf &
                " LEFT JOIN  tblCOA ON tblCOA.AccountCode = tblDV_PaymentType.AccountCode " & vbCrLf &
                " WHERE  tblDV_PaymentType.Status ='Active' AND PaymentType = @PaymentType"
        SQL.FlushParams()
        SQL.AddParam("@PaymentType", ddlType.SelectedValue)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            If ddlType.SelectedValue.ToString.Contains("Transfer") Then
                panelBankTransfer.Visible = True
                panelBank.Visible = True
                panelBankDetails.Visible = False
                lblBank.Text = "From Bank :"
                BankDetails()
            ElseIf SQL.SQLDR("WithBank") = False Then
                Session("AccountCode") = SQL.SQLDR("AccountCode")
                Session("AccountTitle") = SQL.SQLDR("AccountTitle")
                panelBankTransfer.Visible = False
                panelBank.Visible = False
                panelBankDetails.Visible = False
                lblBank.Text = "Bank :"
            Else
                panelBankTransfer.Visible = False
                panelBank.Visible = True
                panelBankDetails.Visible = True
                lblBank.Text = "Bank :"
                BankDetails()
            End If
        Else
            panelBankTransfer.Visible = False
            panelBank.Visible = False
            panelBankDetails.Visible = False
        End If
    End Sub


    Public Sub BankDetails()
        Dim query As String
        query = " SELECT tblBank.AccountCode, AccountTitle" & vbCrLf &
                " FROM   tblBank " & vbCrLf &
                " INNER JOIN tblCOA ON tblBank.AccountCode = tblCOA.AccountCode " & vbCrLf &
                " WHERE  tblBank.Status ='Active' AND Bank = @Bank"
        SQL.FlushParams()
        SQL.AddParam("Bank", ddlBank_List.SelectedValue)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Session("AccountCode") = SQL.SQLDR("AccountCode")
            Session("AccountTitle") = SQL.SQLDR("AccountTitle")
        Else
            Session("AccountCode") = ""
            Session("AccountTitle") = ""
        End If
    End Sub

    Public Sub Initialize()
        Session("AccountCode") = ""
        Session("AccountTitle") = ""
        Session("Transno") = ""
        Session("TotalDebit") = 0
        Session("TotalCredit") = 0
        txtRow.Style.Add("display", "none")
        'txtRef_Type.Attributes.Add("readonly", "readonly")
        'txtRef_No.Attributes.Add("readonly", "readonly")
        txtBank_CheckStatus.Attributes.Add("readonly", "readonly")
        txtStatus.Attributes.Add("readonly", "readonly")
        txtCode.Attributes.Add("readonly", "readonly")
        txtCode.Text = ""
        txtName.Text = ""
        txtAmount.Text = "0.00"
        txtRemarks.Text = ""
        txtStatus.Text = ""
        'txtRef_Type.Text = ""
        'txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        txtBank_CheckName.Text = ""
        txtBank_CheckNo.Text = ""
        txtBank_CheckStatus.Text = ""
        dtpDoc_Date.Value = Now.Date
        dtpBank_Date.Value = Now.Date


        LoadDatagrid()

        ddlType.Items.Clear()
        ddlType.Items.Add("--Select Payment Type--")
        ddlType.DataSource = LoadDisbursementPaymentType().ToArray
        ddlType.DataBind()

        ddlBank_List.Items.Clear()
        ddlBank_List.Items.Add("--Select Bank--")
        ddlBank_List.DataSource = LoadBank().ToArray
        ddlBank_List.DataBind()

        WithBank()

        ddlDisbursementType.Items.Clear()
        ddlDisbursementType.Items.Add("--Select Disbursement Type--")
        ddlDisbursementType.DataSource = LoadDisbursementType().ToArray
        ddlDisbursementType.DataBind()

        'tax
        txtTNetAmount.Text = "0.00"
        txtETaxAmount.Text = "0.00"
        txtTTaxAmount.Text = "0.00"
        txtTTotalAmount.Text = "0.00"
        txtTAmount.Text = "0.00"

        txtTNetAmount.Attributes.Add("readonly", "readonly")
        txtETaxAmount.Attributes.Add("readonly", "readonly")
        txtTTaxAmount.Attributes.Add("readonly", "readonly")
        txtTTotalAmount.Attributes.Add("readonly", "readonly")
        txtTAmount.Attributes.Add("readonly", "readonly")
        txtTPercent.Attributes.Add("readonly", "readonly")
        txtEPercent.Attributes.Add("readonly", "readonly")

        ddlTaxType.Items.Clear()
        ddlTaxType.Items.Add("--Select VAT Rate--")
        ddlTaxType.DataSource = LoadTaxCode("Purchases", "INPUT VAT").ToArray
        ddlTaxType.DataBind()

        ddlETaxType.Items.Clear()
        ddlETaxType.Items.Add("--Select EWT Rate--")
        ddlETaxType.DataSource = LoadTaxCode("Purchases", "EWT").ToArray
        ddlETaxType.DataBind()
        'tax

    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        panelEntry.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        dtpBank_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub CheckVoucher_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
            Session("CopyFromID") = ""
            Session("CopyID") = ""
            Session("Type") = ""
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        CVNo = ""
        Session("TransID") = ""
        txtStatus.Text = "Open"
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCopyFrom.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        dtpBank_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            Dim count As Integer = 0
            For Each rows As GridViewRow In dgvEntry.Rows
                Dim AccntCode As TextBox
                AccntCode = rows.FindControl("txtAccntCode_Entry")
                If AccntCode.Text = "" Then
                    If dgvEntry.Rows.Count - 1 <> count And AccntCode.Text = "" Then
                        Response.Write("<script>alert('Invalid Account Code!');</script>")
                        Exit Sub
                    End If
                End If
                count = count + 1
            Next
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    CVNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    CVNo = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = CVNo
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    CVNo = txtTrans_Num.Text
                    Update()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    CVNo = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        CVNo = txtTrans_Num.Text
                        Update()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        CVNo = txtTrans_Num.Text
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
        query = " SELECT * FROM tblDV WHERE CV_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Save()
        Dim insertSQL As String
        activityStatus = True
        SQL.FlushParams()
        insertSQL = " INSERT INTO " &
                        " tblDV (TransID, CV_No, PaymentType, VCECode, TransDate, TotalAmount, Remarks, WhoCreated, TransAuto ) " &
                        " VALUES (@TransID, @CV_No, @PaymentType, @VCECode, @TransDate, @TotalAmount, @Remarks,  @WhoCreated, @TransAuto)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@CV_No", CVNo)
        SQL.AddParam("@PaymentType", ddlType.SelectedValue)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        SaveCVRef(TransID)

        insertSQL = " INSERT INTO " &
                        " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
                        " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@AppDate", dtpDoc_Date.Value)
        SQL.AddParam("@RefType", "CV")
        SQL.AddParam("@RefTransID", TransID)
        SQL.AddParam("@Book", "Cash Disbursement")
        SQL.AddParam("@TotalDBCR", "0.00")
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        JETransiD = LoadJE("CV", TransID)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtCredit_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtParticulars_Entry")
                    Dim txtEntryCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")
                    Dim txtVATType As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATType")

                    Dim CostID = GetCostCenterID(ddlCostCenter.SelectedValue)

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, CostCenter, RefNo, VatType,  LineNumber, Status) " &
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @CostCenter, @RefNo, @VatType, @LineNumber, @Status)"
                        SQL.FlushParams()
                        SQL.AddParam("@JE_No", JETransiD)
                        SQL.AddParam("@AccntCode", txtAccntCode.Text)
                        If txtEntryCode.Text <> Nothing AndAlso txtEntryCode.Text <> "" Then
                            SQL.AddParam("@VCECode", txtEntryCode.Text)
                        Else
                            SQL.AddParam("@VCECode", txtCode.Text)
                        End If
                        If txtDebit.Text <> Nothing AndAlso IsNumeric(txtDebit.Text) Then
                            SQL.AddParam("@Debit", CDec(txtDebit.Text))
                        Else
                            SQL.AddParam("@Debit", 0)
                        End If
                        If txtCredit.Text <> Nothing AndAlso IsNumeric(txtCredit.Text) Then
                            SQL.AddParam("@Credit", CDec(txtCredit.Text))
                        Else
                            SQL.AddParam("@Credit", 0)
                        End If
                        If txtParticulars.Text <> Nothing AndAlso txtParticulars.Text <> "" Then
                            SQL.AddParam("@Particulars", txtParticulars.Text)
                        Else
                            SQL.AddParam("@Particulars", txtRemarks.Text)
                        End If
                        If ddlCostCenter.SelectedValue <> Nothing AndAlso ddlCostCenter.SelectedValue <> "" Then
                            SQL.AddParam("@CostCenter", CostID)
                        Else
                            SQL.AddParam("@CostCenter", "")
                        End If
                        If txtRefID.Text <> Nothing AndAlso txtRefID.Text <> "" Then
                            SQL.AddParam("@RefNo", txtRefID.Text)
                            If strRefNo.Length = 0 Then
                                strRefNo = txtRefID.Text
                            Else
                                strRefNo = strRefNo & "-" & txtRefID.Text
                            End If
                        Else
                            SQL.AddParam("@RefNo", "")
                        End If
                        If txtVATType.Text <> Nothing AndAlso txtVATType.Text <> "" Then
                            SQL.AddParam("@VATType", txtVATType.Text)
                        Else
                            SQL.AddParam("@VATType", "")
                        End If
                        SQL.AddParam("@LineNumber", line)
                        SQL.AddParam("@Status", "Active")
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If
    End Sub


    Private Sub SaveCVRef(ByVal CVNo As Integer)
        Dim deleteSQL As String
        deleteSQL = "DELETE FROM tblDV_Check_BankRef WHERE CV_No ='" & CVNo & "'"
        SQL.ExecNonQuery(deleteSQL)
        Dim insertSQL As String
        Dim BankID As String = GetBankID(ddlBank_List.SelectedValue)
        insertSQL = " INSERT INTO " &
                    " tblDV_Check_BankRef (CV_No, BankID, RefNo, RefDate, RefAmount, RefName, TransferTo, TransferAccountNo) " &
                    " VALUES(@CV_No, @BankID, @RefNo, @RefDate, @RefAmount, @RefName, @TransferTo, @TransferAccountNo)"
        SQL.FlushParams()
        SQL.AddParam("@CV_No", CVNo)
        SQL.AddParam("@BankID", BankID)
        SQL.AddParam("@RefNo", txtBank_CheckNo.Text)
        SQL.AddParam("@RefDate", dtpBank_Date.Value)
        SQL.AddParam("@RefAmount", CDec(txtAmount.Text))
        SQL.AddParam("@RefName", IIf(txtBank_CheckName.Text = "", txtName.Text, txtBank_CheckName.Text))
        SQL.AddParam("@TransferTo", txtTransferTo.Text)
        SQL.AddParam("@TransferAccountNo", txtAccountNo.Text)
        SQL.ExecNonQuery(insertSQL)
    End Sub

    Private Sub LoadCVRef(ByVal CVNo As Integer)
        Dim query As String
        query = " SELECT Bank, " &
                " 	     RefNo, RefDate, RefAmount, tblDV_Check_BankRef.Status, RefName, TransferAccountNo, TransferTo" &
                " FROM   tblDV_Check_BankRef INNER JOIN tblBank " &
                " ON     tblDV_Check_BankRef.BankID = tblBank.ID " &
                " WHERE  CV_No ='" & CVNo & "' AND tblDV_Check_BankRef.Status <> 'Cancelled' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtBank_CheckNo.Text = SQL.SQLDR("RefNo").ToString
            dtpBank_Date.Value = CDate(SQL.SQLDR("RefDate")).ToString("yyyy-MM-dd")
            ddlBank_List.SelectedValue = SQL.SQLDR("Bank").ToString
            txtBank_CheckStatus.Text = SQL.SQLDR("Status").ToString
            txtBank_CheckName.Text = SQL.SQLDR("RefName").ToString
            ddlBank_List.SelectedValue = SQL.SQLDR("Bank").ToString
            txtAccountNo.Text = SQL.SQLDR("TransferAccountNo").ToString
            txtTransferTo.Text = SQL.SQLDR("TransferTo").ToString
            bankID = GetBankID(ddlBank_List.SelectedValue)
        End If
    End Sub


    Private Sub Update()
        Dim insertSQL, updateSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblDV  " &
                        " SET    CV_No = @CV_No, PaymentType = @PaymentType, VCECode = @VCECode, TransDate = @TransDate, " &
                        "        TotalAmount = @TotalAmount, Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@CV_No", CVNo)
        SQL.AddParam("@PaymentType", ddlType.SelectedValue)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@TotalAmount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        SaveCVRef(Session("TransID").ToString)

        JETransiD = LoadJE("CV", Session("TransID"))
        If JETransiD = 0 Then
            insertSQL = " INSERT INTO " &
                        " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
                        " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDoc_Date.Value)
            SQL.AddParam("@RefType", "CV")
            SQL.AddParam("@RefTransID", Session("TransID"))
            SQL.AddParam("@Book", "Cash Disbursement")
            SQL.AddParam("@TotalDBCR", "0.00")
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoCreated", Session("EmailAddress"))
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("CV", Session("TransID"))
        Else
            updateSQL = " UPDATE tblJE_Header " &
                           " SET    AppDate = @AppDate,  " &
                           "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " &
                           "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " &
                           " WHERE  JE_No = @JE_No "
            SQL.FlushParams()
            SQL.AddParam("@JE_No", JETransiD)
            SQL.AddParam("@AppDate", dtpDoc_Date.Value)
            SQL.AddParam("@RefType", "CV")
            SQL.AddParam("@RefTransID", Session("TransID").ToString)
            SQL.AddParam("@Book", "Cash Disbursement")
            SQL.AddParam("@TotalDBCR", "0.00")
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoModified", Session("EmailAddress"))
            SQL.ExecNonQuery(updateSQL)
        End If

        updateSQL = " UPDATE tblJE_Details SET Status = 'Inactive' WHERE JE_No = @JE_No "
        SQL.FlushParams()
        SQL.AddParam("@JE_No", JETransiD)
        SQL.ExecNonQuery(updateSQL)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtEntryCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")
                    Dim txtVATType As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATType")

                    Dim CostID = GetCostCenterID(ddlCostCenter.SelectedValue)

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, CostCenter, RefNo, VatType, LineNumber, Status) " &
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @CostCenter, @RefNo, @VatType, @LineNumber, @Status)"
                        SQL.FlushParams()
                        SQL.AddParam("@JE_No", JETransiD)
                        SQL.AddParam("@AccntCode", txtAccntCode.Text)
                        If txtEntryCode.Text <> Nothing AndAlso txtEntryCode.Text <> "" Then
                            SQL.AddParam("@VCECode", txtEntryCode.Text)
                        Else
                            SQL.AddParam("@VCECode", txtCode.Text)
                        End If
                        If txtDebit.Text <> Nothing AndAlso IsNumeric(txtDebit.Text) Then
                            SQL.AddParam("@Debit", CDec(txtDebit.Text))
                        Else
                            SQL.AddParam("@Debit", 0)
                        End If
                        If txtCredit.Text <> Nothing AndAlso IsNumeric(txtCredit.Text) Then
                            SQL.AddParam("@Credit", CDec(txtCredit.Text))
                        Else
                            SQL.AddParam("@Credit", 0)
                        End If
                        If txtParticulars.Text <> Nothing AndAlso txtParticulars.Text <> "" Then
                            SQL.AddParam("@Particulars", txtParticulars.Text)
                        Else
                            SQL.AddParam("@Particulars", txtRemarks.Text)
                        End If
                        If ddlCostCenter.SelectedValue <> Nothing AndAlso ddlCostCenter.SelectedValue <> "" Then
                            SQL.AddParam("@CostCenter", CostID)
                        Else
                            SQL.AddParam("@CostCenter", "")
                        End If
                        If txtRefID.Text <> Nothing AndAlso txtRefID.Text <> "" Then
                            SQL.AddParam("@RefNo", txtRefID.Text)
                            If strRefNo.Length = 0 Then
                                strRefNo = txtRefID.Text
                            Else
                                strRefNo = strRefNo & "-" & txtRefID.Text
                            End If
                        Else
                            SQL.AddParam("@RefNo", "")
                        End If
                        If txtVATType.Text <> Nothing AndAlso txtVATType.Text <> "" Then
                            SQL.AddParam("@VATType", txtVATType.Text)
                        Else
                            SQL.AddParam("@VATType", "")
                        End If
                        SQL.AddParam("@LineNumber", line)
                        SQL.AddParam("@Status", "Active")
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query, PaymentType As String
        query = " SELECT  TransID, CV_No, PaymentType, tblDV.VCECode, Name, TransDate, TotalAmount, Remarks, " &
                "         ISNULL(APV_Ref,0) as APV_Ref, OR_Ref, ISNULL(LN_Ref,0) as LN_Ref, tblDV.Status " &
                " FROM    tblDV LEFT JOIN View_VCEMMaster " &
                " ON      tblDV.VCECode = View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            CVNo = SQL.SQLDR("CV_No").ToString
            Session("Transno") = SQL.SQLDR("CV_No").ToString
            txtTrans_Num.Text = CVNo
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("Name").ToString
            txtAmount.Text = CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
            dtpDoc_Date.Value = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            PaymentType = SQL.SQLDR("PaymentType").ToString
            ddlType.Text = PaymentType

            WithBank()

            LoadCVRef(TransID)
            LoadEntry(TransID)

            If txtStatus.Text = "Cancelled" Or txtStatus.Text = "Posted" Then
                btnEdit.Attributes("disabled") = "disabled"
                btnCancel.Attributes("disabled") = "disabled"
            Else
                btnEdit.Attributes.Remove("disabled")
                btnCancel.Attributes.Remove("disabled")
            End If


            btnClose.Attributes("disabled") = "disabled"
            btnSave.Attributes("disabled") = "disabled"
            btnCopyFrom.Attributes("disabled") = "disabled"
            btnNew.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
            btnSearch.Attributes.Remove("disabled")
            btnPreview.Attributes.Remove("disabled")
            EnableControl(False)
        Else
            Initialize()
        End If
    End Sub

    Private Sub LoadEntry(ByVal CVNo As Integer)
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("Particulars"))
        dt.Columns.Add(New DataColumn("Debit"))
        dt.Columns.Add(New DataColumn("Credit"))
        dt.Columns.Add(New DataColumn("Code"))
        dt.Columns.Add(New DataColumn("Name"))
        dt.Columns.Add(New DataColumn("CostCenter"))
        dt.Columns.Add(New DataColumn("RefID"))
        dt.Columns.Add(New DataColumn("VATType"))
        ViewState("EntryTable") = dt
        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = " SELECT ID, JE_No, View_GL.BranchCode, View_GL.AccntCode, AccountTitle, View_GL.VCECode, View_GL.VCEName, Debit, Credit, Particulars, CostID, RefNo , VatType  " &
                " FROM   View_GL INNER JOIN tblCOA " &
                " ON     View_GL.AccntCode = tblCOA.AccountCode " &
                " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE RefType = 'CV' AND RefTransID = " & CVNo & ") " &
                " ORDER BY LineNumber "
        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("AccntCode") = SQL.SQLDR("AccntCode").ToString
            dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
            dr("Particulars") = SQL.SQLDR("Particulars").ToString
            dr("Debit") = CDec(SQL.SQLDR("Debit")).ToString("N2")
            dr("Credit") = CDec(SQL.SQLDR("Credit")).ToString("N2")
            dr("Code") = SQL.SQLDR("VCECode").ToString
            dr("Name") = SQL.SQLDR("VCEName").ToString
            dr("CostCenter") = GetCostCenter(SQL.SQLDR("CostID").ToString)
            dr("RefID") = SQL.SQLDR("RefNo").ToString
            dr("VATType") = SQL.SQLDR("VATType").ToString
            dt.Rows.Add(dr)

            ViewState("EntryTable") = data
            dgvEntry.DataSource = data

            ch = ch + 1
        End While

        dgvEntry.DataBind()
        SetDataTable()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If CVNo = "" Then
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
        btnCopyFrom.Attributes("disabled") = "disabled"
        btnClose.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblDV  WHERE CV_No > '" & Session("Transno") & "' ORDER BY CV_No ASC "
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
            query = " SELECT Top 1 TransID FROM tblDV  WHERE CV_No < '" & Session("Transno") & "' ORDER BY CV_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableControl(True)
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCopyFrom.Attributes.Remove("disabled")
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim query As String
        query = " UPDATE  " & DBTable & " SET Status ='Cancelled' WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID"))
        SQL.ExecNonQuery(query)

        query = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType = '" & ModuleID & "' "
        SQL.FlushParams()
        SQL.AddParam("@RefTransID", Session("TransID"))
        SQL.ExecNonQuery(query)

        LoadTransaction(Session("TransID"))
    End Sub

    Private Function GetCheckNo(ByVal Bank_ID As String) As String
        Dim CheckNo As String = ""
        If Bank_ID <> 0 Then
            Dim query As String
            query = " SELECT  RIGHT('000000000000' + CAST((CAST(MAX(RefNo) AS bigint) + 1) AS nvarchar), LEN(SeriesStart)) AS RefNo, ISNULL(SeriesStart,0) AS SeriesStart " &
                    " FROM    tblBank LEFT JOIN tblDV_Check_BankRef " &
                    " ON      tblDV_Check_BankRef.BankID = tblBank.ID " &
                    " AND     tblDV_Check_BankRef.RefNo BETWEEN SeriesStart AND SeriesEnd " &
                    " LEFT JOIN tblDV ON tblDV_Check_BankRef.CV_No = tblDV.TransID AND PaymentType = 'Check'" &
                    " WHERE   tblBank.ID = '" & Bank_ID & "'   " &
                    " GROUP BY LEN(SeriesStart), SeriesStart  "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read AndAlso Not IsDBNull(SQL.SQLDR("RefNo")) Then
                CheckNo = SQL.SQLDR("RefNo").ToString
            ElseIf Not IsDBNull(SQL.SQLDR("SeriesStart")) Then
                CheckNo = SQL.SQLDR("SeriesStart").ToString
            Else
                CheckNo = ""
            End If
        End If
        Return CheckNo
    End Function

    Public Sub ddlBankSelectChange()
        txtBank_CheckNo.Text = GetCheckNo(GetBankID(ddlBank_List.SelectedValue))
    End Sub

    Private Function GetBankID(ByVal Bank As String) As Integer
        Dim query As String
        query = " SELECT ID,tblBank.AccountCode, tblCOA.AccountTitle FROM tblBank INNER JOIN tblCOA ON tblBank.AccountCode = tblCOA.AccountCode WHERE Bank =  '" & Bank & "' "
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read Then
            Session("AccountCode") = SQL.SQLDR2("AccountCode")
            Session("AccountTitle") = SQL.SQLDR2("AccountTitle")
            Return SQL.SQLDR2("ID")
        Else
            Session("AccountCode") = ""
            Session("AccountTitle") = ""
            Return 0
        End If
    End Function

    Protected Sub ddlDisbursementType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim query, Collection_Type As String
        Collection_Type = ddlDisbursementType.Text
        query = " SELECT  tblDisbursement_Type.AccountCode, AccountTitle, Amount, Description  " &
                " FROM    tblDisbursement_Type INNER JOIN tblCOA " &
                " ON      tblDisbursement_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE   tblDisbursement_Type.Status = 'Active' AND Description = @Description "
        SQL.FlushParams()
        SQL.AddParam("@Description", Collection_Type)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Dim ch As Integer = 0
            Dim dt As DataTable = ViewState("EntryTable")

            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).FindControl("txtAccntCode_Entry")
                    If txtAccntCode.Text <> "" Then
                        ch = i + 1
                    End If
                Next
            End If
            If ch >= 1 Then
                Dim data As DataTable = ViewState("EntryTable")
                Dim drow As DataRow = dt.NewRow
                drow("chNo") = ch
                drow("AccntCode") = SQL.SQLDR("AccountCode").ToString
                drow("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                drow("Debit") = CDec(SQL.SQLDR("Amount")).ToString("N2")
                drow("Credit") = "0.00"
                drow("Particulars") = SQL.SQLDR("Description").ToString
                drow("Code") = ""
                drow("Name") = ""
                drow("RefID") = ""
                dt.Rows.Add(drow)

                ViewState("EntryTable") = data
                dgvEntry.DataSource = data
            Else
                dgvEntry.DataSource = Nothing
                dgvEntry.DataBind()
                Dim data As New DataTable
                data.Columns.Add(New DataColumn("chNo"))
                data.Columns.Add(New DataColumn("AccntCode"))
                data.Columns.Add(New DataColumn("AccntTitle"))
                data.Columns.Add(New DataColumn("Debit"))
                data.Columns.Add(New DataColumn("Credit"))
                data.Columns.Add(New DataColumn("Particulars"))
                data.Columns.Add(New DataColumn("Code"))
                data.Columns.Add(New DataColumn("Name"))
                data.Columns.Add(New DataColumn("RefID"))
                Dim dr As DataRow = data.NewRow
                dr("chNo") = ch
                dr("AccntCode") = SQL.SQLDR("AccountCode").ToString
                dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                dr("Debit") = CDec(SQL.SQLDR("Amount")).ToString("N2")
                dr("Credit") = "0.00"
                dr("Particulars") = SQL.SQLDR("Description").ToString
                dr("Code") = Nothing
                dr("Name") = Nothing
                dr("RefID") = Nothing
                data.Rows.Add(dr)

                ViewState("EntryTable") = data

                dgvEntry.DataSource = data
            End If
        End If
        dgvEntry.DataBind()
        SetDataTable()

        ddlDisbursementType.Items.Clear()
        ddlDisbursementType.Items.Add("--Select Disbursement Type--")
        ddlDisbursementType.DataSource = LoadDisbursementType().ToArray
        ddlDisbursementType.DataBind()
    End Sub


    Protected Sub ComputeRow(sender As Object, e As EventArgs)
        Page.Validate()
        If txtAmount.Text <> "" Then
            If Session("AccountCode") <> "" Then
                'AddNewRow(dgvEntry, EventArgs.Empty)
                txtAmount.Text = CDec(Session("TotalDebit") - Session("TotalCredit")).ToString("N2")
                Dim rowIndex As Integer = 0
                If Not IsNothing(ViewState("EntryTable")) Then
                    Dim dt As DataTable = ViewState("EntryTable")
                    Dim dr As DataRow = Nothing
                    If dt.Rows.Count > 0 Then
                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                            Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                            Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                            Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                            Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                            Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                            Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                            Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")
                            dr = dt.NewRow
                            dr("chNo") = i + 2
                            dr("AccntCode") = Session("AccountCode")
                            dr("AccntTitle") = Session("AccountTitle")
                            dr("Debit") = "0.00"
                            dr("Credit") = txtAmount.Text
                            dr("Particulars") = ""
                            dr("Code") = ""
                            dr("Name") = ""
                            dr("RefID") = ""

                            dt.Rows(i)("AccntCode") = txtAccntCode.Text
                            dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                            dt.Rows(i)("Debit") = txtDebit.Text
                            dt.Rows(i)("Credit") = txtCredit.Text
                            dt.Rows(i)("Particulars") = txtParticulars.Text
                            dt.Rows(i)("Code") = txtCode.Text
                            dt.Rows(i)("Name") = txtName.Text
                            dt.Rows(i)("RefID") = txtRefID.Text


                            rowIndex = i
                        Next
                        dt.Rows.Add(dr)
                        ViewState("EntryTable") = dt

                        dgvEntry.DataSource = dt
                        dgvEntry.DataBind()

                    End If
                    SetDataTable()
                End If
            End If
        End If
    End Sub

    'Copy From
    Private Sub LoadCopyFrom(ByVal CopyFromID As String, ByVal Type As String)
        CopyFromInitialize()
        Dim query As String
        Select Case Type
            Case "APV"
                query = " SELECT TransID, APV_No, VCECode, VCEName, Remarks" &
                        " FROM View_APV_Balance " &
                        " WHERE  TransID ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    txtCode.Text = SQL.SQLDR("VCECode").ToString
                    txtName.Text = SQL.SQLDR("VCEName").ToString
                    txtRemarks.Text = SQL.SQLDR("Remarks").ToString
                    LoadCopyFromEntry(CopyFromID, Type)
                End If
            Case "CA"
                query = " SELECT   TransID, CA_No AS TransNo, VCECode, VCEName, Remarks" &
                        " FROM     View_CA_Balance " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    txtCode.Text = SQL.SQLDR("VCECode").ToString
                    txtName.Text = SQL.SQLDR("VCEName").ToString
                    txtRemarks.Text = SQL.SQLDR("Remarks").ToString
                    LoadCopyFromEntry(CopyFromID, Type)
                End If
            Case "PC"
                query = " SELECT   TransID, PC_No AS TransNo, VCECode, VCEName, Remarks" &
                        " FROM     View_PC_Balance " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    txtCode.Text = SQL.SQLDR("VCECode").ToString
                    txtName.Text = SQL.SQLDR("VCEName").ToString
                    txtRemarks.Text = SQL.SQLDR("Remarks").ToString
                    LoadCopyFromEntry(CopyFromID, Type)
                End If
            Case "CASHL"
                query = " SELECT   TransID, CA_No AS TransNo, VCECode, VCEName, Remarks" &
                        " FROM     View_CA_Return " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    txtCode.Text = SQL.SQLDR("VCECode").ToString
                    txtName.Text = SQL.SQLDR("VCEName").ToString
                    txtRemarks.Text = SQL.SQLDR("Remarks").ToString
                    LoadCopyFromEntry(CopyFromID, Type)
                End If
        End Select

    End Sub

    Private Sub LoadCopyFromEntry(ByVal CopyFromID As String, ByVal Type As String)
        Dim query As String
        Select Case Type
            Case "APV"
                query = " SELECT TransID, APV_No AS TransNo, Date, VCECode, VCEName, Amount AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle, CostID, RefNo, VatType" &
                        " FROM View_APV_Balance " &
                        " WHERE  TransID ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
            Case "CA"
                query = " SELECT TransID, CA_No AS TransNo, Date, VCECode, VCEName, Amount AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle,  CostID, RefNo, VatType " &
                        " FROM  View_CA_Balance " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
            Case "PC"
                query = " SELECT TransID, PC_No AS TransNo, Date, VCECode, VCEName, Amount AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle,  CostID, RefNo, VatType " &
                        " FROM  View_PC_Balance " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
            Case "CASHL"
                query = " SELECT TransID, CA_No AS TransNo, Date, VCECode, VCEName, Amount * -1 AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle,  CostID, RefNo, VatType " &
                        " FROM  View_CA_Return " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
        End Select
        While SQL.SQLDR.Read
            txtAmount.Text = CDec(txtAmount.Text) + CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
            Dim x As Decimal = txtAmount.Text
            Dim ch As Integer = 0
            Dim dt As DataTable = ViewState("EntryTable")

            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).FindControl("txtAccntCode_Entry")
                    If txtAccntCode.Text <> "" Then
                        ch = i + 1
                    End If
                Next
            End If
            If ch >= 1 Then
                Dim data As DataTable = ViewState("EntryTable")
                Dim drow As DataRow = dt.NewRow
                drow("chNo") = ch
                drow("AccntCode") = SQL.SQLDR("AccountCode").ToString
                drow("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                drow("Debit") = CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
                drow("Credit") = "0.00"
                drow("Particulars") = SQL.SQLDR("Particulars").ToString
                drow("Code") = SQL.SQLDR("VCECode").ToString
                drow("Name") = SQL.SQLDR("VCEName").ToString
                drow("CostCenter") = GetCostCenter(SQL.SQLDR("CostID").ToString)
                drow("RefID") = SQL.SQLDR("RefNo").ToString
                drow("VatType") = SQL.SQLDR("VatType").ToString
                dt.Rows.Add(drow)

                ViewState("EntryTable") = data
                dgvEntry.DataSource = data
            Else
                dgvEntry.DataSource = Nothing
                dgvEntry.DataBind()
                Dim data As New DataTable
                data.Columns.Add(New DataColumn("chNo"))
                data.Columns.Add(New DataColumn("AccntCode"))
                data.Columns.Add(New DataColumn("AccntTitle"))
                data.Columns.Add(New DataColumn("Debit"))
                data.Columns.Add(New DataColumn("Credit"))
                data.Columns.Add(New DataColumn("Particulars"))
                data.Columns.Add(New DataColumn("Code"))
                data.Columns.Add(New DataColumn("Name"))
                data.Columns.Add(New DataColumn("CostCenter"))
                data.Columns.Add(New DataColumn("RefID"))
                data.Columns.Add(New DataColumn("VatType"))
                Dim dr As DataRow = data.NewRow
                dr("chNo") = ch
                dr("AccntCode") = SQL.SQLDR("AccountCode").ToString
                dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                dr("Debit") = CDec(SQL.SQLDR("TotalAmount")).ToString("N2")
                dr("Credit") = "0.00"
                dr("Particulars") = SQL.SQLDR("Particulars").ToString
                dr("Code") = SQL.SQLDR("VCECode").ToString
                dr("Name") = SQL.SQLDR("VCEName").ToString
                dr("CostCenter") = GetCostCenter(SQL.SQLDR("CostID").ToString)
                dr("RefID") = SQL.SQLDR("RefNo").ToString
                dr("VatType") = SQL.SQLDR("VatType").ToString
                data.Rows.Add(dr)

                ViewState("EntryTable") = data
                dgvEntry.DataSource = data
            End If

            dgvEntry.DataBind()
            SetDataTable()
        End While

    End Sub

    Public Sub CopyFromInitialize()
        TransID = ""
        CVNo = ""
        Session("TransID") = ""
        txtStatus.Text = "Open"
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCopyFrom.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        dtpBank_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub
    'Copy From



    'Tax
    Public Function LoadTaxCode(ByVal Type As String, ByVal TaxType As String) As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  TaxDescription " &
                " FROM    [Main].dbo.tblTax_Maintenance WHERE Type = @Type AND TaxType = @TaxType ORDER BY Sort"
        SQL.FlushParams()
        SQL.AddParam("@Type", Type, SqlDbType.NVarChar)
        SQL.AddParam("@TaxType", TaxType, SqlDbType.NVarChar)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("TaxDescription").ToString)
        End While
        Return list
    End Function


    <WebMethod()>
    Public Shared Function LoadTaxPercent(TaxCode As String, EWTCode As String) As String
        Dim TaxPercent As String = "0.00%"
        Dim EWTPercent As String = "0.00%"
        Dim query As String
        query = "SELECT TaxRate FROM [Main].dbo.tblTax_Maintenance " & vbCrLf &
                "WHERE TaxDescription = @TaxDescription"
        SQL.FlushParams()
        SQL.AddParam("@TaxDescription", TaxCode, SqlDbType.NVarChar)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read() Then
            TaxPercent = SQL.SQLDR("TaxRate").ToString & "%"
        End If
        query = "SELECT TaxRate FROM [Main].dbo.tblTax_Maintenance " & vbCrLf &
                "WHERE TaxDescription = @TaxDescription"
        SQL.FlushParams()
        SQL.AddParam("@TaxDescription", EWTCode, SqlDbType.NVarChar)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read() Then
            EWTPercent = SQL.SQLDR("TaxRate").ToString & "%"
        End If
        Return TaxPercent & "|" & EWTPercent
    End Function

    Private Sub btnSaveTax_Click(sender As Object, e As EventArgs) Handles btnSaveTax.Click
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = Nothing
            Dim strAccntCode As String = ""
            Dim decRow As Decimal = CDec(IIf(IsNumeric(txtRow.Text) = False, 1, txtRow.Text)) - 1
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")
                    Dim txtVATType As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtVATType")

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("Debit") = txtDebit.Text
                    dt.Rows(i)("Credit") = txtCredit.Text
                    dt.Rows(i)("Particulars") = txtParticulars.Text
                    dt.Rows(i)("Code") = txtCode.Text
                    dt.Rows(i)("Name") = txtName.Text
                    dt.Rows(i)("RefID") = txtRefID.Text
                    dt.Rows(i)("VATType") = txtVATType.Text

                    If decRow = i Then
                        strAccntCode = txtAccntCode.Text
                    End If

                    rowIndex = i
                Next
                dt.Rows.RemoveAt(decRow)
                '-------------------COMPUTE TAX------------------------
                Dim decNetAmount As Decimal = If(IsNumeric(txtTNetAmount.Text), CDec(txtTNetAmount.Text), 0)
                Dim decVAmount As Decimal = If(IsNumeric(txtTTaxAmount.Text), CDec(txtTTaxAmount.Text), 0)
                Dim decEAmount As Decimal = If(IsNumeric(txtETaxAmount.Text), CDec(txtETaxAmount.Text), 0)
                Dim decAmount As Decimal = If(IsNumeric(txtTTotalAmount.Text), CDec(txtTTotalAmount.Text), 0)
                '-----------------------NET AMOUNT---------------------------
                If decAmount > 0 Then
                    dr = dt.NewRow
                    dr("AccntCode") = strAccntCode
                    dr("AccntTitle") = GetAccountTitle(strAccntCode)
                    dr("Debit") = decNetAmount.ToString("N2")
                    dr("Credit") = "0.00"
                    dr("VATType") = IIf(ddlTaxType.SelectedIndex > 0, ddlTaxType.SelectedItem, "")
                    dt.Rows.InsertAt(dr, decRow)
                    decRow += 1
                End If
                '--------------------------VAT-------------------------------
                If decVAmount > 0 Then
                    Dim strTaxType As String = ddlTaxType.SelectedValue
                    Dim query As String
                    query = " SELECT tblSystemSetup.AP_InputVAT, tblCOA.AccountTitle FROM tblSystemSetup " &
                            " INNER JOIN tblCOA ON tblSystemSetup.AP_InputVAT = tblCOA.AccountCode "
                    SQL.FlushParams()
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        dr = dt.NewRow
                        dr("AccntCode") = SQL.SQLDR("AP_InputVAT").ToString
                        dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                        dr("Debit") = decVAmount.ToString("N2")
                        dr("Credit") = "0.00"
                        dr("VATType") = "Input VAT"
                        dt.Rows.InsertAt(dr, decRow)
                        decRow += 1
                    End If
                End If
                '--------------------------EWT-------------------------------
                If decEAmount > 0 Then
                    Dim strTaxType As String = ddlETaxType.SelectedValue
                    Dim query As String
                    query = " SELECT tblSystemSetup.TAX_EWT, tblCOA.AccountTitle FROM tblSystemSetup " &
                            " INNER JOIN tblCOA ON tblSystemSetup.TAX_EWT = tblCOA.AccountCode "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        dr = dt.NewRow
                        dr("chNo") = rowIndex + 1
                        dr("AccntCode") = SQL.SQLDR("TAX_EWT").ToString
                        dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                        dr("Debit") = "0.00"
                        dr("Credit") = decEAmount.ToString("N2")
                        dr("VATType") = "EWT"
                        dt.Rows.InsertAt(dr, decRow)
                    End If
                End If
                '-----------------------AMOUNT---------------------------
                'If decAmount > 0 Then
                '    dr = dt.NewRow
                '    dr("AccntCode") = Session("AccountCode")
                '    dr("AccntTitle") = Session("AccountTitle")
                '    dr("Debit") = "0.00"
                '    dr("Credit") = decAmount.ToString("N2")
                '    dt.Rows.Add(dr)
                '    txtAmount.Text = decAmount.ToString("N2")
                'End If
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
        End If
        txtTNetAmount.Text = "0.00"
        txtETaxAmount.Text = "0.00"
        txtTTaxAmount.Text = "0.00"
        txtTTotalAmount.Text = "0.00"
        txtTAmount.Text = "0.00"

        ddlTaxType.Items.Clear()
        ddlTaxType.Items.Add("--Select VAT Rate--")
        ddlTaxType.DataSource = LoadTaxCode("Purchases", "INPUT VAT").ToArray
        ddlTaxType.DataBind()

        ddlETaxType.Items.Clear()
        ddlETaxType.Items.Add("--Select EWT Rate--")
        ddlETaxType.DataSource = LoadTaxCode("Purchases", "EWT").ToArray
        ddlETaxType.DataBind()
    End Sub

    Dim totalDebit, totalCredit As Decimal
    Private Sub dgvEntry_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles dgvEntry.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = e.Row.DataItem
            If IsNumeric(row(3)) And IsNumeric(row(4)) Then
                totalDebit += Convert.ToDouble(row(3))
                totalCredit += Convert.ToDouble(row(4))
                Session("TotalDebit") = totalDebit
                Session("TotalCredit") = totalCredit
            End If
            If IsNumeric(row(4)) And IsNumeric(row(5)) Then
                totalDebit += Convert.ToDouble(row(4))
                totalCredit += Convert.ToDouble(row(5))
                Session("TotalDebit") = totalDebit
                Session("TotalCredit") = totalCredit
            End If
        End If
    End Sub
End Class