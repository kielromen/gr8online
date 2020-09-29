<%@ Page Title="Tax Setup" Language="vb" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="TaxSetup_Management.aspx.vb" Inherits="GR8Books.TaxSetup_Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtAccountTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("TaxSetup_Management.aspx/ListAccountTitle") %>",
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
                    $(".txtAccountCode").val(i.item.id);
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

      <%-- Save Tax Type --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btnSaveTaxType]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.TaxType = $('[id*=txtTaxType]').val();
                    $.ajax({
                        type: "POST",
                        url: "TaxSetup_Management.aspx/SaveTaxType",
                        data: '{TaxType :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalTaxType').modal('hide')
                            return false;
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
            });
        });
    </script>

    
      <%-- Save Tax Form --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btSaveTaxForm]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.TaxForm = $('[id*=txtTaxForm]').val();
                    $.ajax({
                        type: "POST",
                        url: "TaxSetup_Management.aspx/SaveTaxForm",
                        data: '{TaxForm :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalForm').modal('hide')
                            return false;
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
            });
        });
    </script>


    <asp:Panel runat="server" ID="panelTaxSetUp">
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Tax type:" runat="server" />
            </div>
            <div class="col-9">
                <asp:DropDownList ID="ddlTaxType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="ddlTaxType form-control"></asp:DropDownList>
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlTaxType" InitialValue="--Select Tax Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
           <div class="col-1">
                <button id="btnAddTaxType" runat="server" type="button" class="btn btn-secondary btn-m btn-block" data-toggle="modal" data-target="#modalTaxType" data-whatever="@mdo">Add</button>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Normal:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList runat="server" ID="ddlNormal" EnableViewState="true" AppendDataBoundItems="true" class="form-control">
                </asp:DropDownList>
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator2" runat="Server" InitialValue="--Select Normal--" ControlToValidate="ddlNormal" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Percentage(%):" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtPercentage" Class="txtPercentage form-control" runat="server" Type="number" autocomplete="off" />
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtPercentage" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Name of Tax:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtName" runat="server" class="form-control" autocomplete="off" />
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Form:" runat="server" />
            </div>
            <div class="col-9">
                <asp:DropDownList ID="ddlForm" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="ddlForm form-control"></asp:DropDownList>
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator5" runat="Server" ControlToValidate="ddlForm" InitialValue="--Select Form--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
            <div class="col-1">
                <button id="btnAddTaxForm" runat="server" type="button" class="btn btn-secondary btn-m btn-block" data-toggle="modal" data-target="#modalForm" data-whatever="@mdo">Add</button>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Basis:" runat="server" />
            </div>
            <div class="col"> 
                <asp:TextBox ID="txtBasis" Class="txtBasis form-control" runat="server" autocomplete="off" />
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtBasis" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Deadline:" runat="server" />
            </div>
            <div class="col-5">
                <asp:DropDownList ID="ddlMonth" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="ddlMonth form-control" AutoPostBack="true" OnSelectedIndexChanged="LoadDay"></asp:DropDownList>
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlMonth" InitialValue="--Select Month--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
           <div class="col-5">
               <asp:DropDownList ID="ddlDay" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="ddlDay form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator10" runat="Server" ControlToValidate="ddlDay" InitialValue="--Select Day--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Account code:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtAccountCode" Class="txtAccountCode form-control" runat="server"/>
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtAccountCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto" >
                <asp:Label Text="Account title:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtAccountTitle" Class="txtAccountTitle form-control" runat="server" autocomplete="off" />
                 <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"   ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtAccountTitle" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>




        <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button Text="Save" ID="btnSave" Class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-2 ">
                <asp:Button Text="Cancel" ID="btnCancel" Class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>


       <%-- Add TaxType --%>

        <div class="modal fade" id="modalTaxType" tabindex="-1" role="dialog" aria-labelledby="modalLabelTaxType" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelTaxType">Add Tax Type</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row mb-2">
                                <div class="col-2 my-auto" >
                                    <asp:Label Text="Tax Type" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTaxType" Class="txtTaxType form-control" runat="server" Placeholder="Tax Type" autocomplete="off"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveTaxType" class="btnSaveTaxType btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="TaxType_Load" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Add Form --%>

        <div class="modal fade" id="modalForm" tabindex="-1" role="dialog" aria-labelledby="modalLabelForm" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="CheckDate">Add Tax Form</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row mb-2">
                                <div class="col-2 my-auto" >
                                    <asp:Label Text="Tax Form" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTaxForm" Class="txtTaxForm form-control" runat="server" Placeholder="Tax Form" autocomplete="off"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btSaveTaxForm" class="btSaveTaxForm btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="TaxForm_Load" />
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
