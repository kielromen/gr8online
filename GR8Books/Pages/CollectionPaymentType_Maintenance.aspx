<%@ Page Title="Collection Payment Type Maintenance" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CollectionPaymentType_Maintenance.aspx.vb" Inherits="GR8Books.CollectionPaymentType_Maintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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
    <div>
        <br />
        <asp:Panel runat="server">
            <asp:Table runat="server" class="table">

                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:label text="Payment Type" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:TextBox ID="txtPaymentType" class="txtPaymentType" runat="server" Placeholder="Payment Type" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtPaymentType" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow runat="server">
                    <asp:TableCell runat="server">
                        <asp:label text="With Bank" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell runat="server">
                        <asp:CheckBox ID="chkWithBank" runat="server"/>  
                    </asp:TableCell>
                </asp:TableRow>

              
            </asp:Table>

            <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
            <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
