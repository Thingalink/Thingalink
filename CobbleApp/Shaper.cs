using System.Drawing;

namespace CobbleApp
{
    public static class Shaper
    {
        public delegate void RectMethod(Rectangle rect);

        public delegate Point PointMorph(Point point);
        public delegate Rectangular RectMorph(Rectangular rect);

        public static Rectangle Screen()
        {
            return new Rectangle();
        }

        public static Rectangle RightOf(Rectangular leftZone, int marginL = 0, int marginR = 0)
        {
            return RightOf(leftZone.Rectangle, marginL, marginR);
        }

        public static Rectangle RightOf(Rectangle leftZone, int marginL = 0, int marginR = 0)
        {
            return new Rectangle(leftZone.Right + marginL, leftZone.Y, leftZone.Width, leftZone.Height);
        }

        public static Rectangle OverPercent(Rectangular rectangle, int v, bool w, bool h)
        {
            return OverPercent(rectangle.Rectangle, v, w, h);
        }
        public static Rectangle OverPercent(Rectangle rectangle, int v, bool w, bool h)
        {
            int width = w ? (int)(rectangle.Width * v * .01) : rectangle.Width;
            int x = w ? rectangle.X - (width / 2) : rectangle.X;

            int height = h ? (int)(rectangle.Height * v * .01) : rectangle.Height;
            int y = h ? rectangle.Y - (height / 2) : rectangle.Y;

            return new Rectangle(x, y, width, height);
        }

        public static Rectangle SplitNear(Rectangular rectangle, bool w, bool h)
        {
            return SplitNear(rectangle.Rectangle, w, h);
        }

        public static Rectangle SplitNear(Rectangle zone, bool w, bool h, int wFactor = 2, int hFactor = 0)
        {
            if (hFactor == 0)
                hFactor = wFactor;

            return new Rectangle(zone.X, zone.Y, w ? zone.Width / wFactor : zone.Width, h ? (zone.Height / hFactor) : zone.Height);
        }

        public static Rectangle SplitFar(Rectangular rectangle, bool w, bool h)
        {
            return SplitFar(rectangle.Rectangle, w, h);
        }

        public static Rectangle SplitFar(Rectangle zone, bool w, bool h, int wFactor = 2, int hFactor = 0)
        {
            if (hFactor == 0)
                hFactor = wFactor;

            return new Rectangle(zone.X + (w ? (zone.Width / wFactor) : 0), zone.Y + (h ? (zone.Height / hFactor) : 0),
                w ? zone.Width / wFactor : zone.Width, h ? zone.Y + (zone.Height / hFactor) : zone.Height);
        }

        public static Point CenterPoint(Rectangular rectangle)
        {
            return CenterPoint(rectangle.Rectangle);
        }

        public static Point CenterPoint(Rectangle zone)
        {
            return new Point(zone.X + zone.Width / 2, zone.Y + zone.Height / 2);
        }
        public static Rectangle Center(Rectangular zone, int w, int h)
        {
            return Center(zone.Rectangle, w, h);
        }
        public static Rectangle Center(Rectangle zone, int w, int h)
        {
            if (w > zone.Width)
            {
                w = w % zone.Width;
            }

            if (h > zone.Height)
            {
                h = h % zone.Height;
            }
            int hw = (zone.Width - w) / 2;
            int hh = (zone.Height - h) / 2;

            return new Rectangle(zone.X + hw, zone.Y + hh, w, h);
        }
        public static Rectangle NewRegular(int x, int y, int w, int h)
        {
            return new Rectangle(x, y, w, h);
        }
        public static Rectangle NewNear(Rectangle from, int xOff, int yOff, int wOff, int hOff)
        {
            return new Rectangle(from.X + xOff, from.Y + yOff, from.Width + wOff, from.Height + hOff);
        }

        public static Rectangle CenterPecent(Rectangular zone, double w, double h)
        {
            return CenterPecent(zone.Rectangle, w, h);
        }
        public static Rectangle CenterPecent(Rectangle zone, double w, double h)
        {
            return Center(zone, (int)(zone.Width * w), (int)(zone.Height * h));
        }

        public static Rectangle TopLeft(Rectangular zone, int w, int h, int margin = 0)
        {
            return TopLeft(zone.Rectangle, w, h, margin);
        }
        public static Rectangle TopLeft(Rectangle zone, int w, int h, int margin = 0)
        {
            if (w > zone.Width)
            {
                w = zone.Width - margin - margin;
            }

            if (h > zone.Height)
            {
                h = zone.Height - margin - margin;
            }

            return new Rectangle(zone.X + margin, zone.Y + margin, w, h);
        }

        public static Rectangle RestOfUnder(int y, Rectangle container, int margin = 0)
        {
            return new Rectangle(container.X, y + margin, container.Width, container.Height - y);
        }

        public static Rectangle NextLeft(Rectangular last, int margin = 0)
        {
            return NextLeft(last.Rectangle, margin);
        }
        public static Rectangle NextLeft(Rectangle last, int margin = 0)
        {
            return new Rectangle(last.Right + margin, last.Y, last.Width, last.Height);
        }


        public static Rectangle NextUnder(Rectangular last, int margin = 0)
        {
            return NextUnder(last.Rectangle, margin);
        }
        public static Rectangle NextUnder(Rectangle last, int margin = 0)
        {
            return new Rectangle(last.Left, last.Bottom + margin, last.Width, last.Height);
        }
        public static Rectangle NextUnder(ContainerZone from, int margin = 0)
        {
            var last = (Rectangle)(from.Zones.Last.Object);
            return new Rectangle(last.Left, last.Bottom + margin, last.Width, last.Height);
        }

        public static void MakeCells(RectMethod method, Rectangle zone, int w, int h, int margin = 0)
        {
            var c = TopLeft(zone, w, h, margin);
            method.Invoke(c);

            Rectangle? n;
            n = Next(zone, c, w, h, margin);

            while (n.HasValue)
            {
                method.Invoke(n.Value);
                n = Next(zone, n.Value, w, h, margin);
            }
        }
        public static Rectangle? Next(Rectangular zone, Rectangle last, int w, int h, int margin = 0)
        {
            return Next(zone.Rectangle, last, w, h, margin);
        }
        public static Rectangle? Next(Rectangle zone, Rectangle last, int w, int h, int margin = 0)
        {
            if (RoomW(zone, last))
            {
                return NextLeft(last, margin);
            }
            else if (RoomH(zone, last))
            {
                return NextUnder(last, margin);
            }
            return null;
        }


        public static bool RoomW(Rectangular zone, Rectangle last, int margin = 0)
        {
            return RoomW(zone.Rectangle, last, margin);
        }
        public static bool RoomW(Rectangle zone, Rectangle last, int margin = 0)
        {
            return last.Right + last.Width + margin < zone.Right;
        }
        public static bool RoomH(Rectangular zone, Rectangle last, int margin = 0)
        {
            return RoomH(zone.Rectangle, last, margin);
        }
        public static bool RoomH(Rectangle zone, Rectangle last, int margin = 0)
        {
            return last.Bottom + last.Height + margin < zone.Bottom;
        }
    }
}
