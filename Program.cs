using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;

namespace RecoverDeadLetterQueuesGeneric
{
    class Program
    {
        static void Main(string[] args)
        {
            var cnnStr = ConfigurationManager.ConnectionStrings["servicebus"].ConnectionString;
            var queues = ConfigurationManager.AppSettings["queues"].Split('|');
            foreach (var queueName in queues)
            {
                Console.SetCursorPosition(1, 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(1, 1);
                Console.WriteLine($"queue name: {queueName}");
                var queue = QueueClient.CreateFromConnectionString(cnnStr, queueName);
                var factory = MessagingFactory.CreateFromConnectionString(cnnStr);
                var deadLetterPath = QueueClient.FormatDeadLetterPath(queueName);
                var dlqReceiver = factory.CreateMessageReceiver(deadLetterPath, ReceiveMode.PeekLock);

                while (true)
                {
                    var msg = dlqReceiver.Receive();
                    if (msg == null)
                        break;
                    Console.SetCursorPosition(1, 3);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(1, 3);
                    Console.WriteLine($"msg content: {msg.MessageId}");
                    try
                    {
                        // Send message to queue
                        var bm = msg.Clone();
                        queue.Send(bm);
                        //
                        msg.Complete();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Console.SetCursorPosition(1, 5);
            Console.WriteLine("Press any key to exit!");
            Console.Read();
        }

        protected static BrokeredMessage CreateBrokeredMessage(string id, object message)
        {
            var bm = string.IsNullOrWhiteSpace(id) ? new BrokeredMessage(message) : new BrokeredMessage(message) { MessageId = id };
            if (bm.Size > 250 * 1024)
                throw new ArgumentOutOfRangeException($"MessageId {bm.MessageId} is too large. Size in bytes equals to {bm.Size}");
            return bm;
        }
    }
}
