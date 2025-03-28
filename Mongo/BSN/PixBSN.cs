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
    public class PixBSN
    {
        PixDAL PixDAL = new PixDAL();

        public bool InsertPix(PixModel pix)
        {
            return PixDAL.InsertPix(pix);
        }

        public PixModel PegarPixPorId(ObjectId pixId)
        {
            return PixDAL.PegarPixPorId(pixId);
        }

        public bool SalvarPix(PixModel pix)
        {
            return PixDAL.SalvarPixPorId(pix);
        }

        public bool RemoverPixPorId(ObjectId pixId)
        {
            return PixDAL.RemoverPixPorId(pixId);
        }


        public IList<PixModel> PegarPixPorUsuarioPagador(ObjectId usuarioId)
        {
            return PixDAL.PegarPixPorUsuarioPagador(usuarioId);
        }

        public IList<PixModel> PegarPixPorUsuarioRecebedor(ObjectId usuarioId)
        {
            return PixDAL.PegarPixPorUsuarioRecebedor(usuarioId);
        }
    }
}
