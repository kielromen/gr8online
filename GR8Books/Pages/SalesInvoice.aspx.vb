Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Public Class SalesInvoice
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim SINO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "SI"
    Dim ColumnPK As String = "TransNo"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblSI"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID) 'CANT GENERATE ERROR
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
                    Session("TransID") = ""
                    EnableControl(False)
                End If
                dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
            End If
        End If
    End Sub

    Public Sub LoadDatagrid()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("UOM"))
        dt.Columns.Add(New DataColumn("QTY"))
        dt.Columns.Add(New DataColumn("UnitPrice"))

        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("AccntCode") = Nothing
        dr("AccntTitle") = Nothing
        dr("UOM") = Nothing
        dr("QTY") = "0.00"
        dr("UnitPrice") = "0.00"

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
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")

                    dr = dt.NewRow
                    dr("chNo") = i + 2

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text



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
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtAccntCode.Text = dt.Rows(i)("AccntCode").ToString
                    txtAccntTitle.Text = dt.Rows(i)("AccntTitle").ToString
                    txtUOM.Text = dt.Rows(i)("UOM").ToString
                    txtQTY.Text = dt.Rows(i)("QTY").ToString
                    txtUnitPrice.Text = dt.Rows(i)("UnitPrice").ToString



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
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text

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
    Public Shared Function ItemList(prefix As String) As String()
        Dim ItemName As New List(Of String)()
        Dim query As String
        query = "SELECT ItemCode, ItemName FROM tblItem_master " & vbCrLf &
                "WHERE ItemName LIKE '%' + @ItemName + '%' AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@ItemName", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            ItemName.Add(String.Format("{0}--{1}", SQL.SQLDR("ItemName"), SQL.SQLDR("ItemCode")))
        End While
        Return ItemName.ToArray()
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
        txtVATAmount.Text = ""
        txtDiscount.Text = ""
        txtRemarks.Text = ""
        txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        dtpDoc_Date.Value = Now.Date
        LoadDatagrid()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        panelEntry.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub SalesJournal_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        SINO = ""
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
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If SINO = "" Then
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
        'If (Page.IsValid) Then
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
                SINO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            Else
                SINO = txtTrans_Num.Text
            End If
            txtTrans_Num.Text = SINO
            Save()
            Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
            LoadTransaction(TransID)
        Else
            If Session("TransNo") = txtTrans_Num.Text Then
                SINO = txtTrans_Num.Text
                UpdateCV()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                SINO = txtTrans_Num.Text
                LoadTransaction(Session("TransID"))
            Else
                If Not IfExist(txtTrans_Num.Text) Then
                    SINO = txtTrans_Num.Text
                    UpdateCV()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    SINO = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " already exist!');</script>")
                End If
            End If
        End If
        'End If
    End Sub

    Public Sub Save()
        Dim insertSQL As String
        activityStatus = True
        SQL.FlushParams()
        insertSQL = " INSERT INTO " &
                        " tblSI (TransID, TransNo, VCECode, DateTrans,  Reference, GrossAmount, VATAmount, Discount, Remarks, WhoCreated, TransAuto, Status ) " &
                        " VALUES (@TransID, @TransNo, @VCECode, @TransDate,  @Reference, @GrossAmount, @VATAmount, @Discount, @Remarks,  @WhoCreated, @TransAuto, @Status)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@TransNo", SINO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@GrossAmount", CDec(txtGrossAmount.Text))
        SQL.AddParam("@VATAmount", CDec(txtVATAmount.Text))
        SQL.AddParam("@Discount", CDec(txtDiscount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

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

        JETransiD = LoadJE("SI", TransID)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblSI_Details(TransID, ItemCode, UOM, QTY, UnitPrice, LineNum) " &
                                " VALUES( @TransID, @ItemCode, @UOM, @QTY, @UnitPrice,@LineNum)"
                        SQL.FlushParams()
                        'SQL.AddParam("@ID", TransID)
                        SQL.AddParam("@TransID", TransID)
                        SQL.AddParam("@ItemCode", txtAccntCode.Text)
                        If txtUOM.Text <> Nothing AndAlso txtUOM.Text <> "" Then
                            SQL.AddParam("@UOM", txtUOM.Text)
                        Else
                            SQL.AddParam("@UOM", txtUOM.Text)
                        End If
                        If txtQTY.Text <> Nothing AndAlso IsNumeric(txtQTY.Text) Then
                            SQL.AddParam("@QTY", CDec(txtQTY.Text))
                        Else
                            SQL.AddParam("@QTY", 0)
                        End If
                        If txtUnitPrice.Text <> Nothing AndAlso IsNumeric(txtUnitPrice.Text) Then
                            SQL.AddParam("@UnitPrice", CDec(txtUnitPrice.Text))
                        Else
                            SQL.AddParam("@UnitPrice", 0)
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
        updateSQL = " UPDATE tblSI  " &
                        " SET    TransNo = @TransNo, VCECode = @VCECode, Remarks = @Remarks, GrossAmount = @GrossAmount, VATAmount = @VATAmount," &
                        " Discount = @Discount ,Reference = @Reference,  DateTrans = @DateTrans, TransAuto = @TransAuto ," &
                        " WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@TransNo", SINO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@GrossAmount", CDec(txtGrossAmount.Text))
        SQL.AddParam("@VATAmount", CDec(txtVATAmount.Text))
        SQL.AddParam("@Discount", CDec(txtDiscount.Text))
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@DateTrans", dtpDoc_Date.Value)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        'Delete SI Details
        delSQL = " DELETE FROM tblSI_Details " &
                 " WHERE TransID = @TransID"
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.ExecNonQuery(delSQL)

        'Insert New SI Details
        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblSI_Details(TransID, ItemCode, UOM, QTY, UnitPrice, LineNum) " &
                                " VALUES(@TransID, @ItemCode, @UOM, @QTY, @UnitPrice,@LineNum)"
                        SQL.FlushParams()
                        'SQL.AddParam("@ID", Session("TransID").ToString)
                        SQL.AddParam("@TransID", Session("TransID").ToString)
                        SQL.AddParam("@ItemCode", txtAccntCode.Text)
                        If txtUOM.Text <> Nothing AndAlso txtUOM.Text <> "" Then
                            SQL.AddParam("@UOM", txtUOM.Text)
                        Else
                            SQL.AddParam("@UOM", txtUOM.Text)
                        End If
                        If txtQTY.Text <> Nothing AndAlso IsNumeric(txtQTY.Text) Then
                            SQL.AddParam("@QTY", CDec(txtQTY.Text))
                        Else
                            SQL.AddParam("@QTY", 0)
                        End If
                        If txtUnitPrice.Text <> Nothing AndAlso IsNumeric(txtUnitPrice.Text) Then
                            SQL.AddParam("@UnitPrice", CDec(txtUnitPrice.Text))
                        Else
                            SQL.AddParam("@UnitPrice", 0)
                        End If


                        SQL.AddParam("@LineNum", line)
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If
        'ENTRY
        'JETransiD = LoadJE("SJ", Session("TransID").ToString)
        'If JETransiD = 0 Then
        '    insertSQL = " INSERT INTO " &
        '                " tblJE_Header (AppDate, RefType, RefTransID, Book, TotalDBCR, Remarks, WhoCreated) " &
        '                " VALUES(@AppDate, @RefType, @RefTransID, @Book, @TotalDBCR, @Remarks, @WhoCreated)"
        '    SQL.FlushParams()
        '    SQL.AddParam("@AppDate", dtpDoc_Date.Value)
        '    SQL.AddParam("@RefType", "SJ")
        '    SQL.AddParam("@RefTransID", Session("TransID"))
        '    SQL.AddParam("@Book", "Sales Book")
        '    SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
        '    SQL.AddParam("@Remarks", txtRemarks.Text)
        '    SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        '    SQL.ExecNonQuery(insertSQL)

        '    JETransiD = LoadJE("SJ", Session("TransID"))
        'Else
        '    updateSQL = " UPDATE tblJE_Header " &
        '                   " SET    AppDate = @AppDate,  " &
        '                   "        RefType = @RefType, RefTransID = @RefTransID, Book = @Book, TotalDBCR = @TotalDBCR, " &
        '                   "        Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " &
        '                   " WHERE  JE_No = @JE_No "
        '    SQL.FlushParams()
        '    SQL.AddParam("@JE_No", JETransiD)
        '    SQL.AddParam("@AppDate", dtpDoc_Date.Value)
        '    SQL.AddParam("@RefType", "SJ")
        '    SQL.AddParam("@RefTransID", Session("TransID").ToString)
        '    SQL.AddParam("@Book", "Sales Book")
        '    SQL.AddParam("@TotalDBCR", CDec(txtAmount.Text))
        '    SQL.AddParam("@Remarks", txtRemarks.Text)
        '    SQL.AddParam("@WhoModified", Session("EmailAddress"))
        '    SQL.ExecNonQuery(updateSQL)
        'End If

        'updateSQL = " UPDATE tblJE_Details SET Status = 'Inactive' WHERE JE_No = @JE_No "
        'SQL.FlushParams()
        'SQL.AddParam("@JE_No", JETransiD)
        'SQL.ExecNonQuery(updateSQL)

        'Dim strRefNo As String = ""
        'Dim line As Integer = 1
        'If Not IsNothing(ViewState("EntryTable")) Then
        '    Dim dt As DataTable = ViewState("EntryTable")
        '    If dt.Rows.Count > 0 Then
        '        For i As Integer = 0 To dt.Rows.Count - 1
        '            Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
        '            Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
        '            Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
        '            Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
        '            Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
        '            Dim txtEntryCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
        '            Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
        '            Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")

        '            If txtAccntCode.Text <> Nothing Then
        '                insertSQL = " INSERT INTO " &
        '                        " tblJE_Details(JE_No, AccntCode, VCECode, Debit, Credit, Particulars, RefNo, LineNumber, Status) " &
        '                        " VALUES(@JE_No, @AccntCode, @VCECode, @Debit, @Credit, @Particulars, @RefNo, @LineNumber, @Status)"
        '                SQL.FlushParams()
        '                SQL.AddParam("@JE_No", JETransiD)
        '                SQL.AddParam("@AccntCode", txtAccntCode.Text)
        '                If txtEntryCode.Text <> Nothing AndAlso txtEntryCode.Text <> "" Then
        '                    SQL.AddParam("@VCECode", txtEntryCode.Text)
        '                Else
        '                    SQL.AddParam("@VCECode", txtCode.Text)
        '                End If
        '                If txtDebit.Text <> Nothing AndAlso IsNumeric(txtDebit.Text) Then
        '                    SQL.AddParam("@Debit", CDec(txtDebit.Text))
        '                Else
        '                    SQL.AddParam("@Debit", 0)
        '                End If
        '                If txtCredit.Text <> Nothing AndAlso IsNumeric(txtCredit.Text) Then
        '                    SQL.AddParam("@Credit", CDec(txtCredit.Text))
        '                Else
        '                    SQL.AddParam("@Credit", 0)
        '                End If
        '                If txtParticulars.Text <> Nothing AndAlso txtParticulars.Text <> "" Then
        '                    SQL.AddParam("@Particulars", txtParticulars.Text)
        '                Else
        '                    SQL.AddParam("@Particulars", "")
        '                End If
        '                If txtRefID.Text <> Nothing AndAlso txtRefID.Text <> "" Then
        '                    SQL.AddParam("@RefNo", txtRefID.Text)
        '                    If strRefNo.Length = 0 Then
        '                        strRefNo = txtRefID.Text
        '                    Else
        '                        strRefNo = strRefNo & "-" & txtRefID.Text
        '                    End If
        '                Else
        '                    SQL.AddParam("@RefNo", "")
        '                End If
        '                SQL.AddParam("@LineNumber", line)
        '                SQL.AddParam("@Status", "Active")
        '                SQL.ExecNonQuery(insertSQL)
        '                line += 1
        '            End If
        '        Next
        '    End If
        'End If
    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblSI WHERE TransNo ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, TransNo, tblSI.VCECode, DateTrans, GrossAmount, VATAmount, Discount, Remarks, " &
                "         ISNULL(Reference,0) as Reference, tblSI.Status " &
                " FROM    tblSI LEFT JOIN View_VCEMMaster " &
                " ON      tblSI.VCECode = View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            SINO = SQL.SQLDR("TransNo").ToString
            Session("Transno") = SQL.SQLDR("TransNo").ToString
            txtTrans_Num.Text = SINO
            txtRef_No.Text = SQL.SQLDR("Reference").ToString
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("DateTrans")).ToString("yyyy-MM-dd")
            txtGrossAmount.Text = SQL.SQLDR("GrossAmount").ToString
            txtVATAmount.Text = SQL.SQLDR("VATAmount").ToString
            txtDiscount.Text = SQL.SQLDR("Discount").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtName.Text = GetVCEName(SQL.SQLDR("VCECode").ToString)
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
    Public Function GetVCEName(ByVal VCECode As String) As String

        Dim query As String
        query = "SELECT Code, Name FROM View_VCEMMaster " & vbCrLf &
                "WHERE Code = @Code AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Code", VCECode)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("Name").ToString
        Else
            Return ""
        End If
    End Function

    Private Sub LoadEntry(ByVal SINO As Integer)
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("UOM"))
        dt.Columns.Add(New DataColumn("QTY"))
        dt.Columns.Add(New DataColumn("UnitPrice"))
        ViewState("EntryTable") = dt
        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = " SELECT  TransID, tblItem_Master.ItemName as ItemName,tblSI_Details.ItemCode as ItemCode, UOM, QTY, UnitPrice" &
        " FROM    tblSI_Details LEFT JOIN tblItem_Master " &
        " ON      tblSI_Details.itemcode = tblItem_Master.itemcode " &
        " WHERE   TransID = '" & SINO & "' "

        'query = " SELECT ID, TransID, View_GL.BranchCode, View_GL.AccntCode, AccountTitle, View_GL.VCECode, View_GL.VCEName, Debit, Credit, Particulars, RefNo   " &
        '        " FROM   View_GL INNER JOIN tblCOA " &
        '        " ON     View_GL.AccntCode = tblCOA.AccountCode " &
        '        " WHERE JE_No = (SELECT  JE_No FROM tblJE_Header WHERE RefType = 'SJ' AND RefTransID = " & SJNO & ") " &
        '        " ORDER BY LineNumber "
        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("AccntCode") = SQL.SQLDR("ItemCode").ToString
            dr("AccntTitle") = SQL.SQLDR("ItemName").ToString
            dr("UOM") = SQL.SQLDR("UOM").ToString
            dr("QTY") = SQL.SQLDR("QTY").ToString
            dr("UnitPrice") = CDec(SQL.SQLDR("UnitPrice")).ToString("N2")
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
            query = " SELECT Top 1 TransID FROM tblSI  WHERE TransNo > '" & Session("Transno") & "' ORDER BY TransNO ASC "
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
            query = " SELECT Top 1 TransID FROM tblSI  WHERE TransNo < '" & Session("Transno") & "' ORDER BY TransNO DESC "
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