using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using _8Hours.Commands;
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

        public Action Close { get; set; } = null!;
        public ICommand BtnCloseCommand { get; set; }

        private SeriesCollection _todayCollection = null!;
        public SeriesCollection TodayCollection
        {
            get => _todayCollection;
            set
            {
                _todayCollection = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _yesterdayCollection = null!;
        public SeriesCollection YesterdayCollection
        {
            get => _yesterdayCollection;
            set
            {
                _yesterdayCollection = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _lastWeekCollection = null!;
        public SeriesCollection LastWeekCollection
        {
            get => _lastWeekCollection;
            set
            {
                _lastWeekCollection = value;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _lastMonthCollection = null!;
        public SeriesCollection LastMonthCollection
        {
            get => _lastMonthCollection;
            set
            {
                _lastMonthCollection = value;
                OnPropertyChanged();
            }
        }
        private IList<string> _lastWeekTitle = null!;
        public IList<string> LastWeekTitle
        {
            get => _lastWeekTitle;
            set
            {
                _lastWeekTitle = value;
                OnPropertyChanged();
            }
        }

        private IList<string> _lastMonthTitle = null!;
        public IList<string> LastMonthTitle
        {
            get => _lastMonthTitle;
            set
            {
                _lastMonthTitle = value;
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
                    QueryLastWeekReportData();
                    break;
                case 3:
                    QueryLastMonthReportData();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tabIndex));
            }
        }

        private async Task<List<TimeRecord>> GetJobDetailList(DateTime beginTime, DateTime endTime)
        {
            return await _timeRecordService.GetJobDetail(beginTime, endTime);
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
                    if (x.EndTime != null)
                    {
                        jobTotalHours += x.EndTime.Value.Subtract(x.BeginTime).TotalSeconds / 60 / 60;
                    }

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

        private async void QueryLastWeekReportData()
        {
            BuildLastWeekTitle();
            LastWeekCollection = await QueryTotalDaysTimeRecord(7);
        }

        private async void QueryLastMonthReportData()
        {
            BuildLastMonthTitle();
            LastMonthCollection = await QueryTotalDaysTimeRecord(30);
        }

        private void BuildLastWeekTitle()
        {
            int totalDays = 7;
            LastWeekTitle = new List<string>();
            //这里少取一天的星期值，最后一天使用“今天”表示
            for (int i = totalDays; i > 1; i--)
            {
                string dayOfWeekName =
                    System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DateTime.Now.AddDays(1 - i).DayOfWeek);
                LastWeekTitle.Add(dayOfWeekName);
            }
            LastWeekTitle.Add("今天");
        }

        private void BuildLastMonthTitle()
        {
            int totalDays = 30;
            LastMonthTitle = new List<string>();
            //这里少取一天的日期，最后一天使用“今天”表示
            for (int i = totalDays; i > 1; i--)
            {
                string monthAndDay = DateTime.Now.AddDays(1 - i).ToString("MM-dd");
                LastMonthTitle.Add(monthAndDay);
            }
            LastMonthTitle.Add("今天");
        }
        private async Task<SeriesCollection> QueryTotalDaysTimeRecord(int totalDays)
        {
            DateTime beginTime = Convert.ToDateTime(DateTime.Now.AddDays(1 - totalDays).ToString("yyyy-MM-dd 00:00:00"));
            DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
            var jobDetail = await GetJobDetailList(beginTime, endTime);
            return BuildCartesianChartReport(totalDays, jobDetail);
        }

        private SeriesCollection BuildCartesianChartReport(int totalDays, List<TimeRecord> jobDetail)
        {
            var series = new SeriesCollection();
            foreach (int jobType in Enum.GetValues(typeof(JobTypeEnum)))
            {
                var currentJobType = (JobTypeEnum)jobType;
                var oneTypeJobDetail = GetOneTypeJobDetail(jobDetail, currentJobType);

                var chartValue = new ChartValues<ObservablePoint>();
                for (int i = 0; i < totalDays; i++)
                {
                    string date = DateTime.Now.AddDays(i - totalDays + 1).ToString("yyyy-MM-dd");
                    var dayDetail = oneTypeJobDetail.Where(x => x.EndTime != null && x.EndTime.Value.ToString("yyyy-MM-dd") == date).ToList();
                    double jobTotalHours = 0;
                    dayDetail.ForEach(x =>
                    {
                        if (x.EndTime != null)
                        {
                            jobTotalHours += x.EndTime.Value.Subtract(x.BeginTime).TotalSeconds / 60 / 60;
                        }
                    });
                    chartValue.Add(new ObservablePoint(i, Math.Round(jobTotalHours, 2)));
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
    }
}
