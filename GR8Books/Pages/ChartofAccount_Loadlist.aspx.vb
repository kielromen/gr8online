﻿Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class ChartofAccount_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
                Dim dt As New DataTable
                dt.Columns.Add("")
                gvUpload.DataSource = dt
                gvUpload.DataBind()
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
                " WHERE Status = @Status AND AccountType = @AccountType ORDER BY OrderNo"
        SQL.FlushParams()
        SQL.AddParam("@Status", ddlFilter.SelectedValue)
        SQL.AddParam("@AccountType", ddlAccountType.SelectedValue)
        SQL.GetQuery(Query)
        gvCOA.DataSource = SQL.SQLDS
        gvCOA.DataBind()

        If ddlFilter.SelectedValue = "Active" Then
            For Each row As GridViewRow In gvCOA.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Inactive"
            Next row
        Else
            For Each row As GridViewRow In gvCOA.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Active"
            Next row
        End If
    End Sub

    Private Sub gvCOA_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCOA.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCOA SET Status = @Status WHERE AccountCode = @AccountCode"
            SQL.FlushParams()
            SQL.AddParam("@AccountCode", e.CommandArgument)
            SQL.AddParam("@Status", IIf(ddlFilter.SelectedValue = "Active", "Inactive", "Active"))
            SQL.ExecNonQuery(query)

            If ddlFilter.SelectedValue = "Active" Then
                Response.Write("<script>alert('Removed successfully');</script>")
            Else

                Response.Write("<script>alert('Put Back successfully');</script>")
            End If

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

    <WebMethod()>
    Public Shared Function SaveCOA(AccountCode As String, AccountTitle As String, AccountType As String, AccountGroup As String, AccountNature As String, ReportAlias As String, AccountClass As String, withSubsidiary As String, OrderNo As String) As String
        Dim query As String
        query = " DELETE FROM tblCOA WHERE AccountCode = @AccountCode "
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.ExecNonQuery(query)

        query = " INSERT INTO tblCOA(AccountCode, AccountTitle, AccountType, AccountGroup, AccountNature, ReportAlias, Class, withSubsidiary, OrderNo) " & vbCrLf &
                " VALUES(@AccountCode, @AccountTitle, @AccountType, @AccountGroup, @AccountNature, @ReportAlias, @Class, @withSubsidiary, @OrderNo) "
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", IIf(AccountCode = "undefined", "", AccountCode))
        SQL.AddParam("@AccountTitle", IIf(AccountTitle = "undefined", "", AccountTitle))
        SQL.AddParam("@AccountType", IIf(AccountType = "undefined", "", AccountType))
        SQL.AddParam("@AccountGroup", IIf(AccountGroup = "undefined", "SubAccount", AccountGroup))
        SQL.AddParam("@AccountNature", IIf(AccountNature = "undefined", "Debit", AccountNature))
        SQL.AddParam("@ReportAlias", IIf(ReportAlias = "undefined", "", ReportAlias))
        SQL.AddParam("@Class", IIf(AccountClass = "undefined", "Posting", AccountClass))
        SQL.AddParam("@withSubsidiary", IIf(withSubsidiary = "undefined", False, withSubsidiary))
        SQL.AddParam("@OrderNo", IIf(OrderNo = "undefined", 0, OrderNo))

        If SQL.ExecNonQuery(query) > 0 Then
            Return False
        Else
            Return "Exist"
        End If

    End Function

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvCOA.Rows.Count > 0 Then
            'To Export all pages
            gvCOA.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("COAlist")
            For Each cell As TableCell In gvCOA.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvCOA.Rows
                dt.Rows.Add()
                For i As Integer = 0 To row.Cells.Count - 1
                    row.Cells(i).CssClass = "textmode"
                    dt.Rows(dt.Rows.Count - 1)(i) = HttpUtility.HtmlDecode(row.Cells(i).Text.Trim)
                Next
            Next
            Using wb As New XLWorkbook()
                wb.Worksheets.Add(dt)
                Response.Clear()
                Response.Buffer = True
                Response.Charset = ""
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                Response.AddHeader("content-disposition", "attachment;filename=COAlist.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub

    Private Sub btnUploadSave_Click(sender As Object, e As EventArgs) Handles btnUploadSave.Click
        Response.Write("<script>window.location='ChartofAccount_Loadlist.aspx';</script>")
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim filename As String = "COA.xlsm"
        Dim filePath As String = (Server.MapPath("~/Templates/") + filename)
        Response.ContentType = ContentType
        Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
        Response.WriteFile(filePath)
        Response.End()
    End Sub
End Class