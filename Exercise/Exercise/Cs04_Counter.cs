using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs04_Counter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();

            int count = ids.Count;

            if (count == 0)
            {
                MessageBox.Show("無任何元件被選取");
            }
            else
            {
                MessageBox.Show("您共選擇了 " + count.ToString() + " 個元件");
            }
                
            return Result.Succeeded;
        }
    }
}
