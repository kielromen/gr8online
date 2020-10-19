Imports System.Web.Services
Public Class ChartofAccounts
    Inherits System.Web.UI.Page
    Dim Actions As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") <> True Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                alertSave.Visible = False
                alertUpdate.Visible = False
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtCode.Text = ""
        txtReportAlias.Text = ""
        txtDescription.Text = ""
        txtOrderNo.Text = ""

        ddlType.Items.Clear()
        ddlType.Items.Add("--Select Type--")
        ddlType.Items.Add("Balance Sheet")
        ddlType.Items.Add("Income Statement")

        ddlGroup.Items.Clear()
        ddlGroup.Items.Add("--Select Group--")
        ddlGroup.DataSource = LoadGroup().ToArray
        ddlGroup.DataBind()

        ddlNature.Items.Clear()
        ddlNature.Items.Add("--Select Account Nature--")
        ddlNature.Items.Add("Debit")
        ddlNature.Items.Add("Credit")

    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelCOA.Enabled = Not Value
    End Sub

    Public Function LoadGroup() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT Description" &
                       " FROM   tblCOA_AccountGroup  ORDER BY Hierarchy"
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Description").ToString)
        End While
        Return list
    End Function

    Public Sub GetMaxOrderNo()
        Dim query As String
        query = " SELECT Max(ISNULL(OrderNo,0)) + 1 AS OrderNo" &
                       " FROM  tblCOA  WHERE Status = @Status AND AccountType = @AccountType"
        SQL.FlushParams()
        SQL.AddParam("Status", "Active")
        SQL.AddParam("AccountType", ddlType.SelectedValue)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtOrderNo.Text = SQL.SQLDR("OrderNo").ToString
        Else
            txtOrderNo.Text = 1
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Write("<script>window.location='ChartOfAccount_Loadlist.aspx';</script>")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                If Not IfExist(txtCode.Text) Then
                    Save()
                    alertSave.Visible = True
                    alertUpdate.Visible = False
                    Initialize()
                Else
                    Response.Write("<script>alert('Account Code Already Exist!');</script>")
                End If
            ElseIf btnSave.Text = "Update" Then
                    Update()
                Response.Write("<script>ocation.reload();</script>")
                alertSave.Visible = False
                alertUpdate.Visible = True
            End If
        End If
    End Sub


    Private Function IfExist(ByVal AccountCode As String) As Boolean
        Dim query As String
        query = " SELECT * FROM tblCOA WHERE AccountCode ='" & AccountCode & "'  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub View()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblCOA " &
                " WHERE tblCOA.Status = @Status AND AccountCode = @AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtCode.Text = SQL.SQLDR("AccountCode").ToString
            txtDescription.Text = SQL.SQLDR("AccountTitle").ToString
            txtReportAlias.Text = SQL.SQLDR("ReportAlias").ToString
            ddlType.SelectedValue = SQL.SQLDR("AccountType").ToString
            ddlNature.SelectedValue = SQL.SQLDR("AccountNature").ToString
            chkWithSub.Checked = SQL.SQLDR("withSubsidiary").ToString
            ddlGroup.SelectedValue = SQL.SQLDR("AccountGroup").ToString
            txtOrderNo.Text = SQL.SQLDR("OrderNo").ToString
        End If
    End Sub


    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCOA " &
                    " ( AccountCode, AccountType, AccountTitle, AccountGroup, AccountNature, ReportAlias, Class, withSubsidiary, OrderNo, Status, DateCreated, WhoCreated )" &
                    " VALUES " &
                    " ( @AccountCode, @AccountType, @AccountTitle, @AccountGroup, @AccountNature, @ReportAlias, @Class, @withSubsidiary, @OrderNo,  @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", txtCode.Text)
        SQL.AddParam("@AccountType", ddlType.SelectedValue)
        SQL.AddParam("@AccountTitle", txtDescription.Text)
        SQL.AddParam("@AccountGroup", ddlGroup.SelectedValue)
        SQL.AddParam("@AccountNature", ddlNature.SelectedValue)
        SQL.AddParam("@ReportAlias", txtReportAlias.Text)
        SQL.AddParam("@Class", IIf(ddlGroup.SelectedValue = "Sub Account", "Posting", "Grouping"))
        SQL.AddParam("@withSubsidiary", chkWithSub.Checked)
        SQL.AddParam("@OrderNo", txtOrderNo.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCOA " &
                " SET AccountType = @AccountType, AccountTitle = @AccountTitle, AccountGroup = @AccountGroup, AccountNature = @AccountNature, ReportAlias = @ReportAlias, Class = @Class, withSubsidiary = @withSubsidiary, OrderNo = @OrderNo, " &
                " DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE AccountCode = @AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", ID)
        SQL.AddParam("@AccountType", ddlType.SelectedValue)
        SQL.AddParam("@AccountTitle", txtDescription.Text)
        SQL.AddParam("@AccountGroup", ddlGroup.SelectedValue)
        SQL.AddParam("@AccountNature", ddlNature.SelectedValue)
        SQL.AddParam("@ReportAlias", txtReportAlias.Text)
        SQL.AddParam("@Class", IIf(ddlGroup.SelectedValue = "Sub Account", "Posting", "Grouping"))
        SQL.AddParam("@withSubsidiary", chkWithSub.Checked)
        SQL.AddParam("@OrderNo", txtOrderNo.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub ChartofAccounts_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim AccountCode As String = Request.QueryString("Id")
            Actions = Request.QueryString("Actions")
            If Actions = "Edit" Then
                Initialize()
                EnableControl(False)
                View()
                btnSave.Visible = True
                btnCancel.Visible = True
                txtCode.Attributes.Add("readonly", "readonly")
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


    <WebMethod()>
    Public Shared Function CheckAccountCode(ByVal AccountCode As String) As Boolean
        Dim Code As Boolean = False
        Dim query As String
        query = "SELECT * FROM tblCOA WHERE AccountCode = @AccountCode"
        SQL.FlushParams()
        SQL.AddParam("AccountCode", AccountCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Code = False
        Else
            Code = True
        End If
        Return Code
    End Function

    <WebMethod()>
    Public Shared Function CheckDescription(ByVal Description As String) As Boolean
        Dim Desc As Boolean = False
        Dim query As String
        query = "SELECT * FROM tblCOA WHERE AccountTitle = @AccountTitle AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", Description)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Desc = False
        Else
            Desc = True
        End If
        Return Desc
    End Function
End Class