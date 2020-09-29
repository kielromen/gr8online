<%@ Page Title="Tax Setup"  MaintainScrollPositionOnPostback="true" EnableEventValidation = "false" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Settings.Master" CodeBehind="Settings_Tax.aspx.vb" Inherits="GR8Books.Settings_Tax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="settings" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

     <script type='text/javascript'>
         $(document).ready(function () {
             $(".txtAccntCode_Entry").prop("readonly", true);
             $(".txtCode_Entry").prop("readonly", true);

             $("#<%=txtAccntName.ClientID%>").autocomplete({
                 source: function (request, response) {
                     $.ajax({
                         url: "<%= ResolveUrl("Settings_Tax.aspx/ListAccountTitle")%>",
                        data: "{'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    id: item.split('--')[1],
                                    value: item.split('--')[0]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    ("#<%=txtAccntCode.ClientID%>").val(i.item.id);
                    },
                 minLength: 1
             });

             $("#<%=btnSave.ClientID%>").click(function (e) {
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

    <asp:Panel ID="panelConrols" runat="server">
    <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
            <strong>Successfully saved!</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
    </div>
    <div class="row row-cols-2">
           <div class="col-12">
                <asp:GridView ID="gvTax" runat="server" AutoGenerateColumns="false"  Width="100%" GridLines="Both"  OnSelectedIndexChanged = "OnSelectedIndexChanged"  OnRowDataBound="GridView_RowDataBound"  >                       
                    <Columns>
                        <asp:BoundField DataField="TaxCode" HeaderText="Tax Code" />
                        <asp:BoundField DataField="TaxType" HeaderText="Tax Type"   />
                        <asp:BoundField DataField="TaxDescription" HeaderText="Tax Description" />
                        <asp:BoundField DataField="TaxRate" HeaderText="Tax Rate" />
                        <asp:BoundField DataField="TaxAlias" HeaderText="Tax Alias" />
                        <asp:BoundField DataField="AccountCode" HeaderText="Account Code" />
                        <asp:BoundField DataField="AccountTitle" HeaderText="Account Title" />
                        <asp:BoundField DataField="ATC" HeaderText="ATC" />
                        <asp:BoundField DataField="NatureOfIncome" HeaderText="Nature Of Income" />
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
   </div>

   <asp:Panel ID="panel" runat="server">

            <div class="row mb-2">
                <div class="col-6">
                     <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Tax Code:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                            <asp:TextBox ID="txtTaxCode" runat="server"  class="form-control form-control"  autocomplete="off" />
                         </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Tax Type:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                            <asp:DropDownList ID="ddlTaxType" AutoPostBack="True" runat="server" class="form-control" >
                            </asp:DropDownList>                   
                         </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Tax Description:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                            <asp:TextBox ID="txtTaxDescription" runat="server"  class="form-control"  autocomplete="off" />
                         </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Tax Rate:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                            <asp:TextBox ID="txtTaxRate"  textmode="number" runat="server"  class="txtTaxRate form-control"  autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtTaxRate" ErrorMessage="Field is required." ValidationGroup="g" Visible="True"></asp:RequiredFieldValidator>
                         </div>
                    </div>
                    <div class="row mb-2">
                       <div class="col-3 my-auto">
                            <asp:Label Text="Tax Alias:" class="col-form-label" runat="server" />
                       </div>
                       <div class="col-6">
                             <asp:TextBox ID="txtTaxAlias" runat="server"  class="form-control form-control-sm"  autocomplete="off" />
                       </div>
                 </div>
                </div>
            <div class="col-6">
                 <div class="row mb-2">
                       <div class="col-3 my-auto">
                            <asp:Label Text="AccntCode:" runat="server"  class="col-form-label" />
                       </div>
                       <div class="col-6">
                            <asp:TextBox ID="txtAccntCode" runat="server" class="form-control" autocomplete="off"/>
                       </div>
                 </div>
                 <div class="row mb-2">
                       <div class="col-3 my-auto">
                            <asp:Label Text="AccntName:" runat="server" class="col-form-label"  />
                       </div>
                       <div class="col-6">
                            <asp:TextBox ID="txtAccntName" runat="server" class="form-control" autocomplete="off"/>       
                       </div>
                 </div>
                 <div class="row mb-2">
                       <div class="col-3 my-auto">
                            <asp:Label Text="ATC:" class="col-form-label" runat="server" />
                       </div>
                       <div class="col-6">
                            <asp:TextBox ID="txtATC" runat="server"  class="form-control"  autocomplete="off" />
                       </div>
                 </div>
                 <div class="row mb-2">
                       <div class="col-3 my-auto">
                            <asp:Label Text="Nature of Income:" class="col-form-label" runat="server" />
                       </div>
                       <div class="col-6">
                            <asp:TextBox ID="txtNatureOfIncome" runat="server"  class="form-control"  autocomplete="off" />
                       </div>
                 </div>
                 <div class="row">
                    <asp:HiddenField ID="hfTransType" runat="server"/>               
                 </div>  

                  <div class="row mt-4 justify-content-end">
                    <div class="col-3">
                          <asp:Button ID="btnNew" Text="New" class="btnNew btn btn-primary btn-block" runat="server" />
                    </div>
                    <div class="col-3">
                          <asp:Button ID="btnSave" Text="Save" class="btnSave btn btn-primary btn-block" runat="server" />
                    </div>
                </div>
              </div>
           </div>
           </asp:Panel>
</asp:Panel>



</asp:Content>
