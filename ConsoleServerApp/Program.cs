﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleServerApp
{
    /// <summary>
    /// Program installed on PC server.
    /// </summary>
    internal class Program
    {
        const string DEST_IP_ADDRESS = "PC2_IP_address";
        const int DEST_PORT = 13;


        static void Main(string[] args)
        {
            while (true)
            {
                string data = GetIncomingData();

                SendReceivedData(data);

                Console.ReadLine();
            }
           
        }



        private static string GetIncomingData()
        {
            IPAddress localAdd = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(localAdd, 13);
            Console.WriteLine("Listening...");
            listener.Start();

            // incoming client connected
            TcpClient client = listener.AcceptTcpClient();

            // get the incoming data
            NetworkStream nwStream = client.GetStream();
            byte[] buffer = new byte[client.ReceiveBufferSize];

            // read incoming stream
            int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received : " + dataReceived);

            nwStream.Close();
            client.Close();

            listener.Stop();

            return dataReceived;
        }



        private static void SendReceivedData(string data)
        {
            TcpClient client = null;
            NetworkStream stream = null;

            try
            {
                Console.WriteLine($"Sending data to server to PC2");

                client = new TcpClient();
                client.Connect(DEST_IP_ADDRESS, DEST_PORT);
                stream = client.GetStream();

                byte[] buffer = Encoding.ASCII.GetBytes(data);

                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem to send data to server : {0}", ex.Message);
            }
            finally
            {
                stream?.Close();
                client?.Close();
            }
        }
    }
}
