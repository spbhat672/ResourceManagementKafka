using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM_WithKafka.Utils
{
    public static class ServiceTopics
    {
        public const string RequestTopic = "ResourceManagementRequests";
        public const string ResponseTopic = "ResourceManagementResponse";
        public const string GetResource = "dotGetResource";
        public const string PostResource = "dotPostResource";
        public const string PutResource = "dotPutResource";
        public const string DeleteResource = "dotDeleteResource";
        public const string GetTypesList = "dotGetTypesList";
        public const string GetStatusList = "dotGetStatusList";
    }
}