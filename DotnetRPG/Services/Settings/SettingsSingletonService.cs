using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using Microsoft.Extensions.Options;

namespace DotnetRpg.Services.Settings
{
    public class SettingsSingletonService : ISettingsSingletonService
    {
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly IOptions<ConnectionStrings> _connectionStrings;

        public SettingsSingletonService(IOptions<JwtSettings> jwtSettings,IOptions<ConnectionStrings> ConnectionStrings)
        {
            _jwtSettings = jwtSettings;
            _connectionStrings = ConnectionStrings;
        }

        public JwtSettings JwtSettings => _jwtSettings.Value;

        public ConnectionStrings ConnectionStrings => _connectionStrings.Value;


        
    }
}