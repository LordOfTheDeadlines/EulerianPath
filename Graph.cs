using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SemesterTask
{
    public class Graph
    {

        private int vertices; // No. of vertices 
        private List<int>[] adj; // adjacency list 

        // Constructor 
        public Graph(int numOfVertices)
        {
            // initialise vertex count 
            this.vertices = numOfVertices;

            // initialise adjacency list 
            initGraph();
        }

        // utility method to initialise adjacency list 
        public void initGraph()
        {
            adj = new List<int>[vertices];
            for (int i = 0; i < vertices; i++)
            {
                adj[i] = new List<int>();
            }
        }

        // add edge u-v 
        public void addEdge(int u, int v)
        {
            adj[u].Add(v);
            adj[v].Add(u);
        }

        // This function removes edge u-v from graph. 
        public void removeEdge(int u, int v)
        {
            adj[u].Remove(v);
            adj[v].Remove(u);
        }

        /* The main function that print Eulerian Trail.  
           It first finds an odd degree vertex (if there  
           is any) and then calls printEulerUtil() to 
           print the path */
        public void printEulerTour()
        {
            // Find a vertex with odd degree 
            int u = 0;
            for (int i = 0; i < vertices; i++)
            {
                if (adj[i].Count % 2 == 1)
                {
                    u = i;
                    break;
                }
            }

            // Print tour starting from oddv 
            printEulerUtil(u);
            Console.WriteLine();
        }

        // Print Euler tour starting from vertex u 
        public void printEulerUtil(int u)
        {
            // Recur for all the vertices adjacent to this vertex 
            for (int i = 0; i < adj[u].Count; i++)
            {
                int v = adj[u][i];
                // If edge u-v is a valid next edge 
                if (isValidNextEdge(u, v))
                {
                    Console.Write(u + "-" + v + " ");

                    // This edge is used so remove it now 
                    removeEdge(u, v);
                    printEulerUtil(v);
                }
            }
        }

        // The function to check if edge u-v can be 
        // considered as next edge in Euler Tout 
        private bool isValidNextEdge(int u, int v)
        {
            // The edge u-v is valid in one of the 
            // following two cases: 

            // 1) If v is the only adjacent vertex of u  
            // ie size of adjacent vertex list is 1 
            if (adj[u].Count == 1)
            {
                return true;
            }

            // 2) If there are multiple adjacents, then 
            // u-v is not a bridge Do following steps  
            // to check if u-v is a bridge 
            // 2.a) count of vertices reachable from u 
            bool[] isVisited = new bool[this.vertices];
            int count1 = dfsCount(u, isVisited);

            // 2.b) Remove edge (u, v) and after removing 
            //  the edge, count vertices reachable from u 
            removeEdge(u, v);
            isVisited = new bool[this.vertices];
            int count2 = dfsCount(u, isVisited);

            // 2.c) Add the edge back to the graph 
            addEdge(u, v);
            return (count1 > count2) ? false : true;
        }

        // A DFS based function to count reachable 
        // vertices from v 
        private int dfsCount(int v, bool[] isVisited)
        {
            // Mark the current node as visited 
            isVisited[v] = true;
            int count = 1;
            // Recur for all vertices adjacent to this vertex 
            foreach (int adj in adj[v])
            {
                if (!isVisited[adj])
                {
                    count = count + dfsCount(adj, isVisited);
                }
            }
            return count;
        }
    }
}