using System;
using System.Windows.Input;
using _8Hours.Commands;

namespace _8Hours.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public Action Close { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        internal SettingViewModel()
        {
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
        }
        private void CloseClick()
        {
            Close();
        }
    }
}
