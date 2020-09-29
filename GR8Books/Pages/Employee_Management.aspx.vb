Imports System.Web.Services
Public Class Employee_Management
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ModuleID As String = "Employee"
    Dim ColumnPK As String = "Employee_Code"
    Dim DBTable As String = "tblEmployee_Master"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            End If
        End If
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelEmployee.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtEmployee_Code.Attributes.Add("readonly", "readonly")
        txtEmployee_Code.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtMiddleName.Text = ""
        txtSuffixName.Text = ""
        txtDepartment.Text = ""
        txtSection.Text = ""
        txtUnit.Text = ""
        txtAddress_Blk_Bldg.Text = ""
        txtAddress_Lot_Unit.Text = ""
        txtStreet.Text = ""
        txtSubdivision.Text = ""
        txtZipCode.Text = ""
        txtCellphoneNo.Text = ""
        txtEmail.Text = ""
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
        ddlRegion.Items.Clear()
        ddlRegion.Items.Add("--Select Region--")
        ddlRegion.DataSource = LoadtblAddress_Region().ToArray
        ddlRegion.DataBind()
    End Sub

    Public Sub Region_Changed()
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlProvince.DataSource = LoadtblAddress_Province(ddlRegion.SelectedValue).ToArray
        ddlProvince.DataBind()
    End Sub

    Public Sub Province_Changed()
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlCityMun.DataSource = LoadtblAddress_CityMunicipality(ddlProvince.SelectedValue).ToArray
        ddlCityMun.DataBind()
    End Sub

    Public Sub CityMun_Changed()
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
        ddlBarangay.DataSource = LoadtblAddress_Brgy(ddlCityMun.SelectedValue).ToArray
        ddlBarangay.DataBind()
    End Sub

    Private Sub Employee_Management_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ID As String = Request.QueryString("Employee_Code")
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
                txtEmployee_Code.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Public Sub View()
        Dim Employee_Code As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblEmployee_Master " &
                " WHERE tblEmployee_Master.Status = @Status AND Employee_Code = @Employee_Code"
        SQL.FlushParams()
        SQL.AddParam("@Employee_Code", Employee_Code)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtEmployee_Code.Text = SQL.SQLDR("Employee_Code").ToString
            txtFirstName.Text = SQL.SQLDR("First_Name").ToString
            txtLastName.Text = SQL.SQLDR("Last_Name").ToString
            txtMiddleName.Text = SQL.SQLDR("Middle_Name").ToString
            txtSuffixName.Text = SQL.SQLDR("Suffix_Name").ToString
            txtDepartment.Text = SQL.SQLDR("Department").ToString
            txtSection.Text = SQL.SQLDR("Section").ToString
            txtUnit.Text = SQL.SQLDR("Unit").ToString
            txtAddress_Blk_Bldg.Text = SQL.SQLDR("Address_Blk_Bldg").ToString
            txtAddress_Lot_Unit.Text = SQL.SQLDR("Address_Lot_Unit").ToString
            txtAddress_Blk_Bldg.Text = SQL.SQLDR("Address_Street").ToString
            txtAddress_Lot_Unit.Text = SQL.SQLDR("Address_Subd").ToString
            ddlRegion.SelectedValue = SQL.SQLDR("Address_Region").ToString
            Region_Changed()
            ddlProvince.SelectedValue = SQL.SQLDR("Address_Province").ToString
            Province_Changed()
            ddlCityMun.SelectedValue = SQL.SQLDR("Address_Town_City").ToString
            CityMun_Changed()
            ddlBarangay.SelectedValue = SQL.SQLDR("Address_Brgy").ToString
            txtZipCode.Text = SQL.SQLDR("Address_ZipCode").ToString
            txtCellphoneNo.Text = SQL.SQLDR("CellphoneNo").ToString
            txtEmail.Text = SQL.SQLDR("EmailAddress").ToString
        End If
    End Sub

    Public Sub Save()
        txtEmployee_Code.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        query = " INSERT INTO tblEmployee_Master " &
                " (Employee_Code, Suffix_Name, First_Name, Middle_Name, Last_Name, Department, Section, Unit, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy, Address_Town_City, " &
                "  Address_Province, Address_Region, Address_ZipCode, CellphoneNo,EmailAddress, Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@Employee_Code, @Suffix_Name, @First_Name, @Middle_Name, @Last_Name, @Department, @Section, @Unit, @Address_Lot_Unit, @Address_Blk_Bldg, @Address_Street, @Address_Subd, @Address_Brgy, @Address_Town_City, " &
                "  @Address_Province, @Address_Region, @Address_ZipCode, @CellphoneNo, @EmailAddress, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Employee_Code", txtEmployee_Code.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Department", txtDepartment.Text)
        SQL.AddParam("@Section", txtSection.Text)
        SQL.AddParam("@Unit", txtUnit.Text)
        SQL.AddParam("@Address_Lot_Unit", txtAddress_Lot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtAddress_Blk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtStreet.Text)
        SQL.AddParam("@Address_Subd", txtSubdivision.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtZipCode.Text)
        SQL.AddParam("@CellphoneNo", txtCellphoneNo.Text)
        SQL.AddParam("@EmailAddress", txtEmail.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim Employee_Code As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblEmployee_Master " &
                " SET Suffix_Name = @Suffix_Name, First_Name = @First_Name, Middle_Name = @Middle_Name, " & vbCrLf &
                "     Last_Name = @Last_Name, Department = @Department, Section = @Section, Unit = @Unit, " & vbCrLf &
                "     Address_Lot_Unit = @Address_Lot_Unit, Address_Blk_Bldg = @Address_Blk_Bldg, Address_Street = @Address_Street, " & vbCrLf &
                "     Address_Subd = @Address_Subd, Address_Brgy = @Address_Brgy, Address_Town_City = @Address_Town_City, " &
                "     Address_Province = @Address_Province, Address_Region = @Address_Region, Address_ZipCode = @Address_ZipCode, " & vbCrLf &
                "     CellphoneNo = @CellphoneNo, EmailAddress = @EmailAddress, DateModified = @DateModified, WhoModified = @WhoModified" &
                " WHERE Employee_Code = @Employee_Code"
        SQL.FlushParams()
        SQL.AddParam("@Employee_Code", txtEmployee_Code.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Department", txtDepartment.Text)
        SQL.AddParam("@Section", txtSection.Text)
        SQL.AddParam("@Unit", txtUnit.Text)
        SQL.AddParam("@Address_Lot_Unit", txtAddress_Lot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtAddress_Blk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtStreet.Text)
        SQL.AddParam("@Address_Subd", txtSubdivision.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtZipCode.Text)
        SQL.AddParam("@CellphoneNo", txtCellphoneNo.Text)
        SQL.AddParam("@EmailAddress", txtEmail.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='Employee_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='Employee_Loadlist.aspx';</script>")
        End If
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
    End Sub
End Class