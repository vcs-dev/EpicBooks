$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#selectCartao').on('change', function () {
        $.ajax({
            type: "get",
            url: "/Loja/MinhaConta/AlterarCartaoPreferencial/",
            data: { CartaoId: $('#selectCartao option:selected').val() },
            dataType: "json",
            success: function (response) {
                var text = JSON.parse(response);
                $('#selectCartao option:disabled').removeAttr('disabled');
                $('#selectCartao option:selected').prop('disabled', 'true');
                if (text.Mensagem !== undefined && text.Mensagem !== '') {
                    $('#textoModal').text(text.Mensagem);
                    $('#modalMensagem').modal('show');
                }
            }
        });
    });
});