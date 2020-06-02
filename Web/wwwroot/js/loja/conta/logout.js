$(document).ready(function () {
    $('#linkSair').on('click', function () {
        $('#textoModalConfirmacao').text('Deseja realmente sair?');
        $('#modalConfirmacao').modal('show');
    });
    $('#btnSimModal').on('click', function () {
        if($('#textoModalConfirmacao').text() === 'Deseja realmente sair?')
            location.href = '/Loja/Conta/Logout/'
    });
});