using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_Server_WithKafka.RM_Server.Models
{
    public class Location
    {
        public long Id { get; set; }

        public decimal X { get; set; }

        public decimal Y { get; set; }

        public decimal Z { get; set; }

        public decimal Rotation { get; set; }
    }
}