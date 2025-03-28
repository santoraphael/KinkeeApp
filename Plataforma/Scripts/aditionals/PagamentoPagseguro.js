'use strict';
var bandeira, tokenparcelas;
class Pagamentopagseguro {
    constructor(configuration) {

        this.executionUrl = configuration.executionUrl;
        this.urlProcessaCompra = configuration.urlProcessaCompra;
        this.urlSetaHash = configuration.urlSetaHash;
        this.urlSetaTokenCartao = configuration.urlSetaTokenCartao;
        this.urlFinalizaTransacao = configuration.urlFinalizaTransacao;
        this.urlFinalizaCompraCartaodeCredito = configuration.urlFinalizaCompraCartaodeCredito;
        this.urlFinalizaCompraBoleto = configuration.urlFinalizaCompraBoleto;
        this.urlFinalizaCompraDebitoOnline = configuration.urlFinalizaCompraDebitoOnline;
        this.maximoParcelasSemJuros = configuration.maximoParcelasSemJuros;
        if (configuration.mode === 'dev') {
            this.urlResources = 'https://stc.sandbox.pagseguro.uol.com.br';
            this.urlResourceDirectPayment = 'https://stc.sandbox.pagseguro.uol.com.br/pagseguro/api/v2/checkout/pagseguro.directpayment.js'
        } else {
            this.urlResources = 'https://stc.pagseguro.uol.com.br';
            this.urlResourceDirectPayment = 'https://stc.pagseguro.uol.com.br/pagseguro/api/v2/checkout/pagseguro.directpayment.js'
        }
        $.getScript(this.urlResourceDirectPayment, function () {
        });
    }
    processaCompra() {
        $.ajax({
            url: this.executionUrl + this.urlProcessaCompra,
            dataType: "json",
            type: "post",
            context: this,
            async: false,
            data: {itens: itens},
            success: function (resultado) {
                this.valorCompra = resultado.valor;
                this.idDaSessao = resultado.iddasessao;
            },
            error: function () {
                alert('Houve um erro no processamento e resgate do id da sessão e valor total da compra.');
            }
        });
    }
    pegameiosDePagamentos() {
        var Objecto = this;
        this.valorgeralcompra = this.valorCompra;
        alert(this.idDaSessao);
        PagSeguroDirectPayment.setSessionId(this.idDaSessao);
        PagSeguroDirectPayment.onSenderHashReady(function (response) {
            if (response.status == 'error') {
                console.log(response);
                alert(response.message);
                return false;
            }
            var hash = response.senderHash;
            $.ajax({
                url: Objecto.executionUrl + Objecto.urlSetaHash + hash,
                context: Objecto,
                dataType: 'json',
                type: "POST",
                data: {},
                async: false,
                success: function (data) {
                    Objecto.hashusuario = data.hash;
                    if (!data.success) {
                        alert('Erro ao setar hash');
                    }
                },
                beforeSend: function () {
                }, error: function (mensagem) {
                    alert('Erro ao setar hash')
                }
            });
        });
        var tipospagamentos = new Array();
        tipospagamentos['BOLETO'];
        tipospagamentos['CREDIT_CARD'];
        tipospagamentos['ONLINE_DEBIT'];
        PagSeguroDirectPayment.getPaymentMethods({
            amount: Objecto.valorCompra,
            async: false,
            success: function (response) {
                Objecto.meiosDePagamentos = response.paymentMethods;
            },
            error: function (response) {
                console.log(response);
            },
            complete: function (response) {
            }
        });
    }
    recuperaPagamentos() {
        if (this.tipopagamento.name == "BOLETO") {
            return this.tipopagamento;
        }
        if (this.tipopagamento.name == "CREDIT_CARD") {
            var opcoescartoes = this.tipopagamento.options;
            var arr = $.map(opcoescartoes, function (el) {
                return el
            });
            this.retornoPagamento = arr;
        }
        if (this.tipopagamento.name == "ONLINE_DEBIT") {
            var opcoesdebito = this.tipopagamento.options;
            var arr = $.map(opcoesdebito, function (el) {
                return el
            });
            this.retornoPagamento = arr;
        }
    }
    enviaDadosCartao() {
        var Object = this;

        PagSeguroDirectPayment.getBrand({
            cardBin: Object.numeroDoCartao,
            success: function (response) {
                var arr = $.map(response, function (el) {
                    return el
                });
                bandeira = arr[0].name;
                var param = {
                    cardNumber: Object.numeroDoCartao,
                    cvv: Object.numeroCvv,
                    expirationMonth: Object.mesExpiracaoCartao,
                    expirationYear: Object.anoExpiracaoCartao,
                    success: function (response) {
                        var token = Object.tokencreditcard = response.card.token;
                        
                        $.ajax({
                            url: Object.executionUrl + Object.urlSetaTokenCartao + token,
                            dataType: 'json',
                            type: "POST",
                            data: {},
                            async: false,
                            success: function (data) {
                            },
                            error: function (response) {
                                alert(response);
                            }
                        });


                        return false;
                        PagSeguroDirectPayment.getInstallments({
                            amount: Object.valorgeralcompra,
                            brand: bandeira,
                            maxInstallmentNoInterest: Object.maximoParcelasSemJuros,
                            success: function (response) {
                                var parcelas = $.map(response.installments, function (el) {
                                    return el
                                });
                                Object.parcelas = parcelas;
                            },
                            error: function (response) {
                                console.log(response.message);
                                alert(response.message)
                            },
                            complete: function (response) {
                            }

                        });

                    },
                    error: function (response) {
                        console.log(response.message);
                        alert(response.message)
                    },
                    complete: function (response) {
                    }
                }
                param.brand = bandeira;
                tokenparcelas = PagSeguroDirectPayment.createCardToken(param);
            },
            error: function (response) {
                console.log(response.message);
                alert(response.message)
            },
            complete: function (response) {
            }
        });
    }
    escolheParcela() {
        var Object = this;
        $.ajax({
            url: Object.executionUrl + Object.urlFinalizaCompraCartaodeCredito,
            dataType: 'json',
            type: "POST",
            data: {
                quantidadeparcelas: Object.quantidadeParcelas,
                valorparcelas: Object.valorparcelas,
                totalvalor: Object.totalvalor,
                idcompra: idcompra,
                urlretorno: urlretorno,
                valordacompra: Object.valorCompra,
                nome: typeof usuario != 'undefined' ? usuario.nome : nome,
                datadenascimento: typeof usuario != 'undefined' ? usuario.datadenascimento : datadenascimento,
                datadeabertura: typeof usuario != 'undefined' ? usuario.datadeabertura : datadeabertura,
                ddd: typeof usuario != 'undefined' ? usuario.ddd : ddd,
                telefone: typeof usuario != 'undefined' ? usuario.telefone : telefone,
                cpf: typeof usuario != 'undefined' ? usuario.cpf : cpf,
                cnpj: typeof usuario != 'undefined' ? usuario.cnpj : cnpj,
                endereco: typeof usuario != 'undefined' ? usuario.endereco : endereco,
                numero: typeof usuario != 'undefined' ? usuario.numero : numero,
                complemento: typeof usuario != 'undefined' ? usuario.complemento : complemento,
                bairro: typeof usuario != 'undefined' ? usuario.bairro : bairro,
                cidade: typeof usuario != 'undefined' ? usuario.cidade : cidade,
                estado: typeof usuario != 'undefined' ? usuario.estado : estado,
                cep: typeof usuario != 'undefined' ? usuario.cep : cep,
                tipodepessoa: typeof usuario != 'undefined' ? usuario.tipodepessoa : tipodepessoa,
                tokencartaodecredito: Object.tokencreditcard,
                valorcompra: Object.valorgeralcompra,
                itens: itens
            },
            success: function (data) {
                if (data.error) {
                    alert("Houveram erros na requisição");
                } else {
                    Object.idCompra = data.idcompra;
                }
                escondeOverlay();
            },
            beforeSend: function () {
                mostraOverlay('Processando');
            }, error: function (data) {
                mostraMensagem('Houve um erro na requisição por favor refaça a compra ou entre em contato conosco por email ' + configurationjs.emailcontato + ' ou por telefone ' + configurationjs.telefonecontato);
            },
            complete: function () {
            }
        });
    }
    finalizaOpcaoDebitoOnline() {
        var Objeto = this;
        $.ajax({
            url: Objeto.executionUrl + Objeto.urlFinalizaCompraDebitoOnline,
            dataType: 'json',
            type: "POST",
            data: {
                banco: Objeto.bank,
                idcompra: idcompra,
                urlretorno: urlretorno,
                valordacompra: Objeto.valorCompra,
                nome: typeof usuario != 'undefined' ? usuario.nome : nome,
                datadenascimento: typeof usuario != 'undefined' ? usuario.datadenascimento : datadenascimento,
                datadeabertura: typeof usuario != 'undefined' ? usuario.datadeabertura : datadeabertura,
                ddd: typeof usuario != 'undefined' ? usuario.ddd : ddd,
                telefone: typeof usuario != 'undefined' ? usuario.telefone : telefone,
                cpf: typeof usuario != 'undefined' ? usuario.cpf : cpf,
                cnpj: typeof usuario != 'undefined' ? usuario.cnpj : cnpj,
                endereco: typeof usuario != 'undefined' ? usuario.endereco : endereco,
                numero: typeof usuario != 'undefined' ? usuario.numero : numero,
                complemento: typeof usuario != 'undefined' ? usuario.complemento : complemento,
                bairro: typeof usuario != 'undefined' ? usuario.bairro : bairro,
                cidade: typeof usuario != 'undefined' ? usuario.cidade : cidade,
                estado: typeof usuario != 'undefined' ? usuario.estado : estado,
                cep: typeof usuario != 'undefined' ? usuario.cep : cep,
                tipodepessoa: typeof usuario != 'undefined' ? usuario.tipodepessoa : tipodepessoa,
                valorcompra: Object.valorgeralcompra,
                itens: itens
            },
            success: function (data) {
                if (data.error) {
                    alert("Houveram erros na requisição");
                } else {
                    Objeto.dataLink = data.linkDebitoOnline;
                    Objeto.idCompraMaster = data.idcompra;
                }
                escondeOverlay();
            },
            beforeSend: function () {
                mostraOverlay('Processando.');
            }, error: function (data) {
                mostraMensagem('Houve um erro na requisição da escolha do débito online.');
            }
        });
    }
    finalizatransacao() {
        var Object = this;
        var comprafinalizada = false;
        $.ajax({
            url: Object.executionUrl + Object.urlFinalizaTransacao,
            dataType: 'json',
            type: "POST",
            async: false,
            data: {},
            success: function (data) {
                Object.idCompra = data.idcompra;
                comprafinalizada = data;
            },
            beforeSend: function () {
            },
            complete: function () {
            },
            error: function () {
                mostraMensagem('Houve um erro na requisição por favor refaça a compra ou entre em contato conosco por email ' + configurationjs.emailcontato + ' ou por telefone ' + configurationjs.telefonecontato);
            }
        });
        return comprafinalizada;
    }
    finalizaOpcaoBoleto() {
        var Object = this;
        $.ajax({
            url: Object.executionUrl + Object.urlFinalizaCompraBoleto,
            dataType: 'json',
            type: "POST",
            data: {
                idcompra: idcompra,
                urlretorno: urlretorno,
                valordacompra: Object.valorCompra,
                nome: typeof usuario != 'undefined' ? usuario.nome : nome,
                datadenascimento: typeof usuario != 'undefined' ? usuario.datadenascimento : datadenascimento,
                datadeabertura: typeof usuario != 'undefined' ? usuario.datadeabertura : datadeabertura,
                ddd: typeof usuario != 'undefined' ? usuario.ddd : ddd,
                telefone: typeof usuario != 'undefined' ? usuario.telefone : telefone,
                cpf: typeof usuario != 'undefined' ? usuario.cpf : cpf,
                cnpj: typeof usuario != 'undefined' ? usuario.cnpj : cnpj,
                endereco: typeof usuario != 'undefined' ? usuario.endereco : endereco,
                numero: typeof usuario != 'undefined' ? usuario.numero : numero,
                complemento: typeof usuario != 'undefined' ? usuario.complemento : complemento,
                bairro: typeof usuario != 'undefined' ? usuario.bairro : bairro,
                cidade: typeof usuario != 'undefined' ? usuario.cidade : cidade,
                estado: typeof usuario != 'undefined' ? usuario.estado : estado,
                cep: typeof usuario != 'undefined' ? usuario.cep : cep,
                tipodepessoa: typeof usuario != 'undefined' ? usuario.tipodepessoa : tipodepessoa,
                valorcompra: Object.valorgeralcompra,
                itens: itens
            },
            success: function (data) {
                Object.dataLink = data.linkboleto;
                Object.idCompraMaster = data.idcompra;
                if (data.error) {
                    alert("Houveram erros na requisição");
                } else {
                }
                escondeOverlay();
            },
            beforeSend: function () {
                mostraOverlay('Gerando boleto aguarde');
            }, error: function (data) {
                mostraMensagem('Houve um erro na requisição por favor refaça a compra ou entre em contato conosco por email ' + configurationjs.emailcontato + ' ou por telefone ' + configurationjs.telefonecontato);
            },
            complete: function () {
            }
        });
    }
}