$(document).ready(function () {
    if($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
    $('#modalMensagem').modal('show');
});