using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using View_Models;
using MainIenterfaces;

namespace Services
{
    public class UserInfo : IUserInfo
    {
        public List<UsersVM> GetUsersDetails()
        {
            try
            {
                var userList = new List<UsersVM>();
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    //var data = dc1.Schedules.ToList();
                    var data = dc.Users.ToList();
                    foreach (var item in data)
                    {
                        UsersVM user = new UsersVM();
                        if (item.DeletedAt == null)
                        {
                            var name = string.Concat(item.FirstName + " " + item.LastName);
                            user.UserName = name;
                            user.UserId = item.UserId;
                            userList.Add(user);

                        }
                    }
                }
                return userList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SendMailForForgotPasswordLink(string emailId)
        {
            try
            {
                IRegistrationInfo details = new RegistrationInfo();
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    var account = dc.UserCredentials.Where(a => a.Email == emailId).FirstOrDefault();
                    if (account != null && account.IsEmailVerified == true)
                    {
                        //send mail to user
                        bool status = details.SendVerificationLinkEmail(account.Email, account.ActivationCode.ToString(), "VerifyPassword");
                        if (status)
                        {
                            dc.Configuration.ValidateOnSaveEnabled = false;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
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
