using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// STD Win 抽象
    /// </summary>
    public abstract class STDBaseWindowView : WPFBase.WindowsViewModel, IWindowsVM
    {
        /// <summary>
        /// 命令對應
        /// </summary>
        public STDBaseWindowView(Window window) : base(window)
        {
            HomeCommand = HomePage();
            ImportFileCommand = ImportFile();
            ModifyProjectInfoCommand = ModifyProjectInfo();
            OpenProjectCommand = OpenProject();
            OutProjectNameCommand = OutProjectName();
            SaveAsCommand = SaveAs();
        }


        /// <inheritdoc/>
        public ICommand HomeCommand { get; set; }
        private WPFBase.RelayCommand HomePage()
        {
            return new WPFBase.RelayCommand(() =>
            {
                if (!WPFSTD105.Properties.SofSetting.Default.OfficeMode)//如果是工廠模式
                {
                    ApplicationViewModel.CurrentPage = ApplicationPage.Home;
                }
                else
                {
                    OfficeViewModel.CurrentPage = OfficePage.Home;
                }
            });
        }
        /// <inheritdoc/>
        public ICommand ImportFileCommand { get; set; }
        /// <summary>
        /// 匯入檔案功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand ImportFile()
        {
            return new WPFBase.RelayCommand(() =>
            {
                CommonViewModel.ImportNCFilesVM = new ImportNCFilesVM();
            });
        }
        /// <inheritdoc/>
        public ICommand ModifyProjectInfoCommand { get; set; }
        /// <summary>
        /// 修改專案屬性功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayCommand ModifyProjectInfo()
        {
            return new WPFBase.RelayCommand(() =>
            {
                STDSerialization ser = new STDSerialization();
                CommonViewModel.ProjectProperty.Revise = DateTime.Now;
                ser.SetProjectProperty(CommonViewModel.ProjectProperty);
            });
        }
        /// <inheritdoc/>
        public ICommand OpenProjectCommand { get; set; }
        /// <summary>
        /// 開啟專案功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayParameterizedCommand OpenProject()
        {
            return new WPFBase.RelayParameterizedCommand(e =>
            {
                if (e == null)
                {
                    MessageBox.Show("請選擇專案", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                    return;
                }

                string _ = e.ToString();
                CommonViewModel.ProjectName = _;
                if (CommonViewModel.ActionLoadProfile != null) //如果載入專案實有發現載入斷面規格與材質
                {
                    CommonViewModel.ActionLoadProfile.Invoke(); //觸發委派
                    CommonViewModel.ActionLoadProfile = null;//清除委派
                }
            });
        }
        /// <inheritdoc/>
        public ICommand OutProjectNameCommand { get; set; }
        /// <summary>
        /// 建立專案功能
        /// </summary>
        /// <returns></returns>
        protected abstract WPFBase.RelayParameterizedCommand OutProjectName();
        /// <inheritdoc/>
        public ICommand SaveAsCommand { get; set; }
        /// <summary>
        /// 另存專案功能
        /// </summary>
        /// <returns></returns>
        protected virtual WPFBase.RelayParameterizedCommand SaveAs()
        {
            return new WPFBase.RelayParameterizedCommand(el =>
            {
                SaveAsFu(el);

            });
        }

        private static void SaveAsFu(object el)
        {
            FolderBrowserDialogViewModel vm = (FolderBrowserDialogViewModel)el;
            string path = $@"{vm.ResultPath}\{CommonViewModel.ProjectProperty.Name}";
            //查詢是否有相同專案名稱
            if (Directory.Exists(path)) //如果有相同專案名稱請彈出通知
            {
                MessageBox.Show("已經有此專案名稱了, 請重新輸入", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                return;
            }

            CopyDir(ApplicationVM.DirectoryModel(), path);
            //存取路徑
            Properties.SofSetting.Default.LoadPath = vm.ResultPath;
            Properties.SofSetting.Default.Save();

            CommonViewModel.ProjectName = CommonViewModel.ProjectProperty.Name;
            CommonViewModel.ProjectProperty.Create = DateTime.Now;
            //序列化檔案
            STDSerialization ser = new STDSerialization();
            ser.SetProjectProperty(CommonViewModel.ProjectProperty);
        }

        /// <inheritdoc/>
        public ICommand SettingParCommand { get; set; }
        /// <inheritdoc/>
        public ICommand SoftwareSettingsCommand { get; set; }
        /// <inheritdoc/>
        public ICommand TypeSettingCommand { get; set; }
        /// <inheritdoc/>
        public ICommand WatchProjectCommand { get; set; }

        /// <summary>
        /// 複製文件
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="aimPath"></param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            // 檢查目標目錄是否以目錄分割字元結束如果不是則新增之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                aimPath += Path.DirectorySeparatorChar;
            // 判斷目標目錄是否存在如果不存在則新建之
            if (!Directory.Exists(aimPath))
                Directory.CreateDirectory(aimPath);
            // 得到源目錄的檔案列表，該裡面是包含檔案以及目錄路徑的一個數組
            string[] fileList = Directory.GetFileSystemEntries(srcPath);
            // 遍歷所有的檔案和目錄
            foreach (string file in fileList)
            {

                if (Directory.Exists(file))// 先當作目錄處理如果存在這個目錄就遞迴Copy該目錄下面的檔案
                    CopyDir(file, aimPath + Path.GetFileName(file));

                else // 否則直接Copy檔案
                    File.Copy(file, aimPath + Path.GetFileName(file), true);
            }
        }
    }
}
