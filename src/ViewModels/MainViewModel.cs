﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _8Hours.Enums;
using _8Hours.Models;
using _8Hours.Services;
using JiuLing.CommonLibs.ExtensionMethods;
using Microsoft.Toolkit.Uwp.Notifications;

namespace _8Hours.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public Action Close { get; set; }
        public Action OpenReportWindow { get; set; }
        public Action OpenSettingWindow { get; set; }
        public ICommand BtnShowReportCommand { get; set; }
        public ICommand BtnSettingCommand { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand BtnJobStartCommand { get; set; }
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

        private readonly TimeRecordService _timeRecordService;
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
            WindowClosingCommand = new RelayCommand(parameter => WindowClosing());

            //Init window size
            WindowLocation = new WindowLocationViewModel()
            {
                Width = 40,
                Height = 120
            };
            InitWindowLocation();

            _timeRecordService = new TimeRecordService();
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

        private void JobStart(string jobType)
        {
            var result = (JobTypeEnum)Enum.Parse(typeof(JobTypeEnum), jobType);
            JobStart(result);
        }
        private async void JobStart(JobTypeEnum jobType)
        {
            SetBtnOpacity(jobType);
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

        private void SetBtnOpacity(JobTypeEnum jobType)
        {
            double workingOpacity = 0.4;
            double idlingOpacity = 1;
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

        private async Task WindowClosing()
        {
            await _timeRecordService.JobStop();
        }
    }
}
