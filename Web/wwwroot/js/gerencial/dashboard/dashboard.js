$(document).ready(function () {
    var dataGraficoLinhas;
    var dataGraficoTorta;
    var myChart;
    
    var dataLineChart = {
        labels: [],
        dataset: [],
    };

    var dsLineChart = {
        label: '',
        fill: false,
        data: [],
        lineTension: 0,
        backgroundColor: '',
        borderColor: '',
        borderWidth: 0,
        pointBackgroundColor: '',
    };

    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '') {
        $('#modalMensagem').modal('show');
    }

    $('#btnGerarGraficos').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Gerencial/Dashboard/GerarGraficoLinhas/",
            data: { dataInicial: $('#dataInicial').val(), dataFinal: $('#dataFinal').val() },
            dataType: "json",
            success: function (response) {
                dataGraficoLinhas = JSON.parse(response);
                dataLineChart['labels'] = dataGraficoLinhas['labels'];
                dataLineChart['dataset'] = dataGraficoLinhas['DataSets'][0];
                GerarGraficoLinhas(dataLineChart);
                //AddData(myChart, dataGraficoLinhas);
            }
        });
        $.ajax({
            type: "get",
            url: "/Gerencial/Dashboard/GerarGraficoTorta/",
            data: { dataInicial: $('#dataInicial').val(), dataFinal: $('#dataFinal').val() },
            dataType: "json",
            success: function (response) {
                //dataGraficoTorta = JSON.parse(response);
            }
        });
        $('#partialGraficos').removeClass('d-none');
    });

    /* globals Chart:false, feather:false */
    window.chartColors = {
        red: 'rgb(255, 99, 132)',
        orange: 'rgb(255, 159, 64)',
        yellow: 'rgb(255, 205, 86)',
        green: 'rgb(75, 192, 192)',
        blue: 'rgb(54, 162, 235)',
        purple: 'rgb(153, 102, 255)',
        grey: 'rgb(201, 203, 207)'
    };

    // function AddData(chart){
    //     chart.data.labels = dataLineChart['labels'];
    //     //chart.data.datasets = data['DataSets'][0];
    //     chart.data.datasets.push(dsLineChart);
    //     chart.update();
    // }

    function GerarGraficoLinhas(teste) {
       // 'use strict'

        //feather.replace()

        // Graphs
        var ctx = $('#myChart');
        // eslint-disable-next-line no-unused-vars
        myChart = new Chart(ctx, {
            type: 'line',
            data: teste,
            // data: {
            //     labels: dataGraficoLinhas['Labels'],
            //     datasets: [{
            //         label: dataGraficoLinhas['DataSets'][0]['Label'],
            //         fill: dataGraficoLinhas['DataSets'][0]['Fill'],
            //         data: dataGraficoLinhas['DataSets'][0]['Data'],
            //         lineTension: dataGraficoLinhas['DataSets'][0]['LineTension'],
            //         backgroundColor: dataGraficoLinhas['DataSets'][0]['BackgroundColor'],
            //         borderColor: dataGraficoLinhas['DataSets'][0]['BorderColor'],
            //         borderWidth: dataGraficoLinhas['DataSets'][0]['BorderWidth'],
            //         pointBackgroundColor: dataGraficoLinhas['DataSets'][0]['PointBackgroundColor']
            //     }]
            // },
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

    var randomScalingFactor = function () {
        return Math.round(Math.random() * 100);
    };

    var config = {
        type: 'pie',
        data: {
            datasets: [{
                data: [
                    5467,
                    4543,
                    3546,
                    8765,
                    7654,
                ],
                backgroundColor: [
                    window.chartColors.red,
                    window.chartColors.orange,
                    window.chartColors.yellow,
                    window.chartColors.green,
                    window.chartColors.blue,
                ],
                label: 'Dataset 1'
            }],
            labels: [
                'Terror',
                'Suspense',
                'Desenvolvimento pessoal',
                'Romance',
                'História'
            ]
        },
        options: {
            responsive: true
        }
    };

    var ctx = $('#myChart2');
    window.myPie = new Chart(ctx, config);

});