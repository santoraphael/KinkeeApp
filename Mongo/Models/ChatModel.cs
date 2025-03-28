using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class ChatRoomModel : BaseModel
    {
        public string RoomName { get; set; }
        public ObjectId CreatorRoom { get; set; }
        public ObjectId ParticipantOne { get; set; }
        public ObjectId ParticipantTwo { get; set; }
        public string RoomAvatarUrl { get; set; }
        public ChatMessagesModel LastMessage { get; set; }
    }

    public class ChatMessagesModel : BaseModel
    {
        public ObjectId RoomId { get; set; }
        public ObjectId SenderId { get; set; }
        public string Message_Body { get; set; }
        public bool Message_Read { get; set; }
    }

    public class ImageChatUserLogModel : BaseModel
    {
        public ObjectId SenderId { get; set; }
        public string ImageUrl { get; set; }
        public TypeImageSend TypeImageSend { get; set; }
    }
}