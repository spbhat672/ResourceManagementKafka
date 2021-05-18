using RM_API_Kafka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RM_API_Kafka.Utils;
using KafkaNet.Model;
using KafkaNet;
using KafkaNet.Protocol;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace RM_API_Kafka.WebMethod
{
    public static class KafkaService
    {
        public static List<ResourceWithValue> GetResource(long? id)
        {
            List<ResourceWithValue> resList = new List<ResourceWithValue>();
            try
            {
                string requestTopic = ServiceTopics.rmResourceBulk;
                Uri uri = new Uri("http://localhost:9092");
                var requestOptions = new KafkaOptions(uri);
                var requestRouter = new BrokerRouter(requestOptions);
                var consumer = new Consumer(new ConsumerOptions(requestTopic, requestRouter));
                foreach (var message in consumer.Consume())
                {
                    ResourceWithValue resource = JsonConvert.DeserializeObject<ResourceWithValue>(message.ToString());
                    resList.Add(resource);
                }

                if (id != null && resList != null && resList.Count > 0)
                    return resList.Where(x => x.Id == id).GroupBy(x => x.Id).Select(x => x.LastOrDefault()).ToList<ResourceWithValue>();
                else if (resList != null && resList.Count > 0)
                    return resList.GroupBy(x => x.Id).Select(x => x.LastOrDefault()).ToList<ResourceWithValue>();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error reading bulk to kafka server" + Environment.NewLine + ex.Message);
            }
            return resList;
        }

        public static void PostResource(ResourceWithValue resource)
        {
            try
            {
                JObject jsonObj = JObject.FromObject(resource);
                string payload = jsonObj.ToString();
                string topic = ServiceTopics.rmResourceBulk;
                Message msg = new Message(payload);
                Uri uri = new Uri("http://localhost:9092");
                var options = new KafkaOptions(uri);
                var router = new BrokerRouter(options);
                var client = new Producer(router);
                client.SendMessageAsync(topic, new List<Message> { msg }).Wait();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error saving bulk to kafka server" + Environment.NewLine + ex.Message);
            }
        }
    }
}