using KafkaNet;
using KafkaNet.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RM_Server_WithKafka.RM_Server.Utils;
using Newtonsoft.Json;
using KafkaNet.Protocol;
using RM_Server_WithKafka.RM_Server.Models;
using Newtonsoft.Json.Linq;

namespace RM_Server_WithKafka.RM_Server.WebMethod
{
    public static class KafkaService
    {
        #region Service
        /// <summary>
        /// Invokes the service
        /// Consumer look for the request in the kafka server
        /// Based on the request, it calls the particular method to be served
        /// </summary>
        public async static Task InvokeService()
        {
            Console.WriteLine("Service started");
            string requestTopic = ServiceTopics.RequestTopic;            
            Uri uri = new Uri("http://localhost:9092");
            var requestOptions = new KafkaOptions(uri);
            var requestRouter = new BrokerRouter(requestOptions);
            var consumer = new Consumer(new ConsumerOptions(requestTopic, requestRouter));

            foreach (var message in consumer.Consume())
            {
                if(message.Value.ToString().StartsWith(requestTopic+ServiceTopics.GetResource))
                {
                    ServeGetResource(message.Value.ToString(), requestTopic, uri);
                }
                else if(message.Value.ToString().StartsWith(requestTopic + ServiceTopics.PostResource))
                {
                    ServePostAndPutResource(message.Value.ToString(), requestTopic, true);
                }
                else if(message.Value.ToString().StartsWith(requestTopic + ServiceTopics.PutResource))
                {
                    ServePostAndPutResource(message.Value.ToString(), requestTopic, false);
                }
                else if(message.Value.ToString().StartsWith(requestTopic + ServiceTopics.DeleteResource))
                {
                    ServeDeleteResource(message.Value.ToString(), requestTopic);
                }
                else if(message.Value.ToString().StartsWith(requestTopic + ServiceTopics.GetStatusList))
                {
                    ServeGetTpesAndStatus<Models.Status>(ServiceTopics.GetStatusList, uri);
                }
                else if (message.Value.ToString().StartsWith(requestTopic + ServiceTopics.GetTypesList))
                {
                    ServeGetTpesAndStatus<Models.Type>(ServiceTopics.GetTypesList, uri);
                }
            }
        }
        #endregion

        #region Service Methods
        /// <summary>
        /// serves Get Resource Methods
        /// returns single item if the id is mentioned
        /// otherwise returns list of resource items
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="reqTopic"></param>
        /// <param name="uri"></param>
        private static void ServeGetResource(string msg,string reqTopic,Uri uri)
        {
            string idStr = msg.Substring((reqTopic + ServiceTopics.GetResource).Length);
            long? id;
            if (idStr != null && idStr.Length > 0)
                id = Convert.ToInt64(idStr);
            else
                id = null;
            List<Models.ResourceWithValue> resourceList = ResourceRepository.GetResourceInfo(id);
            string response = JsonConvert.SerializeObject(resourceList);

            string responseTopic = ServiceTopics.ResponseTopic + ServiceTopics.GetResource;
            Message responseMsg = new Message(response);
            var responseOptions = new KafkaOptions(uri);
            var responseRouter = new BrokerRouter(responseOptions);
            var producer = new Producer(responseRouter);
            producer.SendMessageAsync(responseTopic, new List<Message> { responseMsg }).Wait();
        }

        /// <summary>
        /// Serves Post And Put resource Methods
        /// Used for Adding and Editing Resource
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="reqTopic"></param>
        /// <param name="isPost"></param>
        private static void ServePostAndPutResource(string msg,string reqTopic,bool isPost)
        {
            string subTopic = isPost ? ServiceTopics.PostResource : ServiceTopics.PostResource;
            string request = msg.Substring((reqTopic + subTopic).Length);
            ResourceWithValue resourceObj = JsonConvert.DeserializeObject<ResourceWithValue>(request);
            if (isPost)
                ResourceRepository.AddResourceInfo(resourceObj);
            else
                ResourceRepository.UpdateResourceInfo(resourceObj);
        }

        /// <summary>
        /// Serves Delete Resource Method
        /// Deletes the requested id' resource
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="reqTopic"></param>
        private static void ServeDeleteResource(string msg,string reqTopic)
        {
            string idStr = msg.Substring((reqTopic + ServiceTopics.DeleteResource).Length);
            if (idStr != null && idStr.Length > 0)
                ResourceRepository.DeleteResourceInfo(Convert.ToInt64(idStr));
        }

        /// <summary>
        /// Serves GetTypes And GetStatus Methods
        /// Writes Requested data into the kafka server using kafka producer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subTopic"></param>
        /// <param name="uri"></param>
        private static void ServeGetTpesAndStatus<T>(string subTopic,Uri uri)
        {
            List<T> itemList = new List<T>();
            string response = JsonConvert.SerializeObject(itemList);

            string responseTopic = ServiceTopics.ResponseTopic + subTopic;
            Message msg = new Message(response);
            var responseOptions = new KafkaOptions(uri);
            var responseRouter = new BrokerRouter(responseOptions);
            var producer = new Producer(responseRouter);
            producer.SendMessageAsync(responseTopic, new List<Message> { msg }).Wait();
        }
        #endregion
    }
}
