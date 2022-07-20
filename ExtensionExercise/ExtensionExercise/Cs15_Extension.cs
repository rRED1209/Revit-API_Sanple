using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Collections.Generic;


namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs15_Externsion : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            ICollection<ElementId> selElements = uidoc.Selection.GetElementIds();

            foreach (ElementId elemId in selElements)
            {
                Element elem = doc.GetElement(elemId);
                Wall wall = elem as Wall;
                if (wall != null)
                    storeDataInWall(wall, new XYZ(10, 10, 5));
            }

            return Result.Succeeded;
        }

        // 創建資料結構，並連接到牆，最後從牆去檢索創建的資料
        private void storeDataInWall(Wall wall, XYZ dataToStore)
        {
            Transaction createSchemaAndStoreData = new Transaction(wall.Document);
            createSchemaAndStoreData.Start("建立架構並儲存資料");
            SchemaBuilder schemaBuilder = new SchemaBuilder(new Guid("720080CB-DA99-40DC-9415-E53F280AA1F0"));
            schemaBuilder.SetReadAccessLevel(AccessLevel.Public); // 允許任何人讀取該物件
            schemaBuilder.SetWriteAccessLevel(AccessLevel.Vendor); // 限制只有供應商能寫入
            schemaBuilder.SetVendorId("ADSK"); // 因為限制寫入存取需要供應商ID
            schemaBuilder.SetSchemaName("電源插頭位置");

            // 建立儲存位置的欄位
            FieldBuilder fieldBuilder = schemaBuilder.AddSimpleField("電源插頭位置", typeof(XYZ));
            // fieldBuilder.SetUnitType(UnitType.UT_Length); // 在2021以前的做法
            fieldBuilder.SetSpec(SpecTypeId.Length); // 在2022以後的做法
            fieldBuilder.SetDocumentation("儲存電源插頭在牆中的位置值");

            Schema schema = schemaBuilder.Finish(); // 註冊架構物件
            Entity entity = new Entity(schema); // 為此架構(類別)建立實體(物件)

            // 從架構取得欄位
            Field fieldSpliceLocation = schema.GetField("電源插頭位置");

            // 為此實體設定值
            // entity.Set<XYZ>(fieldSpliceLocation, dataToStore, DisplayUnitType.DUT_METERS); // 在2021以前的做法
            entity.Set<XYZ>(fieldSpliceLocation, dataToStore, UnitTypeId.Meters); // 在2022以後的做法
            wall.SetEntity(entity); // 在元件中儲存實體

            // 從牆中取得資料
            Entity retrievedEntity = wall.GetEntity(schema);
            // XYZ retrievedData = retrievedEntity.Get<XYZ>(schema.GetField("電源插頭位置"), DisplayUnitType.DUT_METERS); // 在2021以前的做法
            XYZ retrievedData = retrievedEntity.Get<XYZ>(schema.GetField("電源插頭位置"), UnitTypeId.Meters); // 在2022以後的做法

            createSchemaAndStoreData.Commit();
            TaskDialog.Show("Revit", "架構名稱: " + schema.SchemaName + "\nGUID: " + schema.GUID +
                                     "\n供應商: " + schema.VendorId + "\n插頭位置: (" + retrievedData.ToString());
        }
    }
}
