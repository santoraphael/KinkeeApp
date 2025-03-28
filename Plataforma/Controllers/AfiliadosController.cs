using Mongo.BSN;
using Mongo.Models;
using System.Web.Mvc;
using Miscellaneous.SugarScore;
using MongoDB.Bson;
using System.Linq;
using System.Collections.Generic;
using System;
using Plataforma.Models;

namespace Plataforma.Controllers
{
    [Authorize]
    public partial class AfiliadosController : Controller
    {
        UserBSN _userBSN = new UserBSN();
        AfiliadosBSN afiliadosBSN = new AfiliadosBSN();
        AfiliadosRelatorioBSN afiliadosRelatorioBSN = new AfiliadosRelatorioBSN();
        AfiliadosConfiguracaoPagamentoBSN afiliadosConfiguracaoPagamentoBSN = new AfiliadosConfiguracaoPagamentoBSN();


        public ActionResult Index()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            GerarCodigo(usuarioLogado);

            //var userInformation = _userBSN.GetInformationByUserId(usuarioLogado.Id);
            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            { 
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;
            ViewBag.CodigoConvite = usuarioLogado.CodigoConvite;
            ViewBag.NumeroClicks = afiliadosRelatorioBSN.PegarNumeroClicksPorUsuario(usuarioLogado.Id);

            ViewBag.Tabela = MontarTabela(usuarioLogado.Id, listaOperacoes, 3);
            //PegarTodosClicks(usuarioLogado.Id);
            //PegarOperacoes(listaOperacoes);
            //PegarCadastro(usuarioLogado.Id);

            return View();
        }

        public ActionResult Resumo()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);


            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;
            ViewBag.CodigoConvite = usuarioLogado.CodigoConvite;
            ViewBag.NumeroClicks = afiliadosRelatorioBSN.PegarNumeroClicksPorUsuario(usuarioLogado.Id);
            PegarTodosClicks(usuarioLogado.Id);

            ViewBag.Tabela = MontarTabela(usuarioLogado.Id, listaOperacoes, 3);

