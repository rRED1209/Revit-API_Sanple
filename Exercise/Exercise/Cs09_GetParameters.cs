using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Text;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs09_GetParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            StringBuilder st = new StringBuilder();

            if (ids.Count > 1)
            {
                message = "一次只能選擇一個元件!";
                return Result.Failed;
            }

            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                st.AppendLine(elem.Category.Name.ToString() + " " + elem.Name.ToString());
                st.AppendLine("============================");
                foreach (Parameter para in elem.Parameters)
                    st.AppendLine(getParameterInformation(para, doc));
            }

            MessageBox.Show(st.ToString(), "Revit", MessageBoxButtons.OK);
            return Result.Succeeded;
        }

        private string getParameterInformation(Parameter para, Document document)
        {
            string defName = para.Definition.Name;
            switch (para.StorageType)
            {
                case StorageType.Double:
                    return defName + ":" + para.AsValueString() + " (From Double)";

                case StorageType.ElementId:
                    ElementId id = para.AsElementId();
                    if (id.IntegerValue >= 0)
                        return defName + ":" + document.GetElement(id).Name + " (From ElementId)";
                    //在2014以前取得元件的方法為get_Element()，而在2014方法更新為GetElement()
                    else
                        return defName + ":" + id.IntegerValue.ToString() + " (From ElementId)";

                case StorageType.Integer:
                    // if (ParameterType.YesNo == para.Definition.ParameterType) // 在2022以前的做法
                    if (SpecTypeId.Boolean.YesNo == para.Definition.GetDataType()) // 在2023以後的做法
                    {
                        if (para.AsInteger() == 0)
                            return defName + ":" + "False" + " (From Integer - Boolean)";
                        else
                            return defName + ":" + "True" + " (From Integer - Boolean)";
                    }
                    else
                    {
                        return defName + ":" + para.AsInteger().ToString() + " (From Integer)";
                    }

                case StorageType.String:
                    return defName + ":" + para.AsString() + " (From String)";

                default:
                    return "未公開的參數";
            }
        }
    }
}
