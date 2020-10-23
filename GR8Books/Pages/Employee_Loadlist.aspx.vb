Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Services
Imports ClosedXML.Excel
Public Class Employee_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
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
        query = " SELECT  CASE WHEN Last_Name IS NULL AND First_Name IS NULL THEN Employee_Name ELSE CONCAT(ISNULL(Last_Name, ''), ', ', ISNULL(First_Name, ''), ' ', ISNULL(Middle_Name, ''), ' ', ISNULL(Suffix_Name, '')) END AS Employee_Name, * FROM tblEmployee_Master " & vbCrLf &
                " WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvEmployee.DataSource = SQL.SQLDS
        gvEmployee.DataBind()
    End Sub

    Private Sub gvEmployee_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvEmployee.PageIndexChanging
        gvEmployee.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvEmployee_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvEmployee.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblEmployee_Master SET Status = @Status WHERE Employee_Code = @Employee_Code"
            SQL.FlushParams()
            SQL.AddParam("@Employee_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='Employee_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If gvEmployee.Rows.Count > 0 Then
            'To Export all pages
            gvEmployee.AllowPaging = False
            Me.Loadlist()

            Dim dt As New DataTable("Employeelist")
            For Each cell As TableCell In gvEmployee.HeaderRow.Cells
                dt.Columns.Add(cell.Text)
            Next
            For Each row As GridViewRow In gvEmployee.Rows
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
                Response.AddHeader("content-disposition", "attachment;filename=Employeelist.xlsx")
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
    Public Shared Function SaveVCE(Employee_Code As String, Suffix_Name As String, First_Name As String, Middle_Name As String, Last_Name As String, Employee_Name As String, Department As String, Section As String, Unit As String, Address_Lot_Unit As String, Address_Blk_Bldg As String, Address_Street As String, Address_Subd As String, Address_Brgy As String, Address_Town_City As String, Address_Province As String, Address_Region As String, Address_ZipCode As String, EmailAddress As String, CellphoneNo As String) As String
        Dim query As String
        query = " SELECT Employee_Code FROM tblEmployee_Master WHERE Employee_Code = @Employee_Code "
        SQL.FlushParams()
        SQL.AddParam("@Employee_Code", Employee_Code)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return "Exist"
        Else
            query = " INSERT INTO tblEmployee_Master(Employee_Code, Suffix_Name, First_Name, Middle_Name, Last_Name, Employee_Name, Department, Section, Unit, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy, Address_Town_City, Address_Province, Address_Region, Address_ZipCode, EmailAddress, CellphoneNo) " & vbCrLf &
                    " VALUES (@Employee_Code, @Suffix_Name, @First_Name, @Middle_Name, @Last_Name, @Employee_Name, @Department, @Section, @Unit, @Address_Lot_Unit, @Address_Blk_Bldg, @Address_Street, @Address_Subd, @Address_Brgy, @Address_Town_City, @Address_Province, @Address_Region, @Address_ZipCode, @EmailAddress, @CellphoneNo) "
            SQL.FlushParams()
            SQL.AddParam("@Employee_Code", IIf(Employee_Code = "undefined", DBNull.Value, Employee_Code))
            SQL.AddParam("@Suffix_Name", IIf(Suffix_Name = "undefined", DBNull.Value, Suffix_Name))
            SQL.AddParam("@First_Name", IIf(First_Name = "undefined", DBNull.Value, First_Name))
            SQL.AddParam("@Middle_Name", IIf(Middle_Name = "undefined", DBNull.Value, Middle_Name))
            SQL.AddParam("@Last_Name", IIf(Last_Name = "undefined", DBNull.Value, Last_Name))
            SQL.AddParam("@Employee_Name", IIf(Employee_Name = "undefined", DBNull.Value, Employee_Name))
            SQL.AddParam("@Department", IIf(Department = "undefined", DBNull.Value, Department))
            SQL.AddParam("@Section", IIf(Section = "undefined", DBNull.Value, Section))
            SQL.AddParam("@Unit", IIf(Unit = "undefined", DBNull.Value, Unit))
            SQL.AddParam("@Address_Lot_Unit", IIf(Address_Lot_Unit = "undefined", DBNull.Value, Address_Lot_Unit))
            SQL.AddParam("@Address_Blk_Bldg", IIf(Address_Blk_Bldg = "undefined", DBNull.Value, Address_Blk_Bldg))
            SQL.AddParam("@Address_Street", IIf(Address_Street = "undefined", DBNull.Value, Address_Street))
            SQL.AddParam("@Address_Subd", IIf(Address_Subd = "undefined", DBNull.Value, Address_Subd))
            SQL.AddParam("@Address_Brgy", IIf(Address_Brgy = "undefined", DBNull.Value, Address_Brgy))
            SQL.AddParam("@Address_Town_City", IIf(Address_Town_City = "undefined", DBNull.Value, Address_Town_City))
            SQL.AddParam("@Address_Province", IIf(Address_Province = "undefined", DBNull.Value, Address_Province))
            SQL.AddParam("@Address_Region", IIf(Address_Region = "undefined", DBNull.Value, Address_Region))
            SQL.AddParam("@Address_ZipCode", IIf(Address_ZipCode = "undefined", DBNull.Value, Address_ZipCode))
            SQL.AddParam("@EmailAddress", IIf(EmailAddress = "undefined", DBNull.Value, EmailAddress))
            SQL.AddParam("@CellphoneNo", IIf(CellphoneNo = "undefined", DBNull.Value, CellphoneNo))
            SQL.ExecNonQuery(query)
            Return "False"
        End If
    End Function
End Class