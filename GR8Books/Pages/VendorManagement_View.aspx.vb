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
                Dim dt As New DataTable
                dt.Columns.Add("")
                gvUpload.DataSource = dt
                gvUpload.DataBind()
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = "SELECT * FROM tblVendor_Master WHERE Status = @Status"
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

    <WebMethod()>
    Public Shared Function SaveVCE(Vendor_Code As String, Classification As String, First_Name As String, Last_Name As String, Middle_Name As String, Suffix_Name As String, Vendor_Name As String, TIN_No As String, BranchCode As String, Address_Lot_Unit As String, Address_Blk_Bldg As String, Address_Street As String, Address_Subd As String, Address_Brgy As String, Address_Town_City As String, Address_Province As String, Address_Region As String, Address_ZipCode As String, Contact_Person As String, Contact_Position As String, Contact_Telephone As String, Contact_Cellphone As String, Contact_Fax As String, Contact_Email As String, Contact_Website As String, Terms As String, CutOff As String, VAT_Type As String) As String
        Dim query As String
        query = " SELECT Vendor_Code FROM tblVendor_Master WHERE Vendor_Code = @Vendor_Code "
        SQL.FlushParams()
        SQL.AddParam("@Vendor_Code", Vendor_Code)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return "Exist"
        Else
            query = " INSERT INTO tblVendor_Master(Vendor_Code, Classification, First_Name, Last_Name, Middle_Name, Suffix_Name, Vendor_Name, TIN_No, BranchCode, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy,                          Address_Town_City, Address_Province, Address_Region, Address_ZipCode, Contact_Person, Contact_Position, Contact_Telephone, Contact_Cellphone, Contact_Fax, Contact_Email, Contact_Website, Terms, CutOff, VAT_Type) " & vbCrLf &
                    " VALUES(@Vendor_Code, @Classification, @First_Name, @Last_Name, @Middle_Name, @Suffix_Name, @Vendor_Name, @TIN_No, @BranchCode, @Address_Lot_Unit, @Address_Blk_Bldg, @Address_Street, @Address_Subd, @Address_Brgy, @Address_Town_City, @Address_Province, @Address_Region, @Address_ZipCode, @Contact_Person, @Contact_Position, @Contact_Telephone, @Contact_Cellphone, @Contact_Fax, @Contact_Email, @Contact_Website, @Terms, @CutOff, @VAT_Type) "
            SQL.FlushParams()
            SQL.AddParam("@Vendor_Code", IIf(Vendor_Code = "undefined", DBNull.Value, Vendor_Code))
            SQL.AddParam("@Classification", IIf(Classification = "undefined", DBNull.Value, Classification))
            SQL.AddParam("@First_Name", IIf(First_Name = "undefined", DBNull.Value, First_Name))
            SQL.AddParam("@Last_Name", IIf(Last_Name = "undefined", DBNull.Value, Last_Name))
            SQL.AddParam("@Middle_Name", IIf(Middle_Name = "undefined", DBNull.Value, Middle_Name))
            SQL.AddParam("@Suffix_Name", IIf(Suffix_Name = "undefined", DBNull.Value, Suffix_Name))
            SQL.AddParam("@Vendor_Name", IIf(Vendor_Name = "undefined", DBNull.Value, Vendor_Name))
            SQL.AddParam("@TIN_No", IIf(TIN_No = "undefined", DBNull.Value, TIN_No))
            SQL.AddParam("@BranchCode", IIf(BranchCode = "undefined", DBNull.Value, BranchCode))
            SQL.AddParam("@Address_Lot_Unit", IIf(Address_Lot_Unit = "undefined", DBNull.Value, Address_Lot_Unit))
            SQL.AddParam("@Address_Blk_Bldg", IIf(Address_Blk_Bldg = "undefined", DBNull.Value, Address_Blk_Bldg))
            SQL.AddParam("@Address_Street", IIf(Address_Street = "undefined", DBNull.Value, Address_Street))
            SQL.AddParam("@Address_Subd", IIf(Address_Subd = "undefined", DBNull.Value, Address_Subd))
            SQL.AddParam("@Address_Brgy", IIf(Address_Brgy = "undefined", DBNull.Value, Address_Brgy))
            SQL.AddParam("@Address_Town_City", IIf(Address_Town_City = "undefined", DBNull.Value, Address_Town_City))
            SQL.AddParam("@Address_Province", IIf(Address_Province = "undefined", DBNull.Value, Address_Province))
            SQL.AddParam("@Address_Region", IIf(Address_Region = "undefined", DBNull.Value, Address_Region))
            SQL.AddParam("@Address_ZipCode", IIf(Address_ZipCode = "undefined", DBNull.Value, Address_ZipCode))
            SQL.AddParam("@Contact_Person", IIf(Contact_Person = "undefined", DBNull.Value, Contact_Person))
            SQL.AddParam("@Contact_Position", IIf(Contact_Position = "undefined", DBNull.Value, Contact_Position))
            SQL.AddParam("@Contact_Telephone", IIf(Contact_Telephone = "undefined", DBNull.Value, Contact_Telephone))
            SQL.AddParam("@Contact_Cellphone", IIf(Contact_Cellphone = "undefined", DBNull.Value, Contact_Cellphone))
            SQL.AddParam("@Contact_Fax", IIf(Contact_Fax = "undefined", DBNull.Value, Contact_Fax))
            SQL.AddParam("@Contact_Email", IIf(Contact_Email = "undefined", DBNull.Value, Contact_Email))
            SQL.AddParam("@Contact_Website", IIf(Contact_Website = "undefined", DBNull.Value, Contact_Website))
            SQL.AddParam("@Terms", IIf(Terms = "undefined", DBNull.Value, Terms))
            SQL.AddParam("@CutOff", IIf(CutOff = "undefined", DBNull.Value, CutOff))
            SQL.AddParam("@VAT_Type", IIf(VAT_Type = "undefined", DBNull.Value, VAT_Type))

            SQL.ExecNonQuery(query)

            Return "False"
        End If
    End Function

    Private Sub gvVendor_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvVendor.PageIndexChanging
        gvVendor.PageIndex = e.NewPageIndex
        Loadlist()
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
End Class