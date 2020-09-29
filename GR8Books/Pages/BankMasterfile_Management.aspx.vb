Imports System.Web.Services
Public Class BankMasterfile_Management
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
        ddlType.Items.Clear()
        ddlType.Items.Add("--Select Type--")
        ddlType.Items.Add("Savings")
        ddlType.Items.Add("Current")
        txtBank.Text = ""
        txtBranch.Text = ""
        txtAccntCode.Text = ""
        txtAccntTitle.Text = ""
        txtAccntNo.Text = ""
        txtSeriesDigits.Text = ""
        txtSeriesEnd.Text = ""
        txtSeriesStart.Text = ""
        txtAccntCode.Attributes.Add("readonly", "readonly")
        alertSave.Visible = False
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelBank.enabled = Not Value
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If btnSave.Text = "Save" Then
            Save()
            Response.Write("<script>window.location='BankMasterfile_Loadlist.aspx';</script>")
            alertSave.Visible = True
        ElseIf btnSave.Text = "Update" Then
            Update()
            Response.Write("<script>alert('Successfully Updated.');</script>")
            Response.Write("<script>opener.location.reload();</script>")
            Response.Write("<script>window.close();</script>")
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("BankMasterfile_Loadlist.aspx")
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblBank " &
                " (Type, Bank, Branch, AccountCode, AccountNumber, SeriesStart, SeriesEnd, SeriesDigits, Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@Type, @Bank, @Branch, @AccountCode, @AccountNumber, @SeriesStart, @SeriesEnd, @SeriesDigits, @Status, @DateCreated,@WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Type", ddlType.SelectedValue)
        SQL.AddParam("@Bank", txtBank.Text)
        SQL.AddParam("@Branch", txtBranch.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@AccountNumber", txtAccntNo.Text)
        SQL.AddParam("@SeriesStart", txtSeriesStart.Text)
        SQL.AddParam("@SeriesEnd", txtSeriesEnd.Text)
        SQL.AddParam("@SeriesDigits", txtSeriesDigits.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblBank " &
                " SET Type = @Type, Bank = @Bank, Branch = @Branch, " &
                " AccountCode = @AccountCode,  AccountNumber = @AccountNumber,  SeriesStart = @SeriesStart,  SeriesEnd = @SeriesEnd, " &
                " SeriesDigits = @SeriesDigits, DateModified = @DateModified, @WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Type", ddlType.SelectedValue)
        SQL.AddParam("@Bank", txtBank.Text.Trim)
        SQL.AddParam("@Branch", txtBranch.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@AccountNumber", txtAccntNo.Text)
        SQL.AddParam("@SeriesStart", txtSeriesStart.Text)
        SQL.AddParam("@SeriesEnd", txtSeriesEnd.Text)
        SQL.AddParam("@SeriesDigits", txtSeriesDigits.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub View()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT ID, Type, Bank, Branch, tblBank.AccountCode, ISNULL(AccountTitle,'') as AccountTitle, AccountNumber, SeriesStart," &
                " SeriesEnd, SeriesDigits FROM tblBank" &
                " LEFT JOIN" &
                " tblCOA ON" &
                " tblCOA.AccountCode = tblBank.AccountCode " &
                " WHERE tblBank.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            ddlType.SelectedValue = SQL.SQLDR("Type").ToString
            txtBank.Text = SQL.SQLDR("Bank").ToString
            txtBranch.Text = SQL.SQLDR("Branch").ToString
            txtAccntCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccntTitle.Text = SQL.SQLDR("AccountTitle").ToString
            txtAccntNo.Text = SQL.SQLDR("AccountNumber").ToString
            txtSeriesStart.Text = SQL.SQLDR("SeriesStart").ToString
            txtSeriesEnd.Text = SQL.SQLDR("SeriesEnd").ToString
            txtSeriesDigits.Text = SQL.SQLDR("SeriesDigits").ToString
        End If
    End Sub

    Private Sub BankMasterfile_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
End Class