using System;
using System.Collections.Generic;
using System.Text;
using Training_App;
using View_Models;


namespace MainIenterfaces
{
    public interface IRegistrationInfo
    {
        bool InsertRegistrationDetails(UserCredentialsVM userCredentialsVm);
        bool IsEmailExist(string emailID);
        bool SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount", string controllerFor = "MailVerfication");
        bool VerifyAccount(string id);
    }
}
