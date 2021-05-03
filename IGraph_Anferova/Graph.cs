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

        /// <summary>
        /// Является ли орграф сильно связным.
        /// </summary>
        public void DFS(List<string> a, string vertexname, Dictionary<string, bool> visited)
        {
            visited[vertexname] = true;
            a.Add(vertexname);
            foreach (var v in vertecies[vertexname].Keys)
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
            List<string> l = new List<string>();
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            foreach (var v in vertecies.Keys)
            {
                visited.Add(v, false);
            }
            DFS(l, vertecies.ElementAt(0).Key, visited);
            List<string> l1 = new List<string>();
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

        #region Временно
        public Dictionary<string, string> BFS(string u)
        {
            Queue<string> q = new Queue<string>();
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

    }

}
