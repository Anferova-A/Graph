using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IGraph_Anferova
{
    class Program
    {
        public static void Menu(Graph graph)
        {
            Console.WriteLine("Выберите действие:\n" +
                    "1. Считать граф из файла\n" +
                    "2. Добавить вершину\n" +
                    "3. Добавить ребро(дугу)\n" +
                    "4. Удалить вершину\n" +
                    "5. Удалить ребро(дугу)\n" +
                    "6. Вывести список смежности в файл\n" +
                    "7. Вывести список смежности на экран\n" +
                    "8. Вывести  полустепень исхода данной вершины орграфа.\n" +
                    "9. Вывести  заходящие вершины для данной вершины.\n" +
                    "10. Построить граф, полученный однократным удалением вершин с нечётными степенями.\n" +
                    "11. Является ли орграф сильно связанным? \n" +
                    "12. Распечатать кратчайшие(по числу рёбер) пути от всех вершин до u \n" +
                    "13. Найти каркас минимального веса \n" +
                    "14. Определить, есть ли в графе вершина, минимальные стоимости путей от которой до остальных в сумме не превосходят P. \n" +
                    "15. Вывести длины кратчайших путей для всех пар вершин. \n" +
                    "16. Вывести кратчайшие пути от u до v1 и v2. \n" +
                    "17. Вычисление максимального потока (Форд-Фалкернсон)");

            string n = Console.ReadLine();
            while (n != "18")
            {
                switch (n)
                {
                    case "1":
                        graph = new Graph("Input_Graph.txt");
                        break;
                    case "2":
                        {
                            Console.WriteLine("Введите название вершины, которую хотите добавить:");
                            string vertex_name = Console.ReadLine();
                            if (graph.AddVertex(vertex_name))
                            {
                                Console.WriteLine("Вершина {0} добавлена", vertex_name);
                            }
                            else
                            {
                                Console.WriteLine("Вершина {0} уже существует", vertex_name);
                            }
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Введите название первой вершины:");
                            string istok = Console.ReadLine();
                            Console.WriteLine("Введите название второй вершины:");
                            string stok = Console.ReadLine();
                            if (graph.weigh)
                            {
                                Console.WriteLine("Вес: ");
                                int weigth = int.Parse(Console.ReadLine());

                                if (graph.AddEdge(istok, stok, weigth))
                                {
                                    Console.WriteLine("Ребро {0} {1} {2} добавлено", istok, stok, weigth);
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка при добавлении ребра");
                                }
                            }
                            else
                            {
                                if (graph.AddEdge(istok, stok))
                                {
                                    Console.WriteLine("Ребро {0} {1} добавлено", istok, stok);
                                }
                                else
                                {
                                    Console.WriteLine("Ошибка при добавлении ребра");
                                }
                            }

                            break;
                        }
                    case "4":
                        {
                            Console.WriteLine("Введите название вершины, которую хотите удалить):");
                            string vertex = Console.ReadLine();
                            if (graph.DeleteVertex(vertex))
                            {
                                Console.WriteLine("Вершина {0} удалена", vertex);
                            }
                            else
                            {
                                Console.WriteLine("Ошибка при удалении вершины");
                            }
                            break;
                        }
                    case "5":
                        {
                            Console.WriteLine("Удаление. Введите название первой вершины:");
                            string istok_del = Console.ReadLine();
                            Console.WriteLine("Введите название второй вершины:");
                            string stok_del = Console.ReadLine();

                            if (graph.DeleteEdge(istok_del, stok_del))
                            {
                                Console.WriteLine("Ребро {0} {1} удалено", istok_del, stok_del);
                            }
                            else
                            {
                                Console.WriteLine("Ошибка при удалении ребра");
                            }
                            break;
                        }
                    case "6":
                        {
                            graph.GetEdgesFile("output.txt");
                            Console.WriteLine("Список смежности выведен в файл.");
                            break;
                        }
                    case "7":
                        {
                            Console.Write("Граф: ");
                            Console.Write((graph.focus) ? "Ориентированный, " : "Неориентированный, ");
                            Console.WriteLine((graph.weigh) ? "взвешенный" : "невзвешенный");
                            List<string> k = graph.SpisokSmezh();
                            foreach (var item in k)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case "8":
                        {
                            if (graph.focus)
                            {
                                Console.WriteLine("Введите название вершины: ");
                                string vertexname = Console.ReadLine();
                                if (!graph.ContainsVertex(vertexname))
                                {
                                    Console.WriteLine("Данной вершины не содержится.");
                                }
                                else
                                {
                                    Console.WriteLine("Вершина {0} содержит {1} полустепеней исхода.", vertexname, graph.HalfStepofVertexOut(vertexname));
                                }

                            }
                            else
                            {
                                Console.WriteLine("Операция недопустима для неориентированного графа.");
                            }
                            break;
                        }
                    case "9":
                        {
                            if (graph.focus)
                            {
                                Console.WriteLine("Введите название вершины: ");
                                string vertexname = Console.ReadLine();
                                if (!graph.ContainsVertex(vertexname))
                                {
                                    Console.WriteLine("Данной вершины не содержится.");
                                }
                                else
                                {
                                    Console.Write("Заходящие вершины для {0}: ", vertexname);
                                    List<string> l = graph.InputVertex(vertexname);
                                    if (l.Count != 0)
                                    {
                                        foreach (var item in l)
                                        {
                                            Console.Write("{0} ", item);
                                        }
                                        Console.WriteLine();
                                    }
                                    else
                                    {
                                        Console.WriteLine("заходящих вершин нет.");
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Операция недопустима для неориентированного графа.");
                            }
                            break;
                        }
                    case "10":
                        {
                            Menu(graph.NewGrafDeleteVertex());
                        }
                        break;
                    case "11":
                        {
                            if (!graph.focus)
                            {
                                Console.WriteLine("Граф не ориентированный, невозможно выполнить проверку.");
                            }
                            else
                            {
                                if (graph.Kossaru())
                                {
                                    Console.WriteLine("Сильно связанный.");
                                }
                                else
                                {
                                    Console.WriteLine("Не сильно связанный.");
                                }
                            }
                            break;
                        }
                    case "12":
                        {
                            Console.WriteLine("Введите название вершины u:");
                            string vertexname = Console.ReadLine();
                            if (!graph.ContainsVertex(vertexname))
                            {
                                Console.WriteLine("Данной вершины не содержится.");
                            }
                            else
                            {
                                foreach (var v in graph.ShortestPathToUHeart(vertexname))
                                {
                                    Console.WriteLine(v);
                                }

                            }
                            break;
                        }
                    case "13":
                        {
                            if (!graph.Kossaru())
                            {
                                Console.WriteLine("Сильно не связанный.");
                                break;
                            }
                            
                            if (graph.focus)
                            {
                                Console.WriteLine("Граф должен быть неориентированным.");
                                break;
                            }

                            if (!graph.weigh)
                            {
                                Console.WriteLine("Граф должен быть взвешенным.");
                                break;
                            }

                            List<string> k = graph.Prim();
                            foreach (var item in k)
                            {
                                Console.WriteLine(item);
                            }
                            break;
                        }
                    case "14":
                        {
                            Console.Write("P: ");
                            int p = int.Parse(Console.ReadLine());
                            List<string> list = graph.SumPathMinP(p);
                            foreach(var a in list)
                            {
                                Console.WriteLine(a);
                            }
                            break;
                        }
                    case "15":
                        {
                            if (!graph.weigh)
                            {
                                Console.WriteLine("Граф должен быть взвешенным.");
                                break;
                            }
                            if (!graph.focus)
                            {
                                Console.WriteLine("Граф должен быть ориентированным.");
                                break;
                            }

                            //Dictionary<string, Dictionary<string, int>> c = graph.Floyd();
                            //foreach (var v in c)
                            //{
                            //    foreach (var k in v.Value)
                            //    {
                            //        if (k.Value == int.MaxValue / 2)
                            //        {
                            //            Console.WriteLine("{0} - {1} пути нет.", v.Key, k.Key);
                            //        }
                            //        else
                            //        {
                            //            Console.WriteLine("{0}-{1}  {2}", v.Key, k.Key, k.Value);
                            //        }
                            //    }
                            //}

                            break;
                        }
                    case "16":
                        {
                            if (!graph.weigh)
                            {
                                Console.WriteLine("Граф должен быть взвешенным.");
                                break;
                            }
                            if (!graph.focus)
                            {
                                Console.WriteLine("Граф должен быть ориентированным.");
                                break;
                            }

                            Console.WriteLine("Введите название вершины u:");
                            string u = Console.ReadLine();
                            if (!graph.ContainsVertex(u))
                            {
                                Console.WriteLine("Данной вершины не содержится.");
                                break;
                            }
                            Console.WriteLine("Введите название вершины v1:");
                            string v1 = Console.ReadLine();
                            if (!graph.ContainsVertex(v1))
                            {
                                Console.WriteLine("Данной вершины не содержится.");
                                break;
                            }
                            Console.WriteLine("Введите название вершины v2:");
                            string v2 = Console.ReadLine();
                            if (!graph.ContainsVertex(v2))
                            {
                                Console.WriteLine("Данной вершины не содержится.");
                                break;
                            }
                            List<string> result = graph.FordToFromU(u, v1, v2);
                            foreach(var v in result)
                            {
                                Console.WriteLine(v);
                            }
                            break;
                        }
                    case "17":
                        {
                            if (!graph.weigh)
                            {
                                Console.WriteLine("Граф должен быть взвешенным.");
                                break;
                            }
                            if (!graph.focus)
                            {
                                Console.WriteLine("Граф должен быть ориентированным.");
                                break;
                            }
                            Console.WriteLine(graph.FordFulkerson());

                            break;
                        }
                }
                Console.WriteLine("Выберите новое действие:");
                n = Console.ReadLine();
            }
        }
        static void Main()
        {
            
            Menu(new Graph());

        }
       
    }
}
