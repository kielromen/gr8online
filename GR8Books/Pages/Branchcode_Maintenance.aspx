<%@ Page Title="Branch Code" Language="vb" AutoEventWireup="false" CodeBehind="Branchcode_Maintenance.aspx.vb" Inherits="GR8Books.Branchcode_Maintenance" MasterPageFile="~/Master/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {


            $("#<%=btnSave.ClientID%>").click(function () {
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
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblDescription" runat="server" Text="Description:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtDescription" runat="server" Class="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblBranchcode" runat="server" Text="Branch Code:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtbranchcode" Class="txtBranchcode form-control" runat="server" />
            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtBranchcode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblDate" runat="server" Text="Date:"></asp:Label>
        </div>
        <div class="col">
            <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="TxtStatus" runat="server" Class="form-control" Placeholder="Status:"></asp:TextBox>
        </div>
    </div>
    <div class="row mt-2">
        <div class="col text-right">
            <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="g" class="btnSave btn btn-primary" />
            <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btnCancel btn btn-primary" />
        </div>
    </div>

</asp:Content>
