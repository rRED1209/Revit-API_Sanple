using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Text;
using System.Collections.Generic; // 使用Dictionary需要引用此參考

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs06_CategorizedCounter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            StringBuilder st = new StringBuilder();
            Dictionary<string, int> categories = new Dictionary<string, int>();

            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                Category category = elem.Category;
                if (categories.ContainsKey(category.Name))
                    categories[category.Name]++;
                else
                    categories.Add(category.Name, 1);
            }

            foreach (string key in categories.Keys)
                st.AppendLine(key + ": " + categories[key].ToString());

            st.AppendLine("共有 " + categories.Count.ToString() + " 種品類被選取");

            MessageBox.Show(st.ToString());
            return Result.Succeeded;
        }
    }
}
