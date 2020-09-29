Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Public Class Settings_Tax
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                panel.Visible = True
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Initialize()
        txtAccntCode.Attributes.Add("readonly", "readonly")
        ddlTaxType.Items.Clear()
        ddlTaxType.Items.Add("EWT")
        ddlTaxType.Items.Add("VAT")
        txtTaxCode.Text = ""
        txtAccntCode.Text = ""
        txtAccntName.Text = ""
        txtTaxAlias.Text = ""
        txtTaxDescription.Text = ""
        txtTaxRate.Text = ""
        txtATC.Text = ""
        txtNatureOfIncome.Text = ""
        alertSave.Visible = False
        hfTransType.Value = ""
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT  TaxCode, TaxType, TaxDescription, TaxRate, TaxAlias, tblTax_Maintenance.AccountCode, tblCOA.AccountTitle, ATC, NatureOfIncome" &
                " FROM tblTax_Maintenance LEFT JOIN tblCOA ON tblTax_Maintenance.AccountCode = tblCOA.AccountCode   "
        SQL.GetQuery(query)
        gvTax.DataSource = SQL.SQLDS
        gvTax.DataBind()
    End Sub

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(gvTax, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
            e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim TaxCode As String = gvTax.SelectedRow.Cells(0).Text
        LoadDetails(TaxCode)
        Loadlist()
    End Sub

    Private Sub LoadDetails(ByVal TaxCode As String)
        Initialize()
        panel.Visible = True
        Dim query As String

        query = " SELECT * FROM tblTax_Maintenance WHERE  TaxCode = @TaxCode"
        SQL.FlushParams()
        SQL.AddParam("@TaxCode", TaxCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            hfTransType.Value = SQL.SQLDR("RecordID")
        End If

        query = " SELECT  TaxCode, TaxType, TaxDescription, TaxRate, TaxAlias, tblTax_Maintenance.AccountCode, tblCOA.AccountTitle, ATC, NatureOfIncome" &
                " FROM tblTax_Maintenance LEFT JOIN tblCOA ON tblTax_Maintenance.AccountCode = tblCOA.AccountCode   " &
                " WHERE TaxCode = @TaxCode"
        SQL.FlushParams()
        SQL.AddParam("@TaxCode", TaxCode)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtTaxCode.Text = SQL.SQLDR("TaxCode").ToString
            ddlTaxType.SelectedValue = SQL.SQLDR("TaxType").ToString
            txtTaxDescription.Text = SQL.SQLDR("TaxDescription").ToString
            txtTaxRate.Text = SQL.SQLDR("TaxRate").ToString
            txtTaxAlias.Text = SQL.SQLDR("TaxAlias").ToString
            txtAccntCode.Text = SQL.SQLDR("AccountCode").ToString
            txtAccntName.Text = SQL.SQLDR("AccountTitle").ToString
            txtATC.Text = SQL.SQLDR("ATC").ToString
            txtNatureOfIncome.Text = SQL.SQLDR("NatureOfIncome").ToString
            btnSave.Text = "Update"
        Else
            btnSave.Text = "Save"
        End If
        gvTax.DataSource = SQL.SQLDS
        gvTax.DataBind()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                btnSave.Text = "Update"
                Initialize()
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
        query = " UPDATE tblTax_Maintenance SET TaxCode = @TaxCode, TaxType = @TaxType, TaxDescription = @TaxDescription, TaxRate = @TaxRate, " &
                " TaxAlias = @TaxAlias, AccountCode = @AccountCode, ATC = @ATC , NatureOfIncome = @NatureOfIncome, DateModified = @DateModified, " &
                " WhoModified = @WhoModified WHERE RecordID = @RecordID "
        SQL.FlushParams()
        SQL.AddParam("@RecordID", hfTransType.Value)
        SQL.AddParam("@TaxCode", txtTaxCode.Text)
        SQL.AddParam("@TaxType", ddlTaxType.SelectedValue)
        SQL.AddParam("@TaxDescription", txtTaxDescription.Text)
        SQL.AddParam("@TaxRate", txtTaxRate.Text)
        SQL.AddParam("@TaxAlias", txtTaxAlias.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@ATC", txtATC.Text)
        SQL.AddParam("@NatureOfIncome", txtNatureOfIncome.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblTax_Maintenance  " &
                " (TaxType, TaxCode, TaxDescription, TaxRate, TaxAlias,AccountCode, ATC, NatureOfIncome, DateCreated, WhoCreated)" &
                " VALUES" &
                " (@TaxType, @TaxCode, @TaxDescription, @TaxRate, @TaxAlias, @AccountCode, @ATC, @NatureOfIncome, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@TaxCode", txtTaxCode.Text)
        SQL.AddParam("@TaxType", ddlTaxType.SelectedValue)
        SQL.AddParam("@TaxDescription", txtTaxDescription.Text)
        SQL.AddParam("@TaxRate", txtTaxRate.Text)
        SQL.AddParam("@TaxAlias", txtTaxAlias.Text)
        SQL.AddParam("@AccountCode", txtAccntCode.Text)
        SQL.AddParam("@ATC", txtATC.Text)
        SQL.AddParam("@NatureOfIncome", txtNatureOfIncome.Text)
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
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

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        Initialize()
        btnSave.Text = "Save"
    End Sub
End Class