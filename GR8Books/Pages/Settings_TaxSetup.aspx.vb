Imports System.Web.Services
Public Class Settings_TaxSetup
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
        txtAP_InputVAT.Attributes.Add("readonly", "readonly")
        txtAR_OutputVAT.Attributes.Add("readonly", "readonly")
        txtTAX_Deferred.Attributes.Add("readonly", "readonly")
        txtTAX_EWT.Attributes.Add("readonly", "readonly")
        txtTAX_CWT.Attributes.Add("readonly", "readonly")
        txtAP_InputVAT.Text = ""
        txtAP_InputVAT_Title.Text = ""
        txtAR_OutputVAT.Text = ""
        txtAR_OutputVAT_Title.Text = ""
        txtTAX_Deferred.Text = ""
        txtTAX_Deferred_Title.Text = ""
        txtTAX_EWT.Text = ""
        txtTAX_EWT_Title.Text = ""
        txtTAX_CWT.Text = ""
        txtTAX_CWT_Title.Text = ""
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


    Public Sub LoadTaxDefaultAccount()
        Dim query As String

        query = " SELECT AP_InputVAT, AccountTitle" &
                " FROM tblSystemSetup  " &
                " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
                " ON tblSystemSetup.AP_InputVAT = tblCOA.AccountCode "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtAP_InputVAT.Text = SQL.SQLDR("AP_InputVAT").ToString
            txtAP_InputVAT_Title.Text = SQL.SQLDR("AccountTitle").ToString
        End If

        query = " SELECT AR_OutputVAT, AccountTitle" &
         " FROM tblSystemSetup  " &
         " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
         " ON tblSystemSetup.AR_OutputVAT = tblCOA.AccountCode "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtAR_OutputVAT.Text = SQL.SQLDR("AR_OutputVAT").ToString
            txtAR_OutputVAT_Title.Text = SQL.SQLDR("AccountTitle").ToString
        End If

        query = " SELECT TAX_Deferred, AccountTitle" &
         " FROM tblSystemSetup  " &
         " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
         " ON tblSystemSetup.TAX_Deferred = tblCOA.AccountCode "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtTAX_Deferred.Text = SQL.SQLDR("TAX_Deferred").ToString
            txtTAX_Deferred_Title.Text = SQL.SQLDR("AccountTitle").ToString
        End If

        query = " SELECT TAX_EWT, AccountTitle" &
         " FROM tblSystemSetup  " &
         " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
         " ON tblSystemSetup.TAX_EWT = tblCOA.AccountCode "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtTAX_EWT.Text = SQL.SQLDR("TAX_EWT").ToString
            txtTAX_EWT_Title.Text = SQL.SQLDR("AccountTitle").ToString
        End If

        query = " SELECT TAX_CWT, AccountTitle" &
         " FROM tblSystemSetup  " &
         " INNER JOIN (SELECT AccountCode, AccountTitle FROM tblCOA) AS tblCOA " &
         " ON tblSystemSetup.TAX_CWT = tblCOA.AccountCode "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtTAX_CWT.Text = SQL.SQLDR("TAX_CWT").ToString
            txtTAX_CWT_Title.Text = SQL.SQLDR("AccountTitle").ToString
        End If
    End Sub

    Private Sub Settings_TaxSetup_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        LoadTaxDefaultAccount()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            Save()
            Response.Write("<script>alert('Successfully Updated.');</script>")
            Response.Write("<script>opener.location.reload();</script>")
            Response.Write("<script>window.close();</script>")
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " UPDATE tblSystemSetup " &
                " SET  AP_InputVAT = @AP_InputVAT, AR_OutputVAT = @AR_OutputVAT, TAX_Deferred = @TAX_Deferred, TAX_EWT = @TAX_EWT, " &
                " TAX_CWT = @TAX_CWT, DateModified = @DateModified, WhoModified = @WhoModified "
        SQL.FlushParams()
        SQL.AddParam("@ID", ID)
        SQL.AddParam("@AP_InputVAT", txtAP_InputVAT.Text)
        SQL.AddParam("@AR_OutputVAT", txtAR_OutputVAT.Text)
        SQL.AddParam("@TAX_Deferred", txtTAX_Deferred.Text)
        SQL.AddParam("@TAX_EWT", txtTAX_EWT.Text)
        SQL.AddParam("@TAX_CWT", txtTAX_CWT.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub
End Class