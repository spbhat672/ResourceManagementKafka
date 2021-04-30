using Newtonsoft.Json;
using RM_WithKafka.Models;
using RM_WithKafka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;

namespace RM_WithKafka.WebMethod
{
    public class ServiceRepository
    {
        public static List<ResourceWithValue> GetAllResource()
        {
            List<ResourceWithValue> responseObj = new List<ResourceWithValue>();
            KafkaService.ProduceRequest(ServiceTopics.GetResource, null);
            //Thread.Sleep(500);
            string response = KafkaService.ConsumeResponse(ServiceTopics.GetResource);
            responseObj = JsonConvert.DeserializeObject<List<ResourceWithValue>>(response);
            return responseObj;
        }

        public static void SaveResource(ResourceWithValue resModel)
        {
            string requestStr = JsonConvert.SerializeObject(resModel);
            KafkaService.ProduceRequest(ServiceTopics.PostResource, requestStr);
        }

        public static void UpdateResource(ResourceWithValue resModel)
        {
            string requestStr = JsonConvert.SerializeObject(resModel);
            KafkaService.ProduceRequest(ServiceTopics.PutResource, requestStr);
        }

        public static void DeleteResource(long id)
        {
            KafkaService.ProduceRequest(ServiceTopics.DeleteResource, id.ToString());
        }

        public static List<Models.Type> GetTypeList()
        {
            List<Models.Type> responseObj = new List<Models.Type>();
            KafkaService.ProduceRequest(ServiceTopics.GetTypesList, null);
            //Thread.Sleep(500);
            string response = KafkaService.ConsumeResponse(ServiceTopics.GetTypesList);
            responseObj = JsonConvert.DeserializeObject<List<Models.Type>>(response);
            return responseObj;
        }

        public static List<Models.Status> GetStatusList()
        {
            List<Status> responseObj = new List<Status>();
            KafkaService.ProduceRequest(ServiceTopics.GetStatusList, null);
            //Thread.Sleep(500);
            string response = KafkaService.ConsumeResponse(ServiceTopics.GetStatusList);
            responseObj = JsonConvert.DeserializeObject<List<Status>>(response);
            return responseObj;
        }
    }
}