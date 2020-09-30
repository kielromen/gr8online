Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Public Class Settings_CostCenter
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim COSTID As String
    Dim ModuleID As String = "CC"
    Dim ColumnPK As String = "CostID"
    Dim DBTable As String = "tblCostCenter"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") <> True Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                btnNew.Attributes.Remove("disabled")
                btnEdit.Attributes("disabled") = "disabled"
                btnSave.Attributes("disabled") = "disabled"
                btnCancel.Attributes("disabled") = "disabled"
                btnClose.Attributes("disabled") = "disabled"
                EnableControl(False)
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtCostID.Attributes.Add("readonly", "readonly")
        txtCostID.Text = ""
        txtCostCenter.Text = ""
        alertSave.Visible = False
    End Sub


    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
        If TransAuto Then
            txtCostID.Attributes.Add("readonly", "readonly")
        End If
    End Sub

    Public Sub Save()
        txtCostID.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        query = " INSERT INTO tblCostCenter " &
                " (CostID, CostCenter,
                         Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@CostID, @CostCenter,
                         @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@CostID", txtCostID.Text)
        SQL.AddParam("@CostCenter", txtCostCenter.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim query As String
        query = " UPDATE tblItem_Master " &
                " SET CostCenter = @CostCenter,
                  Status = @Status, DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE CostID = @CostID"
        SQL.FlushParams()
        SQL.AddParam("@CostID", txtCostID.Text)
        SQL.AddParam("@CostCenter", txtCostCenter.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        EnableControl(True)
        txtCostID.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If Session("TransID") = "" Then
                If TransAuto Then
                    COSTID = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
                Else
                    COSTID = txtCostID.Text
                End If
                txtCostID.Text = COSTID
                Save()
                Response.Write("<script>alert('Cost center " & txtCostID.Text & " successfully saved.');</script>")
            Else
                COSTID = txtCostID.Text
                Update()
                Response.Write("<script>alert('Cost center " & txtCostID.Text & " successfully updated.');</script>")
            End If
            Initialize()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If COSTID = "" Then
            Initialize()
            EnableControl(False)
            btnEdit.Attributes("disabled") = "disabled"
            btnCancel.Attributes("disabled") = "disabled"

        Else
            Initialize()
            btnEdit.Attributes.Remove("disabled")
            btnCancel.Attributes.Remove("disabled")
        End If
        btnNew.Attributes.Remove("disabled")
        btnSave.Attributes("disabled") = "disabled"
        btnClose.Attributes("disabled") = "disabled"
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EnableControl(True)
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim query As String
        query = " UPDATE  " & DBTable & " SET Status ='Cancelled' WHERE CostID = @CostID "
        SQL.FlushParams()
        SQL.AddParam("@CostID", txtCostID.Text)
        SQL.ExecNonQuery(query)
    End Sub
End Class