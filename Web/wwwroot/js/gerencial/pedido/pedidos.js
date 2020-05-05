$(document).ready(function () {
    if($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
    $('#modalMensagem').modal('show');

    $('.btnAbreModalPedido').on('click', function () {
        var indice = $('.btnAbreModalPedido').index(this);
        $('#indiceItem').val(indice);
        $('#modalStatusPedido').modal('show');
    });

    $('#btnOkModal').on('click', function() {
        $('#textoModal').text('');
        location.href = '/Gerencial/Dashboard/'; 
    });

    $('#btnAlterarStatus').on('click', function () {
        var indice = $('#indiceItem').val();
        var pedidoId = $('#tbodyPedido').find('.pedidoId').eq(indice).text();
        var status = $('#selectStatus option:selected').val();

        $('#modalStatusPedido').modal('hide');

        $.ajax({
            type: "get",
            url: "/Gerencial/Pedidos/AlterarStatus/",
            data: { pedidoId: pedidoId, status: status},
            dataType: "json",
            success: function (response) {
                var text = JSON.parse(response);
                if(text.Mensagem !== undefined && text.Mensagem !== ''){
                    $('#textoModal').text(text.Mensagem);
                    $('#modalMensagem').modal('show');
                }
            }
        });
    });
});