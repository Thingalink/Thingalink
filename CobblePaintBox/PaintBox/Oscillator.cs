using CobbleApp;
using CobbleControls;
using System;
using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    public class Oscillator : ContainerZone
    {
        public static Oscillator Instance;

        public bool OcilROn => OcilR.On;
        public bool OcilGOn => OcilG.On;
        public bool OcilBOn => OcilB.On;
        public bool AssignROn => AssignR.On;
        public bool AssignGOn => AssignG.On;
        public bool AssignBOn => AssignB.On;

        ToggleButton AssignR;
        ToggleButton AssignG;
        ToggleButton AssignB;
        public ToggleButton OcilR;
        public ToggleButton OcilG;
        public ToggleButton OcilB;

        public DragSelect RUnder;
        public DragSelect ROver;
        public DragSelect GUnder;
        public DragSelect GOver;
        public DragSelect BUnder;
        public DragSelect BOver;
        Action Action;

        public Oscillator(Rectangle rect, ContainerZone parent, Action action = null) : base(rect, parent)
        {
            Action = action;
            AssignR = new ToggleButton("A", null, Rectangle.X, Rectangle.Y, 15, 15, this);
            OcilR = new ToggleButton("R", Action, Rectangle.X, Rectangle.Y + 20, 15, 15, this);

            var r = Shaper.NewRegular(AssignR.Rectangle.Right + 1, AssignR.Y, 25, Height);
            RUnder = new DragSelect(AppSingleton.DefaultBackColor, "R Under", 3, Action, r, this);
            RUnder.Range = 255;
            RUnder.RangeLow = 0;
            RUnder.Set(0);

            r = Shaper.NextLeft(r, 1);
            ROver = new DragSelect(AppSingleton.DefaultBackColor, "R Over", 3, Action, r, this);
            ROver.Range = 255;
            ROver.RangeLow = 0;
            ROver.Set(255);


            AssignG = new ToggleButton("A", null, ROver.Rectangle.Right + 1, Rectangle.Y, 15, 15, this);
            OcilG = new ToggleButton("G", Action, AssignG.X, Rectangle.Y + 20, 15, 15, this);

            r = Shaper.NewRegular(AssignG.Rectangle.Right + 1, AssignG.Y, 25, Height);
            GUnder = new DragSelect(AppSingleton.DefaultBackColor, "G Under", 3, Action, r, this);
            GUnder.Range = 255;
            GUnder.RangeLow = 0;
            GUnder.Set(0);

            r = Shaper.NextLeft(r, 1);
            GOver = new DragSelect(AppSingleton.DefaultBackColor, "G Over", 3, Action, r, this);
            GOver.Range = 255;
            GOver.RangeLow = 0;
            GOver.Set(255);

            AssignB = new ToggleButton("A", null, GOver.Rectangle.Right + 1, Rectangle.Y, 15, 15, this);
            OcilB = new ToggleButton("B", Action, AssignB.X, Rectangle.Y + 20, 15, 15, this);

            r = Shaper.NewRegular(AssignB.Rectangle.Right + 1, AssignB.Y, 25, Height);
            BUnder = new DragSelect(AppSingleton.DefaultBackColor, "B Under", 3, Action, r, this);
            BUnder.Range = 255;
            BUnder.RangeLow = 0;
            BUnder.Set(0);

            r = Shaper.NextLeft(r, 1);
            BOver = new DragSelect(AppSingleton.DefaultBackColor, "B Over", 3, Action, r, this);
            BOver.Range = 255;
            BOver.RangeLow = 0;
            BOver.Set(255);

            Instance = this;
        }

        protected void ConfiChange()
        {
            Action?.Invoke();
        }
    }
}
