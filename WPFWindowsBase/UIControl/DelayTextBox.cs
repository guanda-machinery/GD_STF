using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace WPFWindowsBase.UIControl
{
    /// <summary>
    /// 月曆
    /// </summary>
    public class DelayTextBox : TextBox
    {
        #region 私有
        /// <summary>
        /// 用於延遲
        /// </summary>
        private Timer DelayTimer;
        /// <summary>
        /// 如果true , 觸發<see cref="OnTextChanged"/>
        /// </summary>
        private bool TimerElapsed = false; //如果為true，則觸發OnTextChanged。
        /// <summary>
        /// 如果不是按鍵，則會立即觸發事件
        /// </summary>
        private bool KeysPressed = false;
        /// <summary>
        /// 預設值
        /// </summary>
        private int DELAY_TIME = 250;

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.DelayTimeProperty' 的 XML 註解
        public static readonly DependencyProperty DelayTimeProperty =
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.DelayTimeProperty' 的 XML 註解
            DependencyProperty.Register("DelayTime", typeof(int), typeof(DelayTextBox));
        #endregion
        /// <summary>
        /// 月曆控制項
        /// </summary>
        public DelayTextBox() : base()
        {
            //初始化月曆
            DelayTimer = new Timer(DELAY_TIME);
            DelayTimer.Elapsed += new ElapsedEventHandler(DelayTimer_Elapsed);

            previousTextChangedEventArgs = null;

            AddHandler(TextBox.PreviewKeyDownEvent, new System.Windows.Input.KeyEventHandler(DelayTextBox_PreviewKeyDown));

            PreviousTextValue = String.Empty;
        }
        #region 私有觸發事件
        /// <summary>
        /// 月曆文本框PreviewKeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DelayTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!DelayTimer.Enabled)
                DelayTimer.Enabled = true;
            else
            {
                DelayTimer.Enabled = false;
                DelayTimer.Enabled = true;
            }

            KeysPressed = true;
        }
        private void DelayTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DelayTimer.Enabled = false;//停止計時器。

            TimerElapsed = true;//將計時器設置為true，因此OnTextChange會觸發

            this.Dispatcher.Invoke(new DelayOverHandler(DelayOver), null);//使用invoke來返回UI線程。
        }
        #endregion

        #region 複寫
        /// <summary>
        /// 文字變更時
        /// </summary>
        private TextChangedEventArgs previousTextChangedEventArgs;
        /// <summary>
        /// 先前的文字
        /// </summary>
        public string PreviousTextValue { get; private set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.OnTextChanged(TextChangedEventArgs)' 的 XML 註解
        protected override void OnTextChanged(TextChangedEventArgs e)
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.OnTextChanged(TextChangedEventArgs)' 的 XML 註解
        {
            //如果計時器已過去或除擊鍵以外的其他內容更改了文本
            //觸發base.OnTextChanged
            if (TimerElapsed || !KeysPressed)
            {
                TimerElapsed = false;
                KeysPressed = false;
                base.OnTextChanged(e);

                System.Windows.Data.BindingExpression be = this.GetBindingExpression(TextBox.TextProperty);
                if (be != null && be.Status == System.Windows.Data.BindingStatus.Active) be.UpdateSource();

                PreviousTextValue = Text;
            }

            previousTextChangedEventArgs = e;
        }

        #endregion

        #region 委派
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.DelayOverHandler' 的 XML 註解
        public delegate void DelayOverHandler();
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'DelayTextBox.DelayOverHandler' 的 XML 註解
        #endregion

        private void DelayOver()
        {
            if (previousTextChangedEventArgs != null)
                OnTextChanged(previousTextChangedEventArgs);
        }
    }
}
