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

        private bool isRunning = false;
        private int imageIndex = 0;

        public MainWindowViewModel() 
        {
            start = new DelegateCommand(() => { isRunning = true; });
            stop = new DelegateCommand(() => { isRunning = false; });

            images = new List<BitmapImage>();
            images.Add(new BitmapImage(new Uri(@"C:\temp\images\1.bmp")));
            images.Add(new BitmapImage(new Uri(@"C:\temp\images\2.bmp")));
            images.Add(new BitmapImage(new Uri(@"C:\temp\images\3.bmp")));

            Timer timer = new Timer(TimerTick, null, 0, 50);
        }

        private void TimerTick(object sender)
        {
            if(isRunning)
            {
                try
                {
                    CurrentImage = images[imageIndex];
                    imageIndex = imageIndex >= images.Count - 1 ? 0 : imageIndex + 1;
                }
                catch(Exception exc)
                {

                }
            }
        }

        private List<BitmapImage> images;
        private List<byte[]> imagesRaw;

        private BitmapImage currentImage;
        public BitmapImage CurrentImage
        {
            get
            {
                return currentImage;
            }
            private set
            {
                currentImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentImage"));
            }
        }
    }
}
