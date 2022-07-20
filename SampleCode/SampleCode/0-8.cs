//ArrayList
using System;
using System.Collections;
namespace ConsoleApplication1
{
class Program
{
static void Main(string[] args)
{
ArrayList list = new ArrayList(); // 使用ArrayList需引用System.Collections參考
list.Add(4);
list.Add(5);
list.Add(6);
ArrayList list2 = new ArrayList();
list2.Add(1);
list2.Add(2);
list2.Add(3);
list.AddRange(list2);
foreach (int i in list)
{
Console.Write(i + " ");
}
Console.WriteLine();
list.RemoveAt(1);
list.Insert(0, 4);
list.RemoveRange(2, 2);
list.Sort();
list.Reverse();
foreach (int i in list)
{
Console.Write(i + " ");
}
Console.WriteLine();
Console.WriteLine(list.Count);
list.Clear();
Console.WriteLine(list.Count);
}
}
}