<%@ Page Title="Company Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CompanySetup.aspx.vb" Inherits="GR8Books.Company_SetUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">                
        $(document).ready(
            function () {
                $('.txtTIN_No').click(
                    function () {
                        $(".txtTIN_No").mask("000-000-000-000");
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
                        <asp:Label Text="Company name:" runat="server" />
                        <asp:TextBox ID="txtCompany_Name" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="txtCompany_Name" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <%--Address--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Address" runat="server" Style="color: #808080" /></h6>
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
            <div class="col-sm">
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
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Contact no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCompany_Contact" class="form-control" runat="server" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCompany_Contact" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
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
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="TIN no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtTIN_No" class="txtTIN_No form-control" runat="server" Placeholder="000-000-000-000" />
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtTIN_No" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
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
        </div>
        <div class="row">
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
    </asp:Panel>
    <div class="row justify-content-end mt-4">
        <div class="col-sm-2">
            <asp:Button Text="Save" ID="btnSave" runat="server" class="btn btn-primary btn-block" />
        </div>
    </div>
</asp:Content>
