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
                    "1. Считать граф из файла\n");

            string n = Console.ReadLine();
            while (n != "2")
            {
                switch (n)
                {
                    case "1":
                        graph = new Graph("Input_Graph.txt");
                        break;
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
