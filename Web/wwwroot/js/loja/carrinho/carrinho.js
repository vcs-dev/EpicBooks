$(document).ready(function () {
    if ($('#cupomPromo').length > 0) {
        $('#codCupomPromo').prop('readonly', true);
    }

    if ($('.cupomTroca').length > 0) {
        $('#selectCupomTroca option').each(function () {
            var elem = $('.cupomTroca').find('button[value="' + $(this).val() + '"]');
            if (elem.length > 0)
                $(this).prop('disabled', true);
        });
    }

    if ($('#chkDoisCartoes').prop('checked')) {
        $('#pagarDoisCartoes').removeClass('d-none');
        $('.valorCartao').removeClass('d-none');
    }

    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('#chkDoisCartoes').on('change', function () {
        if ($('#chkDoisCartoes').prop('checked')) {
            $('#pagarDoisCartoes').removeClass('d-none');
            $('.valorCartao').removeClass('d-none');
        }
        else {
            $('.valorCartao input[type="text"]').val('');
            $('#pagarDoisCartoes input[type="text"]').val('');
            $('#pagarDoisCartoes option').prop('selected', false);
            $('#pagarDoisCartoes').addClass('d-none');
            $('.valorCartao').addClass('d-none');
        }
    });

    $('#itensPedido').on('click', '.btnAumentarQtde', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/AumentarQtde/",
            data: { id: $(this).val() },
            dataType: "html",
            success: function (response) {
                $("#itensPedido").html(response);
                CalcularFrete($('#selectEndereco').val());
                //AtualizarResumo();
            }
        });
    });

    $('#itensPedido').on('click', '.btnDiminuirQtde', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/DiminuirQtde/",
            data: { id: $(this).val() },
            dataType: "html",
            success: function (response) {
                $("#itensPedido").html(response);
                CalcularFrete($('#selectEndereco').val());
                //AtualizarResumo();
            }
        });
    });

    $('#itensPedido').on('click', '.btnRemoverItem', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/RemoverItem/",
            data: { id: $(this).val() },
            dataType: "html",
            success: function (response) {
                alert(response);
                $("#itensPedido").html(response);
                CalcularFrete($('#selectEndereco').val());
                //AtualizarResumo();
            },
            error: function () {
                location.href = "/Loja/Carrinho/Index/";
            }
        });
    });

    $('#btnAddCupomPromo').on('click', function () {
        if ($('#codCupomPromo').val() !== undefined && $('#codCupomPromo').val().trim() !== '') {
            $.ajax({
                type: "get",
                url: "/Loja/Carrinho/AdicionarCupomPromo/",
                data: { codCupom: $('#codCupomPromo').val() },
                dataType: "html",
                success: function (response) {
                    $('#resumo').html(response);
                    $('#codCupomPromo').val('');
                    $('#codCupomPromo').prop('readonly', true);
                    $('#labelCupomPromoErro').empty();
                },
                error: function () {
                    $('#labelCupomPromoErro').text('Cupom inválido');
                }
            });
        }
    });

    $('#btnAddCupomTroca').on('click', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/AdicionarCupomTroca/",
            data: { codCupom: $('#selectCupomTroca option:selected').val() },
            dataType: "html",
            success: function (response) {
                $('#resumo').html(response);
                $('#selectCupomTroca option:selected').prop('disabled', true);
            }
        });
    });

    // $('#btnCalcFrete').on('click', function () {
    //     if ($('#inputCep').val() !== undefined && $('#inputCep').val().trim() !== '') {
    //         CalcularFrete();
    //     }
    // });

    $('#selectEndereco').on('change', function () {
        CalcularFrete($(this).val());
    });

    $('#resumo').on('click', '#btnRemoverCupomPromo', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/RemoverCupomPromo/",
            dataType: "html",
            success: function (response) {
                $('#resumo').html(response);
                $('#codCupomPromo').removeAttr('readonly');
            }
        });
    });

    $('#resumo').on('click', '.btnRemoverCupomTroca', function () {
        var elem = $(this);
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/RemoverCupomTroca/",
            data: { codCupom: elem.val() },
            dataType: "html",
            success: function (response) {
                $('#resumo').html(response);
                $('#selectCupomTroca option[value="' + elem.val() + '"').removeAttr('disabled');
            }
        });
    });
});

function CalcularFrete(id) {
    if (parseInt(id) > 0) {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/CalcularFrete/",
            data: { id: id },
            dataType: "html",
            success: function (response) {
                $('#resumo').html(response);
            }
        });
    }
    else
        AtualizarResumo();
}

function AtualizarResumo() {
    $.ajax({
        type: "get",
        url: "/Loja/Carrinho/AtualizarResumo/",
        dataType: "html",
        success: function (response) {
            $('#resumo').html(response);
        }
    });
}
// function CalcularFrete(cep) {
//     if (cep === undefined) {
//         $.ajax({
//             type: "get",
//             url: "/Loja/Carrinho/CalcularFrete/",
//             data: { cep: $('#inputCep').val() },
//             dataType: "json",
//             success: function (data) {
//                 var retorno = JSON.parse(data);
//                 $("#valorFrete").html('R$ ' + retorno.valor);
//             }
//         });
//     }
//     else{
//         $.ajax({
//             type: "get",
//             url: "/Loja/Carrinho/CalcularFrete/",
//             data: { cep: cep },
//             dataType: "json",
//             success: function (data) {
//                 var retorno = JSON.parse(data);
//                 $("#valorFrete").html('R$ ' + retorno.valor);
//             }
//         });
//     }
//}