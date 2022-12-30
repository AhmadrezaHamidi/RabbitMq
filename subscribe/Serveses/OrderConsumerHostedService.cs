using System;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace subscribe.Serveses
{

    public class OrderConsumerHostedService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var orderQueueName = "order-queue";
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            var connection = factory.CreateConnection();
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: orderQueueName,
                                   durable: false,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    IDictionary<string, object> headers = ea.BasicProperties.Headers; // get headers from Received msg

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    //call another service
                };

                channel.BasicConsume(queue: orderQueueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Waiting for feedback. Press [enter] to exit.");
                Console.ReadLine();

            }
        }
    }



}

