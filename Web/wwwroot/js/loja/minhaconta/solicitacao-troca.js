$(document).ready(function () {
    $('.btnSolicitarTroca').on('click', function () {
        var indice = $('.btnSolicitarTroca').index(this);
        $('#indiceItem').val(indice);
        $('#modalTrocaQtde').modal('show');
    });

    $('#btnEnviarSolicitacaoTroca').on('click', function () {
        var indice = $('#indiceItem').val();
        var itemId = $('#tbodyIemPedido').find('.itemId').eq(indice).text();
        var qtde = $('#inputQtde').val();
        var pedidoId = $('#idPedido').val();

        $('#modalTrocaQtde').modal('hide');

        $.ajax({
            type: "get",
            url: "/Loja/MinhaConta/EnviarSolicitacaoTroca/",
            data: {itemId: itemId, qtde: qtde, pedidoId: pedidoId},
            dataType: "json",
            success: function (response) {
                var text = JSON.parse(response);
                if(text.Mensagem !== undefined && text.Mensagem !== ''){
                    $('#textoModal').text(text.Mensagem);
                    $('#modalTrocaMsg').modal('show');
                    $('#btnOkModal').prop('href', '/Loja/MinhaConta/MeusPedidos/');
                }
            }
        });
    });
});