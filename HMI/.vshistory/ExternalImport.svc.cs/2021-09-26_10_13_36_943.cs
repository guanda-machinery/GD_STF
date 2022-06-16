using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Wsdl;
namespace HMI
{
    /// <summary>
    /// 辦公室檔案匯出到加工機台
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ExternalImport : IExternalImport
    {
        /// <inheritdoc/>
        private IImportDuplexCallback _Callback { get => OperationContext.Current.GetCallbackChannel<IImportDuplexCallback>(); }
        /// <inheritdoc/>
        private string SavePath { get; set; }
        /// <inheritdoc/>
        private string ProjectName { get; set; }
        /// <inheritdoc/>
        private int Schedule { get; set; }
        /// <inheritdoc/>
        private int DataLength { get; set; }
        /// <inheritdoc/>
        private FileStream File { get; set; }
        /// <inheritdoc/>
        public void CreateFile(string path, string projectName, int length)
        {
            SavePath = path; //存取路徑
            DataLength = length; //資料長度
            ProjectName = projectName; //專案名稱
            if (!Directory.Exists($@"{SavePath}\{projectName}"))//沒有相同檔案
            {
                File = new FileStream();
                _Callback.CreateFileResult(true);
            }
            else //有相同檔案
            {
                _Callback.CreateFileResult(false);
            }
        }
        /// <inheritdoc/>
        public void ReadDirectory(string path)
        {
            _Callback.ResponseDirectory(Directory.GetDirectories(path)); //回傳目前路徑內部的資料夾給 Client 端
        }
        /// <inheritdoc/>
        public void WriteFile(byte[] data, int writePosition)
        {
            
        }
    }
}
