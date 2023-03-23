using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;

namespace DotnetRpg.Dto
{
    public class CharacterDto
    {
        [Required]
        public string? Name { get; set; } 
        [Range(100,1000)]
        public int Blood { get; set; } = 100 ;

        public int Mana { get; set; } = 100 ;

        public int Strength { get; set; } = 100 ;
        public int Hit { get; set; } = 100 ;

        [Required]
        public JobClass Class { get; set; }
           
    }
}