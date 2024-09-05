using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CobbleApp
{    
    public class Rectangular
    {
        /// <summary>
        /// Everything visual has a rectangle. a circle still fits in one
        /// </summary>
        protected Rectangle Rect;
        public Rectangle Rectangle => Rect;


        public int X => Rectangle.X;
        public int Y => Rectangle.Y;
        public int Width => Rectangle.Width;
        public int Height => Rectangle.Height;
        public int Bottom => Rectangle.Bottom;
        public int Right => Rectangle.Right;

        //convenience items. frequently used calcs may be faster if stored on change
        // (math vs mem acceess i dont actually know if it ever matters)
        public int CenterX => X + ((int)Width / 2);
        public int CenterY => Y + ((int)Height / 2);
        public int HalfX => X / 2;
        public int HalfY => Y / 2;
        public int HalfW => Width / 2;
        public int HalfH => Height / 2;

        public Rectangular(int x, int y, int w, int h) : this(new Rectangle(x, y, w, h))
        {

        }
        public Rectangular(Rectangle rect)
        {
            Set(rect);
        }
        public void Set(int x, int y, int w, int h)
        {
            Set(new Rectangle(x, y, w, h));
        }
        public void SetCopy(Rectangle rectangle)
        {
            Set(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        public void Set(Rectangle rectangle)
        {
            Rect = rectangle;
        }

        public virtual bool IfHit(MouseEventArgs point)
        {
            return IfHit(point.X, point.Y);
        }
        public virtual bool IfHit(Point point)
        {
            return IfHit(point.X, point.Y);
        }
        public virtual bool IfHit(int x, int y)
        {
            if (x >= Rectangle.X && x < Rectangle.Right
                        && y >= Rectangle.Y && y < Rectangle.Bottom)
            {
                return true;
            }
            return false;
        }

    }
}
