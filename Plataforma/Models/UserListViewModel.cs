using System;
using System.Collections.Generic;

namespace Plataforma.Models
{
    public class UserListViewModel
    {
        public string id { get; set; }
        public string imagemPerfil { get; set; }
        public string Usuario { get; set; }
        public string CidadeUsuario { get; set; }
        public string IdadeUsuario { get; set; }
        public int QntFotos { get; set; }
        public int Elo { get; set; }
        public bool NewProfile { get; set; }
        public bool Verify { get; set; }
        public bool Premium { get; set; }
        public bool Online { get; set; }

        public string url_image_score { get; set; }
        public string name_sugar_score { get; set; }
        public int? sugar_score { get; set; }

        public bool displayActions { get; set; }
    }
}