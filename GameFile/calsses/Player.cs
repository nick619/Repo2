using GameFile.calsses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameFile
{
    internal class Player : Peice 
    {
        int _startTop = 500;
        int _startLeft = 350;
        int _size = 50;
        public Player(Canvas _playground) : base("ms-appx:///Assets/DinoSprites_Player.gif")
        {
            _PlaygroundCanvas = _playground;
            _Baseimg = new Image();
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSource));

            _Baseimg.Height = _size; // be the size i chose
            _Baseimg.Width = _size;

            Canvas.SetLeft(_Baseimg, _startLeft);
            Canvas.SetTop(_Baseimg, _startTop);

            _PlaygroundCanvas.Children.Add(_Baseimg);
        }
    }
}