            return View();
        }

        public ActionResult MaterialApoio()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);


            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;
            ViewBag.CodigoConvite = usuarioLogado.CodigoConvite;

            return View();
        }

        public ActionResult Relatorios()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            GerarCodigo(usuarioLogado);


            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;

            ViewBag.Tabela = MontarTabela(usuarioLogado.Id, listaOperacoes, 15);
            return View();
        }

        public ActionResult Referencias()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);

            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;
            ViewBag.CodigoConvite = usuarioLogado.CodigoConvite;
            return View();
        }

        public ActionResult Pagamentos()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);

            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;

            return View();
        }
        public ActionResult Ajuda()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);

            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;

            return View();
        }

        public ActionResult ConfiguracaoPagamento()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            GerarCodigo(usuarioLogado);

            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }


            var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(usuarioLogado.Id).FirstOrDefault();
            ConfiguracaoPagamentoViewModel configuracaoPagamentoViewModel = new ConfiguracaoPagamentoViewModel();

            if(configuracoesPagamento != null)
            {
                configuracaoPagamentoViewModel.Id = configuracoesPagamento.Id;
                configuracaoPagamentoViewModel.DataCadastro = configuracoesPagamento.DataCadastro;
                configuracaoPagamentoViewModel.UsuarioId = configuracoesPagamento.UsuarioId;
                configuracaoPagamentoViewModel.TipoOpcaoPagamento = (Plataforma.Models.TipoOpcaoPagamento)configuracoesPagamento.TipoOpcaoPagamento;
                configuracaoPagamentoViewModel.TipoChavePix = (Plataforma.Models.TipoChavePix)configuracoesPagamento.TipoChavePix;
                configuracaoPagamentoViewModel.TipoPessoaConta = (Plataforma.Models.TipoPessoaConta)configuracoesPagamento.TipoPessoaConta;
                configuracaoPagamentoViewModel.ChavePix = configuracoesPagamento.ChavePix;
                configuracaoPagamentoViewModel.NomeTitular = configuracoesPagamento.NomeTitular;
                configuracaoPagamentoViewModel.CNPJCPF = configuracoesPagamento.CNPJCPF;
                configuracaoPagamentoViewModel.Banco = configuracoesPagamento.Banco;
                configuracaoPagamentoViewModel.Agencia = configuracoesPagamento.Agencia;
                configuracaoPagamentoViewModel.Conta = configuracoesPagamento.Conta;
            }
            

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;

            return View(configuracaoPagamentoViewModel);
        }

        [HttpPost]
        public ActionResult Form2(ConfiguracaoPagamentoViewModel configuracaoPagamentoViewModel)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuarioLogado.Id);

            var valorTotal = 0.0;
            var saldoLiberado = 0.0;

            foreach (var item in listaOperacoes)
            {
                if (item.OperacaoLiberada)
                {
                    saldoLiberado += item.ValorAReceber;
                }
                else
                {
                    valorTotal += item.ValorAReceber - item.ValorComissao;
                }

            }

            var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(usuarioLogado.Id).FirstOrDefault();
            ConfiguracaoPagamentoModel configuracaoPagamento = new ConfiguracaoPagamentoModel();

            if(configuracoesPagamento != null)
            {
                configuracaoPagamento.Id = configuracoesPagamento.Id;
            }
            
            configuracaoPagamento.DataCadastro = configuracaoPagamentoViewModel.DataCadastro;
            configuracaoPagamento.UsuarioId = usuarioLogado.Id;
            configuracaoPagamento.TipoOpcaoPagamento = (Mongo.Models.TipoOpcaoPagamento)configuracaoPagamentoViewModel.TipoOpcaoPagamento;
            configuracaoPagamento.TipoChavePix = (Mongo.Models.TipoChavePix)configuracaoPagamentoViewModel.TipoChavePix;
            configuracaoPagamento.TipoPessoaConta = (Mongo.Models.TipoPessoaConta)configuracaoPagamentoViewModel.TipoPessoaConta;
            configuracaoPagamento.ChavePix = configuracaoPagamentoViewModel.ChavePix;
            configuracaoPagamento.NomeTitular = configuracaoPagamentoViewModel.NomeTitular;
            configuracaoPagamento.CNPJCPF = configuracaoPagamentoViewModel.CNPJCPF;
            configuracaoPagamento.Banco = configuracaoPagamentoViewModel.Banco;
            configuracaoPagamento.Agencia = configuracaoPagamentoViewModel.Agencia;
            configuracaoPagamento.Conta = configuracaoPagamentoViewModel.Conta;

            afiliadosConfiguracaoPagamentoBSN.AlterarCofiguracaoPagamento(configuracaoPagamento);

            ViewBag.valorTotal = valorTotal;
            ViewBag.saldoLiberado = saldoLiberado;

            return View("ConfiguracaoPagamento");
        }



        public ActionResult ConciliacaoFechamento()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            if(!usuarioLogado.Adm)
            {
                return View("Index");
            }

            var usuarios = ListarAfialiados(0, 2000);
            List<ConciliacaoFechamentoViewModel> listaConciliacao = new List<ConciliacaoFechamentoViewModel>();

            foreach (var usuario in usuarios)
            {
                ConciliacaoFechamentoViewModel conciliacaoFechamentoViewModel = new ConciliacaoFechamentoViewModel();


                conciliacaoFechamentoViewModel.UsuarioId = usuario.Id;
                conciliacaoFechamentoViewModel.NomeUsuario = usuario.Usuario;

                conciliacaoFechamentoViewModel.NumeroClicks = afiliadosRelatorioBSN.PegarNumeroClicksPorUsuario(usuario.Id);
                var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuario.Id);

                foreach (var operacao in listaOperacoes)
                {
                    conciliacaoFechamentoViewModel.ValorBruto += operacao.ValorBruto;
                    conciliacaoFechamentoViewModel.ValorLiquido += operacao.ValorLiquido;
                    conciliacaoFechamentoViewModel.ValorPremio += operacao.ValorPremio;
                    conciliacaoFechamentoViewModel.ValorComissao += operacao.ValorComissao;
                    conciliacaoFechamentoViewModel.ValorAReceber += operacao.ValorAReceber;
                }


                if(conciliacaoFechamentoViewModel.ValorBruto > 0)
                {
                    listaConciliacao.Add(conciliacaoFechamentoViewModel);
                }
                
            }

            ViewBag.listaConciliacao = listaConciliacao.OrderByDescending(o => o.ValorBruto).ThenByDescending(o => o.NumeroClicks).ToList();



            //Listar Todos os Afiliados e seu respectivo saldo
            //Listar todas as operações do Afiliado
            //Opção por liberar operações do afiliado
            //Opção por recusar operação do afiliado
            //Fechar lote operações (liberar todas as oparações, adicionar um lote de pagamento)
            ViewBag.valorTotal = 0.0;
            ViewBag.saldoLiberado = 0.0;

            return View();
        }

        public ActionResult LotesFechados()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            if (!usuarioLogado.Adm)
            {
                return View("Index");
            }

            var usuarios = ListarAfialiados(0, 2000);
            List<ConciliacaoFechamentoViewModel> listaConciliacao = new List<ConciliacaoFechamentoViewModel>();

            foreach (var usuario in usuarios)
            {
                ConciliacaoFechamentoViewModel conciliacaoFechamentoViewModel = new ConciliacaoFechamentoViewModel();


                conciliacaoFechamentoViewModel.UsuarioId = usuario.Id;
                conciliacaoFechamentoViewModel.NomeUsuario = usuario.Usuario;

                conciliacaoFechamentoViewModel.NumeroClicks = afiliadosRelatorioBSN.PegarNumeroClicksPorUsuario(usuario.Id);
                var listaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(usuario.Id);

                foreach (var operacao in listaOperacoes)
                {
                    conciliacaoFechamentoViewModel.ValorBruto += operacao.ValorBruto;
                    conciliacaoFechamentoViewModel.ValorLiquido += operacao.ValorLiquido;
                    conciliacaoFechamentoViewModel.ValorPremio += operacao.ValorPremio;
                    conciliacaoFechamentoViewModel.ValorComissao += operacao.ValorComissao;
                    conciliacaoFechamentoViewModel.ValorAReceber += operacao.ValorAReceber;
                }


                if (conciliacaoFechamentoViewModel.ValorBruto > 0)
                {
                    listaConciliacao.Add(conciliacaoFechamentoViewModel);
                }

            }

            ViewBag.listaConciliacao = listaConciliacao.OrderByDescending(o => o.ValorBruto).ThenByDescending(o => o.NumeroClicks).ToList();



            //Listar Todos os Afiliados e seu respectivo saldo
            //Listar todas as operações do Afiliado
            //Opção por liberar operações do afiliado
            //Opção por recusar operação do afiliado
            //Fechar lote operações (liberar todas as oparações, adicionar um lote de pagamento)
            ViewBag.valorTotal = 0.0;
            ViewBag.saldoLiberado = 0.0;

            return View();
        }
        

        public List<UserModel> ListarAfialiados(int pageIndex, int pageSize)
        {
            List<UserModel> query = new List<UserModel>();
            query = (from c in _userBSN.GetListAllUser() select c)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();

            return query;
        }

        public ActionResult ListaOperacoesFechamento(string UsuarioId)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            if (!usuarioLogado.Adm)
            {
                return View("Index");
            }

            UserBSN userBSN = new UserBSN();
            var usuarioAfiliado = userBSN.GetUsuarioByUserId(new ObjectId(UsuarioId));
            ViewBag.NomeUsuario = usuarioAfiliado.Name +" "+ usuarioAfiliado.Lastname +" ("+ usuarioAfiliado.Usuario+")";
            
            ViewBag.NumeroClicks = afiliadosRelatorioBSN.PegarNumeroClicksPorUsuario(new ObjectId(UsuarioId));
            ViewBag.ListaOperacoes = afiliadosBSN.ListaOperacoesByUsuario(new ObjectId(UsuarioId));

            double valorTotalLiberado = 0.0;

            foreach (var item in ViewBag.ListaOperacoes)
            {
                if(item.OperacaoLiberada)
                {
                    valorTotalLiberado += item.ValorAReceber;
                }
            }

            ViewBag.valorTotalLiberado = valorTotalLiberado;

            ViewBag.IdUsuario = usuarioAfiliado.Id;

            ViewBag.valorTotal = 0.0;
            ViewBag.saldoLiberado = 0.0;

            return View();
        }

        [HttpPost]
        public void LiberarOperacao(string OperacaoId)
        {
            var id = new ObjectId(OperacaoId);

            afiliadosBSN.LiberarOperacao(id);
        }

        [HttpPost]
        public void FecharLote(string UsuarioId)
        {
            var id = new ObjectId(UsuarioId);

            afiliadosBSN.FecharLote(id);
        }

        public ActionResult OperacaoRelacionada(string id_operacao, string tipo)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);

            if (!usuarioLogado.Adm)
            {
                return View("Index");
            }

            if(tipo == "Assinatura")
            {
                MinhaContaController minhaConta = new MinhaContaController();
                AssinaturaModel assinaturaModel = minhaConta.GetAssinaturaByID(new ObjectId(id_operacao));

                ViewBag.Operacao = assinaturaModel;
                ViewBag.tipo = tipo;
            }
            else if(tipo == "Item")
            {

            }
            


            ViewBag.valorTotal = 0.0;
            ViewBag.saldoLiberado = 0.0;

            return View();
        }

        public void LiberarOperacaoAfiliado()
        {

        }
        public void RecusarOperacaoAfiliado()
        {

        }

        public void FecharLoteOperacao()
        {

        }




        public void GerarCodigo(UserModel usuarioLogado)
        {
            if (string.IsNullOrEmpty(usuarioLogado.CodigoConvite))
            {
                Geral geral = new Geral();
                var gerarCodigo = true;
                var codigo = "";
                int tentativas = 0;

                while (gerarCodigo)
                {
                    codigo = geral.GerarCodigoConvite();
                    var usuario = _userBSN.GetUserByCodigoConvite(codigo);
                    if (usuario != null)
                    {
                        gerarCodigo = true;
                    }
                    else
                    {
                        gerarCodigo = false;
                    }

                    if (tentativas > 10)
                    {
                        break;
                    }
                }

                usuarioLogado.CodigoConvite = codigo;

                _userBSN.EditarUsuario(usuarioLogado);
            }
        }

        public void PegarTodosClicks(ObjectId UsuarioId)
        {
            var clicks = afiliadosRelatorioBSN.PegarTodosClicksPorUsuario(UsuarioId).GroupBy(c => c.DataHoraClick.ToString("dd/MM/yyyy")).Select(grp => grp.ToList()).ToList();
        }

        public void PegarOperacoes(List<OperacaoModel> list)
        {
            var clicks = list.GroupBy(c => c.DataOperacao.GetValueOrDefault().ToString("dd/MM/yyyy")).Select(grp => grp.ToList()).ToList();
        }

        public void PegarCadastro(ObjectId UsuarioId)
        {
            var clicks = _userBSN.GetListUserByInvited(UsuarioId).GroupBy(c => c.DateCreate.GetValueOrDefault().ToString("dd/MM/yyyy")).Select(grp => grp.ToList()).ToList();
        }

        public List<RelatorioModel> MontarTabela(ObjectId UsuarioId, List<OperacaoModel> listaOperacoes, int qtdDias)
        {
            var clicks = afiliadosRelatorioBSN.PegarTodosClicksPorUsuario(UsuarioId);
            var usuarios = _userBSN.GetListUserByInvited(UsuarioId);

            List<RelatorioModel> lista = new List<RelatorioModel>();
            var hoje = DateTime.Now;

            for (int i = 1; i <= qtdDias; i++)
            {
                RelatorioModel relatorioModel = new RelatorioModel();
                relatorioModel.Data = hoje;

                foreach (var item in clicks)
                {
                    if (item.DataHoraClick.ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy"))
                    {
                        relatorioModel.NumeroClicks += 1;
                    }
                }

                foreach (var item in usuarios)
                {
                    if (item.DateCreate.GetValueOrDefault().ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy"))
                    {
                        relatorioModel.NumeroCadastros += 1;
                    }

                    if (item.DateCreate.GetValueOrDefault().ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy") && item.ProfileCreated == true)
                    {
                        relatorioModel.NumeroPerfisCriados += 1;
                    }

                    if (item.DateCreate.GetValueOrDefault().ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy") && item.ApprovedProfile == true)
                    {
                        relatorioModel.NumeroPerfisAprovados += 1;
                    }
                }

                foreach (var item in listaOperacoes)
                {
                    if (item.DataOperacao.GetValueOrDefault().ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy"))
                    {
                        

                        relatorioModel.ValorDescontoComissao += item.ValorComissao;

                        if (item.OperacaoLiberada)
                        {
                            relatorioModel.ValorLiberado += item.ValorAReceber;
                        }
                        else
                        {
                            relatorioModel.ValorPremioLiquido += item.ValorAReceber - item.ValorComissao;
                        }
                    }

                    if (item.TipoOperacao == TipoOperacao.Assinatura && item.DataOperacao.GetValueOrDefault().ToString("dd/MM/yyyy") == hoje.ToString("dd/MM/yyyy"))
                    {
                        relatorioModel.NumeroAssinaturas += 1;
                    }
                }

                lista.Add(relatorioModel);

                hoje = hoje.AddDays(-1);
            }

            return lista;
        }

    }
}