using System;
using _8Hours.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _8Hours
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _model = new();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = _model;
            _model.Close = Close;
            _model.OpenReportWindow = () =>
            {
                var reportWindow = new ReportWindow();
                reportWindow.ShowDialog();
            };
            _model.OpenSettingWindow = () =>
            {
                var settingWindow = new SettingWindow();
                settingWindow.ShowDialog();
            };
        }

        private Point _startPoint;
        private void Btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(sender as Button);
        }

        private void Btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                throw new ArgumentException(nameof(btn));
            }
            var currentPoint = e.GetPosition(btn);
            if (e.LeftButton == MouseButtonState.Pressed &&
                btn.IsMouseCaptured &&
                (Math.Abs(currentPoint.X - _startPoint.X) >
                 SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(currentPoint.Y - _startPoint.Y) >
                 SystemParameters.MinimumVerticalDragDistance))
            {
                btn.ReleaseMouseCapture();
                DragMove();
            }
        }
    }
}
