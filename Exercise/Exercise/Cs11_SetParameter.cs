using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs11_SetParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            Transaction trans = new Transaction(doc);
            trans.Start("設定參數");

            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                if (elem is Wall)
                {
                    setMark(elem, "1m"); // 或 1 m 1000mm 1000 mm 1000 皆可

                    // 另外也可參考方法二：直接利用BuiltInParameter所提供的參數，直接抓取我們想要改變的參數。如下：
                    // BuiltInParameter paraIndex = BuiltInParameter.WALL_TOP_OFFSET;
                    // Parameter para = elem.get_Parameter(paraIndex);
                    // para.SetValueString("1m");
                }
            }

            trans.Commit();
            MessageBox.Show("選擇的元件已被標註");
            return Result.Succeeded;
        }

        private void setMark(Element elem, string s)
        {
            elem.LookupParameter("頂部偏移").SetValueString(s);
        }
    }
}
