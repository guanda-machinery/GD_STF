using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static WPFSTD105.ViewLocator;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBase = WPFWindowsBase;

namespace WPFSTD105
{
    /// <summary>
    /// 
    /// </summary>
    public class OfficePageVM : WPFBase.BaseViewModel
    {
        /// <summary>
        /// 辦公室當前頁面
        /// </summary>
        public OfficePage CurrentPage { get; set; } = OfficePage.Home;

        /// <summary>
        /// 彈跳視窗的當前頁面
        /// </summary>
        public PopupWindows PopupCurrentPage { get; set; }
        /// <summary>
        /// 互聯網帳號
        /// </summary>
        public AccountNumber AccountNumber { get; set; }
        /// <summary>
        /// 專案名稱
        /// </summary>
        public string ProjectName { get; set; } = null;
        /// <summary>
        /// 專案屬性
        /// </summary>
        public ProjectProperty ProjectProperty { get; set; }
        /// <summary>
        /// 專案列表
        /// </summary>
        public ObservableCollection<string> ProjectList { get; set; } = new ObservableCollection<string>();
        /// <summary>
        /// 更新斷面規格，如果用戶沒有建立專案，所以斷面規格列表與材質列表視唯讀狀態。但她又在參數設定內開啟了新件專案或開啟新件專案就觸發此委派。幫用戶更新斷面規格列表與材質列表。
        /// </summary>
        public Action ActionLoadProfile { get; set; } = null;

        /// <summary>
        /// 取得模型資料夾
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static List<string> GetModelDirectory(string path)
        {
            List<string> result = new List<string>();
            //判斷是軟體用的模型資料夾
            foreach (var model in Directory.GetDirectories(path))
            {
                string name = Path.GetFileNameWithoutExtension(model);

                //TODO : 這邊要檢查邏輯，要新增，請參照一開始建置專案的時候，有產生的資料與資料夾。
                if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.Nc}")) // NC 資料夾
                    if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\\{ModelPath.Profile}")) //斷面規格目錄
                        if (File.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\\{ModelPath.ProjectProperty}"))//專案參數
                            if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.DevMaterial}"))//素材3D序列化目錄
                                if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.Dev_Part}"))//單零件的3D圖形目錄
                                    if (File.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.SteelAssembly}"))//構件資料目錄
                                        if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.SteelPart}"))//零件資料序列化的目錄
                                            if (Directory.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.SteelBolts}"))//螺栓資料序列化的目錄
                                                if (File.Exists(FileProfileList($@"{Properties.SofSetting.Default.LoadPath}\{name}")))//螺栓資料序列化的目錄
                                                    if (File.Exists($@"{Properties.SofSetting.Default.LoadPath}\{name}\{ModelPath.Material}"))//模型材質文件
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{DirectoryModel()}\\{ModelPath.Profile}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return FileProjectProperty(DirectoryModel());

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $"{Properties.SofSetting.Default.LoadPath}\\{OfficeViewModel.ProjectName}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Nc}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.DevMaterial}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Dev_Part}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelAssembly}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelPart}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.SteelBolts}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return FileProfileList($@"{DirectoryModel()}");

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
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
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Material}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
        }
        /// <summary>
        /// Tekla 報表文件
        /// </summary>
        /// <returns></returns>
        public static string FileTeklaBom()
        {
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{DirectoryModel()}\{ModelPath.Bom}";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
        }
        //刪除: PartList()
        /// <summary>
        /// 取得零件對應檔案路徑
        /// </summary>
        /// <returns></returns>
        public static string PartList()
        {
            string projectName = OfficeViewModel.ProjectName; //專案名稱

            if (projectName != null)
                return $@"{Properties.SofSetting.Default.LoadPath}\{OfficeViewModel.ProjectName}\part.lis";

            throw new Exception($"沒有專案路徑 (OfficeViewModel.ProjectName is null)");
        }
    }
}
