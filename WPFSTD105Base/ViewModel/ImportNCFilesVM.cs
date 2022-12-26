using devDept.Eyeshot;
using devDept.Eyeshot.Entities;
using DevExpress.Data.Extensions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using GD_STD;
using GD_STD.Data;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFSTD105.Attribute;
using WPFSTD105.Model;
using WPFSTD105.Tekla;
using WPFSTD105.ViewModel;
using static WPFSTD105.ViewLocator;

namespace WPFSTD105
{
    /// <summary>
    /// 匯入檔案VM
    /// </summary>
    public class ImportNCFilesVM : WPFWindowsBase.BaseViewModel, IProjectProperty
    {
        /// <summary>
        /// 畫面管理器(進度條型)
        /// </summary>
        private  SplashScreenManager ProcessingScreenWin = SplashScreenManager.Create(() => new ProcessingScreenWindow(), new DXSplashScreenViewModel { });

        /// <summary>
        /// 畫面管理器(轉圈型)
        /// </summary>
        //public SplashScreenManager ScreenManagerWaitIndicator { get; set; } = SplashScreenManager.CreateWaitIndicator();
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ImportNCFilesVM()
        {
            STDSerialization ser = new STDSerialization();
            ProjectProperty project = ser.GetProjectProperty();
            WriteProjectProperty(project);
        }
        /// <summary>
        /// 寫入專案參數
        /// </summary>
        /// <param name="project"></param>
        private void WriteProjectProperty(ProjectProperty project)
        {
            BomProperties = project.BomProperties;
            Number = project.Number;
            Create = project.Create;
            Name = project.Name;
            Design = project.Design;
            Location = project.Location;
            NcLoad = project.NcLoad;
            Revise = project.Revise;
            IsNcLoad = project.IsNcLoad;
            IsBomLoad = project.IsBomLoad;
            BomLoad = project.BomLoad;
            NcLoadArray = new bool[] { !IsNcLoad, IsNcLoad };
            BomLoadArray = new bool[] { !IsBomLoad, IsBomLoad };
        }

