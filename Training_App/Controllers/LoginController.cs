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
    public class LoginController : ApiController
    {
        ILoginInfo loginInfo = new LoginInfo();
        // GET: api/Login
        [HttpPost]
        public HttpResponseMessage Post(LoginVM login)
        {
            try
            {
                var user = loginInfo.GetUser(login);
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid User", Configuration.Formatters.JsonFormatter);

                }
                else
                {
                    string token = loginInfo.CreateJwtToken(user);
                    if (token != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, token);
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Token is not generated");

                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
