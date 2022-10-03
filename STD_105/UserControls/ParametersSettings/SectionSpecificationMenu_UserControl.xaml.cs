using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
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
using WPFSTD105.ViewModel;
using WPFWindowsBase;

namespace STD_105
{
    /// <summary>
    /// ParametersSettings_UserControl.xaml 的互動邏輯
    /// </summary>
    public partial class ParametersSettings_UserControl : BaseUserControl
    {
        public ParametersSettings_UserControl()
        {
            InitializeComponent();
        }

        void ValidateCurrentValue(object sender, GridCellValidationEventArgs e)
        {
  
            DataGridData temp = (this.DataContext as SettingParVM).InsertionData[0];
            if (e.Column.FieldName != nameof(temp.Value))
                return;

            var NumberMin = 0;
            if (double.TryParse((string)e.Value, out var DoubleValue))
            {
                if (Convert.ToDouble(e.Value) < NumberMin)
                {
                    e.IsValid = false;
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    e.ErrorContent = $"數值不可小於0";
                }
                else
                {
                    return;
                }
            }
            else
            {
                e.IsValid = false;
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = $"請輸入數字!";
            }
        }
    }
}
