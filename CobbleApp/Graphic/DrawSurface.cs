using System.Drawing;
using Thingalink;

namespace CobbleApp
{

    public abstract class DrawSurface : Rectangular
    {
        protected Graphics Graphics;

        public DrawSurface(int x, int y, int w, int h) : this(new Rectangle(x, y, w, h))
        {

        }
        public DrawSurface(Rectangle rect) : base(rect)
        {

        }
        //GraphicsState State;

        //public void RestoreState()
        //{
        //    if (State != null)
        //        Graphics.Restore(State);
        //}
        //public void SaveState()
        //{
        //    State = Graphics.Save();
        //}

        //public virtual void Snag()
        //{
        //    Rectangle screenRectangle = AppRoot.Form.RectangleToScreen(AppRoot.Form.ClientRectangle);

        //    int titleHeight = screenRectangle.Top - Rectangle.Top;
        //    Graphics.CopyFromScreen(Rectangle.Left,
        //        titleHeight, 0, 0, Rectangle.Size);
        //}


        public Bitmap ToBitmap()
        {
            return new Bitmap(Rectangle.Width, Rectangle.Height, Graphics);
        }

        public void DrawImage(Bitmap bitmap, int x, int y)
        {
            Graphics.DrawImage(bitmap, x, y, bitmap.Width, bitmap.Height);
        }
        public void DrawImage(Bitmap bitmap, int x, int y, int w, int h)
        {
            Graphics.DrawImage(bitmap, x, y, w, h);
        }
        public void DrawImage(Bitmap bitmap, Rectangle rect)
        {
            Graphics.DrawImage(bitmap, rect.X, rect.Y, rect.Width, rect.Height);
        }
        public void DrawImage(Image image, Rectangle dest, Rectangle source)
        {
            Graphics.DrawImage(image, dest, source, GraphicsUnit.Pixel);
        }

        public void DrawRect(Paint paint, int x, int y, int w, int h)
        {
            Graphics.DrawRectangle(paint.Pen, x, y, w, h);
        }
        public void FillBack(Color paint)
        {
            Graphics.FillRectangle(new SolidBrush(paint), 0, 0, Rectangle.Width, Rectangle.Height);
        }
        public void FillBack(Paint paint)
        {
            FillRect(paint, 0, 0, Rectangle.Width, Rectangle.Height);
        }
        public void Outline(Paint paint, Rectangle rect)
        {
            Graphics.DrawRectangle(paint.Pen, rect);
        }

        public void FillRect(Paint paint, int x, int y, int w, int h)
        {
            Graphics.FillRectangle(paint.Brush, x, y, w, h);
        }
        public void FillRect(Paint paint, Rectangle rect)
        {
            Graphics.FillRectangle(paint.Brush, rect);
        }
        public void DrawLine(Paint paint, int x, int y, int x2, int y2)
        {
            Graphics.DrawLine(paint.Pen, x, y, x2, y2);
        }

        public void DrawPoly(Paint paint, ListMember points)
        {
            DrawPoly(paint, (Point[])points.Object);
        }
        public void DrawPoly(Paint paint, Point[] points)
        {
            DrawPoly(paint.Pen, points);
        }
        public void DrawPoly(Pen pen, Point[] points)
        {
            Graphics.DrawPolygon(pen, points);
        }
        public void DrawPoly(Paint paint, PointF[] points)
        {
            Graphics.DrawPolygon(paint.Pen, points);
        }
        public void FillPoly(Paint paint, Point[] points)
        {
            Graphics.FillPolygon(paint.Brush, points);
        }
        public void FillPoly(Paint paint, PointF[] points)
        {
            Graphics.FillPolygon(paint.Brush, points);
        }

        public void DrawEllipse(Paint paint, float x, float y, float width, float height)
        {
            Graphics.DrawEllipse(paint.Pen, x, y, width, height);
        }
        public void DrawEllipse(Paint paint, Rectangle rect)
        {
            Graphics.DrawEllipse(paint.Pen, rect);
        }
        public void FillEllipse(Paint paint, float x, float y, float width, float height)
        {
            Graphics.FillEllipse(paint.Brush, x, y, width, height);
        }

