Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class CustomerManagement_View
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
        query = " SELECT        Customer_Code, TIN_No, Billing_Lot_Unit, Billing_Blk_Bldg, Billing_Street, Billing_Subd, Billing_Brgy, Billing_Town_City, Billing_Province, Billing_Region, Billing_ZipCode, Delivery_Lot_Unit, 
                         Delivery_Blk_Bldg, Delivery_Street, Delivery_Subd, Delivery_Brgy, Delivery_Town_City, Delivery_Province, Delivery_Region, Delivery_ZipCode, SameAddress, Contact_Person, Contact_Position, Contact_Telephone, 
                         Contact_Cellphone, Contact_Fax, Contact_Email, Contact_Website, Terms, CutOff, VAT_Type, Status, DateCreated, DateModified, WhoCreated, WhoModified, TransAuto, Classification, First_Name, Last_Name, Middle_Name, Suffix_Name, 
						 CASE WHEN Classification = 'Individual' THEN CONCAT(Last_Name, ', ', First_name, ' ', Middle_Name,' ', Suffix_Name) ELSE Customer_Name END AS Customer_Name
                  FROM dbo.tblCustomer_Master WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvCustomer.DataSource = SQL.SQLDS
        gvCustomer.DataBind()
    End Sub

    Private Sub gvCustomer_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvCustomer.PageIndexChanging
        gvCustomer.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvCustomer_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCustomer.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblCustomer_Master SET Status = @Status WHERE Customer_Code = @Customer_Code"
            SQL.FlushParams()
            SQL.AddParam("@Customer_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='CustomerManagement_View.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvCustomer.Rows.Count > 0 Then
            'To Export all pages
            gvCustomer.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Customerlist")
            For Each cell As TableCell In gvCustomer.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvCustomer.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Customerlist.xlsx")
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