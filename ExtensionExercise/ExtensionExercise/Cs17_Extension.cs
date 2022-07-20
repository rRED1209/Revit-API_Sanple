using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;


namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs17_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
             ICollection<ElementId> selElements = uidoc.Selection.GetElementIds();

            Transaction trans = new Transaction(doc);
            trans.Start("交易開始");

            foreach (ElementId elemId in selElements)
            {
                Element elem = doc.GetElement(elemId);
		FamilyInstance column = elem as FamilyInstance;
                if (column != null)
                    locationMove(column);
            }

            trans.Commit();
            return Result.Succeeded;
        }

        private void locationMove(FamilyInstance column)
        {
            LocationPoint columnPoint = column.Location as LocationPoint;
            if (columnPoint != null)
            {
                XYZ newLocation = new XYZ(10, 20, 0);

                // 將柱移動至新的位置
                columnPoint.Point = newLocation;
                TaskDialog.Show("Revit", "移動成功");
            }
        }
    }
}
