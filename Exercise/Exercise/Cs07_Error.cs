using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs07_Error : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            message = "錯誤！請檢查亮顯的柱！";
            //ElementSet elemset = commandData.Application.ActiveUIDocument.Selection.Elements;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;
            
            foreach (ElementId elemId in ids)
            {
                // Wall wall = elem as Wall;
                // if (wall != null) elements.Insert(elem);
               

                Element elem = doc.GetElement(elemId);
                if (elem.Category.Name == "柱")
                    elements.Insert(elem);

                //if ((BuiltInCategory)elem.Category.Id.IntegerValue == BuiltInCategory.OST_Columns)
                //   elements.Insert(elem);
            }
            return Result.Failed;
        }
    }
}
