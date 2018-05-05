using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVR
{
    class Router
    {
        RoutingTable routingTable = new RoutingTable();
        public int id;
        public string data;

        public void DisplayRoutingTable()
        {
            for (int i = 0; i < routingTable.table.GetLength(0); i++)
            {
                for (int j = 0; j < routingTable.table.GetLength(1) ; j++ )
                {
                    Console.WriteLine(routingTable.table[i][j] + "    ");
                }
                Console.WriteLine();
            }
        }

        public void InitRoutingTable()
        {

        }
    }
}
