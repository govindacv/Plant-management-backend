using Dapper;
using EmployeeProject.Core.Models;
using PlantManagement.Core.Interfaces;
using PlantManagement.Core.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;

namespace PlantManagement.Data.Repository
{
    public class UserRepository : IUserInterface
    {
        IDbConnection dbConnection = new SqlConnection(AppSettings.ConnectionString.DevOps);

        public int AddUser(User user)
        {
            
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("USERNAME", user.UserName);
            dynamicParameters.Add("USEREMAIL", user.UserEmail);
            dynamicParameters.Add("USERPASSWORD", BCrypt.Net.BCrypt.HashPassword(user.UserPassword));
            dynamicParameters.Add("OUTPUT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dbConnection.Query<int>("AddUsers", dynamicParameters, commandType: CommandType.StoredProcedure);
            int output = dynamicParameters.Get<int>("OUTPUT");

            Console.WriteLine(output);
            return output;


        }

        public string Login(Login signUp)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("USEREMAIL", signUp.UserEmail);
            dynamicParameters.Add("USERPASSWORD", signUp.UserPassword);
            dynamicParameters.Add("MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);

            dbConnection.Query<string>("Login", dynamicParameters, commandType: CommandType.StoredProcedure).ToString();

            string message = dynamicParameters.Get<string>("MESSAGE");

             
             if(message.Equals("INCORRECT EMAIL")) { return message; }
             else
            {
                if (BCrypt.Net.BCrypt.Verify(signUp.UserPassword, message))
                { 
                    return "USER FOUND"; 
                }
                else

                {
                    return "INCORRECT PASSWORD";
                }
            }



           

        }

        public int isValidEmail(string email)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("USEREMAIL",email);
            dynamicParameters.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dbConnection.Query<string>("isValidEmail", dynamicParameters, commandType: CommandType.StoredProcedure);

           int result= dynamicParameters.Get<int>("RESULT");
            return result;

        }
        public int GeneratedOTP(string email)
        {
            Random random = new Random();
            int otp = random.Next(100001, 999999);
            SendMail(email, otp);
            return otp;
        }
        public void SendMail(string email, int OTP)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("govindachamrajnagara.venkatesh@talentpace.com");
                    mailMessage.To.Add(email);

                    mailMessage.Subject = "Plant Management OTP Verification";
                    mailMessage.Body = $"Your Email is {email}. Your Verification OTP is {OTP}.";
                    mailMessage.IsBodyHtml = true;  

                    using (var smtpClient = new SmtpClient("smtp.outlook.com", 587))
                    {
                         smtpClient.Credentials = new NetworkCredential("govindachamrajnagara.venkatesh@talentpace.com", "Talentpace@123");
                        smtpClient.EnableSsl = true;  
                        smtpClient.Send(mailMessage);
                    }
                }

                Console.WriteLine("Email sent successfully.");
                updateTheOTP(OTP, email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public int changePassword(ChangePassword changePassword)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("USEREMAIL", changePassword.UserEmail);
            dynamicParameters.Add("USERPASSWORD", BCrypt.Net.BCrypt.HashPassword(changePassword.UserPassword));
            dynamicParameters.Add("RESULT", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dbConnection.Query<string>("updatePassword", dynamicParameters, commandType: CommandType.StoredProcedure);

            int result = dynamicParameters.Get<int>("RESULT");
            if(result == 1)
            {
                SendMailAfterPasswordChange(changePassword.UserEmail, changePassword.UserPassword);
            }
            return result;
        }

        public void SendMailAfterPasswordChange(string email ,string newPassword)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("govindachamrajnagara.venkatesh@talentpace.com");
                    mailMessage.To.Add(email);

                    mailMessage.Subject = $"Your password has been updated ";
                    mailMessage.Body = $".The new password is {newPassword}";
                    mailMessage.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient("smtp.outlook.com", 587))
                    {
                        smtpClient.Credentials = new NetworkCredential("govindachamrajnagara.venkatesh@talentpace.com", "Talentpace@123");
                        smtpClient.EnableSsl = true;
                        smtpClient.Send(mailMessage);
                    }

                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public int updateTheOTP(int otp,string userEmail)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@OTP",otp);
            dynamicParameters.Add("@USEREMAL", userEmail);
            dynamicParameters.Add("@RESULT",dbType:DbType.Int32,direction:ParameterDirection.Output);

            dbConnection.Query<int>("updateOTP", dynamicParameters, commandType: CommandType.StoredProcedure);
            int result = dynamicParameters.Get<int>("@RESULT");
            return result;
        }


      public int getOTPOfUser(string userEmail)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@USEREMAIL", userEmail);
            dynamicParameters.Add("@OTP", dbType: DbType.Int32, direction: ParameterDirection.Output);

            dbConnection.Query<int>("getOTP",dynamicParameters, commandType: CommandType.StoredProcedure);
            int result = dynamicParameters.Get<int>("@OTP");
            return result;
        }




    }
     
}

