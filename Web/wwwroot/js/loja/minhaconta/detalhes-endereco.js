$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnOkModal').on('click', function () {
        if ($('#textoModal').text() === 'Endereço excluído com sucesso!' || $('#textoModal').text() === 'Endereço cadastrado com sucesso!' ||
            $('#textoModal').text() === 'Alterações salvas com sucesso!')
            location.href = '/Loja/MinhaConta/Enderecos/';
    });
     
    $('#btnExcluirEndereco').on('click', function () {
        $('#textoModalConfirmacao').text('Deseja realmente excluir esse endereço?');
        $('#modalConfirmacao').modal('show');
    });

    $('#btnSimModal').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Loja/MinhaConta/ExcluirEndereco/",
            data: { EnderecoId: $('#btnExcluirEndereco').val() },
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