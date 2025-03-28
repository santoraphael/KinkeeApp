using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.BSN
{
    public class ChatDAL
    {
        private readonly Connection db = new Connection();
        private readonly string CollectionChatRoom = "Chat.Rooms";
        private readonly string CollectionChatMessage = "Chat.Rooms.Messages";

        public ObjectId InsertChatRoom(ChatRoomModel chat)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatRoomModel>(CollectionChatRoom);

            try
            {
                collection.InsertOne(chat);
            }
            catch
            {
                return chat.Id;
            }

            return chat.Id;
        }

        public void AlterChatRoom(ObjectId roomId, ChatMessagesModel message)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatRoomModel>(CollectionChatRoom);

            try
            {
                var filter = Builders<ChatRoomModel>.Filter.Eq(e => e.Id, roomId);
                var update = Builders<ChatRoomModel>.Update
                    .Set(e => e.LastMessage, message)
                    .Set(e => e.DateLastInteraction, DateTime.Now);

                collection.UpdateOne(filter, update);
            }
            catch
            {
            }
        }

        public void ReadChatRoom(ObjectId roomId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatRoomModel>(CollectionChatRoom);

            try
            {
                var filter = Builders<ChatRoomModel>.Filter.Eq(e => e.Id, roomId);
                var update = Builders<ChatRoomModel>.Update.Set(e => e.LastMessage.Message_Read, true);

                collection.UpdateOne(filter, update);
            }
            catch
            {
            }
        }

        public void ReadChatMessage(ObjectId roomId, ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatMessagesModel>(CollectionChatMessage);

            try
            {
                var filter = Builders<ChatMessagesModel>.Filter.Where(
                    message => message.RoomId == roomId &&
                               message.SenderId != userId &&
                               message.Message_Read == false);

                var update = Builders<ChatMessagesModel>.Update.Set(message => message.Message_Read, true);

                collection.UpdateMany(filter, update);
            }
            catch
            {
            }
        }

        public ChatRoomModel GetUniqueChatRoomByParticipants(ObjectId participantOne, ObjectId participantTwo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatRoomModel>(CollectionChatRoom);

            var query = collection.AsQueryable()
                .FirstOrDefault(room =>
                    (room.ParticipantOne == participantOne || room.ParticipantTwo == participantOne) &&
                    (room.ParticipantOne == participantTwo || room.ParticipantTwo == participantTwo));

            return query;
        }

        public bool InsertMessage(ChatMessagesModel message)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatMessagesModel>(CollectionChatMessage);

            bool retorno;
            try
            {
                collection.InsertOne(message);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public List<ChatRoomModel> GetListChatRoomByUser(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatRoomModel>(CollectionChatRoom);

            var query = collection.AsQueryable()
                .Where(room => room.ParticipantOne == userId || room.ParticipantTwo == userId)
                .ToList();

            return query;
        }

        public List<ChatMessagesModel> GetListChatRoomMassagesByUser(ObjectId roomId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatMessagesModel>(CollectionChatMessage);

            var query = collection.AsQueryable()
                .Where(message => message.RoomId == roomId)
                .ToList();

            return query;
        }

        public int GetListChatRoomMassagesByUser(ObjectId roomId, ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatMessagesModel>(CollectionChatMessage);

            var query = collection.AsQueryable()
                .Count(message => message.RoomId == roomId &&
                                  message.SenderId != userId &&
                                  message.Message_Read == false);

            return query;
        }

        public ChatMessagesModel GetLastChatMassagesByRoomId(ObjectId roomId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ChatMessagesModel>(CollectionChatMessage);

            var query = collection.AsQueryable()
                .FirstOrDefault(message => message.RoomId == roomId);

            return query;
        }
    }
}
