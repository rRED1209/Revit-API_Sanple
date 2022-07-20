//For & Foreach 迴圈
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names = { "Jack", "Mary", "John" };
            for (int i = 0; i < names.Length; i++)
            {
                Console.Write(names[i] + " ");
            }
            foreach (string name in names)
            {
                Console.Write(name + " ");
            }
        }
    }
}