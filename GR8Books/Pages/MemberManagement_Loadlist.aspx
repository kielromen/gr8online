<%@ Page Title="Member Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="MemberManagement_Loadlist.aspx.vb" Inherits="GR8Books.MemberManagement_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var Code = $(this).attr("title");
                var Actions = "View"
                window.open("MemberManagement.aspx?id=" + Code + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var Code = $(this).attr("title");
                var Actions = "Edit"
                window.open("MemberManagement.aspx?id=" + Code + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "MemberManagement.aspx";
                return false;
            });

            $(".btnInactive").click(function () {
                if ($(".btnInactive").val() == "Inactive") {
                    if (confirm("Are you sure you want to remove this?")) {
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (confirm("Are you sure you want to put back this?")) {
                    }
                    else {
                        return false;
                    }
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
          <div class="row mb-3 mr-2 justify-content-end">
                <div class="col">
                 <div class="input-group">
                      <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="Member Name"  />
                      <div class="input-group-append">
                          <asp:Button Text="Search" ID="btnSearch"  class="btnSearch btn btn-secondary" runat="server" OnClick="loadlist"  />
                      </div>
                 </div>
                 </div>
                 <div class="col">
                    <asp:DropDownList ID="ddlFilter" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Loadlist"></asp:DropDownList>
                 </div>
                 <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
                     <div class="btn-group" role="group" aria-label="First group">
                        <asp:Button ID="btnAdd" Width="100px" Text="Add" class="btnAdd btn btn-primary btn-block" runat="server" />
                        <asp:Button ID="btnUpload" Width="100px" class="btnUpload btn btn-primary" Text="Upload" runat="server" data-toggle="modal" data-target="#modalCOA" data-whatever="@mdo" />
                        <asp:Button ID="btnDownload" Width="100px" class="btnDownload btn btn-success" Text="Download" runat="server" />
                        <asp:Button ID="btnExport" Width="100px" class="btnExport btn btn-success" Text="Export" runat="server" />
                     </div>
                 </div>
            </div>
        <asp:GridView ID="gvMember"  PageSize="13"  runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Member_Code" HeaderText="Member Code" />
                <asp:BoundField DataField="Title" HeaderText="Title" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Member_Name" HeaderText="Member Name" />
                <asp:BoundField DataField="Member_Type" HeaderText="Member Type" />
                <asp:BoundField DataField="Nick_Name" HeaderText="Nick_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Gender" HeaderText="Gender" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Birth_Date" HeaderText="Birth_Date" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Birth_Place" HeaderText="Birth_Place" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Nationality" HeaderText="Nationality" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Civil_Status" HeaderText="Civil_Status" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Educational_Attainment" HeaderText="Educational_Attainment" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Religion" HeaderText="Religion" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="BRN" HeaderText="BRN" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Member_Type" HeaderText="Member_Type" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Membership_Date" HeaderText="Membership_Date" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Blk_Bldg" HeaderText="Address_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Street" HeaderText="Address_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Subd" HeaderText="Address_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Brgy" HeaderText="Address_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Address_Town_City" HeaderText="Address_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Province" HeaderText="Address_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_Region" HeaderText="Address_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Address_ZipCode" HeaderText="Address_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />     
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Member_Code")%>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Member_Code")%>' />
                        <asp:Button ID="btnInactive" CssClass="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Member_Code")%>' />
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

    </asp:Panel>
</asp:Content>
