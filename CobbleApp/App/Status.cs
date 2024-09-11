using System;
using System.Drawing;
using Thingalink;

namespace CobbleApp
{
    public class Status : Zone
    {
        public new BitmapSurface Surface => (BitmapSurface)base.Surface;
        public BitmapSurface Back;

        public static Status Instance;
        public static void Update()
        {
            Instance?.Tick();
        }
        public ListHead Lines;

        public ListHead Text;
        public ListHead Que;

        public ListMember Now;
        public ListMember AtFrom;
        public ListMember AtTo;
        public ListMember At;

        public Rectangle AtR => ((Rectangular)At.Object).Rectangle;

        public int LineCount;
        public double Maxfill;
        public double Minstay;
        public DateTime LastShow;

        public Paint TextColor => AppSingleton.DefaultTextColor;
        public Font Font;

        // protected Font Erase;
        protected Paint Transparent;

        protected bool Visible;
        public bool IsVisible => Visible;
        protected Action Post;

        protected static string SayBuffer;

        protected string LastAdd;
        protected TimeSpan Lapsed;

        int Idle;

        public bool Flood;

        protected ListMember Deleted;

        public Status(Rectangle rect, int lines, double maxfill, double minstay, bool visible, Action post = null) : base(rect, null, new BitmapSurface(rect))
        {
            if (Status.Instance == null)
                Instance = this;

            Visible = visible;
            Post = post;
            LineCount = lines;
            Maxfill = maxfill;
            Minstay = minstay;

            Transparent = new Paint(Color.Transparent);

            Lines = new ListHead();
            Text = new ListHead();
            Que = new ListHead();

            int h = rect.Height / lines;
            int y = rect.Y;

            Font = Surface.SizeFont("Wider than it is tall and longer", "Arial", rect.Width, h);

            while (y + h < rect.Bottom)
            {
                Lines.Add(new Rectangular(rect.X, y, rect.Width, h));
                y += h;
            }

            At = Lines.First;
            Now = Lines.Last;
            //last out of list
            Lines.DeleteItem(Now);

            LastShow = DateTime.MinValue;
            Flood = false;

            Back = new BitmapSurface(rect);
        }
        public void Clear()
        {
            Text = new ListHead();
            Que = new ListHead();
            At = Lines.First;
            Surface.DrawImage(Back.Bitmap, Rectangle.X, Rectangle.Y);
        }

        public void Show(bool show)
        {
            if (Visible == show)
            {
                return;
            }
            if (show)
            {
                Draw();
                Visible = true;
            }
            else
                Hide();
        }

        public override void Draw()
        {
            LastShow = DateTime.Now;

            At = Lines.First;


            Surface.DrawImage(Back.Bitmap, Rectangle.X, Rectangle.Y);

            while (Text.Count < Lines.Count && Que.Count > 0)
            {
                Text.Add(Que.First.Object);
                Que.DeleteItem(Que.First);
            }


            while (Text.Count > Lines.Count)
            {
                Text.DeleteItem(Text.First);
            }
            Text.Iterate(ShowSay);

            if (Que.Count == 0)
            {
                Flood = false;
            }

            Surface.Refresh();
            Post?.Invoke();
        }

        public void ShowSay(ListMember item)
        {
            Surface.DrawText(Font, TextColor, (string)item.Object, AtR.X, AtR.Y);
            At = At.Next;
        }

        public void Hide()
        {
            Visible = false;
        }

        public static void Log(string text)
        {
            Instance?.Say(text);
        }

        public void Say(string text)
        {
            Que.Add(text);
        }
        public void FillText()
        {
            Surface.DrawImage(Back.Bitmap, Rectangle.X, Rectangle.Y);
            while (Text.Count < LineCount && Que.Count > 0)
            {
                Fill();
            }

            LastShow = DateTime.Now;
            Surface.Refresh();
        }

        public void Tick()
        {
            Idle++;
            if (Idle > 3000000 && Idle != 0 && Text.Count > 0)
            {
                Idle = 0;
                ExpireText();
                Idle++;
                return;
            }

            SayNow();
        }
        private void Fill()
        {
            string line = (string)Que.First.Object;
            Que.DeleteItem(Que.First);
            Fill(line);
        }
        private void Fill(string text)
        {
            Text.Add(text);

            //flush of que only this is not scroll
            if (At == null)
                return;

            Surface.DrawText(Font, TextColor, text, AtR.X, AtR.Y);

            At = At.Next;
        }
        public void SayNow()
        {

            Lapsed = new TimeSpan(DateTime.Now.Ticks - LastShow.Ticks);

            if (Lapsed.TotalMilliseconds < Maxfill)
            {
                Flood = true;
                return;
            }

            if (Que.First == null)
            {
                Flood = false;
                return;
            }

            string line = (string)Que.First.Object;
            Que.DeleteItem(Que.First);
            SayNow(line);
        }


        public void SayNow(string text)
        {
            if (Text.Count == LineCount - 1)
            {
                LastShow = DateTime.Now;

                Text.Add(text);


                Scroll();
                //scroll
            }
            else if (Text.Count >= LineCount)
            {

                Flood = true;
                Que.Add(text);
            }
            else
            {

                LastShow = DateTime.Now;

                Text.Add(text);

                Surface.DrawText(Font, TextColor, text, AtR.X, AtR.Y);
                Surface.Refresh();

                At = At.Next;
                if (At == null)
                {

                }
            }
        }

        public void Scroll()
        {
            ScrollSafe();
        }

        public void ExpireText()
        {
            Idle = 0;

            if (Text.Count > 0)
            {
                ScrollSafe();
            }
        }
        public void SayAll()
        {
            At = Lines.First;
            Text.Iterate(ScrollSay);
        }

        public void ScrollSafe()
        {

            At = Lines.First;

            if (At == null)
                return;

            Deleted = Text.First;
            Text.DeleteItem(Deleted);

            Surface.DrawImage(Back.Bitmap, Rectangle.X, Rectangle.Y);

            Text.Iterate(ScrollSay);

            Surface.Refresh();

            Post?.Invoke();
        }

        public void ScrollSay(ListMember item)
        {
            if (At == null)
            {
                return;
            }
            Surface.DrawText(Font, TextColor, (string)item.Object, AtR.X, AtR.Y);
            At = At.Next;

        }
    }
}
