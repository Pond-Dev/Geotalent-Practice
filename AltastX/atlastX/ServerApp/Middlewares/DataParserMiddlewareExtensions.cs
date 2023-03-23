using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace atlastX.Middlewares
{
    public static class DataParserMiddlewareExtensions
    {
        public static IApplicationBuilder UseDataParser(this IApplicationBuilder builder, string endpoint, IEnumerable<string> paths)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException("The endpoint parameter is null or empty");
            }

            if (paths == null || !paths.Any())
            {
                throw new ArgumentNullException("The paths parameter is null or empty");
            }

            return builder.UseMiddleware<DataParserMiddleware>(endpoint, paths);
        }

    }
}
