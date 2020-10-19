Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class VendorManagement_View
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = "SELECT        Vendor_Code, TIN_No, BranchCode, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy, Address_Town_City, Address_Province, Address_Region, Address_ZipCode, Contact_Person, 
                         Contact_Position, Contact_Telephone, Contact_Cellphone, Contact_Fax, Contact_Email, Contact_Website, Terms, CutOff, VAT_Type, Status, DateCreated, DateModified, WhoCreated, WhoModified, TransAuto, Classification, 
                         First_Name, Last_Name, Middle_Name, Suffix_Name,
						 CASE WHEN Classification = 'Individual' THEN CONCAT(Last_Name, ', ', First_name, ' ', Middle_Name,' ', Suffix_Name) ELSE Vendor_Name END AS Vendor_Name
                 FROM            dbo.tblVendor_Master WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvVendor.DataSource = SQL.SQLDS
        gvVendor.DataBind()
    End Sub

    Private Sub gvVendor_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVendor.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblVendor_Master SET Status = @Status WHERE Vendor_Code = @Vendor_Code"
            SQL.FlushParams()
            SQL.AddParam("@Vendor_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='VendorManagement_View.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvVendor.Rows.Count > 0 Then
            'To Export all pages
            gvVendor.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Vendorlist")
            For Each cell As TableCell In gvVendor.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvVendor.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Vendorlist.xlsx")
                Using MyMemoryStream As New MemoryStream()
                    wb.SaveAs(MyMemoryStream)
                    MyMemoryStream.WriteTo(Response.OutputStream)
                    Response.Flush()
                    Response.[End]()
                End Using
            End Using
        End If
    End Sub

    Private Sub gvVendor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvVendor.PageIndexChanging
        gvVendor.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub
End Class