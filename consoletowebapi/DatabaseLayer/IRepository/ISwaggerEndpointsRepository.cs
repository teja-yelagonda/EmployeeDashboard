using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.DatabaseLayer.IRepository
{
    public interface ISwaggerEndpointsRepository
    {
        int StoreEndpointsInDatabase(List<(string controller, string method)> endpoints);
        int DeleteEndpoints(List<(string controller, string method)> endpoints);
        List<string> GetAllowedRoles(string controller, string method);
        int UpdateEndpointRoles(UpdateEndPointRoles updateRoles);
    }
}
