using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_Server_WithKafka.RM_Server.Models
{
    public class ResourceWithValue
    {
        public long Id { get; set; }

        public int TypeId { get; set; }

        public string Type { get; set; }

        public int StatusId { get; set; }

        public string Status { get; set; }

        public long LocationId { get; set; }

        public Location LocationValue { get; set; }

        public string Name { get; set; }
    }
}