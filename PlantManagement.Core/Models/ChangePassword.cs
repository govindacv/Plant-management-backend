using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantManagement.Core.Models
{
    public class ChangePassword
    {
        public string UserEmail { get; set; }

        public string UserPassword { get; set; }
    }
}
