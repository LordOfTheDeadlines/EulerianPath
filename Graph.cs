using System;
using System.Collections.Generic;

namespace SemesterTask
{
    public class Graph
    {
        private int Length;
        private int[] nodes;
        private List<int>[] adjacencyList; // список смежности
        public Graph(int length)
        {
            Length = length;
            MakeAdjList();
            MakeArrNodes();
        }

        // utility method to initialise adjacency list 
        private void MakeAdjList()
        {
            adjacencyList = new List<int>[Length];
            for (int i = 0; i < Length; i++)
                adjacencyList[i] = new List<int>();
        }
        private void MakeArrNodes()
        {
            nodes = new int[Length];
        }
        public void AddEdge(int startNode, int endNode)
        {
            adjacencyList[startNode].Add(endNode);
            adjacencyList[endNode].Add(startNode);
            nodes[startNode] = startNode;
            nodes[endNode] = endNode;
        }
        public void RemoveEdge(int startNode, int endNode)
        {
            adjacencyList[startNode].Remove(endNode);
            adjacencyList[endNode].Remove(startNode);
            nodes[startNode] = -1;
            nodes[endNode] = -1;
        }
        public void FindNode(int node)
        {
            if (node > Length) Console.WriteLine(":(");
            else Console.WriteLine(adjacencyList[node].Count);
        }
        public void AddNode(int node)
        {
            adjacencyList[node].Add(node);

        }
        public void RemoveNode(int node)
        {
            adjacencyList[node].Remove(node);
        }

        public  bool isConnected()
        {
            // Mark all the vertices as not visited 
            bool[] visited = new bool[Length];
            int i;
            for (i = 0; i < Length; i++)
                visited[i] = false;

            // Find a vertex with non-zero degree 
            for (i = 0; i < Length; i++)
                if (adjacencyList[i].Count != 0)
                    break;

            // If there are no edges in the graph, return true 
            if (i == Length)
                return true;

            // Start DFS traversal from a vertex with non-zero degree 
            DeepSearchCount(i, visited);

            // Check if all non-zero degree vertices are visited 
            for (i = 0; i < Length; i++)
                if (visited[i] == false && adjacencyList[i].Count > 0)
                    return false;

            return true;
        }
        public int isEulerian()
        {
            // Check if all non-zero degree vertices are connected 
            if (isConnected() == false)
                return 0;

            // Count vertices with odd degree 
            int odd = 0;
            for (int i = 0; i < Length; i++)
                if (adjacencyList[i].Count % 2 != 0)
                    odd++;

            // If count is more than 2, then graph is not Eulerian 
            if (odd > 2)
                return 0;

            // If odd count is 2, then semi-eulerian. 
            // If odd count is 0, then eulerian 
            // Note that odd count can never be 1 for undirected graph 
            return (odd == 2) ? 1 : 2;
        }
        public void Test()
        {
            int res = isEulerian();
            if (res == 0)
                Console.WriteLine("graph is not Eulerian");
            else if (res == 1)
            {
                Console.WriteLine("graph has a Euler path");
                FindEulerTour();
            }
            else
            {
                Console.WriteLine("graph has a Euler cycle");
                FindEulerTour();
            }
        }
        public void FindEulerTour()
        {
            // находим вершину нечетной степени
            int u = 0;
            for (int i = 0; i < Length; i++)
            {
                if (adjacencyList[i].Count % 2 == 1)
                {
                    u = i;
                    break;
                }
            }
            // Print tour starting from oddv 
            printEulerUtil(u);
            Console.WriteLine();
        }

        // Печатаем путь, начиная с определенной вершины  
        public void printEulerUtil(int u)
        {
            // Повторяем для вершин, смежных данной
            for (int i = 0; i < adjacencyList[u].Count; i++)
            {
                int v = adjacencyList[u][i];
                // If edge u-v is a valid next edge 
                if (isValidNextEdge(u, v))
                {
                    Console.Write(u + "-" + v + " ");

                    // мы прошли по ребру, следовательно больше его не используем
                    RemoveEdge(u, v);
                    printEulerUtil(v);
                }
            }
        }

        // проверям, может ли ребро u-v быть
        // рассмотрено следующим Euler Tout 
        private bool isValidNextEdge(int u, int v)
        {
            // The edge u-v is valid in one of the 
            // following two cases: 

            // 1) 1) Если v единственная смежная вершина из u
            // т.е. размер списка смежных вершин равен 1
            if (adjacencyList[u].Count == 1) return true;

            // 2) Если есть несколько смежных, то проверяем, что
            // u-v is not a bridge Do following steps  
            // to check if u-v is a bridge 
            // 2.a) count of vertices reachable from u 
            bool[] isVisited = new bool[Length];
            int count1 = DeepSearchCount(u, isVisited);

            // 2.b) Удаляем ребро (u, v), затем считаем вершины достижимые до u 
            RemoveEdge(u, v);
            isVisited = new bool[Length];
            int count2 = DeepSearchCount(u, isVisited);

            // 2.c)Добавляем ребро обратно в граф
            AddEdge(u, v);
            return (count1 > count2) ? false : true;
        }

        // ищем вершины, достижимые от данной
        private int DeepSearchCount(int node, bool[] isVisited)
        {
            // помечаем текущую вершину как посещенную 
            isVisited[node] = true;
            int count = 1;           
            foreach (int adj in adjacencyList[node])// повторяем для всех инцидентных вершин 
            {
                if (!isVisited[adj])
                    count = count + DeepSearchCount(adj, isVisited);
            }
            return count;
        }
    }
}