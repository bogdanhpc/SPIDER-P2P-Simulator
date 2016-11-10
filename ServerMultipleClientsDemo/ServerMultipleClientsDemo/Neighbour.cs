using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPIDER
{
    class Neighbour
    {
        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public Neighbour(bool up, bool down, bool left, bool right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public Neighbour()
        {
        }
    }
}
