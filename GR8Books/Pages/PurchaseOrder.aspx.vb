Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class PurchaseOrder
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim PONO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "PO"
    Dim ColumnPK As String = "PO_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblPO"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                TransAuto = GetTransSetup(ModuleID)
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
                    EnableControl(False)
                End If
                dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                dtpDelivery_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
            End If
        End If
    End Sub

    Public Sub LoadDatagrid()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ItemName"))
        dt.Columns.Add(New DataColumn("ItemCode"))
        dt.Columns.Add(New DataColumn("Description"))
        dt.Columns.Add(New DataColumn("UnitPrice"))
        dt.Columns.Add(New DataColumn("UOM"))
        dt.Columns.Add(New DataColumn("QTY"))
        'dt.Columns.Add(New DataColumn("UnitCost"))
        'dt.Columns.Add(New DataColumn("GrossAmount"))
        'dt.Columns.Add(New DataColumn("VATAmount"))
        'dt.Columns.Add(New DataColumn("VATInc"))
        'dt.Columns.Add(New DataColumn("DiscountRate"))
        'dt.Columns.Add(New DataColumn("Discount"))
        'dt.Columns.Add(New DataColumn("NetAmount"))
        dt.Columns.Add(New DataColumn("VATable"))
        dt.Columns.Add(New DataColumn("WHS"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        '.txtGrossAmount_Entry, .txtVATAmount_Entry, .txtVATInc_Entry, .txtDiscountRate_Entry, .txtDiscount_Entry, .txtNetAmount_Entry,

        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("ItemName") = Nothing
        dr("ItemCode") = Nothing
        dr("Description") = Nothing
        dr("UnitPrice") = "0.00"
        dr("UOM") = Nothing
        dr("QTY") = "0.00"
        'dr("UnitCost") = "0.00"
        'dr("GrossAmount") = "0.00"
        'dr("VATAmount") = "0.00"
        'dr("VATInc") = False
        'dr("DiscountRate") = "0.00"
        'dr("Discount") = "0.00"
        'dr("NetAmount") = "0.00"
        dr("VATable") = False
        dr("WHS") = Nothing
        dr("AccntCode") = Nothing
        dr("AccntTitle") = Nothing

        dt.Rows.Add(dr)

        ViewState("EntryTable") = dt

        dgvEntry.DataSource = dt
        dgvEntry.DataBind()
    End Sub
    Protected Sub AddNewRow(sender As Object, e As EventArgs)
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtCode_Entry")
                    Dim txtItemName As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtName_Entry")
                    Dim txtDescription As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDescription_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUnitPrice_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtQTY_Entry")
                    'Dim txtUnitCost As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtUnitCost_Entry")
                    'Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtGrossAmount_Entry")
                    'Dim txtVATAmount As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATAmount_Entry")
                    'Dim txtVATInc As CheckBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATInc_Entry")
                    'Dim txtDiscountRate As TextBox = dgvEntry.Rows(i).Cells(12).FindControl("txtDiscountRate_Entry")
                    'Dim txtDiscount As TextBox = dgvEntry.Rows(i).Cells(13).FindControl("txtDiscount_Entry")
                    'Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(14).FindControl("txtNetAmount_Entry")
                    Dim txtVATable As CheckBox = dgvEntry.Rows(i).Cells(8).FindControl("txtVATable_Entry")
                    Dim txtWHS As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtWHS_Entry")
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtAccntTitle_Entry")

                    dr = dt.NewRow
                    dr("chNo") = i + 2

                    dt.Rows(i)("ItemCode") = txtItemCode.Text
                    dt.Rows(i)("ItemName") = txtItemName.Text
                    dt.Rows(i)("Description") = txtDescription.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    'dt.Rows(i)("UnitCost") = txtUnitCost.Text
                    'dt.Rows(i)("GrossAmount") = txtGrossAmount.Text
                    'dt.Rows(i)("VATAmount") = txtVATAmount.Text
                    'dt.Rows(i)("VATInc") = txtVATInc.Checked
                    'dt.Rows(i)("DiscountRate") = txtDiscountRate.Text
                    'dt.Rows(i)("Discount") = txtDiscount.Text
                    'dt.Rows(i)("NetAmount") = txtNetAmount.Text
                    dt.Rows(i)("VATable") = txtVATable.Checked
                    dt.Rows(i)("WHS") = txtWHS.Text
                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text

                    rowIndex = i
                Next
                dt.Rows.Add(dr)
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
        End If
    End Sub

    Private Sub SetDataTable()
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtCode_Entry")
                    Dim txtItemName As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtName_Entry")
                    Dim txtDescription As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDescription_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUnitPrice_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtQTY_Entry")
                    'Dim txtUnitCost As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtUnitCost_Entry")
                    'Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtGrossAmount_Entry")
                    'Dim txtVATAmount As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATAmount_Entry")
                    'Dim txtVATInc As CheckBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATInc_Entry")
                    'Dim txtDiscountRate As TextBox = dgvEntry.Rows(i).Cells(12).FindControl("txtDiscountRate_Entry")
                    'Dim txtDiscount As TextBox = dgvEntry.Rows(i).Cells(13).FindControl("txtDiscount_Entry")
                    'Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(14).FindControl("txtNetAmount_Entry")
                    Dim txtVATable As CheckBox = dgvEntry.Rows(i).Cells(8).FindControl("txtVATable_Entry")
                    Dim txtWHS As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtWHS_Entry")
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtAccntTitle_Entry")


                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtItemCode.Text = dt.Rows(i)("ItemCode").ToString
                    txtItemName.Text = dt.Rows(i)("ItemName").ToString
                    txtDescription.Text = dt.Rows(i)("Description").ToString
                    txtUnitPrice.Text = dt.Rows(i)("UnitPrice").ToString
                    txtUOM.Text = dt.Rows(i)("UOM").ToString
                    txtQTY.Text = dt.Rows(i)("QTY").ToString
                    'txtUnitCost.Text = dt.Rows(i)("UnitCost").ToString
                    'txtGrossAmount.Text = dt.Rows(i)("GrossAmount").ToString
                    'txtVATAmount.Text = dt.Rows(i)("VATAmount").ToString
                    'txtVATInc.Text = dt.Rows(i)("VATInc").ToString
                    'txtDiscountRate.Text = dt.Rows(i)("DiscountRate").ToString
                    'txtDiscount.Text = dt.Rows(i)("Discount").ToString
                    'txtNetAmount.Text = dt.Rows(i)("NetAmount").ToString
                    ' If()
                    'MsgBox(dt.Rows(i)("VATable"))
                    'If(IsNothing(dt.Rows(i)("VATable")), False, True)
                    txtVATable.Checked = If(IsNothing(dt.Rows(i)("VATable")), False, True)
                    txtWHS.Text = dt.Rows(i)("WHS").ToString
                    txtAccntCode.Text = dt.Rows(i)("AccntCode").ToString
                    txtAccntTitle.Text = dt.Rows(i)("AccntTitle").ToString

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
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtCode_Entry")
                    Dim txtItemName As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtName_Entry")
                    Dim txtDescription As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDescription_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUnitPrice_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtQTY_Entry")
                    'Dim txtUnitCost As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtUnitCost_Entry")
                    'Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtGrossAmount_Entry")
                    'Dim txtVATAmount As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATAmount_Entry")
                    'Dim txtVATInc As CheckBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATInc_Entry")
                    'Dim txtDiscountRate As TextBox = dgvEntry.Rows(i).Cells(12).FindControl("txtDiscountRate_Entry")
                    'Dim txtDiscount As TextBox = dgvEntry.Rows(i).Cells(13).FindControl("txtDiscount_Entry")
                    'Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(14).FindControl("txtNetAmount_Entry")
                    Dim txtVATable As CheckBox = dgvEntry.Rows(i).Cells(8).FindControl("txtVATable_Entry")
                    Dim txtWHS As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtWHS_Entry")
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtAccntTitle_Entry")

                    dt.Rows(i)("ItemCode") = txtItemCode.Text
                    dt.Rows(i)("ItemName") = txtItemName.Text
                    dt.Rows(i)("Description") = txtDescription.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    'dt.Rows(i)("UnitCost") = txtUnitCost.Text
                    'dt.Rows(i)("GrossAmount") = txtGrossAmount.Text
                    'dt.Rows(i)("VATAmount") = txtVATAmount.Text
                    'dt.Rows(i)("VATInc") = txtVATInc.Checked
                    'dt.Rows(i)("DiscountRate") = txtDiscountRate.Text
                    'dt.Rows(i)("Discount") = txtDiscount.Text
                    'dt.Rows(i)("NetAmount") = txtNetAmount.Text
                    dt.Rows(i)("VATable") = txtVATable.Checked
                    dt.Rows(i)("WHS") = txtWHS.Text
                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text

                Next

                dt.Rows.RemoveAt(rowIndex)
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
    Public Shared Function ListItem(prefix As String) As String()
        Dim strName As New List(Of String)()
        Dim query As String
        query = "SELECT ItemCode, ItemName FROM tblitem_master " & vbCrLf &
                "WHERE ItemName LIKE '%' + @ItemName + '%' AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@ItemName", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            strName.Add(String.Format("{0}--{1}", SQL.SQLDR("ItemName"), SQL.SQLDR("ItemCode")))
        End While
        Return strName.ToArray()
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

    Public Sub Initialize()
        Session("Transno") = ""
        txtCode.Attributes.Add("readonly", "readonly")
        txtCode.Text = ""
        txtName.Text = ""
        txtGrossAmount.Text = ""
        txtDiscount.Text = ""
        txtVATAmount.Text = ""
        txtNetAmount.Text = ""
        txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        txtAddress.Text = ""
        txtContactNo.Text = ""
        txtRef_No.Text = ""
        dtpDoc_Date.Value = Now.Date
        dtpDelivery_Date.Value = Now.Date
        LoadDatagrid()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        panelEntry.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        dtpDelivery_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub PurchaseOrder_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        PONO = ""
        Session("TransID") = ""
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        dtpDelivery_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If PONO = "" Then
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
        btnClose.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            'For Each rows As GridViewRow In dgvEntry.Rows
            '    Dim AccntCode As TextBox
            '    AccntCode = rows.FindControl("txtAccntCode_Entry")
            '    If AccntCode.Text = "" Then
            '        Response.Write("<script>alert('Invalid Account Code!');</script>")
            '        Exit Sub
            '    End If
            'Next
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    PONO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    PONO = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = PONO
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    PONO = txtTrans_Num.Text
                    UpdateCV()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    PONO = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        PONO = txtTrans_Num.Text
                        UpdateCV()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        PONO = txtTrans_Num.Text
                        LoadTransaction(Session("TransID"))
                    Else
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " already exist!');</script>")
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
                        " tblPO (TransID, PO_No, VCECode, VCEName, Address, DeliveryDate, ContactNo, DatePO, GrossAmount, Discount, VATAmount, NetAmount, ReferenceNo," &
                        " Status, WhoCreated, TransAuto ) " &
                        " VALUES (@TransID, @PO_No, @VCECode, @VCEName, @Address, @DeliveryDate, @ContactNo,  @DatePO, @GrossAmount, @Discount, @VATAmount, @NetAmount, @ReferenceNo," &
                        " @Status, @WhoCreated, @TransAuto)"


        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@PO_No", PONO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@VCEName", txtName.Text)
        SQL.AddParam("@Address", txtAddress.Text)
        SQL.AddParam("@DeliveryDate", dtpDoc_Date.Value)
        SQL.AddParam("@ContactNo", txtContactNo.Text)
        SQL.AddParam("@DatePO", dtpDoc_Date.Value)
        SQL.AddParam("@GrossAmount", txtGrossAmount.Text)
        SQL.AddParam("@Discount", txtDiscount.Text)
        SQL.AddParam("@VATAmount", txtVATAmount.Text)
        SQL.AddParam("@NetAmount", txtNetAmount.Text)
        SQL.AddParam("@ReferenceNo", txtRef_No.Text)
        SQL.AddParam("@Status", "Active")
        'SQL.AddParam("@DateCreated", Date.Now)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.ExecNonQuery(insertSQL)

        'ACCOUNTING ENTRY
        'insertSQL = " INSERT INTO " &
        '                " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
        '                " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
        'SQL.FlushParams()
        'SQL.AddParam("@AppDate", dtpDoc_Date.Value)
        'SQL.AddParam("@RefType", "SJ")
        'SQL.AddParam("@RefTransID", TransID)
        'SQL.AddParam("@Book", "Sales Book")
        'SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
        'SQL.AddParam("@Remarks", txtRemarks.Text)
        'SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        'SQL.ExecNonQuery(insertSQL)

        'JETransiD = LoadJE("SJ", TransID)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtCode_Entry")
                    Dim txtItemName As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtName_Entry")
                    Dim txtDescription As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDescription_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUnitPrice_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtQTY_Entry")
                    'Dim txtUnitCost As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtUnitCost_Entry")
                    'Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtGrossAmount_Entry")
                    'Dim txtVATAmount As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATAmount_Entry")
                    'Dim txtVATInc As CheckBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATInc_Entry")
                    'Dim txtDiscountRate As TextBox = dgvEntry.Rows(i).Cells(12).FindControl("txtDiscountRate_Entry")
                    'Dim txtDiscount As TextBox = dgvEntry.Rows(i).Cells(13).FindControl("txtDiscount_Entry")
                    'Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(14).FindControl("txtNetAmount_Entry")
                    Dim txtVATable As CheckBox = dgvEntry.Rows(i).Cells(8).FindControl("txtVATable_Entry")
                    Dim txtWHS As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtWHS_Entry")
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtAccntTitle_Entry")

                    If txtItemCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblPO_Details(TransID, AccntCode, ItemCode, Description, UnitPrice, UOM, QTY, VATable, WHSE, LineNum) " &
                                " VALUES(@TransID, @AccntCode, @ItemCode, @Description, @UnitPrice, @UOM, @QTY, @VATable, @WHSE, @LineNum)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.AddParam("@AccntCode", txtAccntCode.Text)
                        SQL.AddParam("@ItemCode", txtItemCode.Text)
                        If txtDescription.Text <> Nothing AndAlso txtDescription.Text <> "" Then
                            SQL.AddParam("@Description", txtDescription.Text)
                        Else
                            SQL.AddParam("@Description", "")
                        End If
                        If txtUnitPrice.Text <> Nothing AndAlso IsNumeric(txtUnitPrice.Text) Then
                            SQL.AddParam("@UnitPrice", CDec(txtUnitPrice.Text))
                        Else
                            SQL.AddParam("@UnitPrice", 0)
                        End If
                        If txtUOM.Text <> Nothing AndAlso txtUOM.Text <> "" Then
                            SQL.AddParam("@UOM", txtUOM.Text)
                        Else
                            SQL.AddParam("@UOM", "")
                        End If
                        If txtQTY.Text <> Nothing AndAlso txtQTY.Text <> "" Then
                            SQL.AddParam("@QTY", txtQTY.Text)
                        Else
                            SQL.AddParam("@QTY", "")
                        End If
                        If txtVATable.Checked <> Nothing Then
                            SQL.AddParam("@VATable", txtVATable.Checked)
                        Else
                            SQL.AddParam("@VATable", False)
                        End If
                        If txtWHS.Text <> Nothing AndAlso txtWHS.Text <> "" Then
                            SQL.AddParam("@WHSE", txtWHS.Text)
                        Else
                            SQL.AddParam("@WHSE", "")
                        End If

                        SQL.AddParam("@LineNum", line)
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub UpdateCV()
        Dim insertSQL, updateSQL, delSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblPO  " &
                        " SET VCECode = @VCECode, VCEName = @VCEName, Address = @Address, DeliveryDate = @DeliveryDate , DatePO = @DatePO, " &
                        " ContactNo = @ContactNo ,GrossAmount = @GrossAmount, Discount = @Discount, VATAmount = @VATAmount, NetAmount = @NetAmount, " &
                        " ReferenceNo = @ReferenceNo ,TransAuto = @TransAuto , WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@VCEName", txtName.Text)
        SQL.AddParam("@Address", txtAddress.Text)
        SQL.AddParam("@DeliveryDate", dtpDelivery_Date.Value)
        SQL.AddParam("@DatePO", dtpDoc_Date.Value)
        SQL.AddParam("@ContactNo", txtContactNo.Text)
        SQL.AddParam("@GrossAmount", txtGrossAmount.Text)
        SQL.AddParam("@Discount", txtDiscount.Text)
        SQL.AddParam("@VATAmount", txtVATAmount.Text)
        SQL.AddParam("@NetAmount", txtNetAmount.Text)
        SQL.AddParam("@ReferenceNo", txtRef_No.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        'Delete DR Details
        delSQL = " DELETE FROM tblPO_Details " &
                 " WHERE TransID = @TransID"
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.ExecNonQuery(delSQL)

        'Insert New DR Details
        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtCode_Entry")
                    Dim txtItemName As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtName_Entry")
                    Dim txtDescription As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtDescription_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUnitPrice_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtQTY_Entry")
                    'Dim txtUnitCost As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtUnitCost_Entry")
                    'Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtGrossAmount_Entry")
                    'Dim txtVATAmount As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtVATAmount_Entry")
                    'Dim txtVATInc As CheckBox = dgvEntry.Rows(i).Cells(11).FindControl("txtVATInc_Entry")
                    'Dim txtDiscountRate As TextBox = dgvEntry.Rows(i).Cells(12).FindControl("txtDiscountRate_Entry")
                    'Dim txtDiscount As TextBox = dgvEntry.Rows(i).Cells(13).FindControl("txtDiscount_Entry")
                    'Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(14).FindControl("txtNetAmount_Entry")
                    Dim txtVATable As CheckBox = dgvEntry.Rows(i).Cells(8).FindControl("txtVATable_Entry")
                    Dim txtWHS As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtWHS_Entry")
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtAccntTitle_Entry")

                    If txtItemCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblPO_Details(TransID, AccntCode, ItemCode, Description, UnitPrice, UOM, QTY, VATable, WHSE, LineNum) " &
                                " VALUES(@TransID, @AccntCode, @ItemCode, @Description, @UnitPrice, @UOM, @QTY, @VATable, @WHSE, @LineNum)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", Session("TransID").ToString)
                        SQL.AddParam("@AccntCode", txtAccntCode.Text)
                        SQL.AddParam("@ItemCode", txtItemCode.Text)
                        If txtDescription.Text <> Nothing AndAlso txtDescription.Text <> "" Then
                            SQL.AddParam("@Description", txtDescription.Text)
                        Else
                            SQL.AddParam("@Description", "")
                        End If
                        If txtUnitPrice.Text <> Nothing AndAlso IsNumeric(txtUnitPrice.Text) Then
                            SQL.AddParam("@UnitPrice", CDec(txtUnitPrice.Text))
                        Else
                            SQL.AddParam("@UnitPrice", 0)
                        End If
                        If txtUOM.Text <> Nothing AndAlso txtUOM.Text <> "" Then
                            SQL.AddParam("@UOM", txtUOM.Text)
                        Else
                            SQL.AddParam("@UOM", "")
                        End If
                        If txtQTY.Text <> Nothing AndAlso txtQTY.Text <> "" Then
                            SQL.AddParam("@QTY", txtQTY.Text)
                        Else
                            SQL.AddParam("@QTY", "")
                        End If
                        If txtVATable.Checked <> Nothing Then
                            SQL.AddParam("@VATable", txtVATable.Checked)
                        Else
                            SQL.AddParam("@VATable", False)
                        End If
                        If txtWHS.Text <> Nothing AndAlso txtWHS.Text <> "" Then
                            SQL.AddParam("@WHSE", txtWHS.Text)
                        Else
                            SQL.AddParam("@WHSE", "")
                        End If

                        SQL.AddParam("@LineNum", line)
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblSJ WHERE SJ_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, PO_No, VCECode, VCEName, Address, DeliveryDate, ContactNo, DatePO, GrossAmount, Discount, VATAmount, NetAmount, " &
                "         ISNULL(ReferenceNo,0) as ReferenceNo, Status " &
                " FROM    tblPO " &
                " WHERE   TransID = '" & ID & "' "

        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            PONO = SQL.SQLDR("PO_No").ToString
            Session("Transno") = SQL.SQLDR("PO_No").ToString
            txtTrans_Num.Text = PONO
            txtRef_No.Text = SQL.SQLDR("ReferenceNo").ToString
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("VCEName").ToString
            txtAddress.Text = SQL.SQLDR("Address").ToString
            dtpDelivery_Date.Value = CDate(SQL.SQLDR("DeliveryDate")).ToString("yyyy-MM-dd")
            txtContactNo.Text = SQL.SQLDR("ContactNo").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("DatePO")).ToString("yyyy-MM-dd")
            txtGrossAmount.Text = SQL.SQLDR("GrossAmount").ToString
            txtDiscount.Text = SQL.SQLDR("Discount").ToString
            txtVATAmount.Text = SQL.SQLDR("VATAmount").ToString
            txtNetAmount.Text = SQL.SQLDR("NetAmount").ToString

            LoadEntry(TransID)

            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
            btnClose.Attributes("disabled") = "disabled"
            btnSave.Attributes("disabled") = "disabled"
            btnNew.Attributes.Remove("disabled")
            btnNext.Attributes.Remove("disabled")
            btnPrev.Attributes.Remove("disabled")
            btnSearch.Attributes.Remove("disabled")
            EnableControl(False)
        Else
            Initialize()
        End If
    End Sub

    Private Sub LoadEntry(ByVal PONO As Integer)
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ItemName"))
        dt.Columns.Add(New DataColumn("ItemCode"))
        dt.Columns.Add(New DataColumn("Description"))
        dt.Columns.Add(New DataColumn("UnitPrice"))
        dt.Columns.Add(New DataColumn("UOM"))
        dt.Columns.Add(New DataColumn("QTY"))
        'dt.Columns.Add(New DataColumn("UnitCost"))
        'dt.Columns.Add(New DataColumn("GrossAmount"))
        'dt.Columns.Add(New DataColumn("VATAmount"))
        'dt.Columns.Add(New DataColumn("VATInc"))
        'dt.Columns.Add(New DataColumn("DiscountRate"))
        'dt.Columns.Add(New DataColumn("Discount"))
        'dt.Columns.Add(New DataColumn("NetAmount"))
        dt.Columns.Add(New DataColumn("VATable"))
        dt.Columns.Add(New DataColumn("WHS"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        ViewState("EntryTable") = dt
        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = " SELECT  TransID, tblItem_Master.ItemName as ItemName ,tblPO_Details.ItemCode as ItemCode, Description, UnitPrice, UOM, QTY, " &
        " VATable, WHSE, AccntCode" &
        " FROM    tblPO_Details LEFT JOIN tblItem_Master " &
        " ON      tblPO_Details.itemcode = tblItem_Master.itemcode " &
        " WHERE   TransID = '" & PONO & "' "

        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("ItemCode") = SQL.SQLDR("ItemCode").ToString
            dr("ItemName") = SQL.SQLDR("ItemName").ToString
            dr("Description") = SQL.SQLDR("Description").ToString
            dr("UnitPrice") = CDec(SQL.SQLDR("UnitPrice")).ToString("N2")
            dr("UOM") = SQL.SQLDR("UOM").ToString
            dr("QTY") = CDec(SQL.SQLDR("QTY")).ToString("N2")
            dr("VATable") = If(IsNothing(SQL.SQLDR("VATable")), False, True)
            dr("WHS") = SQL.SQLDR("WHSE").ToString
            dr("AccntCode") = SQL.SQLDR("AccntCode").ToString
            dt.Rows.Add(dr)

            ViewState("EntryTable") = data
            dgvEntry.DataSource = data

            ch = ch + 1
        End While

        dgvEntry.DataBind()
        SetDataTable()
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
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblSJ  WHERE SJ_No > '" & Session("Transno") & "' ORDER BY SJ_No ASC "
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
            query = " SELECT Top 1 TransID FROM tblSJ  WHERE SJ_No < '" & Session("Transno") & "' ORDER BY SJ_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub
End Class