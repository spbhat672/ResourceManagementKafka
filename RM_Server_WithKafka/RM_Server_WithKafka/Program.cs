using RM_Server_WithKafka.RM_Server.WebMethod;
using System;

namespace RM_Server_WithKafka
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                KafkaService.InvokeService().Wait();
            }            
            Console.ReadLine();
        }
    }
}
