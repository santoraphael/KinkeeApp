jQuery(document).ready(function ($) {

    




    var actualUrl = 'https://paymentsecure.kinkee.co/';
    $.getScript(actualUrl + 'public/assets/js/frontend/functions.js', function () { });
    $.getScript(actualUrl + 'public/assets/js/frontend/Pagamentopagseguro.js', function () {
        var idcontainerprocessamento = containerrealizaprocessamento;
        var idbtniniciaprocesso = btniniciaprocesso;
        var stringcartaodecredito = "";
        var stringdebito = "";
        var stringparcelas = '';
        var template = ``;
        var PagamentoPagseguro = new Pagamentopagseguro(
            {
                mode: 'prod',
                executionUrl: actualUrl + "",
                urlProcessaCompra: "processacompra",
                urlSetaHash: "setahashusuario/",
                urlSetaTokenCartao: "setatokencartao/",
                urlFinalizaCompraCartaodeCredito: "finalizacompramaster/creditCard",
                urlFinalizaCompraBoleto: "finalizacompramaster/boleto",
                urlFinalizaCompraDebitoOnline: "finalizacompramaster/eft",
                urlFinalizaTransacao: "finalizatransacao",
                maximoParcelasSemJuros: 12,
            }
        );
        $(document).on("click", "#" + idbtniniciaprocesso, function () {
            alert('clicado')
            $('#btniniciaprocesso').addClass('hidden d-none');
            PagamentoPagseguro.processaCompra();
            PagamentoPagseguro.pegameiosDePagamentos();
            var intervalomeiosdepagamento = setInterval(function () {
                if (PagamentoPagseguro.meiosDePagamentos) {
                    $('#' + idcontainerprocessamento).html($(template));
                    window.clearInterval(intervalomeiosdepagamento);
                    $(Object.values(PagamentoPagseguro.meiosDePagamentos)).each(function (indice, elemento) {
                        PagamentoPagseguro.tipopagamento = elemento;
                        PagamentoPagseguro.recuperaPagamentos();
                        if (elemento.name == "BOLETO") {
                            if (tipo == 'comprakoins') {
                                $("#divpagamentoboleto").removeClass("d-none hidden");
                                if (elemento.options.BOLETO.status == "AVAILABLE") {
                                    $("#imgboleto").attr("src", PagamentoPagseguro.urlResources + elemento.options.BOLETO.images.MEDIUM.path);
                                }
                            }
                        } else if (elemento.name == "CREDIT_CARD") {
                            $('#divslideone').slideToggle();
                            $('#divslidetwo').slideToggle();
                            $("#tabelapagamentos").removeClass("d-none hidden");
                            $(PagamentoPagseguro.retornoPagamento).each(function (indice2, elemento2) {
                                if (elemento2.status == "AVAILABLE") {
                                    stringcartaodecredito += "<tr><td></td><td><img src='https://stc.pagseguro.uol.com.br/" + elemento2.images.MEDIUM.path + "'></td><td><input type='radio' name='cartaodecredito' class='cartaodecredito' value='" + elemento2.code + "'><label>" + elemento2.name + "</label></td></tr>";
                                }
                            });
                            $('#divpagamentocartao').removeClass('hidden');
                            $("#containercartaodecredito").html("<div class='row'><div class='col-lg-12'><h4><i class='fa fa-check'></i> Escolha a banderia:</h4></div></div><br/><table class='table table-hover table-striped col-lg-12 text-left'>" + stringcartaodecredito + "</table>");
                        } else if (elemento.name == "ONLINE_DEBIT") {
                            if (tipo == 'comprakoins') {
                                $("#divpagamentodebito").removeClass("d-none hidden");
                                $(PagamentoPagseguro.retornoPagamento).each(function (indice2, elemento3) {
                                    if (elemento3.status == "AVAILABLE") {
                                        stringdebito += "<tr><td>" + elemento3.displayName + "</td><td><img src='https://stc.pagseguro.uol.com.br/" + elemento3.images.MEDIUM.path + "'></td><td><input type='radio' name='debitoonline' class='debitoonline' data-name='" + elemento3.name + "' value='" + elemento3.code + "'></td></tr>";
                                    }
                                });
                                $("#containerdebitoonline").html("<div class='row'><div class='col-lg-12'><h4>Escolha a banderia:</h4></div></div><br/><table class='table table-hover table-striped col-lg-12 text-left'>" + stringdebito + "</table>");
                            }
                        }
                    });
                }
            }, 300, idcontainerprocessamento);

            $(document).on("click", ".tipopagamento", function () {
                $('#listaformasdepagamento').addClass('d-none hidden');
                if ($(this).val() == "1") {
                    $("#containerboleto").removeClass("d-none hidden");
                }
                if ($(this).val() == "2") {
                    $("#containercartaodecredito").removeClass("d-none hidden");
                }
                if ($(this).val() == "3") {
                    $("#containerdebitoonline").removeClass("d-none hidden");
                }
            });
            $(document).on("click", ".debitoonline", function () {
                PagamentoPagseguro.bank = $(this).attr("data-name");
                PagamentoPagseguro.finalizaOpcaoDebitoOnline();
                var intervaloDebito = setInterval(function () {
                    if (PagamentoPagseguro.dataLink) {
                        window.clearInterval(intervaloDebito);
                        $("#containerdebitoonline").html("<table class='table table-hover table-striped'><tr><td></td><td>" + PagamentoPagseguro.dataLink + "</td><td></td></tr></table>");
                    }
                }, 100);
            });
            $(document).on("click", "#btnboleto", function () {
                PagamentoPagseguro.finalizaOpcaoBoleto();
                var intervaloBoleto = setInterval(function () {
                    if (PagamentoPagseguro.dataLink) {
                        window.clearInterval(intervaloBoleto);
                        $('#containerboleto').removeClass('hidden d-none');
                        $("#containerboleto").html(PagamentoPagseguro.dataLink);
                    }
                }, 100);
            });
            $(document).on("change", ".cartaodecredito", function () {
                $("#containercartaodecredito").addClass("hidden d-none");
                $("#containerdadoscartao").removeClass("hidden d-none");
            });
            $(document).on("click", "#btnescolhecartao", function () {
                var formvalido = $('#formcartao').validationEngine('validate');
                if (formvalido) {
                    PagamentoPagseguro.numeroDoCartao = $("#numerodocartao").val();
                    PagamentoPagseguro.numeroCvv = $("#cvv").val();
                    PagamentoPagseguro.mesExpiracaoCartao = $("#mesexpiracartao").val();
                    PagamentoPagseguro.anoExpiracaoCartao = $("#anoexpiracartao").val();
                    PagamentoPagseguro.enviaDadosCartao();
                    function verificaParcelasRetornadas() {
                        if (tipo == 'assinatura' || tipo == 'assinaturafanclub') {
                            if (PagamentoPagseguro.tokencreditcard) {
                                $('#tokencreditcard').val(PagamentoPagseguro.tokencreditcard);
                                $.ajax({
                                    url: actualUrl + 'assina',
                                    dataType: 'json',
                                    type: "POST",
                                    data: {
                                        form: $('#formcartao').serialize()
                                    },
                                    success: function (data) {
                                        if (data.error) {
                                            alert("Houveram erros na requisição");
                                        } else {
                                            //assinatura principal
                                            if (tipo == 'assinatura') {
                                            } else {
                                                ProcessaAssinaturaFanClub(hash);
                                            }
                                        }
                                        escondeOverlay();
                                    },
                                    beforeSend: function () {
                                        mostraOverlay('Processando.');
                                    }, error: function (data) {
                                        mostraMensagem('Houve um erro na requisição da escolha do débito online.');
                                    }
                                });
                                return true;
                            }
                        }
                        if (PagamentoPagseguro.parcelas) {
                            $(PagamentoPagseguro.parcelas).each(function (index, elemento) {
                                stringparcelas += "<tr><td>" + elemento.quantity + "</td><td>" + elemento.installmentAmount + "</td><td>" + elemento.totalAmount + "</td><td><input type='radio' name='escolheparcelas' data-quantidade='" + elemento.quantity + "' data-valortotal='" + elemento.totalAmount + "' data-valorparcela='" + elemento.installmentAmount + "' class='escolheparcelas' value='" + index + "'></td></tr>";
                            });
                            $("#containerparcelascartao").html("<div class='row'><div class='col-lg-12'><h4>Escolha a quantidade do parcelamento:</h4></div></div><table class='table table-hover table-striped'>" + stringparcelas + "</table>");
                            $("#containerparcelascartao").removeClass("d-none hidden");
                            $("#containerdadoscartao").addClass("d-none hidden");
                            return true;
                        }
                        return false;
                    }
                    var intervaloVerificaParcelas = setInterval(function () {
                        if (verificaParcelasRetornadas()) {
                            window.clearInterval(intervaloVerificaParcelas);
                        }
                    }, 1000);
                }
            });
            $(document).on("click", ".escolheparcelas", function () {
                PagamentoPagseguro.quantidadeParcelas = $(this).attr("data-quantidade");
                PagamentoPagseguro.valorparcelas = $(this).attr("data-valorparcela");
                PagamentoPagseguro.totalvalor = $(this).attr("data-totalAmount");
                PagamentoPagseguro.escolheParcela();
                var intervalofinal = setInterval(function () {
                    if (typeof PagamentoPagseguro.idCompra !== "undefined") {
                        window.clearInterval(intervalofinal)
                        setTimeout(function () {
                            alert('Compra realizada com sucesso, redirecionando.');
                            window.location = (actualUrl);
                        }, 3000);
                    }
                }, 100);
            });
        });
        $(document).on('click', '#btnfinalizaboleto', function () {
            alert('Compra realizada com sucesso.');
        });
       
        function ProcessaAssinaturaFanClub(hash) {
            $.ajax({
                url: "/FanClub/CompraAssinatura",
                data: {
                    'HashPagSeguro': hash,
                },
                dataType: "html",
                type: 'POST',
                success: function (data) {
                    location.reload();
                },
                error: function () {
                    alert('Erro ao atualizar a sua nova compra, entre em contato com o suporte');
                }
            });
            e.preventDefault();
        }
    });
});