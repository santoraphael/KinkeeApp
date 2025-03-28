using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plataforma.Helper
{
    public class LocationUf
    {
        public LocationUf(string siglaUf, string nome)
        {
            this.Nome = nome;
            this.SiglaUf = siglaUf;
        }
        public string SiglaUf { get; set; }
        public string Nome { get; set; }
    }

    public static class UfRepository
    {
        public static IList<LocationUf> ListaUf()
        {
            List<LocationUf> cidades = new List<LocationUf>();
            cidades.Add(new LocationUf("SP", "São Paulo"));
            cidades.Add(new LocationUf("RJ", "Rio de Janeiro"));
            cidades.Add(new LocationUf("MG", "Minas Gerais"));
            return cidades;
        }
    }
}