Imports System.Web.Services
Public Class CollectionPaymentType_Maintenance
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
        panelPaymentType.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtAccountCode.Attributes.Add("readonly", "readonly")
        txtAccountCode.Text = ""
        txtAccountTitle.Text = ""
        txtPaymentType.Text = ""
        ddlWithBank.Items.Clear()
        ddlWithBank.Items.Add("--Select Options--")
        ddlWithBank.Items.Add("True")
        ddlWithBank.Items.Add("False")
    End Sub

    Private Sub CollectionPaymentType_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        query = " SELECT  ID, PaymentType, WithBank, tblCollection_PaymentType.AccountCode, AccountTitle, Status, DateCreated, DateModified, WhoCreated, WhoModified " &
                " FROM tblCollection_PaymentType INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
                " ON tblCollection_PaymentType.AccountCode = tblCOA.AccountCode " &
                " WHERE tblCollection_PaymentType.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtPaymentType.Text = SQL.SQLDR("PaymentType").ToString
            ddlWithBank.SelectedValue = IIf(SQL.SQLDR("WithBank").ToString = 1, "True", "False")
            txtAccountCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccountTitle.Text = SQL.SQLDR("AccountTitle").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCollection_PaymentType " &
                " (PaymentType,WithBank,AccountCode, Status,DateCreated,WhoCreated)" &
                " VALUES " &
                " (@PaymentType,@WithBank,@AccountCode, @Status,@DateCreated,@WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@PaymentType", txtPaymentType.Text)
        SQL.AddParam("@WithBank", IIf(ddlWithBank.SelectedValue = "True", 1, 0))
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCollection_PaymentType " &
                " SET PaymentType = @PaymentType, WithBank = @WithBank, AccountCode = @AccountCode, " &
                " DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@PaymentType", txtPaymentType.Text)
        SQL.AddParam("@WithBank", IIf(ddlWithBank.SelectedValue = "True", 1, 0))
        SQL.AddParam("@AccountCode", txtAccountCode.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='CollectionPaymentType_LoadList.aspx';</script>")
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
            Response.Write("<script>window.location='CollectionPaymentType_LoadList.aspx';</script>")
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