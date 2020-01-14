using System;
using System.Diagnostics;
using System.IO;

namespace SemesterTask
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            Console.WriteLine("Для проверки корректности работы алгоритма сгенерируем случайный граф");
            var graph = new Graph();
            graph = InputGraph();
            var n5 = new Node(5);
            graph.AddEdge(n5, n5);
            graph.IsEulerian();
            //while (flag)
            //{
            //    var g1 = RandomGraph();
            //    g1.IsEulerian();
            //    Console.WriteLine("Сгенерировать еще один граф? 1-да, 0-нет");
            //    int num = Convert.ToInt32(Console.ReadLine());
            //    if (num != 1) flag = false;
            //}
        }
        public static Graph RandomGraph()
        {
            var rnd = new Random();
            int length = rnd.Next(2,10);
            var graph = new Graph();
            //Console.WriteLine("Длина графа равна " + length);
            var eaglesCount = rnd.Next(1, length * (length - 1) / 2);
            Console.WriteLine("Количество вершин в графе равно " + eaglesCount);
            Console.WriteLine("Создадим ребра: ");
            for (int i = 0; i < eaglesCount-1; i++)
            {
                var startNode = rnd.Next(0, length);
                var endNode = rnd.Next(0, length);
                while (startNode == endNode) endNode = rnd.Next(0, length);
                Node n1=new Node(startNode);
                Node n2 = new Node(endNode);
                graph.AddEdge(n1, n2);
                Console.WriteLine(startNode + "-" + endNode);
            }
            return graph;
        }
        public static Graph InputGraph()
        {
            var graph = new Graph();
            var reader = new StreamReader(@"C:\Users\Acer\Desktop\Я\Учеба\Алгоритмы и анализ сложности\Практики\SemesterTask\input.txt");
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[]arr=line.Split(' ');
                var node = new Node(Int32.Parse(arr[0]));
                if (arr.Length == 1) graph.AddNode(node);
                else
                for (int i = 1; i < arr.Length; i++)
                {
                    var edNode = new Node(Int32.Parse(arr[i]));
                    graph.AddEdge(node, edNode);
                }
            }
            return graph;
        }
    }
}

