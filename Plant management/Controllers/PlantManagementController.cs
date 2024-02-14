using EmployeeProject.Core.Models;
using Microsoft.AspNetCore.Mvc;
using PlantManagement.Core.Interfaces;
using PlantManagement.Core.Models;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Linq;

namespace Plant_management.Controllers
{
    public class PlantManagementController : Controller
    {
        private readonly IUserInterface userInterface;
        private readonly IAddPlantRepository plantRepository;

        private readonly IWebHostEnvironment webHostEnvironment;
        public PlantManagementController(IUserInterface userInterface, IAddPlantRepository plantRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.userInterface = userInterface;
            this.plantRepository = plantRepository;
            this.webHostEnvironment = webHostEnvironment;
        }
        [Route("/signup")]
        [HttpPost]
        public int AddUser([FromBody] User user)
        {
            return userInterface.AddUser(user);
        }

        [Route("/login")]
        [HttpPost]
        public string SignUp([FromBody] Login signUp)
        {
            return userInterface.Login(signUp);
        }


        [Route("/getcountries")]
        [HttpGet]

        public List<Country> GetCountries()
        {
            return plantRepository.GetCountries();
        }

        [Route("/getstates/{countryName}")]
        [HttpGet]
        public List<State> GetStates(string countryName)
        {
            return plantRepository.GetStatesByCountry(countryName);
        }

        [Route("/getcities/{stateName}")]
        [HttpGet]
        public List<City> GetCity(string stateName)
        {
            return plantRepository.GetCitiesByState(stateName);
        }


        [Route("/addplant")]
        [HttpPost]

        public int AddPlant([FromBody] Plant plant)
        {
            return plantRepository.AddPlant(plant);
        }
        [Route("/getplants")]
        [HttpGet]
        public IList GetPlants()
        {
            return plantRepository.GetPlants();
        }
        [Route("/editplant")]
        [HttpPost]
        public int updatePlant([FromBody] Plant plant)
        {
            return plantRepository.updatePlant(plant);
        }

        [Route("/isValidEmail")]
        [HttpGet]

        public int isValidEmail(string userEmail)
        {
            return userInterface.isValidEmail(userEmail);
        }

        [Route("/sendotp")]
        [HttpGet]
        public int SendOtp(string email)
        {


            return userInterface.GeneratedOTP(email);

        }

        [Route("/updatepassword")]
        [HttpPost]

        public int updatePassword([FromBody] ChangePassword changePassword)
        {
            return userInterface.changePassword(changePassword);
        }
        [Route("/deleteplant")]
        [HttpPost]
        public IList deletePlant(string plantName)
        {
            return plantRepository.deletePlant(plantName);
        }
        [Route("/uploadimage")]
        [HttpPost]
        public async Task<int> UploadImage(List<IFormFile> formFiles, string plantName)
        {
            List<object> arrOfImages = new List<object>();
            int plantId = int.Parse(plantName);
            string UploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot", "Uploads", "PlantImages");

            if (!Directory.Exists(UploadsFolder))
            {
                Directory.CreateDirectory(UploadsFolder);
            }

            foreach (var formFile in formFiles)
            {
                string FileName = Path.GetFileName(formFile.FileName);
                long fileSize = formFile.Length;
                string fileType = formFile.ContentType;

                string uniqueFileName = $"{plantName}_{FileName}";
                string imagePath = Path.Combine(UploadsFolder, uniqueFileName);

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                string imagePathFromBE = "/Uploads/PlantImages/" + uniqueFileName;

                // Create a dictionary to hold path, size, and type
                var imageInfo = new Dictionary<string, object>
        {
            { "pathOfImage", imagePathFromBE },
            { "size", fileSize },
            { "type", fileType }
                   

        };

                arrOfImages.Add(imageInfo);
            }

            // Serialize the list of dictionaries
            string json = JsonSerializer.Serialize(arrOfImages);

            return plantRepository.uploadImage(json, plantId);
        }


        [Route("/getotp")]
        [HttpGet]

        public int getOTP(string userEmail)
        {
            return userInterface.getOTPOfUser(userEmail);
        }

        [Route("/getid")]
        [HttpGet]

        public int GetPlantIdByPlantName(string plantName)
        {
            return plantRepository.GetPlantIdByPlantName(plantName);
        }

        [Route("/getimagedetails")]
        [HttpGet]
        public List<ImageDetails> GetPlantImageDetails(int plantId)

        {
            return plantRepository.GetPlantImageDetails(plantId);

        }


    }
}