        /// <inheritdoc/>
        public DateTime Create { get; set; }
        /// <inheritdoc/>
        public string Design { get; set; }
        /// <inheritdoc/>
        public bool IsNcLoad { get; set; }
        /// <inheritdoc/>
        public bool IsBomLoad { get; set; }
        /// <inheritdoc/>
        public string Location { get; set; }
        /// <inheritdoc/>
        public string Name { get; set; }
        /// <inheritdoc/>
        public string Number { get; set; }
        /// <inheritdoc/>
        public DateTime NcLoad { get; set; }
        /// <inheritdoc/>
        public DateTime BomLoad { get; set; }
        /// <inheritdoc/>
        public DateTime Revise { get; set; }
        /// <summary>
        /// 是否有載入過 nc 檔的選項
        /// </summary>
        public bool[] NcLoadArray { get; set; }
        /// <summary>
        /// 是否有載入過 Bom 檔的選項
        /// </summary>
        public bool[] BomLoadArray { get; set; }
        /// <summary>
        /// nc 資料路徑
        /// </summary>
        public string NcPath { get; set; } = string.Empty;
        /// <summary>
        /// bom 文件路徑
        /// </summary>
        public string BomPath { get; set; } = string.Empty;
        /// <inheritdoc/>
        public ObservableCollection<BomProperty> BomProperties { get; set; }
        /// <summary>
        /// 匯入 nc 命令
        /// </summary>
        public ICommand ImportNcCommand { get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    FolderBrowserDialogService service = DevExpand.NewFolder("請選擇NC放置的資料夾");
                    IFolderBrowserDialogService folder = service;
                    folder.ShowDialog();//Show 視窗
                    NcPath = folder.ResultPath;//選擇的路徑
                });
            }
        }
        /// <summary>
        /// 匯入報表命令
        /// </summary>
        public ICommand ImportBomCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    DXOpenFileDialog dX = new DXOpenFileDialog();
                    dX.Filter = "Csv 檔案 |*.csv";
                    try 
                    { 
                    dX.ShowDialog();//Show 視窗
                    BomPath = dX.FileName;
                    }
                    catch(Exception ex)
                    { 
                    }
                });
            }
        }
        /// <summary>
        /// 關閉命令
        /// </summary>
        public ICommand ClosedCommand { get; set; }
        /// <summary>
        /// 匯入報表命令
        /// </summary>
        public ICommand ImportCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    //ProcessingScreenWin.ViewModel.Status = "";
                    // ProcessingScreenWin.ViewModel.IsIndeterminate = true;


                    //測試用 顯示1->100
                    /*ProcessingScreenWin.ViewModel.IsIndeterminate = false;
                    ProcessingScreenWin.ViewModel.Status = $"Loading data...";
                    for (double i = 0.01; i < 100; i+=0.01)
                    {
                        ProcessingScreenWin.ViewModel.Progress = i;
                        Thread.Sleep(1);
                    }*/
                    ProcessingScreenWin.ViewModel.IsIndeterminate = true;


                    //ProcessingScreenW.Show();
                //if (IsNcLoad || IsBomLoad) //如果有載入過報表
                if (false) //如果有載入過報表
                {
                        ProcessingScreenWin.Close();
                        // 2022/08/22 呂宗霖 因螺栓無法找到其歸屬零件編號，故架構師與副總討論後，決議先讓使用者只能匯入一次檔案，若要再次匯入，必須重新新增專案
                        WinUIMessageBox.Show(null,
                    $"已匯入專案，請重新建立專案",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
                        return;
                    //MessageBoxResult saveAsResult = MessageBox.Show($"請問是否要備份之前載入的檔案", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    //if (saveAsResult == MessageBoxResult.Yes) //如果要備份檔案
                    //{
                    //    FolderBrowserDialogService service = DevExpand.NewFolder("請選擇另存路徑"); //文件夾瀏覽器對話服務
                    //    IFolderBrowserDialogService folder = service;
                    //    folder.ShowDialog();//Show 視窗
                    //    string path = folder.ResultPath;//用戶選擇的路徑
                    //    if (path != string.Empty) //如果有選擇路徑
                    //    {
                    //        string dataName = $"{CommonViewModel.ProjectName}{DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.zip";//壓縮檔名稱
                    //        string zipPath = $@"{path}\{dataName}";//壓縮檔完整路徑
                    //        if (File.Exists(zipPath))//如果有重複的壓縮檔
                    //        {
                    //            MessageBoxResult coverResult = MessageBox.Show($"發現重複的檔案名稱 '{dataName}'，請問是否覆蓋此壓縮檔", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    //            if (coverResult == MessageBoxResult.Yes)//如果選擇要覆蓋檔案
                    //            {
                    //                File.Delete(zipPath);//刪除檔案
                    //                BackupFile(zipPath);//備份檔案
                    //            }
                    //        }
                    //        else
                    //        {
                    //            BackupFile(zipPath);//備份檔案
                    //        }
                    //    }
                    //}
                    //if (NcPath != string.Empty)//有選擇nc路徑
                    //    DeleteFolder(ApplicationVM.DirectoryNc());//刪除既有nc檔案
                    //if (BomPath != string.Empty)//有選擇報表路徑
                    //    File.Delete(ApplicationVM.FileTeklaBom());//刪除既有的報表
                }


                    ProcessingScreenWin.Show(inputBlock: InputBlockMode.Window, timeout: 100);
                    ProcessingScreenWin.ViewModel.IsIndeterminate = true;
                    //ScreenManagerWaitIndicator.Show(inputBlock: InputBlockMode.None, timeout: 100);
                    STDSerialization ser = new STDSerialization();//序列化處理器
                                                                  //ObSettingVM obvm = new ObSettingVM();
                    //Thread.Sleep(1000); //暫停兩秒為了要顯示 ScreenManager
                    //ProcessingScreenWin.ViewModel.Status = "複製文件到 Model 中 ...";
                    //ScreenManagerWaitIndicator.ViewModel.Status = "複製文件到 Model 中 ...";
                    Thread.Sleep(500); //暫停兩秒為了要顯示 ScreenManager



                    #region 只跑BOM
                    // 只跑BOM && string.IsNullOrEmpty(NcPath)
                    if (BomPath != string.Empty)//如果有選擇報表路徑
                    {
                        if (File.Exists(ApplicationVM.FileTeklaBom())) //檔案存在
                        {
                            ProcessingScreenWin.ViewModel.Status = $"正在刪除BOM表舊檔案";
                            File.Delete(ApplicationVM.FileTeklaBom());//刪除檔案
                            ProcessingScreenWin.ViewModel.Status = $"刪除BOM表舊檔案成功！";
                        }

                        ProcessingScreenWin.ViewModel.Status = $"正在複製BOM表檔案到模型";

                        File.Copy(BomPath, ApplicationVM.FileTeklaBom());//複製報表到模型

                        ProcessingScreenWin.ViewModel.Status = $"複製BOM表檔案完成";
                        CommonViewModel.ProjectProperty.BomLoad = DateTime.Now; //bom載入時間
                        CommonViewModel.ProjectProperty.Revise = DateTime.Now;//專案變動所以修改日期


                        ProcessingScreenWin.ViewModel.Status = $"正在複製BOM表檔案到模型";
                        TeklaBomFactory teklaHtemlFactory = new TeklaBomFactory($@"{ApplicationVM.FileTeklaBom()}"); //報表讀取器
                        bool loadBomResult = teklaHtemlFactory.Load(ProcessingScreenWin.ViewModel);//載入報表物件結果

                        if (loadBomResult) //載入成功
                        {
                            ProcessingScreenWin.ViewModel.Status = $"載入報表物件結果...";
                            CommonViewModel.ProjectProperty.IsBomLoad = true;//改變報表載入參數
                            ser.SetSteelAssemblies(teklaHtemlFactory.SteelAssemblies);//序列化構件資訊
                            ser.SetProfileList(teklaHtemlFactory.ProfileList);


                            ProcessingScreenWin.ViewModel.Status = $"正在存取序列化物件";
                            ProcessingScreenWin.ViewModel.IsIndeterminate = true;
                            int KVP_Count = teklaHtemlFactory.KeyValuePairs.Count;
                            int ci = 0;
                            foreach (var el in teklaHtemlFactory.KeyValuePairs)//逐步存取序列化物件
                            {
                                //判斷 type 序列化
                                if (el.Value[0].GetType() == typeof(SteelPart))
                                {
                                    SteelPart steel = (SteelPart)el.Value[0]; //轉換物件
                                    if (ObSettingVM.allowType.Contains(steel.Type))
                                    {
                                        int index = BomProperties.FindIndex(e => e.Type == steel.Type); //查看是否有相同的斷面規格在報表屬性設定檔內
                                        if (index == -1) //不再報表屬性內
                                        {
                                            BomProperties.Add(new BomProperty() { Type = steel.Type });//加入到列表內
                                        }
                                        ser.SetPart(el.Key.GetHashCode().ToString(), el.Value);
                                    }

                                    ci++;
                                }
                                else if (el.Value[0].GetType() == typeof(SteelBolts))
                                {
                                    ser.SetBolts(el.Key.GetHashCode().ToString(), el.Value);
                                }
                                ProcessingScreenWin.ViewModel.Progress = ci / KVP_Count;
                            }
                            ProcessingScreenWin.ViewModel.IsIndeterminate = false;


                            if (teklaHtemlFactory.LackMaterial())//如果報表導入到模型沒有找到符合的材質就序列化物件
                            {
                                ProcessingScreenWin.ViewModel.Status = $"正在存取序列化材質";
                                SerializationHelper.GZipSerializeBinary(teklaHtemlFactory.Material, ApplicationVM.FileMaterial());
                            }
                            if (teklaHtemlFactory.LackProfile())//如果報表導入到模型沒有找到符合的斷面規格就序列化物件
                            {
                                ProcessingScreenWin.ViewModel.Status = $"正在存取序列化斷面規格";
                                foreach (var item in teklaHtemlFactory.Profile)
                                {
                                    SerializationHelper.SerializeBinary(item.Value, $@"{ApplicationVM.DirectoryPorfile()}\{item.Key.ToString()}.inp");//斷面規格不是壓縮物件
                                }
                            }
                        }
                        CommonViewModel.ProjectProperty.BomLoad = DateTime.Now;//bom載入時間
                        CommonViewModel.ProjectProperty.Revise = DateTime.Now;//專案變動所以修改日期
                    }
                    #endregion
                    TeklaNcFactory factory = new TeklaNcFactory();//nc1 讀取器
                    #region 未有NC檔之零件
                    ObSettingVM obvm = new ObSettingVM();
                    //STDSerialization ser = new STDSerialization();
                    Dictionary<string, ObservableCollection<SteelPart>> part = ser.GetPart();
                    // 取得所有NC檔之路徑
                    string path = ApplicationVM.DirectoryNc();
                    string dataName = Path.GetFileName(path);//檔案名稱
                                                             // 取得所有斷面規格
                    List<SteelAttr> saAll = ser.GetSteelAttr().Values.SelectMany(x => x).ToList();
                    // 取得NC檔之檔名(零件)
                    List<String> hasNCPart = GetAllNcPath(path).Select(x => x.Substring(x.LastIndexOf("\\") + 1, x.LastIndexOf(".nc1") - x.LastIndexOf("\\") - 1)).ToList();
                    // 取得無NC之零件
                    List<SteelPart> hasNoNCPart = part.SelectMany(x => x.Value).Where(x => hasNCPart.IndexOf(x.Number)==-1).ToList();
                    ObservableCollection<DataCorrespond> dc = new ObservableCollection<DataCorrespond>();
                    //ModelExt model = new ModelExt();
                    //devDept.Eyeshot.Model model = new devDept.Eyeshot.Model();
                    factory.ncTemps.Clear();
                    foreach (SteelPart item in hasNoNCPart)
                    {
                        SteelAttr sa = new SteelAttr()
                        {      
                            PartNumber = item.Number,
                            Length = item.Length,
                            H = item.H,
                            W = item.W,
                            t1 = item.t1,
                            t2 = item.t2,
                            Number = item.Count,
                            Profile = item.Profile,
                            Type = item.Type,
                            Name = item.DrawingName,
                            GUID = Guid.NewGuid(),
                            Phase = item.Phase,
                            ShippingNumber = item.ShippingNumber,
                            Title1 = item.Title1,
                            Title2 = item.Title2,
                            Material = item.Material,
                        };
                        factory.ncTemps.Add(new NcTemp() { SteelAttr = sa });




                        //string profile = item.Profile;
                        //SteelAttr sa = saAll.Find(x => x.Profile == profile);
                        //model.InitializeViewports();
                        ////model.Blocks.Add(new Steel3DBlock(Steel3DBlock.GetProfile(sa)));//加入鋼構圖塊到模型
                        //var result = new Steel3DBlock(Steel3DBlock.GetProfile(sa));
                        //model.Blocks.Add(result);//加入鋼構圖塊到模型
                        //DataCorrespond data = new DataCorrespond()
                        //{
                        //    DataName = sa.GUID.ToString(),
                        //    Number = sa.PartNumber,
                        //    Type = sa.Type,
                        //    Profile = sa.Profile,
                        //    TP = false,
                        //};
                        //ser.SetPartModel(model.Blocks[1].Name, model);
                        //dc.Add(data);
                    }
                    ser.SetDataCorrespond(dc);

                
                    #endregion

                #region 只跑NC1
                // 只跑NC1 && string.IsNullOrEmpty(BomPath)
                    if (NcPath != string.Empty) //如果有選擇nc路徑
                    {
                        DeleteFolder(ApplicationVM.DirectoryNc());//刪除既有nc檔案
                        CopyFolder(NcPath);//複製nc路徑的nc1檔案
                        var profile = ser.GetSteelAttr();
                        

                        ProcessingScreenWin.ViewModel.Status = $"正在載入NC表檔案到模型";
                        bool loadNcResult = factory.Load(ProcessingScreenWin.ViewModel); //載入NC檔案
                        if (loadNcResult)//NC檔案載入成功
                        {
                            CommonViewModel.ProjectProperty.NcLoad = DateTime.Now;//nc載入時間
                            CommonViewModel.ProjectProperty.Revise = DateTime.Now;//專案變動所以修改日期
                            CommonViewModel.ProjectProperty.IsNcLoad = true;//改變報表載入參數
                        }
                    }
                    #endregion

                    //if (!string.IsNullOrEmpty(BomPath) && !string.IsNullOrEmpty(NcPath))
                    //{
                    //    #region BOM表
                    //    if (File.Exists(ApplicationVM.FileTeklaBom())) //檔案存在
                    //    {
                    //        File.Delete(ApplicationVM.FileTeklaBom());//刪除檔案
                    //    }
                    //    File.Copy(BomPath, ApplicationVM.FileTeklaBom());//複製報表到模型
                    //    CommonViewModel.ProjectProperty.BomLoad = DateTime.Now; //bom載入時間
                    //    CommonViewModel.ProjectProperty.Revise = DateTime.Now;//專案變動所以修改日期

                    //    TeklaBomFactory teklaHtemlFactory = new TeklaBomFactory($@"{ApplicationVM.FileTeklaBom()}"); //報表讀取器 
                    //    bool loadBomResult = teklaHtemlFactory.Load(ScreenManager.ViewModel);//載入報表物件結果
                    //    #endregion

                    //    #region NC檔
                    //    DeleteFolder(ApplicationVM.DirectoryNc());//刪除既有nc檔案
                    //    CopyFolder(NcPath);//複製nc路徑的nc1檔案

                    //    TeklaNcFactory factory = new TeklaNcFactory();//nc1 讀取器
                    //    bool loadNcResult = factory.Load(ScreenManager.ViewModel); //載入NC檔案
                    //    #endregion
                    //}
                    ProcessingScreenWin.ViewModel.Status = $"改變ioc內部報表屬性參數";
                    CommonViewModel.ProjectProperty.BomProperties = BomProperties; //改變ioc內部報表屬性參數
                    WriteProjectProperty(CommonViewModel.ProjectProperty);//改變目前vm的參數
                    ser.SetProjectProperty(CommonViewModel.ProjectProperty);
                    ProcessingScreenWin.ViewModel.Status = "結束 ...";
                    Thread.Sleep(2500); //暫停1秒為了要顯示 ScreenManager
                    ProcessingScreenWin.Close();//關閉等待畫面

                    WinUIMessageBox.Show(null,
                       $"{CommonViewModel.ImportNCFilesVM.Name} 已匯入完成",
                       "通知",
                       MessageBoxButton.OK,
                       MessageBoxImage.Exclamation,
                       MessageBoxResult.None,
                       MessageBoxOptions.None,
                        FloatingMode.Window);


                    //頁面跳轉
                    ProcessingScreenWin.Show();
                    ProcessingScreenWin.ViewModel.Status = $"準備跳轉至製品設定頁面";
                     if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)
                    {
                        WPFSTD105.ViewLocator.ApplicationViewModel.CurrentPage = ApplicationPage.MachineProductSetting;
                    }
                    else
                    {
                        WPFSTD105.ViewLocator.OfficeViewModel.CurrentPage = OfficePage.ProductSettings;
                    }

                    ProcessingScreenWin.Close();

                });
            }
        }
        /// <summary>
        /// 取得模型資料夾所有的 nc1 檔案
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>目前模型內的所有 .nc1 檔案</returns>
        private IEnumerable<string> GetAllNcPath(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string ext = Path.GetExtension(d);//副檔名
                    if (ext == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        yield return d;
                    }
                    else
                    {
                        GetAllNcPath(d);
                    }
                }
            }
        }
        /// <summary>
        /// 屬性設定存取
        /// </summary>
        public ICommand BomPropertiesSaveCommand
        {
            get
            {
                return new WPFWindowsBase.RelayCommand(() =>
                {
                    CommonViewModel.ProjectProperty.BomProperties = BomProperties; //改變ioc內部報表屬性參數
                    STDSerialization ser = new STDSerialization(); //序列化器
                    ser.SetProjectProperty(CommonViewModel.ProjectProperty);//存取設定檔
                    CommonViewModel.Close.Execute(null);
                });
            }
        }

        /// <summary>
        /// 複製文件
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="childDir">子資料夾</param>
        private void CopyFolder(string dir, string childDir = "")
        {

            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    string dataName = Path.GetFileName(d);//檔案名稱
                    string ext = Path.GetExtension(d);//副檔名
                    if (ext == ".nc1") //如果是 nc 檔案
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                            fi.Attributes = FileAttributes.Normal;

                        File.Copy(d, $@"{ApplicationVM.DirectoryNc()}\{childDir}\{dataName}");//複製文件
                    }
                }
                else
                {
                    string create = $@"\{Path.GetFileName(d)}"; //要創建資料夾的相對路徑
                    Directory.CreateDirectory($@"{ApplicationVM.DirectoryNc()}\{create}");//(絕對位置)創建資料夾
                    CopyFolder(d, create);//遞迴複製子文件夹   
                }
            }
        }

        /// <summary>
        /// 刪除文件
        /// </summary>
        /// <param name="dir"></param>
        private void DeleteFolder(string dir)
        {
            foreach (string d in Directory.GetFileSystemEntries(dir))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件   
                }
                else
                    DeleteFolder(d);//遞迴删除子文件夹   
            }
            if (ApplicationVM.DirectoryNc() != dir) //如果不是模型的nc資料夾
            {
                Directory.Delete(dir);//删除已空文件夹   
            }
        }
        /// <summary>
        /// 備份 NC 與 Bom
        /// </summary>
        private void BackupFile(string zipPath)
        {
            ProcessingScreenWin.ViewModel.Status = "備份文件中 ...";
            Thread.Sleep(2000); //暫停兩秒為了要顯示 ScreenManager
            using (ZipFile zip = new ZipFile(Encoding.Default))
            {
                if (IsBomLoad) //如果有載入報表
                {
                    zip.AddFile($@"{ApplicationVM.FileTeklaBom()}", ""); //壓縮報表
                }
                if (IsNcLoad)//如果有載入nc檔
                {
                    zip.AddDirectory($@"{ApplicationVM.DirectoryNc()}"); //壓縮nc檔
                }
                zip.Save($@"{zipPath}");//存取物件
            }
        }
    }
}
