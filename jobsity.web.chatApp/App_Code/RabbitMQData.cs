using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace jobsity.web.chatApp.App_Code
{
    public class RabbitMQServer
    {
        public string hostName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public int port { get; set; }
        public string virtualHost { get; set; }
        public string exchangeName { get; set; }
        public string queueName { get; set; }
    }

    public class RabbitMQData
    {
        private int port = 0;
        private string hostName = "";
        private string usr = "";
        private string pwd = "";
        private string virtualHost = "";

        private string exchangeName = "";
        private bool exchangeDurable = true;

        private string queueName = "";
        private bool queueDurable = true;
        private bool queueExclusive = false;
        private bool queueAutoDelete = false;

        private bool autoAck = true;
        private IConnection oCon;

        public RabbitMQData()
        {
            string RabbitMQServerConfigPath = ConfigurationManager.AppSettings["RabbitMQServerConfigPath"];
            string contenido = File.ReadAllText(RabbitMQServerConfigPath);
            RabbitMQServer rabbitMQServer = JsonConvert.DeserializeObject<RabbitMQServer>(contenido);
            hostName = rabbitMQServer.hostName;
            usr = rabbitMQServer.userName;
            pwd = rabbitMQServer.password;
            port = rabbitMQServer.port;
            virtualHost = rabbitMQServer.virtualHost;
            exchangeName = rabbitMQServer.exchangeName;
            queueName = rabbitMQServer.queueName;

            var factory = new ConnectionFactory() { 
                HostName = hostName,
                UserName = usr,
                Password = pwd,
                Port = port,
                VirtualHost = virtualHost
            };
            oCon = factory.CreateConnection();
        }

        #region Exchange Type Direct - exchangeName, queue

        public bool Send(string message)
        {
            using (var channel = oCon.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, exchangeDurable);

                channel.QueueDeclare(queueName, queueDurable, queueExclusive, queueAutoDelete, null);
                channel.QueueBind(queueName, exchangeName, queueName, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchangeName, queueName, null, msg);
            }
            return true;
        }

        public string Receive()
        {
            using (var channel = oCon.CreateModel())
            {
                channel.QueueDeclare(queueName, queueDurable, queueExclusive, queueAutoDelete, null);
                BasicGetResult result = channel.BasicGet(queueName, autoAck);
                if (result != null)
                {
                    byte[] msg = result.Body.ToArray();
                    string message = Encoding.UTF8.GetString(msg);
                    return message;
                }
            }
            return null;
        }

        #endregion
    }
}