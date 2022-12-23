using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;


namespace LanguageSettingDialog.ViewModel
{
    public class LanguageSettingDialogViewModel : ViewModelBase
    {
        RegistrationViewModel registrationViewModel;

        IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        IDialogService DialogService { get { return GetService<IDialogService>(); } }

        public LanguageSettingDialogViewModel()
        {
            registrationViewModel = new RegistrationViewModel();
        }

        [Command]
        public void ShowRegistrationForm()
        {
            UICommand registerCommand = new UICommand(
                id: null,
                caption: "確定",
                command: new DelegateCommand<CancelEventArgs>(
                    cancelArgs => {
                        try
                        {
                            MyExecuteMethod();
                        }
                        catch (Exception e)
                        {
                            MessageBoxService.ShowMessage(e.Message, "Error", MessageButton.OK);
                            cancelArgs.Cancel = true;
                        }
                    }
                ),
                isDefault: true,
                isCancel: false
            );

            UICommand cancelCommand = new UICommand(
                id: MessageBoxResult.Cancel,
                caption: "取消",
                command: null,
                isDefault: false,
                isCancel: true
            );

            UICommand result = DialogService.ShowDialog(
                dialogCommands: new List<UICommand>() { registerCommand, cancelCommand },
                title: "語言設定",
                viewModel: registrationViewModel
            );

            if (result == registerCommand)
            {
                //提示語言設定完成
                WinUIMessageBox.Show(null,
                    $"語言已設定",
                    "通知",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.None,
                    MessageBoxOptions.None,
                     FloatingMode.Window);
            }
        }

        void MyExecuteMethod()
        {
            // 20220711 張燕華 執行語言轉換
            // 待日後建立多國語系時，在此執行系統內部語系的轉換動作
        }
    }
}


namespace LanguageSettingDialog.View
{
    /// <summary>
    /// ParametersSettings_Office_Language.xaml 的互動邏輯
    /// </summary>
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }
    }
}