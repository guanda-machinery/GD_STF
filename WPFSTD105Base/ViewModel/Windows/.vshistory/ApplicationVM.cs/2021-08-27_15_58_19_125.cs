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

namespace WPFSTD105
{
    /// <summary>
    /// 應用程序狀態作為視圖模型
    /// </summary>
    public class ApplicationVM : WPFBase.BaseViewModel
    {
        #region 公開屬性
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
                if (value == ApplicationPage.ModeSelected)
                    UserMode = MachineMode.NULL;

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
                        //TODO:實測
                        ChangeLevel(LEVEL.HIGH, LEVEL.INFERIOR);
                    }
                    //如果是首頁
                    else
                    {
                        //TODO:實測
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
        public ApplicationPage PreviousPage { get; private set; } = ApplicationPage.ModeSelected;
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
        /// <summary>
        /// 代表物聯網帳號密碼
        /// </summary>
        public AccountNumber AccountNumber { get; set; }
        /// <summary>
        /// 聆聽人機面板訊號
        /// </summary>
        public PanelListening PanelListening { get; set; } = new PanelListening();
        ///// <summary>
        ///// 聆聽斷電保持訊號
        ///// </summary>
        //public OutageListening OutageListening { get; set; } = new OutageListening();
        /// <summary>
        /// 聆聽 Codesys 狀態 
        /// </summary>
        public HostListening HostListening { get; set; } = new HostListening();
        /// <summary>
        /// 聆聽軸向位置與加工訊息
        /// </summary>
        public AxisInfoListening AxisInfoListening = new AxisInfoListening();

        /// <summary>
        /// 軸向訊息
        /// </summary>
        public AxisInfo AxisInfo { get; set; }
        /// <summary>
        /// 工程模式
        /// </summary>
        public bool EngineeringMode { get; set; } = false;
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; } = null;
        /// <summary>
        /// 取得現在日期
        /// </summary>
        public string GetNowDate { get; set; } = "1970/01/01";
        /// <summary>
        /// 取得現在時間
        /// </summary>
        public string GetNowTime { get; set; } = "00:00";
        ///// <summary>
        ///// 錯誤視窗標題
        ///// </summary>
        //public string ErrorTitle { get; set; } = "錯誤";
        ///// <summary>
        ///// 錯誤視窗內容
        ///// </summary>
        //public string ErrorContent { get; set; } = "發生未知的錯誤！";
        ///// <summary>
        ///// 錯誤視窗右按鈕字樣
        ///// </summary>
        //public string ErrorRightButtonText { get; set; } = "OK";
        ///// <summary>
        ///// 錯誤視窗左按鈕字樣
        ///// </summary>
        //public string ErrorLeftButtonText { get; set; } = "Cancel";
        ///// <summary>
        ///// 通知視窗
        ///// </summary>
        //public bool ShowMessage { get; set; } = false;
        ///// <summary>
        ///// 訊息視窗內容
        ///// </summary>
        //public MessageVM Message { get; set; } = new MessageVM();
        /// <summary>
        /// APP 手動操作連線
        /// </summary>
        public bool AppManualConnect { get; set; } = false;
        /// <summary>
        /// 控制Loading開關
        /// </summary>
        public bool IsWaitingIndicatorShown { get; set; }
        /// <summary>
        /// Loading顯示內容
        /// </summary>
        public string LoadingContent { get; set; }
        #endregion


        #region 公開靜態方法
        /// <summary>
        /// 檢測是否是 STD 模型
        /// </summary>
        /// <param name="modelPath"></param>
        /// <returns>如果是回傳 true，如果不是回傳 false。</returns>
        public static bool IsModel(string modelPath)
        {
            if (Directory.Exists($"{modelPath}\\{ModelPath.Nc}")) // NC 資料夾
                if (Directory.Exists($"{modelPath}\\{ModelPath.Material}")) //存放素材序列化的資料夾
                    if (Directory.Exists($"{modelPath}\\{ModelPath.Dev_Part}"))//存放單零件的3D圖形序列化的資料夾
                        if (Directory.Exists($"{modelPath}\\{ModelPath.Profile}"))//斷面規格資料夾
                            if (Directory.Exists($"{modelPath}\\{ModelPath.SteelPart}"))//零件資料夾
                                if (Directory.Exists($"{modelPath}\\{ModelPath.SteelBolts}"))//螺栓資料夾
                                    return true;

            return false;
        }
        /// <summary>
        /// 取得模型資料夾
        /// </summary>
        /// <returns></returns>
        public static List<string> GetModelDirectory()
        {
            List<string> result = new List<string>();
            //判斷是軟體用的模型資料夾
            foreach (var model in Directory.GetDirectories(Properties.SofSetting.Default.LoadPath))
            {
                if (IsModel(model))//檢測是否是 STD 模型
                    result.Add(Path.GetFileNameWithoutExtension(model));//模型資料夾名稱
            }
            return result;
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
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{Properties.SofSetting.Default.LoadPath}\\{ApplicationViewModel.ProjectName}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// .nc 檔案的資料夾路徑
        /// </summary>
        /// <returns>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.Nc"/>
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectoryNc()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.Nc}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 存放素材3D序列化的資料夾路徑
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放素材3D序列化的資料夾路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.Material"/>
        /// </para> 
        /// </returns>
        /// <remarks>
        /// 上一層目錄是 <see cref="DirectoryModel"/>
        /// </remarks>
        public static string DirectoryMaterial()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.Material}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 存放素材3D序列化的資料夾路徑
        /// </summary>
        /// <param name="modelPath">模型資料夾</param>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放素材3D序列化的資料夾路徑。
        /// <para>
        /// 路徑組合 : modelPath\\<see cref="ProjectName"/>\\<see cref="ModelPath.Material"/>
        /// </para> 
        /// </returns>
        public static string DirectoryMaterial(string modelPath) => $@"{modelPath}\{ModelPath.Material}";
        /// <summary>
        /// 存放單零件的3D圖形序列化的路徑
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
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.Dev_Part}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 存放構件資料序列化的路徑
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
        public static string DirectorySteelAssembly()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.SteelAssembly}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }

        /// <summary>
        /// 存放零件資料序列化的路徑
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
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.SteelPart}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 存放螺栓資料序列化的路徑
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
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.SteelBolts}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 斷面規格資料表
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，存放螺栓資料序列化的路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.ProfileList"/>
        /// </para> 
        /// </returns>
        public static string ProfileList()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.ProfileList}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        /// <summary>
        /// 報表屬性設定資料表
        /// </summary>
        /// <returns>
        /// 目前用戶建立或開啟的專案模型資料夾，報表屬性設定資料表路徑。
        /// <para>
        /// 路徑組合 : <see cref="Properties.SofSetting.LoadPath"/>\\<see cref="ProjectName"/>\\<see cref="ModelPath.BomProperty"/>
        /// </para> 
        /// </returns>
        public static string BomProperty()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\{ModelPath.BomProperty}";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
        }
        //刪除: PartList()
        /// <summary>
        /// 取得零件對應檔案路徑
        /// </summary>
        /// <returns></returns>
        public static string PartList()
        {
            string projectName = ApplicationViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{ApplicationViewModel.ProjectName}\part.lis";

            throw new Exception($"沒有專案路徑 (ApplicationViewModel.ProjectName is null)");
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
            if (HostListening != null)
                HostListening.Mode = false;
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
