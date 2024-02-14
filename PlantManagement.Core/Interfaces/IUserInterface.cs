using PlantManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantManagement.Core.Interfaces
{
    public interface IUserInterface
    {
        int AddUser(User user);

        string Login(Login signUp);

        int isValidEmail(string email);

        int GeneratedOTP(string email);

        int changePassword(ChangePassword changePassword);

        int getOTPOfUser(string userEmail);


    }
}
