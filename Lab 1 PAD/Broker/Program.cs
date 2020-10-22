using System;
using System.Threading.Tasks;

namespace Broker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Broker");

            BSocket bSocket = new BSocket();
            bSocket.Start("127.0.0.1", 9999);

            var messageWorker = new MessageWorker();
            Task.Factory.StartNew(messageWorker.SendMessages, TaskCreationOptions.LongRunning);

            Console.ReadLine();
        }
    }
}
