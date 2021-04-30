using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kafka.Client.Producers;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using RM_WithKafka.Utils;

namespace RM_WithKafka.WebMethod
{
    public static class KafkaService
    {
        public static void ProduceRequest(string subTopicAsPayload,string requestString)
        {
            string topic = ServiceTopics.RequestTopic;
            string payload = topic + subTopicAsPayload + requestString;
            Message msg = new Message(payload);
            Uri uri = new Uri("http://localhost:9092");
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
            var client = new KafkaNet.Producer(router);
            client.SendMessageAsync(topic, new List<Message> { msg }).Wait();
        }

        public static string ConsumeResponse(string subTopic)
        {
            string mainTopic = ServiceTopics.ResponseTopic;
            string topic = mainTopic + subTopic;
            Uri uri = new Uri("http://localhost:9092");
            var options = new KafkaOptions(uri);
            var router = new BrokerRouter(options);
            var consumer = new Consumer(new ConsumerOptions(topic, router));
            return consumer.Consume().Last<Message>().Value.ToString();
        }        
    }
}