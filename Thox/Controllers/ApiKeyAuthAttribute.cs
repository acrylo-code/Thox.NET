using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Thox.Data;

namespace Thox.Controllers
{
    public class ApiKeyAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Retrieve API key from request headers
            if (!context.HttpContext.Request.Headers.TryGetValue("Api-Key", out var apiKeyValues))
            {
                context.Result = new UnauthorizedResult(); // No API key provided, unauthorized
                return;
            }

            // Extract the API key value from StringValues
            string apiKey = apiKeyValues.FirstOrDefault();

            var dbContext = context.HttpContext.RequestServices.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;

            // Check if the API key exists in the database
            if (!await dbContext.ApiKeys.AnyAsync(k => k.Key == apiKey))
            {
                context.Result = new UnauthorizedResult(); // Invalid API key, unauthorized
                return;
            }
        }
    }
}