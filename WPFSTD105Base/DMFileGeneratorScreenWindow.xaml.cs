using DevExpress.Data.Extensions;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFSTD105
{
    /// <summary>
    /// InsertPartsWin.xaml 的互動邏輯
    /// </summary>
    public partial class DMFileGeneratorScreenWindow: WPFWindowsBase.BasePage
    {
        private List<string> _materialListGridControl { get; }

        public DMFileGeneratorScreenWindow(List<string> MaterialList)
        {
            InitializeComponent();
           // _materialListGridControl = MaterialList;
        }


        public void DMFileGenerate(List<string> MaterialNumbrLsit)
        {
            /*
            (this.DataContext as DXSplashScreenViewModel).Status = "123";


            Task.Run(() =>
            {
                STDSerialization _Ser = new STDSerialization();
                model.Dispatcher.Invoke(new Action(() =>
                {
                    model.DataContext = new ObSettingVM();
                    model.Unlock("UF20-HM12N-F7K3M-MCRA-FDGT");
                    model.ActionMode = devDept.Eyeshot.actionType.None;
                    model.Clear();
                    MaterialNumbrLsit.ForEach(MNumber =>
                    {
                        (this.DataContext as DXSplashScreenViewModel).Status = $"正在產生檔案{MNumber}";
                        try
                        {
                            if (!System.IO.File.Exists($@"{ApplicationVM.DirectoryMaterial()}\{MNumber}.dm"))
                            {
                                int RetryCount = 0;
                                while (RetryCount < 10)
                                {
                                    try
                                    {

                                        model.AssemblyPart(MNumber);
                                        _Ser.SetMaterialModel(MNumber, model);//儲存 3d 視圖
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        RetryCount++;
                                    }
                                }
                            }
                            model.Clear();
                        }
                        catch (Exception ex)
                        {

                        }
                    });

                    //把自己關掉
                    (this.DataContext as DXSplashScreenViewModel).Status = "完成 正在關閉視窗";
                    System.Threading.Thread.Sleep(5000);
                    this.Close();
                }));
            });

            */
        }



        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ExitWin();
        }

        private void Check_Button_Click(object sender, RoutedEventArgs e)
        {



            ExitWin();
        }

        private void ExitWin()
        {
            //關閉視窗
            Window.GetWindow(this).Close();
        }




    }
}
