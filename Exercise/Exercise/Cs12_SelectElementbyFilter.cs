using System.Collections.Generic; // 要使用IList<>等集合需引用此參考
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs12_SelectElementbyFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // 暫存模型文件中所有元件
            FilteredElementCollector collector = new FilteredElementCollector(doc);

            // 設定篩選條件，這邊條件為所有品類為柱的元件
            // ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Columns);

            // WherePasses(filter): 使用 filter 進行篩選動作
            // WhereElementIsNotElementType(): 實際存在模型中的元件
            // ToElements(): 將篩選結果轉換成元件類別存放在IList<Element>容器內
            // IList<Element> columnList = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            collector.OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_Columns);

            string output = "模型中所有柱元件有:\n";
            foreach (Element ele in collector)
            {
                // 輸出柱元件的品類與名稱
                string eleName = ele.Category.Name + ele.Name + "\n";
                output += eleName;
            }

            TaskDialog.Show("Column", output);
            return Result.Succeeded;
        }
    }
}
