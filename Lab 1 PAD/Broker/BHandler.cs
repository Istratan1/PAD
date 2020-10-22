using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Broker
{
    class BHandler
    {
        public static void Handle(byte[] payloadbytes, ConnInfo connInfo)
        {
            var payloadString = Encoding.UTF8.GetString(payloadbytes);

            if (payloadString.StartsWith("#"))
            {
                connInfo.newsCategory = payloadString.Split("#").LastOrDefault();
                Connections.Add(connInfo);
            }
            else
            {
                PHandler pHandler = JsonConvert.DeserializeObject<PHandler>(payloadString);
                PStorage.Add(pHandler);
            }

            Console.WriteLine(payloadString);
        }
    }
}
