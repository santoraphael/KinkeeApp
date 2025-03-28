var repositoryJsUrl = actualDomain + '/Scripts/aditionals/';


       
$.ajax({
    async: false,
    url: repositoryJsUrl + "comprakoins.js",
    dataType: "script"
});



if (typeof Jss !== "undefined" && typeof tipo == 'undefined') {


    $(".calendario").datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        changeYear: true
    });


    
    $(Jss).each(function (indice, elemento) {
      

        $.ajax({
            async: false,
            url: repositoryJsUrl + elemento + ".js",
            dataType: "script"
        });

    });
  
   
}
