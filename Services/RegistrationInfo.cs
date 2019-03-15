using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using View_Models;
using MainIenterfaces;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Services
{
    public class RegistrationInfo : IRegistrationInfo
    {
        public bool InsertRegistrationInfo(UserCredentialsVM userCredentials)
        {
            try
            {
                bool status = false;
                #region//Save Data To Database
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    User user = new User();
                    user.FirstName = userCredentials.FirstName;
                    user.LastName = userCredentials.LastName;
                    user.IsActive = true;
                    user.RoleId = 1;
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    var obj = dc.Users.Add(user);
                    dc.SaveChanges();

                    #region//Taking data fromviewmodel and assigning to database entity
                    UserCredential credentials = new UserCredential();
                    credentials.Email = userCredentials.Email;
                    

                    #region//Generate Activation
                    credentials.ActivationCode = Guid.NewGuid();
                    #endregion

                    #region//Password Hashing
                    credentials.Password = Crypto.Hash(userCredentials.Password);
                    #endregion

                    credentials.IsActivated = true;
                    credentials.IsEmailVerified = true;
                    credentials.UserId = obj.UserId;
                    dc.UserCredentials.Add(credentials);
                    dc.SaveChanges();
                    #endregion

                    #region//Send Email to User
                    SendVerificationLinkEmail(userCredentials.Email, credentials.ActivationCode.ToString());
                    #endregion
                    status = true;
                }
                #endregion
                return status;
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public bool IsEmailExist(String emailID)
        {
            try
            {
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    var result = dc.UserCredentials.Where(a => a.Email == emailID).FirstOrDefault();
                    return result == null ? false : true;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount", string controllerFor = "MailVerifications")
        {
            try
            {
                var verifyUrl = "/" + controllerFor + "/" + emailFor + "/" + activationCode;
                var link = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, verifyUrl);

                var fromEmail = new MailAddress("pdpatil231994@gmail.com", "DotNet Awesome");
                var toEmail = new MailAddress(emailID);
                var fromEmailPassword = "W72uzJ2s"; // Replace with actual password

                string subject = " ";
                string body = " ";
                if (emailFor == "VerifyAccount")
                {
                    subject = "Your account is successfully created!";
                    body = "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                       " successfully created. Please click on the below link to verify your account" +
                       " <br/><br/><a href='" + link + "'>" + link + "</a> ";
                }

                var smtp = new SmtpClient
                {
                    Host = "smpt.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                }) smtp.Send(message);

                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public bool VerifyAccount(string id)
        {
            try
            {
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    var compare = dc.UserCredentials.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                    if (compare != null)
                    {
                        compare.IsEmailVerified = true;
                        dc.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
