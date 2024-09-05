using CobbleApp;
using System.Drawing;
using System.Drawing.Imaging;
using Thingalink;

namespace CobblePaintBox
{


    public class BrushSlotBar : ContainerZone
    {
        public static BrushSlotBar Instance;

        public DrawnBrush Bitmap;
        public ListHead Slots;
        //Bitmap Selectme;
        DragSelect RepeatBlock;

        Rectangle SlotArea;

        protected Rectangle ButtonArea;
        protected Point ImageAt;
        protected ClickButton Clear;
        protected ClickButton Load;
        public ToggleButton Zoom;


        public BrushSlotBar(int x, int y, int w, int h, ContainerZone parent = null, DrawSurface surface = null) : this(new Rectangle(x, y, w, h), parent, surface)
        {

        }
        public BrushSlotBar(Rectangle rect, ContainerZone parent = null, DrawSurface surface = null) : base(rect, parent, surface)
        {
            Instance = this;

            Slots = new ListHead();

            InitButtons();

            int side = Rectangle.Height - ButtonArea.Height;
            var r = new Rectangle(Rectangle.X, ButtonArea.Bottom, side, side);// Shaper.SplitFar(Rectangle, false, true, 4);


            //Selectme = new BitmapValue();
            //Selectme = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);

            Bitmap = new DrawnBrush(ref Zoom, r, this, RepeatBlock);

            //Selectme.SetBitmap(Bitmap.Surface.Bitmap);

            int x = SlotArea.Right;
            while (x + SlotArea.Width <= Rectangle.Right)
            {
                Slots.Add(new BrushSlot(new Rectangle(x, Rectangle.Y, SlotArea.Width, SlotArea.Height), Selected, this));
                x += SlotArea.Width;
            }
        }

        //public override void ParentAdd()
        //{
        //    Item = Parent.Add(this, true);
        //}

        //public override void ParentRemove()
        //{
        //    Parent.Remove(Item, true);
        //}

        protected void InitButtons()
        {
            //int qh = Rectangle.Height / 4;
            SlotArea = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Height - ClickButton.DefaultHeight, Rectangle.Height);
            //ButtonArea = new Rectangle(slotArea.X, slotArea.Y, slotArea.Width/3, slotArea.Height);// Shaper.SplitNear(DrawScreen.Instance.Rectangle, false, true, 4);
            ButtonArea = new Rectangle(Rectangle.X, Rectangle.Y, SlotArea.Width, ClickButton.DefaultHeight * 3);

            Clear = new ClickButton("Clear", GoClear, Shaper.SplitNear(ButtonArea, false, true, 0, 3), this);
            //Load = new ClickButton("L", GoLoad, Shaper.RightOf(Clear, 1), this);
            Zoom = new ToggleButton("L", GoZoom, Shaper.NextUnder(Clear), this);

            //Zoom = new ToggleButton("Zoom", GoZoom, Shaper.RightOf(Clear), this);

            ImageAt = new Point(Rectangle.X, ButtonArea.Bottom);


        }
        protected virtual void Selected(object value)
        {
            Bitmap.Surface.Load(new Bitmap((Bitmap)value));
            Bitmap.Load(Bitmap.Surface.Bitmap, 0);
            Draw();
        }

        public void SetTarget(BrushedMatrix targetCanvas)
        {
            Bitmap.TargetCanvas = targetCanvas;


        }

        public override void ReSub()
        {
            Bitmap.ReSub();
        }

        protected void GoZoom()
        {
            Bitmap.GoZoom();

        }

        protected void GoClear()
        {
            //Selectme = new Bitmap(Bitmap.W, Bitmap.H, PixelFormat.Format32bppArgb);
            Bitmap.LoadCleared();
        }
        protected void GoLoad()
        {

        }

        public override void Draw()
        {
            Clear.Draw();
            //Load.Draw();
            Zoom?.Draw();
            Bitmap.DrawBack();
            Bitmap.Surface.Refresh();//.Draw();
            Slots.Iterate(Draw);
        }
        public void Draw(ListMember item)
        {
            ((Zone)item.Object).Draw();
        }
    }
    class BrushSlot : ContainerZone
    {
        protected ClickButton Clear;
        protected ClickButton Load;
        protected ClickButton Zoom;

        protected Rectangle ButtonArea;

        protected Bitmap init;
        protected Bitmap Bitmap;
        protected Point ImageAt;

        protected Speck.PassValue SelectingPass;
        //protected Bitmap SelectingFetch;

        public BrushSlot(int x, int y, int w, int h, Speck.PassValue pass, ContainerZone parent = null, DrawSurface surface = null) : this(new Rectangle(x, y, w, h), pass, parent, surface)
        {

        }
        public BrushSlot(Rectangle rect, Speck.PassValue pass, ContainerZone parent = null, DrawSurface surface = null) : base(rect, parent)
        {
            //SelectingFetch = fetch;
            SelectingPass = pass;

            //Bitmap = init = bitmap;
            InitButtons();
            //InitSurface();
        }

        protected virtual void InitButtons()
        {
            ButtonArea = new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, ClickButton.DefaultHeight * 3); //Shaper.SplitNear(Rectangle, false, true, 4);

            Clear = new ClickButton("Clear", GoClear, Shaper.SplitNear(ButtonArea, false, true, 0, 3), this);
            Load = new ClickButton("Load", GoLoad, Shaper.NextUnder(Clear), this);
            Zoom = new ClickButton("Use", GoZoom, Shaper.NextUnder(Load), this);

            ImageAt = new Point(Rectangle.X, ButtonArea.Bottom);
        }
        //protected virtual void InitSurface()
        //{
        //    if (Bitmap == null)
        //    {
        //        Bitmap = new Bitmap(Rectangle.Width, Rectangle.Width, PixelFormat.Format32bppArgb);
        //    }
        //}

        protected virtual void GoZoom()
        {
            if(Bitmap != null)
                SelectingPass.Invoke(Bitmap);
        }

        protected virtual void GoWipe()
        {
            init = null;
            GoClear();
        }
        protected virtual void GoClear()
        {
            Bitmap = init;
            //InitSurface();
            Draw();
        }
        protected virtual void GoLoad()
        {
            //SelectingFetch.changed = false;
            Bitmap = new Bitmap(BrushSlotBar.Instance.Bitmap.Surface.Bitmap);
            Draw();
        }
        public override void Draw()
        {
            Surface.FillRect(AppRoot.ToolText.Textcolor, Rectangle);
            base.Draw();
            if(Bitmap == null)
            {
                Surface.FillRect(new Paint(Color.Black), ImageAt.X, ImageAt.Y, Rectangle.Width, Rectangle.Height - (ImageAt.Y - Rectangle.Y));
            }
            else
                Surface.DrawImage(Bitmap, ImageAt.X, ImageAt.Y);
        }
    }
}
