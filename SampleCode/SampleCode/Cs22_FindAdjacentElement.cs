using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace SampleCode
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs22_FindAdjacentElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            // 針對使用者已經選取的每一個物件，列出它的相鄰元件
            foreach(ElementId elementId in ids)
            {
                Element element = doc.GetElement(elementId);
                BoundingBoxXYZ bbx = element.get_BoundingBox(null);
                Outline outline = new Outline(bbx.Min, bbx.Max); // 取得定界框的對角線
                BoundingBoxIntersectsFilter filterIntersection = new BoundingBoxIntersectsFilter(outline);
                // 這個篩選器會取得「所有和這個定界框相交的所有定界框，並傳回其所代表之元件」
                IList<Element> intersects = new FilteredElementCollector(doc).WherePasses(filterIntersection)
                    .WhereElementIsNotElementType().ToElements();

                // 使用一個字串將結果印出來
                StringBuilder sb = new StringBuilder("這個元件是" + element.Category.Name+"\t id為：" 
                    + element.Id + "\t 名稱為：" + element.Name + "\n");
                sb.AppendLine("和它相鄰的元件有：");
                foreach(Element elemFiltered in intersects)
                {
                    // 列印相鄰元件時需作例外處理，因為有些元件不具備族群名稱，會發生空參考錯誤
                    // 但這些會發生例外的元件也不是我們的目標，所以可以放心地將它們排除
                    try {
                        sb.AppendLine(elemFiltered.Category.Name + "\t id為："
                            + elemFiltered.Id + "\t 名稱為：" + elemFiltered.Name);
                    } catch { }
                }
                MessageBox.Show(sb.ToString()); // 將這個元件的所有相鄰元件列印出來
            }
            return Result.Succeeded;
        }
    }
}
