using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_API_Kafka.Models
{
    public class ResourceGetRequestModel
    {
        public Header header { get; set; }
        public Body body { get; set; }

        public class Header
        {
            public int requestId { get; set; }
            public string requestType { get; set; }
            public Producer producer { get; set; }
            public string version { get; set; }
            public DateTime dateMsg { get; set; }
        }

        public class Producer
        {
            public string id { get; set; }
        }

        public class Body
        {
            public DateTime dates { get; set; }
            public ItemSet itemSet { get; set; }

        }

        public class ItemSet
        {
            public Items items { get; set; }
        }

        public class Items
        {
            public long? id { get; set; }
        }
    }
}