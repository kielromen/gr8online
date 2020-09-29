<%@ Page Title="Employee Management" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Employee_Management.aspx.vb" Inherits="GR8Books.Employee_Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">                
        $(document).ready(function () {
            $('.txtCellphoneNo').click(
                function () {
                    $(".txtCellphoneNo").mask("0999-999-9999");
                });
            $('.txtCellphoneNo').keypress(
                function () {
                    $(".txtCellphoneNo").mask("0999-999-9999");
                });
            $(".btnSave").click(function () {
                if (Page_IsValid) {
                    if (confirm("Are you sure you want to save?")) {

                    }
                    else {
                        return false;
                    }
                }
            });
        });
    </script>

    <asp:Panel ID="panelEmployee" runat="server">
        <div class="row mb-2">
            <div class="col-sm-6 justify-content-start">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Employee code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtEmployee_Code" Class="txtEmployee_Code form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtEmployee_Code" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Last name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtLastName" class="txtLastName  form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtLastName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="First name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtFirstName" class="txtFirstName form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtFirstName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Middle name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtMiddleName" class="txtMiddleName form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtMiddleName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Suffix name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSuffixName" class="txtSuffixName  form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Department:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtDepartment" class="txtDepartment form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtDepartment" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Section:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSection" class="txtSection form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Unit:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtUnit" class="txtUnit form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Cellphone no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCellphoneNo" class="txtCellphoneNo form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtCellphoneNo" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Region:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlRegion" class="ddlRegion form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="Region_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator8" runat="Server" ControlToValidate="ddlRegion" InitialValue="--Select Region--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Province:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlProvince" class="ddlProvince form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="Province_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlProvince" InitialValue="--Select Province--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="City / Municipality:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlCityMun" class="ddlCityMu form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="CityMun_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator10" runat="Server" ControlToValidate="ddlCityMun" InitialValue="--Select City/Municipality--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Barangay:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlBarangay" class="ddlBarangay form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator11" runat="Server" ControlToValidate="ddlBarangay" InitialValue="--Select Barangay--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Lot / Unit:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Lot_Unit" class="txtAddress_Lot_Unit form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Blk / Bldg:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAddress_Blk_Bldg" class="txtAddress_Blk_Bldg form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Street:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtStreet" class="txtStreet form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Subdivision:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSubdivision" class="txtSubdivision form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Zipcode:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtZipCode" class="txtZipCode form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator16" runat="Server" ControlToValidate="txtZipCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Email address:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtEmail" class="txtEmail form-control" runat="server" TextMode="Email" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtEmail" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row justify-content-end">
            <div class="col-sm-2">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>