        public void DrawArc(Paint paint, float x, float y, float width, float height, int start, int sweep)
        {
            Graphics.DrawArc(paint.Pen, x, y, width, height, start, sweep);
        }
        public void DrawArc(Paint paint, Rectangle rect, int start, int sweep)
        {
            Graphics.DrawArc(paint.Pen, rect, start, sweep);
        }
        public void FillPie(Paint paint, float x, float y, float width, float height, int start, int sweep)
        {
            Graphics.FillPie(paint.Brush, x, y, width, height, start, sweep);
        }

        public void DrawText(Font font, Paint paint, string text, int x, int y)
        {
            Graphics.DrawString(text, font, paint.Brush, x, y);

        }
        public void DrawText(Font font, Paint paint, string text, Point point)
        {
            Graphics.DrawString(text, font, paint.Brush, point.X, point.Y);

        }

        public void DrawText(Font font, Paint paint, string text, Rectangle zone)
        {
            Graphics.DrawString(text, font, paint.Brush, CenterText(font, text, zone));

        }

        public Point CenterText(Font font, string text, Rectangle zone)
        {
            var size = Measure(text, font);
            return new Point(zone.X + (zone.Width / 2) - (size.Width / 2),
                                zone.Y + (zone.Height / 2) - (size.Height / 2));

        }
        public MeasuredText SizeFont(string text, string fontName, Rectangle rect)
        {
            MeasuredText measuredText = new MeasuredText();
            measuredText.Text = text;

            var font = new Font(fontName, 20);

            var WHSize = Measure(text, font);

            if (WHSize.Width > rect.Width || WHSize.Height > rect.Height)
            {
                while (WHSize.Width > rect.Width || WHSize.Height > rect.Height)
                {
                    font = new Font(fontName, font.SizeInPoints - (font.SizeInPoints * .1f));
                    WHSize = Measure(text, font);
                }
            }
            else
            {
                Font testFont = font;
                var size = WHSize;
                while (size.Width < rect.Width && size.Height < rect.Height)
                {
                    WHSize = size;
                    font = testFont;
                    testFont = new Font(fontName, testFont.SizeInPoints + (testFont.SizeInPoints * .1f));
                    size = Measure(text, testFont);
                }
            }
            measuredText.Font = font;
            measuredText.Size = WHSize;
            measuredText.Point = new Point(rect.X + (rect.Width / 2) - (WHSize.Width / 2),
                                rect.Y + (rect.Height / 2) - (WHSize.Height / 2));

            return measuredText;
        }

        public Font SizeFont(string text, string fontName, int w, int h)
        {
            var font = new Font(fontName, 20);

            var WHSize = Measure(text, font);

            if (WHSize.Width > w || WHSize.Height > h)
            {
                while (WHSize.Width > w || WHSize.Height > h)
                {
                    font = new Font(fontName, font.SizeInPoints - (font.SizeInPoints * .1f));
                    WHSize = Measure(text, font);
                }
            }
            else
            {
                Font testFont = font;
                var size = WHSize;
                while (size.Width < w && size.Height < h)
                {
                    WHSize = size;
                    font = testFont;
                    testFont = new Font(fontName, testFont.SizeInPoints + (testFont.SizeInPoints * .1f));
                    size = Measure(text, testFont);
                }
            }

            return font;
        }

        public Size Measure(string text, Font font)
        {
            if (text == "")
            {
                return new Size(0, 0);
            }
            RectangleF r;
            //using (var graphics = GetGraphics())
            {
                //not safe to task the Surface's Fraphics must que like a draw.

                r = MeasureDisplayStringWidth(Graphics, text, font);
            }
            return new Size((int)r.Width, (int)r.Height);
        }
        static public RectangleF MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
        {
            System.Drawing.StringFormat format = new System.Drawing.StringFormat();
            System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0,
                                                                          1000, 1000);
            System.Drawing.CharacterRange[] ranges =
            {
                new System.Drawing.CharacterRange(0, text.Length)
            };
            System.Drawing.Region[] regions = new System.Drawing.Region[1];

            format.SetMeasurableCharacterRanges(ranges);

            //not safe to task the Surface's Fraphics.
            regions = graphics.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(graphics);

            return rect;// (int)(rect.Right + 1.0f);
        }
    }
}
