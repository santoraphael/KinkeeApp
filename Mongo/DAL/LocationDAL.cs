using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.BSN
{
    public class LocationDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AdminCountries = "Admin.Countries";
        private readonly string AdminStates = "Admin.States";
        private readonly string AdminCities = "Admin.Cities";

        #region CATEGORY

        public bool InsertCountry(CountriesModel newCountry)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CountriesModel>(AdminCountries);
            try
            {
                collection.InsertOne(newCountry);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertState(StatesModel newState)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<StatesModel>(AdminStates);
            try
            {
                collection.InsertOne(newState);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertCity(CitiesModel newCity)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CitiesModel>(AdminCities);
            try
            {
                collection.InsertOne(newCity);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<CountriesModel> ListCountries()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CountriesModel>(AdminCountries);

            try
            {
                return collection.AsQueryable().ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<StatesModel> ListStates(string cd_coutry)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<StatesModel>(AdminStates);

            try
            {
                return collection.AsQueryable()
                                 .Where(c => c.Country_str_code == cd_coutry)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<CitiesModel> ListCities(string cd_state)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CitiesModel>(AdminCities);

            try
            {
                return collection.AsQueryable()
                                 .Where(c => c.Admin1_str_code == cd_state)
                                 .OrderByDescending(c => c.capital)
                                 .ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        #endregion
    }
}
