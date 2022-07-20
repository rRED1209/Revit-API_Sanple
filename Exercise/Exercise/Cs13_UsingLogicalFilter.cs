using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs13_UsingLogicalFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            ICollection<ElementId> filtered_ids = new List<ElementId>();
            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                filtered_ids.Add(elem.Id);
            }

            FilteredElementCollector coll = new FilteredElementCollector(doc, ids);

            // 篩選條件為窗和柱的篩選器
            ElementCategoryFilter filter1 = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            ElementCategoryFilter filter2 = new ElementCategoryFilter(BuiltInCategory.OST_Columns);

            // 邏輯篩選器，用以組合兩個或多個篩選器做使用
            LogicalOrFilter orfilter = new LogicalOrFilter(filter1, filter2);

            IList<Element> elemList = coll.WherePasses(orfilter).WhereElementIsNotElementType().ToElements();

            // 計算元件數量
            int counter = elemList.Count;

            // 輸出元件的品類和名稱
            string output = null;
            foreach (Element elem in elemList)
            {
                output += elem.Category.Name + ", " + elem.Name + "\n";
            }

            // 展示資訊並回傳成功
            MessageBox.Show("符合篩選條件的元件共有 " + counter + " 個，有:\n" + output);
            return Result.Succeeded;
        }
    }
}
