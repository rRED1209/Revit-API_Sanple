using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;

namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs18_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ICollection<ElementId> selElements = uidoc.Selection.GetElementIds();

            // Transaction狀態開始
            Transaction trans = new Transaction(doc);
            trans.Start("交易開始");

            // 移動所有被選擇的牆元件
            foreach (ElementId elemId in selElements)
            {
                Element elem = doc.GetElement(elemId);
                Wall wall = elem as Wall;
                if (wall != null)
                    moveUsingCurveParam(doc, wall);
            }

            // Transaction狀態結束，確認移動
            trans.Commit();
            return Result.Succeeded;
        }

        private void moveUsingCurveParam(Document document, Wall wall)
        {
            LocationCurve wallLine = wall.Location as LocationCurve;
            XYZ p1 = XYZ.Zero;
            XYZ p2 = new XYZ(10, 20, 0);
            Line newWallLine = Line.CreateBound(p1, p2);

            // 更改牆的定位線為新建立的定位線
            wallLine.Curve = newWallLine;
        }
    }
}
