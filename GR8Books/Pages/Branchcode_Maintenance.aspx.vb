Imports System.Web.Services
Public Class Branchcode_Maintenance
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                dtpDoc_Date.Value = Format(Date.Now, "yyyy-MM-dd")
            End If
        End If

    End Sub
    Public Sub Initialize()
        txtDescription.Text = ""
        txtStatus.Text = ""
        dtpDoc_Date.Value = Format(Date.Now, "yyyy-MM-dd")
        txtbranchcode.Text = AutoBranchcode()
    End Sub
    Public Function AutoBranchcode()
        Dim branchcode As String = ""
        Dim query As String
        query = " SELECT    ISNULL(MAX(SUBSTRING(Branchcode,2+1,1))+ 1,1) AS Branchcode " &
                " FROM tblBranch "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            branchcode = SQL.SQLDR("Branchcode")
            For i As Integer = 1 To 3
                branchcode = "0" & branchcode
            Next
            branchcode = Strings.Right(branchcode, 3)
        End If
        Return branchcode
    End Function
    Public Sub EnableControl(ByVal Value As Boolean)
        txtDescription.ReadOnly = Value
        txtStatus.ReadOnly = Value
        txtbranchcode.ReadOnly = Value
    End Sub
    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblBranch " &
                " ( Branchcode, Description, Status, DateCreated)" &
                " VALUES " &
                " ( @Branchcode, @Description, @Status, @DateCreated)"
        '" ( @Branchcode, @Description, @Status, @DateCreated, @DateModified, @WhoModified, @SortID)"
        '" ( Branchcode, Description, Status, DateCreated, DateModified, WhoModified, SortID)" &
        SQL.FlushParams()
        SQL.AddParam("@Branchcode", txtbranchcode.Text)
        'SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@Description", txtDescription.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", dtpDoc_Date.Value)
        'SQL.AddParam("@DateCreated", Now.Date)
        'SQL.AddParam("@WhoModified", Session("EmailAddress"))
        'SQL.AddParam("@SortID", "")
        SQL.ExecNonQuery(query)

    End Sub
    Public Sub Update()
        Dim ID As String = Request.QueryString("Branchcode")
        Dim query As String
        query = " UPDATE tblBranch " &
                " SET Description = @Description, " &
                " Status = @Status, DateCreated = @DateCreated " &
                " WHERE Branchcode = @Branchcode"
        SQL.FlushParams()
        SQL.AddParam("@Branchcode", txtbranchcode.Text)
        'SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@Description", txtDescription.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", dtpDoc_Date.Value)
        'SQL.AddParam("@DateCreated", Now.Date)
        'SQL.AddParam("@WhoModified", Session("EmailAddress"))
        'SQL.AddParam("@SortID", True)
        SQL.ExecNonQuery(query)
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If btnSave.Text = "Save" Then
            Save()
            Response.Write("<script>alert('Successfully Saved.');window.location='Branchcode_maintenance.aspx';</script>")
        ElseIf btnSave.Text = "Update" Then
            Update()
            Response.Write("<script>alert('Successfully Updated.');</script>")
            Response.Write("<script>opener.location.reload();</script>")
            Response.Write("<script>window.close();</script>")
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("Branchcode_Entry.aspx")
    End Sub
    Private Sub Branchcode_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ID As String = Request.QueryString("Branchcode")
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
    'Public Sub View()
    '    Dim ID As String = Request.QueryString("Branchcode")
    '    Dim query As String
    '    query = " SELECT Branchcode, Descrition, Date, Status,  " &
    '            "  FROM tblBranch" &
    '            " WHERE tblBranch.Status = @Status AND Branchcode = @Branchcode"
    '    SQL.FlushParams()
    '    SQL.AddParam("@Branchcode", ID)
    '    SQL.AddParam("@Status", "Active")
    '    SQL.ReadQuery(query)
    '    If SQL.SQLDR.Read Then
    '        txtbranchcode.Text = SQL.SQLDR("Branchcode")
    '        txtDescription.Text = SQL.SQLDR("Description")
    '        dtpDoc_Date.Value = CDate(SQL.SQLDR("Date")).ToString("yyyy-MM-dd")
    '        'txtDebitType.Text = SQL.SQLDR("DM_Type").ToString
    '        'txtDebitCode.Text = SQL.SQLDR("DebitAccount").ToString
    '        'txtDebitTitle.Text = SQL.SQLDR("AccountTitle").ToString
    '        'txtCustomerCode.Text = SQL.SQLDR("Customer").ToString
    '        'txtChargeToCode.Text = SQL.SQLDR("ChargeTo").ToString
    '        'txtRemarks.Text = SQL.SQLDR("Remarks").ToString
    '        'txtCustomerName.Text = GetVCEName(txtCustomerCode.Text)
    '        'txtChargeToName.Text = GetVCEName(txtChargeToCode.Text)
    '    End If
    'End Sub
    Public Sub View()
        Dim ID As String = Request.QueryString("Branchcode")
        Dim query As String
        query = " SELECT Branchcode, Description, Status,DateCreated " &
                "  FROM tblBranch " &
        " WHERE Branchcode = @Branchcode  "
        SQL.FlushParams()
        SQL.AddParam("@branchcode", ID)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtbranchcode.Text = SQL.SQLDR("Branchcode").ToString
            dtpDoc_Date.Value = CDate(SQL.SQLDR("DateCreated")).ToString("yyyy-MM-dd")
            txtDescription.Text = SQL.SQLDR("Description").ToString
            TxtStatus.Text = SQL.SQLDR("Status").ToString
        End If
    End Sub
End Class
