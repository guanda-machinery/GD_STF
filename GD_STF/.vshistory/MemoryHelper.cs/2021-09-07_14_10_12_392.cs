using GD_STD.Base;
using GD_STD.Enum;
using GD_STD.Phone;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;

namespace GD_STD
{
    /// <summary>
    /// PC 與 Codesys 共享記憶體幫手
    /// </summary>
    public static class MemoryHelper
    {
        #region 常數
        /// <summary>
        /// Codesys 陣列最大長度
        /// </summary>
        public const int MAX_ARRAY_LENGTH = 1000;
        /// <summary>
        /// 左邊刀具庫最大數量
        /// </summary>
        /// <remarks>代表機械的刀具庫最大數量</remarks>
        public const int LEFT_MAX_NUMBER_DRILL = 4;
        /// <summary>
        /// 右邊刀具庫最大數量
        /// </summary>
        /// <remarks>代表機械的刀具庫最大數量</remarks>
        public const int RIGTH_MAX_NUMBER_DRILL = 4;
        /// <summary>
        /// 中間刀具庫最大數量
        /// </summary>
        /// <remarks>代表機械的刀具庫最大數量</remarks>
        public const int MIDDLE_MAX_NUMBER_DRILL = 5;
        /// <summary>
        /// 液壓油系統最大設定數量
        /// </summary>
        public const int HYD_SYSTEM = 20;
        #endregion

        #region 公開屬性
        /// <summary>
        /// 共享 <see cref="ProcessingInfo.DrMiddle"/> 前視圖鑽孔資訊列表給 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="Drill"/></para>
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        public static MemoryMappedFile DrMiddleMemory { get; set; }
        /// <summary>
        ///  共享 <see cref="ProcessingInfo.DrRight"/> 底視圖鑽孔資訊列表給 Codesys
        ///  <para>備註 : <see cref="Type"/> = <see cref="Drill"/></para>
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        public static MemoryMappedFile DrRightMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="ProcessingInfo.DrRight"/> 上視圖鑽孔資訊列表給 Codesys
        /// <para>備註 : <see cref="Type"/> = <see cref="Drill"/></para>
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        public static MemoryMappedFile DrLeftMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="ProcessingInfo.JobInfo"/> 素材加工資訊列表列表給 Codesys
        /// <para>備註 : <see cref="Type"/> = <see cref="JobInfo"/></para>
        /// </summary>
        /// <remarks>
        /// 包含三軸孔位陣列長度、三軸劃線陣列長度、素材規格、長度、三軸陣列起始位置
        /// </remarks>
        public static MemoryMappedFile JobInfoMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="DrillWarehouse.Middle"/> 中軸刀庫資訊列表給 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="DrillSetting"/></para>
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        public static MemoryMappedFile DrSettingMiddleMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="DrillWarehouse.LeftExport"/> 左軸出料口刀庫 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="DrillSetting"/></para>
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        public static MemoryMappedFile DrSettingLeftExportMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="DrillWarehouse.RightExport"/> 右軸出料口刀庫 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="DrillSetting"/></para>
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        public static MemoryMappedFile DrSettingRightExportMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="DrillWarehouse.LeftEntrance"/> 左軸入料口刀庫 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="DrillSetting"/></para>
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        public static MemoryMappedFile DrSettingLeftEntranceMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="DrillWarehouse.RightEntrance"/> 右軸入料口刀庫 Codesys 
        /// <para>備註 : <see cref="Type"/> = <see cref="DrillSetting"/></para>
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        public static MemoryMappedFile DrSettingRightEntranceMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="PanelButton"/> 人機面板 Codesys
        /// <para>備註 : <see cref="Type"/> = <see cref="PanelButton"/></para>
        /// </summary>
        public static MemoryMappedFile PanelMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="AxisInfo"/> 主軸訊息 Codesys
        /// <para>備註 : <see cref="Type"/> = <see cref="AxisInfo"/>只讀不寫</para>
        /// </summary>
        public static MemoryMappedFile AxisInfoMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="OillSystem.HydraulicSystem"/> 訊息 Codesys
        /// <para>備註 <see cref="Type"/> = <see cref="HydraulicSystem"/></para>
        /// </summary>
        public static MemoryMappedFile HydraulicSystemMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="OillSystem.CutOilSystem"/> 訊息 Codesys
        /// <para>備註 <see cref="Type"/> = <see cref="CutOilSystem"/></para>
        /// </summary>
        public static MemoryMappedFile CutSystemMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="OillSystem.LubricantSystem"/> 訊息 Codesys
        /// <para>備註 <see cref="Type"/> = <see cref="LubricantSystem"/></para>
        /// </summary>
        public static MemoryMappedFile LubricantSystemMemory { get; set; }
        /// <summary>
        /// PC 與 Codesys 端主機互相告知狀態的記憶體
        /// <para>備註 <see cref="Type"/> = <see cref="Host"/></para>
        /// </summary>
        public static MemoryMappedFile HostMemory { get; set; }
        /// <summary>
        ///  共享 <see cref="Input"/> 訊息 Codesys
        /// <para>備註 <see cref="Type"/> = <see cref="Input"/></para>
        /// </summary>
        public static MemoryMappedFile InPutMemory { get; set; }
        /// <summary>
        ///  共享 <see cref="Output"/> 訊息 Codesys
        /// <para>備註 <see cref="Type"/> = <see cref="Output"/></para>
        /// </summary>
        public static MemoryMappedFile OutputMemory { get; set; }
        /// <summary>
        /// 共享公司名稱給 Codesys 
        /// </summary>
        public static MemoryMappedFile CompanyMemory { get; set; }
        /// <summary>
        /// 斷電保持 <see cref="Outage"/>
        /// </summary>
        public static MemoryMappedFile OutageMemory { get; set; }
        /// <summary>
        /// 共享 <see cref="MPG"/>
        /// </summary>
        public static MemoryMappedFile MPGMemory { get; set; }
        /// <summary>
        /// 如果 <see cref="ERROR_CODE.Unknown"/> 就來這邊看錯誤代碼
        /// </summary>
        public static MemoryMappedFile UnknownMemory { get; set; }
        /// <summary>
        /// 如果 <see cref="ERROR_CODE.Unknown"/> 就來這邊看錯誤代碼
        /// </summary>
        public static MemoryMappedFile BAVTMemory { get; set; }
        #endregion

