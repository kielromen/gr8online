Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Public Class Company_SetUp

    Inherits System.Web.UI.Page
    Public tbl As String = "tblCompany_Information"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()

            End If
        End If
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panCompanySetUp.Enabled = Value
    End Sub

    Public Sub Initialize()
        txtCompany_Name.Text = ""
        txtAddress_Blk_Bldg.Text = ""
        txtAddress_Lot_Unit.Text = ""
        txtAddress_Street.Text = ""
        txtAddress_Subd.Text = ""
        txtAddress_ZipCode.Text = ""
        txtCompany_Contact.Text = ""
        txtCompany_Name.Text = ""
        txtCompany_Email.Text = ""
        txtTIN_No.Text = ""
        txtBranchCode.Text = ""
        txtFirstName.Text = ""
        txtLastName.Text = ""
        txtMiddleName.Text = ""
        txtSuffixName.Text = ""

        ddlClassification.Items.Clear()
        ddlClassification.Items.Add("--Select Classification--")
        ddlClassification.Items.Add("Individual")
        ddlClassification.Items.Add("Non-Individual")
        ddlClassification.DataBind()

        ddlYear.Items.Clear()
        ddlYear.Items.Add("--Select Fiscal/Calendar Year--")
        ddlYear.Items.Add("Fiscal Year")
        ddlYear.Items.Add("Calendar Year")
        ddlYear.DataBind()

        ddlRDO.Items.Clear()
        ddlRDO.Items.Add("--Select RDO--")
        ddlRDO.DataSource = LoadtblDefault_RDO().ToArray
        ddlRDO.DataBind()

        ddlVAT_Type.Items.Clear()
        ddlVAT_Type.Items.Add("--Select VAT Type--")
        ddlVAT_Type.DataSource = LoadtblDefault_VATType().ToArray
        ddlVAT_Type.DataBind()

        ddlIndustry.Items.Clear()
        ddlIndustry.Items.Add("--Select Industry--")
        ddlIndustry.DataSource = LoadtblDefault_Industry().ToArray
        ddlIndustry.DataBind()

        ddlGeneral_Type.Items.Clear()
        ddlGeneral_Type.Items.Add("--Select General Type--")
        ddlGeneral_Type.DataSource = LoadtblDefault_GeneralType().ToArray
        ddlGeneral_Type.DataBind()

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

        alertSave.Visible = False
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

    Public Sub View(ByVal Company_Code As String)
        Dim query As String
        query = " SELECT * FROM [Main].dbo.tblCompany_Information " & vbCrLf &
                " WHERE Company_Code = @Company_Code AND Status = @Status "
        SQL.FlushParams()
        SQL.AddParam("@Company_Code", Company_Code)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtCompany_Name.Text = SQL.SQLDR("Company_Name").ToString
            txtCompany_Contact.Text = SQL.SQLDR("Company_Contact").ToString
            txtCompany_Email.Text = SQL.SQLDR("Company_Email").ToString
            txtAddress_Blk_Bldg.Text = SQL.SQLDR("Address_Blk_Bldg").ToString
            txtAddress_Lot_Unit.Text = SQL.SQLDR("Address_Lot_Unit").ToString
            txtAddress_Street.Text = SQL.SQLDR("Address_Street").ToString
            txtAddress_Subd.Text = SQL.SQLDR("Address_Subd").ToString
            txtAddress_ZipCode.Text = SQL.SQLDR("Address_ZipCode").ToString
            txtTIN_No.Text = SQL.SQLDR("TIN_No").ToString
            ddlRegion.SelectedValue = SQL.SQLDR("Address_Region").ToString
            Region_Changed()
            ddlProvince.SelectedValue = SQL.SQLDR("Address_Province").ToString
            Province_Changed()
            ddlCityMun.SelectedValue = SQL.SQLDR("Address_Town_City").ToString
            CityMun_Changed()
            ddlBarangay.SelectedValue = SQL.SQLDR("Address_Brgy").ToString
            ddlGeneral_Type.SelectedValue = SQL.SQLDR("General_Type").ToString
            ddlIndustry.SelectedValue = SQL.SQLDR("Industry").ToString
            ddlVAT_Type.SelectedValue = SQL.SQLDR("VAT_Type").ToString

            ddlClassification.SelectedValue = SQL.SQLDR("Classification").ToString
            txtLastName.Text = SQL.SQLDR("Last_Name").ToString
            txtFirstName.Text = SQL.SQLDR("First_Name").ToString
            txtMiddleName.Text = SQL.SQLDR("Middle_Name").ToString
            txtSuffixName.Text = SQL.SQLDR("Suffix_Name").ToString
            txtBranchCode.Text = SQL.SQLDR("BranchCode").ToString
            ddlRDO.SelectedValue = SQL.SQLDR("RDO").ToString

            ddlYear.SelectedValue = SQL.SQLDR("ReportCycle").ToString
            dtpFromDate.Text = CDate(SQL.SQLDR("DateFrom")).ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(SQL.SQLDR("DateTo")).ToString("yyyy-MM-dd")
        End If

        query = " SELECT * FROM [Main].dbo.tblCompany_Information " & vbCrLf &
                 " WHERE Company_Code = @Company_Code AND Status = @Status "
        SQL.FlushParams()
        SQL.AddParam("@Company_Code", Company_Code)
        SQL.AddParam("@Status", "Active")
        SQL.GetDataTable(query)
        gvImages.DataSource = SQL.SQLDT
        gvImages.DataBind()
    End Sub

    Public Sub Save()
        Dim ID As Integer = GenerateID(tbl, "Main")
        Dim bytes As Byte()
        Using br As BinaryReader = New BinaryReader(fuCompanyLogo.PostedFile.InputStream)
            bytes = br.ReadBytes(fuCompanyLogo.PostedFile.ContentLength)
        End Using
        Dim query As String
        query = " INSERT INTO [Main].dbo.tblCompany_Information " &
                " (Company_Code, Company_Name, Company_Logo, Company_Contact, Company_Email, Default_EmailAddress, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy, Address_Town_City, Address_Province, 
                         Address_Region , Address_ZipCode, VAT_Type, TIN_No, General_Type, Industry, Classification, BranchCode, RDO, First_Name, Last_Name, Middle_Name, Suffix_Name,ReportCycle, DateFrom, DateTo, Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@Company_Code, @Company_Name, @Company_Logo, @Company_Contact, @Company_Email, @Default_EmailAddress, @Address_Lot_Unit, @Address_Blk_Bldg, @Address_Street, @Address_Subd, @Address_Brgy, @Address_Town_City, @Address_Province, 
                         @Address_Region, @Address_ZipCode, @VAT_Type, @TIN_No, @General_Type, @Industry, @Classification, @BranchCode, @RDO, @First_Name, @Last_Name, @Middle_Name, @Suffix_Name, @ReportCycle, @DateFrom, @DateTo, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Company_Code", ID)
        SQL.AddParam("@Company_Name", txtCompany_Name.Text)
        SQL.AddParam("@Company_Logo", bytes, SqlDbType.VarBinary)
        SQL.AddParam("@Company_Contact", txtCompany_Contact.Text)
        SQL.AddParam("@Company_Email", txtCompany_Email.Text)
        SQL.AddParam("@Default_EmailAddress", Session("EmailAddress"))
        SQL.AddParam("@Address_Lot_Unit", txtAddress_Lot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtAddress_Blk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtAddress_Street.Text)
        SQL.AddParam("@Address_Subd", txtAddress_Subd.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtAddress_ZipCode.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.Text)
        SQL.AddParam("@TIN_No", txtTIN_No.Text)
        SQL.AddParam("@General_Type", ddlGeneral_Type.Text)
        SQL.AddParam("@Industry", ddlIndustry.Text)
        SQL.AddParam("@Classification", ddlClassification.Text)
        SQL.AddParam("@BranchCode", txtBranchCode.Text)
        SQL.AddParam("@RDO", ddlRDO.SelectedValue)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@ReportCycle", ddlYear.SelectedValue)
        SQL.AddParam("@DateFrom", dtpFromDate.Text)
        SQL.AddParam("@DateTo", dtpToDate.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update(ByVal Company_Code As String)
        Dim query As String
        query = " UPDATE [Main].dbo.tblCompany_Information SET " &
                " Company_Name = @Company_Name, Company_Contact = @Company_Contact, " &
                " Company_Email = @Company_Email, Default_EmailAddress = @Default_EmailAddress, Address_Lot_Unit = @Address_Lot_Unit, " &
                " Address_Blk_Bldg = @Address_Blk_Bldg, Address_Street = @Address_Street, Address_Subd = @Address_Subd, " & vbCrLf &
                " Address_Brgy =@Address_Brgy, Address_Town_City = @Address_Town_City, Address_Province = @Address_Province, " & vbCrLf &
                " Address_Region = @Address_Region, Address_ZipCode = @Address_ZipCode, VAT_Type = @VAT_Type, TIN_No = @TIN_No, " & vbCrLf &
                " General_Type = @General_Type, Industry = @Industry, Classification = @Classification, BranchCode = @BranchCode, RDO = @RDO, " & vbCrLf &
                " First_Name = @First_Name, Last_Name = @Last_Name, Middle_Name = @Middle_Name, Suffix_Name = @Suffix_Name, " & vbCrLf &
                " ReportCycle = @ReportCycle, DateFrom = @DateFrom, DateTo = @DateTo, DateModified = @DateModified, WhoModified = @WhoModified" & vbCrLf &
                " WHERE Company_Code = @Company_Code And Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Company_Code", Company_Code)
        SQL.AddParam("@Company_Name", txtCompany_Name.Text)
        SQL.AddParam("@Company_Contact", txtCompany_Contact.Text)
        SQL.AddParam("@Company_Email", txtCompany_Email.Text)
        SQL.AddParam("@Default_EmailAddress", Session("EmailAddress"))
        SQL.AddParam("@Address_Lot_Unit", txtAddress_Lot_Unit.Text)
        SQL.AddParam("@Address_Blk_Bldg", txtAddress_Blk_Bldg.Text)
        SQL.AddParam("@Address_Street", txtAddress_Street.Text)
        SQL.AddParam("@Address_Subd", txtAddress_Subd.Text)
        SQL.AddParam("@Address_Brgy", ddlBarangay.SelectedValue)
        SQL.AddParam("@Address_Town_City", ddlCityMun.SelectedValue)
        SQL.AddParam("@Address_Province", ddlProvince.SelectedValue)
        SQL.AddParam("@Address_Region", ddlRegion.SelectedValue)
        SQL.AddParam("@Address_ZipCode", txtAddress_ZipCode.Text)
        SQL.AddParam("@VAT_Type", ddlVAT_Type.Text)
        SQL.AddParam("@TIN_No", txtTIN_No.Text)
        SQL.AddParam("@General_Type", ddlGeneral_Type.Text)
        SQL.AddParam("@Industry", ddlIndustry.Text)
        SQL.AddParam("@Classification", ddlClassification.Text)
        SQL.AddParam("@BranchCode", txtBranchCode.Text)
        SQL.AddParam("@RDO", ddlRDO.SelectedValue)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@ReportCycle", ddlYear.SelectedValue)
        SQL.AddParam("@DateFrom", dtpFromDate.Text)
        SQL.AddParam("@DateTo", dtpToDate.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)

        If fuCompanyLogo.HasFile = True Then
            Dim bytes As Byte()
            Using br As BinaryReader = New BinaryReader(fuCompanyLogo.PostedFile.InputStream)
                bytes = br.ReadBytes(fuCompanyLogo.PostedFile.ContentLength)
            End Using
            query = " UPDATE [Main].dbo.tblCompany_Information SET Company_Logo = @Company_Logo " &
                    " WHERE Company_Code = @Company_Code And Status = @Status"
            SQL.FlushParams()
            SQL.AddParam("@Company_Code", Company_Code)
            SQL.AddParam("@Company_Logo", bytes, SqlDbType.VarBinary)
            SQL.AddParam("@Status", "Active")
            SQL.ExecNonQuery(query)
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



    Private Sub Company_SetUp_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            View(Session("Company_Code"))
            If Session("Company_Code").ToString <> "" Then
                btnSave.Text = "Edit"
                EnableControl(False)
            Else
                btnSave.Text = "Save"
                EnableControl(True)
            End If
        End If
    End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim dr As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim imageUrl As String = "data:image/jpg;base64," & Convert.ToBase64String(CType(dr("Company_Logo"), Byte()))
                CType(e.Row.FindControl("Company_Logo"), Image).ImageUrl = imageUrl
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If btnSave.Text = "Edit" Then
            btnSave.Text = "Update"
            EnableControl(True)
            Exit Sub
        End If
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
            ElseIf btnSave.Text = "Update" Then
                Update(Session("Company_Code"))
                btnSave.Text = "Edit"
            End If
            EnableControl(False)
            Initialize()
            View(Session("Company_Code"))
            alertSave.Visible = True
        End If
    End Sub

    Private Sub dtpFromDate_TextChanged(sender As Object, e As EventArgs) Handles dtpFromDate.TextChanged
        LoadPeriod()
    End Sub

    Public Sub LoadPeriod()

        If ddlYear.SelectedValue = "Calendar Year" Then
            dtpFromDate.Text = CDate(Now.Year & "-01-01").ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(Now.Year & "-12-31").ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Add("readonly", "true")
            dtpToDate.Attributes.Add("readonly", "true")
        ElseIf ddlYear.SelectedValue = "Fiscal Year" Then
            Dim enddate As Date = DateAdd(DateInterval.Year, 1, CDate(dtpFromDate.Text)).ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(enddate.AddDays(-1)).ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Remove("readonly")
            dtpToDate.Attributes.Add("readonly", "true")
        Else
            dtpFromDate.Text = CDate(Now.Year & "-01-01").ToString("yyyy-MM-dd")
            dtpToDate.Text = CDate(Now.Year & "-12-31").ToString("yyyy-MM-dd")
            dtpFromDate.Attributes.Add("readonly", "true")
            dtpToDate.Attributes.Add("readonly", "true")
        End If
    End Sub
End Class