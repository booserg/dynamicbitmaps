using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageControl
{
    public class ScrollImageControl : ScrollViewer
    {
        Image image;
        ScaleTransform st;

        public ScrollImageControl()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            image = new Image();

            Content = image;

            image.PreviewMouseLeftButtonDown += Image_PreviewMouseLeftButtonDown;
            image.PreviewMouseLeftButtonUp += Image_PreviewMouseLeftButtonUp;
            image.PreviewMouseMove += Image_PreviewMouseMove;
            image.PreviewMouseWheel += Image_PreviewMouseWheel;
            image.MouseLeave += Image_MouseLeave;

            Binding sourcreBinding = new Binding("Source");
            sourcreBinding.Source = this;
            image.SetBinding(Image.SourceProperty, sourcreBinding);

            Loaded += ScrollImageControl_Loaded;
        }

        private void ScrollImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            var initScale = GetMinScale();

            st = new ScaleTransform(initScale, initScale, 0, 0);
            image.LayoutTransform = st;
        }

        private void Image_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            e.Handled = true;
            var imagePosition = e.MouseDevice.GetPosition(image);
            st.CenterX = imagePosition.X;
            st.CenterY = imagePosition.Y;

            var scrollPosition = e.MouseDevice.GetPosition(this);

            if (e.Delta > 0)
            {
                if (st.ScaleX < 10)
                    st.ScaleX *= 1.1;
                if (st.ScaleY < 10)
                    st.ScaleY *= 1.1;
            }
            else
            {
                var minScale = GetMinScale();
                if (st.ScaleX > minScale)
                {
                    st.ScaleX /= 1.1;
                    st.ScaleY /= 1.1;
                }
            }

            ScrollToHorizontalOffset(st.CenterX * st.ScaleX - (ExtentWidth - ScrollableWidth) / (ViewportWidth / scrollPosition.X));
            ScrollToVerticalOffset(st.CenterY * st.ScaleY - (ExtentHeight - ScrollableHeight) / (ViewportHeight / scrollPosition.Y));
        }

        private double GetMinScale()
        {
            var initXScale = ViewportWidth / 2560;
            var initYScale = ViewportHeight / 2160;

            return initXScale > initYScale ? initYScale : initXScale;
        }

        bool isCaptured = false;
        Point scrollMousePoint = new Point();
        double hOff = 1;
        double vOff = 1;

        private void Image_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isCaptured)
            {
                this.ScrollToHorizontalOffset(hOff + (scrollMousePoint.X - e.GetPosition(this).X));
                this.ScrollToVerticalOffset(vOff + (scrollMousePoint.Y - e.GetPosition(this).Y));
            }
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isCaptured = false;
        }

        private void Image_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isCaptured = false;
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            scrollMousePoint = e.GetPosition(this);
            hOff = this.HorizontalOffset;
            vOff = this.VerticalOffset;
            isCaptured = true;
        }

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ScrollImageControl), new PropertyMetadata(null));
    }
}
