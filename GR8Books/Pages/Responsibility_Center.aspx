<%@ Page Title="Responsibility Center " Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Responsibility_Center.aspx.vb" Inherits="GR8Books.Responsibility_Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type='text/javascript'>
        $(document).ready(function () {

            $("#<%=btnSave.ClientID%>").click(function (e) {
                totalDBCR();
                var totaldebit = $(".lblTotalDebit_Amount").text();
                var totalcredit = $(".lblTotalCredit_Amount").text();
                if (totaldebit != totalcredit) {
                    e.preventDefault
                    alert("Total Debit Credit No Match");
                    return false;
                }
                else {

                    if (Page_IsValid) {
                        if (confirm("Are you sure you want to save?")) {

                        }
                        else {
                            return false;
                        }
                    }
                }
            });

            $(".btnSearch").click(function () {
                window.location = "ResponsibilityCenter_Loadlist.aspx";
                return false;
            });
        });
    </script>

    <asp:Panel ID="Panel1" runat="server">
        <div class="row mt-4 justify-content-start">
            <div class="col">
                <asp:Button Text="Search" ID="btnSearch" class="btnSearch btn btn-primary" runat="server" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btn btn-primary" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
            </div>
        </div>
        <hr />

        <asp:Panel ID="panelConrols" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Cost Center Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCC_Code" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtCC_Code" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Cost Center Name:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCC_Name" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtCC_Name" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row mb-2">
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Profit Center Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtPC_Code" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtPC_Code" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Profit Center Name:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtPC_Name" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtPC_Name" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mb-2">
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Investment Center Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtIC_Code" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtIC_Code" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="row">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Investment Center Name:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtIC_Name" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtIC_Name" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
