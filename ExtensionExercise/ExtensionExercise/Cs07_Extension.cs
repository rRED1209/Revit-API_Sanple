using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs07_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            message = "請注意有特別突出顯示的牆。";
            FilteredElementCollector collector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document);
            System.Collections.Generic.ICollection<Element> collection = collector.OfClass(typeof(Wall)).ToElements();
            foreach (Element e in collection)
            {
                elements.Insert(e);
            }
            return Result.Failed;
        }
    }
}