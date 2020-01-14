using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SemesterTask
{
    public class Node
    {
        public int value;
        public List<Node> ajNodes;
        public bool IsVisited;
        public List<Node> copyNodes;
        private void MakeAdjNodes()
        {
            ajNodes = new List<Node>();
            copyNodes = new List<Node>();
        }
        public Node(int value)
        {
            this.value = value;
            this.IsVisited = false;
            MakeAdjNodes();
        }
    }
    public class Graph
    {
        public static Node first;
        public static Node last;
        private List<Node> nodes;
        private List<Node> copy;
        public Graph()
        {
            MakeNodeList();
        }
        private void MakeNodeList()
        {
            nodes = new List<Node>();
            copy = new List<Node>();
        }
        public void AddEdge(Node startNode, Node endNode)
        {
            //if (startNode.value == endNode.value) return;
            if (!this.FindNode(endNode))
            {
                copy.Add(endNode);
                nodes.Add(endNode);
            }
            else
                foreach(var e in nodes)
                    if(e.value==endNode.value)
                    {
                        endNode = e;
                        break;
                    }
            if (!this.FindNode(startNode))
            {
                nodes.Add(startNode);
                copy.Add(startNode);
            }
            else
                foreach (var e in nodes)
                    if (e.value == startNode.value)
                    {
                        startNode = e;
                        break;
                    }
            if (!startNode.ajNodes.Contains(endNode))
            {
                startNode.ajNodes.Add(endNode);
                endNode.ajNodes.Add(startNode);
                startNode.copyNodes.Add(endNode);
                endNode.copyNodes.Add(startNode);
            }
        }
        public void RemoveEdge(Node startNode, Node endNode)
        {
            if (startNode.ajNodes.Contains(endNode))
            {
                startNode.ajNodes.Remove(endNode);
                endNode.ajNodes.Remove(startNode);
            }
        }

        public bool FindNode(Node node)
        {
           foreach(var e in nodes)
            {
                if (e.value == node.value) return true;
            }
            return false;
        }
        public void AddNode(Node node)
        {
            if (!this.FindNode(node))
            {
                nodes.Add(node);
                copy.Add(node);
            }
        }
        public void RemoveNode(Node node)
        {
            if (!FindNode(node))
            {
                foreach (var e in nodes)
                    if (e.ajNodes.Contains(node))
                        RemoveEdge(e, node);
                nodes.Remove(node);
            }
        }

        private bool IsConnected()
        {
            Node nodeNum=new Node(-1);
            foreach(var e in nodes)
                if(e.ajNodes.Count==0) return false;
            // Находим вершину с ненулевой степенью
            foreach (var e in nodes)
                if (e.ajNodes.Count != 0)
                {
                    nodeNum = e;
                    break;
                }
            // Если в графе нет ребер
            if (nodeNum.value==-1) return true;

            //  Запускаем обход в глубину из вершины с ненулевой степенью
            DeepSearchCount(nodeNum);

            // Проверяем, все ли ненулевые вершины посещены
            foreach (var e in nodes)
                if (e.IsVisited==false && e.ajNodes.Count > 0)
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
            foreach (var e in nodes)
                if (e.ajNodes.Count % 2 != 0)
                    odd++;

            // если количество больше 2, то граф не эйлеров
            if (odd > 2)
            {
                Console.WriteLine("Граф не является Эйлеровым");
                return;
            }
            // если количество равно 2, то граф полу-эйлеров
            // если 0, то эйлеров 
            if (odd == 2)
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
        private void PrintEulerTour()
        {
            // находим вершину нечетной степени
            var u = new Node(-1);
            var v = new Node(-2);
            foreach(var e in nodes)
            {
                v = e;
                if(e.ajNodes.Count%2==1)
                {
                    u = e;
                    first = e;
                    break;
                }
            }
            if (u.value == -1)
            {
                first=last = v;
                FleryPrint(v);
            }
            else FleryPrint(u);
            Console.WriteLine();
            if (first != last)
            {
                foreach (var e in nodes) e.IsVisited = false;
                Console.WriteLine("Возвращаться будем так:");
                DeepSearch(last, last, first);
            }
        }

        // Печатаем путь, начиная с определенной вершины  
        private void FleryPrint(Node u)
        {
            // Повторяем для вершин, смежных данной
            foreach (var e in u.ajNodes)
            {
                Node v = e;
                // если ребро может быть рассмотрено следующим
                if (IsValidNextEdge(u, v))
                {
                    Console.Write(u.value + "-" + v.value + " ");
                    last = v;
                    // мы прошли по ребру, следовательно больше его не используем
                    RemoveEdge(u, v);
                    FleryPrint(v);
                    if (u.ajNodes.Count == 0) return;
                }
            }
        }

        private bool IsValidNextEdge(Node u, Node v)
        {
            if (u.ajNodes.Count == 1) return true;
            int count1 = DeepSearchCount(u);
            RemoveEdge(u, v);
            int count2 = DeepSearchCount(u);
            AddEdge(u, v);
            return (count1 > count2) ? false : true;
        }

        public int DeepSearchCount(Node node)
        {
            // помечаем текущую вершину как посещенную 
            node.IsVisited = true;
            int count = 1;
            foreach(var adj in node.ajNodes)// повторяем для всех инцидентных вершин 
            {
                if (adj.IsVisited==false)
                    count = count + DeepSearchCount(adj);
            }
            return count;
        }
        private void DeepSearch(Node node, Node start, Node final)
        {
            // помечаем текущую вершину как посещенную 
            node.IsVisited = true;
            Console.WriteLine(node.value);
            foreach (var adj in node.copyNodes)// повторяем для всех инцидентных вершин 
            {
                if (adj.IsVisited==false && start != final)
                    DeepSearch(adj, start, final);
            }
        }
    }
}