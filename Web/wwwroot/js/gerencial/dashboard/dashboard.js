$(document).ready(function () {
    var dataGraficoLinhas;
    var dataGraficoTorta;

    feather.replace();

    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '') {
        $('#modalMensagem').modal('show');
    }

    $('#btnGerarGraficos').on('click', function () {
        ResetCanvas();
        $.ajax({
            type: "get",
            url: "/Gerencial/Dashboard/GerarGraficoLinhas/",
            data: { dataInicial: $('#dataInicial').val(), dataFinal: $('#dataFinal').val() },
            dataType: "json",
            success: function (response) {
                dataGraficoLinhas = JSON.parse(response);
                GerarGraficoLinhas(dataGraficoLinhas);
                if (dataGraficoLinhas.datasets.length > 0) {
                    $('#faturamento').text('');
                    $('#faturamento').text('Faturamento ' + $('#faturamento').text() + ' - ' +
                        $('#dataInicial').val() + ' à ' + $('#dataFinal').val());
                    $('#partialGraficos').removeClass('d-none');
                }
                else {
                    $('#faturamento').text('');
                    $('#faturamento').text('Faturamento ' + $('#faturamento').text() + ' - ' +
                        $('#dataInicial').val() + ' à ' + $('#dataFinal').val() + ' - Sem dados no período');
                    $('#textoModal').text(dataGraficoLinhas.mensagemErro);
                    $('#modalMensagem').modal('show');
                    $('#partialGraficos').addClass('d-none');
                }
            }
        });
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
                    $('#maisVendidosCategoria').text('Mais vendidos por categoria ' + $('#maisVendidosCategoria').text() + ' - ' +
                        $('#dataInicial').val() + ' à ' + $('#dataFinal').val());
                    $('#partialGraficos').removeClass('d-none');
                }
                else {
                    $('#maisVendidosCategoria').text('');
                    $('#maisVendidosCategoria').text('Mais vendidos por categoria ' + $('#maisVendidosCategoria').text() + ' - ' +
                        $('#dataInicial').val() + ' à ' + $('#dataFinal').val() + ' - Sem dados no período');
                    $('#textoModal').text(dataGraficoTorta.mensagemErro);
                    $('#modalMensagem').modal('show');
                    $('#partialGraficos').addClass('d-none');
                }
            }
        });
    });
});

var divCanvas = '<div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">' +
    '<h1 class="h2" id="faturamento">Faturamento</h1>' +
    '</div>' +
    '<canvas class="my-4 w-100" id="myChart" width="900" height="380"></canvas>' +
    '<div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">' +
    '<h1 class="h2" id="maisVendidosCategoria">Mais vendidos por categoria</h1>' +
    '</div>' +
    '<canvas class="my-4 w-100" id="myChart2" width="900" height="380"></canvas>';

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
                        labelString: 'Período'
                    },
                    beginAtZero: false
                }],
                yAxes: [{
                    display: true,
                    scaleLabel: {
                        display: true,
                        labelString: 'Valor R$'
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

function ResetCanvas() {
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
    var ctx = $('#myChart2');
    var myPieChart = new Chart(ctx, config);
}