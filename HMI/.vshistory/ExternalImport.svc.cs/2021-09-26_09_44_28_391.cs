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
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼、svc 和組態檔中的類別名稱 "ExternalImport"。
    // 注意: 若要啟動 WCF 測試用戶端以便測試此服務，請在 [方案總管] 中選取 ExternalImport.svc 或 ExternalImport.svc.cs，然後開始偵錯。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ExternalImport : IExternalImport
    {
        /// <inheritdoc/>
        public ImportDuplexCallback Callback { get => OperationContext.Current.GetCallbackChannel<ImportDuplexCallback>(); }
        /// <inheritdoc/>
        public string SavePath { get; set; }
        /// <inheritdoc/>
        public int Schedule { get; set; }
        /// <inheritdoc/>
        public int DataLength { get; set; }
        /// <inheritdoc/>
        public FileStream File { get; set; }
        /// <inheritdoc/>
        public void CreateFile(string path, string projectName, int length)
        {
            SavePath = path;
            DataLength = length;

        }

        public void Directory(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteFile(byte[] data, int writePosition)
        {
            throw new NotImplementedException();
        }
    }
}
