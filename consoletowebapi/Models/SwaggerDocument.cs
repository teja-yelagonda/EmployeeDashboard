using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace consoletowebapi.Models
{
    public class SwaggerDocument
    {
        public Dictionary<string, string> paths { get; set; }
    }
    public class SwaggerSettings
    {
        public string SwaggerUrl { get; set; }
    }
}
