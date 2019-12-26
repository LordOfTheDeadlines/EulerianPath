using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SemesterTask
{
    public class Graph
    {
        public static int first;
        public static int last;
        public static int Length;
        private List<int>[] adjacencyList; // список смежности
        private List<int>[] copy;
        public Graph(int length)
        {
            Length = length;
            MakeAdjList();
        }
        private void MakeAdjList()
        {
            adjacencyList = new List<int>[Length];
            for (int i = 0; i < Length; i++)
                adjacencyList[i] = new List<int>();
            copy = new List<int>[Length];
            for (int i = 0; i < Length; i++)
                copy[i] = new List<int>();
        }

        public void AddEdge(int startNode, int endNode)
        {
            adjacencyList[startNode].Add(endNode);
            adjacencyList[endNode].Add(startNode);
            copy[startNode].Add(endNode);
            copy[endNode].Add(startNode);
        }
        public void RemoveEdge(int startNode, int endNode)
        {
            adjacencyList[startNode].Remove(endNode);
            adjacencyList[endNode].Remove(startNode);
        }
        public bool FindNode(int node)
        {
            if (node > Length)
            {
                Console.WriteLine(":(");
                return false;
            }
            else
            {
                Console.WriteLine(adjacencyList[node].Count);
                return true;
            }
        }
        public void AddNode(int node)
        {
            adjacencyList[node].Add(node);
        }
        public void RemoveNode(int node)
        {
            adjacencyList[node].Remove(node);
        }


        private bool IsConnected()
        {
            // Помечаем все вершины как не посещенные
            bool[] visited = new bool[Length];
            int nodeNum;
            for (int i = 0; i < Length; i++) visited[i] = false;

            // Находим вершину с ненулевой степенью
            for (nodeNum = 0; nodeNum < Length; nodeNum++)
                if (adjacencyList[nodeNum].Count != 0)
                    break;
            // Если в графе нет ребер
            if (nodeNum == Length) return true;

            //  Запускаем обход в глубину из вершины с ненулевой степенью
            DeepSearchCount(nodeNum, visited);

            // Проверяем, все ли ненулевые вершины посещены
            for (int i = 0; i < Length; i++)
                if (visited[i] == false && adjacencyList[i].Count > 0)
                    return false;
            return true;
        }
        public void IsEulerian()
        {
            // Проверяем, все ли вершины ненулевой степени связаны
            if (IsConnected() == false)
            {
                Console.WriteLine("Граф не является Эйлеровым");
                return;
            }
            // Подсчет вершин с нечетной степенью
            int odd = 0;
            for (int i = 0; i < Length; i++)
                if (adjacencyList[i].Count % 2 != 0)
                    odd++;

            // если количество больше 2, то граф не эйлеров
            if (odd > 2)
            {
                Console.WriteLine("Граф не является Эйлеровым");
                return;
            }
            // если количество равно 2, то граф полу-эйлеров
            // если 0, то эйлеров 
            if(odd == 2) 
            {
                Console.WriteLine("Граф имеет Эйлеров путь");
                PrintEulerTour();
                return;
            }
            else
            {
                Console.WriteLine("Граф имеет Эйлеров цикл");
                PrintEulerTour();
                return;
            }        
        }
        static bool[] Visited = new bool[Length];
        public void PrintEulerTour()
        {
            // находим вершину нечетной степени
            int u = 0;
            for (int i = 0; i < Length; i++)
            {
                if (adjacencyList[i].Count % 2 == 1)
                {
                    u = i;
                    first = i;
                    break;
                }
            }
            FleryPrint(u);
            Console.WriteLine();
            if(first!=last)
            {
                Console.WriteLine("Возвращаться будем так:");
                bool[] Visited = new bool[Length];
                DeepSearch(last,last,first, Visited);
            }
        }

        // Печатаем путь, начиная с определенной вершины  
        private void FleryPrint(int u)
        {
            // Повторяем для вершин, смежных данной
            for (int i = 0; i < adjacencyList[u].Count; i++)
            {
                int v = adjacencyList[u][i];
                // если ребро может быть рассмотрено следующим
                if (IsValidNextEdge(u, v))
                {
                    Console.Write(u + "-" + v + " ");
                    last = v;
                    // мы прошли по ребру, следовательно больше его не используем
                    RemoveEdge(u, v);
                    FleryPrint(v);
                }
            }
        }

        private bool IsValidNextEdge(int u, int v)
        {
            if (adjacencyList[u].Count == 1) return true;

            bool[] isVisited = new bool[Length];
            int count1 = DeepSearchCount(u, isVisited);
            RemoveEdge(u, v);
            isVisited = new bool[Length];
            int count2 = DeepSearchCount(u, isVisited);
            AddEdge(u, v);
            return (count1 > count2) ? false : true;
        }

        public int DeepSearchCount(int node, bool[] isVisited)
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
        public void DeepSearch(int node,int start,int final, bool[] isVisited)
        {
            // помечаем текущую вершину как посещенную 
            isVisited[node] = true;
            Console.WriteLine(node);
            foreach (var adj in copy[node])// повторяем для всех инцидентных вершин 
            {
                if (!isVisited[adj]&&start!=final)
                    DeepSearch(adj, start, final, isVisited);

            }
        }
    }
}