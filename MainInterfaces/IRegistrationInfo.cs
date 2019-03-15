using System;
using System.Collections.Generic;
using System.Text;
using DataModel;
using View_Models;


namespace MainIenterfaces
{
    public interface IRegistrationInfo
    {
        bool InsertRegistrationInfo(UserCredentialsVM userCredentialsVm);
        bool IsEmailExist(string emailID);
        bool SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount", string controllerFor = "MailVerfication");
        bool VerifyAccount(string id);
    }
}
