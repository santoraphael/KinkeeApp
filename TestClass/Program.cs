using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClass
{
    public class Program
    {
        public IConnection ConnectionMQ()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = "amqp://nzbmusui:yKdWsFngE1UNmS4Dg1fWSAd6XBHqL3-_@prawn.rmq.cloudamqp.com/nzbmusui";

            IConnection conn = factory.CreateConnection();

            return conn;
        }
    }

    
}
