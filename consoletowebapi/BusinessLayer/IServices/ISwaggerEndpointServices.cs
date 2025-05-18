using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.BusinessLayer.IServices
{
    public interface ISwaggerEndpointServices
    {
        Task FetchAndStoreEndpoints();
        (string controller, string method) ExtractControllerAndAction(string path);
    }
}
