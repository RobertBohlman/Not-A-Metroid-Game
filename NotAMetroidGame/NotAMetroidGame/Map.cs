using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotAMetroidGame
{
    public class Map
    {
        public float left;
        public float right;
        public float top;
        public float bottom;

        public Map()
        {
            left = 0;
            right = 1000;
            top = 0;
            bottom = 600;
        }
    }
}
