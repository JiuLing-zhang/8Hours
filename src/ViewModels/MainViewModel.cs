using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _8Hours.Enums;

namespace _8Hours.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ICommand BtnShowReportCommand { get; set; }
        public ICommand BtnSettingCommand { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand BtnJobStartCommand { get; set; }
        public WindowLocationViewModel WindowLocation { get; set; }

        private double _opacityBtnWork;
        public double OpacityBtnWork
        {
            get => _opacityBtnWork;
            set
            {
                _opacityBtnWork = value;
                OnPropertyChanged();
            }
        }

        private double _opacityBtnStudy;
        public double OpacityBtnStudy
        {
            get => _opacityBtnStudy;
            set
            {
                _opacityBtnStudy = value;
                OnPropertyChanged();
            }
        }

        private double _opacityBtnIdle;
        public double OpacityBtnIdle
        {
            get => _opacityBtnIdle;
            set
            {
                _opacityBtnIdle = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            BtnShowReportCommand = new RelayCommand(parameter => ShowReportClick());
            BtnSettingCommand = new RelayCommand(parameter => SettingClick());
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
            BtnJobStartCommand = new RelayCommand(parameter =>
            {
                if (parameter == null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }
                JobStart(parameter.ToString());
            });
            //Init window size
            WindowLocation = new WindowLocationViewModel()
            {
                Width = 40,
                Height = 120
            };
            InitWindowLocation();

            JobStart(JobTypeEnum.Idle);
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

        private void JobStart(string jobType)
        {
            var result = (JobTypeEnum)Enum.Parse(typeof(JobTypeEnum), jobType);
            JobStart(result);
        }
        private void JobStart(JobTypeEnum jobType)
        {
            SetBtnOpacity(jobType);
        }

        private void SetBtnOpacity(JobTypeEnum jobType)
        {
            double workingOpacity = 1;
            double idlingOpacity = 0.6;
            if (jobType == JobTypeEnum.Work)
            {
                OpacityBtnWork = workingOpacity;
            }
            else
            {
                OpacityBtnWork = idlingOpacity;
            }
            if (jobType == JobTypeEnum.Study)
            {
                OpacityBtnStudy = workingOpacity;
            }
            else
            {
                OpacityBtnStudy = idlingOpacity;
            }
            if (jobType == JobTypeEnum.Idle)
            {
                OpacityBtnIdle = workingOpacity;
            }
            else
            {
                OpacityBtnIdle = idlingOpacity;
            }
        }
    }
}
