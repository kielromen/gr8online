<%@ Page Title="Transaction ID Setup" EnableEventValidation = "false"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Settings.Master" CodeBehind="Settings_TransactionID.aspx.vb" Inherits="GR8Books.Settings_TransactionID" %>
<asp:Content ID="Content1" ContentPlaceHolderID="settings" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


<asp:Panel ID="panelConrols" runat="server">
    <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
            <strong>Successfully saved!</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
    </div>
    <div class="row row-cols-2">
           <div class="col-4">
                <asp:GridView ID="gvTransSetUp" runat="server" AutoGenerateColumns="false"  Width="100%" GridLines="Both"  OnSelectedIndexChanged = "OnSelectedIndexChanged"  OnRowDataBound="GridView_RowDataBound"  >                       
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="Description"   />
                        <asp:BoundField DataField="TransType" HeaderText="Transaction Type" />
                    </Columns>
                     <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
                <br />
               
           </div>
           <asp:Panel ID="panel" runat="server">
           <div class="col">
                 <div class="row">
                    <div class="col-sm-9">
                    <asp:GridView runat="server" ID="gvTransDetails" Width="100%"  AutoGenerateColumns="false">                    
                        <Columns>
                            <asp:BoundField DataField="BranchCode" HeaderText="Branch"  />
                            <asp:BoundField DataField="BusinessCode" HeaderText="Business Type" />
                            <asp:BoundField DataField="Prefix" HeaderText="Prefix" />
                            <asp:BoundField DataField="Digits" HeaderText="Digits"  />
                            <asp:BoundField DataField="StartRecord" HeaderText="Start Record" />
                        </Columns>
                         <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>                     
                    </div>
                 </div>
                 <br />
                 <div class="row">
                        <asp:Label Text="Description:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtDescription" runat="server"  class="form-control form-control-sm"  autocomplete="off" />
                     </div>
                </div>
                <div class="row">
                        <asp:Label Text="Branch:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                            <asp:DropDownList ID="ddlBranchCode" AutoPostBack="True" runat="server" class="form-control" >
                            </asp:DropDownList>                   
                    </div>
                </div>
                 <div class="row">
                        <asp:Label Text="Business Type:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                        <asp:DropDownList ID="ddlBusinessCode" AutoPostBack="True" runat="server" class="form-control" >
                         </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                        <asp:Label Text="Prefix:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtPrefix" runat="server"  class="form-control form-control-sm"  autocomplete="off" />
                    </div>
                </div>
                <div class="row">
                        <asp:Label Text="Digits:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtDigits"  textmode="number" runat="server"  class="txtDigits form-control form-control-sm"  autocomplete="off" />
                       <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtDigits" ErrorMessage="Field is required." ValidationGroup="g" Visible="True"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                <asp:Label Text="Start Record:" class="col-sm-3 col-form-label col-form-label-sm" runat="server" />
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtStartRecord" textmode="number" runat="server"  class="txtStartRecord form-control form-control-sm"  autocomplete="off" />
                       <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtStartRecord" ErrorMessage="Field is required." ValidationGroup="g" Visible="True"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <asp:CheckBox Text="Generate Transaction ID Automacally" ID="chkAuto" runat="server"  class="col-sm col-form-label col-form-label-sm"  />
                </div>
                <div class="row">
                    <asp:CheckBox Text="Global Series"  ID="chkGlobal" runat="server" class="col-sm col-form-label col-form-label-sm"  />
                </div>  
                <div class="row">
                    <asp:HiddenField ID="hfTransType" runat="server"/>               
                </div>  
                <div class="row mt-1 justify-content-start">
                    <div class="col-2 ">
                          <asp:Button ID="btnSave" Text="Save" class="btnSave btn btn-primary btn-block" runat="server" />
                     </div>
                </div>
           </div>
           </asp:Panel>
    </div>
</asp:Panel>
</asp:Content>
