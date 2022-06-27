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
    internal class Enemy : Peice
    {
        int _size = 50;
        int _startTop = 300;
        int _startLeft = 200;
        int _id = 0;
        public Enemy(Canvas playground, int startTop, int startLeft, int id) : base("ms-appx:///Assets/DinoSprites_Enemy.gif")
        {
            //LOCAL PROPS
            _PlaygroundCanvas = playground;
            _startTop = startTop;
            _startLeft = startLeft;
            _id = id;

            //Main props
            _Baseimg = new Image();
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSource));
            _Baseimg.Height = _size;
            _Baseimg.Width = _size;

            Canvas.SetLeft(_Baseimg, _startLeft);
            Canvas.SetTop(_Baseimg, _startTop);

            _PlaygroundCanvas.Children.Add(_Baseimg);
        }
        public int GetId()
        {
            return this._id;
        }
        public void killBaddie()
        {
            _PlaygroundCanvas.Children.Remove(_Baseimg);
        }
    }
}
