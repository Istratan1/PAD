using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{

    class PublisherSocket
    {
        private Socket socket;
        public bool isConnected;
        public PublisherSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        //Conexiunea la broker

        public void Connect(string ip, int port)
        {
            //BeginConnect - varianta asincrona, in background
            socket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectedCallback, null);
            Thread.Sleep(500);
        }

        public void Send(byte[] data)
        {
            try
            {
                socket.Send(data);
            } catch(Exception e)
            {
                Console.WriteLine($"Could not sent data: {e.Message}");
            }
        }


        private void ConnectedCallback(IAsyncResult asyncResult)
        {
            //verificam daca conexiunea s-a efectuat sau nu
            if (socket.Connected)
            {
                Console.WriteLine("Sender connected to broker!");
            }
            else
            {
                Console.WriteLine("Error: Sender couldn't connect to broker!");
            }

            isConnected = socket.Connected;
        }

    }
}
