using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantManagement.Core.Models
{
    public class Plant
    {
        public string PlantName { get; set; }
         public string IsWareHouse { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string IsTransportationAvailable { get; set; }

        public string TransporterName { get; set; }
        
        public string PhoneNumber { get; set; }

        public string PlantPhoto { get; set; } 


    }
}
