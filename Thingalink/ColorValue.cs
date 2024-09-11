using System.Drawing;

namespace Thingalink
{
    /// <summary>
    /// undefined visuality. possibly an owner draw.
    /// seems would be abstact but doesn't even need to be derived.
    /// if a UI component wants a PaintValue you can do whatever.
    /// a blank slate so that ColorValue is not root of the inheritance chain.
    /// </summary>
    public class PaintValue : Mote
    {

    }
    public class ColorValue : PaintValue
    {
        private new Color Value;
        public Color Color => Value;
        
        public virtual void Set(Color c)
        {
            Value = c;
        }
        public void Set(int r, int g, int b)
        {
            Set(Color.FromArgb(Color.A, Conform(r), Conform(g), Conform(b)));
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
    }
}
