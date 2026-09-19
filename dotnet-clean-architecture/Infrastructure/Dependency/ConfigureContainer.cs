using Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Dependency
{
    public static class ConfigureContainer
    {
        public static void UseCustomExceptionHandling(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<CustomExceptionMiddleware>();
        } 
    }
}
