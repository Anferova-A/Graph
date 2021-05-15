using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                    "2. Заполнить пациентами\n" +
                "3. Показать список смежности\n" +
                "4. Проверка на двудольность\n" +
                "5. Алгоритм Куна\n" +
                "6. Алгоритм Форда-Фалкерсона\n" +
                "7. Алгоритм Хопкрофта – Карпа\n" +
                "8. Сохранить в файл");
            string n = Console.ReadLine();
            while (n != "9")
            {
                switch (n)
                {
                    case "1":
                        graph = new Graph("Input_Graph.txt");
                        break;
                    case "2":
                        graph = GetRandomGraph();
                        break;
                    case "3":
                        Console.Write("Граф: ");
                        Console.Write((graph.focus) ? "Ориентированный, " : "Неориентированный, ");
                        Console.WriteLine((graph.weigh) ? "взвешенный" : "невзвешенный");
                        List<string> k = graph.SpisokSmezh();
                        foreach (var item in k)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case "4":
                        {
                            var parts = graph.CheckBipartition();
                            if (parts != null)
                            {
                                Console.WriteLine("Граф двудольный");
                                foreach (var a in parts)
                                {
                                    Console.WriteLine($"{a.Key} - {a.Value}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Граф не двудольный");
                            }
                            break;
                        }
                    case "5":
                        {
                            var coon = graph.CoonAlgorithm();
                            if (coon != null)
                            {
                                Console.WriteLine($"Максимальное паросочетание: {coon.Count}");
                                foreach (var p in coon)
                                {
                                    Console.WriteLine($"{p.Key} - {p.Value}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Граф не является двудольным. Выполнение алгоритма невозможно");
                            }
                            break;
                        }
                    case "6":
                        {
                            var fulk = graph.FordFulkerson();
                            if (fulk != null)
                            {
                                Console.WriteLine($"Максимальное паросочетание: {fulk.Count}");
                                foreach (var p in fulk)
                                {
                                    Console.WriteLine($"{p.Key} - {p.Value}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Граф не является двудольным. Выполнение алгоритма невозможно");
                            }
                            break;
                        }
                    case "7":
                        {
                            var fulk = graph.HopcroftKarp();
                            if (fulk != null)
                            {
                                Console.WriteLine($"Максимальное паросочетание: {fulk.Count}");
                                foreach (var p in fulk)
                                {
                                    Console.WriteLine($"{p.Key} - {p.Value}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Граф не является двудольным. Выполнение алгоритма невозможно");
                            }
                            break;
                        }
                    case "8":
                        {
                            graph.GetEdgesFile("output.txt");
                            Console.WriteLine("Список смежности выведен в файл.");
                            break;
                        }
                }
                Console.WriteLine("Выберите новое действие:");
                n = Console.ReadLine();
            }
        }

        private static Graph GetRandomGraph()
        {
            var graph = new Graph();
            var surnames = File.ReadAllText("Surnames.txt").Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in surnames)
            {
                graph.AddVertex(item);
            }


            var operatingRooms = new string[400];
            for (int i = 0; i < 400; i++)
            {
                operatingRooms[i] = $"Операционная №{i + 1}";
                graph.AddVertex(operatingRooms[i]);
            }

            var rnd = new Random();
            foreach (var patient in surnames)
            {
                int numOfLinks = rnd.Next(1, 7);
                for (int i = 0; i < numOfLinks; i++)
                {
                    graph.AddEdge(patient, operatingRooms[rnd.Next(0, 400)]);
                }
            }


            return graph;
        }

        static void Main()
        {
            
            Menu(new Graph());

        }
       
    }
}
