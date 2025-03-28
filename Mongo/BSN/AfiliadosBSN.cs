using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using Mongo.Infrastruture.Helper;
using Mongo.Models.Compra;
using Mongo.DAL.Afiliados.Definicoes;
using Mongo.Models.Afiliados;
using Mongo.DAL.Afiliados.Operacoes;
using System.Linq;

namespace Mongo.BSN
{
    public class AfiliadosBSN
    {
        ProdutosDAL ProdutosDAL = new ProdutosDAL();
        TaxasCustosDAL TaxasCustosDAL = new TaxasCustosDAL();
        TiposGanhosDAL TiposGanhosDAL = new TiposGanhosDAL();
        OperacoesDAL OperacoesDAL = new OperacoesDAL();

        public List<OperacaoModel> ListaOperacoesByUsuario(ObjectId UsuarioId)
        {
            return OperacoesDAL.GetIAllById(UsuarioId);
        }

        public OperacaoModel Operacao(ObjectId id_item)
        {
            return OperacoesDAL.GetItemById(id_item);
        }

        public void LiberarOperacao(ObjectId id_item)
        {
            var operacao = OperacoesDAL.GetItemById(id_item);

            if(!operacao.OperacaoLiberada)
            {
                operacao.OperacaoLiberada = true;
                OperacoesDAL.AlterarOperacao(operacao);

                //==================================

                UserBSN _userBSN = new UserBSN();
                var afiliado = _userBSN.GetUsuarioByUserId(new ObjectId(operacao.UsuarioId.ToString()));

                var quemConvidou = _userBSN.GetUsuarioByUserId(afiliado.InvitedBy);


                if (quemConvidou != null)
                {
                    OperacaoModel novaOperacao = new OperacaoModel();
                    novaOperacao.DataOperacao = DateTime.Now;
                    novaOperacao.DataAlteracao = null;
                    novaOperacao.OperacaoLiberada = true;

                    novaOperacao.UsuarioId = quemConvidou.Id;
                    novaOperacao.IdConvidado = quemConvidou.Id;

                    var tiposGanho = TiposGanhosDAL.GetTipoGanhoByNome(NomesTiposGanhos.Comissao.ToString(), operacao.NomePerfil.ToString());
                    var perfil = NomesPerfis.semassinatura;

                    if (quemConvidou.ContaGold)
                    {
                        perfil = NomesPerfis.comassinatura;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(quemConvidou.NomesPerfil))
                        {
                            perfil = NomesPerfis.semassinatura;
                        }
                        else
                        {
                            perfil = (NomesPerfis)Enum.Parse(typeof(NomesPerfis), quemConvidou.NomesPerfil);
                        }
                    }

                    novaOperacao.IdTipoGanho = quemConvidou.Id;
                    novaOperacao.NomePerfil = perfil;
                    novaOperacao.TaxaCartao = 0;
                    novaOperacao.TaxaISS = 0;
                    novaOperacao.CustoFixo = 0;
                    novaOperacao.CustoBoleto = 0;
                    novaOperacao.ValorBruto = operacao.ValorComissao;
                    novaOperacao.ValorLiquido = operacao.ValorComissao;
                    novaOperacao.ValorPremio = operacao.ValorComissao;
                    novaOperacao.ValorComissao = 0;
                    novaOperacao.ValorAReceber = operacao.ValorComissao;
                    novaOperacao.LoteFechado = false;
                    novaOperacao.DataFechamento = null;

                    OperacoesDAL.InsertOperacao(novaOperacao);
                }
            }
        }

