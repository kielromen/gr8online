<%@ Page Title="Sales Invoice Transaction List" EnableEventValidation = "false" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="SalesInvoice_Loadlist.aspx.vb" Inherits="GR8Books.SalesInvoice_Loadlist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:Panel runat="server">   
          <div class="row row-cols-2">
                <div class="col">
                    <div class="row">
                         <div class="col-sm-4"> 
                            <asp:TextBox ID="txtFilter" runat="server" class="form-control form-control-sm" autocomplete="off" placeholder="Transno/Name/Remarks"/>
                        </div>
                        <div class="col-sm-3">  
                            <asp:Button Text="Search" ID="btnSearch"  class="btn btn-secondary btn-sm" runat="server"  />
                        </div>
                    </div>
                </div>
          </div> <br />

          <asp:GridView ID="dgvList" runat="server" AutoGenerateColumns="false"  Width="80%" GridLines="none" OnSelectedIndexChanged = "OnSelectedIndexChanged"   OnRowDataBound="GridView_RowDataBound"  >                       
          <Columns>
                <asp:BoundField DataField="TransID" HeaderText="Trans ID" />
                <asp:BoundField DataField="TransNo" HeaderText="Trans No" />
                <asp:BoundField DataField="VCECode" HeaderText="VCE Code" />
                <asp:BoundField DataField="TransDate" HeaderText="Trans Date" />
                <asp:BoundField DataField="GrossAmount" HeaderText="Total Amount" />
                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
           </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="white" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
           </asp:GridView>
      </asp:Panel>
</asp:Content>
