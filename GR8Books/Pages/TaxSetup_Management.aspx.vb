Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class TaxSetup_Management
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

    Public Sub EnableControl(ByVal Value As Boolean)
        panelTaxSetUp.Enabled = Not Value
        btnAddTaxForm.Visible = Not Value
        btnAddTaxType.Visible = Not Value
    End Sub

    Public Sub Initialize()
        txtAccountCode.Text = ""
        txtAccountTitle.Text = ""
        txtBasis.Text = ""
        txtName.Text = ""
        ddlNormal.Items.Clear()
        ddlNormal.Items.Add("--Select Normal--")
        ddlNormal.Items.Add("Debit")
        ddlNormal.Items.Add("Credit")
        txtPercentage.Text = ""
        txtAccountCode.Attributes.Add("readonly", "readonly")

        ddlTaxType.Items.Clear()
        ddlTaxType.Items.Add("--Select Tax Type--")
        ddlTaxType.DataSource = LoadTaxType().ToArray
        ddlTaxType.DataBind()

        ddlForm.Items.Clear()
        ddlForm.Items.Add("--Select Form--")
        ddlForm.DataSource = LoadForm().ToArray
        ddlForm.DataBind()

        ddlMonth.Items.Clear()
        ddlMonth.Items.Add("--Select Month--")
        ddlMonth.Items.Add("January")
        ddlMonth.Items.Add("February")
        ddlMonth.Items.Add("March")
        ddlMonth.Items.Add("April")
        ddlMonth.Items.Add("May")
        ddlMonth.Items.Add("June")
        ddlMonth.Items.Add("July")
        ddlMonth.Items.Add("August")
        ddlMonth.Items.Add("September")
        ddlMonth.Items.Add("October")
        ddlMonth.Items.Add("November")
        ddlMonth.Items.Add("December")

        ddlDay.Items.Clear()
        ddlDay.Items.Add("--Select Day--")
    End Sub



    Public Function LoadTaxType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT TaxType " &
                       " FROM   tblTax_Type"
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("TaxType").ToString)
        End While
        Return list
    End Function

    Public Function LoadForm() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT TaxForm " &
                       " FROM   tblTax_Form"
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("TaxForm").ToString)
        End While
        Return list
    End Function


    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE '%' +  @AccountTitle + '%' AND Status = 'Active'"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            AccountTitle.Add(String.Format("{0}--{1}", SQL.SQLDR("AccountTitle"), SQL.SQLDR("AccountCode")))
        End While
        Return AccountTitle.ToArray()
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='TaxSetup_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub TasSetup_Management_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        query = "  SELECT ID, TAX_Type,Percentage, Normal, tblTax_Setup.Name, Form, Basis, tblTax_Setup.AccountCode, tblCOA.AccountTitle, DeadlineMonth, DeadlineDay FROM tblTax_Setup " &
                "  LEFT JOIN tblCOA ON tblTax_Setup.AccountCode = tblCOA.AccountCode " &
                "  WHERE tblTax_Setup.Status = @Status And ID = @ID "
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ddlTaxType.SelectedValue = SQL.SQLDR("TAX_Type").ToString
            txtPercentage.Text = SQL.SQLDR("Percentage").ToString
            ddlNormal.SelectedValue = SQL.SQLDR("Normal").ToString
            txtName.Text = SQL.SQLDR("Name").ToString
            ddlForm.SelectedValue = SQL.SQLDR("Form").ToString
            txtBasis.Text = SQL.SQLDR("Basis").ToString
            ddlMonth.SelectedValue = SQL.SQLDR("DeadlineMonth").ToString
            LoadDay()
            ddlDay.SelectedValue = SQL.SQLDR("DeadlineDay").ToString
            txtAccountCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccountTitle.Text = SQL.SQLDR("AccountTitle").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblTax_Setup " &
                " (TAX_Type, Normal, Percentage, Name, AccountCode, Form, Basis, Status, DeadlineMonth, DeadlineDay, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@TAX_Type, @Normal, @Percentage, @Name, @AccountCode, @Form, @Basis,  @Status, @DeadlineMonth, @DeadlineDay, @DateCreated,  @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TAX_Type", ddlTaxType.SelectedValue)
        SQL.AddParam("@Normal", ddlNormal.SelectedValue)
        SQL.AddParam("@Percentage", txtPercentage.Text)
        SQL.AddParam("@Name", txtName.Text)
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Form", ddlForm.SelectedValue)
        SQL.AddParam("@Basis", txtBasis.Text)
        SQL.AddParam("@DeadlineMonth", ddlMonth.SelectedValue)
        SQL.AddParam("@DeadlineDay", ddlDay.SelectedValue)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblTax_Setup " &
                " SET TAX_Type = @TAX_Type, Normal = @Normal, Percentage = @Percentage, " &
                "     Name = @Name, AccountCode = @AccountCode, Form = @Form, Basis = @Basis, " &
                "     DeadlineMonth = @DeadlineMonth, DeadlineDay = @DeadlineDay, DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@TAX_Type", ddlTaxType.SelectedValue)
        SQL.AddParam("@Normal", ddlNormal.Text)
        SQL.AddParam("@Percentage", txtPercentage.Text)
        SQL.AddParam("@Name", txtName.Text)
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Form", ddlForm.SelectedValue)
        SQL.AddParam("@Basis", txtBasis.Text)
        SQL.AddParam("@DeadlineMonth", ddlMonth.SelectedValue)
        SQL.AddParam("@DeadlineDay", ddlDay.SelectedValue)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='TaxSetup_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    <WebMethod>
    Public Shared Function SaveTaxType(TaxType As TaxType) As String
        Dim query As String
        query = " INSERT INTO tblTax_Type " &
                    " (TaxType, Status, DateCreated)" &
                    " VALUES " &
                    " (@TaxType, @Status, @DateCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TaxType", TaxType.TaxType)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class TaxType
        Public Property TaxType() As String
            Get
                Return m_TaxType
            End Get
            Set
                m_TaxType = Value
            End Set
        End Property
        Private m_TaxType As String
    End Class


    <WebMethod>
    Public Shared Function SaveTaxForm(TaxForm As TaxForm) As String
        Dim query As String
        query = " INSERT INTO tblTax_Form " &
                    " (TaxForm, Status, DateCreated)" &
                    " VALUES " &
                    " (@TaxForm, @Status, @DateCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TaxForm", TaxForm.TaxForm)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class TaxForm
        Public Property TaxForm() As String
            Get
                Return m_TaxForm
            End Get
            Set
                m_TaxForm = Value
            End Set
        End Property
        Private m_TaxForm As String
    End Class

    Public Sub TaxType_Load()
        txtTaxType.Text = ""
        ddlTaxType.Items.Clear()
        ddlTaxType.Items.Add("--Select Tax Type--")
        ddlTaxType.DataSource = LoadTaxType().ToArray
        ddlTaxType.DataBind()
    End Sub

    Public Sub TaxForm_Load()
        txtTaxForm.Text = ""
        ddlForm.Items.Clear()
        ddlForm.Items.Add("--Select Form--")
        ddlForm.DataSource = LoadForm().ToArray
        ddlForm.DataBind()
    End Sub

    Public Sub LoadDay()
        ddlDay.Items.Clear()
        ddlDay.Items.Add("--Select Day--")
        If ddlMonth.SelectedIndex = 2 Then
            For day As Integer = 1 To 29
                ddlDay.Items.Add(day)
            Next
        ElseIf (ddlMonth.SelectedIndex - 1) Mod 2 = 0 Then
            For day As Integer = 1 To 31
                ddlDay.Items.Add(day)
            Next
        Else
            For day As Integer = 1 To 30
                ddlDay.Items.Add(day)
            Next
        End If
    End Sub
End Class