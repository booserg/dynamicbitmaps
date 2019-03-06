using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageControl
{
    public class ImageControl : Image
    {
        ScaleTransform st;
        TranslateTransform tt;

        public ImageControl() : base()
        {
            this.MouseLeftButtonDown += ImageControl_MouseLeftButtonDown;
            this.MouseLeftButtonUp += ImageControl_MouseLeftButtonUp;
            this.MouseMove += ImageControl_MouseMove;
            this.MouseLeave += ImageControl_MouseLeave;

            this.MouseWheel += ImageControl_MouseWheel;

            st = new ScaleTransform(1, 1, 0, 0);
            tt = new TranslateTransform(0, 0);

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(st);
            tg.Children.Add(tt);

            this.LayoutTransform = tg;
        }

        Point downPoint;
        bool isCaptured = false;
        private void ImageControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isCaptured = true;
            downPoint = e.MouseDevice.GetPosition(this);
        }

        private void ImageControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isCaptured = false;
        }

        private void ImageControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (isCaptured)
            {
                var currentPosition = e.MouseDevice.GetPosition(this);
                tt.X = (currentPosition.X - downPoint.X)*st.ScaleX;
                //this.Source.Width * st.ScaleX;
            }
        }

        private void ImageControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            isCaptured = false;
        }

        private void ImageControl_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var position = e.MouseDevice.GetPosition(this);
            st.CenterX = position.X;
            st.CenterY = position.Y;
            if (e.Delta > 0)
            {
                if(st.ScaleX < 10)
                    st.ScaleX += 1;
                if(st.ScaleY < 10)
                    st.ScaleY += 1;
            }
            else
            {
                if(st.ScaleX > 1)
                    st.ScaleX -= 1;
                if(st.ScaleY > 1)
                    st.ScaleY -= 1;
            }
        }
    }
}
