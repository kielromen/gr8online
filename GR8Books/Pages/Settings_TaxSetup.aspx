<%@ Page Title="Tax Default Account Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Settings.Master" CodeBehind="Settings_TaxSetup.aspx.vb" Inherits="GR8Books.Settings_TaxSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="settings" runat="server">
   
   <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
   <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
   <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    

      <script type='text/javascript'>
          $(document).ready(function () {
              $(".txtAP_InputVAT_Title").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: "<%= ResolveUrl("Settings_TaxSetup.aspx/ListAccountTitle")%>",
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
                    var id = $(this).attr('id');
                    id = id.replace("txtAP_InputVAT_Title", "txtAP_InputVAT");
                    $("#" + id).val(i.item.id);
                },
                minLength: 1
            });
        });
      </script>
        
      <script type='text/javascript'>
          $(document).ready(function () {
              $(".txtAR_OutputVAT_Title").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: "<%= ResolveUrl("Settings_TaxSetup.aspx/ListAccountTitle")%>",
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
                      var id = $(this).attr('id');
                      id = id.replace("txtAR_OutputVAT_Title", "txtAR_OutputVAT");
                      $("#" + id).val(i.item.id);
                  },
                  minLength: 1
              });
          });
      </script>

        
      <script type='text/javascript'>
          $(document).ready(function () {
              $(".txtTAX_Deferred_Title").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: "<%= ResolveUrl("Settings_TaxSetup.aspx/ListAccountTitle")%>",
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
                      var id = $(this).attr('id');
                      id = id.replace("txtTAX_Deferred_Title", "txtTAX_Deferred");
                      $("#" + id).val(i.item.id);
                  },
                  minLength: 1
              });
          });
      </script>

          <script type='text/javascript'>
              $(document).ready(function () {
                  $(".txtTAX_EWT_Title").autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: "<%= ResolveUrl("Settings_TaxSetup.aspx/ListAccountTitle")%>",
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
                      var id = $(this).attr('id');
                      id = id.replace("txtTAX_EWT_Title", "txtTAX_EWT");
                      $("#" + id).val(i.item.id);
                  },
                  minLength: 1
              });
          });
          </script>


         <script type='text/javascript'>
              $(document).ready(function () {
                  $(".txtTAX_CWT_Title").autocomplete({
                      source: function (request, response) {
                          $.ajax({
                              url: "<%= ResolveUrl("Settings_TaxSetup.aspx/ListAccountTitle")%>",
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
                      var id = $(this).attr('id');
                      id = id.replace("txtTAX_CWT_Title", "txtTAX_CWT");
                      $("#" + id).val(i.item.id);
                  },
                  minLength: 1
              });
          });
          </script>

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

      <asp:Panel runat="server" ID="panelDefaultAccount">
        <%-- Inout VAT --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Input VAT" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtAP_InputVAT" class="txtAP_InputVAT form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtAP_InputVAT" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtAP_InputVAT_Title" class="txtAP_InputVAT_Title form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtAP_InputVAT_Title" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <%-- Output VAT --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Output VAT" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtAR_OutputVAT" class="txtAR_OutputVAT form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtAR_OutputVAT" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtAR_OutputVAT_Title" class="txtAR_OutputVAT_Title form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtAR_OutputVAT_Title" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
           <%-- Deffered Output VAT --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Deffered Output VAT" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_Deferred" class="txtTAX_Deferred form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator9" runat="Server" ControlToValidate="txtTAX_Deferred" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_Deferred_Title" class="txtTAX_Deferred_Title form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator10" runat="Server" ControlToValidate="txtTAX_Deferred_Title" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
         <%-- Expanded Withholding TAX --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Expanded Withholding TAX" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_EWT" class="txtTAX_EWT form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtTAX_EWT" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_EWT_Title" class="txtTAX_EWT_Title form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtTAX_EWT_Title" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
         <%-- Creditable Withholding TAX --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Creditable Withholding TAX" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_CWT" class="txtTAX_CWT form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtTAX_CWT" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col-5">
                <asp:TextBox ID="txtTAX_CWT_Title" class="txtTAX_CWT_Title form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtTAX_CWT_Title" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>





        <div class="row mt-4 justify-content-end">
            <div class="col-sm-2 ">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
