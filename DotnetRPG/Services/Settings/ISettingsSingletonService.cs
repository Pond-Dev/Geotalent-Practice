using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;

namespace DotnetRpg.Services.Settings
{
    public interface ISettingsSingletonService
    {

        public JwtSettings JwtSettings {get;}

        public ConnectionStrings ConnectionStrings {get;}
        
    }
}