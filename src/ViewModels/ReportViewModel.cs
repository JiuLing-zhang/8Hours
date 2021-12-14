using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _8Hours.Enums;
using _8Hours.Models;
using _8Hours.Services;
using JiuLing.CommonLibs.ExtensionMethods;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace _8Hours.ViewModels
{
    internal class ReportViewModel : ViewModelBase
    {
        public Action Close { get; set; }
        public ICommand BtnCloseCommand { get; set; }
        public ICommand RadioConditionIsCheckedCommand { get; set; }

        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        private readonly TimeRecordService _timeRecordService;
        internal ReportViewModel()
        {
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
            RadioConditionIsCheckedCommand = new RelayCommand(parameter => ConditionIsChecked(Convert.ToInt32(parameter)));
            _timeRecordService = new TimeRecordService();

            ConditionIsChecked(1);
        }

        private void CloseClick()
        {
            Close();
        }

        private async void ConditionIsChecked(int dayInterval)
        {
            DateTime beginTime = Convert.ToDateTime(DateTime.Now.AddDays(0 - dayInterval).ToString("yyyy-MM-dd 00:00:00"));
            DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            var jobDetail = await GetJobDetailList(beginTime, endTime);

            SeriesCollection = BuildJobReport(dayInterval, jobDetail);
        }

        private async Task<List<TimeRecord>> GetJobDetailList(DateTime beginTime, DateTime endTime)
        {
            return await _timeRecordService.GetJobDetail(beginTime, endTime);
        }

        private SeriesCollection BuildJobReport(int dayInterval, List<TimeRecord> jobDetail)
        {
            var series = new SeriesCollection();
            foreach (int jobType in Enum.GetValues(typeof(JobTypeEnum)))
            {
                var currentJobType = (JobTypeEnum)jobType;
                var oneTypeJobDetail = GetOneTypeJobDetail(jobDetail, currentJobType);

                var chartValue = new ChartValues<ObservablePoint>();
                for (int i = 0; i < dayInterval; i++)
                {
                    string date = DateTime.Now.AddDays(0 - i).ToString("yyyy-MM-dd");
                    var dayDetail = oneTypeJobDetail.Where(x => x.EndTime.Value.ToString("yyyy-MM-dd") == date).ToList();
                    double jobTotalMinutes = 0;
                    dayDetail.ForEach(x =>
                    {
                        jobTotalMinutes += x.EndTime.Value.Subtract(x.BeginTime).TotalSeconds / 60;
                    });
                    chartValue.Add(new ObservablePoint(i, Convert.ToInt32(jobTotalMinutes)));
                }

                series.Add(new LineSeries()
                {
                    Title = currentJobType.GetDescription(),
                    PointGeometrySize = 10,
                    Values = chartValue
                });
            }

            return series;
        }

        private List<TimeRecord> GetOneTypeJobDetail(List<TimeRecord> jobDetail, JobTypeEnum jobType)
        {
            return jobDetail.Where(x => x.JobType == jobType).ToList();
        }
    }
}
