Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class Responsibility_Center
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
            End If
        End If
    End Sub
    Public Sub Initialize()
        txtCostCenter.Text = ""

        ddlType.Items.Clear()
        ddlType.Items.Add("--Select Type--")
        ddlType.Items.Add("Cost Center")
        ddlType.Items.Add("Profit Center")
        ddlType.Items.Add("Investment Center")
        ddlType.DataBind()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelRecCenter.Enabled = Not Value
    End Sub

    Private Sub Responsibility_Center_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ID As String = Request.QueryString("ID")
            Dim Actions As String = Request.QueryString("Actions")
            If Actions = "Edit" Then
                Initialize()
                EnableControl(False)
                View()
                btnSave.Visible = True
                btnCancel.Visible = True
                btnSave.Text = "Update"
            ElseIf Actions = "View" Then
                Initialize()
                EnableControl(True)
                View()
                btnSave.Visible = False
                btnCancel.Visible = False
            Else
                Initialize()
                EnableControl(False)
                btnSave.Visible = True
                btnCancel.Visible = True
            End If
        End If
    End Sub


    Public Sub View()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblResponsibility_Center " &
                " WHERE tblResponsibility_Center.Status = @Status AND CostID = @CostID"
        SQL.FlushParams()
        SQL.AddParam("@CostID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ddlType.SelectedValue = SQL.SQLDR("Type").ToString
            txtCostCenter.Text = SQL.SQLDR("CostCenter").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblResponsibility_Center " &
                " (CostCenter,Type,Status,DateCreated,WhoCreated)" &
                " VALUES " &
                " (@CostCenter,@Type,@Status,@DateCreated,@WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@CostCenter", txtCostCenter.Text)
        SQL.AddParam("@Type", ddlType.SelectedValue)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblResponsibility_Center " &
                " SET CostCenter = @CostCenter, Type = @Type, " &
                " DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE CostID = @CostID"
        SQL.FlushParams()
        SQL.AddParam("@CostID", ID)
        SQL.AddParam("@CostCenter", txtCostCenter.Text)
        SQL.AddParam("@Type", ddlType.SelectedValue)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='ResponsibilityCenter_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='ResponsibilityCenter_Loadlist.aspx';</script>")
        End If
    End Sub
End Class