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
        private int _tabReportSelected;
        public int TabReportSelected
        {
            get => _tabReportSelected;
            set
            {
                _tabReportSelected = value;
                OnPropertyChanged();
                TabReportSelectionChanged(value);
            }
        }
        public Action Close { get; set; }
        public ICommand BtnCloseCommand { get; set; }

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

        private SeriesCollection _todayCollection;
        public SeriesCollection TodayCollection
        {
            get => _todayCollection;
            set
            {
                _todayCollection = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _yesterdayCollection;
        public SeriesCollection YesterdayCollection
        {
            get => _yesterdayCollection;
            set
            {
                _yesterdayCollection = value;
                OnPropertyChanged();
            }
        }

        private readonly TimeRecordService _timeRecordService;
        internal ReportViewModel()
        {
            BtnCloseCommand = new RelayCommand(parameter => CloseClick());
            _timeRecordService = new TimeRecordService();

            TabReportSelected = 0;
        }

        private void CloseClick()
        {
            Close();
        }

        private void TabReportSelectionChanged(int tabIndex)
        {
            switch (tabIndex)
            {
                case 0:
                    QueryTodayReportData();
                    break;
                case 1:
                    QueryYesterdayReportData();
                    break;
                case 2:
                    ConditionIsChecked(7);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tabIndex));
            }
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
                    double jobTotalHours = 0;
                    dayDetail.ForEach(x =>
                    {
                        jobTotalHours += x.EndTime.Value.Subtract(x.BeginTime).TotalSeconds / 60 / 60;
                    });
                    chartValue.Add(new ObservablePoint(i, Convert.ToInt32(jobTotalHours)));
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

        private async void QueryTodayReportData()
        {
            var jobDetail = await QueryOneDayTimeRecord(DateTime.Now);
            TodayCollection = BuildPieChartReport(jobDetail);
        }
        private async void QueryYesterdayReportData()
        {
            var jobDetail = await QueryOneDayTimeRecord(DateTime.Now.AddDays(-1));
            YesterdayCollection = BuildPieChartReport(jobDetail);
        }

        private async Task<List<TimeRecord>> QueryOneDayTimeRecord(DateTime date)
        {
            DateTime beginTime = Convert.ToDateTime(date.ToString("yyyy-MM-dd 00:00:00"));
            DateTime endTime = Convert.ToDateTime(date.ToString("yyyy-MM-dd 23:59:59"));
            return await GetJobDetailList(beginTime, endTime);
        }

        /// <summary>
        /// 饼状图
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <returns></returns>
        private SeriesCollection BuildPieChartReport(List<TimeRecord> jobDetail)
        {
            var series = new SeriesCollection();
            foreach (int jobType in Enum.GetValues(typeof(JobTypeEnum)))
            {
                var currentJobType = (JobTypeEnum)jobType;
                var oneTypeJobDetail = GetOneTypeJobDetail(jobDetail, currentJobType);

                double jobTotalHours = 0;
                oneTypeJobDetail.ForEach(x =>
                {
                    jobTotalHours += x.EndTime.Value.Subtract(x.BeginTime).TotalSeconds / 60 / 60;

                });
                series.Add(new PieSeries()
                {
                    Title = currentJobType.GetDescription(),
                    Values = new ChartValues<ObservableValue> { new ObservableValue(Math.Round(jobTotalHours, 2)) },
                    DataLabels = true,
                    LabelPoint = chartPoint => $"{chartPoint.Y}小时 ({chartPoint.Participation:P})"
                });
            }
            return series;
        }
    }
}
