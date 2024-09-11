using CobbleApp;
using CobbleControls;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace CobblePaintBox
{

    public class BrushMatrix : PaintedMatrix// PaintMatrix
    {
        public BrushMatrix(int x, int y, int w, int h, int initialCells, ContainerZone parent) : this(new Rectangle(x, y, w, h), initialCells, parent)
        {
        }
        public BrushMatrix(Rectangle rect, int initialCells, ContainerZone parent) : base(null, rect, initialCells, parent)
        {
        }

        protected override void InitCells()
        {
            ActaullyInitCells();

            ready = true;
        }

        public override void NewMade(Cell cell)
        {
            cell.Mod = InitStamp;

        }

    }
    public class DrawnBrush : BrushMatrix// DrawZone
    {
        public static DrawnBrush Brush;
        public ToggleButton Zoom;

        public BrushedMatrix TargetCanvas;

        public ZoomZone ZoomCanvas;
        public DragSelect Peat;

        public Color BaseColor;
        internal Action DoClear;

        public DrawnBrush(ref ToggleButton zoom, Rectangle rect, ContainerZone parent, DragSelect peat) : base(rect, rect.Width, parent)
        {
            Brush = this;
            Peat = peat;
            Zoom = zoom;
            _Surface = new BitmapSurface(new Rectangle(X, Y, Width, Height));
        }
        public void LoadCleared()
        {
            Surface.Load(new Bitmap(Width, Height, PixelFormat.Format32bppArgb));
            ClearCells();
            //Bitmap.Load(Selectme.Value, 0);
            Updates = true;
            Draw();
        }

        public override void Refresh()
        {
            if (DoClear != null)
            {
                DoClear.Invoke();
                DoClear = null;
            }

            DrawBack();
            Surface.Refresh();

            if (Zoom.On)
            {
                ZoomCanvas.TriggerRefresh();
            }
        }

        public void DrawBack()
        {
            DrawScreen.Instance.FillRect(AppSingleton.DefaultTextColor, Rectangle);
        }
        public void GoZoom()
        {
            if (TargetCanvas == null)
                return;

            if (Zoom.On)
            {
                TargetCanvas.ParentRemove();

                ZoomCanvas = new ZoomZone(this, TargetCanvas.Rectangle, TargetCanvas.Parent);
                ZoomCanvas.Paint = DrawConfig.SelectedPaint;

                ZoomCanvas.MoveSub();
                ZoomCanvas.QueDraw();
            }
            else
            {
                if (ZoomCanvas == null)
                {//clicks too fast
                    return;
                }
                ZoomCanvas.ParentRemove();
                TargetCanvas.ParentAdd();
                TargetCanvas.MoveSub();
                ZoomCanvas = null;
                TargetCanvas.Refresh();
            }
        }
    }

}
