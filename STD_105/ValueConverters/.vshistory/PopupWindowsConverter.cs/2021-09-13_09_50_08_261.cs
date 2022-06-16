using STD_105.Office;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSTD105;
using WPFBase = WPFWindowsBase;

namespace STD_105
{
    class PopupWindowsConverter : WPFBase.BaseValueConverter<PopupWindowsConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WPFBase.BasePage basePage = null;
            switch ((PopupWindows)value)
            {
                case PopupWindows.FirstPage:
                    basePage = new FirstPage();
                    break;
                case PopupWindows.ImportFiles:
                    basePage = new ImportFiles_Office();
                    break;
                case PopupWindows.ProjectsManager:
                    basePage = new ProjectsManager_Office();
                    break;
                default:
                    break;
            }
            //PopupWindowsBase.ActivateLoading();
            //basePage.CloseLoadingWindow += BasePage_LoadingEvent;
            return basePage;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
