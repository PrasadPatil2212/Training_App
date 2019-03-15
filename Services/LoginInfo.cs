using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using View_Models;
using MainIenterfaces;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;

namespace Services
{
    public class LoginInfo : ILoginInfo
    {
        public User GetUser(LoginVM login)
        {
            try
            {
                using (TrainingAppEntities1 dc = new TrainingAppEntities1())
                {
                    var isValidUser = dc.UserCredentials.Where(a => a.Email == login.Email).FirstOrDefault();
                    if (isValidUser != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Password), isValidUser.Password) == 0 && (isValidUser.IsEmailVerified == true))
                        {
                            var userObj = dc.Users.Where(a => a.UserId == isValidUser.UserId).FirstOrDefault();
                            return userObj;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception )
            {
                return null;
            }
        }

        public string CreateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["TokenKey"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                      new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                      new Claim(ClaimTypes.Role,user.RoleId.ToString()),
                      new Claim(ClaimTypes.Name,string.Concat(user.FirstName + " " +user.LastName)),
                    }),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return tokenString;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
