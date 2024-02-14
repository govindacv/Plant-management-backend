using EmployeeProject.Core.Models;
using PlantManagement.Core.Interfaces;
using PlantManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections;
using System.Text.Json;
using Microsoft.VisualBasic.FileIO;

namespace PlantManagement.Data.Repository
{
    public class AddPlantRepository : IAddPlantRepository
    {
        IDbConnection dBConnection = new SqlConnection(AppSettings.ConnectionString.DevOps);
        public List<Country> GetCountries()
        {
            var result = dBConnection.Query <Country>("GetCountries", commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public List<State> GetStatesByCountry(string   countryName)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("CountryName", countryName);
            var result = dBConnection.Query<State>("GetStatesByCountryName", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public List<City> GetCitiesByState(string stateName)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("StateName", stateName);

            var result = dBConnection.Query<City>("GetCitiesByStateName", dynamicParameters, commandType: CommandType.StoredProcedure).ToList();
            return result;
        }
 

public int AddPlant(Plant plant)
    {
        
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PLANTNAME", plant.PlantName);
            parameters.Add("@ISWAREHOUSE", plant.IsWareHouse);
            parameters.Add("@PLANTCOUNTRY", plant.Country);
            parameters.Add("@PLANTSTATE", plant.State);
            parameters.Add("@PLANTCITY", plant.City);
            parameters.Add("@ISTRANSPORTATIONAVAILABLE", plant.IsTransportationAvailable);
            parameters.Add("@PHONENUMBER", plant.PhoneNumber);
            
            parameters.Add("@TRANSPORTERNAME", plant.TransporterName);
            parameters.Add("@RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dBConnection.Query<int>("AddPlantDetails", parameters, commandType: CommandType.StoredProcedure);

            int result = parameters.Get<int>("@RESULT");
            return result;
         
    }

        public IList  GetPlants()
        {
           
            var result = dBConnection.Query ("GetPlants", commandType: CommandType.StoredProcedure).ToList();
            return result;
        }

        public int updatePlant(Plant plant)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PLANTNAME", plant.PlantName);
            parameters.Add("@ISWAREHOUSE", plant.IsWareHouse);
            parameters.Add("@PLANTCOUNTRY", plant.Country);
            parameters.Add("@PLANTSTATE", plant.State);
            parameters.Add("@PLANTCITY", plant.City);
            parameters.Add("@ISTRANSPORTATIONAVAILABLE", plant.IsTransportationAvailable);
            parameters.Add("@PHONENUMBER", plant.PhoneNumber);
            parameters.Add("@TRANSPORTERNAME", plant.TransporterName);
            parameters.Add("@RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dBConnection.Query<int>("EditPlantDetails", parameters, commandType: CommandType.StoredProcedure);

            int result = parameters.Get<int>("@RESULT");
            return result;
        }

        public IList deletePlant(string plantName)
        {
            DynamicParameters dynamicParameters= new DynamicParameters();
            dynamicParameters.Add("PLANTNAME", plantName);

           var result= dBConnection.Query("UpdatePlantStatus", dynamicParameters, commandType: CommandType.StoredProcedure).ToList() ;
            return result ;
        }
       public int uploadImage(string file,int plantId)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            
            dynamicParameters.Add("FILE", file);
            dynamicParameters.Add("@PLANTID", plantId);
            dynamicParameters.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dBConnection.Query<int>("updateFile",dynamicParameters,commandType: CommandType.StoredProcedure);

            int result = dynamicParameters.Get<int>("RESULT");
            return result;
        }


        public int GetPlantIdByPlantName(string plantName)
        {
            DynamicParameters   dynParameters= new DynamicParameters();
            dynParameters.Add("PLANTNAME", plantName);
            dynParameters.Add("PLANTID",dbType: DbType.Int32,direction:ParameterDirection.Output);

            dBConnection.Query<int>("getPlantIdByPlantName", dynParameters, commandType: CommandType.StoredProcedure);
            int result = dynParameters.Get<int>("PLANTID"); 
            return result;
        }

        public List<ImageDetails> GetPlantImageDetails(int plantId)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("PLANTID", plantId);
            var result=dBConnection.Query <ImageDetails>("GetPlantImageDetails",dynamicParameters,commandType:CommandType.StoredProcedure).ToList(); 
            return result;

        }

      public  string DeletingTheImagesBasedOnId(List<int> arrOfToBeDeletedId)
        {
            List<Object> arrOfJSONId = new List<Object>();
            foreach (int id in arrOfToBeDeletedId)
            {
                
                var JSONId = new Dictionary<string, object>
            {
            { "pathOfImageId", id }
            };
                arrOfJSONId.Add(JSONId);
            }

            string json = JsonSerializer.Serialize(arrOfJSONId);

             DynamicParameters dynamicParameters=new DynamicParameters();
            dynamicParameters.Add("JsonData", json);

            dBConnection.Query("toDeletePlantImages", dynamicParameters, commandType: CommandType.StoredProcedure);
            return json;


            



        }



    }
}
