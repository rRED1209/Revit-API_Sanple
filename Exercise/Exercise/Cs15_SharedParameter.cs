using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;

namespace Exercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs15_SharedParameter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Transaction trans = new Transaction(doc);
            trans.Start("綁定實作元件");

            // 首先必須建立用來存放客製化參數定義的共用參數文檔，你可以設定路徑及檔案名稱，
            // 如果沒有設路徑的話，預設會建立在 System.Environment.CurrentDirectory 的位置；
            // 如果是直接開啟Revit專案檔，則路徑就是Revit專案檔所在目錄下；
            // 如果是先開啟Revit程式，再選擇過去開啟過的檔案，則路徑就是Revit執行檔的位置(C:\Program Files\Autodesk\Revit 2023)
            // 如果是直接從開始列表，直接選擇過去開啟過的檔案，則路徑就是(C:\Windows\System32)
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            System.IO.FileStream fileStream = System.IO.File.Create(path + "\\SharedParamFile.txt");
            fileStream.Close();
            
            // 設定共用參數文檔的路徑
            doc.Application.SharedParametersFilename = path + "\\SharedParamFile.txt";

            // 開啟共用參數文檔
            DefinitionFile definFile = doc.Application.OpenSharedParameterFile();

            // 定義參數並綁定到牆與柱實作元件中
            bool result = setNewParameterToInstance(uidoc.Application, definFile);

            trans.Commit();
            if (result == true)
                return Result.Succeeded;
            else
            {
                message = "客製化的參數綁定元件失敗";
                return Result.Failed;
            }
        }

        // 定義參數與綁定實作元件的Function
        private bool setNewParameterToInstance(UIApplication app, DefinitionFile myDefinitionFile)
        {
            // 共用參數文檔中包含了Group，代表參數所屬的群組，
            // 而Group內又包含了Definition，也就是你想增加的參數，
            // 欲新增Definition首先必須新增一個Group
            DefinitionGroups myGroups = myDefinitionFile.Groups;
            DefinitionGroup myGroup = myGroups.Create("參數群組");

            // 創建Definition的定義以及其儲存類型
            Autodesk.Revit.DB.ExternalDefinitionCreationOptions options 
                // = new Autodesk.Revit.DB.ExternalDefinitionCreationOptions("實作元件建立日期", ParameterType.Text); // 在2022以前的做法
                = new Autodesk.Revit.DB.ExternalDefinitionCreationOptions("實作元件建立日期", SpecTypeId.String.Text); // 在2023以後的做法
            Definition myDefinition = myGroup.Definitions.Create(options);

            // 創建品類集並插入牆及柱品類到裡面
            CategorySet myCategories = app.Application.Create.NewCategorySet();

            // 使用BuiltInCategory來取得牆及柱品類
            Category categoryWall = app.ActiveUIDocument.Document.Settings.Categories.get_Item(BuiltInCategory.OST_Walls);
            myCategories.Insert(categoryWall);
            Category categoryColumn = app.ActiveUIDocument.Document.Settings.Categories.get_Item(BuiltInCategory.OST_Columns);
            myCategories.Insert(categoryColumn);

            // 根據品類創建類型綁定的物件
            //TypeBinding typeBinding = app.Application.Create.NewTypeBinding(myCategories);
            InstanceBinding instanceBinding = app.Application.Create.NewInstanceBinding(myCategories);

            // 取得當前文件中所有的類型綁定
            BindingMap bindingMap = app.ActiveUIDocument.Document.ParameterBindings;

            // 將客製化的參數定義綁定到文件中
            bool instanceBindOK = bindingMap.Insert(myDefinition, instanceBinding, BuiltInParameterGroup.PG_TEXT);

            return instanceBindOK;
        }
    }
}
