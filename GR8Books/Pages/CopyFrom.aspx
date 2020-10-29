<%@ Page Title="Copy From" Language="vb"  EnableEventValidation = "false"  AutoEventWireup="false" MasterPageFile="~/Master/List.Master" CodeBehind="CopyFrom.aspx.vb" Inherits="GR8Books.CopyFrom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
    <style type="text/css">
     .hidden
     {
         display:none;
     }
    </style>

      <asp:Panel runat="server">   
          <div class="row">
                <div class="col">
                    <div class="row">   
                         <div class="col-sm-4"> 
                              <asp:Button Text="Choose" ID="bntChoose"  class="bntChoose btn btn-primary" runat="server"  />
                        </div>
                         <div class="col-sm-8"> 
                             <div class="input-group mb-3">
                                <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="Transno/Name/Remarks"/>
                                    <div class="input-group-append">
                                    <asp:Button Text="Search" ID="btnSearch"  class="btnSearch btn btn-secondary" runat="server"  />
                                </div>
                            </div>
                        </div>

                       
                    </div>
                </div>
          </div> 

          <asp:GridView ID="dgvList" runat="server" AutoGenerateColumns="false"  CellPadding="4" ForeColor="#333333" Width="100%" GridLines="none"   OnRowDataBound="GridView_RowDataBound"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">                       
          <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkCheck" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="TransID" HeaderText="Trans ID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/> 
                <asp:BoundField DataField="TransNo" HeaderText="Trans No" />
                <asp:BoundField DataField="TransDate" HeaderText="Trans Date" />
                <asp:BoundField DataField="Name" HeaderText="Name" /> 
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="TotalAmount" HeaderText="Amount"  ItemStyle-HorizontalAlign="Right"  />
                <asp:BoundField DataField="Status" HeaderText="Status" />
           </Columns>
             <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
           </asp:GridView>
      </asp:Panel>
</asp:Content>
