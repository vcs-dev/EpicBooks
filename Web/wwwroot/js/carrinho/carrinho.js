$(document).ready(function () {
    if ($('#chkDoisCartoes').prop('checked')) {
        $('#pagarDoisCartoes').removeClass('d-none');
        $('.valorCartao').removeClass('d-none');
    }

    $('#chkDoisCartoes').on('change', function () {
        if ($('#chkDoisCartoes').prop('checked')) {
            $('#pagarDoisCartoes').removeClass('d-none');
            $('.valorCartao').removeClass('d-none');
        }
        else{
            $('.valorCartao input[type="text"]').val('');
            $('#pagarDoisCartoes input[type="text"]').val('');
            $('#pagarDoisCartoes option').prop('selected', false);
            $('#pagarDoisCartoes').addClass('d-none');
            $('.valorCartao').addClass('d-none');
        }
    });

    $('#itensPedido').on('click', '.btnAumentarQtde', function () {
        //alert($(this).val());
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/AumentarQtde/",
            data: {id : $(this).val()},
            dataType: "html",
            success: function (response) {
                $("#itensPedido").html(response);
                $(document).ready();
            }
        });
    });

    $('#itensPedido').on('click', '.btnDiminuirQtde', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/DiminuirQtde/",
            data: {id : $(this).val()},
            dataType: "html",
            success: function (response) {
                $("#itensPedido").html(response);
            }
        });
    });

    $('#itensPedido').on('click', '.btnRemoverItem', function () {
        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/RemoverItem/",
            data: {id : $(this).val()},
            dataType: "html",
            success: function (response) {
                $("#itensPedido").html(response);
            }
        });

        $.ajax({
            type: "get",
            url: "/Loja/Carrinho/CalcularFrete/",
            data: {cep : $('#inputCep').val()},
            dataType: "html",
            success: function (response) {
                $("#valorFrete").html('R$ ' + response);
            }
        });
    });
});