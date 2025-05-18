using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Models
{
    public class UpdateEndPointRoles
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Roles { get; set; }
    }
}
