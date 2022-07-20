using System;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs20_RotateElementBySelf : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            // 宣告一個checker來確認是否成功
            bool checker = false;

            Transaction trans = new Transaction(doc);
            trans.Start("旋轉元件");

            // 旋轉被選取的每個元件
            foreach (ElementId elementId in ids)
            {
                Element element = doc.GetElement(elementId);
                if (element is FamilyInstance)
                {
                    if (element.Category.Name == "柱")
                    {
                        checker = rotateZ_BySelf(doc, element);
                    }
                }
            }

            trans.Commit();

            // 若每個元件都被旋轉，則回傳成功
            if (checker == true)
                return Result.Succeeded;
            // 若失敗，則回傳失敗訊息
            else
            {
                message = "旋轉元件失敗";
                return Result.Failed;
            }
        }

        // 對自身旋轉的Function
        private bool rotateZ_BySelf(Document doc, Element element)
        {
            // 抓取自身的定位線
            // LocationCurve locationCurve = element.Location as LocationCurve;
            LocationPoint locationPoint = element.Location as LocationPoint;
            bool successful = false;

            if (locationPoint != null)
            {
                // 抓取定位線的起始點，並對起始點設一個平行Z軸的線
                // Curve curve = locationCurve.Curve;
                // XYZ axisPoint1 = curve.GetEndPoint(0);
                XYZ axisPoint1 = locationPoint.Point;
                XYZ axisPoint2 = new XYZ(axisPoint1.X, axisPoint1.Y, axisPoint1.Z + 10);
                Line axis = Line.CreateBound(axisPoint1, axisPoint2);

                // 以此線為中心旋轉，並回傳訊息
                // successful = locationCurve.Rotate(axis, Math.PI / 4.0);
                ElementTransformUtils.RotateElement(doc, element.Id, axis, Math.PI / 4.0);
                successful = true;
            }
            // 回傳訊息
            return successful;
        }
    }
}
