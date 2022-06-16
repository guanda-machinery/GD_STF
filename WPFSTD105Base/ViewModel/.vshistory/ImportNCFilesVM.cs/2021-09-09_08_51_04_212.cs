using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Dialogs;
using GD_STD.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFSTD105.ViewModel
{
    /// <summary>
    /// 匯入檔案VM
    /// </summary>
    public class ImportNCFilesVM : WPFWindowsBase.BaseViewModel, IProjectProperty
    {
        /// <summary>
        /// 標準建構式
        /// </summary>
        public ImportNCFilesVM()
        {
            STDSerialization ser = new STDSerialization();
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
        public string NcPath { get; set; }
        /// <summary>
        /// bom 文件路徑
        /// </summary>
        public string BomPath { get; set; }
        /// <summary>
        /// 匯入 nc 命令
        /// </summary>
        public ICommand ImportNcCommand { get; set; }
        private WPFWindowsBase.RelayCommand ImportNc()
        {
            return new WPFWindowsBase.RelayCommand(() =>
            {
                FolderBrowserDialogService service = new FolderBrowserDialogService();
                service.Description = "請選擇NC放置的資料夾"; //標題名稱
                service.ShowNewFolderButton = false;//該值指示“新建文件夾”按鈕是否顯示在文件夾瀏覽器對話框
                service.RootFolder = Environment.SpecialFolder.Desktop;
                service.RestorePreviouslySelectedDirectory = true;
                if (IsNcLoad) //如果有載入過 NC 檔案
                {
                    service.StartPath = ApplicationVM.DirectoryNc(); //起始路徑
                }
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

                //TODO:壓縮檔案到指定資料
                string startPath = @".\start";
                string zipPath = @".\result.zip";
                string extractPath = @".\extract";

                ZipFile.CreateFromDirectory(startPath, zipPath);

                ZipFile.ExtractToDirectory(zipPath, extractPath);
            });
        }
    }
}
