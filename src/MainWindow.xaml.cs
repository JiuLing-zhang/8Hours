using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _8Hours
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            SetWindowLocation();
        }
        private void SetWindowLocation()
        {
            var workArea = System.Windows.SystemParameters.WorkArea;
            this.Left = workArea.Right - this.Width;
            this.Top = (workArea.Bottom - this.Height) / 2;
        }

        private Point startPoint;
        private void Btn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(sender as Button);
        }

        private void Btn_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var btn = sender as Button;
            var currentPoint = e.GetPosition(btn);
            if (e.LeftButton == MouseButtonState.Pressed &&
                btn.IsMouseCaptured &&
                (Math.Abs(currentPoint.X - startPoint.X) >
                    SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(currentPoint.Y - startPoint.Y) >
                    SystemParameters.MinimumVerticalDragDistance))
            {
                // Prevent Click from firing
                btn.ReleaseMouseCapture();
                DragMove();
            }
        }
        private void BtnWork_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BtnWork_Click");
        }
        private void BtnStudy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("study");
        }
        private void BtnIdle_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BtnIdle_Click");
        }
        private void BtnMore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("BtnMore_Click");
        }
    }
}
