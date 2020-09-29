Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient

Public Class SO_Entry
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim SO_No As String
    Dim TransID, JETransID As String
    Dim ModuleID As String = "SO"
    Dim ColumnPK As String = "SO_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblSO"
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
                    btnClose.Attributes("disabled") = "disabled"
                    EnableControl(False)
                End If
                dtpDoc_Date.Value = Format(Date.Now, "yyyy-MM-dd")

            End If
        End If
    End Sub
    Private Sub EnableControl(ByVal Value As Boolean)

        dtpDoc_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub
    Private Sub Initialize()
        Session("Transno") = ""
        Session("TransID") = ""
        txtRemarks.Text = ""
        LoadDatagrid()

    End Sub
    Public Sub LoadDatagrid()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ItemCode"))
        dt.Columns.Add(New DataColumn("ItemDesc"))
        dt.Columns.Add(New DataColumn("ItemUOM_QTY"))
        dt.Columns.Add(New DataColumn("ItemUOM"))
        dt.Columns.Add(New DataColumn("ItemPrice"))
        dt.Columns.Add(New DataColumn("TotalPrice"))
        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("ItemCode") = Nothing
        dr("ItemDesc") = Nothing
        dr("ItemUOM_QTY") = Nothing
        dr("ItemUOM") = Nothing
        dr("ItemPrice") = Nothing
        dr("TotalPrice") = Nothing
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
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtItemCode")
                    Dim txtItemDesc As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtItemDesc")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtQTY")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUOM")
                    Dim txtItemPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtItemPrice")
                    Dim txtTotalPrice As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtTotalPrice")
                    dr = dt.NewRow
                    dr("chNo") = i + 2


                    dt.Rows(i)("ItemCode") = txtItemCode.Text
                    dt.Rows(i)("ItemDesc") = txtItemDesc.Text
                    dt.Rows(i)("ItemUOM_QTY") = txtQTY.Text
                    dt.Rows(i)("ItemUOM") = txtUOM.Text
                    dt.Rows(i)("ItemPrice") = txtItemPrice.Text
                    dt.Rows(i)("TotalPrice") = txtTotalPrice.Text

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
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtItemCode")
                    Dim txtItemDesc As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtItemDesc")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtQTY")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUOM")
                    Dim txtItemPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtItemPrice")
                    Dim txtTotalPrice As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtTotalPrice")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtItemCode.Text = dt.Rows(i)("ItemCode").ToString
                    txtItemDesc.Text = dt.Rows(i)("ItemDesc").ToString
                    txtQTY.Text = dt.Rows(i)("ItemUOM_QTY").ToString
                    txtUOM.Text = dt.Rows(i)("ItemUOM").ToString
                    txtItemPrice.Text = dt.Rows(i)("ItemPrice").ToString
                    txtTotalPrice.Text = dt.Rows(i)("TotalPrice").ToString

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
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtItemCode")
                    Dim txtItemDesc As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtItemDesc")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtQTY")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUOM")
                    Dim txtItemPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtItemPrice")
                    Dim txtTotalPrice As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtTotalPrice")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtItemCode.Text = dt.Rows(i)("ItemCode").ToString
                    txtItemDesc.Text = dt.Rows(i)("ItemDesc").ToString
                    txtQTY.Text = dt.Rows(i)("ItemUOM_QTY").ToString
                    txtUOM.Text = dt.Rows(i)("ItemUOM").ToString
                    txtItemPrice.Text = dt.Rows(i)("ItemPrice").ToString
                    txtTotalPrice.Text = dt.Rows(i)("TotalPrice").ToString

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
        Dim ItemName As New List(Of String)()
        Dim query As String
        query = "SELECT ItemCode, ItemName, ItemUOM, ItemPrice  FROM tblItem_Master " & vbCrLf &
                "WHERE  ItemName LIKE '%' + @ItemName + '%'"
        SQL.FlushParams()
        SQL.AddParam("@ItemName", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            ItemName.Add(String.Format("{0}--{1}--{2}--{3}", SQL.SQLDR("ItemName"), SQL.SQLDR("ItemCode"), SQL.SQLDR("ItemUOM"), SQL.SQLDR("ItemPrice")))
        End While
        Return ItemName.ToArray()
    End Function

    <WebMethod()>
    Public Shared Function ListVCE(prefix As String) As String()
        Dim strName As New List(Of String)()
        Dim query As String
        query = "SELECT Code, Name FROM View_VCEMMaster " & vbCrLf &
                "WHERE Status = 'Active' AND Name LIKE '%' + @Name + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Name", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            strName.Add(String.Format("{0}--{1}", SQL.SQLDR("Name"), SQL.SQLDR("Code")))
        End While
        Return strName.ToArray()
    End Function
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        SO_No = ""
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            For Each rows As GridViewRow In dgvEntry.Rows
                Dim AccntCode As TextBox
                AccntCode = rows.FindControl("txtItemCode")
                If AccntCode.Text = "" Then
                    Response.Write("<script>alert('Invalid Item Code!');</script>")
                    Exit Sub
                End If
            Next
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    SO_No = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    SO_No = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = SO_No
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    SO_No = txtTrans_Num.Text
                    Update()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    SO_No = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        SO_No = txtTrans_Num.Text
                        Update()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        SO_No = txtTrans_Num.Text
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
                        " tblSO (TransID, SO_No,  VCECode, DateSO, GrossAmount, Remarks, WhoCreated, TransAuto ) " &
                        " VALUES (@TransID, @SO_No, @VCECode, @DateSO, @GrossAmount, @Remarks,  @WhoCreated, @TransAuto)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@SO_No", SO_No)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@DateSO", dtpDoc_Date.Value)
        SQL.AddParam("@GrossAmount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtItemCode")
                    Dim txtItemDesc As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtItemDesc")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtQTY")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUOM")
                    Dim txtItemPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtItemPrice")
                    Dim txtTotalPrice As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtTotalPrice")


                    If txtItemCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblSO_Details (TransID, ItemCode, UOM, QTY, UnitPrice, GrossAmount , LineNum , Status) " &
                                " VALUES (@TransID, @ItemCode, @UOM, @QTY,  @UnitPrice, @GrossAmount, @LineNum, @Status)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        SQL.AddParam("@ItemCode", txtItemCode.Text)
                        If txtCode.Text <> Nothing AndAlso txtCode.Text <> "" Then
                            SQL.AddParam("@VCECode", txtCode.Text)
                        Else
                            SQL.AddParam("@VCECode", txtCode.Text)
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
                        If txtItemPrice.Text <> Nothing AndAlso txtItemPrice.Text <> "" Then
                            SQL.AddParam("@UnitPrice", txtItemPrice.Text)
                        Else
                            SQL.AddParam("@UnitPrice", "")
                        End If
                        If txtTotalPrice.Text <> Nothing AndAlso txtTotalPrice.Text <> "" Then
                            SQL.AddParam("@GrossAmount", txtTotalPrice.Text)
                        Else
                            SQL.AddParam("@GrossAmount", "")
                        End If

                        SQL.AddParam("@LineNum", line)
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
        query = " SELECT * FROM tblSO WHERE SO_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub Update()
        Dim insertSQL, updateSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblSO  " &
                        " SET    SO_No = @SO_No,  VCECode = @VCECode, DateSO = @DateSO, " &
                        "        GrossAmount = @GrossAmount, Remarks = @Remarks, WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID"))
        SQL.AddParam("@SO_No", SO_No)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@DateSO", dtpDoc_Date.Value)
        SQL.AddParam("@GrossAmount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        'UPDATE ACCOUNTING ENTRIES
        updateSQL = " UPDATE tblSO_Details SET Status = 'Inactive'  WHERE  TransID = @Transid "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.ExecNonQuery(updateSQL)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtItemCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtItemCode")
                    Dim txtItemDesc As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtItemDesc")
                    Dim txtQTY As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtQTY")
                    Dim txtUOM As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtUOM")
                    Dim txtItemPrice As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtItemPrice")
                    Dim txtTotalPrice As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtTotalPrice")


                    If txtItemCode.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblSO_Details (TransID, ItemCode, UOM, QTY, UnitPrice, GrossAmount , LineNum , Status) " &
                                " VALUES (@TransID, @ItemCode, @UOM, @QTY,  @UnitPrice, @GrossAmount, @LineNum, @Status)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", Session("TransID"))
                        SQL.AddParam("@ItemCode", txtItemCode.Text)
                        If txtCode.Text <> Nothing AndAlso txtCode.Text <> "" Then
                            SQL.AddParam("@VCECode", txtCode.Text)
                        Else
                            SQL.AddParam("@VCECode", txtCode.Text)
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
                        If txtItemPrice.Text <> Nothing AndAlso txtItemPrice.Text <> "" Then
                            SQL.AddParam("@UnitPrice", txtItemPrice.Text)
                        Else
                            SQL.AddParam("@UnitPrice", "")
                        End If
                        If txtTotalPrice.Text <> Nothing AndAlso txtTotalPrice.Text <> "" Then
                            SQL.AddParam("@GrossAmount", txtTotalPrice.Text)
                        Else
                            SQL.AddParam("@GrossAmount", "")
                        End If

                        SQL.AddParam("@LineNum", line)
                        SQL.AddParam("@Status", "Active")
                        SQL.ExecNonQuery(insertSQL)
                        line += 1
                    End If
                Next
            End If
        End If

    End Sub
    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, SO_No, VCECode, View_VCEMMaster.Name as Name, GrossAmount, DateSO,  Remarks, " &
                " tblSO.Status " &
                " FROM    tblSO " &
                " INNER JOIN View_VCEMMaster ON tblSO.VCECode =  View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            SO_No = SQL.SQLDR("SO_No").ToString
            Session("Transno") = SQL.SQLDR("SO_No").ToString
            txtTrans_Num.Text = SO_No
            txtAmount.Text = SQL.SQLDR("GrossAmount").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("DateSO")).ToString("yyyy-MM-dd")
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("Name").ToString

            LoadEntry(Session("TransID"))

            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
            btnClose.Attributes("disabled") = "disabled"
            btnSave.Attributes("disabled") = "disabled"
            btnNew.Attributes.Remove("disabled")
            btnSearch.Attributes.Remove("disabled")
            EnableControl(False)
        Else
            Initialize()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ' Toolstrip Buttons
        If SO_No = "" Then
            Initialize()
            EnableControl(False)
            btnEdit.Attributes("disabled") = "disabled"
            btnCancel.Attributes("disabled") = "disabled"
        Else
            Initialize()
            LoadTransaction(TransID)
            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
        End If
        btnSearch.Attributes.Remove("disabled")
        btnNew.Attributes.Remove("disabled")
        btnSave.Attributes("disabled") = "disabled"
        btnClose.Attributes("disabled") = "disabled"
    End Sub


    Private Sub ClearText()
        txtCode.Text = ""
        txtName.Text = ""
        txtAmount.Text = ""
        txtRemarks.Text = ""

        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ItemCode"))
        dt.Columns.Add(New DataColumn("ItemDesc"))
        dt.Columns.Add(New DataColumn("ItemUOM_QTY"))
        dt.Columns.Add(New DataColumn("ItemUOM"))
        dt.Columns.Add(New DataColumn("ItemPrice"))
        dt.Columns.Add(New DataColumn("TotalPrice"))
        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("ItemCode") = Nothing
        dr("ItemDesc") = Nothing
        dr("ItemUOM_QTY") = Nothing
        dr("ItemUOM") = Nothing
        dr("ItemPrice") = Nothing
        dr("TotalPrice") = Nothing
        dt.Rows.Add(dr)


        ViewState("EntryTable") = dt

        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

    End Sub

    Private Sub LoadEntry(ByVal SO_No As Integer)

        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ItemCode"))
        dt.Columns.Add(New DataColumn("ItemDesc"))
        dt.Columns.Add(New DataColumn("ItemUOM_QTY"))
        dt.Columns.Add(New DataColumn("ItemUOM"))
        dt.Columns.Add(New DataColumn("ItemPrice"))
        dt.Columns.Add(New DataColumn("TotalPrice"))

        ViewState("EntryTable") = dt

        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = "  SELECT tblSO_Details.ItemCode, ItemName, QTY, UOM, UnitPrice, GrossAmount  from tblSO_Details " &
                " INNER JOIN tblItem_Master ON " &
                " tblItem_Master.ItemCode = tblSO_Details.ItemCode " &
                "  WHERE tblSO_Details.TransID = '" & SO_No & "' AND tblSO_Details.Status = 'Active'   " &
                "  ORDER BY LineNum  "
        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("ItemCode") = SQL.SQLDR("ItemCode").ToString
            dr("ItemDesc") = SQL.SQLDR("ItemName").ToString
            dr("ItemUOM_QTY") = SQL.SQLDR("QTY").ToString
            dr("ItemUOM") = SQL.SQLDR("UOM").ToString
            dr("ItemPrice") = CDec(SQL.SQLDR("UnitPrice")).ToString("N2")
            dr("TotalPrice") = CDec(SQL.SQLDR("GrossAmount")).ToString("N2")
            dt.Rows.Add(dr)

            ViewState("EntryTable") = data
            dgvEntry.DataSource = data

            ch = ch + 1
        End While

        dgvEntry.DataBind()
        SetDataTable()
    End Sub
    Private Sub JournalVoucher_Entry_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub
    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableControl(True)
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
    End Sub
End Class