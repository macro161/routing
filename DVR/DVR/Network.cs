using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVR
{
    class Network
    {
        int[,] edges = new int[100, 100];
        int[,] via = new int[100, 100];
        int[,,] sharingTable = new int[100,100,100];
        int[,] finalTable = new int[100,100];
        Random rnd = new Random();
        private bool run = true;
        private int routersNumber = 0;
        private List<Router> networkRouters = new List<Router>();

        public void Run() {
            while (run) {
                Console.WriteLine("1. Initiate network");
                Console.WriteLine("2. Remove router");
                Console.WriteLine("3. Send message");
                Console.WriteLine("4. Print routers details");
                Console.WriteLine("5. Add edge between routers");
                Console.WriteLine("6. Add router");
                Console.WriteLine("7. Display Via and Distance tables");
                Console.WriteLine("8. Sharing table");
                Console.WriteLine("9. Compute and display last table");
                Console.WriteLine("10. Display routers routing tables");

                switch (Console.ReadLine())
                {
                    case "1":
                        InitNetwork();
                        break;
                    case "2":
                        RemoveRouter();
                       
                        break;
                    case "3":
                        SendMessage();
                        break;
                    case "4":
                        RoutersDetails();
                        break;
                    case "5":
                        Console.WriteLine();
                        DisplayNetworkRouters();
                        Console.WriteLine();
                        DisplayEdges();
                        Console.WriteLine();
                        AddEdge();
                        break;
                    case "6":
                        routersNumber++;
                        AddRouter();
                        break;
                    case "7":
                        DisplayDistVia();
                        break;
                    case "8":
                        InitSharingTable();
                        DisplaySharingTable();
                        break;
                    case "9":
                        ComputeFinalTable();
                        DisplayFinalTable();
                        break;
                    case "10":
                        DisplayRouterRoutingTables();
                        break;
                    default:
                        Console.WriteLine("Bad input");
                        break;    
                }
            }
        }

        private void RoutersDetails()
        {
            for (int i = 0; i< routersNumber; i++ )
            {
                Console.WriteLine("//////////");

                for (int j = 0; j < networkRouters[i].routingTable.GetLength(0) ; j++ )
                {
                    Console.WriteLine("\t" + networkRouters[i].routingTable[j,0] + "\t" + networkRouters[i].routingTable[j, 1] + "\t" + networkRouters[i].routingTable[j, 2]);
                }
            }
        }

        private void SendMessage()
        {
            int sender;
            int reciever;
            string message = "TEST";

            Console.WriteLine("Enter sender's ID: ");
            sender = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter reciever's ID: ");
            reciever = Int32.Parse(Console.ReadLine());

            Message msg = new Message(reciever,message);

            networkRouters[networkRouters.FindIndex(id => id.id == sender)].SendMessage(msg);

        }

        private void DisplayRouterRoutingTables()
        {
            for (int i = 0; i < routersNumber; i++ )
            {
                networkRouters[i].DisplayRoutingTable(routersNumber);
                Console.WriteLine();
            }
        }

        private void DisplayFinalTable()
        {
            Console.WriteLine("\n After Update :");
            /* Display table Updation */
            for (int i = 0; i < routersNumber; i++)
            {
                Console.WriteLine("\n" + networkRouters[i].id.ToString() + " Table");
                Console.WriteLine("\nNode\tDist\tVia");
                for (int j = 0; j < routersNumber; j++)
                {
                    Console.Write("\n" + networkRouters[j].id.ToString() +   "\t" + finalTable[i,j] + "\t");
                    networkRouters[i].routingTable[j, 0] = networkRouters[j].id;
                    networkRouters[i].routingTable[j, 1] = finalTable[i, j];

                    if (i == via[i, j])
                    {
                        Console.Write("-");
                        networkRouters[i].routingTable[j, 2] = 0;
                    }
                    else
                    {
                        Console.Write(networkRouters[via[i, j]].id.ToString());
                        networkRouters[i].routingTable[j, 2] = networkRouters[via[i, j]].id;
                    }
                        
                }
            }

        }

        private void ComputeFinalTable()
        {
            for (int i = 0; i < routersNumber; i++)
            {
                for (int j = 0; j < routersNumber; j++)
                {
                    finalTable[i, j] = edges[i + 1, j + 1];
                    via[i, j] = i;

                    for (int k = 0; k < routersNumber; k++)
                    {
                        if ((finalTable[i, j] > sharingTable[i, k, j]) || (finalTable[i, j] == 0))
                        {
                            if (sharingTable[i, k, j] > 0)
                            {
                                finalTable[i, j] = sharingTable[i, k, j];
                                via[i, j] = k;
                            }
                        }
                    }

                    if (finalTable[i,j] == 0)
                    {
                        for (int k = 0; k < routersNumber; k++)
                        {

                            if ((finalTable[i,k] != 0) && (finalTable[k,j] != 0))
                            {
                                if ((finalTable[i,j] == 0) || ((finalTable[i,j] != 0) && (finalTable[i,j] > (finalTable[i,k] + finalTable[k,j]))))
                                {
                                    if (finalTable[i,k] + finalTable[k,j] > 0)
                                    {
                                        finalTable[i,j] = finalTable[i,k] + finalTable[k,j];
                                        via[i,j] = k;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void InitSharingTable()
        {
            for (int i = 0; i < routersNumber ; i++)
            {
                for (int j = 0; j < routersNumber ; j++ )
                {
                    for (int k = 0; k < routersNumber ; k++ )
                    {
                        if ((edges[i+1, j+1] > 0) && (edges[j+1,k+1] > 0))
                        {
                            sharingTable[i, j, k] = edges[j+1, k+1] + edges[i+1, j+1];
                        }
                        else
                        {
                            sharingTable[i, j, k] = 0;
                        }
                    }
                }
            }
        }

        private void DisplaySharingTable()
        {
            for (int i = 0; i< routersNumber ; i++ )
            {
                Console.WriteLine("\n\n For" + networkRouters[i].id.ToString());
                for (int j = 0;j < routersNumber ; j++ )
                {
                    Console.WriteLine("\n From" + networkRouters[j].id.ToString());
                    for (int k = 0; k < routersNumber ; k++ )
                    {
                        Console.WriteLine(networkRouters[k].id.ToString() + " "+ sharingTable[i,j,k]);
                    }
                }
            }
        }

        private void InitNetwork() {
            Console.WriteLine("How many routers do you want in network: ");
            routersNumber = Int32.Parse(Console.ReadLine());
            CreateRouters();
            InitialeEdgeTabel();
            InitVia();
        }

        private void InitVia()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    via[i, j] = 0;
                }
            }
        }

        private void CreateRouters() {
            for (int i = 0; i < routersNumber; i++) {
                Router newRouter = new Router(networkRouters);
                newRouter.id = rnd.Next(90) + 10;
                networkRouters.Add(newRouter);
            }
        }

        private void InitiateRandomConnections() //Problems with poor randomness, separate connected routers subsets, probably not going to use it
        {
            for (int i = 0; i < routersNumber/2  ; i++ )
            {
                int firstRandomRouter = rnd.Next(routersNumber - 1) + 1;
                int secondRandomRouter = rnd.Next(routersNumber - 1) + 1;
                int randomWeight = rnd.Next(90) + 1;

                edges[firstRandomRouter, secondRandomRouter] = randomWeight;
                edges[secondRandomRouter, firstRandomRouter] = randomWeight;
            }

            DisplayEdges();
        }

        private void DisplayNetworkRouters()
        {
            foreach (var router in networkRouters)
            {
                Console.WriteLine("Router's data: " + router.data);
                Console.WriteLine("Router's ID: " + router.id);
            }
        }

        private void AddRouter()
        {
            Router newRouter = new Router(networkRouters);
            newRouter.id = rnd.Next(90) + 10;
            networkRouters.Add(newRouter);
            UpdateEdgeTable();
            InitVia();
        }

        private void RemoveRouter()
        {
            DisplayNetworkRouters();
            Console.WriteLine();
            DisplayEdges();
            Console.WriteLine();
            Console.WriteLine("Which router you want to remove: ");
            Console.WriteLine();

            int routerToBeRemovedId = Int32.Parse(Console.ReadLine());

            for (int i = 1; i <= routersNumber; i++)
            {
                edges[CoordinatesOfElement(routerToBeRemovedId).Item2, i] = 0;
                edges[i, CoordinatesOfElement(routerToBeRemovedId).Item2] = 0;
            }
            UpdateEdgeTable();
            InitVia();

            DisplayEdges();



        }

        private void UpdateEdgeTable()
        {
            edges[0, routersNumber] = networkRouters[routersNumber - 1].id;
            edges[routersNumber, 0] = networkRouters[routersNumber - 1].id;
        }

        private void InitialeEdgeTabel()
        {
            edges[0,0] = 0;

            for (int i = 1; i <= routersNumber; i++)
            {
                edges[0,i] = networkRouters[i - 1].id;
                edges[i,0] = networkRouters[i - 1].id;
            }

            for (int i = 1; i <= routersNumber; i++ )
            {
                for (int j = 1; j <= routersNumber; j++)
                {
                    edges[i,j] = 0;
                }
                
            }
        }

        private void DisplayEdges()
        {
            for (int i = 0; i < routersNumber + 1; i++)
            {
                for (int j = 0; j < routersNumber + 1; j++)
                {
                    Console.Write("\t"+edges[i,j]);
                }
                Console.WriteLine();
            }
        }

        private void DisplayDistVia()
        {
            for (int i = 1; i <= routersNumber; i++)
            {
                Console.WriteLine("Router " + edges[0,i].ToString() + " Table" );
                Console.WriteLine("\nRouter\tDist\tVia");
                for (int j = 1; j <= routersNumber ; j++ )
                {
                    Console.WriteLine(edges[0,j].ToString()+ "\t" + edges[i,j] + "\t" + via[i,j]);
                }
            }
        }

        private void AddEdge()
        {
            int firstRouter;
            int secondRouter;

            Console.WriteLine("Which two routers you want to join?");
            Console.WriteLine("First router ID: ");
            firstRouter = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Second router ID: ");
            secondRouter = Int32.Parse(Console.ReadLine());

            int randomWeight = rnd.Next(20) + 1;

            edges[CoordinatesOfElement(firstRouter).Item2, CoordinatesOfElement(secondRouter).Item2] = randomWeight;
            edges[CoordinatesOfElement(secondRouter).Item2, CoordinatesOfElement(firstRouter).Item2] = randomWeight;


            DisplayEdges();

        }

        public Tuple<int, int> CoordinatesOfElement(int firstRouter) //Code from StackOverflow
        {
            int w = edges.GetLength(0); // width
            int h = edges.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (edges[x, y].Equals(firstRouter))
                        return Tuple.Create(x, y);
                }
            }

            return Tuple.Create(-1, -1);
        }
    }
}
