using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using View_Models;
using DataModel;
using MainIenterfaces;
using Services;

namespace Training_App.Controllers
{
    public class RegistrationsController : ApiController
    {
        IRegistrationInfo registrationInfo = new RegistrationInfo();

        

        // POST: api/Registrations
        [HttpPost]
        public HttpResponseMessage Post(UserCredentialsVM userCredentials)
        {
            try
            {
                #region//Email already exists
                var isExists = registrationInfo.IsEmailExist(userCredentials.Email);
                if (isExists)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Ambiguous, "Email already Exists in please change  email"); 
                }
                #endregion

                #region//Save Data to Database
                bool status = registrationInfo.InsertRegistrationInfo(userCredentials);
                #endregion
                if (status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Registration successfull please verify through email");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Registration unsuccessfull");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        
    }
}