        public void FecharLote(ObjectId UsuarioId)
        {
            UserBSN _userBSN = new UserBSN();
            var afiliado = _userBSN.GetUsuarioByUserId(UsuarioId);

            var LoteAberto = OperacoesDAL.PegarLoteNaoPago(UsuarioId);

            if(LoteAberto == null)
            {
                LoteOperacoesModel loteOperacao = new LoteOperacoesModel();
                loteOperacao.DataFechamento = DateTime.Now;
                loteOperacao.UsuarioId = afiliado.Id;

                AfiliadosConfiguracaoPagamentoBSN afiliadosConfiguracaoPagamentoBSN = new AfiliadosConfiguracaoPagamentoBSN();
                var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(afiliado.Id).FirstOrDefault();
                loteOperacao.ContaPagamento = configuracoesPagamento.ChavePix;

                loteOperacao.StatusLote = StatusLote.fechado;

                var lote = OperacoesDAL.InsertLote(loteOperacao);

                FecharListaOperacaoes(afiliado.Id, lote);
            }
        }

        public void FecharListaOperacaoes(ObjectId UsuarioId, LoteOperacoesModel Lote)
        {
            var listaOperacoes = OperacoesDAL.ListarOperacoesLiberadas(UsuarioId);
            List<ObjectId> listaOperacoesFechadas = new List<ObjectId>();
            double valorTotal = 0.0;

            foreach (var item in listaOperacoes)
            {
                item.DataFechamento = DateTime.Now;
                item.idLote = Lote.Id;
                OperacoesDAL.InsertOperacaoHistorico(item);

                OperacoesDAL.DeteleOperacaoFechamento(item);

                valorTotal += item.ValorAReceber;

                listaOperacoesFechadas.Add(item.Id);
            }

            Lote.OperacoesLote = listaOperacoesFechadas;
            Lote.ValorLote = valorTotal;

            OperacoesDAL.AlterarLote(Lote);
        }



