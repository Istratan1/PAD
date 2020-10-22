using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Broker
{
    class ConnInfo
    {
        public const int BUFF_SIZE = 1024;
        public byte[] Data { get; set; }

        public Socket Socket { get; set; } 
        public string Address { get; set; } 
        public string newsCategory { get; set; } 

        public ConnInfo()
        {
            Data = new byte[BUFF_SIZE];
        }

    }
}
