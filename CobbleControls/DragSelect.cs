using System;
using System.Drawing;
using System.Windows.Forms;
using CobbleApp;
using Thingalink;

namespace CobbleControls
{
    public class DragSelect : Zone// ToggleButton
    {
        //speed low / high, digit L-R
        public delegate int IntMod(int i);


        //public Zone DragZone;
        public Point Center;

        //for a tick on chug - not needed
        //int Speed;
        //bool Up;
        private int RawValue;// => IntValue.Value;
        //public IntValue IntValue;


        public int Value => RawValue;// IntValue.Value;// ?? RawValue;

        public IntMod PostOp;
        public int CalcValue;

        Action Action;

        MeasuredText Text;
        Paint Back;

        public float Range = 255f;
        public float RangeLow = 0f;
        //int RangeAdd = 10;

        int ExpDigits;

        //string ValueText => Text.Text + " " + Value.ToString("D" + ExpDigits);
        string ValueText => Value.ToString("D" + ExpDigits);

        int TopT;
        int BottomT;
        float Chunk;
        bool SelfDraw;
        //int ValueAtY;

        Point last;
        Rectangle TickMarkZone;
        Rectangle TextValueZone;
        public DragSelect(Paint back, string prompt, int expDigits, Action action, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, int value = 0, IntMod mod = null, bool selfDraw = true) : base(rect, parent, surface)
        {
            SelfDraw = selfDraw;
            PostOp = mod;
            RawValue = value;
            ExpDigits = expDigits;
            Back = back;
            Action = action;// ?? Draw;
            // DragZone = new Zone(Shaper.OverPercent(Rectangle, 200, false, true));
            Center = Shaper.CenterPoint(Rectangle);

            Text = new MeasuredText();
            Text.Text = prompt;

            //AppRoot.Instance.QueAction(SizeMe);
            Text.Font = AppSingleton.FontsList.SelectedFont;
            Text.Point = new Point(X + Width / 10, Y + (2 * Height / 5));

            Chunk = Height * .8f;
            int dif = (Height - (int)Chunk) / 2;
            TopT = Y + dif;
            BottomT = Rectangle.Bottom - dif;
            TickMarkZone = new Rectangle(X, TopT - 1, 5, BottomT - TopT + 1);
            TextValueZone = new Rectangle(X + 4, Text.Point.Y - 1, Width - 5, ClickButton.DefaultHeight + 2);
            MovePreSub();
        }
        protected void SizeMe()
        {
            Text.Font = Surface.SizeFont(ValueText, "Tahoma", Rectangle.Width - 20, Rectangle.Height - 10);
            Text.Point = Surface.CenterText(Text.Font, ValueText, Rectangle);
            //Text.Point.Y += 15;
        }

        public void Set(int value)
        {
            RawValue = value;
            CalcValue = PostOp?.Invoke(value) ?? value;

            //recalc tick mark position
            if (value == Range)
            {
                last = new Point(X, TopT);
                return;
            }
            if (value == RangeLow)
            {
                last = new Point(X, BottomT);
                return;
            }

            var valuerange = Range;
            if (RangeLow < 0)
            {
                int negrange = (int)Math.Abs(RangeLow);
                valuerange += negrange;
                value += negrange;
            }
            
            var rangepoint = valuerange / (valuerange - value);
            var yrange = BottomT - TopT;
            var yflip = yrange / rangepoint;
            last = new Point(X, TopT + (int)yflip);
        }

        public override void Draw()
        {
            //base.Draw();
            Surface.FillRect(Back, Rectangle);
            //Surface.DrawText(Text.Font, Text.Paint, Text.Text, Text.Point);
            //if (On)
            //    Surface.Outline(Text.Paint, Rectangle);

            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, Text.Text, Text.Point.X, Y + 24);

            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, ValueText, Text.Point.X, Text.Point.Y);// Text.Point.X + 30, Text.Point.Y);

            int midX = 10 + X + Rectangle.Width / 2;
            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, Range.ToString(), X + 4, TopT - 4);
            Surface.DrawLine(AppSingleton.DefaultTextColor, midX, TopT, Rectangle.Right, TopT);

            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, RangeLow.ToString(), X + 4, BottomT - 4);
            Surface.DrawLine(AppSingleton.DefaultTextColor, midX, BottomT, Rectangle.Right, BottomT);

            Surface.DrawLine(AppSingleton.DefaultTextColor, X, last.Y, X + 4, last.Y);
            Update();
        }
        public void Update()
        {
            Surface.FillRect(Back, TickMarkZone);

            Surface.FillRect(Back, TextValueZone);

            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, ValueText, Text.Point.X, Text.Point.Y);

            Surface.DrawLine(AppSingleton.DefaultTextColor, X, last.Y, X + 4, last.Y);

        }

        //public override bool IfMoveHit(Point point)
        //{
        //    return DragZone.IfMoveHit(point);
        //}
        //public override bool KeepMove(MouseEventArgs point)
        //{
        //    return true;
        //}

        public override void Move(MouseEventArgs point)
        {
            if (point.Button != MouseButtons.Left)
            {
                return;
            }

            //if (On)
            {
                //if (DragZone.IfHit(point))
                {
                    last = new Point(point.X, point.Y);

                    if (point.Y <= TopT)
                    {
                        Set((int)Range);
                        last.Y = TopT;
                    }
                    else if (point.Y >= BottomT)
                    {
                        Set((int)RangeLow);
                        last.Y = BottomT;
                    }
                    else
                    {
                        //top = Rectangle.Y
                        float y = BottomT - point.Y;

                        float pct = y / Chunk;// (float)Rectangle.Height * .8f;

                        if (pct == 0)
                            pct = 1;
                        else if (pct == 100)
                            pct = 99;
                        //int y = point.Y - Center.Y;

                        //float pct = Math.Abs(y) / (DragZone.Rectangle.Height / 2f);
                        //if (pct != 0 && pct != .5)
                        //{

                        //}
                        if (RangeLow < 0 && Range > 0)
                        {
                            if (pct < .51 && pct > .49)
                            {
                                Set(0);
                            }
                            else if (pct < .5)
                            {
                                Set((int)(RangeLow * ((.5 - pct) * 2)));

                            }
                            else
                            {
                                Set((int)(Range * ((pct - .5) * 2)));
                            }
                        }
                        else
                            Set((int)((Range - RangeLow) * pct));
                        //                        IntValue?.Set(PostCalc((int)((Range - RangeLow) * pct)));
                    }

                    //AppHost.Instance.UIService.QueAction(Action);

                    if (SelfDraw)
                    {
                        Update();
//                        AppRoot.Instance.QueAction(Update);
                        //   ContainerHost.QueUpdate(this);
                    }
                    Action?.Invoke();
                }

            }
        }

        public virtual int PostCalc(int value)
        {
            return value;
        }
    }
}
