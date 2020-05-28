$(document).ready(function () {
    $('#linkSair').on('click', function () {
        $('#textoModal').text('Deseja realmente sair?');
        $('#modalConfirmacao').modal('show');
    });
    $('#btnSimModal').on('click', function () {
        location.href = '/Loja/Conta/Logout/'
    });
});