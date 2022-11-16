using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace CobbleApp
{
    public class BitmapSurface : DrawSurface
    {
        public delegate Color ModColor(Color color);

        public Bitmap Bitmap;
        //bool scale;

        public BitmapSurface(Rectangle r) : this(r.X, r.Y, r.Width, r.Height)
        {

        }
        public BitmapSurface(int x, int y, int w, int h) : this(new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb), x, y)
        {

        }
        public BitmapSurface(string path, Rectangle r) : base(r)
        {
            if (path != null)
                Load(path);
            else
                Load(new Bitmap(W, H, System.Drawing.Imaging.PixelFormat.Format32bppArgb));

        }
        public BitmapSurface(Bitmap bitmap, int x = 0, int y = 0) : base(x, y, bitmap.Width, bitmap.Height)
        {
            Load(bitmap);
        }

        public BitmapSurface(Bitmap bitmap, Rectangle rect) : base(rect)
        {
            //scale = true;
            Load(bitmap);
        }

        public void Load(string path)
        {
            Load(new Bitmap(path));
        }
        public void Load(Bitmap bitmap)
        {
            Bitmap = bitmap;
            Graphics = Graphics.FromImage(Bitmap);
        }
        public void LoadRefly(ref Bitmap bitmap)
        {
            Bitmap = bitmap;
            Graphics = Graphics.FromImage(Bitmap);
        }

        public void Refresh()
        {
            DrawScreen.Instance.DrawImage(Bitmap, Rectangle);
        }
        public void ClearCell(int x, int y)
        {
            Bitmap.SetPixel(x, y, Color.Transparent);
        }
        public void ReMake()
        {
            Load(new Bitmap(Rectangle.Width, Rectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
        }
        internal void Clear(int x, int y, int width, int height)
        {
            int offX = 0;
            int offY = 0;
            while (offX <= width)// + 1)
            {
                while (offY <= height)// + 1)
                {
                    ClearCell(x + offX, y + offY);

                    offY++;
                }
                offX++;
            }
        }

        internal void LoadClip(Bitmap bitmap)
        {
            DrawImage(bitmap, Rectangle, new Rectangle(240, 0, Rectangle.Width, Rectangle.Height));
        }


        public static void ModPixel(Bitmap bitmap, Point point, ModColor method)
        {
            var r = new Rectangle(point.X, point.Y, 1, 1);

            BitmapData imageData = bitmap.LockBits(r, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int size = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat);

            int colors = size == 24 ? 3 : 4;
            IntPtr _current = imageData.Scan0;

            var Pixels = new byte[colors];
            // Copy data from pointer to array
            Marshal.Copy(_current, Pixels, 0, Pixels.Length);

            byte pixelA = 255;
            if (colors > 3)
                pixelA = Pixels[3];

            var coloris = Color.FromArgb(pixelA, Pixels[2], Pixels[1], Pixels[0]);
            if (coloris.R > 0 || coloris.G > 0 || coloris.B > 0)
            {

            }
            var colornew = method.Invoke(coloris);

            if (coloris.ToArgb() != colornew.ToArgb())
            {
                // _current = imageData.Scan0;
                Marshal.WriteByte(_current, colornew.B);
                _current += 1;
                Marshal.WriteByte(_current, colornew.G);
                _current += 1;
                Marshal.WriteByte(_current, colornew.R);

                if (colors > 3)
                {
                    _current += 1;
                    Marshal.WriteByte(_current, colornew.A);
                }
            }

            bitmap.UnlockBits(imageData);
        }
    }
}
