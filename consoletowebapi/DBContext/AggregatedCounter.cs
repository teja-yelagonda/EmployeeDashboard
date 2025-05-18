using System;
using System.Collections.Generic;

#nullable disable

namespace consoletowebapi.DBContext
{
    public partial class AggregatedCounter
    {
        public string Key { get; set; }
        public long Value { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
