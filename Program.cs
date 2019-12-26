using System;
using System.Diagnostics;

namespace SemesterTask
{
    class Program
    {
        static void Main(string[] args)
        {
            bool flag = true;
            Console.WriteLine("Для проверки корректности работы алгоритма сгенерируем случайный граф");
            while (flag)
            {
                var g1 = RandomGraph();
                g1.IsEulerian();
                Console.WriteLine("Сгенерировать еще один граф? 1-да, 0-нет");
                int num = Convert.ToInt32(Console.ReadLine());
                if (num != 1) flag = false;
            }
        }
        public static Graph RandomGraph()
        {
            var rnd = new Random();
            int length = rnd.Next(2, 20);
            var graph = new Graph(length);
            Console.WriteLine("Длина графа равна " + length);
            var eaglesCount = rnd.Next(1, length * (length - 1) / 2);
            Console.WriteLine("Количество ребер в графе равно " + eaglesCount);
            Console.WriteLine("Создадим ребра: ");
            for (int i = 0; i < eaglesCount; i++)
            {
                var startNode = rnd.Next(0, length);
                var endNode = rnd.Next(0, length);
                graph.AddEdge(startNode, endNode);
                Console.WriteLine(startNode + "-" + endNode);
            }
            return graph;
        }
    }
}

