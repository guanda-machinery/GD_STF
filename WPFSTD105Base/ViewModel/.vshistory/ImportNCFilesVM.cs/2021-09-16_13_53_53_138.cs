using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
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
using WPFSTD105.Tekla;
using static WPFSTD105.ViewLocator;
namespace WPFSTD105
{
    /// <summary>
    /// 匯入檔案VM
    /// </summary>
    public class ImportNCFilesVM : WPFWindowsBase.BaseViewModel, IProjectProperty
    {
        /// <summary>
        /// 啟動畫面管理器
        /// </summary>
        public SplashScreenManager ScreenManager { get; set; } = SplashScreenManager.CreateWaitIndicator();
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ImportNCFilesVM()
        {
            STDSerialization ser = new STDSerialization();
            ScreenManager.Show();
            ProjectProperty project = ser.GetProjectProperty();
            Number = project.Number;
            BomProperties = project.BomProperties;
            Create = project.Create;
            Name = project.Name;
            Design = project.Design;
            Location = project.Location;
            NcLoad = project.NcLoad;
            Revise = project.Revise;
            IsNcLoad = project.IsNcLoad;
            IsBomLoad = project.IsBomLoad;
            BomLoad = project.BomLoad;
            NcLoadArray = new bool[] { IsNcLoad, !IsNcLoad };
            BomLoadArray = new bool[] { IsBomLoad, !IsBomLoad };
            ImportNcCommand = ImportNc();
            ImportBomCommand = ImportBom();
            SaveCommand = Save();
        }
        /// <inheritdoc/>
        public ObservableCollection<BomProperty> BomProperties { get; set; }
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
        /// <summary>
        /// 匯入 nc 命令
        /// </summary>
        public ICommand ImportNcCommand { get; set; }
        private WPFWindowsBase.RelayCommand ImportNc()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                FolderBrowserDialogService service = DevExpand.NewFolder("請選擇NC放置的資料夾");
                IFolderBrowserDialogService folder = service;
                folder.ShowDialog();//Show 視窗
                NcPath = folder.ResultPath;//選擇的路徑
            });
        }
        /// <summary>
        /// 匯入報表命令
        /// </summary>
        public ICommand ImportBomCommand { get; set; }
        private WPFWindowsBase.RelayCommand ImportBom()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                DXOpenFileDialog dX = new DXOpenFileDialog();
                dX.Filter = "Csv 檔案 |*.csv";
                dX.ShowDialog();//Show 視窗
                BomPath = dX.FileName;
            });
        }
        /// <summary>
        /// 關閉命令
        /// </summary>
        public ICommand ClosedCommand { get; set; }
        /// <summary>
        /// 匯入報表命令
        /// </summary>
        public ICommand SaveCommand { get; set; }
        private WPFWindowsBase.RelayCommand Save()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                ScreenManager.ViewModel.Status = "準備匯入 ....";
                ScreenManager.Show();

                Thread.Sleep(2000); //暫停兩秒為了要顯示 ScreenManager
                if (IsNcLoad || IsBomLoad) //如果有載入過報表
                {
                    MessageBoxResult saveAsResult = MessageBox.Show($"請問是否要備份之前載入的檔案", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    if (saveAsResult == MessageBoxResult.Yes) //如果要備份檔案
                    {
                        FolderBrowserDialogService service = DevExpand.NewFolder("請選擇另存路徑"); //文件夾瀏覽器對話服務
                        IFolderBrowserDialogService folder = service;
                        folder.ShowDialog();//Show 視窗
                        string path = folder.ResultPath;//用戶選擇的路徑
                        if (path != string.Empty) //如果有選擇路徑
                        {
                            string dataName = $"{ ApplicationViewModel.ProjectName }{ DateTime.Now.ToString("yy-MM-dd-HH-mm-ss")}.zip";//壓縮檔名稱
                            string zipPath = $@"{path}\{dataName}";//壓縮檔完整路徑
                            if (File.Exists(zipPath))//如果有重複的壓縮檔
                            {
                                MessageBoxResult coverResult = MessageBox.Show($"發現重複的檔案名稱 '{dataName}'，請問是否覆蓋此壓縮檔", "通知", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                                if (coverResult == MessageBoxResult.Yes)//如果選擇要覆蓋檔案
                                {
                                    File.Delete(zipPath);//刪除檔案
                                    BackupFile(zipPath);//備份檔案
                                }
                            }
                            else
                            {
                                BackupFile(zipPath);//備份檔案
                            }
                        }
                    }

                    DeleteFolder(ApplicationVM.DirectoryNc());//刪除既有nc檔案
                    File.Delete(ApplicationVM.FileTeklaBom());//刪除既有的報表
                }
                ScreenManager.ViewModel.Status = "複製文件到 Model 中 ...";
                Thread.Sleep(2000); //暫停兩秒為了要顯示 ScreenManager
                if (NcPath != string.Empty) //如果有選擇nc路徑
                {
                    CopyFolder(NcPath);//複製nc路徑的nc1檔案
                }
                if (BomPath != string.Empty)//如果有選擇報表路徑
                {
                    if (File.Exists(ApplicationVM.FileTeklaBom())) //檔案存在
                    {
                        File.Delete(ApplicationVM.FileTeklaBom());//刪除檔案
                    }
                    File.Copy(BomPath, ApplicationVM.FileTeklaBom());//複製報表到模型
                  
                    //TODO: TEKLA 報表轉換物件尚未完成
                    STDSerialization serialization = new STDSerialization();//序列化處理器
                    TeklaBomFactory teklaHtemlFactory = new TeklaBomFactory($@"{ ApplicationVM.FileTeklaBom()}"); //報表讀取器
                    teklaHtemlFactory.Load(ScreenManager.ViewModel);//載入物件
                    serialization.SetSteelAssemblies(teklaHtemlFactory.SteelAssemblies);//序列化構件資訊
                    serialization.SetProfileList(teklaHtemlFactory.ProfileList);
                    foreach (var el in teklaHtemlFactory.KeyValuePairs)//逐步存取序列化物件
                    {
                        //判斷 type 序列化
                        if (el.Value[0].GetType() == typeof(SteelPart))
                        {
                            //string dataNmae = el.Key;
                            serialization.SetPart(el.Key.GetHashCode().ToString(), el.Value);
                        }
                        else if (el.Value[0].GetType() == typeof(SteelBolts))
                        {
                            serialization.SetBolts(el.Key.GetHashCode().ToString(), el.Value);
                        }
                    }
                    if (teklaHtemlFactory.LackMaterial())//如果報表導入到模型沒有找到符合的材質就序列化物件
                    {
                        SerializationHelper.GZipSerializeBinary(teklaHtemlFactory.Material, ApplicationVM.FileMaterial());
                    }
                    if (teklaHtemlFactory.LackProfile())//如果報表導入到模型沒有找到符合的斷面規格就序列化物件
                    {
                        foreach (var item in teklaHtemlFactory.Profile)
                        {
                            SerializationHelper.SerializeBinary(item.Value, $@"{ApplicationVM.DirectoryPorfile()}\{item.Key.ToString()}.inp");//此項不是壓縮物件
                        }
                    }
                }

            });
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
                    string c = Path.GetExtension(d);//副檔名
                    if (c == ".nc1") //如果是 nc 檔案
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
        /// 載入 tekla 
        /// </summary>
        private void LoadTekla()
        {

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
            ScreenManager.ViewModel.Status = "備份文件中 ...";
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
