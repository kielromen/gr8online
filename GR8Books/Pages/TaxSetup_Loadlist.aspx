<%@ Page Title="Tax Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="TaxSetup_Loadlist.aspx.vb" Inherits="GR8Books.TaxSetup_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("TaxSetup_Management.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=1050,height=600");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("TaxSetup_Management.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=1050,height=600");
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "TaxSetup_Management.aspx";
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
 <%--   <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
        <strong>Record successfully saved!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <div class="alert alert-warning alert-dismissible fade show" role="alert" id="Div1" runat="server">
        <strong>Record successfully updated!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>--%>
    <asp:Panel runat="server">
        <asp:GridView ID="gvTaxSetUp" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                <asp:BoundField DataField="TAX_Type" HeaderText="Tax Type" />
                <asp:BoundField DataField="Name" HeaderText="Name of Tax" />
                <asp:BoundField DataField="Normal" HeaderText="Normal" />
                <asp:BoundField DataField="Percentage" HeaderText="Percentage" />
                <asp:BoundField DataField="Form" HeaderText="Form" />
                <asp:BoundField DataField="AccountCode" HeaderText="Account Code" />
                <asp:BoundField DataField="AccountTitle" HeaderText="Account Title" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("ID") %>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("ID") %>' />
                        <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("ID") %>' />
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
