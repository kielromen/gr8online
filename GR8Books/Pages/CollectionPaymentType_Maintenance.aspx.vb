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
        txtPaymentType.ReadOnly = Value
    End Sub

    Public Sub Initialize()
        txtPaymentType.Text = ""
        chkWithBank.Checked = False
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
        query = " SELECT * FROM tblCollection_PaymentType " &
                " WHERE tblCollection_PaymentType.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtPaymentType.Text = SQL.SQLDR("PaymentType").ToString
            chkWithBank.Checked = SQL.SQLDR("WithBank").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCollection_PaymentType " &
                " (PaymentType, WithBank, Status, Date_Created, Who_Created)" &
                " VALUES " &
                " (@PaymentType, @WithBank, @Status, @Date_Created, @Who_Created)"
        SQL.FlushParams()
        SQL.AddParam("@PaymentType", txtPaymentType.Text)
        SQL.AddParam("@WithBank", chkWithBank.Checked)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Date_Created", Now.Date)
        SQL.AddParam("@Who_Created", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCollection_PaymentType " &
                " SET PaymentType = @PaymentType, WithBank = @WithBank, " &
                " Date_Modified = @Date_Modified, Who_Modified = @Who_Modified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@PaymentType", txtPaymentType.Text)
        SQL.AddParam("@WithBank", chkWithBank.Checked)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Date_Modified", Now.Date)
        SQL.AddParam("@Who_Modified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='CollectionPaymentType_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub
End Class