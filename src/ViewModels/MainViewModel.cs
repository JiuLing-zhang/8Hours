using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _8Hours.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ICommand BtnShowReportCommand { get; set; }
        public ICommand BtnSettingCommand { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand BtnWorkCommand { get; set; }
        public ICommand BtnStudyCommand { get; set; }
        public ICommand BtnIdleCommand { get; set; }

        public WindowLocationViewModel WindowLocation { get; set; }
        public MainViewModel()
        {
            BtnShowReportCommand = new RelayCommand(o => ShowReportClick());
            BtnSettingCommand = new RelayCommand(o => SettingClick());
            BtnCloseCommand = new RelayCommand(o => CloseClick());
            BtnWorkCommand = new RelayCommand(o => WorkClick());
            BtnStudyCommand = new RelayCommand(o => StudyClick());
            BtnIdleCommand = new RelayCommand(o => IdleClick());

            //Init window size
            WindowLocation = new WindowLocationViewModel()
            {
                Width = 40,
                Height = 120
            };
            InitWindowLocation();
        }

        private void InitWindowLocation()
        {
            var workArea = System.Windows.SystemParameters.WorkArea;
            WindowLocation.Left = workArea.Right - WindowLocation.Width;
            WindowLocation.Top = (workArea.Bottom - WindowLocation.Height) / 2;
        }
        private void ShowReportClick()
        {
            MessageBox.Show("BtnShowReport_Click");
        }
        private void SettingClick()
        {
            MessageBox.Show("BtnSetting_Click");
        }
        private void CloseClick()
        {
            Application.Current.Shutdown();
        }
        private void WorkClick()
        {
            MessageBox.Show("BtnWork_Click");
        }
        private void StudyClick()
        {
            MessageBox.Show("BtnStudy_Click");
        }
        private void IdleClick()
        {
            MessageBox.Show("BtnIdle_Click");
        }
    }
}
