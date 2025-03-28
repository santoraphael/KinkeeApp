$(document).ready(function () {

    $.get('../../Dating/PegarNovasNotificacoes', function(data) {

        
        if (data !== "") {
            if (data > 0) {
                $('.js-count-notify').html(data);
                $('.js-count-notify').css("display", "block");
            }
        }
    });

    $.get('../../Dating/PegarQuantidadeNovasMensagens', function (data) {


        if (data !== "") {
            if (data > 0) {
                $('.js-count-messages').html(data);
                $('.js-count-messages').css("display", "block");
            }
        }
    });

    $.get('../../Dating/PegarQuantidadeNovasSolicitacoes', function (data) {

        if (data !== "") {
            if (data > 0) {

                $('#js-count-friends-request').html(data);
                $('#js-count-friends-request').css("display", "block");
            }
        }
    });

    $(document).on('click', '.js-btn-accept-friend', function () {

        var obj = $(this).parent('.actions-wrapper');

        $(obj).children('.js-btn-accept-friend').css('display', 'none');
        $(obj).children('.js-btn-reject-friend').css('display', 'none');
        $(obj).children('.loadingIndicator').css('display', 'block');

        var uid = $(this).attr('uid');

        $.ajax({
            url: '/dating/ResponseFriendRequest',
            data: { Responde: '2', Uid: uid },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
    
                if (data !== "") {
                    $(obj).html("");
                    $(obj).html("<p>Vocês agora são amigos.</p>");
                }
            }
        });
    });

    $(document).on('click', '.js-btn-reject-friend', function () {

        var obj = $(this).parent('.actions-wrapper');

        $(obj).children('.js-btn-accept-friend').css('display', 'none');
        $(obj).children('.js-btn-reject-friend').css('display', 'none');
        $(obj).children('.loadingIndicator').css('display', 'block');

        var uid = $(this).attr('uid');

        $.ajax({
            url: '/dating/ResponseFriendRequest',
            data: { Responde: '3', Uid: uid },
            dataType: 'json',
            type: 'POST',
            success: function (data) {

                if (data !== "") {
                    $(obj).html("");
                    $(obj).html("<p>Solicitação excluida.</p>");
                }
            }
        });
    });


    $('#btnReject').on('click', function (e) {
        alert("btnReject!");
        e.preventDefault();
    });


    $('.dropdown-toggle').click(function (e) {

        $('.dropdown').toggleClass('open');


        e.preventDefault();
    });


    $('main').click(function () {
        $('.js-solicitacoes').removeClass('open');
        $('.js-notificacao').removeClass('open');
        $('.js-mensagens').removeClass('open');

        $('.js-load-list').css("display", "block");


        $('.profile').removeClass('open');
    });




    $('#dropdown-toggle').click(function () {
        $('.profile').toggleClass('open');

    });



    $('.js-koin-click').click(function () {
        $('.js-moeda-menu').toggleClass('js-moeda-menu-open');
        $('.navbar-actions').toggleClass('open');
    });


    $('main').click(function () {
        $('.js-moeda-menu').removeClass('js-moeda-menu-open');
        $('.navbar-actions').removeClass('open');
    });


    $('#asidebutton').click(function () {
        $('.promotion-scroll-block').toggleClass('sidebar-open');

    });

    $('main').click(function () {
        $('.promotion-scroll-block').removeClass('sidebar-open');
    });

    $(document).on("click", ".list-item", function (e) {
        $('.chat-list.clearfix').html("");

        var ID = $(this).attr('id');
        var image = $('.js-thumb-header').attr('src');
        var nameTo = $('.js-name-header').html();

        $(this).find('.badge.unread.ng-binding').css('display', 'none');
        $(this).find('.badge.unread.ng-binding').html('0');


        $.get('../../Dating/MensagensPartial', { inboxID: ID, urlImage: image, NomeTo: nameTo }, function (data) {

            $('.chat-list.clearfix').append(data);

            $('ul').scrollTop($(".clearfix").last().offset().top);
        });
        e.preventDefault();
    });


    $(document).on("click", "#btn-mensagem", function (e) {

        var nameUser = $(this).attr('user-modal-message');

        $.ajax({
            url: '../../Dating/MensagemModal',
            data: { NameUser: nameUser },
            
            type: 'POST',
            success: function (data) {
                $('#mensagemModal').append(data);
            }
        });


        //$.get('../../Dating/MensagemModal', function (data) {

        //    $('#mensagemModal').append(data);
        //});
        e.preventDefault();
    });

    $('#btnSearch').unbind("click").click(function (e) {
        var result = $('#searchResult').val();
        var searchstring = $('#search');
        searchstring.focus();
        if ($('#searchResult').val() !== "" || searchstring.val() !== "") {
            
            if ($('#searchResult').val().split('_')[0] === "1") {
                $.ajax({
                    url: $('#helperFields').data('urlactioncommitsearch'),
                    data: { User: $('#searchResult').val(), PartialName: searchstring.val(), tipoPessoa: $('#genero').val() },
                    dataType: 'json',
                    type: 'GET',
                    success: function (data) {
                        if (data !== "") {
                            window.location.href = '/Dating/Perfil/' + data;
                        }
                        return false;
                    }
                });
            }
            else {
                $.get("/Dating/ResultadoBusca", { busca: result }, function (data) {
                    $("#mainContent").empty();
                    $('#mainContent').html(data);
                });
                $('#searchResult').val(result);
                $("#container").html("");
                
                GetDataSearchZeraIndex();
            }
        }
        e.preventDefault();
    });

    $('#btnSearchMobile').unbind("click").click(function (e) {
        var result = $('#searchResult').val();
        var searchstring = $('#searchMobile');
        searchstring.focus();
        if ($('#searchResult').val() !== "" || searchstring.val() !== "") {

            if ($('#searchResult').val().split('_')[0] === "1") {
                $.ajax({
                    url: $('#helperFields').data('urlactioncommitsearch'),
                    data: { User: $('#searchResult').val(), PartialName: searchstring.val(), tipoPessoa: $('#genero').val() },
                    dataType: 'json',
                    type: 'GET',
                    success: function (data) {
                        if (data !== "") {
                            window.location.href = '/Dating/Perfil/' + data;
                        }
                        return false;
                    }
                });
            }
            else {
                $.get("/Dating/ResultadoBusca", { busca: result }, function (data) {
                    $("#mainContent").empty();
                    $('#mainContent').html(data);
                });
                $('#searchResult').val(result);
                $("#container").html("");

                GetDataSearchZeraIndex();
            }
        }
        e.preventDefault();
    });

    $('#searchMobile').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $('#helperFields').data('urlactionautocomplete'),
                data: { term: request.term, tipoPessoa: $('#genero').val() },
                dataType: 'json',
                type: 'GET',
                appendTo: "#SearchResult",
                success: function(data) {
                    response($.map(data, function(item) {
                        return {
                            label: item.Nome,
                            value: item.Tipo + "_" + item.Codigo,
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#searchMobile').val(ui.item.label);
            $('#searchResult').val(ui.item.value);
            $("#btnSearchMobile").click();
            return false;
        },
        minLength: 3,
        appendTo: "#navbar-search-form-mobile",
        
    });

    $('#search').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $('#helperFields').data('urlactionautocomplete'),
                data: { term: request.term, tipoPessoa: $('#genero').val() },
                dataType: 'json',
                type: 'GET',
                appendTo: "#SearchResult",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Nome,
                            value: item.Tipo + "_" + item.Codigo,
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#search').val(ui.item.label);
            $('#searchResult').val(ui.item.value);
            $("#btnSearch").click();
            return false;
        },
        minLength: 3,
        appendTo: "#navbar-search-form"
    });

    $('#search').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code === 13) {
            $("#btnSearch").click();
        }
    });

    $('#searchMobile').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code === 13) {
            $("#btnSearchMobile").click();
        }
    });






    $('#localizacao').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: $('#helperFieldsLocation').data('urlactionautocomplete'),
                data: { term: request.term, tipoPessoa: $('#genero').val() },
                dataType: 'json',
                type: 'GET',
                appendTo: "#localizacao",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.Nome,
                            value: item.Tipo + "_" + item.Codigo,
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $('#localizacao').val(ui.item.label);
            $('#cidade').val(ui.item.label.split(",")[0]);
            $('#estado').val(ui.item.label.split(",")[1]);
            //$("#btnSearch").click();
            return false;
        },
        minLength: 3,
        appendTo: ".form-group.row.localizacao"
    });

    $('#localizacao').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code === 13) {
            //$("#btnSearch").click();
        }
    });
    

});






