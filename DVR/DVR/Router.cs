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
            Console.WriteLine("Im alaive");
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
            Console.WriteLine("I am here, my ID: " + id);

            if (message.hops > 0)
            {
                message.hops--;
                if (message.reciever == id)
                {
                    Console.WriteLine("I got the message " + message.message);
                }
                else
                {
                    Console.WriteLine("1");

                    int index = GetRouterIndex(message.reciever);

                    if (routingTable[index, 1] == 0)
                    {
                        SendToRandom(message);
                        Console.WriteLine("2");
                    }
                    else
                    {
                        Console.WriteLine("3");
                        if (routingTable[index, 1] != 0 && routingTable[index, 2] == 0)
                        {
                            Console.WriteLine("4");
                            networkRouters[networkRouters.FindIndex(id => id.id == routingTable[index, 0])].SendMessage(message);
                        }
                        else
                        {
                            Console.WriteLine("5");
                            networkRouters[networkRouters.FindIndex(id => id.id == routingTable[index,2])].SendMessage(message);
                        }
                       
                    }
                }
            }

            else
            {
                Console.WriteLine("6");
                Console.WriteLine("Too many hops, by by message");
            }
        }

        public void SendToRandom(Message message)
        {

        }

        private int GetRouterIndex(int routerId)
        {
            for (int i  =  0; i < 100; i++)
            {
                if (routingTable[i,0] == routerId)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}