        #region 公開方法
        /// <summary>
        /// 開啟共享記憶體
        /// </summary>
        public static void OpenSharedMemory()
        {
            /*PC 與 Codesys 本機資訊*/
            HostMemory = MemoryMappedFile.CreateOrOpen("Host", Marshal.SizeOf(typeof(Host)));
            BAVTMemory = MemoryMappedFile.CreateOrOpen("VBAT", Marshal.SizeOf(typeof(double)));
            /*鑽孔加工列表記憶體共享*/
            DrMiddleMemory = DrMemory("DrMiddle");
            DrRightMemory = DrMemory("DrRight");
            DrLeftMemory = DrMemory("DrLeft");


            /*刀庫設定資訊*/
            DrSettingMiddleMemory = DrSettingMemory("Drill_Setting_Middle", MIDDLE_MAX_NUMBER_DRILL);
            DrSettingLeftExportMemory = DrSettingMemory("Drill_Setting_Left_Export", LEFT_MAX_NUMBER_DRILL);
            DrSettingRightExportMemory = DrSettingMemory("Drill_Setting_Right_Export", RIGTH_MAX_NUMBER_DRILL);
            DrSettingLeftEntranceMemory = DrSettingMemory("Drill_Setting_Left_Entrance", LEFT_MAX_NUMBER_DRILL);
            DrSettingRightEntranceMemory = DrSettingMemory("Drill_Setting_Right_Entrance", RIGTH_MAX_NUMBER_DRILL);

            /*人機面板功能*/
            PanelMemory = MemoryMappedFile.CreateOrOpen("PanelBu", Marshal.SizeOf(typeof(PanelButton)));
            /*主軸訊息*/
            AxisInfoMemory = MemoryMappedFile.CreateOrOpen("AxisInfo", Marshal.SizeOf(typeof(AxisInfo)));

            /*保養設定*/
            HydraulicSystemMemory = MemoryMappedFile.CreateOrOpen("HydraulicSystem", Marshal.SizeOf(typeof(HydraulicSystem)) * HYD_SYSTEM);
            CutSystemMemory = MemoryMappedFile.CreateOrOpen("CutOilSystem", Marshal.SizeOf(typeof(CutOilSystem)));
            LubricantSystemMemory = MemoryMappedFile.CreateOrOpen("LubricantSystem", Marshal.SizeOf(typeof(LubricantSystem)));

            /*IO*/
            InPutMemory = MemoryMappedFile.CreateOrOpen("InputList", Marshal.SizeOf(typeof(Input)));
            OutputMemory = MemoryMappedFile.CreateOrOpen("OutputList", Marshal.SizeOf(typeof(Output)));

            /* Phone 共享記憶體 */
            GD_STD.Phone.MemoryHelper.OpenSharedMemory();

            /*斷電保持共享記憶體*/
            OutageMemory = MemoryMappedFile.CreateOrOpen("DingShouzhong", Marshal.SizeOf(typeof(Outage)));

            /*公司名稱*/
            CompanyMemory = MemoryMappedFile.CreateOrOpen("KissMyBird", Marshal.SizeOf(typeof(byte)) * 20);
            //IsOpen(true);//通知 Codesys 開啟軟體
            /*手搖輪*/
            MPGMemory = MemoryMappedFile.CreateOrOpen("MPG", Marshal.SizeOf(typeof(MPG)));
            //其他錯誤代碼
            UnknownMemory = MemoryMappedFile.CreateOrOpen("vivianan1688", Marshal.SizeOf(typeof(byte)) * 10);
            /*機械校正參數*/
            //MechanicalSettingMemory = MemoryMappedFile.CreateOrOpen("babygirl0000", Marshal.SizeOf(typeof(MechanicalSetting)));
        }

