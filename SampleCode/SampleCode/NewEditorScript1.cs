using System.Windows.Forms;
using System.Drawing;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Text; // 使用StringBuilder需要引用此參考
namespace SampleCode
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class test : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;

            string categoryString = Microsoft.VisualBasic.Interaction.InputBox("請輸入流水號前面部分","編寫流水號", "", -1, -1); //跳出視窗輸入流水號前面部分
            //string categoryString = "SH2A"

            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                if (elem is FamilyInstance)
                {
                   FamilyInstance f = elem as FamilyInstance;
                   CreateIndependentTag(doc, f, categoryString);
                }
            }
           
            return Result.Succeeded;

        }

        private void CreateIndependentTag( Autodesk.Revit.DB.Document document, FamilyInstance instance, string categoryString)
        {
           
            // make sure active view is not a 3D view
            Autodesk.Revit.DB.View view = document.ActiveView;
            LocationPoint location = instance.Location as LocationPoint;



            Parameter number = instance.LookupParameter("標註"); // **儲存數字參數的名稱
            string numString = number.AsString();
            string serialNum = categoryString + "-" + numString;
            XYZ place = location.Point + new XYZ(1, 1, 0); // **標籤相對元件的位置
            double width = 0.02; // **標籤的寬度
            TextNoteOptions options = new TextNoteOptions();
            options.HorizontalAlignment = HorizontalTextAlignment.Center;
            options.TypeId = document.GetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
           



            Transaction trans = new Transaction(document);
            trans.Start("插入標籤");
            TextNote txNote = TextNote.Create(document, view.Id, place, width, serialNum, options); //插入標籤
            trans.Commit();

           
        }


       



    }
}