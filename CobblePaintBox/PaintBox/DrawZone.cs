using CobbleApp;
using System;
using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobblePaintBox
{
    public class DrawZone : Zone
    {
        public new BitmapSurface Surface => (BitmapSurface)base.Surface;

        public Paint Paint;

        //public float radiusX = 2;
        //public float radiusY = 2;
        public float radiusX = DrawConfig.SplitH.Value;
        public float radiusY = DrawConfig.SplitV.Value;
        public float SkewX = 0;

        public bool Updates;

        long LastTick;

        public DrawZone(Rectangle rect, ContainerZone parent, Bitmap bitmap = null) : this(rect, parent)
        {
            if (bitmap == null)
                _Surface = new BitmapSurface(X, Y, Width, Height);
            else
                _Surface = new BitmapSurface(bitmap, new Rectangle(X, Y, Width, Height));
        }
        protected DrawZone(Rectangle rect, ContainerZone parent) : base(rect, parent)
        {
            LastTick = DateTime.Now.Ticks;
        }

        //public override void ParentAdd()
        //{
        //    Item = Parent.Add(this, true);
        //}

        //public override void ParentRemove()
        //{
        //    Parent.Remove(Item, true);
        //}

        void Save(ListMember item, int saveFrame)
        {
            ((Bitmap)item.Object).Save("c:\\SlabState\\Gut\\Reel\\" +
                DateTime.Now.Year + "-" + DateTime.Now.Month.ToString("D2") + "-" + DateTime.Now.Day.ToString("D2") +
                "Frame" + saveFrame + ".bmp");
        }
        //int PeatyWas => Peat.Value > 30 ? (Peat.Value * 100) : Peat.Value > 10 ? (Peat.Value * 10) : Peat.Value > 5 ? (Peat.Value * 3) : Peat.Value;

        //int Peaty => Peat.Value > 30 ? (Peat.Value * 100) : Peat.Value > 10 ? (Peat.Value * 50) : Peat.Value > 5 ? (Peat.Value * 3) : Peat.Value;

        public static int PeatModNo(int i)
        {
            int peat = 0;
            if (i < 50)
            {
                if (i < 11)
                {
                    switch (i)
                    {
                        case 0:
                            break;
                        case 1:
                            peat = 20;
                            break;
                        case 2:
                            peat = 50;
                            break;
                        case 3:
                            peat = 100;
                            break;
                        case 4:
                            peat = 200;
                            break;
                        case 5:
                            peat = 350;
                            break;
                        case 6:
                            peat = 600;
                            break;
                        case 7:
                            peat = 800;
                            break;
                        case 8:
                            peat = 1000;
                            break;
                        case 9:
                            peat = 1400;
                            break;
                        case 10:
                            peat = 2000;
                            break;
                    }
                }
                else if (i < 30)
                {
                    peat = i * 400;
                }
                else
                {
                    peat = i * 600;
                }
            }
            else if (i < 75)
            {
                peat = i * 1000;
            }
            else
            {
                peat = i * 1500;
            }
            //Status.Instance.Clear();
            //Status.Log((i * 30).ToString());
            //Status.Instance.FillText();
            return i * 30;
        }
        public override void Click(MouseEventArgs point)
        {
            //try
            //{
            if (point.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            //long ticks = DateTime.Now.Ticks;

            //// int peat = PeatMod(Peat.Value);
            //long span = ticks - LastTick;
            //if (span < Peat.CalcValue) //Peaty)
            //{
            //    return;
            //}
            //Status.Instance.Clear();
            //Status.Log(span.ToString());
            //Status.Instance.FillText();
            //changes often nail it down
            // ClickItem = Item;
            DrawAt(new Point(point.X, point.Y));
            //if (Deepa)
            //{
            //    var p = new Paint(SelectedPaint.Color);
            //    Deeper(ClickItem.Next ?? Depth.First, point, p, deepener, PointMorpher, PaintMorpher);
            //}

            //LastTick = ticks;
            //}
            //catch (Exception e)
            //{
            //    Status.Log(e.Message);
            //}

            //Update();
        }

        public virtual void DrawAt(Point point)
        {
            DrawOn(Surface, point, Paint);
            //Surface.Bitmap.Save("C:\\SlabState\\Gut\\.Snap1.bmp");

            Update();
        }

        public void DrawOn(ListMember item, Point point, Paint paint)
        {
            DrawOn((BitmapSurface)item.Object, point, paint);
        }
        public void DrawOn(BitmapSurface surface, Point point, Paint paint)
        {
            surface.FillEllipse(paint, point.X - Rectangle.X, point.Y - Rectangle.Y, radiusX, radiusY);
        }


        public override void Move(MouseEventArgs point)
        {
            Click(point);
            //AppHost.Instance.UIService.QueAction(Draw);
            //if (AppHost.Instance.UIService.ClickService.LeftDown)
            //{
            //    Surface.FillEllipse(SelectedPaint, point.X - Rectangle.X, point.Y - Rectangle.Y, radiusX, radiusY);
            //    Updates = true;
            //}
        }

        //long lastRefresh;

        protected virtual void Update()
        {

            //long nowTick = DateTime.Now.Ticks;
            //if (lastRefresh < nowTick - 2000000)
            //{
            //    lastRefresh = nowTick;
            //    Refresh();
            //}
            //else
            //{
            Updates = true;
            //}

        }
        public virtual void Refresh()
        {
            if (Updates)
            {
                Draw();
            }
            //Surface.Refresh();
        }

        public override void Draw()
        {

            //empty frames will be transparent
            DrawScreen.Instance.FillRect(AppSingleton.DefaultTextColor, Rectangle);
            Surface.Refresh();

            Updates = false;
        }
    }

}