        /// <summary>
        /// 釋放 <see cref="MemoryHelper"/> 內的所有 <see cref="MemoryMappedFile"/> 資源。
        /// </summary>
        public static void Dispose()
        {
            Host host = PCSharedMemory.GetValue<Host>();
            host.PCOpen = false;
            PCSharedMemory.SetValue<Host>(host); //寫入記憶體通知 Codesys 主機， PC 已關閉
            Thread.Sleep(1000); //等待 Codesys 存取用戶設定的參數到斷電保持內再關閉記憶體，否則資料會丟失 

            /*鑽孔加工列表記憶體共享*/
            DrMiddleMemory.Dispose();
            DrRightMemory.Dispose();
            DrLeftMemory.Dispose();

            /*加工資訊*/
            JobInfoMemory.Dispose();

            /*刀庫設定資訊*/
            DrSettingMiddleMemory.Dispose();
            DrSettingLeftExportMemory.Dispose();
            DrSettingRightExportMemory.Dispose();
            DrSettingLeftEntranceMemory.Dispose();
            DrSettingRightEntranceMemory.Dispose();

            /*人機面板功能*/
            PanelMemory.Dispose();

            /*主軸訊息*/
            AxisInfoMemory.Dispose();

            /*IO*/
            InPutMemory.Dispose();
            OutputMemory.Dispose();

            HostMemory.Dispose();
        }
        /// <summary>
        /// 取得在 Codesys Array SizeOf  
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetArraySizeOf(Type type)
        {
            return Marshal.SizeOf(type) * 1000;
        }
        #endregion

        #region 私有方法
        ///// <summary>
        ///// 寫入 PC 端主機開啟狀態
        ///// </summary>
        ///// <param name="value"></param>
        //public static Host IsOpen(bool value)
        //{
        //    /*通知 Codesys 主機狀態*/
        //    Host host = SharedMemory<Host>.GetValue(); //先讀取，因怕會覆蓋掉原本的值
        //    host.PCOpen = value; //改變 PC 主機狀態
        //    SharedMemory<Host>.SetValue(host); //寫入以改變的PC主機狀態

        //    host = SharedMemory<Host>.GetValue(); //確保資料沒有流失所以重新讀取
        //    //如果資料流失了使用遞迴重新傳送
        //    if (host.PCOpen != value)
        //    {
        //       return IsOpen(value);
        //    }
        //    return host;
        //}
        /// <summary>
        /// 創建鑽孔資訊列表記憶體
        /// </summary>
        /// <param name="Name">記憶體名稱</param>
        /// <returns></returns>
        private static MemoryMappedFile DrMemory(string Name)
        {
            return MemoryMappedFile.CreateOrOpen(Name, GetArraySizeOf(typeof(Drill)));
        }
        /// <summary>
        /// 創建刀具庫資訊列表記憶體
        /// </summary>
        /// <param name="Name">記憶體名稱</param>
        /// <param name="drillNumber">鑽頭數量</param>
        /// <returns></returns>
        private static MemoryMappedFile DrSettingMemory(string Name, int drillNumber)
        {
            return MemoryMappedFile.CreateOrOpen(Name, Marshal.SizeOf(typeof(DrillSetting)) * drillNumber);
        }
        #endregion
    }
}