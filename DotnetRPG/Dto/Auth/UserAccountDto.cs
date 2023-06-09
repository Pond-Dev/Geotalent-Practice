using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetRpg.Dto.Auth
{
    public class UserAccountDto
    {
        [Required]
        public string? UserName { get; set; }

        [Required,MinLength(8),MaxLength(20)]
        public string? Password { get; set; }
    }
}