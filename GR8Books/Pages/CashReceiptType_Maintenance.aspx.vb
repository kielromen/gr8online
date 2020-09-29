Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class CashReeciptType_Maintenance
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
        panelCollectionType.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtAccountCode.Text = ""
        txtAccountCode.Text = ""
        txtAmount.Text = ""
        txtDescription.Text = ""
        txtAccountCode.Attributes.Add("readonly", "readonly")
        ddlModule.Items.Clear()
        ddlModule.Items.Add("--Select Module--")
        ddlModule.Items.Add("All")
        ddlModule.Items.Add("Official Receipt")
        ddlModule.Items.Add("Collection Receipt")
        ddlModule.Items.Add("Acknoledgement Receipt")
        ddlModule.Items.Add("Provisional Receipt")
    End Sub

    Private Sub CollectionType_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        query = " SELECT * FROM tblCollection_Type " &
                " LEFT JOIN " &
                " tblCOA ON tblCollection_Type.AccountCode = tblCOA.AccountCode " &
                " WHERE tblCollection_Type.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtAccountCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccountTitle.Text = SQL.SQLDR("AccountTitle").ToString
            txtDescription.Text = SQL.SQLDR("Description").ToString
            txtAmount.Text = CDec(SQL.SQLDR("Amount")).ToString("N2")
            ddlModule.SelectedValue = SQL.SQLDR("Module").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCollection_Type " &
                " (Description, AccountCode, Amount, Module, Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@Description, @AccountCode, @Amount, @Module, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Description", txtDescription.Text.Trim)
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Amount", CDec(txtAmount.Text).ToString("N2"))
        SQL.AddParam("@Module", ddlModule.SelectedValue)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCollection_Type " &
                " SET Description = @Description, AccountCode = @AccountCode, " &
                " Amount = @Amount, Module = @Module, DateModified = @DateModified, @WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Description", txtDescription.Text.Trim)
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Amount", CDec(txtAmount.Text).ToString("N2"))
        SQL.AddParam("@Module", ddlModule.SelectedValue)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='CashReceiptType_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub


    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE @AccountTitle + '%'"
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
            Response.Write("<script>window.location='CashReceiptType_Loadlist.aspx';</script>")
        End If
    End Sub
End Class