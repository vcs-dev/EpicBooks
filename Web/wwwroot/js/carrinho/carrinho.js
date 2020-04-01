$(document).ready(function () {
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
    });
});