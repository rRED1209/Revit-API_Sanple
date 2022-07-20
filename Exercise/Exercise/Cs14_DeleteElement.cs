using System; // 要使用Exception需引用此參考
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs14_DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            // Autodesk.Revit.UI.Selection.SelElementSet selElements = uidoc.Selection.Elements;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Windows);

            //ICollection<ElementId> ids = new List<ElementId>();
  
            //foreach (Element elem in collector)
            //    ids.Add(elem.Id);
            ICollection<ElementId> ids = collector.ToElementIds();

            // 開始一個transaction，每個改變模型的動作都需在transaction中進行
            Transaction trans = new Transaction(doc);
            trans.Start("刪除元件");

            // 刪除選取的元件
            // foreach (Element elem in collector)
            //     deleteElement(doc, elem);
            ICollection<ElementId> deletedIdSet = doc.Delete(ids);

            trans.Commit();

            if (deletedIdSet.Count == 0)
            {
                throw new Exception("窗元件刪除失敗");
            }

            return Result.Succeeded;
        }

        // 刪除元件的Function
        /* private void deleteElement(Autodesk.Revit.DB.Document document, Element element)
           {
               // 在 2012 以前，習慣使用 Delete(Element) 方法來刪除元件，
               // 而在 2014 習慣透過元件ID來刪除元件，所以請使用 Delete(ElementId) 方法來替代

               // 將指定元件以及所有與該元件相關聯的元件刪除，並將刪除後所有的元件存到到容器中
               ICollection<Autodesk.Revit.DB.ElementId> deletedIdSet = document.Delete(element.Id);

               // 可利用上述容器來查看刪除的數量，若數量為0，則刪除失敗，提供錯誤訊息
               if (deletedIdSet.Count == 0)
               {
                   throw new Exception("選取的元件刪除失敗");
               }
           } */


        private void setMark(Element elem, string s)
        {
            elem.LookupParameter("聯絡人").SetValueString("my");
        }
    }
}
