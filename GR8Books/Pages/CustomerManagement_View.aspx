<%@ Page Title="Customer Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CustomerManagement_View.aspx.vb" Inherits="GR8Books.CustomerManagement_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var Code = $(this).attr("title");
                var Actions = "View"
                window.open("CustomerManagement.aspx?id=" + Code + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var Code = $(this).attr("title");
                var Actions = "Edit"
                window.open("CustomerManagement.aspx?id=" + Code + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "CustomerManagement.aspx";
                return false;
            });

            $(".btnInactive").click(function () {
                if (confirm("Are you sure you want to remove this?")) {
                }
                else {
                    return false;
                }
            });
        });
    </script>
    <style type="text/css">
     .hidden
     {
         display:none;
     }
    </style>

    <asp:Panel runat="server">
         <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button ID="btnExport" class="btnExport btn btn-success btn-block" Text="Export" runat="server" />
            </div>
        </div>
        <br />
        <asp:GridView ID="gvCustomer" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Customer_Code" HeaderText="Customer Code" />
                <asp:BoundField DataField="Classification" HeaderText="Classification" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                <asp:BoundField DataField="First_Name" HeaderText="First_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Last_Name" HeaderText="Last_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Middle_Name" HeaderText="Middle_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Suffix_Name" HeaderText="Suffix_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="TIN_No" HeaderText="TIN_No" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                 <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Lot_Unit" HeaderText="Billing_Lot_Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Blk_Bldg" HeaderText="Billing_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Street" HeaderText="Billing_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Subd" HeaderText="Billing_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Brgy" HeaderText="Billing_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Billing_Town_City" HeaderText="Billing_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Province" HeaderText="Billing_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Region" HeaderText="Billing_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_ZipCode" HeaderText="Billing_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                
                <asp:BoundField DataField="Delivery_Lot_Unit" HeaderText="Delivery_Lot_Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Blk_Bldg" HeaderText="Delivery_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Street" HeaderText="Delivery_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Subd" HeaderText="Delivery_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Brgy" HeaderText="Delivery_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Delivery_Town_City" HeaderText="Delivery_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Province" HeaderText="Delivery_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Region" HeaderText="Delivery_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_ZipCode" HeaderText="Delivery_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Person" HeaderText="Contact Person" />
                <asp:BoundField DataField="Contact_Position" HeaderText="Contact_Position" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Telephone" HeaderText="Contact_Telephone" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Cellphone" HeaderText="Contact Cellphone" />
                <asp:BoundField DataField="Contact_Fax" HeaderText="Contact_Fax" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Email" HeaderText="Contact_Email" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Website" HeaderText="Contact_Website" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Terms" HeaderText="Terms" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="CutOff" HeaderText="CutOff" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="VAT_Type" HeaderText="VAT_Type" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />




                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Customer_Code")%>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Customer_Code")%>' />
                        <asp:Button ID="btnInactive" CssClass="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Customer_Code")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
        <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
