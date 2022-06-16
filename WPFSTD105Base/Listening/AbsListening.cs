using System.Threading;
using System.Threading.Tasks;
using WPFSTD105.Listening;

namespace WPFSTD105
{
    /// <summary>
    /// 抽象外部的聆聽者
    /// </summary>

    public abstract class AbsListening : IAbsListening
    {
        /// <summary>
        /// 標準結構
        /// </summary>
        /// <remarks>
        /// <see cref="Sleep"/> 預設 <see cref="LEVEL.MEDIUM"/>
        /// </remarks>
        public AbsListening()
        {
            Mode = false;
            _Thread = new Thread(new ThreadStart(Listening));
            _Thread.IsBackground = true;
        }
        #region 公開屬性
        /// <summary>
        /// 啟用聆聽模式
        /// <para>如果是 true 執行緒會持續聆聽。如果是 false 執行緒會解除掉持續聆聽狀態</para>
        /// </summary>
        public bool Mode
        {
            get
            {
                return _ListeningMode;
            }
            set
            {
                _ListeningMode = value;
                //如果要聆聽就啟動
                if (value == true)
                {
                    StartListening();
                }
            }
        }
        /// <summary>
        /// 聆聽暫止毫秒數
        /// </summary>
        public int Sleep { get; private set; } = (int)LEVEL.MEDIUM;

        #endregion

        #region 公開方法
        /// <summary>
        /// 變更聆聽等級
        /// </summary>
        public void ChangeLevel(LEVEL level)
        {
            Sleep = (int)level;

        }
        /// <summary>
        /// 變更聆聽毫秒數
        /// </summary>
        public void ChangeLevel(int millisecondsTimeout)
        {
            Sleep = millisecondsTimeout;
        }
        /// <summary>
        /// 強制中斷聆聽執行續
        /// </summary>
        public void Interrupt()
        {
            _Thread.Abort();
            ListeningIng = false;
            _Thread = new Thread(new ThreadStart(Listening));
            _Thread.IsBackground= true;
        }
        #endregion

        #region 私有屬性
        /// <summary>
        /// 是否啟用聆聽模式
        /// </summary>
        private bool _ListeningMode { get; set; }
        /// <summary>
        /// 正在聆聽中
        /// </summary>
        private bool ListeningIng { get; set; }
        protected Thread _Thread = null;
        #endregion
        #region 私有方法
        /// <summary>
        /// 聆聽方法
        /// </summary>
        private void Listening()
        {
            //進入聆聽狀態
            while (Mode)
            {
                ReadCodeSysMemory();
                Thread.Sleep(Sleep);
            }
        }
        #endregion

        #region 保護的方法
        /// <summary>
        /// 啟動聆聽
        /// </summary>
        protected virtual void StartListening()
        {
            //如果正在聆聽擋住進入
            if (ListeningIng)
                return;
            else
                ListeningIng = true;

            _Thread.Start();
            //ThreadPool.QueueUserWorkItem(o =>
            //{
            //    thread = Thread.CurrentThread;
            //    //進入聆聽狀態
            //    while (Mode)
            //    {
            //        ReadCodeSysMemory();
            //        Thread.Sleep(Sleep);
            //    }
            //    //已離開停聽狀態
            //    ListeningIng = false;
            //});

        }

        /// <summary>
        /// 分配給處理或作業的時間已經過期時所擲回的例外狀況次數
        /// </summary>
        protected int TimeoutNumber { get; set; }
        /// <summary>
        /// 讀取 CodeSys 的共享記憶體
        /// </summary>
        protected abstract void ReadCodeSysMemory();
        #endregion
    }
}
