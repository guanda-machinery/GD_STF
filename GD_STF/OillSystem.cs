using GD_STD.Base;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    /// <summary>
    /// 油壓系統參數
    /// </summary>
    /// <remarks>Codesys Memory 讀取/寫入</remarks>
    [DataContract]
    public class OillSystem : IPCSharedMemory, IOillSystem
    {
        /// <summary>
        /// 液壓油系統
        /// </summary>
        [DataMember]
        public HydraulicSystem[] HydraulicSystem { get; set; } = new HydraulicSystem[20];
        /// <summary>
        /// 切消油系統
        /// </summary>
        [DataMember]
        public CutOilSystem CutOilSystem { get; set; }
        /// <summary>
        /// 潤滑油系統
        /// </summary>
        [DataMember]
        public LubricantSystem LubricantSystem { get; set; }


        void ISharedMemory.ReadMemory()
        {
            IOillSystem oillSystem = (IOillSystem)this;
            oillSystem.ReadCut();
            oillSystem.ReadLubricant();
            oillSystem.ReadHydraulic();
        }

        void IPCSharedMemory.WriteMemory()
        {
            IOillSystem oillSystem = (IOillSystem)this;
            oillSystem.WriteCut();
            oillSystem.WriteHydraulic();
            oillSystem.WriteLubricant();
        }
        /// <summary>
        /// 寫入液壓油系統參數給 Codesys 
        /// </summary>
        void IOillSystem.WriteHydraulic()
        {
            using (var hydraulic = HydraulicSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(HydraulicSystem)) * HYD_SYSTEM, MemoryMappedFileAccess.Write))
            {
                /*寫入訊息*/
                hydraulic.WriteArray<HydraulicSystem>(0, HydraulicSystem, 0, HydraulicSystem.Length);
            }
        }
        /// <summary>
        /// 讀取 Codesys 液壓油系統參數
        /// </summary>
        public void ReadHydraulic()
        {
            using (var hydraulic = HydraulicSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(HydraulicSystem)) * HYD_SYSTEM, MemoryMappedFileAccess.Read))
            {
                /*寫入訊息*/
                hydraulic.ReadArray<HydraulicSystem>(0, HydraulicSystem, 0, HydraulicSystem.Length);
            }
        }
        /// <summary>
        /// 寫入潤滑油系統參數給 Codesys 
        /// </summary>
        void IOillSystem.WriteLubricant()
        {
            using (var Lubricant = LubricantSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(LubricantSystem)), MemoryMappedFileAccess.Write))
            {
                LubricantSystem lub = this.LubricantSystem;

                Lubricant.Write<LubricantSystem>(0, ref lub);
            }
        }
        /// <summary>
        /// 讀取 Codesys 滑道油系統參數
        /// </summary>
        void IOillSystem.ReadLubricant()
        {
            using (var Lubricant = LubricantSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(LubricantSystem)), MemoryMappedFileAccess.Read))
            {
                LubricantSystem lub;

                Lubricant.Read<LubricantSystem>(0, out lub);

                this.LubricantSystem = lub;
            }
        }
        /// <summary>
        /// 寫入切削系統參數給 Codesys 
        /// </summary>
        void IOillSystem.WriteCut()
        {
            using (var Cut = CutSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(CutOilSystem)), MemoryMappedFileAccess.Write))
            {
                CutOilSystem cut = this.CutOilSystem;

                /*寫入訊息*/
                Cut.Write<CutOilSystem>(0, ref cut);

            }
        }
        /// <summary>
        /// 讀取 Codesys 切削油系統參數
        /// </summary>
        void IOillSystem.ReadCut()
        {
            using (var Cut = CutSystemMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(CutOilSystem)), MemoryMappedFileAccess.Read))
            {
                CutOilSystem cut;

                /*讀取訊息*/
                Cut.Read<CutOilSystem>(0, out cut);

                this.CutOilSystem = cut;
            }
        }
    }
}
