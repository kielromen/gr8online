Imports System.Web.Services
Public Class CreditMemo_Maintenance
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

    <WebMethod()>
    Public Shared Function ListVCE(prefix As String) As String()
        Dim strName As New List(Of String)()
        Dim query As String
        query = "SELECT Code, Name FROM View_VCEMMaster " & vbCrLf &
                "WHERE Status = 'Active' AND Name LIKE '%' + @Name + '%' AND Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Name", prefix)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            strName.Add(String.Format("{0}--{1}", SQL.SQLDR("Name"), SQL.SQLDR("Code")))
        End While
        Return strName.ToArray()
    End Function
    Public Sub Initialize()
        txtCustomerCode.Text = ""
        txtCustomerName.Text = ""
        txtCreditMemoTo.Text = ""
        txtChargeToName.Text = ""
        txtRemarks.Text = ""
        dtpDoc_Date.Value = Format(Date.Now, "yyyy-MM-dd")
        txtCMno.Text = AutoCMNO()
        TxtAmount.Text = ""
    End Sub

    Public Function AutoCMNO()
        Dim TransNum As String = ""
        Dim query As String
        query = " SELECT    ISNULL(MAX(SUBSTRING(CM_No,3+1,1))+ 1,1) AS TransID " &
                " FROM tblCM "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransNum = SQL.SQLDR("TransID")
            For i As Integer = 1 To 3
                TransNum = "0" & TransNum
            Next
            TransNum = Strings.Right(TransNum, 3)
        End If
        Return TransNum
    End Function

    Public Sub EnableControl(ByVal Value As Boolean)
        txtCustomerCode.ReadOnly = Value
        txtCustomerName.ReadOnly = Value
        txtCreditMemoTo.ReadOnly = Value
        txtChargeToName.ReadOnly = Value
        txtRemarks.ReadOnly = Value
        txtCMno.ReadOnly = Value
        TxtAmount.ReadOnly = Value
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If btnSave.Text = "Save" Then
            Save()
            Response.Write("<script>alert('Successfully Saved.');window.location='CreditMemo_Entry.aspx';</script>")
        ElseIf btnSave.Text = "Update" Then
            Update()
            Response.Write("<script>alert('Successfully Updated.');</script>")
            Response.Write("<script>opener.location.reload();</script>")
            Response.Write("<script>window.close();</script>")
        End If
    End Sub

    Public Sub Save()
        Dim query As String
        query = " INSERT INTO tblCM " &
                " ( CM_No, BranchCode, BusinessCode, TotalAmount, TransDate, CM_Type, CreditAccount, Customer, CreditMemoTo, Remarks, Status, DateCreated, WhoCreated, TransAuto)" &
                " VALUES " &
                " ( @CM_No, @BranchCode, @BusinessCode, @TotalAmount, @TransDate, @CM_Type, @CreditAccount, @Customer, @CreditMemoTo, @Remarks, @Status, @DateCreated, @WhoCreated,  @TransAuto)"
        SQL.FlushParams()
        SQL.AddParam("@CM_No", txtCMno.Text)
        SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@BusinessCode", "")
        SQL.AddParam("@TotalAmount", TxtAmount.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@CM_Type", txtCreditType.Text)
        SQL.AddParam("@CreditAccount", txtCreditCode.Text)
        SQL.AddParam("@Customer", txtCustomerCode.Text)
        SQL.AddParam("@CreditMemoTo", txtCreditMemoTo.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.AddParam("@TransAuto", True)
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("TransID")
        Dim query As String
        query = " UPDATE tblCM " &
                " SET CM_No = @CM_No, BranchCode = @BranchCode, " &
                " TotalAmount = @TotalAmount,  TransDate = @TransDate,  CM_Type = @CM_Type,  CreditAccount = @CreditAccount, " &
                " Customer = @Customer,CreditMemoTo=@CreditMemoTo, Remarks=@Remarks, DateModified = @DateModified, @WhoModified = @WhoModified " &
                " WHERE TransID = @TransID"
        SQL.FlushParams()
        SQL.AddParam("@TransID", ID)
        SQL.AddParam("@CM_No", txtCMno.Text)
        SQL.AddParam("@BranchCode", "")
        SQL.AddParam("@BusinessCode", "")
        SQL.AddParam("@TotalAmount", TxtAmount.Text)
        SQL.AddParam("@TransDate", dtpDoc_Date.Value)
        SQL.AddParam("@CM_Type", txtCreditType.Text)
        SQL.AddParam("@CreditAccount", txtCreditCode.Text)
        SQL.AddParam("@Customer", txtCustomerCode.Text)
        SQL.AddParam("@CreditMemoTo", txtCreditMemoTo.Text)
        SQL.AddParam("@Remarks", txtRemarks.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub CreditMemo_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ID As String = Request.QueryString("TransID")
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
        Dim ID As String = Request.QueryString("TransID")
        Dim query As String
        query = " SELECT TransID, CM_No, BranchCode, BusinessCode, TotalAmount,  " &
                " TransDate, CM_Type, CreditAccount, AccountTitle, Customer, CreditMemoTo, Remarks " &
                "  FROM tblCM" &
                " LEFT JOIN" &
                " tblCOA ON" &
                " tblCOA.AccountCode = tblCM.CreditAccount " &
                " WHERE tblCM.Status = @Status AND TransID = @TransID"
        SQL.FlushParams()
        SQL.AddParam("@TransID", ID)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtCMno.Text = SQL.SQLDR("CM_No").ToString
            TxtAmount.Text = CDec(SQL.SQLDR("TotalAmount").ToString)
            dtpDoc_Date.Value = CDate(SQL.SQLDR("TransDate")).ToString("yyyy-MM-dd")
            txtCreditType.Text = SQL.SQLDR("CM_Type").ToString
            txtCreditCode.Text = SQL.SQLDR("CreditAccount").ToString
            txtCreditTitle.Text = SQL.SQLDR("AccountTitle").ToString
            txtCustomerCode.Text = SQL.SQLDR("Customer").ToString
            txtCreditMemoTo.Text = SQL.SQLDR("CreditMemoTo").ToString
            txtRemarks.Text = SQL.SQLDR("Remarks").ToString
            txtCustomerName.Text = GetVCEName(txtCustomerCode.Text)
            txtCreditMemoTo.Text = GetVCEName(txtCreditMemoTo.Text)
        End If
    End Sub
End Class