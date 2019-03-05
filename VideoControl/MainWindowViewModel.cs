using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VideoControl
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DelegateCommand start;
        public ICommand Start => start;

        private DelegateCommand stop;

        public ICommand Stop => stop;

        public event PropertyChangedEventHandler PropertyChanged;

        private int imageIndex = 0;

        Timer timer;

        public MainWindowViewModel() 
        {
            start = new DelegateCommand(() => { timer = new Timer(TimerTick, null, 0, 100); });
            stop = new DelegateCommand(() => 
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            });

            rawImages = new List<byte[]>();
            
            rawImages.Add(Convert(new BitmapImage(new Uri(@"C:\temp\images\1.bmp"))));
            rawImages.Add(Convert(new BitmapImage(new Uri(@"C:\temp\images\2.bmp"))));
            rawImages.Add(Convert(new BitmapImage(new Uri(@"C:\temp\images\3.bmp"))));

            CurrentImage = new WriteableBitmap(new BitmapImage(new Uri(@"C:\temp\images\1.bmp")));
        }

        private byte[] Convert(BitmapImage image)
        {
            var stride = ((int)image.Width) * 16 + ((int)image.Width) % 4;
            byte[] bits = new byte[((int)image.Height) * stride];
            image.CopyPixels(bits, stride, 0);
            return bits;
        }

        private void TimerTick(object sender)
        {
                try
                {
                    //logo.Freeze();
                    //Action action = delegate { image1.Source = logo; };
                    //image1.Dispatcher.Invoke(action);

                    CurrentImage.Dispatcher.Invoke(() => { CurrentImage.WritePixels(new System.Windows.Int32Rect(0, 0, 2560, 2160), rawImages[imageIndex], 2560 * 16 + 2560 % 4, 0); });
                    
                    imageIndex = imageIndex >= rawImages.Count - 1 ? 0 : imageIndex + 1;
                }
                catch(Exception exc)
                {

                }
        }

        private List<byte[]> rawImages;

        public WriteableBitmap CurrentImage { get; private set; }
    }
}
