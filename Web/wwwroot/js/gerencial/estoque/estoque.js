$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnConsultarProduto').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Gerencial/Estoque/Consultar/",
            data: { stringBusca: $('#stringBusca').val() },
            dataType: "html",
            success: function (response) {
                $("#listaProdutos").html(response);
            }
        });
    });
});