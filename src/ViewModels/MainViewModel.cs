﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using _8Hours.Commands;
using _8Hours.Configs;
using _8Hours.Enums;
using _8Hours.Models;
using _8Hours.Services;
using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.Toolkit.Uwp.Notifications;

namespace _8Hours.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public Action Close { get; set; } = null!;
        public Action OpenReportWindow { get; set; } = null!;
        public Action OpenSettingWindow { get; set; } = null!;
        public ICommand BtnShowReportCommand { get; set; }
        public ICommand BtnSettingCommand { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand BtnJobStartCommand { get; set; }
        public ICommand BtnJobStopCommand { get; set; }
        public ICommand WindowClosingCommand { get; set; }

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

        private double _opacityBtnStop;
        public double OpacityBtnStop
        {
            get => _opacityBtnStop;
            set
            {
                _opacityBtnStop = value;
                OnPropertyChanged();
            }
        }

        private string _windowOrientation = null!;
        public string WindowOrientation
        {
            get => _windowOrientation;
            set
            {
                _windowOrientation = value;
                OnPropertyChanged();
            }
        }

        private readonly TimeRecordService _timeRecordService;
        public MainViewModel()
        {
            _timeRecordService = new TimeRecordService();

            BtnShowReportCommand = new RelayCommand(parameter => ShowReportClick());
            BtnSettingCommand = new RelayCommand(parameter => SettingClick());
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
            BtnJobStartCommand = new RelayCommand(parameter =>
            {
                if (parameter == null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }
                string parameterString = parameter.ToString() ?? throw new ArgumentNullException(nameof(parameter));
                if (!Enum.TryParse(parameterString, out JobTypeEnum jobType))
                {
                    throw new ArgumentNullException(nameof(parameter));
                }
                JobStart(jobType);

            });
            BtnJobStopCommand = new RelayCommand(parameter => JobStop());
            WindowClosingCommand = new RelayCommand(async parameter => await WindowClosing());

            WindowLocation = new WindowLocationViewModel();

            WindowOrientation = GlobalConfig.App.WindowOrientation.ToString();
            InitWindowSize();
            InitWindowLocation();
            InitJobType();
        }

        private void InitWindowSize()
        {
            switch (GlobalConfig.App.WindowOrientation)
            {
                case WindowOrientationEnum.Horizontal:
                    WindowLocation.Width = 160;
                    WindowLocation.Height = 40;
                    break;
                case WindowOrientationEnum.Vertical:
                    WindowLocation.Width = 40;
                    WindowLocation.Height = 160;
                    break;
            }
        }
        private void InitWindowLocation()
        {
            var workArea = System.Windows.SystemParameters.WorkArea;
            switch (GlobalConfig.App.WindowOrientation)
            {
                case WindowOrientationEnum.Horizontal:
                    WindowLocation.Left = (workArea.Right - WindowLocation.Width) / 2;
                    WindowLocation.Top = 0;
                    break;
                case WindowOrientationEnum.Vertical:
                    WindowLocation.Left = workArea.Right - WindowLocation.Width;
                    WindowLocation.Top = (workArea.Bottom - WindowLocation.Height) / 2;
                    break;
            }
        }

        private void InitJobType()
        {
            if (GlobalConfig.App.JobType == null)
            {
                JobStop();
                return;
            }
            JobStart(GlobalConfig.App.JobType.Value);
        }
        private void ShowReportClick()
        {
            OpenReportWindow();
        }
        private void SettingClick()
        {
            OpenSettingWindow();
        }
        private void CloseClick()
        {
            Close();
        }

        private async void JobStart(JobTypeEnum jobType)
        {
            SetActiveStartOpacity(jobType);
            await _timeRecordService.JobStop();
            var item = new TimeRecord()
            {
                JobType = jobType,
                BeginTime = DateTime.Now,
                EndTime = null
            };
            await _timeRecordService.JobStart(item);

            new ToastContentBuilder()
                .AddText($"开始{jobType.GetDescription()}")
                .Show();
        }

        private async void JobStop()
        {
            SetActiveStopOpacity();
            await _timeRecordService.JobStop();
            new ToastContentBuilder()
                .AddText($"停止记录")
                .Show();
        }

        private void SetActiveStartOpacity(JobTypeEnum jobType)
        {
            double activeOpacity = 0.4;
            double InactiveOpacity = 1;
            OpacityBtnStop = InactiveOpacity;
            if (jobType == JobTypeEnum.Work)
            {
                OpacityBtnWork = activeOpacity;
            }
            else
            {
                OpacityBtnWork = InactiveOpacity;
            }
            if (jobType == JobTypeEnum.Study)
            {
                OpacityBtnStudy = activeOpacity;
            }
            else
            {
                OpacityBtnStudy = InactiveOpacity;
            }
            if (jobType == JobTypeEnum.Idle)
            {
                OpacityBtnIdle = activeOpacity;
            }
            else
            {
                OpacityBtnIdle = InactiveOpacity;
            }
        }
        private void SetActiveStopOpacity()
        {
            double activeOpacity = 0.4;
            double InactiveOpacity = 1;

            OpacityBtnWork = InactiveOpacity;
            OpacityBtnStudy = InactiveOpacity;
            OpacityBtnIdle = InactiveOpacity;
            OpacityBtnStop = activeOpacity;
        }

        private async Task WindowClosing()
        {
            await _timeRecordService.JobStop();
        }
    }
}
