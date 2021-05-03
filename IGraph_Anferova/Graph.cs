using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace IGraph_Anferova
{
    class Graph
    {
        private Dictionary<string, Dictionary<string, int>> vertecies;
        public readonly bool focus; //ориентированность
        public readonly bool weigh; //взвешенность
        //конструктор пустой граф
        public Graph(bool focus = false, bool weigh = false)
        {
            vertecies = new Dictionary<string, Dictionary<string, int>>();
            this.focus = focus;
            this.weigh = weigh;
        }

        //конструктор заполнение графа из файла
        public Graph(string inputName)
        {
            vertecies = new Dictionary<string, Dictionary<string, int>>();
            using (StreamReader input = new StreamReader(inputName))
            {
                if (input.ReadLine() == "True")
                {
                    focus = true;
                }

                if (input.ReadLine() == "True")
                {
                    weigh = true;
                }

                string[] str; // переменная для считывания 

                // считывание и заполнение списка вершин
                string str1;
                if ((str1 = input.ReadLine()) != null)
                {
                    str = str1.Split();
                    {
                        foreach (var item in str)
                        {
                            AddVertex(item);
                        }
                    }
                }
                // считывание и заполнение списка ребер
                string pair;
             
                while ((pair = input.ReadLine()) != null)
                {
                        str = pair.Split();
                        AddEdge(str[0], str[1], int.Parse(str[2]));

                }
            

            }
        }

        //КОНСТРУКТОР - КОПИЯ
        public Graph(Graph g)
        {
            vertecies = new Dictionary<string, Dictionary<string, int>>();
            focus = g.focus;
            weigh = g.weigh;
            foreach (var v in g.vertecies)
            {
                AddVertex(v.Key);
            }

            foreach (var v in g.vertecies)
            {
                foreach (var e in v.Value)
                {
                    AddEdge(v.Key, e.Key, e.Value);
                }
            }

        }


        public bool AddVertex(string vertexName)
        {
            if (!vertecies.ContainsKey(vertexName))
            {
                vertecies.Add(vertexName, new Dictionary<string, int>());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddEdge(string a, string b, int weigth = 0)
        {
            if (vertecies.ContainsKey(a) && vertecies.ContainsKey(b))
            {
                if (!vertecies[a].ContainsKey(b))
                {
                    vertecies[a].Add(b, weigth);
                }
                else
                {
                    return false;
                }
                if (!focus)
                {
                    vertecies[b].Add(a, weigth);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteVertex(string vertexName)
        {
            foreach (var ver in vertecies)
            {
                ver.Value.Remove(vertexName);
            }
            return vertecies.Remove(vertexName);
        }

        public bool DeleteEdge(string a, string b)
        {
            if (vertecies.ContainsKey(a) && vertecies.ContainsKey(b))
            {
                if (focus)
                {
                    return vertecies[a].Remove(b) && vertecies[b].Remove(a);
                }
                else
                {
                    return vertecies[a].Remove(b);
                }
            }
            else
            {
                return false;
            }
        }

        public List<string> GetEdges()
        {
            List<string> result = new List<string>();

            result.Add(focus.ToString());
            result.Add(weigh.ToString());
            string ver = "";
            foreach (var v in vertecies.Keys)
            {
                ver += v + " ";
            }
            result.Add(ver);
            foreach (var v in vertecies)
            {

                foreach (var e in v.Value)
                {
                    result.Add(string.Format("{0} {1} {2}", v.Key, e.Key, e.Value));
                }
            }

            return result;
        }

        public void GetEdgesFile(string filename)
        {
            using (StreamWriter output = new StreamWriter(filename))
            {
                List<string> k = GetEdges();
                foreach (var item in k)
                {
                    output.WriteLine(item);
                }
            }
        }

        public List<string> SpisokSmezh()
        {
            List<string> result = new List<string>();
            foreach (var v in vertecies)
            {
                string str = string.Format("{0}: ", v.Key);
                if (weigh)
                {
                    foreach (var e in v.Value)
                    {
                        str += string.Format("{0}, {1}; ", e.Key, e.Value);
                    }
                }
                else
                {
                    foreach (var e in v.Value)
                    {
                        str += string.Format("{0}; ", e.Key);
                    }
                }
                result.Add(str);
            }

            return result;
        }

        public bool ContainsVertex(string vertexname)
        {
            if (vertecies.ContainsKey(vertexname))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //1а.2 Вывести  полустепень исхода данной вершины орграфа.
       public int HalfStepofVertexOut(string vertexname)
        {
            int n = vertecies[vertexname].Count();
            return n; 
        }

        //1a.11 Для данной вершины орграфа вывести все «заходящие» соседние вершины.
        public List<string> InputVertex(string vertexname)
        {
            List<string> result = new List<string>();
            foreach (var v in vertecies)
            {
                foreach(var e in v.Value)
                {
                    if (e.Key == vertexname)
                    {
                        result.Add(v.Key);
                    }
                }
            }
            return result;
          
        }

        public int VertexDegree(string vertexname)
        {
            int n = 0;
            if (focus)
            {
                n += vertecies[vertexname].Count;
                n += InputVertex(vertexname).Count;
            }
            else
            {
                n += HalfStepofVertexOut(vertexname);
            }

            //Console.WriteLine($"{vertexname} : {n}");
            return n;
        }

        //1b.18 Построить граф, полученный однократным удалением вершин с нечётными степенями.
        public Graph NewGrafDeleteVertex()
        {
            Graph newgraph = new Graph(this);
            foreach (var i in vertecies.Keys)
            {
                if ((VertexDegree(i) % 2) == 1)
                {
                    newgraph.DeleteVertex(i);
                }
            }
            return newgraph;
        }

        public Graph GraphReverse()
        {
            Graph result = new Graph(focus, weigh);

            foreach (var v in vertecies)
            {
                result.AddVertex(v.Key);
            }

            foreach (var v in vertecies)
            {
                foreach (var e in v.Value)
                {
                    result.AddEdge(e.Key, v.Key, e.Value);
                }
            }
            return result;
        }


        //II.14 Выяснить, является ли орграф сильно связным.
        public void DFS(List<string> a, string vertexname, Dictionary<string, bool> visited)
        {
            visited[vertexname] = true;
            a.Add(vertexname);
            foreach(var v in vertecies[vertexname].Keys)
            {
                if (visited[v] == false)
                {
                    DFS(a, v, visited);
                }
            }
        }

        public bool Kossaru()
        {
            if (vertecies.Count == 0)
            {
                return true;
            }
            List <string> l = new List <string> ();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var v in vertecies.Keys)
            {
                visited.Add(v, false);
            }
            DFS(l, vertecies.ElementAt(0).Key, visited);
            List <string> l1 = new List <string>();
            if (l.Count() < vertecies.Keys.Count())
            {
                return false;
            }
            else
            {
                Graph grrev = this.GraphReverse();
                Dictionary<string, bool> visited1 = new Dictionary<string, bool>();
                foreach (var v in vertecies.Keys)
                {
                    visited1.Add(v, false);
                }
                grrev.DFS(l1, grrev.vertecies.ElementAt(0).Key, visited1);
                if (l1.Count() < grrev.vertecies.Keys.Count())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        //II.31 Распечатать кратчайшие(по числу рёбер) пути от всех вершин до u.

        public Dictionary<string,string> BFS(string u)
        {
            Queue <string> q = new Queue <string> ();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var v in vertecies.Keys)
            {
                visited[v] = false;
            }
           
            Dictionary<string, string> pathes = new Dictionary<string, string>();
            visited[u] = true;
            q.Enqueue(u);

            pathes[u] = u;

            while (q.Count > 0)
            {
                string now = q.Dequeue();
                foreach (var v in vertecies[now].Keys)
                {
                    if (!visited[v])
                    {
                        q.Enqueue(v);
                        visited[v] = true;
                        pathes[v] = pathes[now] + " " + v;
                    }    
                }
            }
            foreach (var v in visited)
            {
                if (!v.Value)
                {
                    pathes[v.Key] = "path no";
                }
            }
            return pathes;
        }
            public List<string> ShortestPathToUHeart(string vertexname)
        {
            Graph graph = GraphReverse();
            Dictionary<string, string> pathes = new Dictionary<string, string>();
            List <string> result = new List <string>();
            pathes = graph.BFS(vertexname);

            foreach (var v in pathes)
            {
                string str = string.Format("{0}: ", v.Key);
                string[] str1 = v.Value.Split();
                Array.Reverse(str1);
                    foreach (var e in str1)
                    {
                        str += string.Format("{0} ", e);
                    }
                result.Add(str);
            }
            return result;
        }

        ///III.Прим каркас
        public List<string> Prim()
        {
            // словарь не повторяющихся ребер
            Dictionary<KeyValuePair<string, string>, int> edges = new Dictionary<KeyValuePair<string, string>, int>();
            foreach (var ver in vertecies.Keys)
            {
                foreach (var edg in vertecies[ver])
                {
                    // проверка на вхождение в словарь обратного ребра
                    KeyValuePair<string, string> pair = new KeyValuePair<string, string>(edg.Key, ver);
                    if (!edges.ContainsKey(pair))
                    {
                        edges.Add(new KeyValuePair<string, string>(ver, edg.Key), edg.Value);
                    }
                }
            }

            // граф для каркаса
            Graph g = new Graph(focus, weigh);
            foreach (var v in vertecies.Keys)
            {
                g.AddVertex(v);
            }

            List<string> notUsedV = new List<string>(vertecies.Keys);
            List<string> usedV = new List<string>();

            usedV.Add(notUsedV[0]);
            notUsedV.RemoveAt(0);

            // пока остались неиспользованные вершины
            while (notUsedV.Count > 0)
            {
                // находим миниальное из доступных ребер
                KeyValuePair<KeyValuePair<string, string>, int> edge = edges.MinEdge(usedV);
                
                // удаляем его из рассмотрения
                edges.Remove(new KeyValuePair<string, string>(edge.Key.Key, edge.Key.Value));

                // добавляем его в каркас
                g.AddEdge(edge.Key.Key, edge.Key.Value, edge.Value);

                // смотрим, какой какой из его концов "новый"
                if (!usedV.Contains(edge.Key.Key))
                {
                    usedV.Add(edge.Key.Key);
                    notUsedV.Remove(edge.Key.Key);
                }

                if (!usedV.Contains(edge.Key.Value))
                {
                    usedV.Add(edge.Key.Value);
                    notUsedV.Remove(edge.Key.Value);
                }
            }

            // возвращаем список смежности каркаса
            return g.SpisokSmezh();
        }

        //IVa.2 Дейкстра. Определить, есть ли в графе вершина, минимальные стоимости путей от которой до остальных в сумме не превосходят P. 
        public List<string> SumPathMinP(int p)
        {
            List<string> list = new List<string>();
            foreach (var ver in vertecies.Keys)
            {
                Dictionary<string, long> Dists = Dijkstr(ver);
                long sum = 0;
                foreach (var d in Dists.Values)
                {
                    //Console.WriteLine(d);
                    if (d != int.MaxValue)
                    {
                        sum += d;
                    }
                }
                if (sum<=p)
                {
                    list.Add(ver + $" {sum}");
                }
            }

            if (list.Count != 0)
            {
                return list;
            }
            else
            {
                list.Add("Вершина не найдена");
                return list;
            }
        }

        public Dictionary<string, long> Dijkstr(string v)
        {
            Dictionary<string, bool> unvisited = new Dictionary<string, bool>();
            foreach (var u in vertecies.Keys)
            {
                unvisited.Add(u, true);
            }

            unvisited[v] = false; // помечаем вершину v как просмотренную

            //создаем матрицу с
            Dictionary<string, Dictionary<string, int>> c = new Dictionary<string, Dictionary<string, int>>();
            foreach(var ver1 in vertecies.Keys)
            {
                c.Add(ver1, new Dictionary<string, int>());
                foreach (var ver2 in vertecies.Keys)
                {
                    if (!vertecies[ver1].ContainsKey(ver2) || ver1 == ver2)
                    {
                        c[ver1][ver2] = int.MaxValue;
                    }
                    else
                    {
                        c[ver1][ver2] = vertecies[ver1][ver2];
                    }
                }
            }

            //создаем матрицы d и p
            Dictionary<string, long> d = new Dictionary<string, long>();
            foreach (var u in vertecies.Keys)
            {
                if (u != v)
                {
                    d[u] = c[v][u];
                }
            }

            //for (int i = 0; i < c.Count - 1; i++)// на каждом шаге цикла
            foreach (var i in vertecies.Keys)
            {
                // выбираем из множества V\S такую вершину w, что D[w] минимально
                long min = int.MaxValue;
                string w = "";
                //for (int u = 0; u < Size; u++)
                foreach(var u in vertecies.Keys)
                {
                    if (unvisited[u] && min > d[u])
                    {
                        min = d[u];
                        w = u;
                    }
                }

                if (w == "") break;

                unvisited[w] = false; //помещаем w в множество S
                                //для каждой вершины из множества V\S определяем кратчайший путь от
                                // источника до этой вершины
                
                //for (int u = 0; u < Size; u++)
                foreach(var u in vertecies.Keys)
                {
                    long distance = d[w] + c[w][u];
                    if (unvisited[u] && d[u] > distance)
                    {
                        d[u] = distance;
                    }
                }
            }
            return d; //в качестве результата возвращаем массив кратчайших путей для
        } //заданного источника

        //IV.b.9 Вывести длины кратчайших путей для всех пар вершин.

        public Dictionary<string, Dictionary<string, int>> Floyd (out Dictionary<string, Dictionary<string, string>> pathes)
        {
            //матрица смежности
            Dictionary<string, Dictionary<string, int>> c = new Dictionary<string, Dictionary<string, int>>();

            pathes = new Dictionary<string, Dictionary<string, string>>();

            foreach (var ver1 in vertecies.Keys)
            {
                c.Add(ver1, new Dictionary<string, int>());
                pathes.Add(ver1, new Dictionary<string, string>());
                foreach (var ver2 in vertecies.Keys)
                {
                    if (!vertecies[ver1].ContainsKey(ver2) || ver1 == ver2)
                    {
                        c[ver1][ver2] = int.MaxValue/2;
                        pathes[ver1][ver2] = "-";
                    }
                    else
                    {
                        c[ver1][ver2] = vertecies[ver1][ver2];
                        pathes[ver1][ver2] = ver2;
                    }
                }
            }

            foreach (var i in vertecies.Keys)
            {
                foreach (var j in vertecies.Keys)
                {
                    foreach (var k in vertecies.Keys)
                    {
                        if (c[j][k] > c[j][i] + c[i][k])
                        {
                            c[j][k] = c[j][i] + c[i][k];
                            pathes[j][k] = pathes[j][i];
                        }
                    }
                }
            }
            return c;
        }


        //IV.c.Вывести кратчайшие пути из вершины u до v1 и v2.
        public List<string> FordToFromU(string u, string v1, string v2)
        {
            // применяем алгоритм Флойда к графу
            var dists = Ford(u, out var pathes, out var cyrcles);

            List<string> result = new List<string>();
            //if (dists[v1]== - int.MaxValue/2)
            //{
            //    result.Add(string.Format("Из {0} в {1} : цикл отрицательный", u, v1));
            //}
            if (pathes[v1] == "-")
            {
                result.Add(string.Format("Из {0} в {1} : пути нет", u, v1));
            }
            else
            {
                var path = FordWay(pathes, cyrcles, u, v1);
                result.Add($"Из {u} в {v1} : {path.Key} ({path.Value})");
            }

            //if (dists[v2] == -int.MaxValue / 2)
            //{
            //    result.Add(string.Format("Из {0} в {1} : цикл отрицательный", u, v2));
            //}
            if (pathes[v2] == "-")
            {
                result.Add(string.Format("Из {0} в {1} : пути нет", u, v2));
            }
            else
            {
                var path = FordWay(pathes, cyrcles,  u, v2);
                result.Add($"Из {u} в {v2} : {path.Key} ({path.Value})");
            }

            return result;
        }

        public Dictionary<string, int> Ford(string s, out Dictionary<string, string> pathes, out List<string> cyrcles)
        {
            //матрица смежности
            Dictionary<string, int> d = new Dictionary<string, int>();

            pathes = new Dictionary<string, string>();

            foreach (var v in vertecies.Keys)
            {
                d[v] = int.MaxValue / 2;
                pathes[v] = "-";
            }
            d[s] = 0;
            pathes[s] = s;

            cyrcles = new List<string>();

            for(int i = 0; i < vertecies.Count; i++)
            {
                foreach (var v1 in vertecies.Keys)
                {
                    foreach (var v2 in vertecies[v1].Keys)
                    {
                        if (d[v1] < int.MaxValue / 2)// проверка, чтобы работало с ребрами отр. веса
                        {
                            if (d[v2] > d[v1] + vertecies[v1][v2])
                            {
                                d[v2] = d[v1] + vertecies[v1][v2];
                                pathes[v2] = v1;

                                if (i == vertecies.Count - 1)
                                {
                                    cyrcles.Add(v2);
                                }
                            }
                        }
                    }
                }
            }


            return d;
        }
        public KeyValuePair<string, int> FordWay(Dictionary<string, string> pathes, List<string> cyrcles, string u, string v)
        {
            string way = v;
            string cur = v;

            if(cyrcles.Contains(v) || cyrcles.Contains(u))
            {
                return new KeyValuePair<string, int>("цикл отрицательного веса", -int.MaxValue);
            }

            int w = 0;
            while (pathes[cur] != u)
            {
                w += vertecies[pathes[cur]][cur];
                if (cyrcles.Contains(cur))
                {
                    return new KeyValuePair<string, int>("цикл отрицательного веса", -int.MaxValue);
                }
                way += $" {pathes[cur]}";
                cur = pathes[cur];
            }
            way += $" {u}";
            w += vertecies[u][cur];
            char[] arr = way.ToCharArray();
            Array.Reverse(arr);
            return new KeyValuePair<string, int> (new string(arr), w);
        }


        //ПОТОКИ
        bool Bfs(Dictionary<string, Dictionary<string, int>> rGraph, string s, string t, Dictionary<string, string> parent)
        {
            // Создаем словарь посещенных и отмечаем все вершины как не посещенные
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var item in vertecies.Keys)
            {
                visited.Add(item, false);
            }

            // Создаем очередь, исходная вершина как посещенная
            Queue<string> q = new Queue<string>();
            q.Enqueue(s);
            visited[s] = true;
            parent[s] = "";

            // BFS
            //Если мы достигли t, то true
            while (q.Count != 0)
            {
                string u = q.Dequeue();

                //for (int v = 0; v < V; v++)
                foreach (var v in rGraph.Keys)
                {
                    if (visited[v] == false && rGraph[u][v] > 0)
                    {
                        q.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }
            return (visited[t] == true);
        }

        // Возвращает максимальный поток от s до t в данном графике
        public int FordFulkerson(string s = "s", string t = "t")
        {
            string u, v;

            //создаем матрицу rGraph
            //матрица смежности
            Dictionary<string, Dictionary<string, int>> rGraph = new Dictionary<string, Dictionary<string, int>>();
            foreach (var ver1 in vertecies.Keys)
            {
                rGraph.Add(ver1, new Dictionary<string, int>());
                foreach (var ver2 in vertecies.Keys)
                {
                    if (!vertecies[ver1].ContainsKey(ver2) || ver1 == ver2)
                    {
                        rGraph[ver1][ver2] = 0;
                    }
                    else
                    {
                        rGraph[ver1][ver2] = vertecies[ver1][ver2];
                    }
                }
            }

            // Остаточный граф (уч в бфс)
            Dictionary<string, string> parent = new Dictionary<string, string>();  

            int max_flow = 0;  

            // Увелич. поток, пока есть путь от s к t
            while (Bfs(rGraph, s, t, parent))
            {
                // Минимальная остаточная емкость ребер
                int path_flow = int.MaxValue;
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    path_flow = Math.Min(path_flow, rGraph[u][v]);
                }

                // изменяем значения пропускной способности
                for (v = t; v != s; v = parent[v])
                {
                    u = parent[v];
                    rGraph[u][v] -= path_flow;
                    rGraph[v][u] += path_flow;
                }

                
                max_flow += path_flow;
            }

            return max_flow;
        }

    }


    public static class Expansion
    {
        // метод возвращает ребро с минимальным весом, один конец которого входит в дерево
        public static KeyValuePair<KeyValuePair<string, string>, int> MinEdge
            (this Dictionary<KeyValuePair<string, string>, int> edges,
            List<string> usedV)
        {
            // делаем новый словарь, куда входят только подходящие ребра
            Dictionary<KeyValuePair<string, string>, int> rightEdges = new Dictionary<KeyValuePair<string, string>, int>();
            foreach (var item in edges)
            {
                if ((usedV.Contains(item.Key.Key) && !usedV.Contains(item.Key.Value)) ||
                   (!usedV.Contains(item.Key.Key) && usedV.Contains(item.Key.Value)))
                {
                    rightEdges.Add(item.Key, item.Value);
                }
            }


            // находим минимальное из подходящих ребер
            KeyValuePair<KeyValuePair<string, string>, int> minEdge = rightEdges.First();
            foreach (var item in rightEdges)
            {
                if (item.Value < minEdge.Value)
                {
                    minEdge = item;
                }
            }

            return minEdge;
        }
    }
}
