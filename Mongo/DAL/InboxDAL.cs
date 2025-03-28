using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL
{
    public class InboxDAL
    {
        private readonly Connection db = new Connection();
        private readonly UserDAL _userDal = new UserDAL();

        /// <summary>
        /// Cria uma nova Inbox ou adiciona uma mensagem a uma já existente,
        /// atualizando os objetos de e para.
        /// </summary>
        public void NewIbox(InboxModel inbox, UserModel de, UserModel para)
        {
            // Coleção tipada
            var database = db.ConnectServer();
            var collection = database.GetCollection<InboxModel>("inbox");

            try
            {
                // Lista de mensagens enviadas ou recebidas (caso não existam ainda)
                if (de.Inboxes == null) de.Inboxes = new List<UserInboxModel>();
                if (para.Inboxes == null) para.Inboxes = new List<UserInboxModel>();

                // mensagensMandada: procura no 'de' um registro para 'para'
                var mensagemMandada = de.Inboxes.Where(i => i.ParaUsuarioID == para.Id).ToList();
                // mensagensRecebida: procura no 'para' um registro para 'de'
                var mensagemRecebida = para.Inboxes.Where(i => i.ParaUsuarioID == de.Id).ToList();

                // Se não existe um registro de conversa entre 'de' e 'para', cria a Inbox.
                if (mensagemMandada.FirstOrDefault() == null)
                {
                    // Inserimos a inbox no banco pela primeira vez
                    collection.InsertOne(inbox);

                    // 'de' => adiciona referência da nova inbox
                    de.Inboxes.Add(new UserInboxModel
                    {
                        isActive = true,
                        InboxeID = inbox.Id,
                        ParaUsuarioID = para.Id,
                        nomeUsuario = para.Usuario,
                        FotoUsuario = para.imagemPerfil,
                        HasUnreadMessage = false,
                        DateLastInteraction = DateTime.Now,
                        DateCreate = DateTime.Now
                    });
                    _userDal.AlterarUser(de);

                    // 'para' => adiciona referência da nova inbox
                    para.Inboxes.Add(new UserInboxModel
                    {
                        isActive = true,
                        InboxeID = inbox.Id,
                        ParaUsuarioID = de.Id,
                        nomeUsuario = de.Usuario,
                        FotoUsuario = de.imagemPerfil,
                        HasUnreadMessage = true,
                        DateCreate = DateTime.Now
                    });
                    _userDal.AlterarUser(para);
                }
                else
                {
                    // Já existe inbox => pega a existente e adiciona nova mensagem
                    var existingInbox = GetIboxByID(mensagemMandada.First().InboxeID);

                    existingInbox.Messages.Add(new MessageModel
                    {
                        FromId = de.Id,
                        ToId = para.Id,
                        Message = inbox.Messages.FirstOrDefault()?.Message
                    });

                    // Atualiza a inbox existente (chama NewMessage, que efetua ReplaceOne)
                    NewMessage(existingInbox, de, para);

                    // Atualiza a referência 'de'
                    var inboxDe = mensagemMandada.First();
                    de.Inboxes.Remove(inboxDe);
                    inboxDe.DateLastInteraction = DateTime.Now;
                    inboxDe.isActive = true;
                    inboxDe.HasUnreadMessage = false;
                    de.Inboxes.Add(inboxDe);

                    // Atualiza a referência 'para'
                    var inboxPara = mensagemRecebida.FirstOrDefault();
                    if (inboxPara != null)
                    {
                        para.Inboxes.Remove(inboxPara);
                        inboxPara.DateLastInteraction = DateTime.Now;
                        inboxPara.isActive = true;
                        inboxPara.HasUnreadMessage = true;
                        para.Inboxes.Add(inboxPara);
                    }

                    // Persiste alterações no BD
                    _userDal.AlterarUser(de);
                    _userDal.AlterarUser(para);
                }
            }
            catch (Exception e)
            {
                // Tratamento de exceção customizado (se necessário)
                // e.Message == "Valor não pode ser nulo.\r\nNome do parâmetro: source"
                // (Mantido comentado como no seu código original)
            }
        }

        /// <summary>
        /// Adiciona uma nova mensagem à Inbox existente, atualizando-a no banco.
        /// </summary>
        public void NewMessage(InboxModel inbox, UserModel de, UserModel para)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<InboxModel>("inbox");

            try
            {
                // Em vez de Save<InboxModel>(...), fazemos ReplaceOne com upsert
                var filter = Builders<InboxModel>.Filter.Eq(x => x.Id, inbox.Id);
                collection.ReplaceOne(filter, inbox, new ReplaceOptions { IsUpsert = true });
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Retorna a Inbox com base no ObjectId (chave) passado.
        /// </summary>
        public InboxModel GetIboxByID(ObjectId inboxID)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<InboxModel>("inbox");

            var inbox = collection.AsQueryable()
                                  .FirstOrDefault(i => i.Id == inboxID);
            return inbox;
        }
    }
}
