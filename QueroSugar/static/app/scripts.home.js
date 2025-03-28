


$(document).ready(function () {

    var imagemRotation = 1;
    var refreshId = setInterval(backgrounChange, 5000);
    
    function backgrounChange() {
        
        if (imagemRotation > 3) {
            imagemRotation = 1;
        }

        if (imagemRotation === 1) {
           
            $('.background_selection_image3').removeClass('Op(1)');
            $('.background_selection_image3').addClass('Op(0)');

            $('.background_selection_image1').removeClass('Op(0)');
            $('.background_selection_image1').addClass('Op(1)');
        }
        else if (imagemRotation === 2) {
            $('.background_selection_image1').removeClass('Op(1)');
            $('.background_selection_image1').addClass('Op(0)');

            $('.background_selection_image2').removeClass('Op(0)');
            $('.background_selection_image2').addClass('Trsde($xslow) Op(1)');
        }
        else if (imagemRotation === 3) {
            $('.background_selection_image2').removeClass(' Op(1)');
            $('.background_selection_image2').addClass('Op(0)');

            $('.background_selection_image3').removeClass('Op(0)');
            $('.background_selection_image3').addClass('Op(1)');
        }

        imagemRotation++;
    }

    $(document).on("click", ".btn_abreModal_Entrar", function (e) {

        $('#modal-manager').html("");

        $.get('/Home/_ModalLoginPartial', function (data) {
            
            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });

    $(document).on("click", "#btnProblemaLogin", function (e) {

        $('#modal-manager').html("");

        $.get('/Home/_ModalRecuperaContaPartial', function (data) {
            
            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });

    $(document).on("click", ".btnRecuperaContaEmail", function (e) {


        $('#modal-manager').html("");

        $.get('/Home/_ModalRecuperaPorEmailPartial', function (data) {
            
            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });


    $(document).on("click", ".btnCadastrarCeluar", function (e) {


        $('#modal-manager').html("");

        $.get('/Home/_ModalCadastrarCelularPartial', function (data) {

            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });

    $(document).on("click", ".btnCadastrarEmail", function (e) {


        $('#modal-manager').html("");

        $.get('/Home/_ModalCadastrarEmailPartial', function (data) {

            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });

    $(document).on("click", ".btnCadastrarInformacoes", function (e) {


        $('#modal-manager').html("");

        $.get('/Home/_ModalCadastrarInformacoesPartial', function (data) {

            $('#modal-manager').append(data);
        });
        e.preventDefault();
    });


    

    //$(document).on("click", ".StretchedBox", function (e) {
    //    $('#modal-manager').html("");
    //    e.preventDefault();
    //});

    $(document).on("click", ".StretchedBox", function (e) {
        var container = $(".panel_modal");

        if (!container.is(e.target) && container.has(e.target).length === 0) {

            $('#modal-manager').html("");

        }
        e.preventDefault();
    });

    $(document).on("click", ".btn_fechar", function (e) {
        
        $('#modal-manager').html("");
        e.preventDefault();
    });



    $(document).on("click", "#btnLoginKinkee", function (e) {

        location.href = 'https://app.kinkee.co/';
        e.preventDefault();
    });

   
    
});