using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SPIDER;
using System.Security.Cryptography;
using System.IO;

namespace ServerMultipleClientsDemo
{
    class Client: INod
    {
        private Socket ClientSocket;
        private byte[] receivedMessage = new byte[8096];

        public string Id { get; set; }
        public bool IsSuperPeer { get; set; }
        public int Chain { get; set; }
        public int Ring { get; set; }
        public Neighbour Status { get; set; }
        public object Content { get; set; }

        public Client()
        {
            Id = string.Empty;
            IsSuperPeer = false;
            Chain = -1;
            Ring = -1;
            Status = new Neighbour();

            Content = CreateContnt();
        }

        private object CreateContnt()
        {
            byte[] data = new byte[4];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    // Fill buffer.
                    rng.GetBytes(data);

                }
            }
            SHA1Managed hashstring = new SHA1Managed();
            byte[] hash = hashstring.ComputeHash(data);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        public Client(int chain, int ring, bool flag)
        {
            Chain = chain;
            Ring = ring;
            Id = chain.ToString() + "-" + ring.ToString();

            Content = CreateContnt();

            if (flag == true)
            {
                IsSuperPeer = true;
            }
            else
            {
                IsSuperPeer = false;
            }

            Status = new Neighbour();
        }
        public void Connect()
        {
            ClientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            ClientSocket.Connect(IPAddress.Parse("127.0.0.1"), 12578);
            ClientSocket.BeginReceive(receivedMessage, 0, receivedMessage.Length, SocketFlags.None, ReceiveDone, null);
        }

        private void ReceiveDone(IAsyncResult ar)
        {
            var result = ClientSocket.EndReceive(ar);
            ClientSocket.BeginReceive(receivedMessage, 0, receivedMessage.Length, SocketFlags.None, ReceiveDone, null);
        }

        public void Send(string message)
        {
            ClientSocket.Send(Encoding.ASCII.GetBytes(message));
            FileLogger.Instance.CreateEntry(message);
        }

        public Neighbour GetStatus()
        {
            return Status;
        }

        public void PrintNod()
        {
            using (StreamWriter writetext = new StreamWriter("Output.txt", true))
            {
                if (IsSuperPeer)
                {
                    writetext.WriteLine("Node ID: " + Id + "SUPER PEER" + "\r\n****************\r\n");
                }
                else
                {
                    writetext.WriteLine("Node ID: " + Id + "\r\nChain :" + Chain + "\r\nRing :" + Ring
           + "\r\nContent :" + Content + "\r\n****************\r\n");
                }
            }
        }
    }
}
