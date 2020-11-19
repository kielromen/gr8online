Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class MemberManagement_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
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
        query = "SELECT * FROM tblMember_Master WHERE Status = @Status AND Member_Name LIKE '%' + @Member_Name + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Status", ddlFilter.SelectedValue)
        SQL.AddParam("@Member_Name", txtFilter.Text)
        SQL.GetQuery(query)
        gvMember.DataSource = SQL.SQLDS
        gvMember.DataBind()

        If ddlFilter.SelectedValue = "Active" Then
            For Each row As GridViewRow In gvMember.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Inactive"
            Next row
        Else
            For Each row As GridViewRow In gvMember.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Active"
            Next row
        End If
    End Sub

    Private Sub gvMember_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvMember.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblMember_Master SET Status = @Status WHERE Member_Code = @Member_Code"
            SQL.FlushParams()
            SQL.AddParam("@Member_Code", e.CommandArgument)
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

    Private Sub gvMember_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvMember.PageIndexChanging
        gvMember.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvMember.Rows.Count > 0 Then
            'To Export all pages
            gvMember.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Memberlist")
            For Each cell As TableCell In gvMember.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvMember.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Memberlist.xlsx")
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