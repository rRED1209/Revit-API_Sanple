using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace SampleCode
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs23_Paint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            Transaction trans = new Transaction(doc);
            trans.Start("自動貼材質");
            // 先自整個模型當中篩選出所有屬於「材料」的元件，並且從其中找到磚石材料
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Materials);
            IList<Element> matList = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

            int materialID = 0; // 用一個整數變數記錄「磚石」材質的編號以供稍後使用
            foreach(Element mat in matList)
            {
                if (mat.Name == "磚石")
                    materialID = mat.Id.IntegerValue;
            }

            // 針對使用者選取的所有元件貼上磚石材質
            foreach(ElementId elementId in ids)
            {
                Element element = doc.GetElement(elementId);
                ElementId matID = new ElementId(materialID);
                PaintElementFace(element, matID); // 貼材質的流程細節以額外的函式處理
            }
            trans.Commit();

            return Result.Succeeded;
        }

        // 能將一個元件的所有表面都貼上材質的函式
        private void PaintElementFace(Element element, ElementId matID)
        {
            Document doc = element.Document;
            GeometryElement geometryElement = element.get_Geometry(new Options());
            // GeometryElement集合物件存放一個建築元件的所有幾何資訊
            // 它們會以「量體(Solid)」及「表面(Face)」的形式藏在GeometryObject之中
            foreach (GeometryObject geometryObject in geometryElement)
            {
                if(geometryObject is Solid)
                {
                    Solid solid = (Solid)geometryObject;
                    foreach (Face face in solid.Faces)
                    {
                        if (doc.IsPainted(element.Id, face) == false) 
                            doc.Paint(element.Id, face, matID); 
                    }
                }
            }

        }
    }
}
