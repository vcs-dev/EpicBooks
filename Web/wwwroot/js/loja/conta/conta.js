$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');
    ExibeEsconde();

    $('#endEntregaECobranca').on('click', function () {
        ExibeEsconde();
    });
});

function ExibeEsconde() {
    if ($('#endEntregaECobranca').prop('checked') == true) {
        $('#divEnderecoCobranca input').val('');
        $('.inputSelect option').prop('selected', false);
        $('#divEnderecoCobranca').addClass('d-none');
    }
    else {
        $('#divEnderecoCobranca').removeClass('d-none');
    }
}