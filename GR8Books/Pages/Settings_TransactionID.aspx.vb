Public Class Settings_TransactionID
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                panel.Visible = False
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtDescription.Attributes.Add("readonly", "readonly")
        ddlBranchCode.Items.Clear()
        ddlBranchCode.Items.Add("All")
        ddlBusinessCode.Items.Clear()
        ddlBusinessCode.Items.Add("All")
        txtPrefix.Text = ""
        txtStartRecord.Text = ""
        txtDigits.Text = ""
        txtDescription.Text = ""
        alertSave.Visible = False
        hfTransType.Value = ""
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT * FROM tblTransactionSetup  "
        SQL.GetQuery(query)
        gvTransSetUp.DataSource = SQL.SQLDS
        gvTransSetUp.DataBind()
    End Sub

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvTransSetUp, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim TransType As String = gvTransSetUp.SelectedRow.Cells(1).Text
        LoadDetails(TransType)
    End Sub

    Private Sub LoadDetails(ByVal TransType As String)
        Initialize()
        panel.Visible = True
        Dim query As String
        query = " SELECT * FROM tblTransactionSetup WHERE  TransType = @TransType  "
        SQL.FlushParams()
        SQL.AddParam("@TransType", TransType)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            hfTransType.Value = SQL.SQLDR("TransType")
            chkAuto.Checked = SQL.SQLDR("AutoSeries")
            chkGlobal.Checked = SQL.SQLDR("GlobalSeries")
            txtDescription.Text = SQL.SQLDR("Description")
        End If

        query = " SELECT * FROM tblTransactionDetails WHERE TransType = @TransType "
        SQL.FlushParams()
        SQL.AddParam("@TransType", TransType)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtStartRecord.Text = SQL.SQLDR("StartRecord")
            txtDigits.Text = SQL.SQLDR("Digits")
            txtPrefix.Text = SQL.SQLDR("Prefix")
            ddlBranchCode.SelectedValue = SQL.SQLDR("BranchCode")
            ddlBusinessCode.SelectedValue = SQL.SQLDR("BusinessCode")
            btnSave.Text = "Update"
        Else
            btnSave.Text = "Save"
        End If

        query = " SELECT * FROM tblTransactionDetails WHERE TransType = @TransType "
        SQL.FlushParams()
        SQL.AddParam("@TransType", TransType)
        SQL.GetQuery(query)
        gvTransDetails.DataSource = SQL.SQLDS
        gvTransDetails.DataBind()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                btnSave.Text = "Update"
            Else
                Update()
                btnSave.Text = "Update"
            End If

            Loadlist()
            alertSave.Visible = True
        End If
    End Sub


    Public Sub Update()
        Dim query As String
        query = " UPDATE tblTransactionDetails SET Prefix = @Prefix, Digits = @Digits, BranchCode = @BranchCode, BusinessCode = @BusinessCode, " &
                " StartRecord = @StartRecord,DateModified = @DateModified , WhoModified = @WhoModified WHERE TransType = @TransType"
        SQL.FlushParams()
        SQL.AddParam("@TransType", hfTransType.Value)
        SQL.AddParam("@Prefix", txtPrefix.Text)
        SQL.AddParam("@Digits", txtDigits.Text)
        SQL.AddParam("@BranchCode", ddlBranchCode.SelectedValue)
        SQL.AddParam("@BusinessCode", ddlBusinessCode.SelectedValue)
        SQL.AddParam("@StartRecord", txtStartRecord.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)

        query = " UPDATE tblTransactionSetup SET AutoSeries = @AutoSeries, GlobalSeries = @GlobalSeries" &
                " WHERE TransType = @TransType"
        SQL.FlushParams()
        SQL.AddParam("@AutoSeries", chkAuto.Checked)
        SQL.AddParam("@GlobalSeries", chkGlobal.Checked)
        SQL.AddParam("@TransType", hfTransType.Value)
        SQL.ExecNonQuery(query)
    End Sub


    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblTransactionDetails  " &
                " (TransType, BranchCode, BusinessCode, TransGroup, Prefix, Digits, StartRecord, DateCreated, WhoCreated)" &
                " VALUES" &
                " (@TransType, @BranchCode, @BusinessCode, @TransGroup, @Prefix, @Digits, @StartRecord, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TransType", hfTransType.Value)
        SQL.AddParam("@Prefix", txtPrefix.Text)
        SQL.AddParam("@Digits", txtDigits.Text)
        SQL.AddParam("@BranchCode", ddlBranchCode.SelectedValue)
        SQL.AddParam("@TransGroup", "1")
        SQL.AddParam("@BusinessCode", ddlBusinessCode.SelectedValue)
        SQL.AddParam("@StartRecord", txtStartRecord.Text)
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)

        query = " UPDATE tblTransactionSetup SET AutoSeries = @AutoSeries, GlobalSeries = @GlobalSeries" &
                " WHERE TransType = @TransType"
        SQL.FlushParams()
        SQL.AddParam("@AutoSeries", chkAuto.Checked)
        SQL.AddParam("@GlobalSeries", chkGlobal.Checked)
        SQL.AddParam("@TransType", hfTransType.Value)
        SQL.ExecNonQuery(query)
    End Sub
End Class