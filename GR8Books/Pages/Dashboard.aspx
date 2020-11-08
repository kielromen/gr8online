<%@ Page Title="Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Dashboard.aspx.vb" Inherits="GR8Books.Dashboard1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script src="../Scripts/excanvas.js"></script>
    <script src="../Scripts/Chart.js"></script>
    
    <script type="text/javascript">                
        $(document).ready(function () {
            LoadChartAPV();
            LoadChartGM();
            LoadChartSales();
            $("#<%=ddlAccntTitleAPV.ClientID%>,#<%=ddlAccntTitleYearAPV.ClientID%>").change(function () {
                LoadChartAPV();
            });
            $("#<%=ddlAccntTitleMonthGM.ClientID%>,#<%=ddlAccntTitleYearGM.ClientID%>").change(function () {
                LoadChartGM();
            });
            $("#<%=ddlAccntTitleSales.ClientID%>,#<%=ddlAccntTitleYearSales.ClientID%>").change(function () {
                LoadChartSales();
            });
            function LoadChartAPV() {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetChartType",
                    contentType: "application/json; charset=utf-8",
                    data: "{'AccountTitle': '" + $("#<%=ddlAccntTitleAPV.ClientID%>").val() + "', 'ModuleID' : 'APV'}",
                    dataType: "json",
                    success: function (r) {
                        var labels = r.d[0];
                        var series1 = r.d[1];
                        $("#dvChartAPV").html("");
                        var canvas = document.createElement('canvas');
                        $("#dvChartAPV")[0].appendChild(canvas);
 
                        var ctx = canvas.getContext('2d');
                        ctx.canvas.height = 300;
                        ctx.canvas.width = 470;
                        var lineChart = new Chart(ctx,
                            {
                                type: 'line',
                                data: {
                                    labels: r.d[0],
                                    datasets: [
                                        {
                                            label: $("#<%=ddlAccntTitleAPV.ClientID%>").val(),
                                            lineTension: 0,
                                            backgroundColor: "rgba(0, 123, 255, 0.2)",
                                            borderColor: "rgba(0, 123, 255, 1)",
                                            data: series1
                                        }
                                    ]
                                },
                                options: {
                                    responsive: false,
                                    legend: {
                                        display: false
                                    },
                                    tooltips: {
                                        callbacks: {
                                            label: function(t, d) {
                                                var xLabel = d.datasets[t.datasetIndex].label;
                                                var yLabel = '₱ ' + parseFloat(t.yLabel).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                                return xLabel + ': ' + yLabel;
                                            }
                                        }
                                    },
                                    scales: {
                                        yAxes: [{
                                        ticks: {
                                            callback: function(value, index, values) {
                                                return '₱ ' + parseFloat(value).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                            }
                                        }
                                        }]
                                    }
                                }
                            }
                        );
                    },
                    failure: function (response) {
                        alert('There was an error.');
                    }
                });
            }
            function LoadChartSales() {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetChartTypeByMonth",
                    contentType: "application/json; charset=utf-8",
                    data: "{'AccountTitle': '" + $("#<%=ddlAccntTitleSales.ClientID%>").val() + "', 'ModuleID' : 'Sales'}",
                    dataType: "json",
                    success: function (r) {
                        var labels = r.d[0];
                        var series1 = r.d[1];
                        $("#dvChartSales").html("");
                        var canvas = document.createElement('canvas');
                        $("#dvChartSales")[0].appendChild(canvas);
 
                        var ctx = canvas.getContext('2d');
                        ctx.canvas.height = 300;
                        ctx.canvas.width = 470;
                        var lineChart = new Chart(ctx,
                            {
                                type: 'line',
                                data: {
                                    labels: r.d[0],
                                    datasets: [
                                        {
                                            label: $("#<%=ddlAccntTitleSales.ClientID%>").val(),
                                            lineTension: 0,
                                            backgroundColor: "rgba(0, 123, 255, 0.2)",
                                            borderColor: "rgba(0, 123, 255, 1)",
                                            data: series1
                                        }
                                    ]
                                },
                                options: {
                                    responsive: false,
                                    legend: {
                                        display: false
                                    },
                                    tooltips: {
                                        callbacks: {
                                            label: function(t, d) {
                                                var xLabel = d.datasets[t.datasetIndex].label;
                                                var yLabel = '₱ ' + parseFloat(t.yLabel).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                                return xLabel + ': ' + yLabel;
                                            }
                                        }
                                    },
                                    scales: {
                                        yAxes: [{
                                        ticks: {
                                            callback: function(value, index, values) {
                                                return '₱ ' + parseFloat(value).toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                                            }
                                        }
                                        }]
                                    }
                                }
                            }
                        );
                    },
                    failure: function (response) {
                        alert('There was an error.');
                    }
                });
            }
            function LoadChartGM() {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/GetGM",
                    contentType: "application/json; charset=utf-8",
                    data: "{'strMonth': '" + $("#<%=ddlAccntTitleMonthGM.ClientID%>").val() + "','strYear' : '" + $("#<%=ddlAccntTitleYearGM.ClientID%>").val() + "'}",
                    dataType: "json",
                    success: function (r) {
                        var labels = r.d[0];
                        var series1 = r.d[1];
                        $("#dvChartGM").html("");
                        var canvas = document.createElement('canvas');
                        $("#dvChartGM")[0].appendChild(canvas);
 
                        var ctx = canvas.getContext('2d');
                        ctx.canvas.height = 300;
                        ctx.canvas.width = 470;
                        var lineChart = new Chart(ctx,
                            {
                                type: 'pie',
                                data: {
                                    labels: r.d[0],
                                    datasets: [
                                        {
                                            backgroundColor: ["rgba(0, 123, 255, 1)", "orange"],
                                            data: series1
                                        }
                                    ]
                                }
                            }
                        );
                    },
                    failure: function (response) {
                        alert('There was an error.');
                    }
                });
            }
        });
    </script>
    <style>
        .chart {

        }
    </style>
    <asp:Panel runat="server">
        <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertWelcome" runat="server">
            Welcome!
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <asp:Label ID="lblUserCompany" Text="" runat="server" />
        <div class="row">
            <div id="APV" class="md-auto p-3 border ml-3 mt-3">
                <h5>APV</h5>
                <div class="input-group">
                    <asp:DropDownList ID="ddlAccntTitleAPV" runat="server" class="form-control"></asp:DropDownList>
                    <div class="input-group-append">
                        <asp:DropDownList ID="ddlAccntTitleYearAPV" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div id="dvChartAPV">
                </div>
            </div>
            <div id="GM" class="md-auto p-3 border ml-3 mt-3">
                <h5>Gross Margin</h5>
                <div class="input-group">
                    <asp:DropDownList ID="ddlAccntTitleMonthGM" runat="server" class="form-control"></asp:DropDownList>
                    <div class="input-group-append">
                        <asp:DropDownList ID="ddlAccntTitleYearGM" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div id="dvChartGM">
                </div>
            </div>
            <div id="Sales" class="md-auto p-3 border ml-3 mt-3">
                <h5>Sales</h5>
                <div class="input-group">
                    <asp:DropDownList ID="ddlAccntTitleSales" runat="server" class="form-control"></asp:DropDownList>
                    <div class="input-group-append">
                        <asp:DropDownList ID="ddlAccntTitleYearSales" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>
                <br />
                <div id="dvChartSales">
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
