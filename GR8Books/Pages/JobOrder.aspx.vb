Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class JobOrder
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim JONO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "JO"
    Dim ColumnPK As String = "TransNo"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblJO"

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
                    Session("TransID") = ""
                    EnableControl(False)
                End If
                dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                dtpps_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
                dtptc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
            End If
        End If
    End Sub

    Public Sub LoadDatagrid()

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ProjectName"))
        dt.Columns.Add(New DataColumn("Location"))
        dt.Columns.Add(New DataColumn("Services"))
        dt.Columns.Add(New DataColumn("ProjectCost"))

        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("ProjectName") = Nothing
        dr("Location") = Nothing
        dr("Services") = Nothing
        dr("ProjectCost") = "0.00"

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
                    Dim txtProjectName As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtProjectName_Entry")
                    Dim txtLocation As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtLocation_Entry")
                    Dim txtServices As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtServices_Entry")
                    Dim txtProjectCost As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtProjectCost_Entry")


                    dr = dt.NewRow
                    dr("chNo") = i + 2

                    dt.Rows(i)("ProjectName") = txtProjectName.Text
                    dt.Rows(i)("Location") = txtLocation.Text
                    dt.Rows(i)("Services") = txtServices.Text
                    dt.Rows(i)("ProjectCost") = txtProjectCost.Text

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
                    Dim txtProjectName As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtProjectName_Entry")
                    Dim txtLocation As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtLocation_Entry")
                    Dim txtServices As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtServices_Entry")
                    Dim txtProjectCost As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtProjectCost_Entry")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtProjectName.Text = dt.Rows(i)("ProjectName").ToString
                    txtLocation.Text = dt.Rows(i)("Location").ToString
                    txtServices.Text = dt.Rows(i)("Services").ToString
                    txtProjectCost.Text = dt.Rows(i)("ProjectCost").ToString

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
                    Dim txtProjectName As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtProjectName_Entry")
                    Dim txtLocation As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtLocation_Entry")
                    Dim txtServices As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtServices_Entry")
                    Dim txtProjectCost As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtProjectCost_Entry")

                    dt.Rows(i)("ProjectName") = txtProjectName.Text
                    dt.Rows(i)("Location") = txtLocation.Text
                    dt.Rows(i)("Services") = txtServices.Text
                    dt.Rows(i)("ProjectCost") = txtProjectCost.Text

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
        txtRemarks.Text = ""
        txtRef_No.Text = ""
        txtTrans_Num.Text = ""
        txtTotalAmount.Text = ""
        dtpDoc_Date.Value = Now.Date
        dtpps_Date.Value = Now.Date
        dtptc_Date.Value = Now.Date
        LoadDatagrid()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        panelEntry.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        dtpps_Date.Disabled = Not Value
        dtptc_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub JobOrder_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        JONO = ""
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
        dtpps_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
        dtptc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If JONO = "" Then
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
            'If Session("TransID") = "" Then
            'Session("TransID") = ""
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    JONO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    JONO = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = JONO
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    JONO = txtTrans_Num.Text
                    UpdateCV()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    JONO = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        JONO = txtTrans_Num.Text
                        UpdateCV()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        JONO = txtTrans_Num.Text
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
        'insertSQL = " INSERT INTO " &
        '                " tblJO (TransID, TransNo, VCECode, TransDate,  ProjectStartDate, TargetCompletionDate, TotalAmount, Reference, Remarks, Status, DateCreated, TransAuto ) " &
        '                " VALUES (@TransID, @TransNo, @VCECode, @TransDate, @ProjectStartDate,  @TargetCompletionDate, @TotalAmount, @Reference, @Remarks,  @Status, @WhoCreated, @TransAuto)"
        insertSQL = " INSERT INTO " &
                    " tblJO (TransID, TransNo, VCECode, TransDate,  ProjectStartDate, TargetCompletionDate, TotalAmount, Reference, Remarks, Status, WhoCreated, TransAuto) " &
                    " VALUES (@TransID, @TransNo, @VCECode, @TransDate, @ProjectStartDate, @TargetCompletionDate, @TotalAmount, @Reference, @Remarks,  @Status, @WhoCreated, @TransAuto)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@TransNo", JONO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@ProjectStartDate", dtpps_Date.Value)
        SQL.AddParam("@TargetCompletionDate", dtptc_Date.Value)
        SQL.AddParam("@TotalAmount", txtTotalAmount.Text)
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@TransAuto", TransAuto)
        'SQL.AddParam("@DateCreated", Date.Now)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

        'For Accnt Entry only not needed here
        JETransiD = LoadJE("JO", TransID)

        Dim strRefNo As String = ""
        Dim line As Integer = 1
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtProjectName As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtProjectName_Entry")
                    Dim txtLocation As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtLocation_Entry")
                    Dim txtServices As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtServices_Entry")
                    Dim txtProjectCost As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtProjectCost_Entry")

                    If txtProjectName.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblJO_Details(TransID, ProjectName, Location, Services, ProjectCost, LineNum) " &
                                " VALUES(@TransID, @ProjectName, @Location, @Services, @ProjectCost, @LineNum)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", TransID)
                        If txtProjectName.Text <> Nothing AndAlso txtProjectName.Text <> "" Then
                            SQL.AddParam("@ProjectName", txtProjectName.Text)
                        Else
                            SQL.AddParam("@ProjectName", txtProjectName.Text)
                        End If

                        If txtLocation.Text <> Nothing AndAlso txtLocation.Text <> "" Then
                            SQL.AddParam("@Location", txtLocation.Text)
                        Else
                            SQL.AddParam("@Location", txtLocation.Text)
                        End If

                        If txtServices.Text <> Nothing AndAlso txtServices.Text <> "" Then
                            SQL.AddParam("@Services", txtServices.Text)
                        Else
                            SQL.AddParam("@Services", txtServices.Text)
                        End If

                        If txtProjectCost.Text <> Nothing AndAlso IsNumeric(txtProjectCost.Text) Then
                            SQL.AddParam("@ProjectCost", CDec(txtProjectCost.Text))
                        Else
                            SQL.AddParam("@ProjectCost", 0)
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
        updateSQL = " UPDATE tblJO  " &
                        " SET    TransNo = @TransNo, VCECode = @VCECode, TransDate = @TransDate , ProjectStartDate = @ProjectStartDate, " &
                        " TargetCompletionDate = @TargetCompletionDate ,TotalAmount = @TotalAmount, Reference = @Reference, Remarks = @Remarks, " &
                        " TransAuto = @TransAuto , WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@TransNo", JONO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@ProjectStartDate", dtpps_Date.Value)
        SQL.AddParam("@TargetCompletionDate", dtptc_Date.Value)
        SQL.AddParam("@TotalAmount", txtTotalAmount.Text)
        SQL.AddParam("@Reference", txtRef_No.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

        'Delete DR Details
        delSQL = " DELETE FROM tblJO_Details " &
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
                    Dim txtProjectName As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtProjectName_Entry")
                    Dim txtLocation As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtLocation_Entry")
                    Dim txtServices As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtServices_Entry")
                    Dim txtProjectCost As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtProjectCost_Entry")

                    If txtProjectName.Text <> Nothing Then
                        insertSQL = " INSERT INTO " &
                                " tblJO_Details(TransID, ProjectName, Location, Services, ProjectCost, LineNum) " &
                                " VALUES(@TransID, @ProjectName, @Location, @Services, @ProjectCost, @LineNum)"
                        SQL.FlushParams()
                        SQL.AddParam("@TransID", Session("TransID").ToString)
                        If txtProjectName.Text <> Nothing AndAlso txtProjectName.Text <> "" Then
                            SQL.AddParam("@ProjectName", txtProjectName.Text)
                        Else
                            SQL.AddParam("@ProjectName", txtProjectName.Text)
                        End If

                        If txtLocation.Text <> Nothing AndAlso txtLocation.Text <> "" Then
                            SQL.AddParam("@Location", txtLocation.Text)
                        Else
                            SQL.AddParam("@Location", txtLocation.Text)
                        End If

                        If txtServices.Text <> Nothing AndAlso IsNumeric(txtServices.Text) Then
                            SQL.AddParam("@Services", CDec(txtServices.Text))
                        Else
                            SQL.AddParam("@Services", 0)
                        End If

                        If txtProjectCost.Text <> Nothing AndAlso IsNumeric(txtProjectCost.Text) Then
                            SQL.AddParam("@ProjectCost", CDec(txtProjectCost.Text))
                        Else
                            SQL.AddParam("@ProjectCost", 0)
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
        query = " SELECT * FROM tblJO WHERE TransNo ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  TransID, TransNo, tblJO.VCECode, TransDate, ProjectStartDate, TargetCompletionDate, TotalAmount, ISNULL(Reference,0) as Reference," &
                "         Remarks, tblJO.Status " &
                " FROM    tblJO LEFT JOIN View_VCEMMaster " &
                " ON      tblJO.VCECode = View_VCEMMaster.Code " &
                " WHERE   TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            JONO = SQL.SQLDR("TransNo").ToString
            Session("Transno") = SQL.SQLDR("TransNo").ToString
            txtTrans_Num.Text = JONO
            dtpDoc_Date.Value = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            dtpps_Date.Value = CDate(SQL.SQLDR("ProjectStartDate")).ToString("yyyy-MM-dd")
            dtptc_Date.Value = CDate(SQL.SQLDR("TargetCompletionDate")).ToString("yyyy-MM-dd")
            txtTotalAmount.Text = SQL.SQLDR("TotalAmount").ToString
            txtRef_No.Text = SQL.SQLDR("Reference").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtCode.Text = SQL.SQLDR("VCECode").ToString
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

    Private Sub LoadEntry(ByVal JONO As Integer)
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("ProjectName"))
        dt.Columns.Add(New DataColumn("Location"))
        dt.Columns.Add(New DataColumn("Services"))
        dt.Columns.Add(New DataColumn("ProjectCost"))
        ViewState("EntryTable") = dt
        dgvEntry.DataSource = dt
        dgvEntry.DataBind()

        Dim query As String
        query = " SELECT TransID, ProjectName, Location, Services, ProjectCost " &
                " FROM tblJO_Details" &
                " WHERE   TransID = '" & JONO & "' "

        SQL.ReadQuery(query)
        Dim ch As Integer = 1

        While SQL.SQLDR.Read

            Dim data As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = ch
            dr("ProjectName") = SQL.SQLDR("ProjectName").ToString
            dr("Location") = SQL.SQLDR("Location").ToString
            dr("Services") = SQL.SQLDR("Services").ToString
            dr("ProjectCost") = CDec(SQL.SQLDR("ProjectCost")).ToString("N2")

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
            query = " SELECT Top 1 TransID FROM tblJO  WHERE TransNo > '" & Session("Transno") & "' ORDER BY TransNO ASC "
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
            query = " SELECT Top 1 TransID FROM tblJO  WHERE TransNo < '" & Session("Transno") & "' ORDER BY TransNO DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
            End If
        End If
    End Sub

    Private Function If_TransID_Exist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblJO WHERE TransID ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

End Class