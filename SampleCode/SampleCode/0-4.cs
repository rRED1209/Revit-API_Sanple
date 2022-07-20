//While&Do 迴圈
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
int c = 0;
while (c < 5)
{
Console.Write(c);
c++;
}
do
{
Console.Write(c);
c++;
} while (c < 10);
}
}
}