<%@ Page Title="Terms Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Terms_Management.aspx.vb" Inherits="GR8Books.Terms_Management" %>
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
    <asp:Panel runat="server" ID="panelCollector">
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Description:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtDescription" class="txtDescription form-control" runat="server" AutoComplete="off"  />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtDescription" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Datemode:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlDateMode" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlDateMode" InitialValue="--Select Datemode--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-2 my-auto">
                <asp:Label Text="Period:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtPeriod" TextMode="Number" class="txtPeriod form-control" runat="server" AutoComplete="off" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtPeriod" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="row mt-4 justify-content-end">
            <div class="col-sm-2 ">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2 ">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
