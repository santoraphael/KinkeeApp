$(document).ready(function () {

    //USERNAME
    $('.js-btn-open-edit-username').click(function () {
        $('.js-edit-username').css("display", "block");
        $('.js-edit-username-header').css("display", "none");
    });

    $('.js-btn-cancel-edit-username').click(function () {
        $('.js-edit-username').css("display", "none");
        $('.js-edit-username-header').css("display", "block");
    });

    $("#inputEnterUsername").bind('keyup', function (e) {


        if ($(this).val().length > 4) {
            //$('.js-load-username').css("display", "block");
            ValidaUsuarioRegex($(this).val().trim());
        }
        else {
            $('.js-alert-username').html('Nome de usuário muito curto');
            $(".js-btn-salvar-Username-Gold").attr('disabled', true);
        }


        //if ($(".js-nova-senha").val()) {
        //    $(".js-nova-senha-confirmacao").removeAttr("disabled");
        //}
        //else {
        //    $(".js-nova-senha-confirmacao").attr('disabled', true);

        //}

    });

    $('.js-btn-salvar-Username-Gold').click(function () {
        SalvarUsuario();
    });

    $('.js-btn-salvar-Username').click(function () {
        ChamarPainelAssinatura();
    });

    $(document).on("click", ".js-close-button-assinatura", function (e) {

        $('#mensagemModal').empty();

        e.preventDefault();
    });

    function SalvarUsuario()
    {
        SalvarNomedeUsuario($('#inputEnterUsername').val().trim());
    }

    function ChamarPainelAssinatura() {
        $.get('/MinhaConta/_AssinaturaPartial', function (data) {
            $('#mensagemModal').append("<div class='modal-backdrop in' ng-style='{'z-index': 1040 + (index & amp;&amp; 1 || 0) + index * 10}' modal-backdrop='' style='z- index: 1051;'></div>");
            $('#mensagemModal').append(data);
        });
        e.preventDefault();
    }
    

    function ValidaUsuarioRegex(Usuario) {

        $.ajax({
            url: "../Home/ValidaUsuarioRegex",
            async: false,
            data: {
                'Usuario': Usuario,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;

                if (retorno == 0) {
                    $('.js-alert-username').html('Usuário Inválido. Exemplo de Nome de usuário Válido: RAPHAELSANTO, BABYLINDA123.');
                }
                else {
                    $('.js-alert-username').html('Validando se já existe um usuário com o mesmo nome.');
                    ValidaUsuario(Usuario);
                }
            },
            error: function () {

            }
        });

    }

    function SalvarNomedeUsuario(Usuario) {
        $('.js-load-username').css("display", "block");
        $.ajax({
            url: "/MinhaConta/SalvarNomedeUsuario",
            async: false,
            data: {
                'Usernsame': Usuario.toUpperCase(),
            },
            dataType: "json",
            type: 'POST',
            success: function () {
                $('.js-alert-username').html('Seu perfil será desconectado do site em 3 segundos. Entre novamente!');
                $('.js-load-username').css("display", "none");
            },
            done: function () {
                $('.js-alert-username').html('Seu perfil será desconectado do site em 3 segundos. Entre novamente!');
                $('.js-load-username').css("display", "none");
            },
            error: function () {
                $('.js-alert-username').html('Seu perfil foi desconectado. Entre novamente!');
            }
        });

    }

    function ValidaUsuario(Usuario) {
        $.ajax({
            url: "../Home/ValidaUsuario",
            async: false,
            data: {
                'Usuario': Usuario,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;

                if (retorno == 0) {

                    $('.js-alert-username').html('Nome de usuário disponível NÃO disponível.');
                    $(".js-btn-salvar-Username-Gold").attr('disabled', true);
                    
                }
                else {
                    $('.js-alert-username').html('Nome de usuário disponível.');
                    
                    $(".js-btn-salvar-Username-Gold").removeAttr("disabled");
                }

            },
            error: function () {

            }
        });
    }


    


    //EMAIL
    $('.js-btn-open-edit-email').click(function () {
        $('.js-edit-email').css("display", "block");
        $('.js-edit-email-header').css("display", "none");
    });

    $('.js-btn-cancel-edit-email').click(function () {
        $('.js-edit-email').css("display", "none");
        $('.js-edit-email-header').css("display", "block");
    });

    //SENHA
    $('.js-btn-open-edit-senha').click(function () {
        $('.js-edit-senha').css("display", "block");
        $('.js-edit-email-header').css("display", "none");
    });

    $('.js-btn-cancel-edit-senha').click(function () {
        $('.js-edit-senha').css("display", "none");
        $('.js-edit-senha-header').css("display", "block");
    });

    //ALTERANDO SENHA
    $(".js-nova-senha").bind('keyup', function (e) {

        if ($(".js-nova-senha").val()) {
            $(".js-nova-senha-confirmacao").removeAttr("disabled");
        }
        else {
            $(".js-nova-senha-confirmacao").attr('disabled', true);

        }
    }).on('keyup', function (e) {
        if (e.keyCode == 8)
            $('.js-nova-senha').trigger('keyup');
        });

    $(".js-nova-senha-confirmacao").bind('keyup', function (e) {
        verificaSenhasIguais($(".js-nova-senha").val(), $(".js-nova-senha-confirmacao").val());

    }).on('keyup', function (e) {
        if (e.keyCode == 8) {

        }
            //$('.js-nova-senha').trigger('keyup');
    });

    function verificaSenhasIguais(senhaUm, SenhaDois)
    {
        if (senhaUm == SenhaDois)
        {
            $('.js-alert-password').css('display', 'none');
            $(".js-btn-salvar-senha").removeAttr("disabled");
        }
        else
        {
            $('.js-alert-password').html('As senhas não são iguais! Digite igual a primeira.');
            $('.js-alert-password').css('display', 'block');
            $(".js-btn-salvar-senha").attr('disabled', true);
        }
    }


    $('.js-btn-salvar-senha').click(function () {

        var novaSenha = $(".js-nova-senha").text();
        var novaSenhaConfirmacao = $(".js-nova-senha-confirmacao").text();
        if (novaSenha === novaSenhaConfirmacao) {
            alert('Senha Iguais');
        }
        else {
            alert('Senhas diferentes');
        }
    });


    $('.js-click-menu').click(function () {

        $('.js-mostra-esconde-menu').css("display", "block");

    });

    $('.js-btn-desativarConta').click(function () {

        desativarConta();

    });

    function desativarConta() {
        var txt;
        var r = confirm("Você realmente deseja desativar sua conta?");

        if (r == true) {

            $.ajax({
                url: "../../Account/DesativarContaUsuario",
                dataType: "json",
                type: 'POST',
                success: function (data) {
                    alert('Seu perfil foi desativado. Para reativa-lo basta entrar com seu login.');
                    window.location.href = data;
                },
                error: function () {

                }
            });

        }
    }


    $('.js-btn-open-edit-location').click(function (e) {
        $('.js-edit-location').css("display", "block");
        $('.js-edit-location-header').css("display", "none");

        //GetLocation();
        e.preventDefault();
    });

    $('.js-btn-cancel-edit-location').click(function (e) {
        $('.js-edit-location').css("display", "none");
        $('.js-edit-location-header').css("display", "block");

        e.preventDefault();
    });


    $('.js-btn-location').click(function () {

        GetLocation();
       
    });

    $('.js-btn-cancel-auto-location').click(function () {

        $('.js-input-location').attr('placeholder', 'Qual é sua Cidade?');
        $('.js-input-location').val("");
        $(".js-btn-salvar-location").attr("disabled",true);
    });

    $("#txtAddress").geocomplete({
        appendToParent: true,
        fields: {
            ".js-input-estado-location-hidden": "short state",
            ".js-input-pais-location-hidden": "short county",
            ".js-input-cidade-location-hidden": "short city"
        }
    });

    $(".js-input-location").bind('keyup', function (e) {

        $('.js-btn-salvar-location').removeAttr("disabled");
    });

    $('.js-btn-salvar-location').click(function () {


        var location = {

            pais: $('.js-input-pais-location-hidden').val(),
            estado: $('.js-input-estado-location-hidden').val(),
            cidade: $('.js-input-cidade-location-hidden').val(),
            latitude: $('.js-input-latitude-location-hidden').val(),
            longitude: $('.js-input-longitude-location-hidden').val(),
            ipv4: $('.js-input-ipv4-location-hidden').val()
        };

        SalvarLocationUsuario(location);

    });

    function SalvarLocationUsuario(location) {
        $.ajax({
            url: "/MinhaConta/SalvarLocation",
            async: false,
            data: {
                'Pais': location.pais,
                'Estado': location.estado,
                'Cidade': location.cidade,
                'Latitude': location.latitude,
                'Longitude': location.longitude,
                'IPv4': location.ipv4
            },
            dataType: "json",
            type: 'POST',
            success: function () {
                
            },
            done: function () {
                
            },
            error: function () {
                
            }
        });
    }

    function GetLocation() {
        $('.js-load-location').css("display", "block");
        $.ajax({
            url: "https://geoip-db.com/jsonp",
            jsonpCallback: "callback",
            dataType: "jsonp",
            success: function (location) {
                $('.js-input-location').val(location.city + ", " + location.state + ", " + location.country_name);
                $('.js-input-location-hidden').val(location.city + ", " + location.state + ", " + location.country_name);
                $(".js-btn-salvar-location").removeAttr("disabled");
                $('.js-load-location').css("display", "none");

                $('#country').html(location.city + ", " + location.state + ", " + location.country_name);

                $('.js-input-pais-location-hidden').val(location.country_name);
                $('.js-input-estado-location-hidden').val(location.state);
                $('.js-input-cidade-location-hidden').val(location.city);
                $('.js-input-latitude-location-hidden').val(location.latitude);
                $('.js-input-longitude-location-hidden').val(location.longitude);
                $('.js-input-ipv4-location-hidden').val(location.IPv4);
            }
        });
    }



    





    //GetCountUsuariosAtivos();
    //function GetCountUsuariosAtivos() {
    //    $.ajax({
    //        url: '@Url.Action("GetCountUsuariosAtivos", "Home")',
    //        dataType: 'json',
    //        type: 'GET',
    //        success: function (data) {
    //            $('.home-count-up').empty();
    //            $('.home-count-up').append(data);
    //            return false;
    //        }
    //    });
    //}

});






