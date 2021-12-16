using System;
using System.Windows.Input;
using _8Hours.Commands;
using _8Hours.Configs;
using _8Hours.Enums;

namespace _8Hours.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public Action Close { get; set; } = null!;
        public ICommand BtnCloseCommand { get; set; }

        private WindowOrientationEnum _windowOrientation;
        public WindowOrientationEnum WindowOrientation
        {
            get => _windowOrientation;
            set
            {
                _windowOrientation = value;
                OnPropertyChanged();

                WindowOrientationChanged(value);
            }
        }

        private JobTypeEnum? _defaultJobType;
        public JobTypeEnum? DefaultJobType
        {
            get => _defaultJobType;
            set
            {
                _defaultJobType = value;
                OnPropertyChanged();

                DefaultJobTypeChanged(value);
            }
        }

        internal SettingViewModel()
        {
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());

            WindowOrientation = GlobalConfig.App.WindowOrientation;
            DefaultJobType = GlobalConfig.App.JobType;
        }
        private void CloseClick()
        {
            Close();
        }

        private void WindowOrientationChanged(WindowOrientationEnum windowOrientation)
        {
            GlobalConfig.App.WindowOrientation = windowOrientation;
            GlobalConfig.SaveConfig();
        }

        private void DefaultJobTypeChanged(JobTypeEnum? defaultJobType)
        {
            GlobalConfig.App.JobType = defaultJobType;
            GlobalConfig.SaveConfig();
        }
    }
}
