using DevExpress.Xpf.Grid;
using System;
using System.Web.Configuration;
using WPFSTD105.ViewModel;
using WPFWindowsBase;

namespace STD_105.Office
{
    /// <summary>
    /// ParametersSettings_Office.xaml 的互動邏輯
    /// </summary>
    public partial class SectionSpecificationMenu : BasePage<SettingParVM>
    {
        public SectionSpecificationMenu()
        {
            InitializeComponent();
        }

        /*void ValidateCurrentValue(object sender, GridCellValidationEventArgs e)
        {
            DataGridData temp = ViewModel.InsertionData[0];
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
        }*/
    }
}
