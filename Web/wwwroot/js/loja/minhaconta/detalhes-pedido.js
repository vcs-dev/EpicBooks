$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnOkModal').on('click', function () {
        location.href = '/Loja/MinhaConta/MeusPedidos/';
    });

    $('#btnCancelarPedido').on('click', function () {
        $('#textoModal').text('Tem certeza que deseja cancelar o pedido ' + $('#idPedido').val() + '?');
        $('#modalConfirmacao').modal('show');
    });

    $('#btnSimModal').on('click', function () {
        location.href = '/Loja/MinhaConta/CancelarPedido?pedidoId=' + $('#idPedido').val();
    });
});