using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_API_Kafka.Models
{
    public class ResourceResponseModel
    {
        public Header header { get; set; }
        public Body body { get; set; }

        public class Header
        {
            public SourceRequest sourceRequest { get; set; }
            public string messageType { get; set; }
            public string version { get; set; }            
            public DateTime dateMsg { get; set; }
        }

        public class SourceRequest
        {
            public int requestId { get; set; }
            public string requestType { get; set; }
            public string version { get; set; }
        }

        public class Body
        {
            public ResourceState resourceState { get; set; }
        }

        public class ResourceState
        {
            public string id { get; set; }
            public DateTime updateDataDate { get; set; }
            public ResourceWithValue tags { get; set; }
        }
    }
}