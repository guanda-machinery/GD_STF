using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
namespace WPFWindowsBase
{
    /// <summary>
    /// 應用程序的ioc容器
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// ioc容器的內核
        /// </summary>
        public static IKernel Kernel { get; private set; } = new StandardKernel();
        /// <summary>
        /// 從IoC獲取指定類型的服務
        /// </summary>
        /// <typeparam name="T">獲得的類型</typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Kernel.Get<T>();
        }
        /// <summary>
        ///設置IoC容器，綁定所有必需的信息並準備使用
        ///<para>注意：必須在應用程序啟動後立即調用，以確保所有可以找到服務</para> 
        /// </summary>
        public static void Setup()
        {
            //綁定所有必需的ViewModel
            BindViewModel();
        }
        /// <summary>
        /// 綁定所有單例視圖模型
        /// </summary>
        private static void BindViewModel()
        {
            //綁定到Application ViewModel 的單個實例
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }
    }
}
