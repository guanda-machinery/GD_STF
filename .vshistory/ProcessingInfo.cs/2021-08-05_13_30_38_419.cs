


using GD_STD.Base;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;

namespace GD_STD
{
    /// <summary>
    /// 給予加工三個軸向的加工資訊列表
    /// <para>可序列化的類型</para></summary>
    /// <remarks>
    ///  <see cref="DrMiddle" />Codesys Memory 讀取/寫入
    /// <see cref="DrRight" />Codesys Memory 讀取/寫入
    /// <see cref="DrLeft" />Codesys Memory 讀取/寫入
    /// </remarks>
    [Serializable()]
    [DataContract]
    public class ProcessingInfo : SerializationHelper<ProcessingInfo>, IPCSharedMemory
    {
        /// <summary>
        /// 預設
        /// </summary>
        public ProcessingInfo()
        {
            DrMiddle = new Drill[1000];
            DrRight = new Drill[1000];
            DrLeft = new Drill[1000];
        }
        //TODO:刪除
        #region 刪除區塊
        /// <summary>
        /// 面對加工機出料中間的軸向
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        [DataMember]
        public Drill[] DrMiddle { get; private set; }
        /// <summary>
        /// 面對加工機出料右邊的軸向
        /// </summary>
        /// <remarks>面對加工機出料右邊的軸向</remarks>
        [DataMember]
        public Drill[] DrRight { get; private set; }
        /// <summary>
        /// 面對加工機出料左邊的軸向
        /// </summary>
        /// <remarks>面對加工機出料左邊的軸向</remarks>
        [DataMember]
        public Drill[] DrLeft { get; private set; }
        /// <summary>
        /// 素材加工訊息
        /// </summary>
        [DataMember]
        public JobInfo JobInfo { get; set; }
        #endregion
        /// <summary>
        /// 存入到<see cref="DrLeft"/>
        /// </summary>
        /// <param name="drills">鑽孔參數</param>
        ///  <remarks>面對加工機出料左邊的軸向</remarks>
        public void SetDrLeft(Drill[] drills)
        {
            DrLeft = drills;
            JobInfo job = JobInfo;
            job.BoltsCountL = (short)drills.Length;
            JobInfo = job;
        }
        /// <summary>
        /// 存入到<see cref="DrRight"/>
        /// </summary>
        /// <param name="drills">鑽孔參數</param>
        ///  <remarks>面對加工機出料右邊的軸向</remarks>
        public void SetDrRight(Drill[] drills)
        {
            DrRight = drills;
            JobInfo job = JobInfo;
            job.BoltsCountR = (short)drills.Length;
            JobInfo = job;
        }
        /// <summary>
        /// 存入到<see cref="DrMiddle"/>
        /// </summary>
        /// <remarks>面對加工機出料中間的軸向</remarks>
        public void SetDrMiddle(Drill[] drills)
        {
            DrMiddle = drills;
            JobInfo job = JobInfo;
            job.BoltsCountM = (short)drills.Length;
            JobInfo = job;
        }
        #region 公開方法
        /// <summary>
        /// 讀取 Codesys 的共享記憶體內
        /// </summary>
        void ISharedMemory.ReadMemory()
        {
            using (var drFront = DrMiddleMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Read))
            using (var drBottom = DrRightMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Read))
            using (var drTop = DrLeftMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Read))
            //using (var jobInfo = JobInfoMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(JobInfo)), MemoryMappedFileAccess.Read))
            {
                this.JobInfo = Base.SharedMemory.GetValue<JobInfo>();
                
                /*讀取鑽孔訊息*/
                drFront.ReadArray<Drill>(0, DrMiddle, 0, DrMiddle.Length);
                drBottom.ReadArray<Drill>(0, DrRight, 0, DrRight.Length);
                drTop.ReadArray<Drill>(0, DrLeft, 0, DrLeft.Length);

                /*讀取素材訊息*/
                //jobInfo.Read<JobInfo>(0, out job);
                //this.JobInfo = job;
            }
        }
        /// <summary>
        /// 寫入 Codesys 的共享記憶體內
        /// </summary>
        void IPCSharedMemory.WriteMemory()
        {
            /*寫入全部訊息*/
            using (var drFront = DrMiddleMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Write))
            using (var drBottom = DrRightMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Write))
            using (var drTop = DrLeftMemory.CreateViewAccessor(0, GetArraySizeOf(typeof(Drill)), MemoryMappedFileAccess.Write))
            //using (var other = JobInfoMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(JobInfo)), MemoryMappedFileAccess.Write))
            {
                SharedMemory.SetValue<JobInfo>(this.JobInfo);

                /*寫入鑽孔訊息*/
                drFront.WriteArray<Drill>(0, DrMiddle, 0, DrMiddle.Length);
                drBottom.WriteArray<Drill>(0, DrRight, 0, DrRight.Length);
                drTop.WriteArray<Drill>(0, DrLeft, 0, DrLeft.Length);

            }
        }
        #endregion
    }
}