using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace Mongo.BSN
{
    public class ConnectionsBSN
    {
        ConnectionsDAL connectionDAL = new ConnectionsDAL();

        public bool InsertConnection(ConnectionModel connection)
        {
            bool retorno = false;
            try 
            {
                connectionDAL.InserConnection(connection);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool EditarConnecion(ConnectionModel connection)
        {
            bool retorno = false;
            try
            {
                connectionDAL.AlterarConnection(connection);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }


        public ConnectionModel GetConnection(string ConnectionId)
        {
            ConnectionModel connection = new ConnectionModel();
            try
            {
                connectionDAL.GetConnection(ConnectionId);
            }
            catch
            {

            }

            return connection;
        }

    }
}
