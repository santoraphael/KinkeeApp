$(document).ready(function () {

    $('.maiuscula').keyup(function () {
        this.value = this.value.toUpperCase();
    });



    $('.js-envia-btn-pn').click(function () {
        
        validaForm();

    });


    function validaForm()
    {
        var valida = false;

        var Email = $('.cm-email-pn').val();
        var Usuario = $('.cm-user-pn').val();
        var Genero = $('.cm-gen-pn option:selected').val();
        var Senha = $('.cm-senha-pn').val();

        ValidaEmailRegex(Email);
        ValidaUsuarioRegex(Usuario);
        validateGender();
        validatePassword();

        if(!$('.form-group.email.has-feedback').hasClass( "has-error" )
            && !$('.form-group.input-group-lg.login.has-feedback').hasClass("has-error")
            && !$('.form-group.gender.has-feedback').hasClass("has-error")
            && !$('.form-group.password.has-feedback').hasClass("has-error")) {

            valida = true;
        }

        if(valida)
        {
            var tamanho = $('.modal-body').height();
            $('.modal-loading').css("display", "block");

            $('.modal-body').css("height", tamanho);
            $('.sl-form').css("display", "none");
            $('.js-envia-btn-pn').css("display", "none");

            GetForm(Email, Usuario, Genero, Senha);

            //alert('Cadastrado. Verifique seu email com a confirmação');
            //window.location.replace("https://Kinkee.co");

            setTimeout(function (e) {
                alert('Prontinho. Você já pode acessar a sua conta.');
                window.location.replace("/Dating");
                e.stopPropagation();
            }, 1000);
            return true;
            
        }
        else {
            alert('complete o cadastro');
        }

    }


    function GetForm(Email, Usuario, Genero, Senha) {
        $.ajax({
            url: "Home/GetForm",
            data: {
                'EmailUsuario': Email,
                'Usuario': Usuario,
                'Genero': Genero,
                'Senha': Senha,
                'Invite': getUrlParameter('Invite')
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {
                $('.loading-header').text(data);
            },
            error: function () {
                
            }
        });

    }

    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    };


    $('.cm-email-pn').focusout(function (e) {

        var Email = $('.cm-email-pn').val();

        ValidaEmailRegex(Email);

        e.stopPropagation();

    });


    $('.cm-user-pn ').focusout(function (e) {

        var Usuario = $('.cm-user-pn').val();
        

        ValidaUsuarioRegex(Usuario);
        e.stopPropagation();

    });

    $('.cm-gen-pn').focusout(function (e) {

        validateGender();

        e.stopPropagation();

    });

    function validateGender() {
        var valor = $('.cm-gen-pn option:selected').val()

        if (valor == 0) {
            $('.forSpanError').text('Selecione uma opção.');
            $('.form-group.gender.has-feedback').addClass('has-error');
        }
        else {
            $('.form-group.gender.has-feedback').removeClass('has-error');

        }
    }

    $('.cm-senha-pn').focusout(function (e) {
        var tamanho = $('.cm-senha-pn').val().length;

        if (tamanho < 5) {

            $('.form-group.password.has-feedback').addClass('has-error');
        }
        else {
            $('.form-group.password.has-feedback').removeClass('has-error');
        }


        e.stopPropagation();

    });

    function ValidaUsuario(Usuario) {
        $.ajax({
            url: "Home/ValidaUsuario",
            async:false,
            data: {
                'Usuario': Usuario,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;

                if (retorno == 0) {
                    
                    $('.form-group.input-group-lg.login.has-feedback').addClass('has-error');
                }
                else
                {
                    $('.form-group.input-group-lg.login.has-feedback').removeClass('has-error');
                }

            },
            error: function () {

            }
        });
    }

    function ValidaEmailRegex(Email) {

        $.ajax({
            url: "Home/ValidaEmailRegex",
            async: false,
            data: {
                'Email': Email,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;
  
                if (retorno == 0) {
                    $('.forSpanError').text('Email inválido.');
                    $('.form-group.email.has-feedback').addClass('has-error');
                    //return false;
                }
                else {
                    $('.form-group.email.has-feedback').removeClass('has-error');
                    ValidaEmail(Email);
                }
            },
            error: function () {

            }
        });

    }

    function ValidaUsuarioRegex(Usuario) {

        $.ajax({
            url: "Home/ValidaUsuarioRegex",
            async: false,
            data: {
                'Usuario': Usuario,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;

                if (retorno == 0) {
                    $('.forSpanError').text('Email inválido.');
                    $('.form-group.input-group-lg.login.has-feedback').addClass('has-error');
                }
                else {
                    $('.form-group.input-group-lg.login.has-feedback').removeClass('has-error');
                    ValidaUsuario(Usuario);
                }
            },
            error: function () {

            }
        });

    }

    function validatePassword() {
        var tamanho = $('.cm-senha-pn').val().length;

        if (tamanho < 5) {
            $('.form-group.password.has-feedback').addClass('has-error');
        }
        else {
            $('.form-group.password.has-feedback').removeClass('has-error');
        }
    }

    function ValidaEmail(Email) {

        $.ajax({
            url: "Home/ValidaEmail",
            async: false,
            data: {
                'Email': Email,
            },
            dataType: "json",
            type: 'POST',
            success: function (data) {

                var retorno = data;

                if (retorno == 0) {
                    $('.forSpanError').text('Email já cadastrado');
                    $('.form-group.email.has-feedback').addClass('has-error');
                }
                else {
                    $('.form-group.email.has-feedback').removeClass('has-error');
                    
                }
            },
            error: function () {

            }
        });

    }

    
    
});