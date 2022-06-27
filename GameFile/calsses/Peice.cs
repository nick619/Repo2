using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

namespace GameFile.calsses
{
    class Peice
    {
        //Local
        private bool _Alive;
        protected Image _Baseimg; //makes a path to the img
        public Canvas _PlaygroundCanvas; //path to canvas
        public int _Steps = 10;
        protected string _BaseSource; //connects the img to the source
        public Image BaseImage //prop
        {
            get { return _Baseimg; }
            set { _Baseimg = value; }
        }
        public bool Alive //prop
        {
            get { return _Alive; }
        }
        public Peice(string Img)
        {
            if (Img == "ms-appx:///Assets/DinoSprites_Player.gif") //makes sure player
            {
                _Alive = true;
                _BaseSource = Img;
            }
            else
            {
                string EnemyP = "ms-appx:///Assets/DinoSprites_Enemy.gif"; // //Otherwise its a 
                _BaseSource = EnemyP;
                _Alive = true;
            }
        }
        public double GetTop()
        {
            return Canvas.GetTop(_Baseimg);
        }
        public double GetLeft()
        {
            return Canvas.GetLeft(_Baseimg);
        }
        public double GetHeight()
        {
            return _Baseimg.Height;
        }
        public double GetWidth()
        {
            return _Baseimg.Width;
        }
        public void Destroy()
        {
            _PlaygroundCanvas.Children.Remove(_Baseimg);
        }
        public void M_Up()
        {
            if (Canvas.GetTop(_Baseimg) <= _Steps)
            {
                return;
            }

            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg));
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg) - _Steps); //function  of momvent 
        }
        public void M_Down()
        {
            if (Canvas.GetTop(_Baseimg) + _Steps + _Baseimg.Height > _PlaygroundCanvas.ActualHeight)
            {
                return;
            }
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg));
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg) + _Steps);  //function  of momvent 
        }
        public void M_Right()
        {
            if (Canvas.GetLeft(_Baseimg) + _Steps + _Baseimg.Width > _PlaygroundCanvas.ActualWidth) //function  of momvent 
            {
                return;
            }
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg) + _Steps);
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg));
        }
        public void M_Left()
        {
            if (Canvas.GetLeft(_Baseimg) <= _Steps) //function  of momvent 
            {
                return;
            }
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg) - _Steps);
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg));
        }
    }
}
