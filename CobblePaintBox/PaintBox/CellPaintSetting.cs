using Thingalink;

namespace CobblePaintBox
{
    public class CellPaint
    {
        public static int Layer = 0;

        public CellPaintSetting Back => Layer == 0 ? Background : Fore;

        public CellPaintSetting Background;
        public CellPaintSetting Fore;

        public CellPaint()
        {
            Background = new CellPaintSetting();
            Fore = new CellPaintSetting();
        }

    }
    public class CellPaintSetting
    {
        public int OcilR = 0;
        public int OcilG = 0;
        public int OcilB = 0;
        public bool OcilROn;
        public bool OcilGOn;
        public bool OcilBOn;

        public int OcilRLow = 0;
        public int OcilRHigh = 255;
        public int OcilGLow = 0;
        public int OcilGHigh = 255;
        public int OcilBLow = 0;
        public int OcilBHigh = 255;

        public int Holding = 0;
        public bool To;
        public Paint Origin;
        public Paint Paint;
        public Paint Target;
        public Paint Alt;
    }
}
