Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class VendorManagement
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ModuleID As String = "Vendor"
    Dim ColumnPK As String = "Vendor_Code"
    Dim DBTable As String = "tblVendor_Master"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='VendorManagement_View.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    Public Sub Save()
        txtVendorCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        query = " INSERT INTO tblVendor_Master " &
                        " (Vendor_Code, Vendor_Name, TIN_No, BranchCode, First_Name,Last_Name,Middle_Name, Suffix_Name, Classification, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, " &
                        " Address_Subd, Address_Brgy, Address_Town_City, Address_Province, Address_Region, Address_ZipCode, Contact_Person, Contact_Position, Contact_Telephone," &
                        " Contact_Cellphone , Contact_Email , Contact_Fax, Contact_Website, Terms, CutOff, VAT_Type, AccountNo, Status, DateCreated, WhoCreated)" &
                        " VALUES " &
                        " (@VendorCode, @Vendor_Name, @TIN_No, @BranchCode, @First_Name,@Last_Name,@Middle_Name, @Suffix_Name, @Classification, @Address_Lot_Unit,@Address_Blk_Bldg, @Address_Street, " &
                        " @Address_Subd, @Address_Brgy, @Address_Town_City, @Address_Province, @Address_Region, @Address_ZipCode, @Contact_Person, @Contact_Position, @Contact_Telephone, " &
                        " @Contact_Cellphone, @Contact_Email, @Contact_Fax, @Contact_Website, @Terms, @CutOff, @VAT_Type, @AccountNo, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@VendorCode", txtVendorCode.Text)
        SQL.AddParam("@Vendor_Name", txtVendorName.Text)
        SQL.AddParam("@Classification", ddlClassification.SelectedValue)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@TIN_No", txtTINNO.Text)
        SQL.AddParam("@BranchCode", txtBranchCode.Text)
        SQL.AddParam("@Address_Lot_Unit", txtLot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtBlk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtStreet.Text)
        SQL.AddParam("@Address_Subd", txtSubd.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtZipCode.Text)
        SQL.AddParam("@Contact_Person", txtContactPerson.Text)
        SQL.AddParam("@Contact_Position", txtPosition.text)
        SQL.AddParam("@Contact_Telephone", txtTelephone.Text)
        SQL.AddParam("@Contact_Cellphone", txtCellphone.Text)
        SQL.AddParam("@Contact_Email", txtEmail.Text)
        SQL.AddParam("@Contact_Fax", txtFax.Text)
        SQL.AddParam("@Contact_Website", txtWebsite.Text)
        SQL.AddParam("@Terms", ddlTerms.SelectedValue)
        SQL.AddParam("@CutOff", txtCutOff.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.SelectedValue)
        SQL.AddParam("@AccountNo", txtAccountNo.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblVendor_Master " &
                        " SET Vendor_Name = @Vendor_Name, TIN_No = @TIN_No,BranchCode = @BranchCode,  First_Name = @First_Name ,Last_Name = @Last_Name ,Middle_Name = @Middle_Name, Suffix_Name = @Suffix_Name, Classification = @Classification," &
                        "     Address_Lot_Unit = @Address_Lot_Unit, Address_Blk_Bldg = @Address_Blk_Bldg, Address_Street = @Address_Street," &
                        "     Address_Subd = @Address_Subd, Address_Brgy = @Address_Brgy, Address_Town_City = @Address_Town_City, " &
                        "     Address_Province = @Address_Province, Address_Region = @Address_Region, Address_ZipCode = @Address_ZipCode, " &
                        "     Contact_Person = @Contact_Person, Contact_Position = @Contact_Position, Contact_Telephone = @Contact_Telephone, Contact_Cellphone = @Contact_Cellphone, Contact_Email = @Contact_Email, Contact_Fax = @Contact_Fax," &
                        "     Contact_Website = @Contact_Website, Terms=@Terms, CutOff=@CutOff, VAT_Type=@VAT_Type, AccountNo = @AccountNo, DateModified = @DateModified, WhoModified = @WhoModified" &
                        " WHERE Vendor_Code = @Vendor_Code"
        SQL.FlushParams()
        SQL.AddParam("@Vendor_Code", txtVendorCode.Text)
        SQL.AddParam("@Vendor_Name", txtVendorName.Text)
        SQL.AddParam("@Classification", ddlClassification.SelectedValue)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@TIN_No", txtTINNO.Text)
        SQL.AddParam("@BranchCode", txtBranchCode.Text)
        SQL.AddParam("@Address_Lot_Unit", txtLot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtBlk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtStreet.Text)
        SQL.AddParam("@Address_Subd", txtSubd.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtZipCode.Text)
        SQL.AddParam("@Contact_Person", txtContactPerson.Text)
        SQL.AddParam("@Contact_Position", txtPosition.text)
        SQL.AddParam("@Contact_Telephone", txtTelephone.Text)
        SQL.AddParam("@Contact_Cellphone", txtCellphone.Text)
        SQL.AddParam("@Contact_Email", txtEmail.Text)
        SQL.AddParam("@Contact_Fax", txtFax.Text)
        SQL.AddParam("@Contact_Website", txtWebsite.Text)
        SQL.AddParam("@Terms", ddlTerms.SelectedValue)
        SQL.AddParam("@CutOff", txtCutOff.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.SelectedValue)
        SQL.AddParam("@AccountNo", txtAccountNo.Text)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub Page_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim Code As String = Request.QueryString("ID")
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
                txtVendorCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelVendor.Enabled = Not Value
    End Sub

    Public Sub Initialize()
        txtVendorCode.Attributes.Add("readonly", "readonly")
        txtVendorCode.Text = ""
        txtVendorName.Text = ""
        txtTINNO.Text = ""
        txtBranchCode.Text = ""
        txtCutOff.Text = ""
        txtTelephone.Text = ""
        txtCellphone.Text = ""
        txtContactPerson.Text = ""
        txtEmail.Text = ""
        txtBlk_Bldg.Text = ""
        txtFax.Text = ""
        txtLot_Unit.Text = ""
        txtStreet.Text = ""
        txtSubd.Text = ""
        txtWebsite.Text = ""
        txtZipCode.Text = ""
        txtPosition.Text = ""
        txtAccountNo.Text = ""

        ddlTerms.Items.Clear()
        ddlTerms.Items.Add("--Select Terms--")
        ddlTerms.DataSource = LoadTerms().ToArray
        ddlTerms.DataBind()

        ddlVAT_Type.Items.Clear()
        ddlVAT_Type.Items.Add("--Select VAT Type--")
        ddlVAT_Type.DataSource = LoadtblDefault_VATType().ToArray
        ddlVAT_Type.DataBind()

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

        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtMiddleName.Text = ""
        txtSuffixName.Text = ""

        ddlClassification.Items.Clear()
        ddlClassification.Items.Add("--Select Classification--")
        ddlClassification.Items.Add("Individual")
        ddlClassification.Items.Add("Non-Individual")
        ddlClassification.DataBind()
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

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
    End Sub

    Public Sub View()
        Dim Code As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblVendor_Master " &
                " WHERE Status = @Status AND Vendor_Code = @Vendor_Code"

        SQL.FlushParams()
        SQL.AddParam("@Vendor_Code", Code)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtVendorCode.Text = SQL.SQLDR("Vendor_Code").ToString
            txtVendorName.Text = SQL.SQLDR("Vendor_Name").ToString
            txtFirstName.Text = SQL.SQLDR("First_Name").ToString
            txtLastName.Text = SQL.SQLDR("Last_Name").ToString
            txtMiddleName.Text = SQL.SQLDR("Middle_Name").ToString
            txtSuffixName.Text = SQL.SQLDR("Suffix_Name").ToString
            txtTINNO.Text = SQL.SQLDR("TIN_No").ToString
            txtBranchCode.Text = SQL.SQLDR("BranchCode").ToString
            txtLot_Unit.Text = SQL.SQLDR("Address_Lot_Unit").ToString
            txtBlk_Bldg.Text = SQL.SQLDR("Address_Blk_Bldg").ToString
            txtStreet.Text = SQL.SQLDR("Address_Street").ToString
            txtSubd.Text = SQL.SQLDR("Address_Subd").ToString
            ddlClassification.SelectedValue = SQL.SQLDR("Classification").ToString
            ddlRegion.SelectedValue = SQL.SQLDR("Address_Region").ToString
            Region_Changed()
            ddlProvince.SelectedValue = SQL.SQLDR("Address_Province").ToString
            Province_Changed()
            ddlCityMun.SelectedValue = SQL.SQLDR("Address_Town_City").ToString
            CityMun_Changed()
            ddlBarangay.SelectedValue = SQL.SQLDR("Address_Brgy").ToString
            txtZipCode.Text = SQL.SQLDR("Address_ZipCode").ToString
            txtContactPerson.Text = SQL.SQLDR("Contact_Person").ToString
            txtPosition.text = SQL.SQLDR("Contact_Position").ToString
            txtTelephone.Text = SQL.SQLDR("Contact_Telephone").ToString
            txtEmail.Text = SQL.SQLDR("Contact_Email").ToString
            txtCellphone.Text = SQL.SQLDR("Contact_Cellphone").ToString
            txtFax.Text = SQL.SQLDR("Contact_Fax").ToString
            txtWebsite.Text = SQL.SQLDR("Contact_Website").ToString
            ddlTerms.SelectedValue = SQL.SQLDR("Terms").ToString
            txtCutOff.Text = SQL.SQLDR("CutOff").ToString
            ddlVAT_Type.SelectedValue = SQL.SQLDR("VAT_Type").ToString
            txtAccountNo.Text = SQL.SQLDR("AccountNo").ToString
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='VendorManagement_View.aspx';</script>")
        End If
    End Sub
End Class