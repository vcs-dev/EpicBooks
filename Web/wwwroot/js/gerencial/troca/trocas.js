$(document).ready(function () {
    if($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
    $('#modalMensagem').modal('show');

    $('.btnAbreModalTroca').on('click', function () {
        var indice = $('.btnAbreModalTroca').index(this);
        $('#indiceItem').val(indice);
        $('#modalTroca').modal('show');
    });

    $('#btnOkModal').on('click', function() {
        if( $('#textoModal').text() !== 'Não existem solicitações de troca pendentes.')
            location.href = '/Gerencial/Trocas/'; 
        $('#textoModal').text('');
    });

    $('.btnAutorizarTroca').on('click', function () {
        var indice = $(this).parents('tr').index();
        var pedidoId = $('#tbodyItemTroca').find('.pedidoId').eq(indice).text();
        var itemId = $('#tbodyItemTroca').find('.itemId').eq(indice).text();
        var usuarioId = $('#tbodyItemTroca').find('.usuarioId').eq(indice).text();

        $.ajax({
            type: "get",
            url: "/Gerencial/Trocas/AutorizarTroca/",
            data: {pedidoId: pedidoId, itemId: itemId, usuarioId: usuarioId},
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

    $('.btnNegarTroca').on('click', function () {
        var indice = $(this).parents('tr').index();
        var pedidoId = $('#tbodyItemTroca').find('.pedidoId').eq(indice).text();
        var itemId = $('#tbodyItemTroca').find('.itemId').eq(indice).text();

        $.ajax({
            type: "get",
            url: "/Gerencial/Trocas/NegarTroca/",
            data: {pedidoId: pedidoId, itemId: itemId},
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