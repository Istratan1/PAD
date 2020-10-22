using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Broker
{
    class BSocket
    {
        private Socket _socket;

        public BSocket()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Start(string ip, int port)
        {
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            //Limita la conexiuni
            _socket.Listen(5);
            Accept();
        }

        private void Accept()
        {
            _socket.BeginAccept(AcceptedCallback, null);
        }

        private void AcceptedCallback(IAsyncResult asyncResult)
        {
            ConnInfo conn = new ConnInfo();

            try
            {
                conn.Socket = _socket.EndAccept(asyncResult);
                conn.Address = conn.Socket.RemoteEndPoint.ToString();
                conn.Socket.BeginReceive(conn.Data, 0, conn.Data.Length, 
                    SocketFlags.None, ReceiveCallback, conn);
            } catch(Exception e)
            {
                Console.WriteLine($"Can't accept: {e.Message}");
            }
            finally
            {
                Accept();
            }
        }
        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            ConnInfo conn = asyncResult.AsyncState as ConnInfo;

            try
            {
                Socket senderSocket = conn.Socket;
                SocketError response;
                int buff_size = senderSocket.EndReceive(asyncResult, out response);
                if(response == SocketError.Success)
                {
                    byte[] payload = new byte[buff_size];
                    Array.Copy(conn.Data, payload, payload.Length);

                    BHandler.Handle(payload, conn);

                }

            } catch(Exception e)
            {
                Console.WriteLine($"Can't receive data:{e.Message}");
            }
            finally
            {
                try
                {
                    conn.Socket.BeginReceive(conn.Data, 0, conn.Data.Length,
                   SocketFlags.None, ReceiveCallback, conn);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    var address = conn.Socket.RemoteEndPoint.ToString();

                    Connections.Remove(address);
                    conn.Socket.Close();
                    
                }
               
            }


        }
    }
}
