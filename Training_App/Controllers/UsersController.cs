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
using Training_App.Helper;
using Training_App.MyFilter;


namespace Training_App.Controllers
{
    public class UsersController : ApiController
    {
        IUserInfo userInfo = new UserInfo();
        // GET: api/Users
        [HttpGet]
        [JwtTokenAuthentication]

        public HttpResponseMessage Get()
        {
            try
            {
                var users = userInfo.GetUsersDetails();
                return Request.CreateResponse(HttpStatusCode.OK, users);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        
        

        // GET: api/Users/5
        

        // POST: api/Users
        [HttpPost]
        [JwtTokenAuthentication]

        public HttpResponseMessage Post(string emailId)
        {
            try
            {
                bool status = userInfo.SendMailForForgotPasswordLink(emailId);
                if (status)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Link is sent to mail and reset your password via link");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Something went wrong");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/Users/5
        

        // DELETE: api/Users/5
        
    }
}
