﻿Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class BankMasterfile_LoadList
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
        ddlFilter.Items.Clear()
        ddlFilter.Items.Add("Active")
        ddlFilter.Items.Add("Inactive")
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT * FROM tblBank" &
                              " LEFT JOIN" &
                              " (SELECT AccountCode,AccountTitle FROM tblCOA) AS tblCOA  ON" &
                              " tblCOA.AccountCode = tblBank.AccountCode " &
                              " WHERE tblBank.Status = @Status AND Bank LIKE '%' + @Bank + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Status", ddlFilter.SelectedValue)
        SQL.AddParam("@Bank", txtFilter.Text)
        SQL.GetQuery(query)
        dgvBankList.DataSource = SQL.SQLDS
        dgvBankList.DataBind()

        If ddlFilter.SelectedValue = "Active" Then
            For Each row As GridViewRow In dgvBankList.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Inactive"
            Next row
        Else
            For Each row As GridViewRow In dgvBankList.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Active"
            Next row
        End If
    End Sub

    Private Sub dgvBankList_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles dgvBankList.PageIndexChanging
        dgvBankList.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub dgvBankList_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvBankList.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblBank SET Status = @Status WHERE ID = @ID"
            SQL.FlushParams()
            SQL.AddParam("@ID", e.CommandArgument)
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

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvBankList.Rows.Count > 0 Then
            'To Export all pages
            dgvBankList.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Banklist")
            For Each cell As TableCell In dgvBankList.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In dgvBankList.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Banklist.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub
End Class