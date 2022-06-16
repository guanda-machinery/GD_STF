using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace STD_105
{
    /// <summary>
    /// DigitialClock.xaml 的互動邏輯
    /// </summary>
    public partial class DigitialClock : UserControl
    {
        readonly System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public NowTime nowTime;

        public DigitialClock()
        {
            InitializeComponent();

            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
        }
        private void Timer_Click(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            // 時區切換
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            System.Globalization.Calendar calendar = DateTimeFormatInfo.CurrentInfo.Calendar;
            DataContext = new NowTime
            {
                NowDate = d.Date.ToShortDateString(),
                NowYear = d.ToString(@"yyyy"),
                NowMonth = d.ToString(@"MM"),
                NowTT = d.ToString(@"tt", culture),
                NowWeekOfYear = "Week " + calendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Sunday).ToString(),
                NowDaysOfWeek = d.ToString(@"ddd", culture),
                NowDay = d.ToString(@"dd"),
                NowHour = d.ToString(@"hh"),
                NowMinute = d.ToString(@"mm"),
                NowSecond = d.ToString(@"ss"),
                NowHM = d.ToString(@"hh:mm"),
            };
        }
    }
    public class NowTime
    {
        public string NowYear { get; set; }
        public string NowMonth { get; set; }
        public string NowWeekOfYear { get; set; }
        public string NowDate { get; set; }
        public string NowDaysOfWeek { get; set; }
        public string NowDay { get; set; }
        public string NowHM { get; set; }
        public string NowHour { get; set; }
        public string NowMinute { get; set; }
        public string NowSecond { get; set; }
        public string NowTT { get; set; }
    }
}
