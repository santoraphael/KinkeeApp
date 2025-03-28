using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mongo.BSN;
using Mongo.Models;

namespace Plataforma.Helper
{
    public class LocationHelper
    {
        LocationBSN location = new LocationBSN();
        public IList<CountriesModel> ListaPaises()
        {
            CountriesModel countriesModel = new CountriesModel();

            countriesModel.Country_str_code = "BR";
            countriesModel.Country_str_name = "Brasil";

            List<CountriesModel> paises = location.ListCountries();
            paises.Insert(0, countriesModel);


            return paises;
        }


        public IList<StatesModel> ListaEstados(string cod_pais)
        {
            List<StatesModel> estados = location.ListStates(cod_pais);

            return estados;
        }


        public IList<CitiesModel> ListaCidades(string cod_estado)
        {
            List<CitiesModel> cidades = location.ListCities(cod_estado);

            return cidades;
        }
    }
}