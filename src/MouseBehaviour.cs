using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace _8Hours
{
    internal class MouseBehaviour : Behavior<UIElement>
    {
        public static readonly DependencyProperty MouseXProperty = DependencyProperty.Register(
            "MouseX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.Register(
            "MouseY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));
        public double MouseX
        {
            get => (double)GetValue(MouseXProperty);
            set => SetValue(MouseXProperty, value);
        }
        public double MouseY
        {
            get => (double)GetValue(MouseYProperty);
            set => SetValue(MouseYProperty, value);
        }

        public static readonly DependencyProperty MouseLeftButtonDownXProperty = DependencyProperty.Register(
            "MouseLeftButtonDownX", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty MouseLeftButtonDownYProperty = DependencyProperty.Register(
            "MouseLeftButtonDownY", typeof(double), typeof(MouseBehaviour), new PropertyMetadata(default(double)));

        public double MouseLeftButtonDownX
        {
            get => (double)GetValue(MouseLeftButtonDownXProperty);
            set => SetValue(MouseLeftButtonDownXProperty, value);
        }
        public double MouseLeftButtonDownY
        {
            get => (double)GetValue(MouseLeftButtonDownYProperty);
            set => SetValue(MouseLeftButtonDownYProperty, value);
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
        }
        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
        }
        private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(AssociatedObject);
            MouseLeftButtonDownX = pos.X;
            MouseLeftButtonDownY = pos.Y;
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;
        }

    }
}
