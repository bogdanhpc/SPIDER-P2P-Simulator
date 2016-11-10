using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SPIDER
{
    class Nod : INod
    {
        public string Id { get; set; }
        public bool IsSuperPeer { get; set; }
        public int Chain { get; set; }
        public int Ring { get; set; }
        public string Content { get; set; }
        public Neighbour Status { get; set; }

        

        public Nod()
        {
            Id = string.Empty;
            IsSuperPeer = false;
            Chain = -1;
            Ring = -1;
            Status = new Neighbour();
            
            Content = CreateContnt();
        }
        public Nod(int chain, int ring, bool flag)
        {
            Chain = chain;
            Ring = ring;
            Id = chain.ToString() + "-" + ring.ToString() + " ";

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

        private string CreateContnt()
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

        //public bool IsSuperPeer()
        //{
        //    if (isSuperPeer == true)
        //        return true;
        //    else
        //        return false;
        //}

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

        public Neighbour GetStatus()
        {
            return Status;
        }
    }

}
