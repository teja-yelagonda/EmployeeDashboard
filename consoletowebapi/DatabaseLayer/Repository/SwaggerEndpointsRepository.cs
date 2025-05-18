using consoletowebapi.DatabaseLayer.IRepository;
using consoletowebapi.DBContext;
using consoletowebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.DatabaseLayer.Repository
{
    public class SwaggerEndpointsRepository: ISwaggerEndpointsRepository
    {
        private readonly OrganizationContext _context;
        public SwaggerEndpointsRepository(OrganizationContext context)
        {
             _context = context;
        }
        public int StoreEndpointsInDatabase(List<(string controller, string method)> endpoints)
        {
            foreach(var endpoint in endpoints)
            {
                bool isExist = _context.EndPointPermissions.Any(x => x.ControllerName == endpoint.controller && x.ActionName == endpoint.method);
                if (!isExist)
                {
                    EndPointPermission endpointsPermission = new EndPointPermission
                    {
                        ControllerName = endpoint.controller,
                        ActionName = endpoint.method,
                        Roles = "Admin"
                    };

                    _context.EndPointPermissions.Add(endpointsPermission);
                }
            }
            int rowsEffected = _context.SaveChanges();
            return rowsEffected;
        }
        public int DeleteEndpoints(List<(string controller, string method)> endpoints)
        {
            List<EndPointPermission> currentEndpoints = endpoints.Select(x=>new EndPointPermission { ControllerName=x.controller,ActionName=x.method}).ToList();
            
            List<EndPointPermission> DatabaseEndpoints = _context.EndPointPermissions.ToList();
            List<EndPointPermission> recordsToDelete = new List<EndPointPermission>();
            foreach(var endpoint in DatabaseEndpoints)
            {
                EndPointPermission record= currentEndpoints.FirstOrDefault(x => x.ControllerName == endpoint.ControllerName && x.ActionName == endpoint.ActionName);
                if (record==null)
                {
                    recordsToDelete.Add(endpoint);
                }
            }
            int rowsEffected = 0;
            if (recordsToDelete.Count>0)
            {
                _context.RemoveRange(recordsToDelete);
                rowsEffected = _context.SaveChanges();
            }
            return rowsEffected;
        }
        public List<string> GetAllowedRoles(string controller,string method)
        {
            string roles = _context.EndPointPermissions.FirstOrDefault(x => x.ControllerName == controller && x.ActionName == method).Roles;
            List<string> allowedRoles = roles.Split(',').ToList();
            return allowedRoles;
        }
        public int UpdateEndpointRoles(UpdateEndPointRoles updateRoles)
        {
            var existingEndpoints = _context.EndPointPermissions.FirstOrDefault(x => x.ControllerName == updateRoles.ControllerName && x.ActionName==updateRoles.ActionName);
            existingEndpoints.Roles = string.Join(",",new[] { existingEndpoints.Roles,updateRoles.Roles });
            _context.Update(existingEndpoints);
            int rowsEffected=_context.SaveChanges();
            return rowsEffected;
        }
    }
}
