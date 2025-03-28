

const idcompra = 1;
const usuarioModel = {};
const urlretorno = 'http://www.kinkee.com/retornopagamentos/';

const containerrealizaprocessamento2 = 'process';
const btniniciaprocesso2 = 'btnpagseguro';
const tipo = 'assinatura';
const tokenplano = '';
var itens;
$(document).on('click', '.btnbuykoin', function () {

    itens = [[$(this).data('identity'), $(this).data('description'), $(this).data('value'), '1']];
    $('#tablelistproducts').slideToggle();
    $('#btnpagseguro').click();
});

var nome;
var datadenascimento;
var datadeabertura = '';
var ddd;
var telefone;
var cpf;
var cnpj = '';
var endereco;
var numero;
var complemento;
var bairro;
var cidade;
var estado;
var cep;
var tipodepessoa = 1;



jQuery(document).ready(function ($) {






    var actualUrl = 'https://paymentsecure.kinkee.co/';
    $.getScript(actualUrl + 'public/assets/js/frontend/functions.js', function () { });
    $.getScript(actualUrl + 'public/assets/js/frontend/Pagamentopagseguro.js', function () {
        var idcontainerprocessamento = containerrealizaprocessamento2;
        var idbtniniciaprocesso2 = btniniciaprocesso2;
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
        $(document).on("click", "#" + idbtniniciaprocesso2, function () {


            PagamentoPagseguro.processaCompra();
            PagamentoPagseguro.pegameiosDePagamentos();
            var intervalomeiosdepagamento = setInterval(function () {
                if (PagamentoPagseguro.meiosDePagamentos) {

                    $('#tablelistproducts').hide();
                    $('#tabelapagamentos').removeClass('hidden');
                    window.clearInterval(intervalomeiosdepagamento);
                    $(Object.values(PagamentoPagseguro.meiosDePagamentos)).each(function (indice, elemento) {

                        PagamentoPagseguro.tipopagamento = elemento;
                        PagamentoPagseguro.recuperaPagamentos();
                        if (elemento.name == "BOLETO") {
                            $("#divpagamentoboleto2").removeClass("d-none hidden");
                            if (elemento.options.BOLETO.status == "AVAILABLE") {
                                $("#imgboleto").attr("src", PagamentoPagseguro.urlResources + elemento.options.BOLETO.images.MEDIUM.path);

                            }
                        } else if (elemento.name == "CREDIT_CARD") {
                            $("#tabelapagamentos2").removeClass("d-none hidden");
                            $(PagamentoPagseguro.retornoPagamento).each(function (indice2, elemento2) {
                                if (elemento2.status == "AVAILABLE") {
                                    stringcartaodecredito += "<tr><td></td><td><img src='https://stc.pagseguro.uol.com.br/" + elemento2.images.MEDIUM.path + "'></td><td><input type='radio' name='cartaodecredito' class='cartaodecredito2' value='" + elemento2.code + "'><label>" + elemento2.name + "</label></td></tr>";
                                }
                            });
                            $('#divpagamentocartao2').removeClass('hidden');
                            $("#containercartaodecredito2").html("<div class='row'><div class='col-lg-12'><h4><i class='fa fa-check'></i> Escolha a banderia:</h4></div></div><br/><table class='table table-hover table-striped col-lg-12 text-left'>" + stringcartaodecredito + "</table>");
                        } else if (elemento.name == "ONLINE_DEBIT") {
                            $("#divpagamentodebito2").removeClass("d-none hidden");
                            $(PagamentoPagseguro.retornoPagamento).each(function (indice2, elemento3) {
                                if (elemento3.status == "AVAILABLE") {
                                    stringdebito += "<tr><td>" + elemento3.displayName + "</td><td><img src='https://stc.pagseguro.uol.com.br/" + elemento3.images.MEDIUM.path + "'></td><td><input type='radio' name='debitoonline' class='debitoonline2' data-name='" + elemento3.name + "' value='" + elemento3.code + "'></td></tr>";
                                }
                            });
                            $("#containerdebitoonline2").html("<div class='row'><div class='col-lg-12'><h4>Escolha a banderia:</h4></div></div><br/><table class='table table-hover table-striped col-lg-12 text-left'>" + stringdebito + "</table>");

                        }
                    });
                }
            }, 300, idcontainerprocessamento);
            var formadepagamento;
            $(document).on("click", ".tipopagamento2", function () {
                formadepagamento = $(this).val();
                $('#dadosusuario').removeClass('hidden');
                $('#listaformasdepagamento2').addClass('d-none hidden');
                if ($(this).val() == "1") {
                    //$("#containerboleto2").removeClass("d-none hidden");
                }
                if ($(this).val() == "2") {
                    //$("#containercartaodecredito2").removeClass("d-none hidden");
                }
                if ($(this).val() == "3") {
                    //$("#containerdebitoonline2").removeClass("d-none hidden");
                }
            });
            $(document).on('click', '#enviadadosusuario', function () {
                nome = $('#sendername').val();
                datadenascimento = $('#datadenascimento').val();
                datadeabertura = '';
                ddd = $('#phoneareaCode').val();
                telefone = $('#phonenumber').val();
                cpf = $('#documentsvalue').val();
                cnpj = '';
                endereco = $('#addressstreet').val();
                numero = $('#addressnumber').val();
                complemento = $('#complemento').val();
                bairro = $('#addressdistrict').val();
                cidade = $('#addresscity').val();
                estado = $('#addressstate').val();
                cep = $('#addresspostalCode').val();
                tipodepessoa = 1;
                var formvalido = $('#form').validationEngine('validate');
                if (formvalido) {
                    if (formadepagamento == 1) {
                        $("#btnboleto2").click();
                        $('#dadosusuario').hide();
                    } else if (formadepagamento == 2) {
                        $("#containercartaodecredito2").removeClass("d-none hidden");
                        $('#dadosusuario').hide();
                    } else {
                        $('#dadosusuario').hide();
                        $("#containerdebitoonline2").removeClass("d-none hidden");
                    }
                }
            });
            $(document).on("click", ".debitoonline2", function () {
                PagamentoPagseguro.bank = $(this).attr("data-name");
                PagamentoPagseguro.finalizaOpcaoDebitoOnline();
                var intervaloDebito = setInterval(function () {
                    if (PagamentoPagseguro.dataLink) {
                        window.clearInterval(intervaloDebito);
                        $("#containerdebitoonline2").html("<table class='table table-hover table-striped'><tr><td></td><td>" + PagamentoPagseguro.dataLink + "</td><td></td></tr></table>");
                    }
                }, 100);
            });
            $(document).on("click", "#btnboleto2", function () {
                PagamentoPagseguro.finalizaOpcaoBoleto();
                var intervaloBoleto = setInterval(function () {
                    if (PagamentoPagseguro.dataLink) {
                        window.clearInterval(intervaloBoleto);
                        $('#containerboleto2').removeClass('hidden d-none');
                        $("#containerboleto2").html(PagamentoPagseguro.dataLink);
                    }
                }, 100);
            });
            $(document).on("change", ".cartaodecredito2", function () {
                $("#containercartaodecredito2").addClass("hidden d-none");
                $("#containerdadoscartao2").removeClass("hidden d-none");
            });
            $(document).on("click", "#btnescolhecartao2", function () {
                var formvalido = $('#formcartao').validationEngine('validate');
                if (formvalido) {
                    PagamentoPagseguro.numeroDoCartao = $("#numerodocartao2").val();
                    PagamentoPagseguro.numeroCvv = $("#cvv2").val();
                    PagamentoPagseguro.mesExpiracaoCartao = $("#mesexpiracartao2").val();
                    PagamentoPagseguro.anoExpiracaoCartao = $("#anoexpiracartao2").val();
                    PagamentoPagseguro.enviaDadosCartao();
                    function verificaParcelasRetornadas() {

                        if (PagamentoPagseguro.parcelas) {
                            $(PagamentoPagseguro.parcelas).each(function (index, elemento) {
                                stringparcelas += "<tr><td>" + elemento.quantity + "</td><td>" + elemento.installmentAmount + "</td><td>" + elemento.totalAmount + "</td><td><input type='radio' name='escolheparcelas' data-quantidade='" + elemento.quantity + "' data-valortotal='" + elemento.totalAmount + "' data-valorparcela='" + elemento.installmentAmount + "' class='escolheparcelas2' value='" + index + "'></td></tr>";
                            });
                            $("#containerparcelascartao2").html("<div class='row'><div class='col-lg-12'><h4>Escolha a quantidade do parcelamento:</h4></div></div><table class='table table-hover table-striped'>" + stringparcelas + "</table>");
                            $("#containerparcelascartao2").removeClass("d-none hidden");
                            $("#containerdadoscartao2").addClass("d-none hidden");
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
            $(document).on("click", ".escolheparcelas2", function () {
                PagamentoPagseguro.quantidadeParcelas = $(this).attr("data-quantidade");
                PagamentoPagseguro.valorparcelas = $(this).attr("data-valorparcela");
                PagamentoPagseguro.totalvalor = $(this).attr("data-totalAmount");
                PagamentoPagseguro.escolheParcela();
                var intervalofinal = setInterval(function () {
                    if (typeof PagamentoPagseguro.idCompra !== "undefined") {
                        window.clearInterval(intervalofinal)
                        setTimeout(function () {
                            alert('Compra realizada com sucesso, redirecionando.');
                            window.location.reload();
                        }, 3000);
                    }
                }, 100);
            });
        });
        $(document).on('click', '#btnfinalizaboleto2', function () {
            alert('Compra realizada com sucesso.');
        });

    });
});







