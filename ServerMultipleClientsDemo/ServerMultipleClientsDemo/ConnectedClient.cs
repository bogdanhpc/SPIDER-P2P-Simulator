using System;
using System.Net.Sockets;
using System.Text;
using SPIDER;
using System.Security.Cryptography;
using System.IO;

namespace ServerMultipleClientsDemo
{
    internal class ConnectedClient    : INod
    {
        private Socket myClientSocket;
        private byte[] receivedMessage = new byte[8096];

        public string Id { get; private set; }
        public bool IsSuperPeer { get; private set; }
        public int Chain { get; private set; }
        public int Ring { get; private set; }
        public Neighbour Status { get; private set; }
        public object Content { get; private set; }

        public ConnectedClient(Socket myClientSocket)
        {
            this.myClientSocket = myClientSocket;
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

        internal void startReceice()
        {
            myClientSocket.BeginReceive(receivedMessage, 0, receivedMessage.Length, SocketFlags.None, ReceiveDone, null);
        }

        private void ReceiveDone(IAsyncResult ar)
        {
            var result = myClientSocket.EndReceive(ar);
            //Console.WriteLine(Encoding.ASCII.GetString(receivedMessage));
            myClientSocket.BeginReceive(receivedMessage, 0, receivedMessage.Length, SocketFlags.None, ReceiveDone, null);
        }

        public void Send(string message)
        {
            myClientSocket.Send(Encoding.ASCII.GetBytes(message));
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