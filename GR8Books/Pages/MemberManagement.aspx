<%@ Page Title="Member Management" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="MemberManagement.aspx.vb" Inherits="GR8Books.MemberManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">                
        $(document).ready(function () {
            $('.txtCellphone').click(
                function () {
                    $(".txtCellphone").mask("0999-999-9999");
                });
            $('.txtCellphone').keypress(
                function () {
                    $(".txtCellphone").mask("0999-999-9999");
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


    <asp:Panel runat="server" ID="panelMember">
        <%--Personal Information--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Personal Information" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Member code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtMemberCode" Class="txtMemberCode form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtMemberCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Member name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtMemberName" class="txtMemberName form-control" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Title:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlTitle" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="ddlTitle" InitialValue="--Select Title--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:Label Text="Last name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtLastName" class="txtLastName  form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtLastName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtFirstName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:Label Text="Nick name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtNickName" class="txtNickName form-control" runat="server" autocomplete="off"  />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Gender:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlGender" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="ddlGender" InitialValue="--Select Gender--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Birthdate:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpBirthdate" class="form-control">
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="dtpBirthdate" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Birth place:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBirthplace" class="txtBirthplace  form-control" runat="server" autocomplete="off"  />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtBirthplace" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Nationality:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlNationality" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlNationality" InitialValue="--Select Nationality--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Civil status:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlCivilStatus" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator10" runat="Server" ControlToValidate="ddlCivilStatus" InitialValue="--Select Civil Status--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Educational attainment:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtEdu" class="txtEdu  form-control" runat="server" autocomplete="off"  />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Religion:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtReligion" class="txtReligion  form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator11" runat="Server" ControlToValidate="txtReligion" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Cellphone no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCellphoneNo" Class="txtCellphone form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator20" runat="Server" ControlToValidate="txtCellphoneNo" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Email:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtEmail" Class="txtEmail form-control" runat="server" TextMode="Email" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator21" runat="Server" ControlToValidate="txtEmail" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Photo:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:FileUpload ID="fuPhoto" class="fuPhoto" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-6">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Signature:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:FileUpload ID="fuSignature" Class="fuSignature" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <%--Membership Information--%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Membership Information" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Member type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlMemberType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator12" runat="Server" ControlToValidate="ddlMemberType" InitialValue="--Select Member Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Membership date:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpMembershipDate" class="form-control">
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator13" runat="Server" ControlToValidate="dtpMembershipDate" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-6">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Board resolution no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBRN" class="txtBRN form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator14" runat="Server" ControlToValidate="txtBRN" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:TextBox ID="txtStreet" class="txtStreet form-control" runat="server" autocomplete="off"  />
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
                        <asp:Label Text="Region:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList runat="server" ID="ddlRegion" class="ddlRegion form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="Region_Changed">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator15" runat="Server" ControlToValidate="ddlRegion" InitialValue="--Select Region--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator16" runat="Server" ControlToValidate="ddlProvince" InitialValue="--Select Province--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator17" runat="Server" ControlToValidate="ddlCityMun" InitialValue="--Select City/Municipality--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator18" runat="Server" ControlToValidate="ddlBarangay" InitialValue="--Select Barangay--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-6">
                <div class="row mb-2">
                    <div class="col-3">
                        <asp:Label Text="Zip code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtZipCode" class="txtZipCode form-control" runat="server" autocomplete="off" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator19" runat="Server" ControlToValidate="txtZipCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row justify-content-end mt-4">
            <div class="col-sm-2">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
