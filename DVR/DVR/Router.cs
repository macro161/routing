using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVR
{
    class Router
    {
        public List<Router> networkRouters;
        public int[,] routingTable = new int[100, 100];
        public int id;
        public string data;

        public Router(List<Router> routers)
        {
            networkRouters = routers;
        }

        public void DisplayRoutingTable(int routersCount)
        {
            Console.WriteLine("My ID: " + id + "\n");

            for (int i = 0; i < routersCount; i++)
            {
                for (int j = 0; j < routersCount ; j++ )
                {
                    Console.Write(routingTable[i,j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public void SendMessage(Message message)
        {
            if (message.hops > 0)
            {
                message.hops--;
                if (message.reciever == id)
                {
                    Console.WriteLine("I got the message " + message.message);
                }
                else
                {

                }
            }
            else
            {
                Console.WriteLine("Too many hops, by by message");
            }
        }
    }
}
