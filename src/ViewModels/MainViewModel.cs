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
        public ICommand PreviewMouseMoveCommand { get; set; }
        public WindowLocationViewModel WindowLocation { get; set; }
        /// <summary>
        /// mouse point
        /// </summary>
        public MousePointViewModel ButtonMousePoint { get; set; }
        /// <summary>
        /// mouse point when left button down
        /// </summary>
        public MousePointViewModel ButtonLeftDownMousePoint { get; set; }
        public MainViewModel()
        {
            ButtonMousePoint = new MousePointViewModel();
            ButtonLeftDownMousePoint = new MousePointViewModel();

            BtnShowReportCommand = new RelayCommand(x => ShowReportClick());
            BtnSettingCommand = new RelayCommand(x => SettingClick());
            BtnCloseCommand = new RelayCommand(x => CloseClick());
            BtnWorkCommand = new RelayCommand(x => WorkClick());
            BtnStudyCommand = new RelayCommand(x => StudyClick());
            BtnIdleCommand = new RelayCommand(x => IdleClick());
            PreviewMouseMoveCommand = new RelayCommand(x => BtnPreviewMouseMove(x as MouseEventArgs));
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

        private void BtnPreviewMouseMove(MouseEventArgs? e)
        {
            Window window = App.Current.MainWindow;
            if (window != null)
            {
                if (Mouse.LeftButton != MouseButtonState.Pressed)
                {
                    return;
                }

                if (Math.Abs(ButtonMousePoint.X - ButtonLeftDownMousePoint.X) <= SystemParameters.MinimumHorizontalDragDistance)
                {
                    return;
                }
                if (Math.Abs(ButtonMousePoint.Y - ButtonLeftDownMousePoint.Y) <= SystemParameters.MinimumHorizontalDragDistance)
                {
                    return;
                }
                window.DragMove();
            }
        }
    }
}
