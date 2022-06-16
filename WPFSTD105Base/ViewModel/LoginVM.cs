using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WPFWindowsBase;
namespace WPFSTD105.ViewModel
{
    /// <summary>
    ///登錄屏幕的查看模型
    /// </summary>
    public class LoginVM : BaseViewModel, IDisposable
    {
        /// <summary>
        /// 計時器
        /// </summary>
        private System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        /// <summary>
        /// 到期時間
        /// </summary>
        private DateTime _Maturity = DateTime.Now.AddMinutes(10);
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// 目前時間
        /// </summary>
        public string Refresh { get; set; }
        public LoginVM()
        {
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            
            Token = CodesysIIS.ReadDuplexMemory.GetToken();
            CodesysIIS.ReadDuplexMemory.StartLogin(true);
            Timer.Start();
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            try
            {
                CodesysIIS.ReadDuplexMemory.StartLogin(true);
            }
            catch (Exception ex)
            {
             
            }
            finally
            {
                DateTime d = DateTime.Now;
                long ticks = _Maturity.Subtract(d).Ticks;
                if (ticks <=0)
                {
                    Token= CodesysIIS.ReadDuplexMemory.GetToken();
                    _Maturity =  DateTime.Now.AddSeconds(30);
                    Refresh =new DateTime(_Maturity.Subtract(d).Ticks).ToString("ss");
                }
                else
                {
                    Refresh =new DateTime(ticks).ToString("ss");
                }
                //StartLogin(true);
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Timer.Tick -= Timer_Click;
            Timer = null;
        }
    }
}
