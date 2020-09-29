Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class CustomerManagement
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ModuleID As String = "Customer"
    Dim ColumnPK As String = "Customer_Code"
    Dim DBTable As String = "tblCustomer_Master"
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

    Public Sub Initialize()
        txtCustomerCode.Attributes.Add("readonly", "readonly")
        txtCustomerCode.Text = ""
        txtCustomerName.Text = ""
        txtTINNO.Text = ""
        txtCutOff.Text = ""
        txtTelephone.Text = ""
        txtCellphone.Text = ""
        txtContactPerson.Text = ""
        txtEmail.Text = ""
        txtTerms.Text = ""
        txtWebsite.Text = ""
        txtFax.Text = ""
        txtPosition.Text = ""
        ddlVAT_Type.Items.Clear()
        ddlVAT_Type.Items.Add("--Select VAT Type--")
        ddlVAT_Type.DataSource = LoadtblDefault_VATType().ToArray
        ddlVAT_Type.DataBind()
        chkAddress.Checked = False
        'Billing Address
        txtBlk_Bldg.Text = ""
        txtLot_Unit.Text = ""
        txtStreet.Text = ""
        txtSubd.Text = ""
        txtZipCode.Text = ""
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
        'Delivery Address
        txtDelivery_BlkBldg.Text = ""
        txtDelivery_LotUnit.Text = ""
        txtDelivery_Street.Text = ""
        txtDelivery_Subdivision.Text = ""
        txtDelivery_ZipCode.Text = ""
        ddlDelivery_Province.Items.Clear()
        ddlDelivery_Province.Items.Add("--Select Province--")
        ddlDelivery_City.Items.Clear()
        ddlDelivery_City.Items.Add("--Select City/Municipality--")
        ddlDelivery_Brgy.Items.Clear()
        ddlDelivery_Brgy.Items.Add("--Select Barangay--")
        ddlDelivery_Region.Items.Clear()
        ddlDelivery_Region.Items.Add("--Select Region--")
        ddlDelivery_Region.DataSource = LoadtblAddress_Region().ToArray
        ddlDelivery_Region.DataBind()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='CustomerManagement_View.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    Private Sub CustomerManagement_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
                txtCustomerCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelCustomer.Enabled = Not Value
    End Sub

    Public Sub View()
        Dim Code As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblCustomer_Master " &
                " WHERE Status = @Status AND Customer_Code = @Customer_Code"
        SQL.FlushParams()
        SQL.AddParam("@Customer_Code", Code)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtCustomerCode.Text = SQL.SQLDR("Customer_Code").ToString
            txtCustomerName.Text = SQL.SQLDR("Customer_name").ToString
            txtTINNO.Text = SQL.SQLDR("TIN_No").ToString
            txtTerms.Text = SQL.SQLDR("Terms").ToString
            txtCutOff.Text = SQL.SQLDR("CutOff").ToString
            ddlVAT_Type.SelectedValue = SQL.SQLDR("VAT_Type").ToString
            'Billing Address
            txtLot_Unit.Text = SQL.SQLDR("Billing_Lot_Unit").ToString
            txtBlk_Bldg.Text = SQL.SQLDR("Billing_Blk_Bldg").ToString
            txtStreet.Text = SQL.SQLDR("Billing_Street").ToString
            txtSubd.Text = SQL.SQLDR("Billing_Subd").ToString
            ddlRegion.SelectedValue = SQL.SQLDR("Billing_Region").ToString
            Region_Changed()
            ddlProvince.SelectedValue = SQL.SQLDR("Billing_Province").ToString
            Province_Changed()
            ddlCityMun.SelectedValue = SQL.SQLDR("Billing_Town_City").ToString
            CityMun_Changed()
            ddlBarangay.SelectedValue = SQL.SQLDR("Billing_Brgy").ToString
            txtZipCode.Text = SQL.SQLDR("Billing_ZipCode").ToString
            chkAddress.Checked = SQL.SQLDR("SameAddress").ToString
            'Delivery Address
            txtDelivery_LotUnit.Text = SQL.SQLDR("Delivery_Lot_Unit").ToString
            txtDelivery_BlkBldg.Text = SQL.SQLDR("Delivery_Blk_Bldg").ToString
            txtDelivery_Street.Text = SQL.SQLDR("Delivery_Street").ToString
            txtDelivery_Subdivision.Text = SQL.SQLDR("Delivery_Subd").ToString
            ddlDelivery_Region.SelectedValue = SQL.SQLDR("Delivery_Region").ToString
            DeliveryRegion_Changed()
            ddlDelivery_Province.SelectedValue = SQL.SQLDR("Delivery_Province").ToString
            DeliveryProvince_Changed()
            ddlDelivery_City.SelectedValue = SQL.SQLDR("Delivery_Town_City").ToString
            DeliveryCityMun_Changed()
            ddlDelivery_Brgy.SelectedValue = SQL.SQLDR("Delivery_Brgy").ToString
            txtDelivery_ZipCode.Text = SQL.SQLDR("Delivery_ZipCode").ToString
            'Contact Person
            txtContactPerson.Text = SQL.SQLDR("Contact_Person").ToString
            txtPosition.Text = SQL.SQLDR("Contact_Position").ToString
            txtTelephone.Text = SQL.SQLDR("Contact_Telephone").ToString
            txtEmail.Text = SQL.SQLDR("Contact_Email").ToString
            txtCellphone.Text = SQL.SQLDR("Contact_Cellphone").ToString
            txtFax.Text = SQL.SQLDR("Contact_Fax").ToString
            txtWebsite.Text = SQL.SQLDR("Contact_Website").ToString
        End If
    End Sub

    Public Sub Region_Changed()
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlProvince.DataSource = LoadtblAddress_Province(ddlRegion.SelectedValue).ToArray
        ddlProvince.DataBind()
    End Sub

    Public Sub DeliveryRegion_Changed()
        ddlDelivery_Province.Items.Clear()
        ddlDelivery_Province.Items.Add("--Select Province--")
        ddlDelivery_Province.DataSource = LoadtblAddress_Province(ddlDelivery_Region.SelectedValue).ToArray
        ddlDelivery_Province.DataBind()
    End Sub

    Public Sub Province_Changed()
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlCityMun.DataSource = LoadtblAddress_CityMunicipality(ddlProvince.SelectedValue).ToArray
        ddlCityMun.DataBind()
    End Sub

    Public Sub DeliveryProvince_Changed()
        ddlDelivery_City.Items.Clear()
        ddlDelivery_City.Items.Add("--Select City/Municipality--")
        ddlDelivery_City.DataSource = LoadtblAddress_CityMunicipality(ddlDelivery_Province.SelectedValue).ToArray
        ddlDelivery_City.DataBind()
    End Sub



    Public Sub CityMun_Changed()
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
        ddlBarangay.DataSource = LoadtblAddress_Brgy(ddlCityMun.SelectedValue).ToArray
        ddlBarangay.DataBind()
    End Sub


    Public Sub DeliveryCityMun_Changed()
        ddlDelivery_Brgy.Items.Clear()
        ddlDelivery_Brgy.Items.Add("--Select Barangay--")
        ddlDelivery_Brgy.DataSource = LoadtblAddress_Brgy(ddlDelivery_City.SelectedValue).ToArray
        ddlDelivery_Brgy.DataBind()
    End Sub

    Private Sub ddlRegion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlRegion.SelectedIndexChanged
        ddlProvince.Items.Clear()
        ddlProvince.Items.Add("--Select Province--")
        ddlCityMun.Items.Clear()
        ddlCityMun.Items.Add("--Select City/Municipality--")
        ddlBarangay.Items.Clear()
        ddlBarangay.Items.Add("--Select Barangay--")
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='CustomerManagement_View.aspx';</script>")
        End If
    End Sub

    Public Sub Save()
        txtCustomerCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        query = " INSERT INTO tblCustomer_Master " &
                        " (Customer_Code, Customer_Name, TIN_No, " &
                        " Billing_Lot_Unit, Billing_Blk_Bldg, Billing_Street, Billing_Subd, Billing_Brgy, Billing_Town_City, Billing_Province, Billing_Region, Billing_ZipCode, " &
                        " Delivery_Lot_Unit, Delivery_Blk_Bldg, Delivery_Street, Delivery_Subd, Delivery_Brgy, Delivery_Town_City, Delivery_Province, Delivery_Region, Delivery_ZipCode, " &
                        " SameAddress, Contact_Person, Contact_Position, Contact_Telephone," &
                        " Contact_Cellphone , Contact_Email , Contact_Fax, Contact_Website, Terms, CutOff, VAT_Type, Status, DateCreated, WhoCreated)" &
                        " VALUES " &
                        " (@Customer_Code, @Customer_Name, @TIN_No, " &
                        " @Billing_Lot_Unit,@Billing_Blk_Bldg, @Billing_Street, @Billing_Subd, @Billing_Brgy, @Billing_Town_City, @Billing_Province, @Billing_Region, @Billing_ZipCode, " &
                        " @Delivery_Lot_Unit,@Delivery_Blk_Bldg, @Delivery_Street, @Delivery_Subd, @Delivery_Brgy, @Delivery_Town_City, @Delivery_Province, @Delivery_Region, @Delivery_ZipCode, " &
                        " @SameAddress, @Contact_Person, @Contact_Position, @Contact_Telephone, " &
                        " @Contact_Cellphone, @Contact_Email, @Contact_Fax, @Contact_Website, @Terms, @CutOff, @VAT_Type, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Customer_Code", txtCustomerCode.Text)
        SQL.AddParam("@Customer_Name", txtCustomerName.Text)
        SQL.AddParam("@TIN_No", txtTINNO.Text)
        SQL.AddParam("@Billing_Lot_Unit", txtLot_Unit.Text)
        SQL.AddParam("@Billing_Blk_Bldg", txtBlk_Bldg.Text)
        SQL.AddParam("@Billing_Street", txtStreet.Text)
        SQL.AddParam("@Billing_Subd", txtSubd.Text)
        SQL.AddParam("@Billing_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Billing_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Billing_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Billing_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Billing_ZipCode", txtZipCode.Text)
        SQL.AddParam("@Delivery_Lot_Unit", txtDelivery_LotUnit.Text)
        SQL.AddParam("@Delivery_Blk_Bldg", txtDelivery_BlkBldg.Text)
        SQL.AddParam("@Delivery_Street", txtDelivery_Street.Text)
        SQL.AddParam("@Delivery_Subd", txtDelivery_Subdivision.Text)
        SQL.AddParam("@Delivery_Brgy", ddlDelivery_Brgy.SelectedValue)
        SQL.AddParam("@Delivery_Town_City", ddlDelivery_City.SelectedValue)
        SQL.AddParam("@Delivery_Province", ddlDelivery_Province.SelectedValue)
        SQL.AddParam("@Delivery_Region", ddlDelivery_Region.SelectedValue)
        SQL.AddParam("@Delivery_ZipCode", txtDelivery_ZipCode.Text)
        SQL.AddParam("@SameAddress", chkAddress.Checked)
        SQL.AddParam("@Contact_Person", txtContactPerson.Text)
        SQL.AddParam("@Contact_Position", txtPosition.Text)
        SQL.AddParam("@Contact_Telephone", txtTelephone.Text)
        SQL.AddParam("@Contact_Cellphone", txtCellphone.Text)
        SQL.AddParam("@Contact_Email", txtEmail.Text)
        SQL.AddParam("@Contact_Fax", txtFax.Text)
        SQL.AddParam("@Contact_Website", txtWebsite.Text)
        SQL.AddParam("@Terms", txtTerms.Text)
        SQL.AddParam("@CutOff", txtCutOff.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.SelectedValue)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ID As String = Request.QueryString("ID")
        Dim query As String
        query = " UPDATE tblCustomer_Master " &
                        " SET Customer_Name = @Customer_Name, TIN_No = @TIN_No, " &
                        "     Billing_Lot_Unit = @Billing_Lot_Unit, Billing_Blk_Bldg = @Billing_Blk_Bldg, Billing_Street = @Billing_Street, Billing_Subd = @Billing_Subd, Billing_Brgy = @Billing_Brgy, Billing_Town_City = @Billing_Town_City, Billing_Province = @Billing_Province, Billing_Region = @Billing_Region, Billing_ZipCode = @Billing_ZipCode," &
                        "     Delivery_Lot_Unit = @Delivery_Lot_Unit, Delivery_Blk_Bldg = @Delivery_Blk_Bldg, Delivery_Street = @Delivery_Street, Delivery_Subd = @Delivery_Subd, Delivery_Brgy = @Delivery_Brgy, Delivery_Town_City = @Delivery_Town_City, Delivery_Province = @Delivery_Province, Delivery_Region = @Delivery_Region, Delivery_ZipCode = @Delivery_ZipCode," &
                        "     SameAddress = @SameAddress, Contact_Person = @Contact_Person, Contact_Position= @Contact_Position, Contact_Telephone = @Contact_Telephone, Contact_Cellphone = @Contact_Cellphone, Contact_Email = @Contact_Email, Contact_Fax = @Contact_Fax," &
                        "     Contact_Website = @Contact_Website, Terms=@Terms, CutOff=@CutOff, VAT_Type=@VAT_Type, DateModified = @DateModified, WhoModified = @WhoModified" &
                        " WHERE Customer_Code = @Customer_Code"
        SQL.FlushParams()
        SQL.AddParam("@Customer_Code", txtCustomerCode.Text)
        SQL.AddParam("@Customer_Name", txtCustomerName.Text)
        SQL.AddParam("@TIN_No", txtTINNO.Text)
        SQL.AddParam("@Billing_Lot_Unit", txtLot_Unit.Text)
        SQL.AddParam("@Billing_Blk_Bldg", txtBlk_Bldg.Text)
        SQL.AddParam("@Billing_Street", txtStreet.Text)
        SQL.AddParam("@Billing_Subd", txtSubd.Text)
        SQL.AddParam("@Billing_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Billing_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Billing_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Billing_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Billing_ZipCode", txtZipCode.Text)
        SQL.AddParam("@Delivery_Lot_Unit", txtDelivery_LotUnit.Text)
        SQL.AddParam("@Delivery_Blk_Bldg", txtDelivery_BlkBldg.Text)
        SQL.AddParam("@Delivery_Street", txtDelivery_Street.Text)
        SQL.AddParam("@Delivery_Subd", txtDelivery_Subdivision.Text)
        SQL.AddParam("@Delivery_Brgy", ddlDelivery_Brgy.SelectedValue)
        SQL.AddParam("@Delivery_Town_City", ddlDelivery_City.SelectedValue)
        SQL.AddParam("@Delivery_Province", ddlDelivery_Province.SelectedValue)
        SQL.AddParam("@Delivery_Region", ddlDelivery_Region.SelectedValue)
        SQL.AddParam("@Delivery_ZipCode", txtDelivery_ZipCode.Text)
        SQL.AddParam("@SameAddress", chkAddress.Checked)
        SQL.AddParam("@Contact_Person", txtContactPerson.Text)
        SQL.AddParam("@Contact_Position", txtPosition.Text)
        SQL.AddParam("@Contact_Telephone", txtTelephone.Text)
        SQL.AddParam("@Contact_Cellphone", txtCellphone.Text)
        SQL.AddParam("@Contact_Email", txtEmail.Text)
        SQL.AddParam("@Contact_Fax", txtFax.Text)
        SQL.AddParam("@Contact_Website", txtWebsite.Text)
        SQL.AddParam("@Terms", txtTerms.Text)
        SQL.AddParam("@CutOff", txtCutOff.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.SelectedValue)
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Private Sub ddlDelivery_Region_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDelivery_Region.SelectedIndexChanged
        ddlDelivery_Province.Items.Clear()
        ddlDelivery_Province.Items.Add("--Select Province--")
        ddlDelivery_City.Items.Clear()
        ddlDelivery_City.Items.Add("--Select City/Municipality--")
        ddlDelivery_Brgy.Items.Clear()
        ddlDelivery_Brgy.Items.Add("--Select Barangay--")
    End Sub

    Private Sub chkAddress_CheckedChanged(sender As Object, e As EventArgs) Handles chkAddress.CheckedChanged
        If chkAddress.Checked = True Then
            txtDelivery_BlkBldg.Text = txtBlk_Bldg.Text
            txtDelivery_LotUnit.Text = txtLot_Unit.Text
            txtDelivery_Street.Text = txtStreet.Text
            txtDelivery_Subdivision.Text = txtSubd.Text
            txtDelivery_ZipCode.Text = txtZipCode.Text
            ddlDelivery_Region.SelectedValue = ddlRegion.SelectedValue
            DeliveryRegion_Changed()
            ddlDelivery_Province.SelectedValue = ddlProvince.SelectedValue
            DeliveryProvince_Changed()
            ddlDelivery_City.SelectedValue = ddlCityMun.SelectedValue
            DeliveryCityMun_Changed()
            ddlDelivery_Brgy.SelectedValue = ddlBarangay.SelectedValue
        Else
            txtDelivery_BlkBldg.Text = ""
            txtDelivery_LotUnit.Text = ""
            txtDelivery_Street.Text = ""
            txtDelivery_Subdivision.Text = ""
            txtDelivery_ZipCode.Text = ""
            ddlDelivery_Province.Items.Clear()
            ddlDelivery_Province.Items.Add("--Select Province--")
            ddlDelivery_City.Items.Clear()
            ddlDelivery_City.Items.Add("--Select City/Municipality--")
            ddlDelivery_Brgy.Items.Clear()
            ddlDelivery_Brgy.Items.Add("--Select Barangay--")
            ddlDelivery_Region.Items.Clear()
            ddlDelivery_Region.Items.Add("--Select Region--")
            ddlDelivery_Region.DataSource = LoadtblAddress_Region().ToArray
            ddlDelivery_Region.DataBind()
        End If
    End Sub
End Class