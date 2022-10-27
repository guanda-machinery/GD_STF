using GD_STD.Enum;
namespace WPFSTD105
{
    /// <summary>
    /// 應用程式頁面
    /// </summary>
    public enum ApplicationPage
    {
        /// <summary>
        /// 上鎖
        /// <para>備註 : <see cref="KEY_HOLE.LOCK"/> 狀態才需使用</para>
        /// </summary>
        Lock,
        /// <summary>
        /// 模式選擇
        /// </summary>
        ModeSelected,
        /// <summary>
        /// 物聯網登入頁面
        /// </summary>
        IoTLogin,
        /// <summary>
        /// 單人模式
        /// </summary>
        StandAlone,
        /// <summary>
        /// 首頁
        /// </summary>
        Home,
        /// <summary>
        /// 加工監控
        /// </summary>
        Monitor,//待刪除
        /// <summary>
        /// 參數設定
        /// </summary>
        SettingPar,
        /// <summary>
        /// 製品設定
        /// </summary>
        ObSetting,//待刪除
        /// <summary>
        /// 左軸主軸模式
        /// </summary>
        LeftAxis,
        /// <summary>
        /// 中軸主軸模式
        /// </summary>
        MiddleAxis,
        /// <summary>
        /// 右軸主軸模式
        /// </summary>
        RightAxis,
        /// <summary>
        /// 警報頁面
        /// </summary>
        Alarm,
        /// <summary>
        /// 入口下壓夾具
        /// </summary>
        EnClampDown,
        /// <summary>
        /// 出口下壓夾具
        /// </summary>
        ExClampDown,
        /// <summary>
        /// 側壓夾具
        /// </summary>
        SideClamp,
        /// <summary>
        /// 左軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        DrillExport_L,
        /// <summary>
        /// 右軸出料口刀庫
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        DrillExport_R,
        /// <summary>
        /// 中軸刀庫
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        DrillMiddle,
        /// <summary>
        /// 左軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料左邊的軸向</remarks>
        DrillEntrance_L,
        /// <summary>
        /// 右軸入料口刀庫
        /// </summary>
        /// <remarks>面對加工機入料右邊的軸向</remarks>
        DrillEntrance_R,
        ///// <summary>
        ///// 入口料架
        ///// </summary>
        //EntranceRack,
        ///// <summary>
        ///// 出口料架
        ///// </summary>
        //ExportRack,
        /// <summary>
        /// 周邊料架
        /// </summary>
        RackOperation,
        /// <summary>
        /// 送料手臂
        /// </summary>
        Hand,
        /// <summary>
        /// 軟體設定
        /// </summary>
        SofSettings,
        /// <summary>
        /// 捲削機
        /// </summary>
        Volume,
        /// <summary>
        /// 排版設定
        /// </summary>
        TypeSetting,//待刪除

        /// <summary>
        /// 新排版設定
        /// </summary>
        MachineTypeSetting,
        /// <summary>
        /// 新製品設定
        /// </summary>
        MachineProductSetting,
        /// <summary>
        /// 新加工監控
        /// </summary>
        MachineMonitor,
        /// <summary>
        /// 機台功能
        /// </summary>
       MachineFunction,
        /// <summary>
        /// 主軸模式
        /// </summary>
        MainSpindleMode

    }
    /// <summary>
    /// 辦公室頁面，由左而右依序排列
    /// </summary>
    public enum OfficePage
    {
        /// <summary>
        /// 空白遮蔽頁面
        /// </summary>
        Home,
        /// <summary>
        /// (舊)製品設定
        /// </summary>
        old_ObSettings,
        /// <summary>
        /// 製品設定
        /// </summary>
        ObSettings,
        /// <summary>
        /// 排版設定
        /// </summary>
        AutoTypeSettings,
        /// <summary>
        ///(舊) 排版設定
        /// </summary>
        old_PartsList,
        /// <summary>
        /// 加工監控
        /// </summary>
        ProcessingMonitor,
        /// <summary>
        /// 廠區監控
        /// </summary>
        WorkingAreaMonitor,
        /// <summary>
        /// 參數設定2
        /// </summary>
        ParameterSettings,
        /// <summary>
        /// 20220711 張燕華 參數設定 - 選單功能
        /// </summary>
        ParameterSettings_FuncList,
    }
    /// <summary>
    /// 20220711 張燕華 參數設定 - 選單功能的所有頁面
    /// </summary>
    public enum ParameterSettingPage
    {
        /// <summary>
        /// 空白遮蔽頁面
        /// </summary>
        Home,
        /// <summary>
        /// 原斷面規格頁面
        /// </summary>
        //SectionSpecificationSetting,
        /// <summary>
        /// 斷面規格目錄
        /// </summary>
        SectionSpecificationMenu,
        /// <summary>
        /// 語言設定頁面
        /// </summary>
        LanguageSetting,
        /// <summary>
        /// 語言設定頁面
        /// </summary>
        SectionSpecificationExcel2Inp,
        /// <summary>
        /// 顯示切割線畫面
        /// </summary>
        SpiltLineSetting,
        /// <summary>
        /// 型鋼加工區域設定
        /// </summary>
        ShapedSteelMachiningAreaSetting,
        /// <summary>
        /// 刀具管理設定
        /// </summary>
        ToolManagerSetting,
        /// <summary>
        /// 軟體版本
        /// </summary>
        SoftwareVersion,
        /// <summary>
        /// 報表LOGO
        /// </summary>
        ReportLogo,
    }
    /// <summary>
    /// 彈跳視窗
    /// </summary>
    public enum PopupWindows
    {
        /// <summary>
        /// 空白頁
        /// </summary>
        FirstPage,
        /// <summary>
        /// 專案管理
        /// </summary>
        ProjectsManager,
        /// <summary>
        /// BOM表屬性設定
        /// </summary>
        BOMSettings,
        /// <summary>
        /// 圖形預覽
        /// </summary>
        GraphBrowser,
    }
}
