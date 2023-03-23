using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetRpg.Core
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public T? Data { get; set; } 
        public ErrorCode Code { get; set; } = ErrorCode.None;
        public string? Message { get; set; }



    }
}