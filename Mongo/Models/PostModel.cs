using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class PublicacaoModel : BaseModel
    {
        public ObjectId UsuarioPublicacaoID { get; set; }
        public string Publicacao { get; set; }
        public Int32 Likes { get; set; }
        public List<UsuarioCurtiuPublicacaoModel> usuarioCurtiuPublicacao { get; set; }
        public List<UsuarioComentarioPublicacaoModel> Comentarios { get; set; }
        public PostType PostType { get; set; }
    }

    public class UsuarioCurtiuPublicacaoModel
    {
        public string imagensPerfilUsuarioLike { get; set; }
        public string NomeUsuarioLike { get; set; }
    }

    public class UsuarioComentarioPublicacaoModel : BaseModel
    {
        public ObjectId UsuarioComentarioPublicacaoID { get; set; }
        public string NomeUsuarioComentario { get; set; }
        public string imagemPerfilUsuarioComentario { get; set; }
        public string Comentario { get; set; }
    }

    public enum PostType
    {
        PublicacaoPadrao = 0,
        FotoPerfil = 1,
        FotoGaleria = 2,
    }

}