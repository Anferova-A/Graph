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
                "2. Показать список смежности\n" +
                "3. Проверка на двудольность\n" +
                "4. Алгоритм Куна\n" +
                "5. Алгоритм Форда-Фалкерсона\n");
            string n = Console.ReadLine();
            while (n != "6")
            {
                switch (n)
                {
                    case "1":
                        graph = new Graph("Input_Graph.txt");
                        break;
                    case "2":
                        Console.Write("Граф: ");
                        Console.Write((graph.focus) ? "Ориентированный, " : "Неориентированный, ");
                        Console.WriteLine((graph.weigh) ? "взвешенный" : "невзвешенный");
                        List<string> k = graph.SpisokSmezh();
                        foreach (var item in k)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    case "3":
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
                    case "4":
                        {
                            var coon = graph.CoonAlgorithm();
                            if (coon != null)
                            {
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
                    case "5":
                        {
                            var fulk = graph.FordFulkerson();
                            if (fulk != null)
                            {
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
