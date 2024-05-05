using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethod
{
    enum COLORS_VERTEX
    {
        WHITE,
        GRAY,
        BLACK
    }
    class Vertex
    {
        private static int IDV = 0;
        private int ID;
        public string label; // Метка (имя вершины)
        private List<Edge> edges; // Список ребер, связанных с вершиной
        public double sumdistance; // Сумма растояний
        public COLORS_VERTEX color; // Цвет вершины
        public Vertex prevvertex; // Ссылка на предшественника
        public bool visited;
        public int d;//время открытия
        public int t;//время закрытия

        public Vertex(string label) // Конструктор
        {
            this.label = label;
            IDV++;
            edges = new List<Edge>();
            sumdistance = Double.MaxValue;
            color = COLORS_VERTEX.WHITE;
            prevvertex = null;
            ID = IDV;
            this.visited = false;
        }
        public int GetID() { return ID; }
        // Получение списка ребер
        public List<Edge> GetEdges() { return edges; }
        public List<Vertex> GetNeighbor() { 
            List<Vertex> neighbors = new List<Vertex>();

            foreach (Edge curedge in edges)
                neighbors.Add(curedge.EndPoint);

            return neighbors;
        }
        public override string ToString()
        {
            string sout = "";
            sout = sout + label;
            sout = sout + "  ID=" + ID.ToString();
            return sout;
        }
        // Просмотр ребер, связанных с вершиной
        public void ViewEdges()
        {
            Console.Write("Edges for {0}", this);
            foreach (Edge curedge in edges)
                Console.Write("  {0}", curedge);
            Console.WriteLine();
        }
        // Добавление ребра
        public bool AddEdge(Edge edge)
        {
            if (edge.BeginPoint != this) return false;
            foreach (Edge CurEdge in edges)
            {
                if (edge.EndPoint.Equals(CurEdge.EndPoint)) return false;
            }
            edges.Add(edge);
            return true;
        }
    }

    class Edge
    {
        public Vertex BeginPoint; // Начальная вершина
        public Vertex EndPoint;  // Конечная вершина

        public double distance; // Длина ребра

        // Конструктор
        public Edge(Vertex begin, Vertex end, double d)
        {
            this.BeginPoint = begin;
            this.EndPoint = end;
            this.distance = d;
        }
        public override string ToString()
        {
            string sout = "";
            sout = "{" + BeginPoint.label + "  " + EndPoint.label + " D=" + distance.ToString() + "}";
            return sout;
        }
    }

    class Graph
    {
        public List<Vertex> allVertexs; // Список всех вершин
        public List<Edge> allEdges; // Список всех ребер

        int time = 0;// Конструктор
        public Graph()
        {
            allVertexs = new List<Vertex>();
            allEdges = new List<Edge>();
        }
        
        public bool AddVertex(Vertex vertex)
        {
            if (allVertexs.Contains(vertex)) return false;

            allVertexs.Add(vertex);

            return true;
        }

        // Добавление ребра ориентированный
        public bool AddEdgeDirected(Vertex v1, Vertex v2, double d)
        {
            if (!allVertexs.Contains(v1)) return false;
            if (!allVertexs.Contains(v2)) return false;
            foreach (Edge cure in v1.GetEdges())
            {
                if (cure.EndPoint.GetID() == v2.GetID()) return false;
            }


            Edge ev1v2 = new Edge(v1, v2, d);
            v1.GetEdges().Add(ev1v2); allEdges.Add(ev1v2);
            return true;
        }

        public bool AddEdgeUndirected(Vertex v1, Vertex v2, double d)
        {
            if (!allVertexs.Contains(v1)) return false;
            if (!allVertexs.Contains(v2)) return false;
            foreach (Edge cure in v1.GetEdges())
            {
                if (cure.EndPoint.GetID() == v2.GetID()) return false;
            }


            Edge ev1v2 = new Edge(v1, v2, d);
            Edge ev2v1 = new Edge(v2, v1, d);
            v1.GetEdges().Add(ev1v2); allEdges.Add(ev1v2);
            v2.GetEdges().Add(ev2v1); 
            return true;
        }

        // Поиск в ширину
        public List<Vertex> BFS(Vertex s)
        {
            List<Vertex> Vertices = new List<Vertex>();
            Queue<Vertex> Q = new Queue<Vertex>(); // Очередь вершин
                                                   // Инициализация
            foreach (Vertex cv in allVertexs)
            {
                cv.sumdistance = double.MaxValue;
                cv.prevvertex = null;
            }

            s.color = COLORS_VERTEX.GRAY;
            s.sumdistance = 0;
            Q.Enqueue(s);

            Vertex u, v;
            List<Edge> edges_u;
            // Основной цикл
            while (Q.Count > 0)
            {

                u = Q.Dequeue();

                foreach(Edge e in u.GetEdges())
                {
                    v=e.EndPoint;
                    if (v.color==COLORS_VERTEX.WHITE)
                    {
                        v.color = COLORS_VERTEX.GRAY;
                        v.sumdistance = u.sumdistance + 1;
                        v.prevvertex=u;
                        Q.Enqueue(v);
                    }
                }
                u.color=COLORS_VERTEX.BLACK;

                Vertices.Add(u);
            
            }
            return Vertices;
        }

        public List<Vertex> DFS(Vertex s)
        {
            List<Vertex> Vertices = new List<Vertex>();

            foreach (Vertex u in allVertexs)
            {
                u.color = COLORS_VERTEX.WHITE;
                u.prevvertex = null;
            }
            time = 0;
            
            DFS_Visit(s);

            foreach(Vertex v in allVertexs)
            {
                if(v.color==COLORS_VERTEX.WHITE) DFS_Visit(v);
            }
             

            void DFS_Visit(Vertex u)
            {
                u.color = COLORS_VERTEX.GRAY;
                time++;
                u.d = time;
                foreach(Edge e in u.GetEdges())
                {
                    Vertex v = e.EndPoint;
                    if (v.color == COLORS_VERTEX.WHITE)
                    {
                        v.prevvertex=u;

                        DFS_Visit(v);
                    }
                }

                u.color = COLORS_VERTEX.BLACK;
                
                time++;
                u.t=time;

                Vertices.Add(u);

            }
            return Vertices;
        }
        


        public List<Vertex> Get_Path(Vertex s, Vertex v)
        {
            List<Vertex> list = new List<Vertex>();
            if (v.sumdistance == double.MaxValue) return list;
            if (v == s) { list.Add(s); return list; }
            Vertex tmp;
            tmp = v;
            list.Add(v);
            while (tmp != null)
            { 
                if (tmp == s) return list;
                tmp = tmp.prevvertex;
                list.Add(tmp);
            }

            return new List<Vertex>();
        }


        public List<Edge> MST_Prim(Vertex r)
        {
            //Список для заполнения ребрами
            List<Edge> edges = new List<Edge>();

            //для подсчета суммарной длины
            double distance=0;

            //Используется список вместо очереди с приоритетами,
            // так как очередь с приоритетами не имеет методы Contain
            // для проверки есть ли вершина v в Q
            List<Vertex> Q = new List<Vertex>(allVertexs);

            //обнуляем все данные вершин
            foreach (Vertex v in allVertexs)
            {
                v.prevvertex = null;
                v.sumdistance = double.MaxValue;
            }

            //вес начальной вершины=0, чтобы начать с него
            r.sumdistance = 0;

            while (Q.Count>0)
            {
                //Выбирается вершина с минимальным весом
                Vertex u = Q[0];
                
                foreach(Vertex i in Q) { 
                    if(i.sumdistance < u.sumdistance) u = i;
                }
                
                //И удаляется
                Q.Remove(u);

                //Добавляется ребро
                if (u.prevvertex != null)
                {
                    foreach (Edge e in u.prevvertex.GetEdges())
                    {
                        if(u==e.EndPoint) edges.Add(e);
                    }
                    
                }
                //суммируется расстояние
                distance=distance+u.sumdistance;

                //Находятся расстояния точек смежных с u
                foreach(Edge e in u.GetEdges())
                {
                    Vertex v = e.EndPoint;

                    if (Q.Contains(v) && e.distance<v.sumdistance){
                        v.prevvertex = u;
                        v.sumdistance = e.distance;
                    }
                }
            }
            Console.WriteLine(distance);

            return edges;
        }







        //матрицa смежности

        public Matrix AdjacencyMatrix()
        {
            Matrix AdjacencyMatrix = new Matrix(allVertexs.Count,allVertexs.Count);


            for(int i = 0; i < allVertexs.Count; i++)
            {
                for(int j = 0; j < allVertexs.Count; j++)
                {
                    if ((allVertexs[i].GetNeighbor()).Contains(allVertexs[j]))
                    {
                        foreach (Edge edge in allVertexs[i].GetEdges())
                        {

                            if (allVertexs[j] == edge.EndPoint)
                            { 
                                AdjacencyMatrix[i, j] = edge.distance;

                                break;
                            }
                        }
                    }
                    else
                    {
                        AdjacencyMatrix[i, j] = 0.0;
                    }

                }
            }

            return AdjacencyMatrix;
        }

    }
}
