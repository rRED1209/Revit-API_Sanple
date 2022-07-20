using System;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows.Media.Imaging; //使用BitmapImage需要加入PresentationCore、WindowsBase、System.Xaml參考

namespace ExtensionExercise
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class Cs03_Extension : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // 尋找檔案在電腦上的路徑
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location; // ExtensionExercise.dll位置
            string picture = assemblyPath + @"\..\..\..\..\picture\";

            // 建立客製化的功能區頁籤
            String tabName = "頁籤名稱";
            application.CreateRibbonTab(tabName);

            // 建立功能區面板
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "面板名稱");

            // 建立HelloWorld連結按鈕
            PushButton pushButton = panel.AddItem(new PushButtonData("HelloWorld", "世界你好", assemblyPath,
                                                        "ExtensionExercise.Cs03_HelloWorld")) as PushButton;
            pushButton.ToolTip = "向世界問好";

            // 設定按鈕上顯示的大圖示
            pushButton.LargeImage = new BitmapImage(new Uri(picture + "global32x32.png"));

            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Cs03_HelloWorld : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            MessageBox.Show("Hello World");
            TaskDialog.Show("Hello World in TaskDialog!", "Which one looks better?\nTaskDialog or MessageBox?");
            return Result.Succeeded;
        }
    }
}
