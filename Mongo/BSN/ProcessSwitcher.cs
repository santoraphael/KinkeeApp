using Mongo.DAL;
using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.BSN
{
    public class ProcessSwitcher
    {
        SwitcherDAL switcherDAL = new SwitcherDAL();
        NotificationBSN _notificationBSN = new NotificationBSN();

        public bool InsertWallet(ObjectId donoCarteira)
        {
            return switcherDAL.InsertWallet(donoCarteira);
        }

        public bool AlterarWallet(WalletModel Carteira)
        {
            return switcherDAL.AlterarWallet(Carteira);
        }

        public WalletModel GetWalletByDono(ObjectId donoCarteira)
        {
            return switcherDAL.GetWalletByDono(donoCarteira);
        }

        public Double GetSaldoWallet(ObjectId donoCarteira)
        {
            Double saldo = 0;
            var wallet = GetWalletByDono(donoCarteira);

            if (wallet != null)
            {
                saldo = Convert.ToInt32(wallet.Saldo);
            }

            return saldo;
        }

        public WalletModel NewTransaction(ObjectId From, string WalletNameFrom, ObjectId To, double AmountTransaction, TransactionType TransactionType, WalletModel wallet)
        {
            try
            {
                TransactionModel transacao = new TransactionModel();

                transacao.WalletAdressFrom = From;
                transacao.WalletNameFrom = WalletNameFrom;
                transacao.WalletAdressTo = To;
                transacao.Amount = AmountTransaction;
                transacao.TransactionType = TransactionType;
                transacao.Descricao = TransactionType.GetDescription();

                wallet.Transactions.Add(transacao);

                GerarNotificacaoTransacao(From, WalletNameFrom, To, AmountTransaction, TransactionType);

                return wallet;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool VeficicarSeTemSaldoParaDebito(ObjectId WalletFrom, Double AmountTransaction)
        {
            try
            {
                var wallet = switcherDAL.GetWalletByDono(WalletFrom);

                if (Convert.ToUInt32(wallet.Saldo) >= AmountTransaction)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool RealizarTransferenciaKoins(ObjectId From, String WalletNameFrom, ObjectId To, Int64 AmountTransaction, TransactionType TransactionType)
        {
            try
            {
                if (VeficicarSeTemSaldoParaDebito(From, AmountTransaction))
                {
                    var carteiraSaida = GetWalletByDono(From);
                    var carteiraEntrada = GetWalletByDono(To);

                    carteiraSaida.Saldo = carteiraSaida.Saldo - AmountTransaction;
                    carteiraEntrada.Saldo += AmountTransaction;

                    //SAIDA
                    carteiraSaida = NewTransaction(From, WalletNameFrom, To, AmountTransaction, TransactionType.Debit, carteiraSaida);
                    AlterarWallet(carteiraSaida);

                    //ENTRADA
                    carteiraEntrada = NewTransaction(From, WalletNameFrom, To, AmountTransaction, TransactionType.Credit, carteiraEntrada);
                    AlterarWallet(carteiraEntrada);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool CreditarKoins(ObjectId To, string FromName,double AmountTransaction, TransactionType TransactionType)
        {
            try
            {
                var wallet = GetWalletByDono(To);
                if (wallet == null)
                {
                    InsertWallet(To);

                    wallet = GetWalletByDono(To);

                }

                wallet.Saldo += AmountTransaction;

                //ENTRADA
                wallet = NewTransaction(To, FromName, To, AmountTransaction, TransactionType, wallet);
                AlterarWallet(wallet);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DebitarKoins(ObjectId To, string FromName, double AmountTransaction, TransactionType TransactionType = TransactionType.Debit)
        {
            try
            {
                var wallet = GetWalletByDono(To);
                if (wallet == null)
                {
                    InsertWallet(To);

                    wallet = GetWalletByDono(To);

                }

                wallet.Saldo -= AmountTransaction;

                //ENTRADA
                wallet = NewTransaction(To, FromName, To, AmountTransaction, TransactionType, wallet);
                AlterarWallet(wallet);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GerarNotificacaoTransacao(ObjectId From, String WalletNameFrom,  ObjectId To, double AmountTransaction, TransactionType TransactionType)
        {
            Dictionary<string, string> valores = new Dictionary<string, string>();
            valores.Add("URL_DESTINO", "/ContaDigital/");
            valores.Add("ALT_IMAGEM", "Crédito Koins");
            valores.Add("URL_IMAGEM", "../../modules/img/credit-koins.png");
            valores.Add("TITULO", "Transação Bancária");
            valores.Add("SUBTITULO", TransactionType.GetDescription());
            
            string texto = "";

            if (TransactionType == TransactionType.LoginCredit)
            {
                if (WalletNameFrom == "Kinkee")
                {
                    texto = String.Format("Você recebeu <b>{0} Koin(s)</b> em crédito(s) da <b>Kinkee</b> como bonificação.", AmountTransaction);
                }
                else
                {
                    texto = String.Format("Você recebeu <b>{0} Koin(s)</b> em crédito(s) do usuário <b>{1}</b> como bonificação.", AmountTransaction, WalletNameFrom);
                }
            }
            else
            {
                texto = String.Format("Você recebeu <b>{0} Koin(s)</b> em crédito(s) do usuário <b>{1}</b> como bonificação.", AmountTransaction, WalletNameFrom);
            }

            valores.Add("DESCRICAO", texto);

            dynamic dados = valores;
            List<NotificationSendTo> notificationSends = new List<NotificationSendTo>();

            notificationSends.Add(NotificationSendTo.Site);
            return _notificationBSN.GerarNovaNotificacao(dados, Notificationtype.NotificacaoGeral, To, From, notificationSends);
        }
    }
}
