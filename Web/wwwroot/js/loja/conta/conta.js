$(document).ready(function () {
    $('#endEntregaECobranca').on('click', function () {
        if ($('#endEntregaECobranca').prop('checked') == true) {
            $('#divEnderecoCobranca input').val('');
            $('.inputSelect option').prop('selected', false);
            $('#divEnderecoCobranca').addClass('d-none');
        }
        else{
            $('#divEnderecoCobranca').removeClass('d-none');
        }
    });
});