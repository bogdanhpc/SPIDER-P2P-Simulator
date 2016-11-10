using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SPIDER;

namespace ServerMultipleClientsDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            FileLogger.Instance.Open(@"SPIDER.log", false);
            //Console.WriteLine("Simulator Started ...");

            var clientList = new List<ConnectedClient>(1000);

            var serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),12578));
            serverSocket.Listen(1000);

            SpiderOverlay spiderOverlay = new SpiderOverlay(10, 100);
            spiderOverlay.CreateOverlay();

            
            
            

            Task.Run(() =>
            {
                Task.Delay(TimeSpan.FromSeconds(5));

                for (int i = 0; i < spiderOverlay.getMaximumNumberOfPeers(); i++)
                {
                    if (spiderOverlay.Peers.Count != 0)
                    {
                        //var client = new Client();
                        var existingNode = spiderOverlay.GetLastNode();
                        spiderOverlay.JoinOverlay(existingNode);
                        //client.Connect();
                    }
                }
            });

            while (true)
            {
                var myClientSocket = serverSocket.Accept();
                var newClient = new ConnectedClient(myClientSocket);
                clientList.Add(newClient);
                //Console.WriteLine("Client Connected");
                FileLogger.Instance.CreateEntry("Thread STARTTED");
                newClient.startReceice();
            }

            FileLogger.Instance.Close();

        }
    }
}
