$(document).ready(function () {

    //$(document).on("click", "#btEditNoPicture", function (e) {

    //    $.get('../../Dating/UploadFotoModal', function (data) {

    //        $('#mensagemModal').append(data);
    //    });
    //    e.preventDefault();
    //});

    

    $(document).on("click", ".js-exclui-foto", function (e) {

        if (confirm('Deseja excluir essa foto?')) {

            idIMG = $(this).children("p").children(".js-click-exclui-foto").attr("data-id-action");
            
            ExcluirFotoGaleria(idIMG);

            $(this).parent('.js-div-img').remove();
            location.reload();
        }
        
        e.preventDefault();
    });

    $(document).on("click", ".js-privada-foto", function (e) {
        if (confirm('Deseja tornar essa foto privada?')) {

            //idIMG = $(this).parent().attr("src");
            //alert(idIMG);
            AlteraPrivacidadeFoto(idIMG);
            location.reload();
        }

        e.preventDefault();

    });


    

    

    $(document).on("click", ".slmodal__close", function (e) {

        $('#mensagemModal').empty();
        e.preventDefault();
    });

    $(document).on("click", ".profile-edit-avatar", function (e) {

        $('#myModalEnviaFoto').css("display", "block");
        e.preventDefault();
    });


    function ExcluirFotoGaleria(idIMG) {
        $.ajax({
            url: "../../FileUpload/ExcluirFotoGaleria",
            data: {
                'idFoto': idIMG,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {
                //alert("Successfully Inserted!");
            },
            error: function () {
                //Notification();
            }
        });
    }

    function AlteraPrivacidadeFoto(idIMG) {
        $.ajax({
            url: "../../FileUpload/AlteraPrivacidadeFoto",
            data: {
                'idFoto': idIMG,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {
                //alert("Successfully Inserted!");
            },
            error: function () {
                //Notification();
            }
        });
    }

    



    $(document).on("click", "#btn-favorito", function (e) {


        nomeUsuarioFavorito = $(this).attr('js-usuario');
        AddAndRemoveUserFavorite(nomeUsuarioFavorito);

        e.preventDefault();


    });

    function AddAndRemoveUserFavorite(nomeUsuarioFavorito) {
        $.ajax({
            url: "/Dating/AddAndRemoveUserFavorite",
            data: {
                'nomeUsuarioFavorito': nomeUsuarioFavorito,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {
                //alert("Successfully Inserted!");
            },
            error: function () {
                //Notification();
            }
        });
    }



    $(document).on("click", ".js-btn-relationship.send-request", function (e) {

        $('.js-btn-relationship').css("display", "none");
        $('.load-btn-friendship').css("display", "block");
        var url = $(location).attr("href").split('/').pop();


        $.ajax({
            url: '/Dating/RequestFriendShip',
            data: { NomeUsuario: url},
            dataType: 'html',
            type: 'GET',
            success: function (data) {
                if (data !== "") {
                    if (data === "True") {
                        $('.js-btn-relationship').removeClass("send-request");
                        $('.js_text-btn-relationship').html("Solicitação Enviada");
                        $('.js-btn-relationship').removeClass("no-friend");
                        $('.js-btn-relationship').addClass("friend");
                        $('.load-btn-friendship').css("display", "none");
                        $('.js-btn-relationship').css("display", "block");
                        $('.js-action-undo-relationship').css("display", "block");

                        location.reload();
                    }
                    else {
                        $('.js-btn-relationship').css("display", "block");
                        $('.load-btn-friendship').css("display", "none");
                    }
                }
            }
        });


        e.preventDefault();

    });

    $(document).on("click", ".js-solicitar-amizade", function (e) {

        $('.js-btn-relationship').css("display", "none");
        $('.load-btn-friendship').css("display", "block");

        var uid = $("#idUsuarioAmizade").val();
       
        $.ajax({
            url: '/Dating/ResponseFriendRequest',
            data: { Responde: '1', Uid: uid },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data !== "") {

                    location.reload();
                }
            }
        });
    });

    $(document).on("click", ".js-confirmar-amizade", function (e) {

        $('.js-btn-relationship').css("display", "none");
        $('.load-btn-friendship').css("display", "block");

        var uid = $("#idUsuarioAmizade").val();


        $.ajax({
            url: '/Dating/ResponseFriendRequest',
            data: { Responde: '2', Uid: uid },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data !== "") {

                    location.reload();
                }
            }
        });
    });

    $(document).on("click", ".js-recusar-amizade", function (e) {

        $('.js-btn-relationship').css("display", "none");
        $('.load-btn-friendship').css("display", "block");

        var uid = $("#idUsuarioAmizade").val();


        $.ajax({
            url: '/Dating/ResponseFriendRequest',
            data: { Responde: '3', Uid: uid },
            dataType: 'json',
            type: 'POST',
            success: function (data) {
                if (data !== "") {

                    location.reload();
                }
            }
        });
    });
});






