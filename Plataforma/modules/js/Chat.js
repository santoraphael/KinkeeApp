$(document).ready(function () {

    $('.list-item').on("click",function (e) {

        $('.list-item').removeClass('active');
        $('.no-content').addClass('hide');
        $('.chat-header-login').removeClass('hide');

        $('.text.ng-pristine.ng-valid').removeAttr('disabled');
        $('.sl-btn.btn-default.btn-send').removeAttr('disabled');

        $('.fileupload-btn').removeAttr('disabled');

        $(this).toggleClass('active');
        
        $('.js-name-header').html("");
        
        var img = $(this).children('.avatar').children('.thumb').children('.thumb-image').attr('src');
        var name = $(this).children('.info').children('.user').children('.user-main').children('.name').html();
        var userid = $(this).attr('user-id-report');
        var useridReport = $(this).attr('user-id-report');



        $('.js-name-header').append(name);
        $('.chat-header').attr('user-id', userid);
        $('.js-thumb-header').attr('src', img);

        GetUserConnectedByName(name);
        
        

        $('.profile-thumb').attr('href', '../../Dating/Perfil/' + name.replace(" ",""));
        $('.overlayContentCard').attr('href', '../../Dating/Perfil/' + name.replace(" ", ""));

        $('.text.ng-pristine').focus();
        $('.options-header').removeClass('hide');

        //MOBILE
       
        
        $('.container-fluid.message-header').css('display', 'block');
        $('.message-chat').css('display', 'block');
        $('.message-list-header').css('display', 'none');

        $('.chat-header').css('display', 'block');
        $('.chat-header').css('position', 'relative');
        $('.js-options').css('display', 'none');

    });



    //USAR QUANDO FOR IMPLEMENTAR CARREGAMENTO DE CONVESA VIA URL
    //function SelectConversation()
    //{
    //    var url = "localhost/x/y/page/1/";
    //    var parts = url.split("/");
    //    var last_part = parts[parts.length - 2];
    //}

    $('.back.icon-back-page').click(function (e) {
        $('.container-fluid.message-header').css('display', 'none');
        $('.message-chat').css('display', 'none');
        $('.message-list-header').css('display', 'block');

        $('.chat-header').css('display', 'none');
        $('.chat-header').css('position', 'relative');
        $('.js-options').css('display', 'block');
    });
    


    //$(function () {
    //    $('form').submit(function () {
    //        if ($('.text.ng-pristine.ng-valid').val() === "") {
    //            //alert('Please enter Username and Password.');
    //            return false;
    //        }
    //    });
    //});

    



    var btnSetting = document.getElementById('js-message-settings-btn');
    var messageSettingsBody = document.getElementById('messageSettingsBody');
    $(btnSetting).on("click", function () {

        $(messageSettingsBody).addClass('activate');
        $(messageSettingsBody).css('display','block');
    });


    $(".js-btn-close-panel").on("click",function (e) {
        $(messageSettingsBody).removeClass('activate');
        $(messageSettingsBody).css('display', 'none');
    });

    $('.js-btn-trash-conversation').on("click", function (e) {

        $('#panel-delete-conversation').css('display', 'block');
    });

    $('.js-close-delete-conversation').on("click", function (e) {

        $('#panel-delete-conversation').css('display', 'none');

    });


    $('.js-confirm-delete-conversation').on("click", function (e) {

        var idUsuarioSelecionado = $('.list-item.active').attr('user-id-report');
        var nameTo = $('.js-name-header').html();
        //alert(idUsuarioSelecionado);

        ArquivarConversa(idUsuarioSelecionado);
    });

    function ArquivarConversa(idUsuarioSelecionado) {
        $.ajax({
            url: "/Dating/ArquivarMensagem",
            data: {
                'idUsuarioSelecionado': idUsuarioSelecionado,
            },
            
            type: 'POST',
            complete: function (jqXHR) {
                if (jqXHR.readyState === 4) {
                    location.reload();
                }
            },
            success: function () {
                
            },
            error: function () {
                //Notification();
            }
        });
    }

    ///



    //function SelecionaMensagens(var idInbox) {
    //    $.ajax({
    //        url: "Board/UpDateNameCard",
    //        data: {
    //            'CardID': CardID,
    //            'NewNameCard': NewNameCard,
    //        },
    //        dataType: "json",
    //        type: 'POST',
    //        success: function (data) {
    //            //alert("Successfully Inserted!");
    //        },
    //        error: function () {
    //            //Notification();
    //        }
    //    });


    var chatHub = $.connection.chatHub;
    function GetUserConnectedByName(UserName) {

        var connected = chatHub.server.getActiveConnectionByUserId(UserName);

        if (connected === "true") {
            $('.js-status-on-user').css('display', 'block');
        }
    }
});