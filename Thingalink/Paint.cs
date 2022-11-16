using System.Drawing;

namespace Thingalink
{
    //undefined visuality
    public class PaintValue : Mote
    {

    }
    
    public class ColorValue : PaintValue
    {
        private new Color Value;
        public Color Color => Value;

        //set requires brush clear(for change)
        public virtual void Set(Color c)
        {
            //any override will still need base call
            Value = c;
        }
    }
    public class Paint : ColorValue
    {
        public delegate void PassPaint(Paint thing);
        public delegate Paint Morph(Paint paint);
        public delegate int Feeder();

        public static Feeder AlphaFeeder;

        public static int NewAlpha()
        {
            return AlphaFeeder?.Invoke() ?? 255;
        }

        public int Thick = 2;

        private Pen LastPen;
        public Pen Pen => LastPen ?? (LastPen = new Pen(Color, Thick));
        public Pen GetPen(int thick)
        {
            if (thick != Thick)
            {
                Thick = thick;
                LastPen = new Pen(Color, Thick);
            }
            return Pen;
        }

        public bool MostR()
        {
            if (Color.R > Color.G && Color.R >= Color.B)
                return true;
            else if (Color.R == Color.G && Color.R == Color.B && RandomAccess.Percent(35))
                return true;
            else
                return false;
        }
        public bool MostG()
        {
            if (Color.G >= Color.R && Color.G > Color.B)
                return true;
            else if (Color.R == Color.G && Color.R == Color.B && RandomAccess.Percent(50))
                return true;
            else
                return false;
        }
        public bool MostB()
        {
            if (Color.B >= Color.G && Color.B > Color.R)
                return true;
            else if (Color.R == Color.G && Color.R == Color.B)
                return true;
            else
                return false;
        }
        private SolidBrush LastBrush;
        public Brush Brush => LastBrush ?? (LastBrush = new SolidBrush(Color));

        public Paint(int a, int r, int g, int b, int thick = 2) : this(Color.FromArgb(Conform(a), Conform(r), Conform(g), Conform(b)), thick)
        {
            Thick = thick;
        }
        public Paint(Color o, int thick = 2) //: base(o)
        {

            Set(o);
            Thick = thick;
        }

        public static int Conform(int v)
        {
            if (v < 0)
            {
                return 0;
            }
            if (v > 255)
            {
                return 255;
            }
            return v;
        }

        //internal Brush MakeBrush(Color paint)
        //{
        //    return new SolidBrush(paint);
        //}
        public void Free()
        {
            LastPen = null;
            LastBrush = null;
        }
        public override void Set(Color c)
        {
            Free();
            //if(c.A == 0)
            //{
            //    Color = Color.FromArgb(1, c);
            //}
            //else
            if (AlphaFeeder != null)
            {
                int a = NewAlpha();
                if (c.A != a)
                {
                    base.Set(Color.FromArgb(a, c));
                }
                else
                    base.Set(c);
            }
            else
                base.Set(c);
        }
        public void SetA(int a)
        {
            Set(Color.FromArgb(Conform(a), Color.R, Color.G, Color.B));
        }
        public void SetR(int r)
        {
            Set(Color.FromArgb(Color.A, Conform(r), Color.G, Color.B));
        }
        public void SetG(int g)
        {
            Set(Color.FromArgb(Color.A, Color.R, Conform(g), Color.B));
        }
        public void SetB(int b)
        {
            Set(Color.FromArgb(Color.A, Color.R, Color.G, Conform(b)));
        }
        public void Set(int r, int g, int b)
        {
            Set(Color.FromArgb(Color.A, Conform(r), Conform(g), Conform(b)));
        }
        public void SetAdd(int r, int g, int b)
        {
            Set(Color.FromArgb(Color.A, Conform(Color.R + r), Conform(Color.G + g), Conform(Color.B + b)));
        }
        public static Color AddTo(Color color, int a, int r, int g, int b)
        {
            return Color.FromArgb(Conform(color.A + a), Conform(color.R + r), Conform(color.G + g), Conform(color.B + b));
        }

        public int Sum()
        {
            return Color.R + Color.G + Color.B;
        }
    }
}
