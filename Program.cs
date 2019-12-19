using System;

namespace SemesterTask
{
    class Program
    {
        static void Main(string[] args)
        {
            //var graph = MyGraph.MakeGraph(
            //0, 1,
            //0, 2,
            //1, 3,
            //1, 4,
            //2, 3,
            //3, 4);

            Graph g1 = new Graph(10);
            g1.AddEdge(0, 1);
            g1.AddEdge(0, 2);
            g1.AddEdge(1, 2);
            g1.AddEdge(2, 3);
            g1.AddEdge(3, 4);
            g1.AddEdge(4, 2);
            g1.AddNode(5);
            g1.FindNode(8);
            //if(g1.IsEulerian())
            //g1.FindEulerTour();
            g1.Test();



        }
    }
}

