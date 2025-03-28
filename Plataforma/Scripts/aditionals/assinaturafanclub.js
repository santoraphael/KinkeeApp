$(document).on('click', '.assinaplanofanclub', function () {
    var newhash = $(this).data('data-hash');
    $('#btnpagseguro').click();
    $('#tabelapagamentos').removeClass('d-none hidden');
    $('#hashplan').val(newhash);
    hash = newhash;
    $('#listaplanosbannerbloqueio').slideToggle();
});