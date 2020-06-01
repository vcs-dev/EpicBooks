﻿$(document).ready(function () {
    if ($('#textoModal').text() !== undefined && $('#textoModal').text().trim() !== '')
        $('#modalMensagem').modal('show');

    $('.mensagem').on('click', function () {
        var indice = $('.mensagem').index(this);
        $('.modal-title').text($('.tituloNotf').eq(indice).text());
        $('#textoModal').text($('.inputDescricao').eq(indice).val() + ' Data: ' + $('.dataNotf').eq(indice).text());
        $('#modalMensagem').modal('show');
        $('#btnOkModal').val($('.inputId').eq(indice).val());
    });

    $('#btnOkModal').on('click', function () {
        location.href = '/Loja/Minhaconta/OcultarNotificacao?' + 'notificacaoId=' + $(this).val();
    });
});