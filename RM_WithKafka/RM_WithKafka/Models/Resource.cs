using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_WithKafka.Models
{
    public class Resource
    {
        public long Id { get; set; }

        public int TypeId { get; set; }

        public int StatusId { get; set; }

        public int LocationId { get; set; }

        public string Name { get; set; }
    }
}