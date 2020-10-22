using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Broker
{
    class MessageWorker
    {

        public void SendMessages()
        {
            while (true)
            {
                while (!PStorage.isEmpty())
                {
                    var message = PStorage.getNext();

                    if(message != null)
                    {
                        var connections = Connections.GetConnInfosByCategory(message.newsCategory);
                        
                        foreach(var connection in connections)
                        {
                            var messageString = JsonConvert.SerializeObject(message);
                            byte[] data = Encoding.UTF8.GetBytes(messageString);

                            connection.Socket.Send(data);
                            Thread.Sleep(500);
                        }
                    }
                }

                Thread.Sleep(3000);
            }
        }
    }
}
