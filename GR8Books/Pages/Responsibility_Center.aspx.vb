Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class Responsibility_Center
    Inherits System.Web.UI.Page
    Dim TransID As Integer
    Dim PC_ID As String
    Dim AutogenPC_ID As Integer
    Dim ModuleID As String = "RC"
    Dim ColumnPK As String = "ID"
    Dim ColumnID As String = "ID"
    Dim DBTable As String = "tblRespon_Center"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
            End If
        End If
    End Sub
    Public Sub Initialize()
        Session("Transno") = ""
        txtCC_Code.Text = ""
        txtCC_Name.Text = ""
        txtIC_Code.Text = ""
        txtIC_Name.Text = ""
        txtPC_Code.Text = ""
        txtPC_Name.Text = ""
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelConrols.Enabled = Value
    End Sub
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        ID = ""
        Session("TransID") = ""
        btnSearch.Attributes("disabled") = "disabled"
        btnNew.Attributes("disabled") = "disabled"
        btnEdit.Attributes("disabled") = "disabled"
        btnSave.Attributes.Remove("disabled")
        btnCancel.Attributes("disabled") = "disabled"
        btnClose.Attributes.Remove("disabled")
        EnableControl(True)

    End Sub

    Private Sub Responsibility_Center_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Session("ID") = ""
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If PC_ID = "" Then
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
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If Session("TransID") = "" Then
                TransID = GenerateTransID(ColumnID, DBTable)
                Save()
                Response.Write("<script>alert('Transaction successfully saved.');</script>")
                LoadTransaction(TransID)
            Else
                ID = Session("TransNo")
                UpdateCV()
                Response.Write("<script>alert('Transaction  successfully updated.');</script>")
                LoadTransaction(Session("TransID"))
            End If
        End If
    End Sub
    Public Sub Save()
        Dim insertSQL As String
        activityStatus = True
        SQL.FlushParams()
        insertSQL = " INSERT INTO " &
                        " tblRespon_Center (ID, CC_Code, CC_Name, PC_Code, PC_Name, IC_Code, IC_Name, WhoCreated, DateCreated )" &
                        " VALUES (@ID, @CC_Code, @CC_Name, @PC_Code, @PC_Name, @IC_Code, @IC_Name, @WhoCreated, @DateCreated)"


        SQL.FlushParams()
        SQL.AddParam("@ID", TransID)
        SQL.AddParam("@CC_Code", txtCC_Code.Text)
        SQL.AddParam("@CC_Name", txtCC_Name.Text)
        SQL.AddParam("@PC_Code", txtPC_Code.Text)
        SQL.AddParam("@PC_Name", txtPC_Name.Text)
        SQL.AddParam("@IC_Code", txtIC_Code.Text)
        SQL.AddParam("@IC_Name", txtIC_Name.Text)
        SQL.AddParam("@DateCreated", Date.Now)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(insertSQL)

    End Sub
    Private Sub UpdateCV()
        Dim updateSQL As String
        activityStatus = True
        updateSQL = " UPDATE tblrespon_center  " &
                        " SET CC_Code = @CC_Code, CC_Name = @CC_Name, IC_Code = @IC_Code, IC_Name = @IC_Name , PC_Code = @PC_Code, " &
                        " WhoModified = @WhoModified, DateModified = GETDATE() " &
                        " WHERE ID = @ID "
        SQL.FlushParams()
        SQL.AddParam("@ID", Session("TransID").ToString)
        SQL.AddParam("@CC_Code", txtCC_Code.Text)
        SQL.AddParam("@CC_Name", txtCC_Name.Text)
        SQL.AddParam("@IC_Code", txtIC_Code.Text)
        SQL.AddParam("@IC_Name", txtIC_Name.Text)
        SQL.AddParam("@PC_Code", txtPC_Code.Text)
        SQL.AddParam("@PC_Name", txtPC_Name.Text)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(updateSQL)

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
        query = " SELECT  ID, CC_Code, CC_Name, PC_Code, PC_Name, IC_Code, IC_Name " &
                " FROM    tblRespon_Center " &
                " WHERE   ID = '" & ID & "' "

        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("ID").ToString
            Session("TransID") = SQL.SQLDR("ID").ToString
            'Session("Transno") = SQL.SQLDR("ID").ToString
            txtCC_Code.Text = SQL.SQLDR("CC_Code").ToString
            txtCC_Name.Text = SQL.SQLDR("CC_Name").ToString
            txtIC_Code.Text = SQL.SQLDR("IC_Code").ToString
            txtIC_Name.Text = SQL.SQLDR("IC_Name").ToString
            txtPC_Code.Text = SQL.SQLDR("PC_Code").ToString
            txtPC_Name.Text = SQL.SQLDR("PC_Name").ToString

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