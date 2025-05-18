using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class JobParameter
    {
        public long JobId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public virtual Job Job { get; set; }
    }
}
