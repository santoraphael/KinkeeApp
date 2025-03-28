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
using Mongo.DAL.Afiliados.Relatorios;

namespace Mongo.BSN
{
    public class LogErrorBSN
    {
        LogErrorDAL LogErrorDAL = new LogErrorDAL();

        public bool InsertLogErro(LogErrorModel logError)
        {
            return LogErrorDAL.InsertLogErro(logError);
        }
    }
}
