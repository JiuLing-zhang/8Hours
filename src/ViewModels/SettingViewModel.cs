using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
