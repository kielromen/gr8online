﻿Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Public Class AcknowledgementReceipt
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ARNo As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "AR"
    Dim ColumnPK As String = "AR_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblCollection_AR"
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
                    EnableControl(False)
                    dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                    dtpBank_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                End If

            End If
        End If
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

    Public Sub Initialize()
        Session("Transno") = ""
        Session("AccountCode") = ""
        Session("AccountTitle") = ""
        Session("TotalDebit") = 0
        Session("TotalCredit") = 0
        txtRow.Style.Add("display", "none")
        txtCode.Attributes.Add("readonly", "readonly")
        txtStatus.Text = ""
        txtStatus.Attributes.Add("readonly", "readonly")
        txtCode.Text = ""
        txtName.Text = ""
        txtAmount.Text = "0.00"
        txtRemarks.Text = ""
        'txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        txtBank_CheckNo.Text = ""
        txtBank_Name.Text = ""
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        dtpBank_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        WithBank()
        BankDetails()
        LoadDatagrid()

        ddlType.Items.Clear()
        ddlType.Items.Add("--Select Payment Type--")
        ddlType.DataSource = LoadCollectionPaymentType().ToArray
        ddlType.DataBind()

        ddlBank.Items.Clear()
        ddlBank.Items.Add("--Select Bank--")
        ddlBank.DataSource = LoadBank().ToArray
        ddlBank.DataBind()

        ddlCollectionType.Items.Clear()
        ddlCollectionType.Items.Add("--Select Collection Type--")
        ddlCollectionType.DataSource = LoadCollectionType().ToArray
        ddlCollectionType.DataBind()


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
        ddlTaxType.DataSource = LoadTaxCode("Sales", "OUTPUT VAT").ToArray
        ddlTaxType.DataBind()

        ddlETaxType.Items.Clear()
        ddlETaxType.Items.Add("--Select EWT Rate--")
        ddlETaxType.DataSource = LoadTaxCode("Sales", "CWT").ToArray
        ddlETaxType.DataBind()
        'tax
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

    Private Sub AcknowledgementReceipt_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        ARNo = ""
        Session("TransID") = ""
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCopyFrom.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If ARNo = "" Then
            Initialize()
            EnableControl(False)
            btnEdit.Attributes("disabled") = "disabled"
            btnCancel.Attributes("disabled") = "disabled"
            btnPrev.Attributes("disabled") = "disabled"
            btnNext.Attributes("disabled") = "disabled"
        Else
            Initialize()
            LoadTransaction(TransID)
            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
        End If
        btnSearch.Attributes.Remove("disabled")
        btnNew.Attributes.Remove("disabled")
        btnSave.Attributes("disabled") = "disabled"
        btnCopyFrom.Attributes("disabled") = "disabled"
        btnClose.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableControl(True)
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCopyFrom.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblCollection_AR  WHERE AR_No > '" & Session("Transno") & "' ORDER BY AR_No ASC "
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
            query = " SELECT Top 1 TransID FROM tblCollection_AR  WHERE AR_No < '" & Session("Transno") & "' ORDER BY AR_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query, PaymentType As String
        query = " SELECT  TransID, AR_No, tblCollection_AR.VCECode, Name, TransDate, Amount, Remarks, PaymentType, DepositTo," &
                "          tblCollection_AR.Status, CheckDate, BankRef, CheckRef " &
                " FROM    tblCollection_AR LEFT JOIN View_VCEMMaster " &
                " ON      tblCollection_AR.VCECode = View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            ARNo = SQL.SQLDR("AR_No").ToString
            Session("Transno") = SQL.SQLDR("AR_No").ToString
            txtTrans_Num.Text = ARNo
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("Name").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            txtAmount.Text = SQL.SQLDR("Amount").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtStatus.Text = SQL.SQLDR("Status").ToString
            dtpBank_Date.Value = CDate(SQL.SQLDR("CheckDate")).ToString("yyyy-MM-dd")
            txtBank_Name.Text = SQL.SQLDR("BankRef").ToString
            txtBank_CheckNo.Text = SQL.SQLDR("CheckRef").ToString

            PaymentType = SQL.SQLDR("PaymentType").ToString
            ddlType.Text = PaymentType
            ddlBank.SelectedValue = SQL.SQLDR("DepositTo").ToString
            WithBank()
            BankDetails()

            LoadEntry(TransID)
            If txtStatus.Text = "Cancelled" Or txtStatus.Text = "Posted" Then
                btnEdit.Attributes("disabled") = "disabled"
                btnCancel.Attributes("disabled") = "disabled"
            Else
                btnEdit.Attributes.Remove("disabled")
                btnCancel.Attributes.Remove("disabled")
            End If
            btnClose.Attributes.Remove("disabled")
            btnSave.Attributes("disabled") = "disabled"
            btnCopyFrom.Attributes("disabled") = "disabled"
            btnNew.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
            btnSearch.Attributes.Remove("disabled")
            EnableControl(False)
        Else
            Initialize()
        End If
    End Sub

    Private Sub LoadEntry(ByVal ARNo As Integer)
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
        query = " SELECT ID, JE_No, View_GL.BranchCode, View_GL.AccntCode, AccountTitle, View_GL.VCECode, View_GL.VCEName, Debit, Credit, Particulars,CostID, RefNo,VATType   " &
                " FROM   View_GL INNER JOIN tblCOA " &
                " ON     View_GL.AccntCode = tblCOA.AccountCode " &
                " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE Upload = 0 AND RefType = 'AR' AND RefTransID = " & ARNo & ") " &
                " ORDER BY Debit DESC, AccntCode ASC "
        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("AccntCode") = SQL.SQLDR("AccntCode").ToString
            dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
            dr("Debit") = CDec(SQL.SQLDR("Debit")).ToString("N2")
            dr("Credit") = CDec(SQL.SQLDR("Credit")).ToString("N2")
            dr("Particulars") = SQL.SQLDR("Particulars").ToString
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

    Public Sub WithBank()
        Dim query As String
        query = " SELECT WithBank" & vbCrLf &
                " FROM   tblCollection_PaymentType " & vbCrLf &
                " WHERE  tblCollection_PaymentType.Status ='Active' AND PaymentType = @PaymentType"
        SQL.FlushParams()
        SQL.AddParam("PaymentType", ddlType.SelectedValue)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            panelBank.Visible = SQL.SQLDR("WithBank")
            txtBank_Name.Text = ""
            txtBank_CheckNo.Text = ""
        Else
            panelBank.Visible = False
            txtBank_Name.Text = ""
            txtBank_CheckNo.Text = ""
        End If
    End Sub

    Public Sub BankDetails()
        Dim query As String
        query = " SELECT tblBank.AccountCode, AccountTitle" & vbCrLf &
                " FROM   tblBank " & vbCrLf &
                " INNER JOIN tblCOA ON tblBank.AccountCode = tblCOA.AccountCode " & vbCrLf &
                " WHERE  tblBank.Status ='Active' AND Bank = @Bank"
        SQL.FlushParams()
        SQL.AddParam("Bank", ddlBank.SelectedValue)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Session("AccountCode") = SQL.SQLDR("AccountCode")
            Session("AccountTitle") = SQL.SQLDR("AccountTitle")
        Else
            Session("AccountCode") = ""
            Session("AccountTitle") = ""
        End If
    End Sub

    Protected Sub ddlCollectionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim query, Collection_Type As String
        Collection_Type = ddlCollectionType.Text
        query = " SELECT  tblCollection_Type.AccountCode, AccountTitle, Amount, Description  " &
                " FROM    tblCollection_Type INNER JOIN tblCOA " &
                " ON      tblCollection_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE   tblCollection_Type.Status = 'Active' AND Description = @Description "
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
                drow("Debit") = "0.00"
                drow("Credit") = CDec(SQL.SQLDR("Amount")).ToString("N2")
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
                dr("Debit") = "0.00"
                dr("Credit") = CDec(SQL.SQLDR("Amount")).ToString("N2")
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
                    ARNo = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    ARNo = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = ARNo
                If Not IfExist(txtTrans_Num.Text) Then

                    Save()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                    LoadTransaction(TransID)
                Else
                    Response.Write("<script>alert('Not Save, transaction " & txtTrans_Num.Text & " already exist!');</script>")
                End If
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    ARNo = txtTrans_Num.Text
                    Update()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    ARNo = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        ARNo = txtTrans_Num.Text
                        Update()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        ARNo = txtTrans_Num.Text
                        LoadTransaction(Session("TransID"))
                    Else
                        Response.Write("<script>alert(''Not Save, transaction " & txtTrans_Num.Text & " already exist!');</script>")
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub Save()
        Dim insertSQL As String
        activityStatus = True
        SQL.FlushParams()
        insertSQL = " INSERT INTO " &
                        " tblCollection_AR (TransID,  AR_No, TransDate, VCECode, PaymentType, DepositTo, Amount,  CheckRef, BankRef, CheckDate, Remarks, TransAuto, WhoCreated) " &
                        " VALUES(@TransID, @AR_No, @TransDate, @VCECode, @PaymentType, @DepositTo, @Amount, @CheckRef, @BankRef, @CheckDate, @Remarks, @TransAuto, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@AR_No", ARNo)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@PaymentType", ddlType.Text)
        SQL.AddParam("@DepositTo", ddlBank.SelectedValue)
        SQL.AddParam("@VCECode", txtCode.Text)
        If IsNumeric(txtAmount.Text) Then
            SQL.AddParam("@Amount", CDec(txtAmount.Text))
        Else
            SQL.AddParam("@Amount", 0)
        End If
        SQL.AddParam("@CheckRef", txtBank_CheckNo.Text)
        SQL.AddParam("@BankRef", txtBank_Name.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@CheckDate", dtpDoc_Date.Value)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        insertSQL = " INSERT INTO " &
                        " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
                        " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@AppDate", dtpDoc_Date.Value)
        SQL.AddParam("@RefType", "AR")
        SQL.AddParam("@RefTransID", TransID)
        SQL.AddParam("@Book", "Cash Receipt")
        SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        JETransiD = LoadJE("AR", TransID)

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
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, CostCenter, RefNo, VATType, LineNumber, Status) " &
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @CostCenter, @RefNo, @VATType, @LineNumber, @Status)"
                        SQL.FlushParams()
                        SQL.AddParam("@JE_No", JETransiD)
                        SQL.AddParam("@AccntCode", txtAccntCode.Text)
                        If txtEntryCode.Text <> Nothing AndAlso txtEntryCode.Text <> "" Then
                            SQL.AddParam("@VCECode", txtEntryCode.Text)
                        Else
                            SQL.AddParam("@VCECode", "")
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

    Private Sub Update()
        Dim insertSQL, updateSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblCollection_AR  " &
                        " SET     AR_No = @AR_No, TransDate = @TransDate, PaymentType = @PaymentType, DepositTo = @DepositTo, " &
                        "        VCECode = @VCECode, Amount = @Amount, CheckRef = @CheckRef, BankRef = @BankRef, CheckDate = @CheckDate, " &
                        "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE()" &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@AR_No", ARNo)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@PaymentType", ddlType.Text)
        SQL.AddParam("@DepositTo", ddlBank.SelectedValue)
        SQL.AddParam("@VCECode", txtCode.Text)
        If IsNumeric(txtAmount.Text) Then
            SQL.AddParam("@Amount", CDec(txtAmount.Text))
        Else
            SQL.AddParam("@Amount", 0)
        End If
        SQL.AddParam("@CheckRef", txtBank_CheckNo.Text)
        SQL.AddParam("@BankRef", txtBank_Name.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@CheckDate", dtpBank_Date.Value)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)


        JETransiD = LoadJE("AR", Session("TransID").ToString)
        If JETransiD = 0 Then
            insertSQL = " INSERT INTO " &
                        " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
                        " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
            SQL.FlushParams()
            SQL.AddParam("@AppDate", dtpDoc_Date.Value)
            SQL.AddParam("@RefType", "AR")
            SQL.AddParam("@RefTransID", Session("TransID").ToString)
            SQL.AddParam("@Book", "Cash Receipt")
            SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoCreated", Session("EmailAddress"))
            SQL.ExecNonQuery(insertSQL)

            JETransiD = LoadJE("AR", Session("TransID").ToString)
        Else
            updateSQL = " UPDATE tblJE_Header " &
                           " SET    AppDate = @AppDate,  " &
                           "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " &
                           "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " &
                           " WHERE  JE_No = @JE_No "
            SQL.FlushParams()
            SQL.AddParam("@JE_No", JETransiD)
            SQL.AddParam("@AppDate", dtpDoc_Date.Value)
            SQL.AddParam("@RefType", "AR")
            SQL.AddParam("@RefTransID", Session("TransID").ToString)
            SQL.AddParam("@Book", "Cash Receipt")
            SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
            SQL.AddParam("@Remarks", txtRemarks.Text)
            SQL.AddParam("@WhoModified", Session("EmailAddress"))
            SQL.ExecNonQuery(updateSQL)
        End If


        ' UPDATE ACCOUNTING ENTRIES
        updateSQL = " UPDATE tblJE_Details SET Status = 'Inactive'  WHERE  JE_No = @JE_No "
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
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtCredit_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtParticulars_Entry")
                    Dim txtEntryCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim ddlCostCenter As DropDownList = dgvEntry.Rows(i).Cells(9).FindControl("ddlCostCenter")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtRefID_Entry")
                    Dim txtVATType As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATType")

                    Dim CostID = GetCostCenterID(ddlCostCenter.SelectedValue)

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, CostCenter, RefNo, VATType, LineNumber, Status) " &
                                " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @CostCenter, @RefNo, @VATType, @LineNumber, @Status)"
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

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblCollection_AR WHERE AR_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Protected Sub ComputeRow(sender As Object, e As EventArgs)
        Page.Validate()
        If txtAmount.Text <> "" Then
            If Session("AccountCode") <> "" Then
                'AddNewRow(dgvEntry, EventArgs.Empty)
                txtAmount.Text = CDec(Session("TotalCredit") - Session("TotalDebit")).ToString("N2")
                Dim rowIndex As Integer = 0
                If Not IsNothing(ViewState("EntryTable")) Then
                    Dim dt As DataTable = ViewState("EntryTable")
                    Dim dr As DataRow = Nothing

                    dr = dt.NewRow
                    dr("chNo") = 1
                    dr("AccntCode") = Session("AccountCode")
                    dr("AccntTitle") = Session("AccountTitle")
                    dr("Debit") = txtAmount.Text
                    dr("Credit") = "0.00"
                    dr("Particulars") = ""
                    dr("Code") = ""
                    dr("Name") = ""
                    dr("RefID") = ""

                    dt.Rows.InsertAt(dr, 0)

                    If dgvEntry.Rows.Count > 0 Then
                        For i As Integer = 0 To dgvEntry.Rows.Count - 1
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

                            dt.Rows(i + 1)("AccntCode") = txtAccntCode.Text
                            dt.Rows(i + 1)("AccntTitle") = txtAccntTitle.Text
                            dt.Rows(i + 1)("Debit") = txtDebit.Text
                            dt.Rows(i + 1)("Credit") = txtCredit.Text
                            dt.Rows(i + 1)("Particulars") = txtParticulars.Text
                            dt.Rows(i + 1)("Code") = txtCode.Text
                            dt.Rows(i + 1)("Name") = txtName.Text
                            dt.Rows(i + 1)("RefID") = txtRefID.Text
                        Next
                        ViewState("EntryTable") = dt

                        dgvEntry.DataSource = dt
                        dgvEntry.DataBind()
                    End If
                    SetDataTable()
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

        query = " UPDATE  tblJE_Header SET Status ='Cancelled' WHERE RefTransID = @RefTransID  AND RefType = '" & ModuleID & "' "
        SQL.FlushParams()
        SQL.AddParam("@RefTransID", Session("TransID"))
        SQL.ExecNonQuery(query)

        LoadTransaction(Session("TransID"))
    End Sub

    'Copy From
    Private Sub LoadCopyFrom(ByVal CopyFromID As String, ByVal Type As String)
        CopyFromInitialize()
        Dim query As String
        Select Case Type
            Case "CASHR"
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
            Case "SJ"
                query = " SELECT   TransID, SJ_No AS TransNo, VCECode, VCEName, Remarks" &
                        " FROM     View_SJ_Balance " &
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
        Dim differed As Boolean = False
        Select Case Type
            Case "CASHR"
                query = " SELECT TransID, CA_No AS TransNo, Date, VCECode, VCEName, Amount AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle,  CostID, RefNo, Nature, VatType " &
                        " FROM  View_CA_Return " &
                        " WHERE TransID  ='" & CopyFromID & "' "
                SQL.ReadQuery(query)
            Case "SJ"
                query = " SELECT TransID, SJ_No AS TransNo, Date, VCECode, VCEName, Amount AS TotalAmount, Remarks, Particulars, AccountCode, AccountTitle,  CostID, RefNo, Nature, VatType " &
                        " FROM  View_SJ_Balance " &
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
                drow("Debit") = IIf(SQL.SQLDR("Nature").ToString = "Debit", CDec(SQL.SQLDR("TotalAmount")).ToString("N2"), "0.00")
                drow("Credit") = IIf(SQL.SQLDR("Nature").ToString = "Credit", CDec(SQL.SQLDR("TotalAmount")).ToString("N2"), "0.00")
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
                dr("Debit") = IIf(SQL.SQLDR("Nature").ToString = "Debit", CDec(SQL.SQLDR("TotalAmount")).ToString("N2"), "0.00")
                dr("Credit") = IIf(SQL.SQLDR("Nature").ToString = "Credit", CDec(SQL.SQLDR("TotalAmount")).ToString("N2"), "0.00")
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
        ARNo = ""
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
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
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
                '-----------------------AMOUNT---------------------------
                If decAmount > 0 Then
                    dr = dt.NewRow
                    dr("AccntCode") = Session("AccountCode")
                    dr("AccntTitle") = Session("AccountTitle")
                    dr("Debit") = decAmount.ToString("N2")
                    dr("Credit") = "0.00"
                    dt.Rows.InsertAt(dr, decRow)

                    txtAmount.Text = decAmount.ToString("N2")
                    decRow += 1
                End If

                '--------------------------EWT-------------------------------
                If decEAmount > 0 Then
                    Dim strTaxType As String = ddlETaxType.SelectedValue
                    Dim query As String
                    query = " SELECT tblSystemSetup.TAX_CWT, tblCOA.AccountTitle FROM tblSystemSetup " &
                            " INNER JOIN tblCOA ON tblSystemSetup.TAX_CWT = tblCOA.AccountCode "
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        dr = dt.NewRow
                        dr("chNo") = rowIndex + 1
                        dr("AccntCode") = SQL.SQLDR("TAX_CWT").ToString
                        dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                        dr("Debit") = decEAmount.ToString("N2")
                        dr("Credit") = "0.00"
                        dr("VATType") = "CWT"
                        dt.Rows.InsertAt(dr, decRow)
                        decRow += 1
                    End If
                End If

                '--------------------------VAT-------------------------------
                If decVAmount > 0 Then
                    Dim strTaxType As String = ddlTaxType.SelectedValue
                    Dim query As String
                    query = " SELECT tblSystemSetup.TAX_Deferred, tblCOA.AccountTitle FROM tblSystemSetup " &
                            " INNER JOIN tblCOA ON tblSystemSetup.TAX_Deferred = tblCOA.AccountCode "
                    SQL.FlushParams()
                    SQL.ReadQuery(query)
                    If SQL.SQLDR.Read Then
                        dr = dt.NewRow
                        dr("AccntCode") = SQL.SQLDR("TAX_Deferred").ToString
                        dr("AccntTitle") = SQL.SQLDR("AccountTitle").ToString
                        dr("Debit") = "0.00"
                        dr("Credit") = decVAmount.ToString("N2")
                        dr("VATType") = "Output VAT"
                        dt.Rows.InsertAt(dr, decRow)
                        decRow += 1
                    End If
                End If

                '-----------------------NET AMOUNT---------------------------
                If decAmount > 0 Then
                    dr = dt.NewRow
                    dr("AccntCode") = strAccntCode
                    dr("AccntTitle") = GetAccountTitle(strAccntCode)
                    dr("Debit") = "0.00"
                    dr("Credit") = decNetAmount.ToString("N2")
                    dr("VATType") = IIf(ddlTaxType.SelectedIndex > 0, ddlTaxType.SelectedItem, "")
                    dt.Rows.InsertAt(dr, decRow)
                    decRow += 1
                End If


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
        ddlTaxType.DataSource = LoadTaxCode("Sales", "OUTPUT VAT").ToArray
        ddlTaxType.DataBind()

        ddlETaxType.Items.Clear()
        ddlETaxType.Items.Add("--Select EWT Rate--")
        ddlETaxType.DataSource = LoadTaxCode("Sales", "CWT").ToArray
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