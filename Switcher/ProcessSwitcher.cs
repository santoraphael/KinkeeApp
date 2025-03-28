using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcher
{
    public class ProcessSwitcher
    {
        public bool NewTransaction(ObjectId From, ObjectId To, TransactionType TransactionType)
        {
            switch (TransactionType)
            {
            case TransactionType.Credit:
                

                break;
            case TransactionType.Debit:
                    // código 2
                break;
            case TransactionType.Chargeback:
                // código 2
                break;
            }
        }

        public bool VerifyWalletAmount(ObjectId WalletFrom)
        {
            
        }
    }
}
