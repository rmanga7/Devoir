using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                string data = GetIncomingData();

                SendDataToClipboard(data);

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

        private static void SendDataToClipboard(string data)
        {
            Console.WriteLine("Sending data to Clipboard...");

            Clipboard.SetText(data);
        }
    }
}
