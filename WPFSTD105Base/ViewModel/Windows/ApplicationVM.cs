using GD_STD;
using GD_STD.Enum;
using System;
using WPFSTD105.Listening;
using WPFSTD105.ViewModel;
using static WPFSTD105.ViewLocator;
using WPFBase = WPFWindowsBase;
using DevExpress.Xpf.Core;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GD_STD.Data;
using System.Windows.Input;
using DevExpress.Xpf.WindowsUI;

namespace WPFSTD105
{
    /// <summary>
    /// 應用程序狀態作為視圖模型
    /// </summary>
    public class ApplicationVM : WPFBase.BaseViewModel, IApplicationVM
    {
        public ApplicationVM()
        {
            Close = new WPFBase.RelayCommand(() =>
            {
                if (ChildWin != null)
                {
                    ChildWin.Close();
                    ChildWin = null;
                }
            });
        }
        #region 公開屬性
        ///// <summary>
        ///// 接續專案
        ///// </summary>
        //public bool ContinueProject { get; set; }
        /// <summary>
        /// 完成第一次原點復歸
        /// </summary>
        public bool FirstOrigin { get; set; } = true;
        /// <summary>
        /// 是否在首頁
        /// </summary>
        public bool IsHome { get; set; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string ErrorInfo { get; set; }
        /// <summary>
        /// 專案列表
        /// </summary>
        /// <remarks>
        /// 在 <see cref="ApplicationVM.DirectoryModel"/> 內所有的專案
        /// </remarks>
        public ObservableCollection<string> ProjectList { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// 應用程序目前頁面
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => _CurrentPage;
            set
            {
                PreviousPage = _CurrentPage;//紀錄上一頁

                //判斷是否為選擇機器模式
                //if (value == ApplicationPage.ModeSelected)
                //    UserMode = MachineMode.NULL;

                //判斷是否要隱藏次要物件
                if (value >= ApplicationPage.Home)
                {
                    if (!SideMenuVisible)
                        SideMenuVisible = true;
                }
                else
                {
                    if (SideMenuVisible)
                        SideMenuVisible = false;
                }

                //如果是首頁或加工監控頁面開啟聆聽軸向位置
                if (value == ApplicationPage.Home || value == ApplicationPage.Monitor)
                {
                    //如果是加工監控
                    if (value == ApplicationPage.Monitor)
                    {
                        ChangeLevel(LEVEL.INFERIOR, LEVEL.INFERIOR);
                    }
                    //如果是首頁
                    else
                    {
                        ChangeLevel(LEVEL.HIGH, LEVEL.MEDIUM);
                    }
                    AxisInfoListening.Mode = true;
                }
                //如果不是首頁或加工監控
                else
                {
                    AxisInfoListening.Mode = false;//關閉聆聽
                }
                _CurrentPage = value;
            }
        }
        /// <summary>
        /// 應用程序上個頁面
        /// </summary>
        public ApplicationPage PreviousPage { get; private set; } = ApplicationPage.Home;
        /// <summary>
        /// 目前是讀取 Codesys 狀態
        /// </summary>
        public bool IsRead { get; set; } = false;
        /// <summary>
        /// 人機面板功能按鈕
        /// </summary>
        public PanelButton PanelButton { get; set; } = new PanelButton();
        /// <summary>
        /// 用戶選擇機器模式
        /// </summary>
        public MachineMode UserMode { get; set; }
        /// <summary>
        /// 隱藏次要物件
        /// </summary>
        public bool SideMenuVisible { get; set; } = false;
        /// <inheritdoc/>
        public ICommand Close { get; set; }
        /// <inheritdoc/>
        public Window ChildWin { get; set; }
        /// <summary>
        /// 網際網路的狀態
        /// </summary>
        public bool IsConnect
        {
            get
            {
                string url = "http://www.google.com";
                try
                {
                    System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                    System.Net.WebResponse myResponse = myRequest.GetResponse();
                }
                catch (System.Net.WebException)
                {
                    return false;
                }
                return true;
            }
        }
        /// <inheritdoc/>
        public AccountNumber AccountNumber { get; set; } = new AccountNumber();
        /// <summary>
        /// 聆聽人機面板訊號
        /// </summary>
        public PanelListening PanelListening { get; set; } = new PanelListening();
        ///// <summary>
        ///// 聆聽斷電保持訊號
        ///// </summary>
        //public OutageListening OutageListening { get; set; } = new OutageListening();
        ///// <summary>
        ///// 本機狀態
        ///// </summary>
        //public Host Host { get; set; }
        ///// <summary>
        ///// 聆聽 Codesys 狀態 
        ///// </summary>
        //public HostListening HostListening { get; set; } = new HostListening();
        /// <summary>
        /// 聆聽軸向位置
        /// </summary>
        public AxisInfoListening AxisInfoListening { get; set; } = new AxisInfoListening();
        /// <summary>
        /// 手機聆聽端
        /// </summary>
        public PhoneListening PhoneListening { get; set; } = new PhoneListening();
        /// <summary>
        /// 軸向訊息
        /// </summary>
        public AxisInfo AxisInfo { get; set; }
        /// <summary>
        /// 工程模式
        /// </summary>
        public bool EngineeringMode { get; set; } = false;
        /// <inheritdoc/>
        public string ProjectName { get; set; } = null;
        /// <inheritdoc/>
        public string GetNowDate { get; set; } = "1970/01/01";
        /// <inheritdoc/>
        public string GetNowTime { get; set; } = "00:00";
        /// <summary>
        /// APP 手動操作連線
        /// </summary>
        public bool AppManualConnect { get; set; } = false;
        /// <inheritdoc/>
        public ProjectProperty ProjectProperty { get; set; }
        /// <summary>
        /// 斷電保持電量
        /// </summary>
        public BAVT BAVT { get; set; }
        /// <inheritdoc/>
        public Action ActionLoadProfile { get; set; } = null;
        /// <inheritdoc/>
        public ImportNCFilesVM ImportNCFilesVM { get; set; }
        /// <inheritdoc/>
        public MainAxisLocation MainAxisLocation { get; set; }
        /// <inheritdoc/>
        public MainAxisListening MainAxisListening { get; set; }
        #endregion


        #region 公開靜態方法

        /// <summary>
        /// 取得模型資料夾
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static List<string> GetModelDirectory(string path)
        {
            List<string> result = new List<string>();
            // 2020.06.23 呂宗霖 將以下判斷式的Properties.SofSetting.Default.LoadPath都改為newPath
            string newPath = path;
            //判斷是軟體用的模型資料夾
            foreach (var model in Directory.GetDirectories(path))
            {
                string name = Path.GetFileNameWithoutExtension(model);
                //TODO : 這邊要檢查邏輯，要新增，請參照一開始建置專案的時候，有產生的資料與資料夾。
                if (Directory.Exists($@"{newPath}\{name}\{ModelPath.Nc}")) // NC 資料夾
                    if (Directory.Exists($@"{newPath}\{name}\{ModelPath.Profile}")) //斷面規格目錄
                        if (File.Exists($@"{newPath}\{name}\{ModelPath.ProjectProperty}"))//專案參數
                            if (Directory.Exists($@"{newPath}\{name}\{ModelPath.DevMaterial}"))//素材3D序列化目錄
                                if (Directory.Exists($@"{newPath}\{name}\{ModelPath.Dev_Part}"))//單零件的3D圖形目錄
                                    if (File.Exists($@"{newPath}\{name}\{ModelPath.SteelAssembly}"))//構件資料目錄
                                        if (Directory.Exists($@"{newPath}\{name}\{ModelPath.SteelPart}"))//零件資料序列化的目錄
                                            if (Directory.Exists($@"{newPath}\{name}\{ModelPath.SteelBolts}"))//螺栓資料序列化的目錄
                                                if (File.Exists(FileProfileList($@"{newPath}\{name}")))//螺栓資料序列化的目錄
                                                    if (File.Exists($@"{newPath}\{name}\{ModelPath.Material}"))//模型材質文件
                                                        if (Directory.Exists($@"{newPath}\{name}\{ModelPath.WorkMaterialBackup}"))//加工資料序列化的目錄
                                                            result.Add(name);//模型資料夾名稱
            }
            return result;
        }
        /// <summary>
        /// 模型資料夾內的斷面規格
        /// </summary>
        /// <returns>
        /// 目前用戶建立專案模型資料夾內的斷面規格路徑
        /// <para>路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.Profile"/></para>
        /// </returns>
        public static string DirectoryPorfile()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{DirectoryModel()}\\{ModelPath.Profile}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 專案參數
        /// </summary>
        /// <returns>
        /// 目前用戶開啟的模型資料夾的專案屬性
        /// <para>路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.ProjectProperty"/></para>
        /// </returns>
        public static string FileProjectProperty()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return FileProjectProperty(DirectoryModel());

            return null;
            //throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 專案參數
        /// </summary>
        /// <returns>
        /// 目前用戶指定的模型資料夾的專案屬性
        /// <para>路徑組合 : path \\<see cref="ModelPath.ProjectProperty"/></para>
        /// </returns>
        public static string FileProjectProperty(string path)
        {
            return $"{path}\\{ModelPath.ProjectProperty}";
        }
        /// <summary>
        /// 專案模型的資料夾
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾路徑
        /// <para>路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/></para>
        /// </returns>
        public static string DirectoryModel()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{Properties.SofSetting.Default.LoadPath}\\{CommonViewModel.ProjectName}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 加工的資訊
        /// </summary>
        /// <returns></returns>
        public static string DirectoryWorkMaterialBackup()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{Properties.SofSetting.Default.LoadPath}\\{CommonViewModel.ProjectName}\\{ModelPath.WorkMaterialBackup}";
            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        ///// <summary>
        ///// 加工列表鑽孔參數備份檔案
        ///// </summary>
        //public static string WorkMaterialBackup()
        //{
        //    return $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\{ModelPath.WorkMaterialBackup}";
        //}
        /// <summary>
        /// 加工列表其他參數備份檔案
        /// </summary>
        public static string WorkMaterialOtherBackup()
        {
            return $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\{ModelPath.WorkMaterialOtherBackup}";
        }
        /// <summary>
        /// 加工列表其他參數備份檔案
        /// </summary>
        public static string WorkMaterialIndexBackup()
        {
            return $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\{ModelPath.WorkMaterialIndexBackup}";
        }
        /// <summary>
        /// 刀具品牌
        /// </summary>
        /// <returns></returns>
        public static string FileDrillBrand()
        {
            string str = System.Environment.CurrentDirectory;
            return $@"{str}\{ModelPath.DrillBrand}";
        }
        /// <summary>
        /// 目前模型 .nc 檔案的資料夾路徑
        /// </summary>
        /// <returns>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.Nc"/>
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectoryNc()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Nc}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 目前模型存放素材3D序列化的資料夾路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放素材3D序列化的資料夾路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.DevMaterial"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectoryMaterial()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.DevMaterial}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 指定模型存放素材3D序列化的資料夾路徑
        /// </summary>
        /// <param name="modelPath">模型資料夾</param>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放素材3D序列化的資料夾路徑。
        /// <para>
        /// 路徑組合 : modelPath\\<see cref="ProjectName"/>\\<see cref="ModelPath.DevMaterial"/>
        /// </para> 
        /// </returns>
        public static string DirectoryMaterial(string modelPath) => $@"{modelPath}\{ModelPath.DevMaterial}";
        /// <summary>
        /// 目前模型存放單零件的3D圖形序列化的路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放單零件的3D圖形序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.Dev_Part"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectoryDevPart()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Dev_Part}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 目前模型存放構件資料序列化的路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放構件資料序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.SteelAssembly"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string FileSteelAssembly()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelAssembly}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 刀庫檔案路徑
        /// </summary>
        /// <returns></returns>
        public static string FileDrillWarehouse()
        {
            string str = System.Environment.CurrentDirectory;
            return $@"{str}\{ModelPath.DrillWarehouse}";
        }
        /// <summary>
        /// 目前模型存放零件資料序列化的路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放零件資料序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.SteelPart"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectorySteelPart()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelPart}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 目前模型存放螺栓資料序列化的路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放螺栓資料序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.SteelBolts"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectorySteelBolts()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelBolts}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 目前模型有使用的斷面規格資料表路徑。
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放螺栓資料序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>
        /// </para> 
        /// </returns>
        public static string FileProfileList()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return FileProfileList($@"{DirectoryModel()}");

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 指定模型有使用的斷面規格資料表路徑。
        /// </summary>
        /// <param name="modelPath">模型資料夾</param>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放素材3D序列化的資料夾路徑。
        /// <para>
        /// 路徑組合 : modelPath\\<see cref="ModelPath.ProfileList"/>
        /// </para> 
        /// </returns>
        public static string FileProfileList(string modelPath) => $@"{modelPath}\{ModelPath.ProfileList}";
        /// <summary>
        /// 模型材質文件
        /// </summary>
        /// <returns></returns>
        public static string FileMaterial()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Material}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 排版設定列表 VM 
        /// </summary>
        /// <returns></returns>
        public static string FileMaterialDataView()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.MaterialDataView}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 指定 Model 排版設定列表 VM 
        /// </summary>
        /// <returns></returns>
        public static string FileMaterialDataView(string modelPath) => $@"{modelPath}\{ModelPath.MaterialDataView}";

        /// <summary>
        /// Tekla 報表文件
        /// </summary>
        /// <returns></returns>
        public static string FileTeklaBom()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Bom}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// nc 尚未實體化的資料路徑
        /// </summary>
        /// <returns></returns>
        public static string FileNcTemp()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.NcBackup}";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 創建模型資料
        /// </summary>
        /// <param name="savePath">要存取的路徑</param>
        /// <returns>創建成功回傳 true，失敗則false</returns>
        public static bool CreateModel(string savePath)
        {
            // 2022/08/05 呂宗霖 請選擇專案路徑
            if (string.IsNullOrEmpty(savePath))
            {
                WinUIMessageBox.Show(null,
               $"專案路徑不可空白，請選擇專案路徑",
               "通知",
               MessageBoxButton.OK,
               MessageBoxImage.Exclamation,
               MessageBoxResult.None,
               MessageBoxOptions.None,
               FloatingMode.Popup);
                return false;
            }
            //FolderBrowserDialogViewModel vm =value;
            Properties.SofSetting.Default.LoadPath = savePath; //記住用戶載入的路徑
            Properties.SofSetting.Default.Save();//存取在設定檔內

            string path = $"{Properties.SofSetting.Default.LoadPath}\\{CommonViewModel.ProjectProperty.Name}";
            //查詢是否有相同專案名稱
            if (Directory.Exists(path)) //如果有相同專案名稱請彈出通知
            {
                //MessageBox.Show("已經有此專案名稱了, 請重新輸入", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                $"已經有此專案名稱了, 請重新輸入",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Popup);
                return false;
            }
            else
            {
                CommonViewModel.ProjectName = CommonViewModel.ProjectProperty.Name;
                //TODO: 這裡是一開始新建專案後建置資料夾的地方
                Directory.CreateDirectory(ApplicationVM.DirectoryModel());//專案資料夾路徑
                Directory.CreateDirectory(ApplicationVM.DirectoryNc());//存放 .nc 的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectoryMaterial());//素材檔案的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectoryDevPart()); //單零件的3D檔案的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectorySteelPart());//存放零件資料序列化的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectorySteelBolts());//存放螺栓資料序列化的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectoryPorfile());//存放螺栓資料序列化的資料夾
                Directory.CreateDirectory(ApplicationVM.DirectoryWorkMaterialBackup());//存放螺栓資料序列化的資料夾
                SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileSteelAssembly()); //斷面規格列表
                SerializationHelper.GZipSerializeBinary(new ObservableCollection<string>(), ApplicationVM.FileProfileList());//斷面規格列表
                SerializationHelper.GZipSerializeBinary(new ObservableCollection<DataCorrespond>(), ApplicationVM.FilePartList());//斷面規格列表
                SerializationHelper.GZipSerializeBinary(new ObservableCollection<MaterialDataView>(), ApplicationVM.FileMaterialDataView());//斷面規格列表
                SerializationHelper.GZipSerializeBinary(new NcTempList(), ApplicationVM.FileNcTemp()); //nc 尚未實體化的資料
                File.Copy($@"Profile\BH.inp", $@"{ApplicationVM.DirectoryPorfile()}\BH.inp");//複製 BH 斷面規格到模型內
                File.Copy($@"Profile\RH.inp", $@"{ApplicationVM.DirectoryPorfile()}\RH.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\L.inp", $@"{ApplicationVM.DirectoryPorfile()}\L.inp");//複製 L 斷面規格到模型內
                File.Copy($@"Profile\CH.inp", $@"{ApplicationVM.DirectoryPorfile()}\CH.inp");//複製 CH 斷面規格到模型內
                File.Copy($@"Profile\BOX.inp", $@"{ApplicationVM.DirectoryPorfile()}\BOX.inp");//複製 BOX 斷面規格到模型內
                //20220729 張燕華 斷面規格目錄-增加斷面規格
                File.Copy($@"Profile\TUBE.inp", $@"{ApplicationVM.DirectoryPorfile()}\TUBE.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\H.inp", $@"{ApplicationVM.DirectoryPorfile()}\H.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\I.inp", $@"{ApplicationVM.DirectoryPorfile()}\I.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\LB.inp", $@"{ApplicationVM.DirectoryPorfile()}\LB.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\BT.inp", $@"{ApplicationVM.DirectoryPorfile()}\BT.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\CT.inp", $@"{ApplicationVM.DirectoryPorfile()}\CT.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\T.inp", $@"{ApplicationVM.DirectoryPorfile()}\T.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\C.inp", $@"{ApplicationVM.DirectoryPorfile()}\C.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\PIPE.inp", $@"{ApplicationVM.DirectoryPorfile()}\PIPE.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\TURN_BUCKLE.inp", $@"{ApplicationVM.DirectoryPorfile()}\TURN_BUCKLE.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\WELD.inp", $@"{ApplicationVM.DirectoryPorfile()}\WELD.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\SA.inp", $@"{ApplicationVM.DirectoryPorfile()}\SA.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\GRATING.inp", $@"{ApplicationVM.DirectoryPorfile()}\GRATING.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\FB.inp", $@"{ApplicationVM.DirectoryPorfile()}\FB.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\RB.inp", $@"{ApplicationVM.DirectoryPorfile()}\RB.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\HNUT.inp", $@"{ApplicationVM.DirectoryPorfile()}\HNUT.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Profile\NUT.inp", $@"{ApplicationVM.DirectoryPorfile()}\NUT.inp");//複製 RH 斷面規格到模型內
                File.Copy($@"Mater.lis", $@"{ApplicationVM.FileMaterial()}");//複製材質到模型內
                STDSerialization ser = new STDSerialization();
                CommonViewModel.ProjectProperty.Create = DateTime.Now;
                ser.SetProjectProperty(CommonViewModel.ProjectProperty); //存取設定
                if (CommonViewModel.ActionLoadProfile != null) //如果載入專案實有發現載入斷面規格與材質
                {
                    CommonViewModel.ActionLoadProfile.Invoke(); //觸發委派
                    CommonViewModel.ActionLoadProfile = null;//清除委派
                }
                return true;
            }
        }

        /// <summary>
        /// 另存專案
        /// </summary>
        /// <param name="savePath">存取路徑</param>
        /// <returns>儲存成功回傳 true，失敗則false。</returns>
        public static bool SaveAs(string savePath)
        {
            string path = $@"{savePath}\{CommonViewModel.ProjectProperty.Name}";
            //查詢是否有相同專案名稱
            if (Directory.Exists(path)) //如果有相同專案名稱請彈出通知
            {
                //MessageBox.Show("已經有此專案名稱了, 請重新輸入", "通知", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.None, MessageBoxOptions.ServiceNotification);
                WinUIMessageBox.Show(null,
                $"已經有此專案名稱了, 請重新輸入",
                "通知",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation,
                MessageBoxResult.None,
                MessageBoxOptions.None,
                FloatingMode.Popup);
                return false;
            }

            CopyDir(ApplicationVM.DirectoryModel(), path);
            //存取路徑
            Properties.SofSetting.Default.LoadPath = savePath;
            Properties.SofSetting.Default.Save();

            CommonViewModel.ProjectName = CommonViewModel.ProjectProperty.Name;
            CommonViewModel.ProjectProperty.Create = DateTime.Now;
            //序列化檔案
            STDSerialization ser = new STDSerialization();
            ser.SetProjectProperty(CommonViewModel.ProjectProperty);
            return true;
        }
        ///// <summary>
        ///// 專案瀏覽
        ///// </summary>
        //public static void WatchProject()
        //{
        //    CommonViewModel.ProjectList = new ObservableCollection<string>(ApplicationVM.GetModelDirectory(Properties.SofSetting.Default.LoadPath));
        //} 
        /// <summary>
        /// 取得零件對應檔案路徑
        /// </summary>
        /// <returns></returns>
        public static string FilePartList()
        {
            string projectName = CommonViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{CommonViewModel.ProjectName}\part.lis";

            throw new Exception($"沒有專案路徑 (CommonViewModel.ProjectName is null)");
        }
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
        #endregion

        #region 公開方法

        /// <summary>
        /// 釋放所有聆聽模式
        /// </summary>
        public void DisposeListening()
        {
            if (PanelListening != null)
                PanelListening.Mode = false;
            if (AxisInfoListening != null)
                AxisInfoListening.Mode = false;
            //if (HostListening != null)
            //    HostListening.Mode = false;
            //if (OutageListening != null)
            //    HostListening.Mode = false;
        }
        #endregion

        #region 私有屬性
        private ApplicationPage _CurrentPage { get; set; } = ApplicationPage.Lock;
        #endregion

        #region 私有方法
        /// <summary>
        /// 判斷 Codesys 是否有啟動，自動調整 <see cref="PanelButton"/> and <see cref="AxisInfoListening"/>
        /// </summary>
        /// <param name="main">主要聆聽的等級</param>
        /// <param name="sec">次要聆聽的等級</param>
        private void ChangeLevel(LEVEL main, LEVEL sec)
        {
            //如果面板是執行狀態
            if (PanelButton.Run == true)
            {
                PanelListening.ChangeLevel(sec);//更改面板聆聽等級為次要等級
                AxisInfoListening.ChangeLevel(main);//更改軸向聆聽等級為主要等級
            }
            else
            {
                PanelListening.ChangeLevel(main);//更改面板聆聽等級為主要等級
                AxisInfoListening.ChangeLevel(sec);//更改軸向聆聽等級次要等級
            }
        }
        #endregion
    }
}
