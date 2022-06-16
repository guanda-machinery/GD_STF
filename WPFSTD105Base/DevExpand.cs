using DevExpress.Mvvm.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    /// <summary>
    /// Dev 擴充功能
    /// </summary>
    public static class DevExpand
    {
        /// <summary>
        /// 文件夾瀏覽器對話服務
        /// </summary>
        /// <param name="description">標題名稱</param>
        /// <returns></returns>
        public static FolderBrowserDialogService NewFolder(string description)
        {
            FolderBrowserDialogService service = new FolderBrowserDialogService();
            service.Description = "請選擇NC放置的資料夾"; //標題名稱
            service.ShowNewFolderButton = false;//該值指示“新建文件夾”按鈕是否顯示在文件夾瀏覽器對話框
            service.RootFolder = Environment.SpecialFolder.Desktop;
            service.RestorePreviouslySelectedDirectory = true;
            return service;
        }
    }
}
