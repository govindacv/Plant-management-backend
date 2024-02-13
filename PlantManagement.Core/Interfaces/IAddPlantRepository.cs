using PlantManagement.Core.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlantManagement.Core.Interfaces
{
    public  interface IAddPlantRepository
    {
        List<Country> GetCountries();
        List<State> GetStatesByCountry(string countryName);
        List<City> GetCitiesByState(string stateName);

        int AddPlant(Plant plant);

        IList  GetPlants();
        int updatePlant(Plant plant);

        IList deletePlant(string plantName);

        int uploadImage(string file, string plantName);
    }
}
