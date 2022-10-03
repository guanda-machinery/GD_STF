using DevExpress.Mvvm;
using static WPFSTD105.ViewLocator;
using WPFSTD105;

namespace StickerDialog.ViewModel
{
    public class RegistrationViewModel : ViewModelBase
    {
        //20220711 張燕華 在此設定依賴屬性
    }
}

namespace StickerDialog.View
{
    /// <summary>
    /// ParametersSettings_Office_Language.xaml 的互動邏輯
    /// </summary>
    public partial class RegistrationView
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        //private void cmLanguage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    //20220711 張燕華 combobox被選擇後，由此cmLanguage_SelectionChanged事件函式內執行接下來的動作
        //    //參考網址：https://wpf-tutorial.com/zh/73/%E5%88%97%E8%A1%A8%E6%8E%A7%E4%BB%B6/combobox%E6%8E%A7%E4%BB%B6/
        //}
    }
}