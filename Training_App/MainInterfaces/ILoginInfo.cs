﻿using System;
using System.Collections.Generic;
using System.Text;
using Training_App;
using View_Models;

namespace MainIenterfaces
{
    
        public interface ILoginInfo
        {
            User GetUser(LoginVM login);
            string CreateJwtToken(User user);
        }

    
}
