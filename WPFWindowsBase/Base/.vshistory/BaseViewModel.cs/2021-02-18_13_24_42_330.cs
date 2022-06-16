using PropertyChanged;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace WPFWindowsBase
{
    /// <summary>
    /// 一個基本視圖模型，可根據需要觸發屬性更改事件
    /// </summary>
    [ImplementPropertyChanged]
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// VM 建構式
        /// </summary>
        public BaseViewModel()
        {

        }
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        /// <summary>
        /// 屬性變更事件
        /// </summary>
        /// <param name="name">屬性名稱</param>
        public void OnPropertyChanged(string name)
        {
            if (name != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        /// <summary>
        /// 如果未設置更新標誌，則運行命令。
        ///如果該標誌為true（表示該函數已在運行），則該操作未運行。
        ///如果該標誌為flase（表示沒有運行功能），則運行該動作。
        ///如果操作已運行，則該操作完成後，該標誌將重置為false
        /// </summary>
        /// <param name="updatingFlag"><see cref="bool"/>屬性標誌，定義命令是否已在運行</param>
        /// <param name="action">該命令尚未運行時要執行的操作</param>
        /// <returns></returns>
        protected async Task RunCommand(Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            //檢查flag屬性是否為true(表示該函數已經在運行)
            if (updatingFlag.GetPropertyValue())
                return;

            //將屬性標誌設置為true表示我們正在執行
            updatingFlag.SetPropertyValue(true);

            try
            {
                //執行傳遞的動作
                await action();
            }
            finally
            {
                //現在將屬性標誌設置回false
                updatingFlag.SetPropertyValue(false);
            }
        }
    }
}
