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
        ddlFilter.Items.Clear()
        ddlFilter.Items.Add("Active")
        ddlFilter.Items.Add("Inactive")
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT Customer_Code, TIN_No, BranchCode, Billing_Lot_Unit, Billing_Blk_Bldg, Billing_Street, Billing_Subd, Billing_Brgy, Billing_Town_City, Billing_Province, Billing_Region, Billing_ZipCode, Delivery_Lot_Unit, 
                         Delivery_Blk_Bldg, Delivery_Street, Delivery_Subd, Delivery_Brgy, Delivery_Town_City, Delivery_Province, Delivery_Region, Delivery_ZipCode, SameAddress, Contact_Person, Contact_Position, Contact_Telephone, 
                         Contact_Cellphone, Contact_Fax, Contact_Email, Contact_Website, Terms, CutOff, VAT_Type, AccountNo, Status, DateCreated, DateModified, WhoCreated, WhoModified, TransAuto, Classification, First_Name, Last_Name, Middle_Name, Suffix_Name, 
						 CASE WHEN Classification = 'Individual' THEN CONCAT(Last_Name, ', ', First_name, ' ', Middle_Name,' ', Suffix_Name) ELSE Customer_Name END AS Customer_Name
                  FROM dbo.tblCustomer_Master WHERE Status = @Status AND CASE WHEN Classification = 'Individual' THEN CONCAT(Last_Name, ', ', First_name, ' ', Middle_Name,' ', Suffix_Name) ELSE Customer_Name END LIKE '%' + @Customer_Name + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Status", ddlFilter.SelectedValue)
        SQL.AddParam("@Customer_Name", txtFilter.Text)
        SQL.GetQuery(query)
        gvCustomer.DataSource = SQL.SQLDS
        gvCustomer.DataBind()


        If ddlFilter.SelectedValue = "Active" Then
            For Each row As GridViewRow In gvCustomer.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Inactive"
            Next row
        Else
            For Each row As GridViewRow In gvCustomer.Rows
                Dim Inactive As Button = CType(row.FindControl("btnInactive"), Button)
                Inactive.Text = "Active"
            Next row
        End If
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

    <WebMethod()>
    Public Shared Function SaveVCE(Customer_Code As String, Classification As String, First_Name As String, Last_Name As String, Middle_Name As String, Suffix_Name As String, Customer_Name As String, TIN_No As String, BranchCode As String, Billing_Lot_Unit As String, Billing_Blk_Bldg As String, Billing_Street As String, Billing_Subd As String, Billing_Brgy As String, Billing_Town_City As String, Billing_Province As String, Billing_Region As String, Billing_ZipCode As String, Delivery_Lot_Unit As String, Delivery_Blk_Bldg As String, Delivery_Street As String, Delivery_Subd As String, Delivery_Brgy As String, Delivery_Town_City As String, Delivery_Province As String, Delivery_Region As String, Delivery_ZipCode As String, SameAddress As String, Contact_Person As String, Contact_Position As String, Contact_Telephone As String, Contact_Cellphone As String, Contact_Fax As String, Contact_Email As String, Contact_Website As String, Terms As String, CutOff As String, VAT_Type As String, AccountNo As String) As String
        Dim query As String
        query = " SELECT Customer_Code FROM tblCustomer_Master WHERE Customer_Code = @Customer_Code "
        SQL.FlushParams()
        SQL.AddParam("@Customer_Code", Customer_Code)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return "Exist"
        Else
            query = " INSERT INTO tblCustomer_Master(Customer_Code, Classification, First_Name, Last_Name, Middle_Name, Suffix_Name, Customer_Name, TIN_No, BranchCode, Billing_Lot_Unit, Billing_Blk_Bldg, Billing_Street, Billing_Subd, Billing_Brgy, Billing_Town_City, Billing_Province, Billing_Region, Billing_ZipCode, Delivery_Lot_Unit, Delivery_Blk_Bldg, Delivery_Street, Delivery_Subd, Delivery_Brgy, Delivery_Town_City, Delivery_Province, Delivery_Region, Delivery_ZipCode, SameAddress, Contact_Person, Contact_Position, Contact_Telephone, Contact_Cellphone, Contact_Fax, Contact_Email, Contact_Website, Terms, CutOff, VAT_Type) " & vbCrLf &
                    " VALUES (@Customer_Code, @Classification, @First_Name, @Last_Name, @Middle_Name, @Suffix_Name, @Customer_Name, @TIN_No, @BranchCode, @Billing_Lot_Unit, @Billing_Blk_Bldg, @Billing_Street, @Billing_Subd, @Billing_Brgy, @Billing_Town_City, @Billing_Province, @Billing_Region, @Billing_ZipCode, @Delivery_Lot_Unit, @Delivery_Blk_Bldg, @Delivery_Street, @Delivery_Subd, @Delivery_Brgy, @Delivery_Town_City, @Delivery_Province, @Delivery_Region, @Delivery_ZipCode, @SameAddress, @Contact_Person, @Contact_Position, @Contact_Telephone, @Contact_Cellphone, @Contact_Fax, @Contact_Email, @Contact_Website, @Terms, @CutOff, @VAT_Type) "
            SQL.FlushParams()
            SQL.AddParam("@Customer_Code", IIf(Customer_Code = "undefined", DBNull.Value, Customer_Code))
            SQL.AddParam("@Classification", IIf(Classification = "undefined", DBNull.Value, Classification))
            SQL.AddParam("@First_Name", IIf(First_Name = "undefined", DBNull.Value, First_Name))
            SQL.AddParam("@Last_Name", IIf(Last_Name = "undefined", DBNull.Value, Last_Name))
            SQL.AddParam("@Middle_Name", IIf(Middle_Name = "undefined", DBNull.Value, Middle_Name))
            SQL.AddParam("@Suffix_Name", IIf(Suffix_Name = "undefined", DBNull.Value, Suffix_Name))
            SQL.AddParam("@Customer_Name", IIf(Customer_Name = "undefined", DBNull.Value, Customer_Name))
            SQL.AddParam("@TIN_No", IIf(TIN_No = "undefined", DBNull.Value, TIN_No))
            SQL.AddParam("@BranchCode", IIf(BranchCode = "undefined", DBNull.Value, BranchCode))
            SQL.AddParam("@Billing_Lot_Unit", IIf(Billing_Lot_Unit = "undefined", DBNull.Value, Billing_Lot_Unit))
            SQL.AddParam("@Billing_Blk_Bldg", IIf(Billing_Blk_Bldg = "undefined", DBNull.Value, Billing_Blk_Bldg))
            SQL.AddParam("@Billing_Street", IIf(Billing_Street = "undefined", DBNull.Value, Billing_Street))
            SQL.AddParam("@Billing_Subd", IIf(Billing_Subd = "undefined", DBNull.Value, Billing_Subd))
            SQL.AddParam("@Billing_Brgy", IIf(Billing_Brgy = "undefined", DBNull.Value, Billing_Brgy))
            SQL.AddParam("@Billing_Town_City", IIf(Billing_Town_City = "undefined", DBNull.Value, Billing_Town_City))
            SQL.AddParam("@Billing_Province", IIf(Billing_Province = "undefined", DBNull.Value, Billing_Province))
            SQL.AddParam("@Billing_Region", IIf(Billing_Region = "undefined", DBNull.Value, Billing_Region))
            SQL.AddParam("@Billing_ZipCode", IIf(Billing_ZipCode = "undefined", DBNull.Value, Billing_ZipCode))
            SQL.AddParam("@Delivery_Lot_Unit", IIf(Delivery_Lot_Unit = "undefined", DBNull.Value, Delivery_Lot_Unit))
            SQL.AddParam("@Delivery_Blk_Bldg", IIf(Delivery_Blk_Bldg = "undefined", DBNull.Value, Delivery_Blk_Bldg))
            SQL.AddParam("@Delivery_Street", IIf(Delivery_Street = "undefined", DBNull.Value, Delivery_Street))
            SQL.AddParam("@Delivery_Subd", IIf(Delivery_Subd = "undefined", DBNull.Value, Delivery_Subd))
            SQL.AddParam("@Delivery_Brgy", IIf(Delivery_Brgy = "undefined", DBNull.Value, Delivery_Brgy))
            SQL.AddParam("@Delivery_Town_City", IIf(Delivery_Town_City = "undefined", DBNull.Value, Delivery_Town_City))
            SQL.AddParam("@Delivery_Province", IIf(Delivery_Province = "undefined", DBNull.Value, Delivery_Province))
            SQL.AddParam("@Delivery_Region", IIf(Delivery_Region = "undefined", DBNull.Value, Delivery_Region))
            SQL.AddParam("@Delivery_ZipCode", IIf(Delivery_ZipCode = "undefined", DBNull.Value, Delivery_ZipCode))
            SQL.AddParam("@SameAddress", IIf(SameAddress = "undefined", False, SameAddress))
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
            SQL.AddParam("@AccountNo", IIf(VAT_Type = "undefined", DBNull.Value, AccountNo))
            SQL.ExecNonQuery(query)
            Return "False"
        End If
    End Function

    Private Sub btnUploadSave_Click(sender As Object, e As EventArgs) Handles btnUploadSave.Click
        Response.Write("<script>window.location='CustomerManagement_View.aspx';</script>")
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim filename As String = "Customer.xlsm"
        Dim filePath As String = (Server.MapPath("~/Templates/") + filename)
        Response.ContentType = ContentType
        Response.AppendHeader("Content-Disposition", ("attachment; filename=" + Path.GetFileName(filePath)))
        Response.WriteFile(filePath)
        Response.End()
    End Sub
End Class