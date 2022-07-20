using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs05_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(document);
            Selection choices = uidoc.Selection;

            // 在Revit中選擇一個物件
            Reference hasPickOne = choices.PickObject(ObjectType.Element, "請選擇一個元件");
            ICollection<ElementId> FirstPickSet=choices.GetElementIds();
            FirstPickSet.Add(hasPickOne.ElementId);
            if (hasPickOne != null)
            {
                TaskDialog.Show("Revit", "選擇 " + FirstPickSet.Count + " 個元件並加入到Selection中");
            }

            // 在Revit中選擇多個物件
            IList<Element> hasPickSome = choices.PickElementsByRectangle("請框選一些元件");
            ICollection<ElementId> SecondPickSet = choices.GetElementIds();
            foreach (Element elem in hasPickSome)
            {
                SecondPickSet.Add(elem.Id);
            }
            if (hasPickSome.Count > 0)
            {
                TaskDialog.Show("Revit", "選擇 " + SecondPickSet.Count + " 個元件並加入Selection中");
            }

            return Result.Succeeded;
        }
    }
}
