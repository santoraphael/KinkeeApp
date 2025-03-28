$(document).ready(function () {

    //
    var qtdSteps = $(".wizard-step").length;

    for (var i = 1; i < qtdSteps; i++) {
        $(".onboarding-steps").append("<span class='step'></span>");
    }


    var stepAtual = $('div.wizard-step.active').index();
    var stepTotal = $('div.wizard-step').length;
    
    $('.wizard-step.active>.onboarding-header>.info').html((stepAtual + 1) + " de " + stepTotal);

    
    // ======== BOTÃO AVANÇAR ========//
    $('.js-btn-avancar').on("click", function (e) {


        var stepAtual = $('div.wizard-step.active').index();
        var stepTotal = $('div.wizard-step').length;
        $('.wizard-step.active>.onboarding-header>.info').html((stepAtual + 1) + " de " + stepTotal);


        var step = $('.js-modal-body').attr("vl-step");

        if (step >= qtdSteps) {
            return false;
        }

        var penultimoStep = qtdSteps - 1;
        if (step == penultimoStep) {
            $(this).removeClass('visible');
            $('.js-btn-enviar-wizard').addClass('visible');
        }

        var nextStep = parseInt(step) + 1;
        var valor = $('.js-modal-body').attr("vl-data");
        var calc = parseInt(valor) + 660;

       

        var divAtual = $(".wizard-step.active");
        divAtual.css("display", "none");
        divAtual.removeClass("active");
        var proximaDiv = $(divAtual).next(".wizard-step");
        proximaDiv.addClass("active");
        proximaDiv.css("left", calc + "px");
        proximaDiv.css("display", "block");

        var spanAtual = $(".step.active");
        spanAtual.removeClass("active");
        var proximoSpan = $(spanAtual).next(".step");
        proximoSpan.addClass("active");


        //DESABILITA BOTAO CASO PRECISE DE VALIDAÇÃO
        if ($(proximaDiv).hasClass('cad-validate')) {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('.js-btn-avancar').removeAttr('disabled');
        }


        $('.js-modal-body').css("transform", "translateX(-" + calc + "px)");


        $('.js-modal-body').attr("vl-data", calc);
        $('.js-modal-body').attr("vl-step", nextStep);

        var stepWelcome = $(".step-welcome");
        stepWelcome.css("display", "flex");

        e.preventDefault();
    });


    // ======== BOTÃO VOLTAR ========//
    $('.js-btn-voltar').on("click", function (e) {
        //alert('teste');
        var step = $('.js-modal-body').attr("vl-step");

        if (step <= 1) {
            return false;
        }

        $('.js-btn-enviar-wizard').removeClass('visible');
        $('.js-btn-avancar').addClass('visible');

        var nextStep = parseInt(step) - 1;
        var valor = $('.js-modal-body').attr("vl-data");
        var calc = parseInt(valor) - 660;

        var divAtual = $(".wizard-step.active");
        divAtual.removeClass("active");
        var anteriorDiv = $(divAtual).prev(".wizard-step");
        anteriorDiv.addClass("active");
        anteriorDiv.css("left", calc + "px");
        anteriorDiv.css("display", "block");

        var spanAtual = $(".step.active");
        spanAtual.removeClass("active");
        var anteriorSpan = $(spanAtual).prev(".step");
        anteriorSpan.addClass("active");

        //HABILITA BOTAO CASO PRECISE DE VALIDAÇÃO
        if ($(anteriorDiv).hasClass('cad-validate')) {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('.js-btn-avancar').removeAttr('disabled');
        }


        $('.js-modal-body').css("transform", "translateX(-" + calc + "px)");


        $('.js-modal-body').attr("vl-data", calc);
        $('.js-modal-body').attr("vl-step", nextStep);
        var stepWelcome = $(".step-welcome");
        stepWelcome.css("display", "flex");
    });

    $('.js-btn-enviar-wizard').on("click", function (e) {

        //SalvarLocationUsuario();
    });

    $('.interests-list li.interest').on("click", function (e) {
        $(this).toggleClass("active");

        //var context = $(this).children('[type="checkbox"]:hidden');

        //$(context).prop("checked", true);

    });

    $('#CategoriaRelacionamentSugar').on("click", function (e) {
        $("#CheckCategoriaRelacionamentSugar").prop("checked", true);
        //$('input').attr('name', 'CategoriaRelacionamentSugar').value(true);
    });

    $('#CategoriaPacksFotosFas').on("click", function (e) {
        $("#CheckCategoriaPacksFotosFas").prop("checked", !$("#CheckCategoriaPacksFotosFas").prop("checked"));
        //$('input').attr('name', 'CategoriaPacksFotosFas').value(true);
    });

    $('#CategoriaBDSM').on("click", function (e) {
        $("#CheckCategoriaBDSM").prop("checked", !$("#CheckCategoriaBDSM").prop("checked"));
        //$('input').attr('name', 'CategoriaRelacionamentSugar').value(true);
    });

    $(document).on("click", ".js-add-user-wizard", function (e) {

        $(this).children('span').html("Enviando...");
        var usuario = $(this).attr('data-user-name');

        $.ajax({
            url: '/Dating/RequestFriendShip',
            data: { NomeUsuario: usuario },
            dataType: 'html',
            type: 'GET',
            success: function (data) {
                if (data !== "") {

                    $(this).children('span').html("Solicitado");
                    $(this).attr('disabled', 'disabled');
                }
            }
        });

        $(this).children('span').html("Solicitado");
        $(this).attr('disabled', 'disabled');

        e.preventDefault();

    });

   

    $(".js-input-location").bind('keyup', function (e) {

        $('.js-btn-salvar-location').removeAttr("disabled");
    });


    // ===== VALIDAÇÕES FORMULARIO ==== //
    //VALIDAÇÂO SELECT
    $("#Country_str_code").change(function () {
        
    });

    $("#Estados").change(function () {
        
    });

    $("#Cidades").change(function () {


        if ($("#Cidades option:selected").text() === "-- Cidades --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('.js-btn-avancar').removeAttr('disabled');

            $('#Feature_int_id').val($(this).val());
            $('#Feature_str_name').val($('#Cidades option:selected').text());
        }
    });


    $("#ProfissoesSubcategorias").change(function () {

        if ($("#ProfissoesSubcategorias option:selected").text() === "-- Ecolha --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('#IdProfissao').val($(this).val());
            $('#NomeProfissao').val($('#ProfissoesSubcategorias option:selected').text());

            $('.js-btn-avancar').removeAttr('disabled');
        }
    });


    $("#RendaMensal").change(function () {

        if ($("#RendaMensal option:selected").text() === "-- Ecolha --" || $("#Patrimonio option:selected").text() === "-- Seu patrimômio pessoal declarado --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('#IdRenaMensal').val($(this).val());
            $('#DescricaoRenaMensal').val($('#Patrimonio option:selected').text());

            $('.js-btn-avancar').removeAttr('disabled');
        }
    });

    $("#Patrimonio").change(function () {

        if ($("#RendaMensal option:selected").text() === "-- Ecolha --" || $("#Patrimonio option:selected").text() === "-- Seu patrimômio pessoal declarado --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('#IdPatrimonio').val($(this).val());
            $('#DescricaoPatrimonio').val($('#Patrimonio option:selected').text());

            $('.js-btn-avancar').removeAttr('disabled');
        }
    });



    $("#Generosidade").change(function () {

        if ($("#Generosidade option:selected").text() === "-- Você acha que é... --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('#IdGenerosidade').val($(this).val());
            $('#DescricaoGenerosidade').val($('#Generosidade option:selected').text());

            $('.js-btn-avancar').removeAttr('disabled');
        }
    });


    $("#MotivoBaby").change(function () {

        if ($("#Generosidade option:selected").text() === "-- Você acha que é... --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('#IdMotivoBaby').val($(this).val());
            $('#DescricaoMotivoBaby').val($('#MotivoBaby option:selected').text());

            $('.js-btn-avancar').removeAttr('disabled');
        }
    });



    $("#EstadoCivil").change(function () {

        if ($("#EstadoCivil option:selected").text() === "-- Estado Civil --") {
            $('.js-btn-avancar').attr('disabled', 'disabled');
            $('#DescricaoRelacionamento').css('display', 'none');
            $('.lbl-descricao').css('display', 'none');
        }
        else {
            $('.js-btn-avancar').removeAttr('disabled');

            var escolha = $("#EstadoCivil option:selected").val();

            $('#StatusRelacionamento').val($(this).val());
            $('#NomeStatusRelacionamento').val($('#EstadoCivil option:selected').text());


            if (escolha === "2" || escolha === "3" || escolha === "4") {
                $('#DescricaoRelacionamento').css('display', 'block');
                $('.lbl-descricao').css('display', 'block');

                var placeholder = "";

                if (escolha === "2") {
                    placeholder = "Diga qual a sua disponibilidade para que não atrapalhe sua relação.";
                }
                else if (escolha === "3") {
                    placeholder = "Diga quais são suas disponibilidades de contato e encontro para manter sua relação sugar em sigilo.";
                }
                else if (escolha === "4") {
                    placeholder = "Diga como pretende levar o relacionaemtno sugar que busca. Informe sobre dias e horarios que são possíveis conversar e se encontrar.";
                }

                $('#DescricaoRelacionamento').attr('placeholder', placeholder);
            }
            else {
                $('#DescricaoRelacionamento').css('display', 'none');
                $('.lbl-descricao').css('display', 'none');
            }
        }
    });


    //-- SELECT

    //-- dasd
    $('#txtAddress').keyup(function (e) {

        if ($("#txtAddress").val().length < 20) {
            $('.js-btn-avancar').attr('disabled', 'disabled');
        }
        else {
            $('.js-btn-avancar').removeAttr('disabled');
        }
    });


    function SalvarLocationUsuario() {

        var location = {

            pais: $('.js-input-pais-location-hidden').val(),
            estado: $('.js-input-estado-location-hidden').val(),
            cidade: $('.js-input-cidade-location-hidden').val(),
            latitude: $('.js-input-latitude-location-hidden').val(),
            longitude: $('.js-input-longitude-location-hidden').val(),
            ipv4: $('.js-input-ipv4-location-hidden').val()
        };

        SalvarLocationUsuario(location);


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




     //$('.js-btn-location').click(function () {

    //    //GetLocation();

    //});

    //function GetLocation() {
    //    $('.js-load-location').css("display", "block");
    //    $.ajax({
    //        url: "https://geoip-db.com/jsonp",
    //        jsonpCallback: "callback",
    //        dataType: "jsonp",
    //        success: function (location) {
    //            $('.js-input-location').val(location.city + ", " + location.state + ", " + location.country_name);
    //            $('.js-input-location-hidden').val(location.city + ", " + location.state + ", " + location.country_name);
    //            $(".js-btn-salvar-location").removeAttr("disabled");
    //            $('.js-load-location').css("display", "none");

    //            //$('#country').html(location.city + ", " + location.state + ", " + location.country_name);

    //            $('.js-input-pais-location-hidden').val(location.country_name);
    //            $('.js-input-estado-location-hidden').val(location.state);
    //            $('.js-input-cidade-location-hidden').val(location.city);
    //            $('.js-input-latitude-location-hidden').val(location.latitude);
    //            $('.js-input-longitude-location-hidden').val(location.longitude);
    //            $('.js-input-ipv4-location-hidden').val(location.IPv4);
    //        }
    //    });
    //}


     //$("#txtAddress").geocomplete({
    //    appendToParent: true,
    //    fields: {
    //        ".js-input-estado-location-hidden": "short state",
    //        ".js-input-pais-location-hidden": "short county",
    //        ".js-input-cidade-location-hidden": "short city"
    //    }
    //});
});







