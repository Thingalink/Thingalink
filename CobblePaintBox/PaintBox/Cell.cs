using CobbleApp;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CobblePaintBox
{
    
    public class Cell : NeighborZone
    {
        public static Bitmap Transparent;

        public delegate void CellPass(Cell cell);


        public new Cell NeighborNW => (Cell)base.NeighborNW;
        public new Cell NeighborN => (Cell)base.NeighborN;
        public new Cell NeighborNE => (Cell)base.NeighborNE;
        public new Cell NeighborW => (Cell)base.NeighborW;
        public new Cell NeighborE => (Cell)base.NeighborE;
        public new Cell NeighborSW => (Cell)base.NeighborSW;
        public new Cell NeighborS => (Cell)base.NeighborS;
        public new Cell NeighborSE => (Cell)base.NeighborSE;

        public CellPaint Paint;

        public CellPaintSetting PS => Paint.Back;
        public Paint PP => PS.Paint;

        public DateTime Mod;

        public int SurfX;
        public int SurfY;

        public Cell(int x, int y, int w, int h, SquareMatrix parent) : this(new Rectangle(x, y, w, h), parent)
        {

        }
        public Cell(Rectangle rect, SquareMatrix parent) : base(rect, parent)
        {
            Paint = new CellPaint();
            if (Transparent == null)
            {
                Transparent = new Bitmap(Rectangle.Width, Rectangle.Height, PixelFormat.Format32bppArgb);
            }
        }

        //public override void ParentAdd()
        //{
        //}
        //public override void ParentRemove()
        //{
        //}

        public override void Draw()
        {
            var duh = Paint;
            var duhduh = Paint.Back?.Paint;
            if (duh == null || duhduh == null)
                return;
            if (Paint.Back.Paint.Color.A < 255)
            {
                Surface.DrawImage(Transparent, Rectangle);
            }

            Surface.FillRect(Paint.Back.Paint, Rectangle);

            //((BitmapSurface)Surface).Bitmap.Save("C:\\SlabState\\snapme.bmp");
            //not reused enough to keep around
            Paint.Back.Paint.Free();
        }
    }
}
