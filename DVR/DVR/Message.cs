using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVR
{
    class Message
    {
        public int id = new Random().Next(1000000) + 1;
        public int reciever;
        public string message;
        public int hops = 10;
    }
}
