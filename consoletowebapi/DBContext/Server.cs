using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class Server
    {
        public string Id { get; set; }
        public string Data { get; set; }
        public DateTime LastHeartbeat { get; set; }
    }
}
