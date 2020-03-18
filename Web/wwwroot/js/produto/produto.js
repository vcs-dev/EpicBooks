$(document).ready(function () {
    const valStatus = $('#status').val();

    $('#status').on('change', function () {
        if ($('#status').val() !== valStatus) {
            $('#divMudaStatus').removeClass('d-none');
            $('#motivoMudancaStatus').prop('required', 'true');
        }
        else {
            $('#divMudaStatus').addClass('d-none');
            $('#motivoMudancaStatus').val('');
            $('#motivoMudancaStatus').removeAttr('required');
        }
        if ($('#status').val() === "1" && $('#status').val() !== valStatus) {
            $('#divCatAtivacao').removeClass('d-none');
            $('#categoriaAtivacao').prop('required', 'true');
            $('#divCatInativacao').addClass('d-none');
            $('#categoriaInativacao').val('');
            $('#categoriaInativacao').removeAttr('required');
        }
        else if ($('#status').val() === "0" && $('#status').val() !== valStatus) {
            $('#divCatInativacao').removeAttr('required');
            $('#categoriaInativacao').prop('required', 'true');
            $('#divCatAtivacao').addClass('d-none');
            $('#categoriaAtivacao').removeAttr('required');
            $('#categoriaAtivacao').val('');
        }
        else {
            $('#divCatAtivacao').addClass('d-none');
            $('#categoriaAtivacao').val('');
            $('#categoriaAtivacao').removeAttr('required');
            $('#divCatInativacao').addClass('d-none');
            $('#categoriaInativacao').val('');
            $('#categoriaInativacao').removeAttr('required');
        }
    });

    $('#btnConsultarProduto').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Gerencial/Produtos/Consultar/",
            data: {stringBusca : $('#stringBusca').val()},
            dataType: "html",
            success: function (response) {
                $("#listaProdutos").html(response);
            }
        });
    });
});