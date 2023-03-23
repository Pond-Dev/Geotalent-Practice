using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;

namespace DotnetRpg.Services.AuthService
{
    public interface IAuthService
    {
        int Register(String username,string password);

        bool ExistUser(String username);
        string Login(String username,string password);
    }
}