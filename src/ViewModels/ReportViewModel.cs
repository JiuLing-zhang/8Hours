using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _8Hours.ViewModels
{
    internal class ReportViewModel : ViewModelBase
    {
        public Action Close { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand RadioConditionIsCheckedCommand { get; set; }
        internal ReportViewModel()
        {
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
            RadioConditionIsCheckedCommand = new RelayCommand(parameter => ConditionIsChecked());
        }

        private void CloseClick()
        {
            Close();
        }

        private async void ConditionIsChecked()
        {
            throw new NotImplementedException();
        }
    }
}
