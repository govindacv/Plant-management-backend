using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeProject.Core.Models
{
    public class AppSettings
    {
        public static ConnectionString ConnectionString { get; set; } = new ConnectionString(); 
    }
    public class ConnectionString
    {
        public string DevOps { get; set; }
        public string InetData { get; set; }
        public string TransregData { get; set; }
    }
}
