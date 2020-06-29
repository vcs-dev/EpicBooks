$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnInativarConta').on('click', function () {
        var indice = $('.mensagem').index(this);
        
        $('#textoModalConfirmacao').text('Deseja realmente inativar a conta?');
        $('#modalConfirmacao').modal('show');
    });

    $('#btnOkModal').on('click', function () {
        location.href = '/Loja/Conta/Logout/';
    });
    $('#btnSimModal').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Minhaconta/Inativar/",
            dataType: "json",
            success: function (response) {
                var text = JSON.parse(response);
                if (text.Mensagem !== undefined && text.Mensagem !== '') {
                    $('#textoModal').text(text.Mensagem);
                    $('#modalMensagem').modal('show');
                }
            }
        });
    });
});