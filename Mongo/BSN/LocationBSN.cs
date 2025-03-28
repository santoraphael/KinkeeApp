using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using Mongo.Infrastruture.Helper;

namespace Mongo.BSN
{
    public class LocationBSN
    {
        LocationDAL locationDAL = new LocationDAL();


        #region INSERT

        public bool InsertCountry(CountriesModel newCountry)
        {
            return locationDAL.InsertCountry(newCountry);
        }

        public bool InsertState(StatesModel newState)
        {
            return locationDAL.InsertState(newState);
        }

        public bool InsertCity(CitiesModel newCity)
        {
            return locationDAL.InsertCity(newCity);
        }

        //public CategoryModel GetCategoryById(ObjectId categoryID)
        //{
        //    return storeDAL.GetCategoryById(categoryID);
        //}

        //public CategoryModel GetCategoryByName(string categoryName)
        //{
        //    return storeDAL.GetCategoryByName(categoryName);
        //}

        #endregion


        public List<CountriesModel> ListCountries()
        {
            return locationDAL.ListCountries();
        }

        public List<StatesModel> ListStates(string cd_coutry)
        {
            return locationDAL.ListStates(cd_coutry);
        }

        public List<CitiesModel> ListCities(string cd_state)
        {
            return locationDAL.ListCities(cd_state);
        }
    }
}
