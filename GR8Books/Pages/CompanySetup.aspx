<%@ Page Title="Company Setup"  MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CompanySetup.aspx.vb" Inherits="GR8Books.Company_SetUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">                
        $(document).ready(
            function () {
                $('.txtCompany_Contact').click(
                    function () {
                        $(".txtCompany_Contact").mask("0999-999-9999");
                    });
                $('.txtCompany_Contact').keypress(
                    function () {
                        $(".txtCompany_Contact").mask("0999-999-9999");
                    });
                $('.txtTIN_No').click(
                    function () {
                        $(".txtTIN_No").mask("000-000-000");
                    });
                $('.txtBranchCode').click(
                    function () {
                        $(".txtBranchCode").mask("00000");
                    });

                $('#<%= ddlClassification.ClientID%>').change(function () {
                    if ($('#<%=ddlClassification.ClientID%> :selected').text() == "Individual") {
                        $(".txtCompany_Name").val("N/A");
                        $(".txtFirstName").val("");
                        $(".txtMiddleName").val("");
                        $(".txtLastName").val("");
                        $(".txtSuffixName").val("");
                        $(".txtFirstName").prop("readonly", false);
                        $(".txtMiddleName").prop("readonly", false);
                        $(".txtLastName").prop("readonly", false);
                        $(".txtSuffixName").prop("readonly", false);
                        $(".txtCompany_Name").prop("readonly", true);
                        return;
                    }
                    if ($('#<%=ddlClassification.ClientID%> :selected').text() == "Non-Individual") {
                        $(".txtFirstName").prop("readonly", true);
                        $(".txtMiddleName").prop("readonly", true);
                        $(".txtLastName").prop("readonly", true);
                        $(".txtSuffixName").prop("readonly", true);
                        $(".txtCompany_Name").prop("readonly", false);
                        $(".txtFirstName").val("N/A");
                        $(".txtMiddleName").val("N/A");
                        $(".txtLastName").val("N/A");
                        $(".txtSuffixName").val("N/A");
                        $(".txtCompany_Name").val("");
                        return;
                    }
                    else {
                        $(".txtFirstName").prop("readonly", true);
                        $(".txtMiddleName").prop("readonly", true);
                        $(".txtLastName").prop("readonly", true);
                        $(".txtSuffixName").prop("readonly", true);
                        $(".txtCompany_Name").prop("readonly", true);
                        return;
                    }
                 });
            });


    </script>

    <asp:Panel runat="server" ID="panCompanySetUp">
        <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
            <strong>Record successfully saved!</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="row">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Company logo:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:GridView ID="gvImages" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" BorderStyle="None" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Company_Code" HeaderText="CompanyCode" Visible="false" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Image ID="Company_Logo" Height="100px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:FileUpload ID="fuCompanyLogo" class="fuCompanyLogo" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col">
                        <asp:Label Text="Classification:" runat="server" />
                        <asp:DropDownList ID="ddlClassification" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator20" runat="Server" ControlToValidate="ddlClassification" InitialValue="--Select Classification--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row" id="NonIndividual" >
                    <div class="col">
                        <asp:Label Text="Company name:" runat="server" />
                        <asp:TextBox ID="txtCompany_Name" runat="server" class="txtCompany_Name form-control" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="txtCompany_Name" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div id="Individual">
                <div class="row">
                    <div class="col">
                        <asp:Label Text="Last Name:" runat="server" />
                        <asp:TextBox ID="txtLastName" runat="server" class="txtLastName form-control" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator14" runat="Server" ControlToValidate="txtLastName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col">
                        <asp:Label Text="First Name:" runat="server" />
                        <asp:TextBox ID="txtFirstName" runat="server" class="txtFirstName form-control" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator15" runat="Server" ControlToValidate="txtFirstName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <asp:Label Text="Middle Name:" runat="server" />
                        <asp:TextBox ID="txtMiddleName" runat="server" class="txtMiddleName form-control" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator17" runat="Server" ControlToValidate="txtMiddleName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col">
                        <asp:Label Text="Suffix Name:" runat="server" />
                        <asp:TextBox ID="txtSuffixName" runat="server" class="txtSuffixName form-control" />
                    </div>
                </div>
                </div>
            </div>
        </div>
        <%--Company Information--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Company Information" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
             <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="General type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlGeneral_Type" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="ddlGeneral_Type" InitialValue="--Select General Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Industry:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlIndustry" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="ddlIndustry" InitialValue="--Select Industry--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="VAT type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlVAT_Type" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="ddlVAT_Type" InitialValue="--Select VAT Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="TIN:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtTIN_No" class="txtTIN_No form-control" runat="server" Placeholder="000-000-000" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtTIN_No" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div> 
        </div>
         <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Branch Code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBranchCode" class="txtBranchCode form-control" runat="server" Placeholder="00000" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator12" runat="Server" ControlToValidate="txtBranchCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>       
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="RDO:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlRDO" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator13" runat="Server" ControlToValidate="ddlRDO" InitialValue="--Select RDO--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>      
        </div>
        <%--Address--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Address Information" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Region:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlRegion" class="ddlRegion form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="Region_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlRegion" InitialValue="--Select Region--" ErrorMessage="Field is required." ValidationGroup="g" Visible="True"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Province:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlProvince" class="ddlProvince form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="Province_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="ddlProvince" InitialValue="--Select Province--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="City/Municipality:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlCityMun" class="ddlCityMu form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="CityMun_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator10" runat="Server" ControlToValidate="ddlCityMun" InitialValue="--Select City/Municipality--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Barangay:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlBarangay" class="ddlBarangay form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator11" runat="Server" ControlToValidate="ddlBarangay" InitialValue="--Select Barangay--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row  mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Lot / Unit:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Lot_Unit" class="txtAddress_Lot_Unit form-control" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Blk / Bldg:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Blk_Bldg" class="txtAddress_Blk_Bldg form-control" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Street:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Street" class="txtAddress_Street form-control" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Subdivision:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Subd" class="txtAddress_Subd form-control" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-6">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Zip code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_ZipCode" class="txtAddress_ZipCode form-control" runat="server" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator16" runat="Server" ControlToValidate="txtAddress_ZipCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <%--Contact Information--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Contact Information" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
             <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Contact no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCompany_Contact" class="txtCompany_Contact form-control" runat="server" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCompany_Contact" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Email address:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCompany_Email" class="form-control" runat="server" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtCompany_Email" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <div class="row justify-content-end mt-4">
        <div class="col-sm-2">
            <asp:Button Text="Save" ID="btnSave" runat="server" class="btn btn-primary btn-block" />
        </div>
    </div>
</asp:Content>
