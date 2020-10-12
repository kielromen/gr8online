<%@ Page Title="Default Account Setup" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="DefaultAccounts.aspx.vb" Inherits="GR8Books.DefaultAccounts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
   <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
   <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    
      <script type='text/javascript'>
          $(document).ready(function () {
              $(".txtDefaultAccountTitle").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: "<%= ResolveUrl("DefaultAccounts.aspx/ListAccountTitle")%>",
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
                    id = id.replace("txtDefaultAccountTitle", "txtDefaultAccountCode");
                    $("#" + id).val(i.item.id);
                },
                minLength: 1
            });
        });
     </script>
        
      <script type='text/javascript'>
          $(document).ready(function () {
              $(".txtRefAccountTitle").autocomplete({
                  source: function (request, response) {
                      $.ajax({
                          url: "<%= ResolveUrl("DefaultAccounts.aspx/ListAccountTitle")%>",
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
                      id = id.replace("txtRefAccountTitle", "txtRefAccountCode");
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
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Description:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlDescription" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlDescription" InitialValue="--Select Description--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Code:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtDefaultAccountCode" class="txtDefaultAccountCode form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtDefaultAccountCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Default Account Title:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtDefaultAccountTitle" class="txtDefaultAccountTitle form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtDefaultAccountTitle" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Ref Account Code:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtRefAccountCode" class="txtRefAccountCode form-control" runat="server" />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Ref Account Title:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtRefAccountTitle" class="txtRefAccountTitle form-control" runat="server" />
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
