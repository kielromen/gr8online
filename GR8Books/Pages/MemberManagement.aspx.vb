Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Data.SqlClient
Public Class MemberManagement
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ModuleID As String = "Member"
    Dim ColumnPK As String = "Member_Code"
    Dim DBTable As String = "tblMember_Master"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
            End If
        End If
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelMember.Enabled = Not Value
        fuSignature.Enabled = Not Value
        fuPhoto.Enabled = Not Value
        dtpBirthdate.Disabled = Value
        dtpMembershipDate.Disabled = Value
    End Sub

    Public Sub Initialize()
        txtMemberCode.Text = ""
        txtMemberName.Text = ""
        txtMemberCode.Attributes.Add("readonly", "readonly")
        txtMemberName.Attributes.Add("readonly", "readonly")
        txtSuffixName.Text = ""
        txtFirstName.Text = ""
        txtMiddleName.Text = ""
        txtLastName.Text = ""
        txtNickName.Text = ""
        txtBirthplace.Text = ""
        txtEdu.Text = ""
        txtReligion.Text = ""
        txtEmail.Text = ""
        txtCellphoneNo.Text = ""
        txtBRN.Text = ""
        txtAddress_Lot_Unit.Text = ""
        txtAddress_Blk_Bldg.Text = ""
        txtStreet.Text = ""
        txtSubdivision.Text = ""
        txtZipCode.Text = ""
        'Title
        ddlTitle.Items.Clear()
        ddlTitle.Items.Add("--Select Title--")
        ddlTitle.Items.Add("Mr.")
        ddlTitle.Items.Add("MS.")
        ddlTitle.Items.Add("Miss")
        ddlTitle.Items.Add("Mrs.")
        ddlTitle.Items.Add("Dr.")
        ddlTitle.Items.Add("Prof.")
        ddlTitle.Items.Add("Hon.")
        ddlTitle.Items.Add("Lady")
        ddlTitle.Items.Add("Major")
        ddlTitle.Items.Add("Sir")
        ddlTitle.Items.Add("Madam")
        'Select Gender
        ddlGender.Items.Clear()
        ddlGender.Items.Add("--Select Gender--")
        ddlGender.Items.Add("Male")
        ddlGender.Items.Add("Female")
        '-Select Nationality
        ddlNationality.Items.Clear()
        ddlNationality.Items.Add("--Select Nationality--")
        ddlNationality.Items.Add("Filipino")
        'Select Civil Status
        ddlCivilStatus.Items.Clear()
        ddlCivilStatus.Items.Add("--Select Civil Status--")
        ddlCivilStatus.Items.Add("Single")
        ddlCivilStatus.Items.Add("Married")
        ddlCivilStatus.Items.Add("Widowed")
        ddlCivilStatus.Items.Add("Divorced")
        ddlCivilStatus.Items.Add("Separated")
        'Select Member Type
        ddlMemberType.Items.Clear()
        ddlMemberType.Items.Add("--Select Member Type--")
        ddlMemberType.Items.Add("Regular")
        ddlMemberType.Items.Add("Associate")
        'Address
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

    Private Sub MemberManagement_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ID As String = Request.QueryString("Member_Code")
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
                txtMemberCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Public Sub View()
        Dim Member_Code As String = Request.QueryString("ID")
        Dim query As String
        query = " SELECT * FROM tblMember_Master " &
                " WHERE tblMember_Master.Status = @Status AND Member_Code = @Member_Code"
        SQL.FlushParams()
        SQL.AddParam("@Member_Code", Member_Code)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtMemberCode.Text = SQL.SQLDR("Member_Code").ToString
            txtMemberName.Text = SQL.SQLDR("Member_Name").ToString
            ddlTitle.SelectedValue = SQL.SQLDR("Title").ToString
            txtFirstName.Text = SQL.SQLDR("First_Name").ToString
            txtLastName.Text = SQL.SQLDR("Last_Name").ToString
            txtMiddleName.Text = SQL.SQLDR("Middle_Name").ToString
            txtSuffixName.Text = SQL.SQLDR("Suffix_Name").ToString
            txtNickName.Text = SQL.SQLDR("Nick_Name").ToString
            ddlGender.SelectedValue = SQL.SQLDR("Gender").ToString
            dtpBirthdate.Value = CDate(SQL.SQLDR("BIrth_Date")).ToString("yyyy-MM-dd")
            txtBirthplace.Text = SQL.SQLDR("BIrth_Place").ToString
            ddlNationality.SelectedValue = SQL.SQLDR("Nationality").ToString
            ddlCivilStatus.SelectedValue = SQL.SQLDR("Civil_Status").ToString
            txtEdu.Text = SQL.SQLDR("Educational_Attainment").ToString
            txtReligion.Text = SQL.SQLDR("Religion").ToString
            txtBRN.Text = SQL.SQLDR("BRN").ToString
            ddlMemberType.SelectedValue = SQL.SQLDR("Member_Type").ToString
            dtpMembershipDate.Value = CDate(SQL.SQLDR("Membership_Date")).ToString("yyyy-MM-dd")
            txtAddress_Blk_Bldg.Text = SQL.SQLDR("Address_Blk_Bldg").ToString
            txtAddress_Lot_Unit.Text = SQL.SQLDR("Address_Lot_Unit").ToString
            txtStreet.Text = SQL.SQLDR("Address_Street").ToString
            txtSubdivision.Text = SQL.SQLDR("Address_Subd").ToString
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
            Response.Write("<script>window.location='MemberManagement_Loadlist.aspx';</script>")
        End If
    End Sub

    Public Sub Save()
        Dim bytesPho As Byte()
        Using br As BinaryReader = New BinaryReader(fuPhoto.PostedFile.InputStream)
            bytesPho = br.ReadBytes(fuPhoto.PostedFile.ContentLength)
        End Using

        Dim bytesSig As Byte()
        Using br As BinaryReader = New BinaryReader(fuSignature.PostedFile.InputStream)
            bytesSig = br.ReadBytes(fuSignature.PostedFile.ContentLength)
        End Using

        txtMemberCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        query = " INSERT INTO tblMember_Master " &
                " (Member_Code, Member_Name, Title, First_Name, Last_Name, Middle_Name, Suffix_Name, Nick_Name, Gender, BIrth_Date, BIrth_Place, Nationality, Civil_Status, Educational_Attainment, Religion, Photo, Signature, BRN, 
                         Member_Type, Membership_Date, Address_Lot_Unit, Address_Blk_Bldg, Address_Street, Address_Subd, Address_Brgy, Address_Town_City, Address_Province, Address_Region, Address_ZipCode, EmailAddress, 
                         CellphoneNo, Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@Member_Code, @Member_Name, @Title, @First_Name, @Last_Name, @Middle_Name, @Suffix_Name, @Nick_Name, @Gender, @BIrth_Date, @BIrth_Place, @Nationality, @Civil_Status, @Educational_Attainment, @Religion, @Photo, @Signature, @BRN, 
                         @Member_Type, @Membership_Date, @Address_Lot_Unit, @Address_Blk_Bldg, @Address_Street, @Address_Subd, @Address_Brgy, @Address_Town_City, @Address_Province, @Address_Region, @Address_ZipCode, @EmailAddress, 
                         @CellphoneNo, @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@Member_Code", txtMemberCode.Text)
        SQL.AddParam("@Member_Name", txtLastName.Text + ", " + txtFirstName.Text + IIf(txtMiddleName.Text = "", "", " " + txtMiddleName.Text) + IIf(txtSuffixName.Text = "", "", " " + txtSuffixName.Text))
        SQL.AddParam("@Title", ddlTitle.SelectedValue)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Nick_Name", txtNickName.Text)
        SQL.AddParam("@Gender", ddlGender.SelectedValue)
        SQL.AddParam("@BIrth_Date", dtpBirthdate.Value)
        SQL.AddParam("@BIrth_Place", txtBirthplace.Text)
        SQL.AddParam("@Nationality", ddlNationality.SelectedValue)
        SQL.AddParam("@Civil_Status", ddlCivilStatus.SelectedValue)
        SQL.AddParam("@Educational_Attainment", txtEdu.Text)
        SQL.AddParam("@Religion", txtReligion.Text)
        SQL.AddParam("@Photo", bytesPho, SqlDbType.VarBinary)
        SQL.AddParam("@Signature", bytesSig, SqlDbType.VarBinary)
        SQL.AddParam("@BRN", txtBRN.Text)
        SQL.AddParam("@Member_Type", ddlMemberType.SelectedValue)
        SQL.AddParam("@Membership_Date", dtpMembershipDate.Value)
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
        Dim query As String
        query = " UPDATE tblMember_Master " &
                " SET Title = @Title, Suffix_Name = @Suffix_Name, First_Name = @First_Name, Middle_Name = @Middle_Name, " & vbCrLf &
                "     Last_Name = @Last_Name, Nick_Name = @Nick_Name, Member_Name = @Member_Name, Gender = @Gender, Birth_Date = @Birth_Date, Birth_Place = @Birth_Place, " & vbCrLf &
                "     Nationality = @Nationality, Civil_Status = @Civil_Status, Educational_Attainment = @Educational_Attainment, Religion = @Religion, BRN = @BRN, Member_Type = @Member_Type, " &
                "     Membership_Date = @Membership_Date, Address_Lot_Unit = @Address_Lot_Unit, Address_Blk_Bldg = @Address_Blk_Bldg, Address_Street = @Address_Street, " & vbCrLf &
                "     Address_Subd = @Address_Subd, Address_Brgy = @Address_Brgy, Address_Town_City = @Address_Town_City, " &
                "     Address_Province = @Address_Province, Address_Region = @Address_Region, Address_ZipCode = @Address_ZipCode, " & vbCrLf &
                "     CellphoneNo = @CellphoneNo, EmailAddress = @EmailAddress, DateModified = @DateModified, WhoModified = @WhoModified" &
                " WHERE Member_Code = @Member_Code"
        SQL.FlushParams()
        SQL.AddParam("@Member_Code", txtMemberCode.Text)
        SQL.AddParam("@Member_Name", txtLastName.Text + ", " + txtFirstName.Text + IIf(txtMiddleName.Text = "", "", " " + txtMiddleName.Text) + IIf(txtSuffixName.Text = "", "", " " + txtSuffixName.Text))
        SQL.AddParam("@Title", ddlTitle.SelectedValue)
        SQL.AddParam("@Suffix_Name", txtSuffixName.Text)
        SQL.AddParam("@First_Name", txtFirstName.Text)
        SQL.AddParam("@Middle_Name", txtMiddleName.Text)
        SQL.AddParam("@Last_Name", txtLastName.Text)
        SQL.AddParam("@Nick_Name", txtNickName.Text)
        SQL.AddParam("@Gender", ddlGender.SelectedValue)
        SQL.AddParam("@BIrth_Date", dtpBirthdate.Value)
        SQL.AddParam("@BIrth_Place", txtBirthplace.Text)
        SQL.AddParam("@Nationality", ddlNationality.SelectedValue)
        SQL.AddParam("@Civil_Status", ddlCivilStatus.SelectedValue)
        SQL.AddParam("@Educational_Attainment", txtEdu.Text)
        SQL.AddParam("@Religion", txtReligion.Text)
        SQL.AddParam("@BRN", txtBRN.Text)
        SQL.AddParam("@Member_Type", ddlMemberType.SelectedValue)
        SQL.AddParam("@Membership_Date", dtpMembershipDate.Value)
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

        If fuPhoto.HasFile = True Then
            Dim bytesPho As Byte()
            Using br As BinaryReader = New BinaryReader(fuPhoto.PostedFile.InputStream)
                bytesPho = br.ReadBytes(fuPhoto.PostedFile.ContentLength)
            End Using
            query = " UPDATE tblMember_Master SET Photo = @Photo " &
                    " WHERE Member_Code = @Member_Code And Status = @Status"
            SQL.FlushParams()
            SQL.AddParam("@Member_Code", txtMemberCode.Text)
            SQL.AddParam("@Photo", bytesPho, SqlDbType.VarBinary)
            SQL.AddParam("@Status", "Active")
            SQL.ExecNonQuery(query)
        End If

        If fuSignature.HasFile = True Then
            Dim bytesSig As Byte()
            Using br As BinaryReader = New BinaryReader(fuSignature.PostedFile.InputStream)
                bytesSig = br.ReadBytes(fuSignature.PostedFile.ContentLength)
            End Using
            query = " UPDATE tblMember_Master SET Signature = @Signature " &
                     " WHERE Member_Code = @Member_Code And Status = @Status"
            SQL.FlushParams()
            SQL.AddParam("@Member_Code", txtMemberCode.Text)
            SQL.AddParam("@Signature", bytesSig, SqlDbType.VarBinary)
            SQL.AddParam("@Status", "Active")
            SQL.ExecNonQuery(query)
        End If


    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='MemberManagement_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub
End Class