$(document).ready(function () {
    var dataGraficoLinhas;
    var dataGraficoTorta;
    var divCanvas = '<div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">' +
        '<h1 class="h2" id="maisVendidosTitulo">Mais vendidos por título</h1>' +
        '</div>' +
        '<canvas class="my-4 w-100" id="myChart" width="900" height="380"></canvas>' +
        '<div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">' +
        '<h1 class="h2" id="maisVendidosCategoria">Mais vendidos por categoria</h1>' +
        '</div>' +
        '<canvas class="my-4 w-100" id="myChart2" width="900" height="380"></canvas>';

    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '') {
        $('#modalMensagem').modal('show');
    }

    $('#btnGerarGraficos').on('click', function () {
        // $.ajax({
        //     type: "get",
        //     url: "/Gerencial/Dashboard/GerarGraficoLinhas/",
        //     data: { dataInicial: $('#dataInicial').val(), dataFinal: $('#dataFinal').val() },
        //     dataType: "json",
        //     success: function (response) {
        //         dataGraficoLinhas = JSON.parse(response);
        //         GerarGraficoLinhas(dataGraficoLinhas);
        // $('#maisVendidosTitulo').text('');
        // $('#maisVendidosTitulo').text('Mais vendidos por título ' + $('#maisVendidosTitulo').text() + ' - ' + $('#dataInicial').val() + ' à ' + $('#dataFinal').val());
        //     }
        // });
        $.ajax({
            type: "get",
            url: "/Gerencial/Dashboard/GerarGraficoTorta/",
            data: { dataInicial: $('#dataInicial').val(), dataFinal: $('#dataFinal').val() },
            dataType: "json",
            success: function (response) {
                dataGraficoTorta = JSON.parse(response);
                GerarGraficoTorta(dataGraficoTorta);
                if (dataGraficoTorta.datasets.length > 0) {
                    $('#maisVendidosCategoria').text('');
                    $('#maisVendidosCategoria').text('Mais vendidos por categoria ' + $('#maisVendidosCategoria').text() + ' - ' + $('#dataInicial').val() + ' à ' + $('#dataFinal').val());
                }
                else {
                    $('#maisVendidosCategoria').text('');
                    $('#maisVendidosCategoria').text('Mais vendidos por categoria ' + $('#maisVendidosCategoria').text() + ' - ' +
                        $('#dataInicial').val() + ' à ' + $('#dataFinal').val() + ' - Sem dados no período');
                }
            }
        });
        $('#partialGraficos').removeClass('d-none');
    });

    /* globals Chart:false, feather:false */
    // window.chartColors = {
    //     red: 'rgb(255, 99, 132)',
    //     orange: 'rgb(255, 159, 64)',
    //     yellow: 'rgb(255, 205, 86)',
    //     green: 'rgb(75, 192, 192)',
    //     blue: 'rgb(54, 162, 235)',
    //     purple: 'rgb(153, 102, 255)',
    //     grey: 'rgb(201, 203, 207)'
    // };

    function GerarGraficoLinhas(data) {
        // Graphs
        var ctx = $('#myChart');
        // eslint-disable-next-line no-unused-vars
        var myChart = new Chart(ctx, {
            type: 'line',
            data: data,
            options: {
                scales: {
                    xAxes: [{
                        display: true,
                        scaleLabel: {
                            display: true,
                            labelString: 'Mês'
                        },
                        beginAtZero: false
                    }],
                    yAxes: [{
                        display: true,
                        scaleLabel: {
                            display: true,
                            labelString: 'Quantidade'
                        }
                    }]
                },
                legend: {
                    display: true
                },
                hover: {
                    mode: 'nearest',
                    intersect: true
                },
            }
        })
    }

    // var randomScalingFactor = function () {
    //     return Math.round(Math.random() * 100);
    // };

    function ResetCanvas() {
        // $('#myChart').remove();
        // $('#myChart2').remove();
        $('#partialGraficos').empty();
        $('#partialGraficos').html(divCanvas);
    }

    function GerarGraficoTorta(data) {
        var config = {
            type: 'pie',
            data: data,
            options: {
                responsive: true
            }
        };
        ResetCanvas();
        var ctx = $('#myChart2');
        var myPieChart = new Chart(ctx, config);
    }

});