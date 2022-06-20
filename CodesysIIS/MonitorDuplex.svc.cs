using GD_STD.Phone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Wsdl;
using static GD_STD.ServerLogHelper;
using static GD_STD.MemoryHelper;
using GD_STD;

namespace CodesysIIS
{
    /// <summary>
    /// 機械監控雙向合約
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class MonitorDuplex : IMonitorDuplex
    {
        IMonitorDuplexCallback DuplexCallback { get; set; }

        /// <summary>
        /// 標準建構式
        /// </summary>
        public MonitorDuplex()
        {
            DuplexCallback = OperationContext.Current.GetCallbackChannel<IMonitorDuplexCallback>();
        }
        /// <inheritdoc/>
        public void SetMonitorWorkOffset([WsdlParamOrReturnDocumentation("修改參數")] byte[] value, [WsdlParamOrReturnDocumentation("偏移量")] long offset)
        {
            try
            {
                PCSharedMemory.SetValue<MonitorWork>(offset, value);
                WriteInfo(ReadMemorLog, "寫入 MonitorWork 內容", $"offset : {offset}");
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        [return: WsdlParamOrReturnDocumentation("指定索引的加工資訊")]
        public WorkMaterial GetWorkMaterial([WsdlParamOrReturnDocumentation("陣列索引位置")] int index)
        {
            try
            {
                Type type = typeof(MonitorWork);
                long workOffset = Marshal.OffsetOf(type, nameof(MonitorWork.WorkMaterial)).ToInt64(); //加工陣列記憶體起始位置偏移位置
                int workSize = Marshal.SizeOf(typeof(WorkMaterial)); //WorkMaterial 結構大小 
                long cWorkOffset = workOffset + (workSize * index);
                WorkMaterial work = SharedMemory.GetValue<MonitorWork, WorkMaterial>(cWorkOffset, Marshal.SizeOf(typeof(WorkMaterial)));
                return work;
            }
            catch (Exception ex)
            {
                WriteError(WriteMemorLog, ex);
                throw;
            }
        }
        /// <inheritdoc/>
        public void GetIndex()
        {
            try
            {
                lock (this)
                {
                    Type type = typeof(MonitorWork);
                    long indexOffset = Marshal.OffsetOf(type, nameof(MonitorWork.Index)).ToInt64();//index 記憶體起始偏移位置
                    long countOffset = Marshal.OffsetOf(type, nameof(MonitorWork.Count)).ToInt64();//數量記憶體偏移位置
                    int indexSize = Marshal.SizeOf(typeof(short));
                    List<short> indexList = new List<short>(); //index 儲存列表
                    int count = SharedMemory.GetValue<MonitorWork, short>(countOffset, Marshal.SizeOf(typeof(ushort))); //目前加工陣列的數量
                    for (int i = 0; i < count; i++)
                    {
                        long cIndexOffet = indexOffset +(indexSize * i); //index 陣列偏移位置
                        short cIndex = SharedMemory.GetValue<MonitorWork, short>(cIndexOffet, Marshal.SizeOf(typeof(short)));
                        if (cIndex != -1) //如果不是 -1 
                        {
                            indexList.Add(cIndex);//加入到index 列表
                        }
                        else
                        {
                            DuplexCallback.SendIndex(indexList.ToArray());
                            break;
                        }
                    }
                    Task.Factory.StartNew(() =>
                    {
                        if (indexList.Count > 1)
                        {
                            string info = indexList.Select(el => el.ToString()).Aggregate((str1, str2) => $@"{str1},{str2}");
                            WriteInfo(ReadMemorLog, "讀取 MonitorWork.Index 內容", info);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                WriteError(ReadMemorLog, ex);
                throw;
            }
        }
    }
}
