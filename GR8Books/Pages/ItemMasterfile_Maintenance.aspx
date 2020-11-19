<%@ Page Title="Item Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="ItemMasterfile_Maintenance.aspx.vb" Inherits="GR8Books.ItemMasterfile_Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtAccountTitle_Inv").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("ItemMasterfile_Maintenance.aspx/ListAccountTitle") %>",
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
                    $(".txtAccountCode_Inv").val(i.item.id);
                },
                minLength: 1
            });
            $(".txtAccountTitle_CostOfSales").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("ItemMasterfile_Maintenance.aspx/ListAccountTitle") %>",
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
                    $(".txtAccountCode_CostOfSales").val(i.item.id);
                },
                minLength: 1
            });
            $(".txtAccountTitle_Sales").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("ItemMasterfile_Maintenance.aspx/ListAccountTitle") %>",
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
                    $(".txtAccountCode_Sales").val(i.item.id);
                },
                minLength: 1
            });
            $(".txtName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("ItemMasterfile_Maintenance.aspx/ListName") %>",
                        data: "{'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    id: item.split('-')[1],
                                    value: item.split('-')[0]
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
                    $(".txtCode").val(i.item.id);
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
            $(".btnAddItemType").click(function () {
                window.open("ItemType_Management.aspx", "newWin", "width=500,height=500")
            });

        });
    </script>

    <%-- Save Item Type --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btnSaveItemType]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.ItemType = $('[id*=txtItemType]').val();
                    $.ajax({
                        type: "POST",
                        url: "ItemMasterfile_Maintenance.aspx/SaveItemType",
                        data: '{ItemType :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalItemType').modal('hide')
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


    <%-- Save Item Category --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btnSaveItemCategory]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.ItemCategory = $('[id*=txtItemCategory]').val();
                    $.ajax({
                        type: "POST",
                        url: "ItemMasterfile_Maintenance.aspx/SaveItemCategory",
                        data: '{ItemCategory :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalItemCategory').modal('hide')
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

    <%-- Save Item UOM --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btnSaveItemUOM]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.ItemUOM = $('[id*=txtItem_UOM]').val();
                    $.ajax({
                        type: "POST",
                        url: "ItemMasterfile_Maintenance.aspx/SaveItemUOM",
                        data: '{ItemUOM :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalItemUOM').modal('hide')
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

    <%-- Save Item UOM --%>
    <script type="text/javascript">
        $(function () {
            $('[id*=btnSaveWarehouse]').click(function () {
                if (Page_IsValid) {
                    var UserDetail = {};
                    UserDetail.ItemWarehouse = $('[id*=txtItem_Warehouse]').val();
                    $.ajax({
                        type: "POST",
                        url: "ItemMasterfile_Maintenance.aspx/SaveItemWarehouse",
                        data: '{ItemWarehouse :' + JSON.stringify(UserDetail) + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            alert(response.d);
                            $('.modalItemWarehouse').modal('hide')
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


    <asp:Panel runat="server" ID="panelItem">
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Photo:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:FileUpload ID="fuItemPhoto" class="fuItemPhoto" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemCode" Text="Item code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtItemCode" class="txtItemCode form-control" runat="server" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtItemCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblBarcode" Text="Barcode:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBarcode" class="txtBarcode form-control" runat="server" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtBarcode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemName" Text="Item name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtItemName" class="txtItemName form-control" runat="server" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtItemName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemType" Text="Item type :" runat="server" />
                    </div>
                    <div class="col">
                        <div class="input-group mb-3">
                        <asp:DropDownList ID="ddlItemType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator3" runat="Server" ControlToValidate="ddlItemType" InitialValue="--Select Item Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                            <div class="input-group-append">
                                <button id="tbnAddItemType" runat="server" type="button" class="btn btn-primary btn-m btn-block" data-toggle="modal" data-target="#modalItemType" data-whatever="@mdo">Add</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemCategory" Text="Item category:" runat="server" />
                    </div>
                    <div class="col">
                        <div class="input-group mb-3">
                            <asp:DropDownList ID="ddlItemCategory" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator4" runat="Server" ControlToValidate="ddlItemCategory" InitialValue="--Select Item Category--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        <div class="input-group-append">
                            <button id="btnAddItemCategory" runat="server" type="button"  class="btn btn-primary btn-m btn-block" data-toggle="modal" data-target="#modalItemCategory" data-whatever="@mdo">Add</button>
                         </div>
                        </div>   
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemUOM" Text="Item UOM:" runat="server" />
                    </div>


                     <div class="col">
                        <div class="input-group mb-3">
                        <asp:DropDownList ID="ddlItemUOM" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator8" runat="Server" ControlToValidate="ddlItemUOM" InitialValue="--Select Item UOM--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        <div class="input-group-append">
                        <button id="btnAddItemUOM" runat="server" type="button"  class="btn btn-primary btn-m btn-block" data-toggle="modal" data-target="#modalItemUom" data-whatever="@mdo">Add</button>
                         </div>
                        </div>   
                    </div>


                 
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblItemUOM_QTY" Text="UOM QTY:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtItemUOM_QTY" class="txtCost form-control" runat="server" Type="number" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator7" runat="Server" ControlToValidate="txtItemUOM_QTY" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Label1" Text="Cost:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCost" class="txtCost form-control" runat="server" Type="number" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator9" runat="Server" ControlToValidate="txtCost" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="lblPrice" Text="Price:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtPrice" class="txtPrice form-control" runat="server" Type="number" autocomplete="off" />
                         <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtPrice" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Label2" Text="Location:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtLocation" class="txtLocation form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Label3" Text="Warehouse:" runat="server" />
                    </div>


                     <div class="col">
                        <div class="input-group mb-3">
                            <asp:DropDownList ID="ddlWarehouse" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <div class="input-group-append">
                            <button id="btnAddWarehouse" runat="server" type="button" class="btn btn-primary btn-m btn-block" data-toggle="modal" data-target="#modalItemWarehouse" data-whatever="@mdo">Add</button>
                         </div>
                        </div>   
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Label8" Text="Lot no.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtLotNo" class="txtLotNo form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm">
                <div class="row">
                    <div class="col-3 my-auto">
                        <asp:Label ID="Label9" Text="Bin:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBin" class="txtBin form-control" runat="server" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <%-- Default Supplier --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Default Supplier" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row">
            <div class="col-sm">
                <div class="row">
                    <div class="col-2 my-auto">
                        <asp:Label ID="Label4" Text="Name:" runat="server" />
                    </div>
                    <div class="col-3 my-auto">
                        <asp:TextBox ID="txtCode" class="txtCode form-control" runat="server" Placeholder="VCECode" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtName" class="txtName form-control" runat="server" Placeholder="VCEName" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <%-- Default Accounting Entry --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Default Accounting Entry" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-2 my-auto">
                        <asp:Label ID="Label5" Text="Inventory account:" runat="server" />
                    </div>
                    <div class="col-3 my-auto">
                        <asp:TextBox ID="txtAccountCode_Inv" class="txtAccountCode_Inv form-control" runat="server" Placeholder="Account code" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccountTitle_Inv" class="txtAccountTitle_Inv form-control" runat="server" Placeholder="Account title"  autocomplete="off"/>
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-2 my-auto">
                        <asp:Label ID="Label6" Text="Sales account:" runat="server" />
                    </div>
                    <div class="col-3 my-auto">
                        <asp:TextBox ID="txtAccountCode_Sales" class="txtAccountCode_Sales form-control" runat="server" Placeholder="Account code" autocomplete="off" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccountTitle_Sales" class="txtAccountTitle_Sales form-control" runat="server" Placeholder="Account title" autocomplete="off" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm">
                <div class="row">
                    <div class="col-2 my-auto">
                        <asp:Label ID="Label7" Text="Cost of sales account:" runat="server" />
                    </div>
                    <div class="col-3 my-auto">
                        <asp:TextBox ID="txtAccountCode_CostOfSales" class="txtAccountCode_CostOfSales form-control" runat="server" Placeholder="Account code" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAccountTitle_CostOfSales" class="txtAccountTitle_CostOfSales form-control" runat="server" Placeholder="Account title" autocomplete="off"/>
                    </div>
                </div>
            </div>
        </div>
        <div class="row justify-content-end mb-4">
            <div class="col-2">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-2">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>

        <%-- Add Item Type Modal --%>

        <div class="modal fade" id="modalItemType" tabindex="-1" role="dialog" aria-labelledby="modalLabelItemType" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelItemType">Add Item Type</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label Text="Item Type" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtItemType" Class="txtItemType form-control" runat="server" Placeholder="Item Type" autocomplete="off" />
                                    <%-- <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator10" runat="Server" ControlToValidate="txtItemType" ErrorMessage="Field is required." ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveItemType" class="btnSaveItemType btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="ItemType_Load" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Add Item Categoty Modal --%>

        <div class="modal fade" id="modalItemCategory" tabindex="-1" role="dialog" aria-labelledby="modalLabelItemCategory" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelItemCategory">Add Item Category</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label Text="Item Category" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtItemCategory" Class="txtItemCategory form-control" runat="server" Placeholder="Item Category" autocomplete="off"  />
                                    <%-- <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator11" runat="Server" ControlToValidate="txtItemCategory" ErrorMessage="Field is required." ValidationGroup="b"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveItemCategory" class="btnSaveItemCategory btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="ItemCategory_Load" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Add Item UOM Modal --%>

        <div class="modal fade" id="modalItemUom" tabindex="-1" role="dialog" aria-labelledby="modalLabelItemUOM" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelItemUOM">Add Item UOM</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label Text="Item UOM" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtItem_UOM" Class="txtItem_UOM form-control" runat="server" Placeholder="Item UOM" autocomplete="off" />
                                    <%-- <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator12" runat="Server" ControlToValidate="txtItem_UOM" ErrorMessage="Field is required." ValidationGroup="c"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveItemUOM" class="btnSaveItemUOM btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="ItemUOM_Load" />
                    </div>
                </div>
            </div>
        </div>


        <%-- Add Item Warehouse Modal --%>
        <div class="modal fade" id="modalItemWarehouse" tabindex="-1" role="dialog" aria-labelledby="modalLabelItemWarehouse" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelItemWarehouse">Add Item Warehouse</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-2">
                                    <asp:Label Text="Item Warehouse" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtItem_Warehouse" Class="txtItem_Warehouse form-control" runat="server" Placeholder="Item Warehouse" autocomplete="off" />
                                    <%-- <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic"    ID="RequiredFieldValidator13" runat="Server" ControlToValidate="txtItem_Warehouse" ErrorMessage="Field is required." ValidationGroup="d"></asp:RequiredFieldValidator>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveWarehouse" class="btnSaveWarehouse btn btn-primary btn-block" runat="server" AutoPostBack="true" OnClick="ItemWarehouse_Load" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
