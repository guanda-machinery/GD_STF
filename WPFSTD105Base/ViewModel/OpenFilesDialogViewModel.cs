using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// 開啟檔案
    /// </summary>
    [POCOViewModel]
    public class OpenFilesDialogViewModel
    {
        #region Common properties
        /// <summary>
        /// 篩選器
        /// </summary>
        public virtual string Filter { get; set; }
        /// <summary>
        /// 篩選器索引
        /// </summary>
        public virtual int FilterIndex { get; set; }
        /// <summary>
        /// 標題
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 回傳結果
        /// </summary>
        public virtual bool DialogResult { get; protected set; }
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public virtual string ResultFileName { get; protected set; }
        /// <summary>
        /// 檔案主體
        /// </summary>
        public virtual string FileBody { get; set; }
        #endregion

        #region SaveFileDialogService specific properties
        /// <summary>
        /// 預設副檔名
        /// </summary>
        public virtual string DefaultExt { get; set; }
        /// <summary>
        /// 預設檔名
        /// </summary>
        public virtual string DefaultFileName { get; set; }
        /// <summary>
        /// 覆蓋提示
        /// </summary>
        public virtual bool OverwritePrompt { get; set; }
        #endregion
        /// <summary>
        /// 儲存檔案服務
        /// </summary>
        protected ISaveFileDialogService SaveFileDialogService { get { return this.GetService<ISaveFileDialogService>(); } }
        /// <summary>
        /// 開啟檔案服務
        /// </summary>
        protected IOpenFileDialogService OpenFileDialogService { get { return this.GetService<IOpenFileDialogService>(); } }
        /// <summary>
        /// 開啟
        /// </summary>
        public  void OpenFiles()
        {
            OpenFileDialogService.Filter = Filter;
            OpenFileDialogService.FilterIndex = FilterIndex;
            DialogResult = OpenFileDialogService.ShowDialog();
            if (!DialogResult)
            {
                ResultFileName = string.Empty;
            }
            else
            {
                IFileInfo file = OpenFileDialogService.Files.First();
                ResultFileName = file.DirectoryName + "\\"+ file.Name;
                using (var stream = file.OpenText())
                {
                    FileBody = stream.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 儲存
        /// </summary>
        public void SaveFiles()
        {
            SaveFileDialogService.DefaultExt = DefaultExt;
            SaveFileDialogService.DefaultFileName = DefaultFileName;
            SaveFileDialogService.Filter = Filter;
            SaveFileDialogService.FilterIndex = FilterIndex;
            DialogResult = SaveFileDialogService.ShowDialog();
            if (!DialogResult)
            {
                ResultFileName = string.Empty;
            }
            else
            {
                using (var stream = new StreamWriter(SaveFileDialogService.OpenFile()))
                {
                    stream.Write(FileBody);
                }
            }
        }
    }
}
