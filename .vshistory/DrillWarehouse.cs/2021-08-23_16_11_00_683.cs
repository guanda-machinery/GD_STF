using GD_STD.Base;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;

namespace GD_STD
{
    /// <summary>
    /// 使用者設定的刀具庫  
    /// <para>可序列化的類型</para></summary>
    /// <remarks>
    /// <see cref="Middle"/> Codesys Memory 讀取/寫入
    /// <see cref="LeftExport"/> Codesys Memory 讀取/寫入
    /// <see cref="RightExport"/> Codesys Memory 讀取/寫入
    /// <see cref="LeftEntrance"/> Codesys Memory 讀取/寫入
    /// <see cref="RightEntrance"/> Codesys Memory 讀取/寫入
    /// </remarks>
    [Serializable()]
    [DataContract]
    public class DrillWarehouse : SerializationHelper<DrillWarehouse>, IPCSharedMemory, IDrillWarehouse
    {
        /// <summary>
        /// 初始化刀庫
        /// </summary>
        /// <param name="mecOptional"></param>
        /// <returns></returns>
        public static DrillWarehouse Initialization(Phone.MecOptional mecOptional)
        {
            DrillSetting[] Middle;
            DrillSetting[] LeftExport;
            DrillSetting[] RightExport;
            DrillSetting[] LeftEntrance;
            DrillSetting[] RightEntrance;

            if (mecOptional.Middle)
                Middle = new DrillSetting[5];
            else
                Middle = new DrillSetting[1];

            if (mecOptional.LeftExport)
                LeftExport = new DrillSetting[4];
            else
                LeftExport = new DrillSetting[1];

            if (mecOptional.RightExport)
                RightExport = new DrillSetting[4];
            else
                RightExport = new DrillSetting[1];

            if (mecOptional.LeftEntrance)
                LeftEntrance = new DrillSetting[4];
            else
                LeftEntrance = new DrillSetting[0];

            if (mecOptional.RightEntrance)
                RightEntrance = new DrillSetting[4];
            else
                RightEntrance = new DrillSetting[0];

            return new DrillWarehouse(LeftEntrance, LeftExport, Middle, RightEntrance, RightExport);
        }

        /// <summary>
        /// 預設建構式
        /// </summary>
        public DrillWarehouse(GD_STD.Phone.DrillWarehouse drillWarehouse)
        {
            LeftEntrance = drillWarehouse.LeftEntrance;
            LeftExport = drillWarehouse.LeftExport;
            Middle = drillWarehouse.Middle;
            RightEntrance = drillWarehouse.RightEntrance;
            RightExport = drillWarehouse.RightExport;
        }

        /// <inheritdoc/>
        [DataMember]
        public DrillSetting[] LeftEntrance { get; set; } = new DrillSetting[0];
        /// <inheritdoc/>
        [DataMember]
        public DrillSetting[] LeftExport { get; set; } = new DrillSetting[1];
        /// <inheritdoc/>
        [DataMember]
        public DrillSetting[] Middle { get; set; } = new DrillSetting[1];
        /// <inheritdoc/>
        [DataMember]
        public DrillSetting[] RightEntrance { get; set; } = new DrillSetting[0];
        /// <inheritdoc/>
        [DataMember]
        public DrillSetting[] RightExport { get; set; } = new DrillSetting[1];

        /// <inheritdoc/>
        void ISharedMemory.ReadMemory()
        {
            using (var middle = DrSettingMiddleMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * MIDDLE_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Read))
            using (var leftExport = DrSettingLeftExportMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * LEFT_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Read))
            using (var rightExport = DrSettingRightExportMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * RIGTH_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Read))
            using (var leftEntrance = DrSettingLeftEntranceMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * LEFT_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Read))
            using (var rightEntrance = DrSettingRightEntranceMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * RIGTH_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Read))
            {
                /*讀取刀庫訊息*/
                middle.ReadArray<DrillSetting>(0, Middle, 0, Middle.Length);
                leftExport.ReadArray<DrillSetting>(0, LeftExport, 0, LeftExport.Length);
                rightExport.ReadArray<DrillSetting>(0, RightExport, 0, RightExport.Length);
                leftEntrance.ReadArray<DrillSetting>(0, LeftEntrance, 0, LeftEntrance.Length);
                rightEntrance.ReadArray<DrillSetting>(0, RightEntrance, 0, RightEntrance.Length);
            }
        }
        /// <inheritdoc/>
        void IPCSharedMemory.WriteMemory()
        {
            using (var middle = DrSettingMiddleMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * MIDDLE_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Write))
            using (var leftExport = DrSettingLeftExportMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * LEFT_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Write))
            using (var rightExport = DrSettingRightExportMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * RIGTH_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Write))
            using (var leftEntrance = DrSettingLeftEntranceMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * LEFT_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Write))
            using (var rightEntrance = DrSettingRightEntranceMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(DrillSetting)) * RIGTH_MAX_NUMBER_DRILL, MemoryMappedFileAccess.Write))
            {
                /*寫入刀庫訊息*/
                middle.WriteArray<DrillSetting>(0, Middle, 0, Middle.Length);
                leftExport.WriteArray<DrillSetting>(0, LeftExport, 0, LeftExport.Length);
                rightExport.WriteArray<DrillSetting>(0, RightExport, 0, RightExport.Length);
                leftEntrance.WriteArray<DrillSetting>(0, LeftEntrance, 0, LeftEntrance.Length);
                rightEntrance.WriteArray<DrillSetting>(0, RightEntrance, 0, RightEntrance.Length);
            }
        }
    }
}