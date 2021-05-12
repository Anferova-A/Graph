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
        private Dictionary<string, Dictionary<string, int>> vertices;
        public readonly bool focus; //ориентированность
        public readonly bool weigh; //взвешенность
        //конструктор пустой граф
        public Graph(bool focus = false, bool weigh = false)
        {
            vertices = new Dictionary<string, Dictionary<string, int>>();
            this.focus = focus;
            this.weigh = weigh;
        }

        //конструктор заполнение графа из файла
        public Graph(string inputName)
        {
            vertices = new Dictionary<string, Dictionary<string, int>>();
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
            vertices = new Dictionary<string, Dictionary<string, int>>();
            focus = g.focus;
            weigh = g.weigh;
            foreach (var v in g.vertices)
            {
                AddVertex(v.Key);
            }

            foreach (var v in g.vertices)
            {
                foreach (var e in v.Value)
                {
                    AddEdge(v.Key, e.Key, e.Value);
                }
            }

        }


        public bool AddVertex(string vertexName)
        {
            if (!vertices.ContainsKey(vertexName))
            {
                vertices.Add(vertexName, new Dictionary<string, int>());
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddEdge(string a, string b, int weigth = 0)
        {
            if (vertices.ContainsKey(a) && vertices.ContainsKey(b))
            {
                if (!vertices[a].ContainsKey(b))
                {
                    vertices[a].Add(b, weigth);
                }
                else
                {
                    return false;
                }
                if (!focus)
                {
                    vertices[b].Add(a, weigth);
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
            foreach (var ver in vertices)
            {
                ver.Value.Remove(vertexName);
            }
            return vertices.Remove(vertexName);
        }

        public bool DeleteEdge(string a, string b)
        {
            if (vertices.ContainsKey(a) && vertices.ContainsKey(b))
            {
                if (focus)
                {
                    return vertices[a].Remove(b) && vertices[b].Remove(a);
                }
                else
                {
                    return vertices[a].Remove(b);
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
            foreach (var v in vertices.Keys)
            {
                ver += v + " ";
            }
            result.Add(ver);
            foreach (var v in vertices)
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
            foreach (var v in vertices)
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
            if (vertices.ContainsKey(vertexname))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Является ли орграф сильно связным.
        /// </summary>
        public void DFS(List<string> a, string vertexname, Dictionary<string, bool> visited)
        {
            visited[vertexname] = true;
            a.Add(vertexname);
            foreach (var v in vertices[vertexname].Keys)
            {
                if (visited[v] == false)
                {
                    DFS(a, v, visited);
                }
            }
        }

        public bool Kossaru()
        {
            if (vertices.Count == 0)
            {
                return true;
            }
            List<string> l = new List<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var v in vertices.Keys)
            {
                visited.Add(v, false);
            }
            DFS(l, vertices.ElementAt(0).Key, visited);
            List<string> l1 = new List<string>();
            if (l.Count() < vertices.Keys.Count())
            {
                return false;
            }
            else
            {
                Graph grrev = this.GraphReverse();
                Dictionary<string, bool> visited1 = new Dictionary<string, bool>();
                foreach (var v in vertices.Keys)
                {
                    visited1.Add(v, false);
                }
                grrev.DFS(l1, grrev.vertices.ElementAt(0).Key, visited1);
                if (l1.Count() < grrev.vertices.Keys.Count())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public Graph GraphReverse()
        {
            Graph result = new Graph(focus, weigh);

            foreach (var v in vertices)
            {
                result.AddVertex(v.Key);
            }

            foreach (var v in vertices)
            {
                foreach (var e in v.Value)
                {
                    result.AddEdge(e.Key, v.Key, e.Value);
                }
            }
            return result;
        }

        public enum Part
        {
            None,
            First,
            Second
        }

        /// <summary>
        /// Проверка графа на двудольность
        /// (https://e-maxx.ru/algo/bipartite_checking)
        /// </summary>
        /// <returns>
        /// Cловарь разбиений, если граф двудольный, в противном случе null
        /// </returns>
        public IDictionary<string, Part> CheckBipartition()
        {
            // Инициализация словаря вершин-долей
            var partDictionary = new Dictionary<string, Part>();
            foreach (var item in vertices.Keys)
            {
                partDictionary.Add(item, Part.None);
            }

            // запускаем серию обходов в ширину
            foreach (var startV in vertices.Keys)
            {
                // Если вершина не посещена, начинаем обход ширину из нее
                if (partDictionary[startV] is Part.None)
                {
                    // Инициализируем очередь
                    var q = new Queue<string>();
                    q.Enqueue(startV);

                    // Помещаем стартовую вершину в первую долю
                    partDictionary[startV] = Part.First;

                    while (q.Count > 0)
                    {
                        var now = q.Dequeue();
                        foreach (var adj in vertices[now].Keys) // проходим по смежным вершинам
                        {
                            // если вершина еще не посещена 
                            if (partDictionary[adj] is Part.None)
                            {
                                // то добавляем ее в очередь
                                q.Enqueue(adj);

                                // и помещаем ее в какую-то долю
                                partDictionary[adj] = partDictionary[now] == Part.First
                                                    ? Part.Second
                                                    : Part.First;
                            }
                            // если вершина посещена, смотрим, чтобы они не были из одной доли
                            else if (partDictionary[now] == partDictionary[adj])
                            {
                                return null;
                            }
                        }

                    }

                }
            }


            return partDictionary;
        }

      


        /// <summary>
        /// Алгоритм Куна для поиска максимального паросочетания
        /// (http://e-maxx.ru/algo/kuhn_matching)
        /// </summary>
        public IDictionary<string, string> CoonAlgorithm()
        {
            var parts = CheckBipartition();

            if (parts == null)
                return null;

            var firstPart = parts.Where(item => item.Value == Part.First)
                                 .Select(item => item.Key);

            var secondPart = parts.Where(item => item.Value == Part.Second)
                                 .Select(item => item.Key);

            var matching = new Dictionary<string, string>();
            foreach (var item in secondPart)
            {
                matching.Add(item, null);
            }

            foreach (var item in firstPart)
            {
                var used = new Dictionary<string, bool>();
                foreach (var v in firstPart)
                {
                    used.Add(v, false);
                }

                DfsKun(item, used, matching);
            }


            return matching;
        }

        private bool DfsKun(string v, Dictionary<string, bool> used, Dictionary<string, string> matching)
        {
            if (used[v])
                return false;

            used[v] = true;

            foreach (var to in vertices[v].Keys)
            {
                if (matching[to] == null || DfsKun(matching[to], used, matching))
                {
                    matching[to] = v;
                    return true;
                }
            }
            return false;
        }

        #region Временно
        public Dictionary<string, string> BFS(string u)
        {
            Queue<string> q = new Queue<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var v in vertices.Keys)
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
                foreach (var v in vertices[now].Keys)
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
            List<string> result = new List<string>();
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
        #endregion

        /// <summary>
        /// Алгоритм Форда-Фалкерсона для поиска максимального паросочетания
        /// (<see href="link">https://clck.ru/EvgtS</see>)
        /// </summary>
        /// <returns>Словарь паросочетаний (Null, если граф не двудольный)</returns>
        public IDictionary<string, string> FordFulkerson()
        {

            var parts = CheckBipartition();

            if (parts == null)
                return null;

            var firstPart = parts.Where(item => item.Value == Part.First)
                                 .Select(item => item.Key);

            var secondPart = parts.Where(item => item.Value == Part.Second)
                                 .Select(item => item.Key);

            var px = new Dictionary<string, string>();
            foreach (var item in firstPart)
            {
                px.Add(item, null);
            }


            var py = new Dictionary<string, string>();
            foreach (var item in secondPart)
            {
                py.Add(item, null);
            }

            var vis = new Dictionary<string, bool>();



            bool isPath = true;

            while (isPath)
            {
                isPath = false;

                // fill
                foreach (var item in vertices.Keys)
                {
                    vis[item] = false;
                }

                foreach (var x in firstPart)
                {
                    if (px[x] == null)
                    {
                        isPath = DfsFord(x, px, py, vis);
                    }
                }
            }



            return px.Where(item => item.Value != null)
                     .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private bool DfsFord(string x, Dictionary<string, string> px, Dictionary<string, string> py, Dictionary<string, bool> vis)
        {
            if (vis[x])
                return false;

            vis[x] = true;

            foreach (var y in vertices[x].Keys)
            {
                if (py[y] == null)
                {
                    py[y] = x;
                    px[x] = y;
                    return true;
                }
                else
                {
                    if (DfsFord(py[y], px, py, vis))
                    {
                        py[y] = x;
                        px[x] = y;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Алгоритм Хопкрофта—Карпа для поиска максимального паросочетания
        /// (<see href="link">https://clck.ru/UksT4</see>)
        /// </summary>
        /// <returns>Словарь паросочетаний (Null, если граф не двудольный)</returns>
        public IDictionary<string, string> HopcroftKarp()
        {
            AddVertex("NIL");

            var parts = CheckBipartition();

            if (parts == null)
                return null;

            var firstPart = parts.Where(item => item.Value == Part.First)
                                 .Select(item => item.Key);

            var secondPart = parts.Where(item => item.Value == Part.Second)
                                 .Select(item => item.Key);

            var partU = new Dictionary<string, string>();
            foreach (var u in firstPart)
            {
                partU[u] = "NIL";
            }

            var partV = new Dictionary<string, string>();
            foreach (var v in secondPart)
            {
                partV[v] = "NIL";
            }

            var dist = new Dictionary<string, int>();

            while (BfsHopcroft(partU, partV, dist))
            {
                foreach (var u in firstPart)
                {
                    if (partU[u] == "NIL")
                    {
                        DfsHopcroft(u, partU, partV, dist);

                    }
                }
            }


            DeleteVertex("NIL");

            
            return partU.Where(item => item.Value != "NIL")
                        .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private bool DfsHopcroft(string u, Dictionary<string, string> partU, Dictionary<string, string> partV, Dictionary<string, int> dist)
        {
            if (u != "NIL")
            {
                foreach (var v in vertices[u].Keys)
                {
                    if (dist[partV[v]] == dist[u] + 1)
                    {
                        if (DfsHopcroft(partV[v], partU, partV, dist))
                        {
                            partV[v] = u;
                            partU[u] = v;
                            return true;
                        }
                    }
                }
                dist[u] = int.MaxValue;
                return false;
            }
            return true;
        }

        private bool BfsHopcroft(Dictionary<string, string> partU, Dictionary<string, string> partV, Dictionary<string, int> dist)
        {
            var q = new Queue<string>();

            foreach (var u in partU.Keys)
            {
                if (partU[u] == "NIL")
                {
                    dist[u] = 0;
                    q.Enqueue(u);
                }
                else
                {
                    dist[u] = int.MaxValue;
                }
            }

            dist["NIL"] = int.MaxValue;

            while (q.Count > 0)
            {
                string u = q.Dequeue();

                if (dist[u] < dist["NIL"])
                {
                    foreach (var v in vertices[u].Keys)
                    {
                        if (dist[partV[v]] == int.MaxValue)
                        {
                            dist[partV[v]] = dist[u] + 1;
                            q.Enqueue(partV[v]);
                        }
                    }
                }
            }

            return dist["NIL"] != int.MaxValue;
        }

    }

}
