using System;
using System.Collections.Generic;

namespace Plataforma.Models
{
    public class EstadoModel
    {
        public string UF { get; set; }
        public string Estado { get; set; }
        public List<String> Cidades { get; set; }
    }
}