        /// <summary>
        /// Método para salvar operações executadas para pagamento de afiliados.
        /// </summary>
        /// <param name="IdPlanPagamento">IdPlanPagamento na tabela Definições Produtos caso TipoOperacao.Assinatura</param>
        /// <param name="TransacaoItemId">TransacaoItemId na tabela Definições Produtos caso TipoOperacao.Item</param>
        /// <param name="IdOperacaoRelacionada">_id tanto da tabela Users.Subscriptios (TipoOperacao.Assinatura) e Users.Transacao (TipoOperacao.Item)</param>
        /// <param name="TipoOperacao">Item ou Assinatura</param>
        /// <param name="nomesTiposGanhos">Percentual Compras (TipoOperacao.Item ou TipoOperacao.Item) ou Valor Por Daddy Aprovado e Interacao Daddy, nesse caso o Tipo Operacao deve ser Item</param>
        /// <param name="AfiliadoId">Para quem vai o valor da operação</param>
        /// <param name="IdConvidado">Quem executou a ação para gerar a operação</param>
        /// <returns>Retorna Operação Salva.</returns>
        /// <remarks>Deve ser implementado sempre que uma ação executada por um usuário gerar receita para um afiliado.</remarks>
        public OperacaoModel GerarOperacaoOperacao(string IdPlanPagamento, string TransacaoItemId, string IdOperacaoRelacionada, TipoOperacao TipoOperacao,  NomesTiposGanhos nomesTiposGanhos, ObjectId AfiliadoId, ObjectId? IdConvidado)
        {
            UserBSN _userBSN = new UserBSN();
            var afiliado = _userBSN.GetUsuarioByUserId(AfiliadoId);

            var ItemId = new ObjectId();
            var operacaoRelacionada = new ObjectId();

            if (!string.IsNullOrEmpty(TransacaoItemId))
            {
                ItemId = ObjectId.Parse(TransacaoItemId);
            }

            if (!string.IsNullOrEmpty(IdOperacaoRelacionada))
            {
                operacaoRelacionada = ObjectId.Parse(IdOperacaoRelacionada);
            }


            ProdutoModel produtoModel = PegaProduto(TipoOperacao, IdPlanPagamento, ItemId);

            OperacaoModel operacaoModel = new OperacaoModel();

            var perfil = NomesPerfis.semassinatura;


            if(afiliado.ContaGold)
            {
                perfil = NomesPerfis.comassinatura;
            }
            else
            {
                if (string.IsNullOrEmpty(afiliado.NomesPerfil))
                {
                    perfil = NomesPerfis.semassinatura;
                }
                else
                {
                    perfil = (NomesPerfis)Enum.Parse(typeof(NomesPerfis), afiliado.NomesPerfil);
                }
            }


            if (produtoModel != null)
            {
                var produtoCalculado = CalculaValorLiquidoProduto(produtoModel, perfil);
                var valorPremio = CalcularPremioSobreValorProduto(produtoCalculado.ValorLiquido, perfil);
                var valorComissao = CalcularComissaoSobreValorPremio(valorPremio, perfil);
                var tiposGanho = TiposGanhosDAL.GetTipoGanhoByNome(nomesTiposGanhos.ToString(), perfil.ToString());

                var listTaxasCustos = TaxasCustosDAL.GetAllItem();
                double taxa_cartao = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.TaxaCartaoCredito.ToString() && t.Perfil == perfil.ToString()).Valor;
                double taxa_iss = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.TaxaIIS.ToString() && t.Perfil == perfil.ToString()).Valor;
                double custo_fixo = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.CustoFixoTransacao.ToString() && t.Perfil == perfil.ToString()).Valor;
                double valor_boleto = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.ValorBoletoPago.ToString() && t.Perfil == perfil.ToString()).Valor;

                operacaoModel.DataOperacao = DateTime.Now;
                operacaoModel.DataAlteracao = null;
                operacaoModel.OperacaoLiberada = false;

                operacaoModel.IdOperacaoRelacionada = operacaoRelacionada;
                operacaoModel.TipoOperacao = TipoOperacao;

                operacaoModel.UsuarioId = AfiliadoId;
                operacaoModel.IdConvidado = IdConvidado;
                operacaoModel.IdTipoGanho = tiposGanho.Id;
                operacaoModel.NomePerfil = perfil;
                operacaoModel.TaxaCartao = taxa_cartao;
                operacaoModel.TaxaISS = taxa_iss;
                operacaoModel.CustoFixo = custo_fixo;
                operacaoModel.CustoBoleto = valor_boleto;
                operacaoModel.ValorBruto = produtoModel.ValorBruto;
                operacaoModel.ValorLiquido = produtoCalculado.ValorLiquido;
                operacaoModel.ValorPremio = valorPremio;
                operacaoModel.ValorComissao = valorComissao;
                operacaoModel.ValorAReceber = valorPremio - valorComissao;
                operacaoModel.LoteFechado = false;
                operacaoModel.DataFechamento = null;


            }
            else
            {
                var tiposGanho = TiposGanhosDAL.GetTipoGanhoByNome(nomesTiposGanhos.ToString(), perfil.ToString());

                operacaoModel.DataOperacao = DateTime.Now;
                operacaoModel.DataAlteracao = null;
                operacaoModel.OperacaoLiberada = true;

                operacaoModel.UsuarioId = AfiliadoId;
                operacaoModel.IdConvidado = IdConvidado;
                operacaoModel.IdTipoGanho = tiposGanho.Id;
                operacaoModel.NomePerfil = perfil;
                operacaoModel.TaxaCartao = 0;
                operacaoModel.TaxaISS = 0;
                operacaoModel.CustoFixo = 0;
                operacaoModel.CustoBoleto = 0;
                operacaoModel.ValorBruto = tiposGanho.Valor;
                operacaoModel.ValorLiquido = tiposGanho.Valor;
                operacaoModel.ValorPremio = tiposGanho.Valor;
                operacaoModel.ValorComissao = 0;
                operacaoModel.ValorAReceber = tiposGanho.Valor;
                operacaoModel.LoteFechado = false;
                operacaoModel.DataFechamento = null;
            }

            OperacoesDAL.InsertOperacao(operacaoModel);
            return operacaoModel;
        }

        public ProdutoModel PegaProduto(TipoOperacao tipoOperacao ,string IdPlanPagamento, ObjectId TransacaoItemId)
        {
            if(tipoOperacao == TipoOperacao.Assinatura)
            {
                return GetProdutoByIdPlanPagamento(IdPlanPagamento);
                
            }
            else
            {
                return GetProdutoByTransacaoItemId(TransacaoItemId);
            }
        }

        public ProdutoLiquidoModel CalculaValorLiquidoProduto(ProdutoModel produto, NomesPerfis perfil)
        {
            ProdutoLiquidoModel produtoCalculado = new ProdutoLiquidoModel();
            produtoCalculado.Id = produto.Id;
            produtoCalculado.NomeProduto = produto.NomeProduto;

            //FORMULA
            //VALOR_PRODUTO - (VALOR_PRODUTO * TAXA_CARTAO/100) - (VALOR_PRODUTO * TAXA_ISS/100) - (CUSTO_FIXO + VALOR_BOLETO_PAGO)

            var listTaxasCustos = TaxasCustosDAL.GetAllItem();
            double taxa_cartao = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.TaxaCartaoCredito.ToString() && t.Perfil == perfil.ToString()).Valor;
            double taxa_iss = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.TaxaIIS.ToString() && t.Perfil == perfil.ToString()).Valor;
            double custo_fixo = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.CustoFixoTransacao.ToString() && t.Perfil == perfil.ToString()).Valor;
            double valor_boleto = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.ValorBoletoPago.ToString() && t.Perfil == perfil.ToString()).Valor;



            produtoCalculado.ValorLiquido = produto.ValorBruto - (produto.ValorBruto * taxa_cartao / 100) - (produto.ValorBruto * taxa_iss / 100) - (custo_fixo + valor_boleto);


            return produtoCalculado;
        }

        public double CalcularPremioSobreValorProduto(double valorProduto, NomesPerfis perfil)
        {
            var listTiposGanhos = TiposGanhosDAL.GetAllItem();
            double percentual_compra = listTiposGanhos.Find(t => t.NomeTipoGanho == NomesTiposGanhos.PercentualCompras.ToString() && t.Perfil == perfil.ToString()).Valor;

            //FORMULA
            //(VALOR_PRODUTO * PercentualCompras/100)
            return valorProduto * percentual_compra / 100;
        }

        public double CalcularComissaoSobreValorPremio(double valorPremio, NomesPerfis perfil)
        {
            var listTaxasCustos = TaxasCustosDAL.GetAllItem();
            double percentual_comissao = listTaxasCustos.Find(t => t.NomeTaxaCusto == NomesTaxasCustos.PercentualComissao.ToString() && t.Perfil == perfil.ToString()).Valor;

            //FORMULA
            //(VALOR_PREMIO * PercentualCompras/100)
            return valorPremio * percentual_comissao / 100;
        }

        //public double CalculaPremioDaddyAprovado()
        //{
        //    var listTiposGanhos = TiposGanhosDAL.GetAllItem();
        //    double valor_aprovacao = listTiposGanhos.Find(t => t.NomeTipoGanho == NomesTiposGanhos.PercentualCompras.ToString() && t.Perfil == perfil.ToString()).Valor;
        //}


        public ProdutoModel GetProdutoByNome(NomesProdutos nomeProduto)
        {
            return ProdutosDAL.GetProdutoByNome(nomeProduto.ToString());
        }

        public ProdutoModel GetProdutoByTransacaoItemId(ObjectId TransacaoItemId)
        {
            return ProdutosDAL.GetProdutoByTransacaoItemId(TransacaoItemId);
        }

        public ProdutoModel GetProdutoByIdPlanPagamento(string IdPlanPagamento)
        {
            return ProdutosDAL.GetProdutoByIdPlanPagamento(IdPlanPagamento);
        }

        //public List<Item> GetAllItems()
        //{

        //    return transacaoDAL.GetAllItem();

        //}

        //public Item GetItemById(ObjectId id_item)
        //{
        //    return transacaoDAL.GetItemById(id_item);
        //}
    }
}
