$(document).ready(function () {

    //$('#precadastro_se').click(function (event) {
    //    $.get('modules/views/form_cadastro.html', function (data) {
    //        $('#html_injetado').append(data);
    //    });
    //    event.preventDefault();
    //});
    GetCountUsuariosAtivos();
    function GetCountUsuariosAtivos() {
        $.ajax({
            url: '@Url.Action("GetCountUsuariosAtivos", "Home")',
            dataType: 'json',
            type: 'GET',
            success: function (data) {
                $('.home-count-up').empty();
                $('.home-count-up').append(data);
                return false;
            }
        });
    }

    $(document).on("click", ".precadastro_se", function (e) {
       
        $.get('CadastroModal', function (data) {

            $('#html_injetado').append(data);
        });
        e.preventDefault();
    });


    $(document).on("click", ".btn-signin", function (e) {

        $.get('LoginModal', function (data) {

            $('#html_injetado').append(data);
        });
        e.preventDefault();
    });



    


    //$(document).on("click", ".pl-close", function (e) {
    //    //$('#html_injetado').remove();

    //    alert();
    //    e.preventDefault();
    //});

});






