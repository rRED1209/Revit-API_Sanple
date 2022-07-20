using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class HelloClass
    {
        public void speak()
        {
            Console.WriteLine("I am a HelloClass!");
        }
        public void getName()
        {
            Console.Write("Please input your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Hi! " + name + ".");
            Console.WriteLine("Your name is {0}, right?", name);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            HelloClass helloClass = new HelloClass();
            helloClass.speak();
            helloClass.getName();
        }
    }
}
