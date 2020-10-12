Imports System.Web.Services
Public Class DefaultAccounts
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
        panelDefaultAccount.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtDefaultAccountCode.Attributes.Add("readonly", "readonly")
        txtRefAccountCode.Attributes.Add("readonly", "readonly")
        ddlDescription.Items.Clear()
        ddlDescription.Items.Add("--Select Description--")
        ddlDescription.DataSource = LoadDefaultModuleAccount().ToArray
        ddlDescription.DataBind()

        txtDefaultAccountCode.Text = ""
        txtDefaultAccountTitle.Text = ""
        txtRefAccountCode.Text = ""
        txtRefAccountTitle.Text = ""
    End Sub

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

    Private Sub DefaultAccounts_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        query = " SELECT ID,  ModuleID, Description, tblDefaultAccount.AccountCode, AccountTitle, RefAccount, RefAccountTitle, Status, " &
                " 	   DateCreated, DateModified, WhoCreated, WhoModified " &
                " FROM tblDefaultAccount  " &
                " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS DefaultAccount " &
                " ON tblDefaultAccount.AccountCode = DefaultAccount.AccountCode " &
                " LEFT JOIN (SELECT AccountCode, AccountTitle AS RefAccountTitle FROM tblCOA) AS RefAccount " &
                " ON tblDefaultAccount.RefAccount = RefAccount.AccountCode " &
                " WHERE tblDefaultAccount.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ddlDescription.SelectedValue = SQL.SQLDR("Description").ToString
            txtDefaultAccountCode.Text = SQL.SQLDR("AccountCode").ToString
            txtDefaultAccountTitle.Text = SQL.SQLDR("AccountTitle").ToString
            txtRefAccountCode.Text = SQL.SQLDR("RefAccount").ToString
            txtRefAccountTitle.Text = SQL.SQLDR("RefAccountTitle").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        Dim ModuleID As String = GetModuleID(ddlDescription.SelectedValue)
        query = " INSERT INTO tblDefaultAccount " &
                " (ModuleID, Description,AccountCode,RefAccount,Status,DateCreated,WhoCreated)" &
                " VALUES " &
                " (@ModuleID, @Description,@AccountCode,@RefAccount,@Status,@DateCreated,@WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@ModuleID", ModuleID)
        SQL.AddParam("@Description", ddlDescription.SelectedValue)
        SQL.AddParam("@AccountCode", txtDefaultAccountCode.Text)
        SQL.AddParam("@RefAccount", txtRefAccountCode.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        Dim ModuleID As String = GetModuleID(ddlDescription.SelectedValue)
        query = " UPDATE tblDefaultAccount " &
                " SET  ModuleID = @ModuleID, Description = @Description, AccountCode = @AccountCode, RefAccount = @RefAccount, " &
                " DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@ModuleID", ModuleID)
        SQL.AddParam("@Description", ddlDescription.SelectedValue)
        SQL.AddParam("@AccountCode", txtDefaultAccountCode.Text)
        SQL.AddParam("@RefAccount", txtRefAccountCode.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='DefaultAccounts_Loadlist.aspx';</script>")
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
            Response.Write("<script>window.location='DefaultAccounts_Loadlist.aspx';</script>")
        End If
    End Sub
End Class