using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace SemesterTask
{
    public class Edge
    {
        public readonly MyNode From;
        public readonly MyNode To;
        public Edge(MyNode first, MyNode second)
        {
            this.From = first;
            this.To = second;
        }
        public bool IsIncident(MyNode node)
        {
            return From == node || To == node;
        }
        public MyNode OtherNode(MyNode node)
        {
            if (!IsIncident(node)) throw new ArgumentException();
            if (From == node) return To;
            return From;
        }
    }

    public class MyNode
    {
        readonly List<Edge> edges = new List<Edge>();
        public readonly int Data;

        public MyNode(int number)
        {
            Data = number;
        }

        public IEnumerable<MyNode> IncidentNodes
        {
            get
            {
                return edges.Select(z => z.OtherNode(this));
            }
        }
        public IEnumerable<Edge> IncidentEdges
        {
            get
            {
                foreach (var e in edges) yield return e;
            }
        }
        public static Edge Connect(MyNode node1, MyNode node2, MyGraph graph)
        {
            if (!graph.Nodes.Contains(node1) || !graph.Nodes.Contains(node2)) throw new ArgumentException();
            var edge = new Edge(node1, node2);
            node1.edges.Add(edge);
            node2.edges.Add(edge);
            return edge;
        }
        public static void Disconnect(Edge edge)
        {
            edge.From.edges.Remove(edge);
            edge.To.edges.Remove(edge);
        }
    }

    public class MyGraph
    {
        private MyNode[] nodes;
        public MyGraph(int nodesCount)
        {
            nodes = Enumerable.Range(0, nodesCount).Select(z => new MyNode(z)).ToArray();
        }
        public int Length { get { return nodes.Length; } }

        public MyNode this[int index] { get { return nodes[index]; } }

        public IEnumerable<MyNode> Nodes
        {
            get
            {
                foreach (var node in nodes) yield return node;
            }
        }

        public void Connect(int index1, int index2)
        {
            MyNode.Connect(nodes[index1], nodes[index2], this);
        }

        public void Delete(Edge edge)
        {
            MyNode.Disconnect(edge);
        }

        public IEnumerable<Edge> Edges
        {
            get
            {
                return nodes.SelectMany(z => z.IncidentEdges).Distinct();
            }
        }

        public static MyGraph MakeGraph(params int[] incidentNodes)
        {
            var graph = new MyGraph(incidentNodes.Max() + 1);
            for (int i = 0; i < incidentNodes.Length - 1; i += 2)
                graph.Connect(incidentNodes[i], incidentNodes[i + 1]);
            return graph;
        }
    }
}
