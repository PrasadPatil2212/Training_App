using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Training_App.MyFilter
{
    public class JwtTokenAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsUserAuthorized(actionContext))
            {
                ShowAuthenticationError(actionContext);
                return;
            }
        }

        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            try
            {
                var idToken = actionContext.Request.Headers.Authorization.Parameter;
                if (idToken != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(idToken) as JwtSecurityToken;
                    var SymmetricKey = Encoding.ASCII.GetBytes("SuperKeySnnhddhdhdhdhdhdhdhdhdhdhddhhdhd");
                    var validationParameters = new TokenValidationParameters()
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(SymmetricKey)
                    };
                    // var simplePrinciple = GetPrincipal(idToken);
                    SecurityToken securityToken;
                    var principal = tokenHandler.ValidateToken(idToken, validationParameters, out securityToken);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unable to access, Please login again");
        }
    }
}