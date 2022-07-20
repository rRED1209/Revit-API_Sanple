using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Forms;
using System.Collections.Generic;


namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs09_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document document = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(document);
            ICollection<ElementId> collection = uidoc.Selection.GetElementIds();

            Parameter foundParameter = null;
            foreach (ElementId elemId in collection)
            {
                Element elem = document.GetElement(elemId);
                if (elem is Wall)
                {
                    Wall elemIsWall = elem as Wall;
                    foundParameter = findWithBuiltinParameterID(elemIsWall);
                    break;
                }
                else
                {
                    MessageBox.Show("請選擇牆元件，重新執行此程式", "Revit", MessageBoxButtons.OK);
                }
            }

            String prompt = foundParameter.Definition.Name + " = " + foundParameter.AsValueString();
            MessageBox.Show("找到程式碼中指定的參數：" + prompt, "Revit", MessageBoxButtons.OK);

            return Result.Succeeded;
        }

        private Parameter findWithBuiltinParameterID(Wall wall)
        {
            // 使用WALL_BASE_OFFSET 參數ID，以取得牆的"基準偏移"參數
            BuiltInParameter paraIndex = BuiltInParameter.WALL_BASE_OFFSET;
            Parameter parameter = wall.get_Parameter(paraIndex);
            return parameter;
        }
    }
}
