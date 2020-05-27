$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnOkModal').on('click', function () {
        if ($('#textoModal').text() === 'Cadastro efetuado com sucesso!') {
            location.href = '/Loja/Home/';
        }
    });

    exibirEsconder();

    $('#endEntregaECobranca').on('click', function () {
        exibirEsconder();
    });
});

function exibirEsconder() {
    if ($('#endEntregaECobranca').prop('checked') == true) {
        $('#divEnderecoCobranca input').val('');
        $('.inputSelect option').prop('selected', false);
        $('#divEnderecoCobranca').addClass('d-none');
    }
    else {
        $('#divEnderecoCobranca').removeClass('d-none');
    }
}