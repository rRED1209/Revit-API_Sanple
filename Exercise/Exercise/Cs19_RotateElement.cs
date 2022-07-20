using System; // 要使用Math等函數需要引用此參考
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs19_RotateElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            // Transaction開始
            Transaction trans = new Transaction(doc);
            trans.Start("旋轉元件");

            // 旋轉被選擇的每個元件
            foreach (ElementId elementId in ids)
            {
                Element element = doc.GetElement(elementId);
                rotateZ(doc, element);
            }

            // Transaction結束，確認動作
            trans.Commit();

            return Result.Succeeded;
        }

        // 旋轉元件的Function
        private void rotateZ(Document doc, Element element)
        {
            // 設定兩點
            XYZ axisPoint1 = new XYZ(0, 0, 0);
            XYZ axisPoint2 = new XYZ(0, 0, 10);

            // 由此兩點定出一線，此線為旋轉中心
            Line axis = Line.CreateBound(axisPoint1, axisPoint2);
            // 在2014以前建立Line的方法為Create.NewLineBound()，而在2014方法更新為Line.CreateBound()

            // 旋轉
            // ElementTransformUtils.RotateElement(doc, element.Id, axis, Math.PI / 4.0);
            element.Location.Rotate(axis, Math.PI / 4.0);
        }
    }
}
