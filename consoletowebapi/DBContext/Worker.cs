using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}
