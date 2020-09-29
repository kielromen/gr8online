<%@ Page Title="Bank Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="BankMasterfile_Management.aspx.vb" Inherits="GR8Books.BankMasterfile_Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtAccntTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("DisbursementType_Maintenance.aspx/ListAccountTitle") %>",
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
                    $(".txtAccntCode").val(i.item.id);
                },
                minLength: 1
            });

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
    <asp:Panel runat="server" ID="panelBank">
        <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
            <strong>Record successfully saved!</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblBank" Text="Bank:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBank" class="txtBank form-control" runat="server" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtBank" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
             <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Type" Text="Type of Bank Account:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlType" InitialValue="--Select Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
          
            
        </div>
        <div class="row mb-2">
             <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblBranch" Text="Branch:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBranch" class="txtBranch form-control" runat="server" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtBranch" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>   
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblSeriesDigits" Text="No. of Checks per Booklet:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSeriesDigits" class="txtSeriesDigits form-control" runat="server" Type="number" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtSeriesDigits" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>     
        </div>
        <div class="row  mb-2">
              
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblAccntCode" Text="Account code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccntCode" class="txtAccntCode form-control" runat="server" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtAccntCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
          <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblSeriesStart" Text="Series start:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSeriesStart" class="txtSeriesStart form-control" runat="server" Type="number" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtSeriesStart" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
           
        </div>
        <div class="row mb-2">
              <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblAccntTitle" Text="Account Title:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccntTitle" class="txtAccntTitle form-control" runat="server" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtAccntTitle" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblSeriesEnd" Text="Series end:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtSeriesEnd" class="txtSeriesEnd form-control" runat="server" Type="number" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtSeriesEnd" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>    
        </div>
        <div class="row mb-2 justify-content-start">     
             <div class="col-sm-6">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblAccntNo" Text="Account number:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccntNo" class="txtAccntNo form-control" runat="server" />
                        <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtAccntNo" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>      
        </div>
        <div class="row justify-content-end mb-4">
            <div class="col-sm-2 mb-2">
                <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="g" class="btnSave btn btn-primary btn-block" />
            </div>
            <div class="col-sm-2">
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btnCancel btn btn-primary btn-block" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
