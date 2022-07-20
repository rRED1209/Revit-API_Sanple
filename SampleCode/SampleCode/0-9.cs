using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ConsoleApplication1
{
class Program{
  static void Main(string[] args){
    Shape circle = new Circle(1, 2, 3);
    Shape square = new Square(1, 2, 3, 4);
    List<Shape> shapes = new List<Shape>();
    shapes.Add(circle);
    shapes.Add(square);
    foreach(Shape s in shapes) 
      s.draw();
  }
}
abstract class Shape{
  protected int _x;
  protected int _y;
  public abstract void draw();
}

class Circle : Shape{
  private int _r;
  public Circle(int x, int y, int r){
    this._x = x; this._y = y; this._r = r; 
  }
  public override void draw(){
    Console.WriteLine("Draw a Circle at (" + this._x + ", " + this._y + "), radius is " + this._r);
  }
}
class Square : Shape{
  private int _width;
  private int _height;
  public Square(int x, int y, int w, int h) { 
    this._x = x; this._y = y; this._width = w; this._height = h; 
  }
  public override void draw(){
    Console.WriteLine("Draw a Square at (" + this._x + ", " + this._y +"), width is " + this._width + ", height is " + this._height);
  }
}

}
