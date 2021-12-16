using System;
using System.Windows.Input;
using _8Hours.Commands;
using _8Hours.Enums;

namespace _8Hours.ViewModels
{
    internal class SettingViewModel : ViewModelBase
    {
        public Action Close { get; set; }
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

        }
        private void CloseClick()
        {
            Close();
        }

        private void WindowOrientationChanged(WindowOrientationEnum windowOrientation)
        {
            Console.WriteLine($"WindowOrientationChanged You selected {windowOrientation}");
        }

        private void DefaultJobTypeChanged(JobTypeEnum? defaultJobType)
        {
            Console.WriteLine($"DefaultJobType You selected {defaultJobType}");
        }
    }
}
