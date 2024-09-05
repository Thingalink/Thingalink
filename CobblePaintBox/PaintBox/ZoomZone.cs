using CobbleApp;
using System;
using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    public class ZoomZone : DrawZone
    {
        DrawnBrush Brush;
        public ZoomZone(DrawnBrush brush, Rectangle rect, ContainerZone parent) : base(rect, parent, brush.Surface.Bitmap)
        {
            Brush = brush;
        }

        public void TriggerRefresh()
        {
            Draw();
        }

        public void JustDrawAt(Point point)
        {
            int x = point.X - Rectangle.X;
            int y = point.Y - Rectangle.Y;
            float xf = (float)x / Surface.Width;

            x = (int)(Brush.Width * xf);

            float yf = (float)y / Surface.Height;

            y = (int)(Brush.Height * yf);

            var p = new Point(x + Brush.X, y + Brush.Y);
            Brush.Click(p);//.Surface.FillRect(Paint, x, y, 1, 1);// .DrawRect.DrawImage(Surface.Bitmap, p.X, p.Y);
                           //            Brush.DrawOn(Surface, p, Paint);
                           //Surface.Bitmap.Save("C:\\SlabState\\Gut\\.Snap1.bmp");
        }

        int GetSwing(int value)
        {
            if (DrawConfig.Swing.Value == 0)
                return value;

            if (RandomAccess.Fifty50)
            {
                return RandomAccess.Next(value - (DrawConfig.Swing.Value), value);
            }
            return RandomAccess.Next(value + 1, value + 1 + DrawConfig.Swing.Value);
        }
        int GetSwing(int value, int origin)
        {
            int dif = (origin - value) / 2;
            if (Math.Abs(dif) > 1)
            {
                if (RandomAccess.Fifty50)
                {
                    if (origin > value)
                        return RandomAccess.Next(value - (DrawConfig.Swing.Value) - dif, value + dif);
                    else
                        return RandomAccess.Next(value - (DrawConfig.Swing.Value) + dif, value - dif);
                }
                else
                {
                    if (origin > value)
                        return RandomAccess.Next(value + 1 - dif, value + DrawConfig.Swing.Value + dif);
                    else
                        return RandomAccess.Next(value + 1 + dif, value + DrawConfig.Swing.Value - dif);
                }
            }
            return GetSwing(value);
        }
        public override void DrawAt(Point point)
        {
            JustDrawAt(point);

            if (DrawConfig.SplitH.Value > 1)
            {
                Point prev = point;
                Point pointNow = new Point(GetSwing(point.X), GetSwing(point.Y));

                int i = 0;
                while (i < DrawConfig.SplitH.Value)
                {
                    pointNow = new Point(GetSwing(pointNow.X, prev.X), GetSwing(pointNow.Y, prev.Y));

                    prev = pointNow;
                    JustDrawAt(pointNow);

                    i++;
                }
            }
        }
    }

}
