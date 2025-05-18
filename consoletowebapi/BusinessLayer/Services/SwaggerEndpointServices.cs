using consoletowebapi.BusinessLayer.IServices;
using consoletowebapi.DatabaseLayer.IRepository;
using consoletowebapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace consoletowebapi.BusinessLayer.Services
{
    public class SwaggerEndpointServices
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RequestDelegate _next;
        private readonly IOptions<SwaggerSettings> _options;
        public SwaggerEndpointServices(IServiceScopeFactory serviceScopeFactory,RequestDelegate next, IOptions<SwaggerSettings> options)
        {
            _options = options;
            _serviceScopeFactory = serviceScopeFactory;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await FetchAndStoreEndpoints();
            PathString routeData = context.Request.Path;
            bool isAuthorized=await AuthorizeUser(routeData,context);

            if (isAuthorized)
            {
                await _next(context);
            }
        }
        public async Task FetchAndStoreEndpoints()
        {
            using (var httpClient=new HttpClient())
            {
                string response = await httpClient.GetStringAsync(_options.Value.SwaggerUrl);
                List<string> paths = GetPaths(response);

                List<(string controller, string ActionMethod)> endpoints =ExtractControllerAndAction(paths);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var swaggerRepository = scope.ServiceProvider.GetRequiredService<ISwaggerEndpointsRepository>();
                    int insertedCount = swaggerRepository.StoreEndpointsInDatabase(endpoints);
                    int deletedCount = swaggerRepository.DeleteEndpoints(endpoints);
                }

            }
        }
        public static List<string> GetPaths(string jsonString)
        {
            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                if (document.RootElement.TryGetProperty("paths", out JsonElement pathsElement) && pathsElement.ValueKind == JsonValueKind.Object)
                {
                    var paths = new List<string>();
                    foreach (JsonProperty property in pathsElement.EnumerateObject())
                    {
                        paths.Add(property.Name);
                    }
                    return paths;
                }
                else
                {
                    return null;
                }
            }
        }
        public List<(string controller, string method)> ExtractControllerAndAction(List<string> paths)
        {
            List<(string controller, string method)> endpoints = new List<(string controller, string method)>();
            foreach(string path in paths)
            {
                string[] routeParts = path.Substring("/api".Length).Trim('/').Split('/');
                string controller = routeParts[0].Length > 0 ? routeParts[0] : "unknown";
                string method = routeParts[1].Length > 0 ? routeParts[1] : "Index";
                var endpoint = (controller, method);
                endpoints.Add(endpoint);
            }
            
            return endpoints;
        }
        public async Task<bool> AuthorizeUser(PathString routeData,HttpContext context)
        {
            string[] routeParts = routeData.Value.Substring("/api".Length).Trim('/').Split('/');
            string controller = routeParts[0];
            string method = routeParts[1];

            if (controller == "auth")
            {
                return true;
            }
            if (!string.IsNullOrEmpty(controller) && !string.IsNullOrEmpty(method))
            {
                List<string> allowedRoles = new List<string>();
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    ISwaggerEndpointsRepository swaggerRepository = scope.ServiceProvider.GetRequiredService<ISwaggerEndpointsRepository>();
                    allowedRoles = swaggerRepository.GetAllowedRoles(controller, method);
                }
                List<string> userRoles = context.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                if (!userRoles.Any(role => allowedRoles.Contains(role)))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access Denied: Insufficient permissions");
                    return false;
                }
                return true;
            }
            return false;
        }


    }
}
