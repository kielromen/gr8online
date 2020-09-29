Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Public Class DeliveryReceipt
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim DRNO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "DR"
    Dim ColumnPK As String = "TransNo"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblDR"

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
                    Session("TransID") = ""
                    EnableControl(False)
                End If
                dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                dtpDel_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
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
        dt.Columns.Add(New DataColumn("DeliveryAmount"))
        dt.Columns.Add(New DataColumn("GrossAmount"))
        dt.Columns.Add(New DataColumn("NetAmount"))
        dt.Columns.Add(New DataColumn("WHSE"))
        dt.Columns.Add(New DataColumn("Reference"))

        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("AccntCode") = Nothing
        dr("AccntTitle") = Nothing
        dr("UOM") = Nothing
        dr("QTY") = "0"
        dr("UnitPrice") = "0.00"
        dr("DeliveryAmount") = "0"
        dr("GrossAmount") = "0.00"
        dr("NetAmount") = "0.00"
        dr("WHSE") = Nothing
        dr("Reference") = Nothing

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
                    Dim txtDeliveryAmount As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtDeliveryAmount_Entry")
                    Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtGrossAmount_Entry")
                    Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtNetAmount_Entry")
                    Dim txtWHSE As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtWHSE_Entry")
                    Dim txtReference As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtReference_Entry")

                    dr = dt.NewRow
                    dr("chNo") = i + 2

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text
                    dt.Rows(i)("DeliveryAmount") = txtDeliveryAmount.Text
                    dt.Rows(i)("GrossAmount") = txtGrossAmount.Text
                    dt.Rows(i)("NetAmount") = txtNetAmount.Text
                    dt.Rows(i)("WHSE") = txtWHSE.Text
                    dt.Rows(i)("Reference") = txtReference.Text
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
                    Dim txtDeliveryAmount As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtDeliveryAmount_Entry")
                    Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtGrossAmount_Entry")
                    Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtNetAmount_Entry")
                    Dim txtWHSE As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtWHSE_Entry")
                    Dim txtReference As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtReference_Entry")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtAccntCode.Text = dt.Rows(i)("AccntCode").ToString
                    txtAccntTitle.Text = dt.Rows(i)("AccntTitle").ToString
                    txtUOM.Text = dt.Rows(i)("UOM").ToString
                    txtQTY.Text = dt.Rows(i)("QTY").ToString
                    txtUnitPrice.Text = dt.Rows(i)("UnitPrice").ToString
                    txtDeliveryAmount.Text = dt.Rows(i)("DeliveryAmount").ToString
                    txtGrossAmount.Text = dt.Rows(i)("GrossAmount").ToString
                    txtNetAmount.Text = dt.Rows(i)("NetAmount").ToString
                    txtWHSE.Text = dt.Rows(i)("WHSE").ToString
                    txtReference.Text = dt.Rows(i)("Reference").ToString



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
                    Dim txtDeliveryAmount As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtDeliveryAmount_Entry")
                    Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtGrossAmount_Entry")
                    Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtNetAmount_Entry")
                    Dim txtWHSE As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtWHSE_Entry")
                    Dim txtReference As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtReference_Entry")

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("UOM") = txtUOM.Text
                    dt.Rows(i)("QTY") = txtQTY.Text
                    dt.Rows(i)("UnitPrice") = txtUnitPrice.Text
                    dt.Rows(i)("DeliveryAmount") = txtDeliveryAmount.Text
                    dt.Rows(i)("GrossAmount") = txtGrossAmount.Text
                    dt.Rows(i)("NetAmount") = txtNetAmount.Text
                    dt.Rows(i)("WHSE") = txtWHSE.Text
                    dt.Rows(i)("Reference") = txtReference.Text

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
        txtPlateNo.Text = ""
        txtDriverName.Text = ""
        txtRemarks.Text = ""
        txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        dtpDoc_Date.Value = Now.Date
        dtpDel_Date.Value = Now.Date
        LoadDatagrid()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        panelEntry.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        dtpDel_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        DRNO = ""
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
        If DRNO = "" Then
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
                DRNO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            Else
                DRNO = txtTrans_Num.Text
            End If
            txtTrans_Num.Text = DRNO
            Save()
            Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
            LoadTransaction(TransID)
        Else
            If Session("TransNo") = txtTrans_Num.Text Then
                DRNO = txtTrans_Num.Text
                UpdateCV()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                DRNO = txtTrans_Num.Text
                LoadTransaction(Session("TransID"))
            Else
                If Not IfExist(txtTrans_Num.Text) Then
                    DRNO = txtTrans_Num.Text
                    UpdateCV()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    DRNO = txtTrans_Num.Text
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
                        " tblDR (TransID, TransNo, VCECode, DateTrans,  DateDeliver, Remarks, PlateNumber, DriverName, Reference, WhoCreated, TransAuto, Status ) " &
                        " VALUES (@TransID, @TransNo, @VCECode, @DateTrans, @DateDeliver,  @Remarks, @PlateNumber, @DriverName, @Reference,  @WhoCreated, @TransAuto, @Status)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@TransNo", DRNO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@DateTrans", dtpDoc_Date.Value)
        SQL.AddParam("@DateDeliver", dtpDel_Date.Value)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@PlateNumber", txtPlateNo.Text)
        SQL.AddParam("@DriverName", txtDriverName.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        'For Accnt Entry only not needed here
        JETransiD = LoadJE("DR", TransID)

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
                    Dim txtDeliveryAmount As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtDeliveryAmount_Entry")
                    Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtGrossAmount_Entry")
                    Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtNetAmount_Entry")
                    Dim txtWHSE As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtWHSE_Entry")
                    Dim txtReference As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtReference_Entry")

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblDR_Details(TransID, ItemCode, UOM, QTY, UnitPrice, DeliveryAmount, GrossAmount, NetAmount, WHSE, LineNum) " &
                                " VALUES(@TransID, @ItemCode, @UOM, @QTY, @UnitPrice, @DeliveryAmount, @GrossAmount, @NetAmount, @WHSE, @LineNum)"
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
                        If txtDeliveryAmount.Text <> Nothing AndAlso IsNumeric(txtDeliveryAmount.Text) Then
                            SQL.AddParam("@DeliveryAmount", CDec(txtDeliveryAmount.Text))
                        Else
                            SQL.AddParam("@DeliveryAmount", 0)
                        End If
                        If txtGrossAmount.Text <> Nothing AndAlso IsNumeric(txtGrossAmount.Text) Then
                            SQL.AddParam("@GrossAmount", CDec(txtGrossAmount.Text))
                        Else
                            SQL.AddParam("@GrossAmount", 0)
                        End If
                        If txtNetAmount.Text <> Nothing AndAlso IsNumeric(txtNetAmount.Text) Then
                            SQL.AddParam("@NetAmount", CDec(txtNetAmount.Text))
                        Else
                            SQL.AddParam("@NetAmount", 0)
                        End If
                        If txtWHSE.Text <> Nothing AndAlso txtWHSE.Text <> "" Then
                            SQL.AddParam("@WHSE", txtWHSE.Text)
                        Else
                            SQL.AddParam("@WHSE", txtWHSE.Text)
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
        updateSQL = " UPDATE tblDR  " &
                        " SET    TransNo = @TransNo, VCECode = @VCECode, DateTrans = @DateTrans , DateDeliver = @DateDeliver, Remarks = @Remarks, PlateNumber = @PlateNumber," &
                        " DriverName = @DriverName , Reference = @Reference, TransAuto = @TransAuto ," &
                        " WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@TransNo", DRNO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@DateTrans", dtpDoc_Date.Value)
        SQL.AddParam("@DateDeliver", dtpDel_Date.Value)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@PlateNumber", txtPlateNo.Text)
        SQL.AddParam("@DriverName", txtDriverName.Text)
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        'Delete DR Details
        delSQL = " DELETE FROM tblDR_Details " &
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
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtUOM_Entry")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtQTY_Entry")
                    Dim txtUnitPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtUnitPrice_Entry")
                    Dim txtDeliveryAmount As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtDeliveryAmount_Entry")
                    Dim txtGrossAmount As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtGrossAmount_Entry")
                    Dim txtNetAmount As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtNetAmount_Entry")
                    Dim txtWHSE As TextBox = dgvEntry.Rows(i).Cells(10).FindControl("txtWHSE_Entry")
                    Dim txtReference As TextBox = dgvEntry.Rows(i).Cells(11).FindControl("txtReference_Entry")

                    If txtAccntCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblDR_Details(TransID, ItemCode, UOM, QTY, UnitPrice, DeliveryAmount, GrossAmount, NetAmount, WHSE, LineNum) " &
                                " VALUES(@TransID, @ItemCode, @UOM, @QTY, @UnitPrice, @DeliveryAmount, @GrossAmount, @NetAmount, @WHSE, @LineNum)"
                        SQL.FlushParams()
                        'SQL.AddParam("@ID", TransID)
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
                        If txtDeliveryAmount.Text <> Nothing AndAlso IsNumeric(txtDeliveryAmount.Text) Then
                            SQL.AddParam("@DeliveryAmount", CDec(txtDeliveryAmount.Text))
                        Else
                            SQL.AddParam("@DeliveryAmount", 0)
                        End If
                        If txtGrossAmount.Text <> Nothing AndAlso IsNumeric(txtGrossAmount.Text) Then
                            SQL.AddParam("@GrossAmount", CDec(txtGrossAmount.Text))
                        Else
                            SQL.AddParam("@GrossAmount", 0)
                        End If
                        If txtNetAmount.Text <> Nothing AndAlso IsNumeric(txtNetAmount.Text) Then
                            SQL.AddParam("@NetAmount", CDec(txtNetAmount.Text))
                        Else
                            SQL.AddParam("@NetAmount", 0)
                        End If
                        If txtWHSE.Text <> Nothing AndAlso txtWHSE.Text <> "" Then
                            SQL.AddParam("@WHSE", txtWHSE.Text)
                        Else
                            SQL.AddParam("@WHSE", txtWHSE.Text)
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
        query = " SELECT * FROM tblDR WHERE TransNo ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, TransNo, tblDR.VCECode, DateTrans, DateDeliver, Remarks, PlateNumber, DriverName, ISNULL(Reference,0) as Reference," &
                "         tblDR.Status " &
                " FROM    tblDR LEFT JOIN View_VCEMMaster " &
                " ON      tblDR.VCECode = View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            DRNO = SQL.SQLDR("TransNo").ToString
            Session("Transno") = SQL.SQLDR("TransNo").ToString
            txtTrans_Num.Text = DRNO
            txtRef_No.Text = SQL.SQLDR("Reference").ToString
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("DateTrans")).ToString("yyyy-MM-dd")
            dtpDel_Date.Value = CDate(SQL.SQLDR("DateDeliver")).ToString("yyyy-MM-dd")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtPlateNo.Text = SQL.SQLDR("PlateNumber").ToString
            txtDriverName.Text = SQL.SQLDR("DriverName").ToString
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

    Private Sub LoadEntry(ByVal DRNO As Integer)
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("UOM"))
        dt.Columns.Add(New DataColumn("QTY"))
        dt.Columns.Add(New DataColumn("UnitPrice"))
        dt.Columns.Add(New DataColumn("DeliveryAmount"))
        dt.Columns.Add(New DataColumn("GrossAmount"))
        dt.Columns.Add(New DataColumn("NetAmount"))
        dt.Columns.Add(New DataColumn("WHSE"))
        dt.Columns.Add(New DataColumn("Reference"))
        ViewState("EntryTable") = dt
        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = " SELECT  TransID, tblItem_Master.ItemName as ItemName ,tblDR_Details.ItemCode as ItemCode, UOM, QTY, UnitPrice, " &
        " DeliveryAmount, GrossAmount, NetAmount, WHSE, Reference" &
        " FROM    tblDR_Details LEFT JOIN tblItem_Master " &
        " ON      tblDR_Details.itemcode = tblItem_Master.itemcode " &
        " WHERE   TransID = '" & DRNO & "' "

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
            dr("DeliveryAmount") = CDec(SQL.SQLDR("DeliveryAmount")).ToString("N2")
            dr("GrossAmount") = CDec(SQL.SQLDR("GrossAmount")).ToString("N2")
            dr("NetAmount") = CDec(SQL.SQLDR("NetAmount")).ToString("N2")
            dr("WHSE") = SQL.SQLDR("WHSE").ToString
            dr("Reference") = SQL.SQLDR("Reference").ToString
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
            query = " SELECT Top 1 TransID FROM tblDR  WHERE TransNo > '" & Session("Transno") & "' ORDER BY TransNO ASC "
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
            query = " SELECT Top 1 TransID FROM tblDR  WHERE TransNo < '" & Session("Transno") & "' ORDER BY TransNO DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub

    Private Sub DeliveryReceipt_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub
End Class