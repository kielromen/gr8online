Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class PettyCash
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim PCNO As String
    Dim TransID, JETransiD As String
    Dim ModuleID As String = "PC"
    Dim ColumnPK As String = "PC_No"
    Dim ColumnID As String = "TransID"
    Dim DBTable As String = "tblPC"

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
                End If
                dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
            End If
        End If
    End Sub

    Public Sub Initialize()
        Session("Transno") = ""
        txtCode.Attributes.Add("readonly", "readonly")
        txtStatus.Attributes.Add("readonly", "readonly")
        txtAccntCode.Attributes.Add("readonly", "readonly")
        txtCode.Text = ""
        txtName.Text = ""
        txtStatus.Text = ""
        txtAccntCode.Text = ""
        txtAccntName.Text = ""
        txtAmount.Text = ""
        txtRemarks.Text = ""
        txtTrans_Num.Text = ""
        dtpDoc_Date.Value = Now.Date

        ddlCostCenter.Items.Clear()
        ddlCostCenter.Items.Add("")
        ddlCostCenter.DataSource = LoadCostCenter().ToArray
        ddlCostCenter.DataBind()
    End Sub

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

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        dtpDoc_Date.Disabled = Not Value
        If TransAuto Then
            txtTrans_Num.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Private Sub PettyCash_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        TransID = ""
        PCNO = ""
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
        EnableControl(True)
        txtTrans_Num.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        dtpDoc_Date.Value = CDate(Date.Now).ToString("yyyy-MM-dd")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If PCNO = "" Then
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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then

            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                If TransAuto Then
                    PCNO = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    PCNO = txtTrans_Num.Text
                End If
                txtTrans_Num.Text = PCNO
                Save()
                Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                If Session("TransNo") = txtTrans_Num.Text Then
                    PCNO = txtTrans_Num.Text
                    UpdateCA()
                    Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                    PCNO = txtTrans_Num.Text
                    LoadTransaction(Session("TransID"))
                Else
                    If Not IfExist(txtTrans_Num.Text) Then
                        PCNO = txtTrans_Num.Text
                        UpdateCA()
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " successfully updated.');</script>")
                        PCNO = txtTrans_Num.Text
                        LoadTransaction(Session("TransID"))
                    Else
                        Response.Write("<script>alert('Transaction " & txtTrans_Num.Text & " already exist!');</script>")
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub Save()
        Dim CostID = GetCostCenterID(ddlCostCenter.SelectedValue)
        Dim insertSQL As String
        activityStatus = True
        SQL.FlushParams()
        insertSQL = " INSERT INTO " &
                        " tblPC (TransID, PC_No, VCECode, TransDate, AccountCode, Amount,  Remarks, CostID, Status, WhoCreated, TransAuto ) " &
                        " VALUES (@TransID, @PC_No, @VCECode, @TransDate, @AccountCode, @Amount, @Remarks, @CostID, @Status, @WhoCreated, @TransAuto)"
        SQL.FlushParams()
        SQL.AddParam("@TransID", TransID)
        SQL.AddParam("@PC_No", PCNO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@Amount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@CostID", CostID)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)
    End Sub

    Private Sub UpdateCA()
        Dim CostID = GetCostCenterID(ddlCostCenter.SelectedValue)
        Dim updateSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblPC  " &
                        " SET    PC_No = @PC_No, VCECode = @VCECode, TransDate = @TransDate, Amount = @Amount," &
                        " AccountCode = @AccountCode, Remarks = @Remarks, CostID = @CostID, TransAuto = @TransAuto, WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE TransID = @TransID "
        SQL.FlushParams()
        SQL.AddParam("@TransID", Session("TransID").ToString)
        SQL.AddParam("@PC_No", PCNO)
        SQL.AddParam("@VCECode", txtCode.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@Amount", CDec(txtAmount.Text))
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@CostID", CostID)
        SQL.AddParam("@TransAuto", TransAuto)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

    End Sub

    Private Function IfExist(ByVal ID As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblPC WHERE PC_No ='" & ID & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub LoadTransaction(ByVal ID As String)
        Dim query As String
        query = " SELECT  tblPC.TransID, PC_No, tblPC.VCECode, Name, TransDate, Amount, CostID, Remarks, tblPC.AccountCode ,Accounttitle," & vbCrLf &
                " CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN 'Active'  " & vbCrLf &
                " 	      WHEN tblPC.Status ='Active' THEN 'Closed'  " & vbCrLf &
                " 	    ELSE tblPC.Status END AS Status  " & vbCrLf &
                " FROM    tblPC  " & vbCrLf &
                " LEFT JOIN (SELECT TransID FROM View_CA_Balance) AS View_CA_Balance ON tblPC.TransID = View_CA_Balance.TransID " & vbCrLf &
                " LEFT JOIN View_VCEMMaster ON tblPC.VCECode = View_VCEMMaster.Code " & vbCrLf &
                " LEFT JOIN tblCOA ON tblPC.AccountCode = tblCOA.AccountCode " & vbCrLf &
                " WHERE   tblPC.TransID = '" & ID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID").ToString
            Session("TransID") = SQL.SQLDR("TransID").ToString
            PCNO = SQL.SQLDR("PC_No").ToString
            Session("Transno") = SQL.SQLDR("PC_No").ToString
            txtTrans_Num.Text = PCNO
            txtCode.Text = SQL.SQLDR("VCECode").ToString
            txtName.Text = SQL.SQLDR("Name").ToString
            txtAccntCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccntName.Text = SQL.SQLDR("Accounttitle").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            txtAmount.Text = SQL.SQLDR("Amount").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            ddlCostCenter.SelectedValue = GetCostCenter(SQL.SQLDR("CostID").ToString)
            txtStatus.Text = SQL.SQLDR("Status").ToString

            If txtStatus.Text = "Cancelled" Or txtStatus.Text = "Closed" Then
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
        Else
            Initialize()
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
        btnPrev.Attributes("disabled") = "disabled"
        btnNext.Attributes("disabled") = "disabled"
        btnPreview.Attributes("disabled") = "disabled"
    End Sub
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If Session("Transno") <> "" Then
            Dim query As String
            query = " SELECT Top 1 TransID FROM tblPC  WHERE PC_No > '" & Session("Transno") & "' ORDER BY PC_No ASC "
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
            query = " SELECT Top 1 TransID FROM tblPC  WHERE PC_No < '" & Session("Transno") & "' ORDER BY PC_No DESC "
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                TransID = SQL.SQLDR("TransID").ToString
                LoadTransaction(TransID)
            Else
                Response.Write("<script>alert('Reached the beginning of record!');</script>")
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
End Class