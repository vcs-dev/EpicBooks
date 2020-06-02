$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnOkModal').on('click', function () {
        location.href = '/Loja/MinhaConta/MeusPedidos/';
    });

    $('#btnCancelarPedido').on('click', function () {
        $('#textoModalConfirmacao').text('Tem certeza que deseja cancelar o pedido ' + $('#idPedido').val() + '?');
        $('#modalConfirmacao').modal('show');
    });

    $('#btnSimModal').on('click', function () {
        if ($('#textoModalConfirmacao').text() === 'Tem certeza que deseja cancelar o pedido ' + $('#idPedido').val() + '?') {
            $.ajax({
                type: "get",
                url: "/Loja/MinhaConta/CancelarPedido/",
                data: { pedidoId: $('#idPedido').val() },
                dataType: "json",
                success: function (response) {
                    var text = JSON.parse(response);
                    if (text.Mensagem !== undefined && text.Mensagem !== '') {
                        $('#textoModal').text(text.Mensagem);
                        $('#modalMensagem').modal('show');
                    }
                }
            });
        }
    });
});