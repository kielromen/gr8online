Public Class Collector_Management
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
        panelCollector.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtArea.Text = ""
        txtCollectorName.Text = ""
    End Sub

    Private Sub Collector_Management_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
        query = " SELECT * FROM tblCollector " &
                " WHERE tblCollector.Status = @Status AND ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtArea.Text = SQL.SQLDR("Area").ToString
            txtCollectorName.Text = SQL.SQLDR("Collector_Name").ToString
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCollector " &
                " (Area,Collector_Name,Status,DateCreated,WhoCreated)" &
                " VALUES " &
                " (@Area,@Collector_Name,@Status,@DateCreated,@WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Area", txtArea.Text.Trim)
        SQL.AddParam("@Collector_Name", txtCollectorName.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCollector " &
                " SET Area = @Area, Collector_Name = @Collector_Name, " &
                " DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE ID = @ID"
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@Area", txtArea.Text.Trim)
        SQL.AddParam("@Collector_Name", txtCollectorName.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='Collector_Loadlist.aspx';</script>")
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
            Response.Write("<script>window.location='Collector_Loadlist.aspx';</script>")
        End If
    End Sub
End Class