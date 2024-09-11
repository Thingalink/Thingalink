using System;
using System.Drawing;
using CobbleApp;
using Thingalink;

namespace CobbleControls
{
    
    public class ColorPicker : ContainerZone
    {
        DragSelect A;
        DragSelect R;
        DragSelect G;
        DragSelect B;

        public Paint Back;
        public Paint PaintA;
        public Paint PaintR;
        public Paint PaintG;
        public Paint PaintB;

        Rectangle Splotch;

        Paint Selection;
        public Paint Paint => Selection;

        Action Action;

        public ColorPicker(Color init, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, Action action = null) : base(rect, parent, surface)
        {
            Selection = new Paint(init);
            Init(Selection.Color, action);
        }
        public ColorPicker(ref Paint selection, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, Action action = null) : base(rect, parent, surface)
        {
            Selection = selection ?? new Paint(RandomAccess.RandomColor());
            Color color = Selection.Color;
            Init(Selection.Color, action);
            //MovePreSub();
        }
        public void Init(Color color, Action action = null)
        {
            Action = action;
            Back = new Paint(Color.Black);
            PaintA = new Paint(color.A, 255, 255, 255);
            PaintR = new Paint(255, color.R, 0, 0);
            PaintG = new Paint(255, 0, color.G, 0);
            PaintB = new Paint(255, 0, 0, color.B);

            Selection.Set(color);

            Splotch = Shaper.TopLeft(Rectangle, Width / 5, Height);
            A = new DragSelect(PaintA, "A", 3, SetA, Splotch, this, selfDraw: false);
            Splotch = Shaper.RightOf(A.Rectangle);
            R = new DragSelect(PaintR, "R", 3, SetR, Splotch, this, selfDraw: false);
            Splotch = Shaper.RightOf(R.Rectangle);
            G = new DragSelect(PaintG, "G", 3, SetG, Splotch, this, selfDraw: false);
            Splotch = Shaper.RightOf(G.Rectangle);
            B = new DragSelect(PaintB, "B", 3, SetB, Splotch, this, selfDraw: false);
            Splotch = Shaper.RightOf(B.Rectangle);

            A.Set(PaintA.Color.A);
            R.Set(PaintR.Color.R);
            G.Set(PaintG.Color.G);
            B.Set(PaintB.Color.B);

        }

        public new void MoveSub()
        {
            A.MoveSub();
            R.MoveSub();
            G.MoveSub();
            B.MoveSub();
        }

        public override void Draw()
        {
            Surface.FillRect(Back, A.Rectangle);
            Surface.FillRect(Back, Splotch);
            base.Draw();
            Surface.FillRect(Selection, Splotch);
        }

        void SetA()
        {
            if (A.Value == Selection.Color.A)
                return;

            Selection.SetA(A.Value);
            PaintA.SetA(A.Value);


            Draw();

            Action?.Invoke();
        }
        void SetR()
        {
            if (R.Value != Selection.Color.R)
            {
                Selection.SetR(R.Value);
                PaintR.SetR(R.Value);


                Draw();
                Action?.Invoke();
            }

        }
        void SetG()
        {

            if (G.Value == Selection.Color.G)
                return;

            Selection.SetG(G.Value);
            PaintG.SetG(G.Value);


            Draw();
            Action?.Invoke();

        }
        void SetB()
        {
            if (B.Value == Selection.Color.B)
                return;

            Selection.SetB(B.Value);
            PaintB.SetB(B.Value);


            Draw();
            Action?.Invoke();

        }
    }
}
