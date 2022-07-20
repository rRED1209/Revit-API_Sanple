using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs03_HelloWorld : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //MessageBox.Show("Hello World");
            TaskDialog.Show("Hello World in TaskDialog!",
                "Which one looks better?\nTaskDialog or MessageBox?");
            return Result.Succeeded;
        }
    }
}