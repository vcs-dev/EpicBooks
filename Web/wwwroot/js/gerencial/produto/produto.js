$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#btnOkModal').on('click', function () {
        if($('#textoModal').text() === 'Produto alterado com sucesso!')
            location.href = '/Gerencial/Produtos/';
    });

    if ($('#status').val() === "1") {
        $('#divCatAtivacao').removeClass('d-none');
        $('#divCatInativacao').addClass('d-none');
        $('#categoriaInativacao').val('');
    }
    else if ($('#status').val() === "0") {
        $('#divCatInativacao').removeClass('d-none');
        $('#divCatAtivacao').addClass('d-none');
        $('#categoriaAtivacao').val('');
    }

    $('#status').on('change', function () {

        if ($('#status').val() === "1") {
            $('#divCatAtivacao').removeClass('d-none');
            $('#divCatInativacao').addClass('d-none');
            $('#categoriaInativacao').val('');
        }
        else if ($('#status').val() === "0") {
            $('#divCatInativacao').removeClass('d-none');
            $('#divCatAtivacao').addClass('d-none');
            $('#categoriaAtivacao').val('');
        }
    });

    $('#btnConsultarProduto').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Gerencial/Produtos/Consultar/",
            data: { stringBusca: $('#stringBusca').val() },
            dataType: "html",
            success: function (response) {
                $("#listaProdutos").html(response);
            }
        });
    });
});