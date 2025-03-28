using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Mongo.BSN
{
    public class ChatBSN
    {
        private readonly UserBSN _userBSN = new UserBSN();
        private readonly ChatDAL _chatDAL = new ChatDAL();

        public ObjectId InsertChatRoom(
            ObjectId ParticipantOne,
            ObjectId ParticipantTwo,
            string RoomAvatarUrl = "",
            string RoomName = "")
        {
            var Room = GetUniqueChatRoomByParticipants(ParticipantOne, ParticipantTwo);
            if (Room != null)
            {
                return Room.Id;
            }

            var ChatRoom = new ChatRoomModel
            {
                DateCreate = DateTime.Now,
                DateLastInteraction = DateTime.Now,
                RoomName = RoomName,
                CreatorRoom = ParticipantOne,
                ParticipantOne = ParticipantOne,
                ParticipantTwo = ParticipantTwo,
                RoomAvatarUrl = RoomAvatarUrl
            };

            return _chatDAL.InsertChatRoom(ChatRoom);
        }

        public void AlterChatRoom(ObjectId RoomId, ChatMessagesModel Message)
        {
            _chatDAL.AlterChatRoom(RoomId, Message);
        }

        public void ReadChatRoom(ObjectId RoomId)
        {
            _chatDAL.ReadChatRoom(RoomId);
        }

        public void ReadChatMessage(ObjectId RoomId, ObjectId UserId)
        {
            _chatDAL.ReadChatMessage(RoomId, UserId);
        }

        public bool InsertMessage(ObjectId RoomId, ObjectId SenderId, string Message_Body, bool Message_Read)
        {
            var Message = new ChatMessagesModel
            {
                DateCreate = DateTime.Now,
                DateLastInteraction = DateTime.Now,
                RoomId = RoomId,
                SenderId = SenderId,
                Message_Body = Message_Body,
                Message_Read = Message_Read
            };

            AlterChatRoom(RoomId, Message);
            return _chatDAL.InsertMessage(Message);
        }

        public ChatRoomModel GetUniqueChatRoomByParticipants(ObjectId ParticipantOne, ObjectId ParticipantTwo)
        {
            return _chatDAL.GetUniqueChatRoomByParticipants(ParticipantOne, ParticipantTwo);
        }

        public List<ChatRoomModel> GetListChatRoomByUser(ObjectId UserId)
        {
            return _chatDAL.GetListChatRoomByUser(UserId);
        }

        public List<ChatMessagesModel> GetListChatRoomMassagesByUser(ObjectId RoomId)
        {
            return _chatDAL.GetListChatRoomMassagesByUser(RoomId);
        }

        public int GetListChatRoomMassagesByUser(ObjectId RoomId, ObjectId UserId)
        {
            return _chatDAL.GetListChatRoomMassagesByUser(RoomId, UserId);
        }

        public int GetCountMessagesNotReadByUser(ObjectId UserId)
        {
            var listRoom = GetListChatRoomByUser(UserId);
            int cont = 0;
            foreach (var Room in listRoom)
            {
                cont += GetListChatRoomMassagesByUser(Room.Id, UserId);
            }

            return cont;
        }

        public ChatMessagesModel GetLastChatMassagesByRoomId(ObjectId RoomId)
        {
            return _chatDAL.GetLastChatMassagesByRoomId(RoomId);
        }
    }
}
