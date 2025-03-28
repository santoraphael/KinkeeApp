using Miscellaneous.ELOCalulate;
using Mongo.BSN;
using Mongo.DAL;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using Mongo.Models.Afiliados;
using Mongo.Models.Compra;
using Mongo.Services;
using MongoDB.Bson;
using Plataforma.Helper;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace Plataforma.Controllers
{
    [Authorize]
    public class MinhaContaController : Controller
    {
        UserBSN Usuario = new UserBSN();
        LocationHelper locationHelper = new LocationHelper();

        // GET: MinhaConta
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TrocarKoins()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = Usuario.GetUserByUsuario(u);

            ProcessSwitcher processSwitcher = new ProcessSwitcher();
            ViewBag.Saldo = processSwitcher.GetSaldoWallet(usuarioLogado.Id);

            return View();
        }

        public ActionResult Loja()
        {
            return View();
        }

        public ActionResult NotificacaoTransacao()
        {
            return View();
        }

        public ActionResult Configuracoes()
        {
            return View();
        }

        public ActionResult ConfiguracoesAvancadas()
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);


            if (varUsuario.Endereco == null)
            {
                Mongo.Models.Endereco endereco = new Mongo.Models.Endereco();

                varUsuario.Endereco = endereco;
            }

            ViewBag.Usuario = varUsuario;

            return View();
        }

        public ActionResult SalvarLocation(string Pais, string Estado, string Cidade, string Latitude, string Longitude, string IPv4)
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            if (varUsuario.Endereco == null)
            {
                Mongo.Models.Endereco endereco = new Mongo.Models.Endereco();

                varUsuario.Endereco = endereco;
            }

            if (String.IsNullOrEmpty(Pais))
            {
                Pais = "None";
            }
            if (String.IsNullOrEmpty(Estado))
            {
                Estado = "None";
            }
            if (String.IsNullOrEmpty(Cidade))
            {
                Cidade = "None";
            }


            varUsuario.Endereco.Pais = Pais;
            varUsuario.Endereco.Estado = Estado;
            varUsuario.Endereco.Cidade = Cidade;
            varUsuario.Endereco.Latitude = Latitude;
            varUsuario.Endereco.Longitude = Longitude;
            varUsuario.Endereco.IPv4 = IPv4;

            if (!String.IsNullOrEmpty(Pais) && !String.IsNullOrEmpty(Estado) && !String.IsNullOrEmpty(Cidade))
            {
                Usuario.EditarUsuario(varUsuario);
            }

            return View();
        }

        public ActionResult SalvarLocationDefault(string Pais, string Estado, string Cidade, string Latitude, string Longitude, string IPv4)
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
            if (varUsuario.Endereco == null)
            {
                Mongo.Models.Endereco endereco = new Mongo.Models.Endereco();

                varUsuario.Endereco = endereco;
            }

            if (String.IsNullOrEmpty(Pais))
            {
                Pais = "None";
            }
            if (String.IsNullOrEmpty(Estado))
            {
                Estado = "None";
            }
            if (String.IsNullOrEmpty(Cidade))
            {
                Cidade = "None";
            }


            if (String.IsNullOrEmpty(varUsuario.Endereco.Pais) && String.IsNullOrEmpty(varUsuario.Endereco.Estado) && String.IsNullOrEmpty(varUsuario.Endereco.Cidade))
            {
                varUsuario.Endereco.Pais = Pais;
                varUsuario.Endereco.Estado = Estado;
                varUsuario.Endereco.Cidade = Cidade;
                varUsuario.Endereco.Latitude = Latitude;
                varUsuario.Endereco.Longitude = Longitude;
                varUsuario.Endereco.IPv4 = IPv4;

                if (!String.IsNullOrEmpty(Pais) && !String.IsNullOrEmpty(Estado) && !String.IsNullOrEmpty(Cidade))
                {
                    Usuario.EditarUsuario(varUsuario);
                }
            }

            else
            {
                varUsuario.Endereco.Latitude = Latitude;
                varUsuario.Endereco.Longitude = Longitude;
                varUsuario.Endereco.IPv4 = IPv4;

                if (!String.IsNullOrEmpty(Pais) && !String.IsNullOrEmpty(Estado) && !String.IsNullOrEmpty(Cidade))
                {
                    Usuario.EditarUsuario(varUsuario);
                }
            }

            return View();
        }

        public ActionResult SalvarNomedeUsuario(string Usernsame)
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            varUsuario.Usuario = Usernsame;

            Usuario.EditarUsuario(varUsuario);

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _AssinaturaPartial()
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            //if(varUsuario.Genero.Replace(" ","") == "1")
            //{
            //    return View();
            //}
            //else
            //{
            //    //string referrer = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            //    return Redirect(System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
            //}

            if (varUsuario.ContaGold == true && varUsuario.PerfilVerificado == true)
            {
                ViewBag.ContaCompleta = true;
            }
            else
            {
                ViewBag.ContaCompleta = false;
            }

            ViewBag.TipoSugar = varUsuario.TipoSugar;

            ViewBag.ContaGold = varUsuario.ContaGold;
            ViewBag.PerfilVerificado = varUsuario.PerfilVerificado;

            return View();
        }

        public ActionResult _ModalPremiumPartial()
        {
            var assinaturaValida = ValidarAssinatura();

            if(assinaturaValida)
            {
                return View("Assinatura");
            }

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
            PaymentViewModel model = new PaymentViewModel();


            List<MeioPagamento> listaMeios = new List<MeioPagamento>();
            //listaMeios.Add(MeioPagamento.BoletoBancario);
            listaMeios.Add(MeioPagamento.CartaoCredito);
            //listaMeios.Add(MeioPagamento.TransferenciaBancaria);


            if (varUsuario.Genero == "Homem")
            {
                model.valorPromocionalSemestralMensal = "19,95";
                model.valorCheioSemDesconto = "239,40";

                

                PlanModel planoMensal = new PlanModel();
                planoMensal.descricaoNomeValorPlano = "Mensal - R$ 39,90";
                planoMensal.valorPlanoReais = "39";
                planoMensal.valorPlanoCentavos = "90";
                planoMensal.valorPoints = "1189";
                planoMensal.descricaoParcelamento = "";
                planoMensal.meioPagamentos = listaMeios;
                planoMensal.dataPlanId = "1584932";

                PlanModel planoTrimestral = new PlanModel();
                planoTrimestral.descricaoNomeValorPlano = "Trimestral - R$ 89,77 em 3x de R$ 29,92";
                planoTrimestral.valorPlanoReais = "89";
                planoTrimestral.valorPlanoCentavos = "77";
                planoMensal.valorPoints = "2675";
                planoTrimestral.descricaoParcelamento = "Total: <strong>R$ 89,77</strong> em 3x iguais no cartão de crédito.";
                planoTrimestral.meioPagamentos = listaMeios;
                planoTrimestral.dataPlanId = "1584938";

                PlanModel planoSemestral = new PlanModel();
                planoSemestral.descricaoNomeValorPlano = "Semestral - R$ 119,70 em 6x de R$ 19,95";
                planoSemestral.valorPlanoReais = "119";
                planoSemestral.valorPlanoCentavos = "70";
                planoMensal.valorPoints = "3567";
                planoSemestral.descricaoParcelamento = "Total: <strong>R$ 119,70</strong> em 6x iguais no cartão de crédito.";
                planoSemestral.meioPagamentos = listaMeios;
                planoSemestral.dataPlanId = "1584941";

                model.planoMensal = planoMensal;
                model.planoTrimestral = planoTrimestral;
                model.planoSemestral = planoSemestral;
            }
            else if(varUsuario.Genero == "Mulher")
            {
                model.valorPromocionalSemestralMensal = "9,95";
                model.valorCheioSemDesconto = "119,40";

                PlanModel planoMensal = new PlanModel();
                planoMensal.descricaoNomeValorPlano = "Mensal - R$ 19,90";
                planoMensal.valorPlanoReais = "19";
                planoMensal.valorPlanoCentavos = "90";
                planoMensal.valorPoints = "593";
                planoMensal.descricaoParcelamento = "";
                planoMensal.meioPagamentos = listaMeios;
                planoMensal.dataPlanId = "1584945";

                PlanModel planoTrimestral = new PlanModel();
                planoTrimestral.descricaoNomeValorPlano = "Trimestral - R$ 44,77 em 3x de R$ 14,92";
                planoTrimestral.valorPlanoReais = "14";
                planoTrimestral.valorPlanoCentavos = "92";
                planoMensal.valorPoints = "1334";
                planoTrimestral.descricaoParcelamento = "Total: <strong>R$ 44,77</strong> em 3x iguais no cartão de crédito.";
                planoTrimestral.meioPagamentos = listaMeios;
                planoTrimestral.dataPlanId = "1584953";

                PlanModel planoSemestral = new PlanModel();
                planoSemestral.descricaoNomeValorPlano = "Semestral - R$ 59,70 em 6x de R$ 9,95";
                planoSemestral.valorPlanoReais = "9";
                planoSemestral.valorPlanoCentavos = "95";
                planoMensal.valorPoints = "1779";
                planoSemestral.descricaoParcelamento = "Total: <strong>R$ 59,70</strong> em 6x iguais no cartão de crédito.";
                planoSemestral.meioPagamentos = listaMeios;
                planoSemestral.dataPlanId = "1584956";

                model.planoMensal = planoMensal;
                model.planoTrimestral = planoTrimestral;
                model.planoSemestral = planoSemestral;
            }


            var tempUsuarioAprovado = Convert.ToBoolean(TempData["tempUsuarioAprovado"]);
            TempData.Keep("tempUsuarioAprovado");
            ViewBag.tempUsuarioAprovado = tempUsuarioAprovado;

            ViewBag.ApprovedProfile = varUsuario.ApprovedProfile;

            return View(model);
        }


        public ActionResult _ModalPointsPartial()
        {
            string specifier = "F";
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("pt-BR");

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
            PaymentStoreViewModel model = new PaymentStoreViewModel();
            TransacaoBSN transacaoBSN = new TransacaoBSN();


            List<MeioPagamento> listaMeios = new List<MeioPagamento>();
            //listaMeios.Add(MeioPagamento.BoletoBancario);
            listaMeios.Add(MeioPagamento.CartaoCredito);
            //listaMeios.Add(MeioPagamento.TransferenciaBancaria);

            List<Plataforma.Models.Item> listaDepacotes = new List<Plataforma.Models.Item>();

            foreach (var item in transacaoBSN.GetAllItems())
            {
                Plataforma.Models.Item itemPacote = new Plataforma.Models.Item();
                itemPacote.id = item.id;
                itemPacote.title = item.title;
                itemPacote.unit_price = item.unit_price;
                itemPacote.quantity = item.quantity;
                itemPacote.tangible = item.tangible;

                var numeroDedigitos = item.unit_price.ToString().Count();

                itemPacote.valor_reais = item.unit_price.ToString().Substring(0, numeroDedigitos - 2);
                itemPacote.valor_centavos = item.unit_price.ToString().Substring(numeroDedigitos - 2, 2);

                double cotacao = 3;

                itemPacote.valor_original = ((cotacao * item.quantity) / 100).ToString(specifier, culture);

                listaDepacotes.Add(itemPacote);
            }


            model.listaItens = listaDepacotes;

            return View(model);
        }

        public ActionResult _ModalLovrPartial()
        {
            
            return View();
        }

        public ActionResult _TabPagamentoPointsPartial(int numeroParcelas, double valorTotal, string plan_id)
        {
            var pagamentoPoints = new PagamentoPoints();
            DatingController datingController = new DatingController();


            switch (plan_id)
            {
                case "1584932":
                    pagamentoPoints.ValorItem = 1189;
                    break;

                case "1584938":
                    pagamentoPoints.ValorItem = 2675;
                    break;

                case "1584941":
                    pagamentoPoints.ValorItem = 3567;
                    break;

                case "1584945":
                    pagamentoPoints.ValorItem = 593;
                    break;

                case "1584953":
                    pagamentoPoints.ValorItem = 1334;
                    break;

                case "1584956":
                    pagamentoPoints.ValorItem = 1779;
                    break;
            }



            pagamentoPoints.ValorSaldo = Convert.ToInt32(datingController.PegarSaldo());
            
            pagamentoPoints.ValorFalta = pagamentoPoints.ValorItem - pagamentoPoints.ValorSaldo;

            if(pagamentoPoints.ValorFalta <= 0 )
            {
                pagamentoPoints.ValorFalta = 0;
                ViewBag.AvisoCreditosFaltantes = "";
            }
            else if(pagamentoPoints.ValorFalta > 0)
            {
                ViewBag.AvisoCreditosFaltantes = "* Seu Saldo de Points é insuficiente para adquirir este plano.";
            }


            pagamentoPoints.plan_id = plan_id;

            var valor = pagamentoPoints.ValorItem;
            var texto = " Valor total " + pagamentoPoints.ValorItem.ToString("C");


            return View(pagamentoPoints);
        }

        public ActionResult _TabPagamentoCartaoCreditoPartial(int numeroParcelas, double valorTotal, string plan_id)
        {
            var pagamentoCartao = new PagamentoCartao();
            pagamentoCartao.numeroParcelas = numeroParcelas;
            pagamentoCartao.valorTotal = valorTotal;
            pagamentoCartao.plan_id = plan_id;
            List<SelectListItem> parcelasList = new List<SelectListItem>();

            for (int i = 0; i < pagamentoCartao.numeroParcelas; i++)
            {
                var valor = pagamentoCartao.valorTotal / (i + 1);
                var texto = " Parcelado em "+(i+1)+"x de R$ "+ valor.ToString("C");

                SelectListItem parcela = new SelectListItem();

                if (i == (pagamentoCartao.numeroParcelas-1))
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;
                    parcela.Selected = true;
                }
                else
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;
                    
                }
                
                parcelasList.Add(parcela);
            }


            //pagamentoCartao.parcelas = new SelectList(parcelasList, "Value", "Text", pagamentoCartao.numeroParcelas.ToString());

            return View(pagamentoCartao);
        }

        public ActionResult _TabPagamentoCompraCartaoCreditoPartial(string idPacote)
        {
            TransacaoBSN transacaoBSN = new TransacaoBSN();
            var item_pacote = transacaoBSN.GetItemById(new ObjectId(idPacote));

            ViewBag.NomePacote = item_pacote.title;

            var pagamentoCartao = new PagamentoCartao();
            pagamentoCartao.numeroParcelas = 6;
            pagamentoCartao.valorTotal = item_pacote.unit_price;
            pagamentoCartao.plan_id = item_pacote.id.ToString();
            
            
            List<SelectListItem> parcelasList = new List<SelectListItem>();

            for (int i = 0; i < pagamentoCartao.numeroParcelas; i++)
            {
                var valor = (pagamentoCartao.valorTotal / (i + 1)/100);
                var texto = " Parcelado em " + (i + 1) + "x de R$ " + valor.ToString("F");

                SelectListItem parcela = new SelectListItem();

                if (i == (pagamentoCartao.numeroParcelas - 1))
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;
                    parcela.Selected = true;
                }
                else
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;

                }

                parcelasList.Add(parcela);
            }

            pagamentoCartao.numeroParcelas = 1;
            ViewData["parcelas"] = new SelectList(parcelasList, "Value", "Text", 1);

            return View(pagamentoCartao);
        }


        public ActionResult _TabPagamentoCompraBoletoCreditoPartial(string idPacote)
        {
            TransacaoBSN transacaoBSN = new TransacaoBSN();
            var item_pacote = transacaoBSN.GetItemById(new ObjectId(idPacote));

            ViewBag.NomePacote = item_pacote.title;

            var pagamentoCartao = new PagamentoCartao();
            pagamentoCartao.numeroParcelas = 6;
            pagamentoCartao.valorTotal = item_pacote.unit_price;
            pagamentoCartao.plan_id = item_pacote.id.ToString();


            List<SelectListItem> parcelasList = new List<SelectListItem>();

            for (int i = 0; i < pagamentoCartao.numeroParcelas; i++)
            {
                var valor = (pagamentoCartao.valorTotal / (i + 1) / 100);
                var texto = " Parcelado em " + (i + 1) + "x de R$ " + valor.ToString("F");

                SelectListItem parcela = new SelectListItem();

                if (i == (pagamentoCartao.numeroParcelas - 1))
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;
                    parcela.Selected = true;
                }
                else
                {
                    parcela.Value = (i + 1).ToString();
                    parcela.Text = texto;

                }

                parcelasList.Add(parcela);
            }

            pagamentoCartao.numeroParcelas = 1;
            ViewData["parcelas"] = new SelectList(parcelasList, "Value", "Text", 1);

            return View(pagamentoCartao);
        }
        

        public ActionResult ComprarAssinatura(PagamentoCartao pagamentoCartao)
        {
            Assinatura assinatura = new Assinatura();
            CartaoDTO cartaoDTO = new CartaoDTO();

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            if(varUsuario.ContaGold)
            {
                return View("Assinatura");
            }

            cartaoDTO.card_expiration_date = pagamentoCartao.mesValidadeCartao+ pagamentoCartao.anoValidadeCartao;
            cartaoDTO.card_number = pagamentoCartao.numeroCartaoCredito;
            cartaoDTO.card_cvv = pagamentoCartao.codigoSeguranca;
            cartaoDTO.card_holder_name = pagamentoCartao.nomeTitularCartao;

            String ERRO = "";

            try
            {
                var cartaoCriado = assinatura.CriarCartao(cartaoDTO, varUsuario.Id);
                if(cartaoCriado.valid)
                {
                    AssinaturaDTO assinaturaDTO = new AssinaturaDTO();
                    assinaturaDTO.card_id = cartaoCriado.id_cartao;

                    var address = assinatura.RequestConsultaCEP(pagamentoCartao.cep);

                    assinaturaDTO.customer.address = address;

                    assinaturaDTO.customer.document_number = pagamentoCartao.numeroDocumento;
                    assinaturaDTO.customer.email = varUsuario.Email;
                    assinaturaDTO.customer.name = varUsuario.Name +" "+ varUsuario.Lastname;
                    assinaturaDTO.customer.phone.ddd = varUsuario.NumeroTelefone.Substring(0, 2);
                    assinaturaDTO.customer.phone.number = varUsuario.NumeroTelefone.Substring(2);
                    assinaturaDTO.payment_method = "credit_card";
                    assinaturaDTO.plan_id = pagamentoCartao.plan_id;
                    assinaturaDTO.postback_url = "http://requestb.in/zyn5obzy";

                    var retorno = assinatura.FazerAssinatura(assinaturaDTO, varUsuario.Id);

                    if(!string.IsNullOrEmpty(retorno.subscriptionId))
                    {
                        AfiliadosBSN afiliadosBSN = new AfiliadosBSN();

                        varUsuario.ContaGold = true;
                        varUsuario.DataVencimentoGold = retorno.current_period_end;
                        varUsuario.NomesPerfil = NomesPerfis.comassinatura.ToString();

                        if(!varUsuario.ApprovedProfile)
                        {
                            afiliadosBSN.GerarOperacaoOperacao(null, null, retorno.Id.ToString(), TipoOperacao.Item, NomesTiposGanhos.ValorPorDaddyAprovado, varUsuario.InvitedBy, varUsuario.Id);
                        }

                        varUsuario.ApprovedProfile = true;
                        Usuario.EditarUsuario(varUsuario);
                        
                        afiliadosBSN.GerarOperacaoOperacao(pagamentoCartao.plan_id, null, retorno.Id.ToString(), TipoOperacao.Assinatura, NomesTiposGanhos.PercentualCompras, varUsuario.InvitedBy, varUsuario.Id);

                       var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuario.Id);
                        EloHelper.AtualizaMMR(varUsuario.Id, varUsuario.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Importante, ELOVE.Acao.Positiva);

                        return RedirectToAction("Index", "Dating");
                    }
                    else
                    {
                        return View("RetornoAssinatura");
                    }

                }
                else
                {
                    return View("RetornoAssinatura");
                }
            }
            catch
            {
                return View("RetornoAssinatura");
            }
        }

        public ActionResult ComprarAssinaturaPoints(PagamentoPoints pagamentoPoints)
        {
            Assinatura assinatura = new Assinatura();
            CartaoDTO cartaoDTO = new CartaoDTO();

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            if (varUsuario.ContaGold)
            {
                return View("Assinatura");
            }

            String ERRO = "";

            try
            {
                SwitcherDAL switcherDAL = new SwitcherDAL();
                var wallet = switcherDAL.GetWalletByDono(varUsuario.Id);

                if(wallet.Saldo >= pagamentoPoints.ValorItem)
                {
                    ProcessSwitcher processSwitcher = new ProcessSwitcher();
                    processSwitcher.DebitarKoins(varUsuario.Id, "Kinkee", pagamentoPoints.ValorItem, TransactionType.Debit);


                    var dataVencimentoPlano = DateTime.Now;

                    switch (pagamentoPoints.plan_id)
                    {
                        case "1584932":
                            dataVencimentoPlano = DateTime.Now.AddDays(30);
                            break;

                        case "1584938":
                            dataVencimentoPlano = DateTime.Now.AddDays(90);
                            break;

                        case "1584941":
                            dataVencimentoPlano = DateTime.Now.AddDays(180);
                            break;

                        case "1584945":
                            dataVencimentoPlano = DateTime.Now.AddDays(30);
                            break;

                        case "1584953":
                            dataVencimentoPlano = DateTime.Now.AddDays(90);
                            break;

                        case "1584956":
                            dataVencimentoPlano = DateTime.Now.AddDays(180);
                            break;
                    }

                    varUsuario.ContaGold = true;
                    varUsuario.DataVencimentoGold = dataVencimentoPlano;
                    Usuario.EditarUsuario(varUsuario);

                    var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuario.Id);
                    EloHelper.AtualizaMMR(varUsuario.Id, varUsuario.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Importante, ELOVE.Acao.Positiva);
                }

                return RedirectToAction("Index", "Dating");
            }
            catch
            {
                return View("RetornoAssinatura");
            }
        }

        [ValidateAntiForgeryToken()]
        public ActionResult ComprarItemCartao(PagamentoCartao pagamentoCartao)
        {
            Assinatura assinatura = new Assinatura();
            CartaoDTO cartaoDTO = new CartaoDTO();

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            cartaoDTO.card_expiration_date = pagamentoCartao.mesValidadeCartao + pagamentoCartao.anoValidadeCartao;
            cartaoDTO.card_number = pagamentoCartao.numeroCartaoCredito;
            cartaoDTO.card_cvv = pagamentoCartao.codigoSeguranca;
            cartaoDTO.card_holder_name = pagamentoCartao.nomeTitularCartao;

            String ERRO = "";

            try
            {
                var cartaoCriado = assinatura.CriarCartao(cartaoDTO, varUsuario.Id);
                
                if (cartaoCriado.valid)
                {
                    Compra criarCompra = new Compra();
                    CompraModel compraModel = new CompraModel();

                    TransacaoBSN transacaoBSN = new TransacaoBSN();
                    var item_pacote = transacaoBSN.GetItemById(new ObjectId(pagamentoCartao.plan_id));

                    compraModel.amount = item_pacote.unit_price;
                    compraModel.card_id = cartaoCriado.id_cartao;

                    compraModel.customer.external_id = varUsuario.Id.ToString();
                    compraModel.customer.name = varUsuario.Name + " " + varUsuario.Lastname;
                    compraModel.customer.type = "individual";
                    compraModel.customer.country = "br";
                    compraModel.customer.email = varUsuario.Email;
                   
                    Mongo.Models.Compra.Document document = new Mongo.Models.Compra.Document();
                    document.type = "cpf";
                    document.number = pagamentoCartao.numeroDocumento;
                    compraModel.customer.documents.Add(document);
                    compraModel.customer.phone_numbers.Add("+5511999998888");
                    compraModel.customer.birthday = varUsuario.DataAniversario.ToString("yyyy-MM-dd");


                    compraModel.billing.name = pagamentoCartao.nomeTitularCartao;
                    var address = assinatura.RequestConsultaCEP(pagamentoCartao.cep);
                    compraModel.billing.address.country = "br";
                    compraModel.billing.address.state = address.state;
                    compraModel.billing.address.city = address.city;
                    compraModel.billing.address.neighborhood = address.neighborhood;
                    compraModel.billing.address.street = address.street;
                    compraModel.billing.address.street_number = "100";
                    compraModel.billing.address.zipcode = address.zipcode;

                    Mongo.Models.Compra.Item item = new Mongo.Models.Compra.Item();

                    item.id = item_pacote.id;
                    item.title = item_pacote.title;
                    item.unit_price = item_pacote.unit_price;
                    item.quantity = 1;
                    item.tangible = false;
                    compraModel.items.Add(item);

                    var pago = criarCompra.CriarNovaTransacao(compraModel);

                    if(pago)
                    {
                        ProcessSwitcher processSwitcher = new ProcessSwitcher();
                        processSwitcher.CreditarKoins(varUsuario.Id, "Kinkee", item_pacote.quantity, TransactionType.Credit);
                        EloHelper.AtualizaMMR(varUsuario.Id, varUsuario.ContaGold, 0, ELOVE.TipoAcao.Importante, ELOVE.Acao.Positiva);
                    }
                }

                return View("Index");
            }
            catch(Exception ex)
            {
                ex.Message.ToString();

                return View("Index");
            }
        }

        [ValidateAntiForgeryToken()]
        public ActionResult ComprarItemBoleto(PagamentoCartao pagamentoCartao)
        {
            Assinatura assinatura = new Assinatura();

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            String ERRO = "";

            try
            {

                Compra criarCompra = new Compra();
                CompraBoletoModel compraModel = new CompraBoletoModel();

                TransacaoBSN transacaoBSN = new TransacaoBSN();
                var item_pacote = transacaoBSN.GetItemById(new ObjectId(pagamentoCartao.plan_id));

                compraModel.amount = item_pacote.unit_price;
                compraModel.payment_method = "boleto";

                compraModel.customer.external_id = varUsuario.Id.ToString();
                compraModel.customer.name = varUsuario.Name + " " + varUsuario.Lastname;
                compraModel.customer.type = "individual";
                compraModel.customer.country = "br";
                compraModel.customer.email = varUsuario.Email;

                Mongo.Models.Compra.Document document = new Mongo.Models.Compra.Document();
                document.type = "cpf";
                document.number = pagamentoCartao.numeroDocumento;
                compraModel.customer.documents.Add(document);
                compraModel.customer.phone_numbers.Add("+5511999998888");
                compraModel.customer.birthday = varUsuario.DataAniversario.ToString("yyyy-MM-dd");


                var boleto = criarCompra.CriarNovaTransacaoBoleto(compraModel);

                RetornoPagamentoBoleto retornoPagamentoBoleto = new RetornoPagamentoBoleto();
                var numeroDedigitos = item_pacote.unit_price.ToString().Count();
                retornoPagamentoBoleto.valor_reais = item_pacote.unit_price.ToString().Substring(0, numeroDedigitos - 2);
                retornoPagamentoBoleto.valor_centavos = item_pacote.unit_price.ToString().Substring(numeroDedigitos - 2, 2);

                retornoPagamentoBoleto.dia_vencimento = Convert.ToDateTime(boleto.boleto_expiration_date).Day.ToString();
                retornoPagamentoBoleto.mes_vencimento = Convert.ToDateTime(boleto.boleto_expiration_date).Month.ToString();
                retornoPagamentoBoleto.codigo_barra_boleto = boleto.boleto_barcode;

                retornoPagamentoBoleto.url_boleto = boleto.boleto_url;

                return View("_RetornoCompraBoletoPartial", retornoPagamentoBoleto);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();

                return View("_RetornoCompraPartial");
            }
        }

        public bool ConsultarAssinatura(string subscription_id)
        {
            Assinatura assinatura = new Assinatura();
            var assin = assinatura.ConsultarAssinatura(subscription_id);

            if(assin.status != null)
            {
                if (assin.status.ToUpper() == ("paid").ToUpper())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool ValidarAssinatura()
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            if(varUsuario.DataVencimentoGold > DateTime.Now && varUsuario.ContaGold)
            {
                return true;
            }
            else
            {
                SubscriptionDAL subscriptionDAL = new SubscriptionDAL();
                var subscription = subscriptionDAL.GetAssinaturaByUserId(varUsuario.Id);
                var valido = false;


                if (subscription != null)
                {
                    valido = ConsultarAssinatura(subscription.subscriptionId);
                }
                

                if (valido)
                {
                    Assinatura assinatura = new Assinatura();
                    var assin = assinatura.ConsultarAssinatura(subscription.subscriptionId);

                    varUsuario.ContaGold = valido;
                    varUsuario.DataVencimentoGold = assin.current_period_end;
                    Usuario.EditarUsuario(modelUsuario);
                    return valido;
                }
                else
                {
                    varUsuario.ContaGold = valido;
                    Usuario.EditarUsuario(varUsuario);
                }

                return valido;
            }
        }

        public AssinaturaModel GetAssinaturaByID(ObjectId id_subscription)
        {
            SubscriptionDAL subscriptionDAL = new SubscriptionDAL();
            return subscriptionDAL.GetAssinaturaByID(id_subscription);
        }

        public ActionResult _RetornoCompraPartial()
        {
            return View();
        }

        public ActionResult _RetornoCompraBoletoPartial()
        {
            return View();
        }

        public ActionResult RetornoAssinatura()
        {
            return View();
        }

        public ActionResult _ComprarKoinsPartial()
        {
            return View();
        }

        public ActionResult Assinatura()
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            ViewBag.ContaGold = varUsuario.ContaGold;
            ViewBag.AssinaturaValida = ValidarAssinatura();

            return View();
        }

        public ActionResult Privacidade()
        {
            var listBlockedUseres = Usuario.GetListUsersBlocked();

            List<UserModel> usuariosBloqueados = new List<UserModel>();

            foreach (var item in listBlockedUseres)
            {
                var userBlocked = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(item.UserBlocked));
                usuariosBloqueados.Add(userBlocked);
            }


            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            ViewBag.PermiteDivulgacaoPerfil = false;
            if (varUsuario.PermiteDivulgacaoPerfil)
            {
                ViewBag.PermiteDivulgacaoPerfil = varUsuario.PermiteDivulgacaoPerfil;
            }
            ViewBag.usuariosBloqueados = usuariosBloqueados;


            List<UserModel> usuariosSolicitacoes = new List<UserModel>();
            List<UserModel> usuariosPermitidos = new List<UserModel>();

            if (varUsuario.PedidosVisualizarFtPrivadas != null)
            {
                foreach (var item in varUsuario.PedidosVisualizarFtPrivadas)
                {
                    if (item.Aprovado)
                    {
                        var userSolicitacao = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(item.idUsuario));
                        usuariosPermitidos.Add(userSolicitacao);
                    }
                    else
                    {
                        var userSolicitacao = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(item.idUsuario));
                        usuariosSolicitacoes.Add(userSolicitacao);
                    }
                }
            }

            ViewBag.usuariosPermitidos = usuariosPermitidos;
            ViewBag.usuariosSolicitacoes = usuariosSolicitacoes;

            return View();
        }

        public ActionResult AceitaSolicitacao(string Solicitacao)
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            varUsuario.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == Solicitacao).FirstOrDefault().Aprovado = true;

            UsuarioHelper.EditarUsuario(varUsuario);


            var usuarioMensagem = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(Solicitacao));
            SharedViewModel model = new SharedViewModel();
            MessageViewModel mensagemModel = new MessageViewModel();

            mensagemModel.Mensagem = "MENSAGEM AUTOMATICA: Oi! Eu dei acesso as minhas fotos privadas, va ate o meu perfil para ve-las.";
            mensagemModel.Para = "/Perfil/" + usuarioMensagem.Usuario;

            model.MessageViewModel = mensagemModel;
            DatingController dating = new DatingController();

            dating.EnviarMensagem(model);

            return View();
        }

        public ActionResult RetiraPermissao(string Solicitacao)
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            var permissao = varUsuario.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == Solicitacao).FirstOrDefault();
            varUsuario.PedidosVisualizarFtPrivadas.Remove(permissao);

            UsuarioHelper.EditarUsuario(varUsuario);

            return View();
        }

        public ActionResult RemoveBlocked(string BlockedUser)
        {
            UserModel modelUsuario = new UserModel();

            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

            UsuarioHelper.RemoveBlocked(varUsuario.Id.ToString(), BlockedUser);

            return View();
        }

        public ActionResult AddRemoveDivulgacaoPerfil(bool permissao)
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = Usuario.GetUserByUsuario(modelUsuario);


            varUsuario.PermiteDivulgacaoPerfil = permissao;

            UsuarioHelper.SalvarUsuarioDivulgacao(varUsuario);

            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }

        public ActionResult EditarPerfil()
        {
            SharedViewModel model = new SharedViewModel();
            PerfilViewModel modelPerfil = new PerfilViewModel();
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                var information = Usuario.GetInformationByUserId(varUsuario.Id);


                modelPerfil.Name = varUsuario.Name;
                modelPerfil.Lastname = varUsuario.Lastname;
                modelPerfil.Descricao_Curta = varUsuario.Descricao;
                modelPerfil.Descricao_Longa = varUsuario.Genero == "Homem" ? (String.IsNullOrEmpty(information.DescricaoGenerosidade) ? "" : information.DescricaoGenerosidade) : (String.IsNullOrEmpty(information.DescricaoMotivoBaby) ? "" : information.DescricaoMotivoBaby);

                modelPerfil.Usuario = varUsuario.Usuario;
                modelPerfil.ContaGold = varUsuario.ContaGold;
                modelPerfil.ContaSelectBlack = varUsuario.ContaSelectBlack;
                modelPerfil.PerfilVerificado = varUsuario.PerfilVerificado;
                modelPerfil.PerfilTop = varUsuario.PerfilTop;
                modelPerfil.PromotionalCode = varUsuario.CodigoConvite;
                modelPerfil.Genero = varUsuario.Genero;
                modelPerfil.TipoSugar = varUsuario.TipoSugar;
                modelPerfil.TenhoInteresse = varUsuario.TenhoInteresse;
                modelPerfil.Adm = varUsuario.Adm;

                modelPerfil.Idade = CommonHelper.CalculateAge(varUsuario.DataAniversario).ToString();
                modelPerfil.OrientacaoSexual = information.OrientacaoSexual;
                modelPerfil.Profissao = information.NomeGrupoProfissao + ", " + information.NomeProfissao;
                modelPerfil.EstadoCivil = information.EstadoCivil;
                modelPerfil.Signo = information.Signo;
                modelPerfil.Etnia = information.Etnia;
                modelPerfil.Cabelos = information.Cabelos;
                modelPerfil.Olhos = information.Olhos;
                modelPerfil.Altura = information.Altura;
                modelPerfil.Corpo = information.Corpo;
                modelPerfil.Fuma = information.Fuma;
                modelPerfil.Bebe = information.Bebe;

                CreateProfileModel createProfileModel = new CreateProfileModel();

                createProfileModel.AceitoOsTermos = information.AceitoOsTermos;
                createProfileModel.Admin1_str_code = information.Admin1_str_code;
                createProfileModel.Admin1_str_name = information.Admin1_str_name;
                createProfileModel.ApprovedProfile = information.ApprovedProfile;
                createProfileModel.Country_str_code = information.Country_str_code;
                createProfileModel.DescricaoGenerosidade = information.DescricaoGenerosidade;
                createProfileModel.DescricaoMotivoBaby = information.DescricaoMotivoBaby;
                createProfileModel.DescricaoPatrimonio = information.DescricaoPatrimonio;
                createProfileModel.DescricaoRelacionamento = information.DescricaoRelacionamento;
                createProfileModel.DescricaoRenaMensal = information.DescricaoRenaMensal;
                createProfileModel.DesejoReceberComunicacao = information.DesejoReceberComunicacao;
                createProfileModel.DetalheGenerosidade = information.DetalheGenerosidade;
                createProfileModel.DetalheMotivoBaby = information.DetalheMotivoBaby;
                createProfileModel.DisponibilidadeViagens = information.DisponibilidadeViagens;
                createProfileModel.Feature_int_id = information.Feature_int_id;
                createProfileModel.Feature_str_name = information.Feature_str_name;
                createProfileModel.Finalizado = information.Finalizado;
                createProfileModel.Foto = information.Foto;
                createProfileModel.GeneroUsuario = information.GeneroUsuario;
                createProfileModel.IdGenerosidade = information.IdGenerosidade;
                createProfileModel.IdGrupoProfissao = information.IdGrupoProfissao;
                createProfileModel.IdMotivoBaby = information.IdMotivoBaby;
                createProfileModel.IdPatrimonio = information.IdPatrimonio;
                createProfileModel.IdProfissao = information.IdProfissao;
                createProfileModel.IdRenaMensal = information.IdRenaMensal;
                createProfileModel.LinkFacebook = information.LinkFacebook;
                createProfileModel.LinkInstagram = information.LinkInstagram;
                createProfileModel.NomeGrupoProfissao = information.NomeGrupoProfissao;
                createProfileModel.NomeProfissao = information.NomeProfissao;
                createProfileModel.NomeStatusRelacionamento = information.NomeStatusRelacionamento;
                createProfileModel.NomeUsuario = information.NomeUsuario;
                createProfileModel.PrimeiroNome = information.PrimeiroNome;
                createProfileModel.ProfileCreated = information.ProfileCreated;
                createProfileModel.StatusRelacionamento = information.StatusRelacionamento;

                modelPerfil.createProfileModel = createProfileModel;

                ViewData["Countries"] = new SelectList(locationHelper.ListaPaises(), "Country_str_code", "Country_str_name");


                if (varUsuario.imagemPerfil != null)
                {
                    modelPerfil.imagemPerfil = varUsuario.imagemPerfil.Replace("\\", "/");
                }

                model.PerfilViewModel = modelPerfil;

                ViewBag.InserirImagemPerfil = false;
                ViewBag.InserirCidade = false;

                if (varUsuario.imagemPerfil == " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png" || varUsuario.imagemPerfil == "http://Kinkee.me/pulsar/Kinkee/assets/images/modules/sobre/profile.png")
                {
                    ViewBag.InserirImagemPerfil = true;
                }
                else
                {
                    ViewBag.InserirImagemPerfil = false;
                }
            }
            catch
            {

            }


            return View(model);
        }

        [HttpPost]
        public ActionResult EditarPerfil(SharedViewModel model)
        {
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                var information = Usuario.GetInformationByUserId(varUsuario.Id);


                varUsuario.Name = model.PerfilViewModel.Name;
                varUsuario.Lastname = model.PerfilViewModel.Lastname;
                varUsuario.Descricao = model.PerfilViewModel.Descricao_Curta;
                
                if(varUsuario.Genero == "Homem")
                {
                    information.DescricaoGenerosidade = model.PerfilViewModel.Descricao_Longa;
                }
                else
                {
                    information.DescricaoMotivoBaby = model.PerfilViewModel.Descricao_Longa;
                }

                Usuario.AlterUserInformation(information);
                Usuario.EditarUsuario(varUsuario);
            }
            catch
            {

            }

            return RedirectToAction("EditarPerfil", "MinhaConta");
        }


        [HttpPost]
        public ActionResult EditarPerfilModal(SharedViewModel model)
        {
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                var information = Usuario.GetInformationByUserId(varUsuario.Id);


                varUsuario.Name = model.PerfilViewModel.Name;
                varUsuario.Lastname = model.PerfilViewModel.Lastname;
                varUsuario.Descricao = model.PerfilViewModel.Descricao_Curta;

                if (varUsuario.Genero == "Homem")
                {
                    information.DescricaoGenerosidade = model.PerfilViewModel.Descricao_Longa;
                }
                else
                {
                    information.DescricaoMotivoBaby = model.PerfilViewModel.Descricao_Longa;
                }

                Usuario.AlterUserInformation(information);
                Usuario.EditarUsuario(varUsuario);
            }
            catch
            {

            }

            return RedirectToAction("EditarPerfil", "MinhaConta");
        }


        [HttpPost]
        public ActionResult EditarFotoPerfil(SharedViewModel model)
        {
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);


                varUsuario.imagemPerfil = model.PerfilViewModel.imagemPerfil;
                Usuario.EditarUsuario(varUsuario);
            }
            catch
            {

            }

            return RedirectToAction("EditarFotoPerfil", "MinhaConta");
        }
    }
}