using System;
using System.Collections.Generic;
using System.Text;
using Training_App;
using View_Models;

namespace MainIenterfaces
{
    public interface IUserInfo
    {
        List<UsersVM> GetUsersDetails();
        bool SendMailForForgotPasswordLink(string emailId);
    }
}
