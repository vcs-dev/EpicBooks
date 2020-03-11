$(document).ready(function () {
    $('#status').on('change', function () {
        alert('asodao');
        $('#motivoMudancaStatus').removeClass('d-none');
        if ($('#status').val() === "1")
            $('#categoriaAtivacao').removeClass('d-none');
        else if ($('#status').val() === "0")
            $('#categoriaInativacao').removeClass('d-none');
    });
});