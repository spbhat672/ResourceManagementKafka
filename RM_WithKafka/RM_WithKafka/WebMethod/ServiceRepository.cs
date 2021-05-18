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
        private static string baseUrl = "https://localhost:44361/ResourceInfo/";

        public static List<ResourceWithValue> GetAllResource()
        {
            List<ResourceWithValue> responseObj = new List<ResourceWithValue>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string request = ModelDataConversion.DataModelToGetRequestModel(null);
                HttpResponseMessage response = client.GetAsync($"api/GetResource?id={null}").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Get the response
                    var resourceJsonString = response.Content.ReadAsStringAsync().Result;

                    // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                    var deserialized = JsonConvert.DeserializeObject(resourceJsonString, typeof(List<ResourceWithValue>));
                    responseObj = (List<ResourceWithValue>)deserialized;
                }
            }
            return responseObj;
        }

        public static ResourceWithValue SaveResource(ResourceWithValue resModel)
        {
            ResourceWithValue responseObj = new ResourceWithValue();
            using (var client = new HttpClient())
            {
                //string requestModel = ModelDataConversion.DataModelToRequestModel(resModel);
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.PostAsJsonAsync($"api/PostResource/", resModel).Result;


                if (response.IsSuccessStatusCode)
                {
                    // Get the response
                    var resourceJsonString = response.Content.ReadAsStringAsync().Result;

                    // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                    var deserialized = JsonConvert.DeserializeObject<ResourceResponseModel>(resourceJsonString);
                    responseObj = deserialized.body.resourceState.tags;
                }
            }
            return responseObj;
        }

        public static bool UpdateResource(ResourceWithValue resModel)
        {
            bool isUpdateSuccess = false;
            ResourceWithValue responseObj = new ResourceWithValue();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //string requestModel = ModelDataConversion.DataModelToRequestModel(resModel);
                HttpResponseMessage response = client.PutAsJsonAsync($"api/PutResource/", resModel).Result;

                if (response.IsSuccessStatusCode)
                {
                    isUpdateSuccess = true;
                }
            }
            return isUpdateSuccess;
        }

        public static bool DeleteResource(long id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.DeleteAsync("api/DeleteResource?id=" + id).Result;

                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
        }

        public static List<Models.Type> GetTypeList()
        {
            List<Models.Type> responseObj = new List<Models.Type>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"api/GetTypeList/").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Get the response
                    var resourceJsonString = response.Content.ReadAsStringAsync().Result;

                    // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                    var deserialized = JsonConvert.DeserializeObject(resourceJsonString, typeof(List<Models.Type>));
                    responseObj = (List<Models.Type>)deserialized;
                    responseObj.Insert(0, new Models.Type() { Id = -9999, Name = "Choose One Type" });
                }
            }
            return responseObj;
        }

        public static List<Models.Status> GetStatusList()
        {
            List<Models.Status> responseObj = new List<Models.Status>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync($"api/GetStatusList/").Result;

                if (response.IsSuccessStatusCode)
                {
                    // Get the response
                    var resourceJsonString = response.Content.ReadAsStringAsync().Result;

                    // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)
                    var deserialized = JsonConvert.DeserializeObject(resourceJsonString, typeof(List<Models.Status>));
                    responseObj = (List<Models.Status>)deserialized;
                    responseObj.Insert(0, new Models.Status() { Id = -9999, Name = "Choose One Status" });
                }
            }
            return responseObj;
        }
    }

    //    public static List<ResourceWithValue> GetAllResource()
    //    {
    //        List<ResourceWithValue> responseObj = new List<ResourceWithValue>();
    //        KafkaService.ProduceRequest(ServiceTopics.GetResource, null);
    //        //Thread.Sleep(500);
    //        string response = KafkaService.ConsumeResponse(ServiceTopics.GetResource);
    //        responseObj = JsonConvert.DeserializeObject<List<ResourceWithValue>>(response);
    //        return responseObj;
    //    }

    //    public static void SaveResource(ResourceWithValue resModel)
    //    {
    //        string requestStr = JsonConvert.SerializeObject(resModel);
    //        KafkaService.ProduceRequest(ServiceTopics.PostResource, requestStr);
    //    }

    //    public static void UpdateResource(ResourceWithValue resModel)
    //    {
    //        string requestStr = JsonConvert.SerializeObject(resModel);
    //        KafkaService.ProduceRequest(ServiceTopics.PutResource, requestStr);
    //    }

    //    public static void DeleteResource(long id)
    //    {
    //        KafkaService.ProduceRequest(ServiceTopics.DeleteResource, id.ToString());
    //    }

    //    public static List<Models.Type> GetTypeList()
    //    {
    //        List<Models.Type> responseObj = new List<Models.Type>();
    //        KafkaService.ProduceRequest(ServiceTopics.GetTypesList, null);
    //        //Thread.Sleep(500);
    //        string response = KafkaService.ConsumeResponse(ServiceTopics.GetTypesList);
    //        responseObj = JsonConvert.DeserializeObject<List<Models.Type>>(response);
    //        return responseObj;
    //    }

    //    public static List<Models.Status> GetStatusList()
    //    {
    //        List<Status> responseObj = new List<Status>();
    //        KafkaService.ProduceRequest(ServiceTopics.GetStatusList, null);
    //        //Thread.Sleep(500);
    //        string response = KafkaService.ConsumeResponse(ServiceTopics.GetStatusList);
    //        responseObj = JsonConvert.DeserializeObject<List<Status>>(response);
    //        return responseObj;
    //    }
}