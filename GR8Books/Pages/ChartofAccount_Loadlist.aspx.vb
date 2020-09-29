Imports System.Web.Services
Public Class ChartofAccount_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Initialize()
        btnSort.Style.Add("display", "none")
        ddlAccountType.Items.Clear()
        ddlAccountType.Items.Add("Balance Sheet")
        ddlAccountType.Items.Add("Income Statement")
        ddlFilter.Items.Clear()
        ddlFilter.Items.Add("Active")
        ddlFilter.Items.Add("Inactive")
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT    AccountCode, AccountType, AccountTitle , AccountGroup, AccountNature, ReportAlias, Class, withSubsidiary, OrderNo, Status FROM tblCOA " &
                " WHERE Status = @Status AND AccountType = '" & ddlAccountType.SelectedValue & "'ORDER BY OrderNo"
        SQL.FlushParams()
        SQL.AddParam("@Status", ddlFilter.SelectedValue)
        SQL.GetQuery(Query)
        gvCOA.DataSource = SQL.SQLDS
        gvCOA.DataBind()
    End Sub

    Private Sub gvCOA_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCOA.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCOA SET Status = @Status WHERE AccountCode = @AccountCode"
            SQL.FlushParams()
            SQL.AddParam("@AccountCode", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');</script>")
            Loadlist()
        End If
    End Sub


    Protected Sub UpdateSort(sender As Object, e As EventArgs)
        Try
            Dim Codes As String() = Request.Form.GetValues("Code")
            Dim sortNumber As Integer = 1

            For Each Code In Codes
                Me.UpdateSort(Code, sortNumber)
                sortNumber += 1
            Next
            Loadlist()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub UpdateSort(AccountCode As String, OrderNo As Integer)
        Dim query As String
        query = "Update tblCOA Set OrderNo = @OrderNo WHERE AccountCode = @AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.AddParam("@OrderNo", OrderNo)
        SQL.ExecNonQuery(query)
    End Sub
End Class