﻿$(document).ready(function () {
    if($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
    $('#modalMensagem').modal('show');

    $('.btnAbreModalTroca').on('click', function () {
        var indice = $('.btnAbreModalTroca').index(this);
        $('#indiceItem').val(indice);
        $('#modalTroca').modal('show');
    });

    $('#btnOkModal').on('click', function() {
        location.reload(); 
    });

    $('#btnConfirmarRebebimento').on('click', function () {
        var indice = $('#indiceItem').val();
        var pedidoId = $('#tbodyItemTroca').find('.pedidoId').eq(indice).text();
        var usuarioId = $('#tbodyItemTroca').find('.usuarioId').eq(indice).text();
        var itemId = $('#tbodyItemTroca').find('.itemId').eq(indice).text();
        var qtde = $('#tbodyItemTroca').find('.qtde').eq(indice).text();
        var voltaParaEstoque;

        if($('#radioSim').prop('checked'))
            voltaParaEstoque = $('#radioSim').val();
        else if($('#radioNao').prop('checked'))
            voltaParaEstoque = $('#radioNao').val();

        $('#modalTroca').modal('hide');

        $.ajax({
            type: "get",
            url: "/Gerencial/Trocas/ConfirmarRecebimentoItem/",
            data: {pedidoId: pedidoId, itemId: itemId, usuarioId: usuarioId, qtde: qtde, voltaParaEstoque: voltaParaEstoque},
            dataType: "json",
            success: function (response) {
                var text = JSON.parse(response);
                if(text.Mensagem !== undefined && text.Mensagem !== ''){
                    $('#textoModal').text(text.Mensagem);
                    $('#modalMensagem').modal('show');
                }
            }
        });
    });
});