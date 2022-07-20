using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;

namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs13_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // 暫存專案中所有的元件
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // 設定篩選條件，這邊條件為所有品類為牆的元件
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Walls);

            collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            // 使用Linq查詢來尋找類型名稱為"磚牆 90mm"的牆元件
            var query = from element in collector
                        where element.Name == "磚牆 90mm"
                        select element;

            // 拋出找到的元件到牆元件集合中
            List<Wall> walls = query.Cast<Wall>().ToList<Wall>();
            string prompt = "透過LINQ查詢找到的牆元件共有: " + walls.Count + " 個，如下:";
            foreach (Wall wall in walls)
            {
                prompt += "\n品類：" + wall.Category.Name + "，類型：" + wall.WallType.Name;
            }
            TaskDialog.Show("Revit", prompt);
            return Result.Succeeded;
        }
    }
}
