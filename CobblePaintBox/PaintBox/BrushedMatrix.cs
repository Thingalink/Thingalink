using CobbleApp;
using System;
using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobblePaintBox
{
    public class BrushedMatrix : PaintedMatrix
    {
        public DrawnBrush Brush;

        int xMod;
        int yMod;

        long lastTick;

        Cell ImageCell;
        Cell ImageRowCell;

        int loopR;
        int loopC;


        public BrushedMatrix(ListHead frames, ref DrawnBrush brush, Rectangle rect, ContainerZone parent, bool byCenter, Bitmap bitmap = null) : base(frames, rect, 0, parent, bitmap)
        {
            Brush = brush;
            if (byCenter)
            {
                xMod = Brush.Width / 2;
                yMod = Brush.Height / 2;
            }
            else
            {
                xMod = 0;
                yMod = 0;
            }
            lastTick = DateTime.Now.Ticks;

        }

        public void DrawImage(ref DrawnBrush brush, int x, int y)
        {
            Brush = brush;
            loopR = 0;

            //this.Set(new Rectangle(Rectangle.X, Rectangle.Y- 150, Rectangle.Width, Rectangle.Height));
            //var fw = Brush.Rectangle.Width / (float)x;
            //var fh = Brush.Rectangle.Height / (float)y;
            //            new Point(Rectangle.X + (int)(Rectangle.Width * fw), Rectangle.Y + (int)(Rectangle.Height * fh)
            //ImageCell = ChildCell(new Point(Rectangle.X, Rectangle.Y));
            x = Math.Max(Rectangle.X, x - Brush.Rectangle.Width / 2);
            y = Math.Max(Rectangle.Y, y - Brush.Rectangle.Height / 2);

            ImageRowCell = ChildCell(new Point(x, y));

            if (ImageRowCell != null)
                Brush.Rows.Iterate(DrawImageRow);
            else
            {

            }
        }
        public void DrawImageRow(ListMember item)
        {
            ListHead Row = (ListHead)item.Object;

            if (ImageRowCell == null)
            {
                return;
            }
            ImageCell = ImageRowCell;

            loopC = 0;

            if (ImageCell != null)
                Row.Iterate(DrawImageCell);
            else
            {
            }

            ImageRowCell = ImageRowCell.NeighborS;
            loopR++;
        }


        public void DrawImageCell(ListMember item)
        {
            if (ImageCell == null)
                return;

            var cell = (Cell)item.Object;

            if (cell.PP != null && cell.PP.Color.ToArgb() != Brush.BaseColor.ToArgb())
            {

                if (DrawConfig.Blend.On)
                {
                    ImageCell.PP.SetAdd(
                        LakeshoreConfig.Tword(ImageCell.PP.Color.R, cell.PP.Color.R, .2),
                       LakeshoreConfig.Tword(ImageCell.PP.Color.G, cell.PP.Color.G, .2),
                       LakeshoreConfig.Tword(ImageCell.PP.Color.B, cell.PP.Color.B, .2));

                }
                else
                {
                    ImageCell.PS.Paint = new Paint(cell.PP.Color);
                }

                Update(ImageCell);
            }
            if (ImageCell.NeighborE == null)
                return;
            ImageCell = ImageCell.NeighborE;

            loopC++;
        }

        //public void DrawImage(Bitmap bitmap, int x, int y)
        //{
        //    Surface.DrawImage(bitmap, x, y);
        //    Refresh();
        //    //Surface.DrawImage(Brush.Surface.Bitmap, point.X - Rectangle.X - xMod, point.Y - Rectangle.Y - yMod);

        //    //Update();
        //}

        //public override void Move(MouseEvent point)
        //{
        //    if (point.E.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        Click(point);
        //    }
        //    else
        //    {

        //    }
        //}

        public override void Click(MouseEventArgs point)
        {
            //long thelastTick = lastTick;
            //var ms = (long)((point.Ticks - thelastTick) / 2500);

            //var ts = new TimeSpan(point.Ticks - thelastTick);

            //if (ts.TotalSeconds < 4)
            //{
            //    if (ts.TotalMilliseconds < DrawConfig.ClickBlock.Value)
            //        return;
            //}
            //lastTick = point.Ticks;

            if (!DrawConfig.Solid.On)
            {
                DrawImage(ref DrawnBrush.Brush, point.X, point.Y);
            }
            else
            {
                base.Click(point);
            }
        }
    }
}
