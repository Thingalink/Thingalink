using CobbleApp;
using CobbleControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    public class PaintedMatrix : SquareMatrix
    {
        ModRollSheet RollSheet;

        public DateTime DrawTimer;

        public ListHead MovieFrames;

        public ListMember FrameItem;

        public Dictionary<Point, Cell> RecentCells;
        //public ListHead RecentCells;

        protected bool ready;
        public bool IsReady => ready;

        public PaintedMatrix(ListHead frames, Rectangle rect, int initialCells, ContainerZone parent, Bitmap bitmap = null) : base(rect, initialCells, parent, bitmap)
        {
            MovieFrames = frames ?? new ListHead();
            InitRecentCells();
        }

        public void InitGuts(int cellSide = 1)
        {
            if (cellSide != 1)
            {
                //todo
            }
            ActaullyInitCells();
            ready = true;
        }
        public void InitRecentCells()
        {
            //RecentCells = new ListHead();
            RecentCells = new Dictionary<Point, Cell>();
        }
        public void LopOldCells()
        {
            //if(RecentCells.LastCount>20)
            //    RecentCells.DeleteFirstN(RecentCells.LastCount/20);
        }

        protected override void InitCells()
        {
        }

        public override void Click(Point point)
        {
            RollSheet = new ModRollSheet(point, 0);
            base.Click(point);
        }

        public bool CheckOver(Cell c)
        {
            if (DrawConfig.Overwrite.On)
            {
                if (c.PP == null)
                    return true;

                return CheckOver(c.PP.Color);
            }

            if (DrawConfig.Racer.On && c.Mod.Ticks != InitStamp.Ticks)
                return true;

            return c.Mod.Ticks == InitStamp.Ticks;
        }
        public bool CheckOver(Color c)
        {
            //if (DrawConfig.Clude.On)
            //{
            //    if (c.R >= DrawConfig.OverwriteMin.Paint.Color.R && c.R <= DrawConfig.OverwriteMax.Paint.Color.R
            //           && c.G >= DrawConfig.OverwriteMin.Paint.Color.G && c.G <= DrawConfig.OverwriteMax.Paint.Color.G
            //             && c.B >= DrawConfig.OverwriteMin.Paint.Color.B && c.B <= DrawConfig.OverwriteMax.Paint.Color.B)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
            //else
            //{
            //    if (c.R >= DrawConfig.OverwriteMin.Paint.Color.R && c.R <= DrawConfig.OverwriteMax.Paint.Color.R
            //           || c.G >= DrawConfig.OverwriteMin.Paint.Color.G && c.G <= DrawConfig.OverwriteMax.Paint.Color.G
            //             || c.B >= DrawConfig.OverwriteMin.Paint.Color.B && c.B <= DrawConfig.OverwriteMax.Paint.Color.B)
            //    {
            //        return false;
            //    }
            //    return true;
            //}

            return false;
        }

        public override void Click(Cell c)
        {

            //if (c.Paint == null || c.Paint.Back == null)
            //    return;
            RollSheet.Add();
            Paint(c, RollSheet.Color);

            if (RollSheet.Setting.SplitH > 0 || RollSheet.Setting.SplitV > 0)
            {
                c.NeighborDo(RollSheet.Setting.SplitH, RollSheet.Setting.SplitV, NeighborPaint, null, RollSheet);
            }


            //if (drill > 0)// DrawConfig.Drill.Value
            //{
            //    DrawNextFrames(c, drill, new Point(c.X - Surface.Rectangle.X, c.Y - Surface.Rectangle.Y));

            //}
        }
        int drill => DrawConfig.Drill?.Value ?? 99;


        public override bool Draw()
        {
            if (!ready)
                return false;
            return base.Draw();
        }
        public void Paint(Cell cell, Color color, int distance = 0)
        {
            if(!RecentCells.ContainsKey(cell.Rectangle.Location))
                RecentCells.Add(cell.Rectangle.Location, cell); //.AddUnique(cell);

            if (cell.PP == null)
            {
                cell.PS.Paint = new Paint(color);
                Update(cell);
                return;
            }

            if (CheckOver(cell))
            {
                var c = ModPaint(cell.PP, color, distance);

                cell.PS.Paint = new Paint(c);// 255, r, g, b);


                if (DrawConfig.Oscillator.AssignROn)
                {
                    cell.PS.OcilROn = DrawConfig.Oscillator.OcilROn;// !cell.PS.OcilROn;
                    cell.PS.OcilRLow = DrawConfig.Oscillator.RUnder.Value;
                    cell.PS.OcilRHigh = DrawConfig.Oscillator.ROver.Value;
                }
                if (DrawConfig.Oscillator.AssignGOn)
                {
                    cell.PS.OcilGOn = DrawConfig.Oscillator.OcilGOn;// !cell.PS.OcilGOn;
                    cell.PS.OcilGLow = DrawConfig.Oscillator.GUnder.Value;
                    cell.PS.OcilGHigh = DrawConfig.Oscillator.GOver.Value;
                }
                if (DrawConfig.Oscillator.AssignBOn)
                {
                    cell.PS.OcilBOn = DrawConfig.Oscillator.OcilBOn;// !cell.PS.OcilBOn;
                    cell.PS.OcilBLow = DrawConfig.Oscillator.BUnder.Value;
                    cell.PS.OcilBHigh = DrawConfig.Oscillator.BOver.Value;
                }

                Update(cell);
            }
        }
        public Color ModPaint(Paint from, Color color, int distance = 0)
        {
            return ModColor(from.Color);// DrawConfig.SelectedPaint.Color
        }
        public Color ModColor(Color color)
        {

            //if (DrawConfig.Overwrite.On)
            //{
            //    if (!CheckOver(color))
            //        return color;
            //}

            int r = RollSheet.Color.R;
            int g = RollSheet.Color.G;
            int b = RollSheet.Color.B;

            if (ColorPush())
            {
                r = color.R;
                g = color.G;
                b = color.B;

                if (RollSheet.Setting.RFlat != 0)
                {
                    r += RollSheet.Setting.RFlat;
                }

                if (RollSheet.Setting.GFlat != 0)
                {
                    g += RollSheet.Setting.GFlat;
                }

                if (RollSheet.Setting.BFlat != 0)
                {
                    b += RollSheet.Setting.BFlat;
                }

                if (DrawConfig.ModPush != null && RollSheet.Setting.ModPush && ColorMod())
                {
                    if (RollSheet.Roll.Rmod)// .Setting.Rmod != 0 && RollSheet.Setting.R != 0)
                    {
                        r = ModCValue(RollSheet.Roll.R, RollSheet.Setting.Rswing, 0, r);
                    }

                    if (RollSheet.Roll.Gmod)//.Value != 0 && DrawConfig.G.Value != 0)
                    {
                        g = ModCValue(RollSheet.Roll.G, RollSheet.Setting.Gswing, 0, g);
                    }

                    if (RollSheet.Roll.Bmod)//.Value != 0 && DrawConfig.B.Value != 0)
                    {
                        b = ModCValue(RollSheet.Roll.B, RollSheet.Setting.Bswing, 0, b);
                    }
                }

                return Color.FromArgb(color.A, ColorValue.Conform(r), ColorValue.Conform(g), ColorValue.Conform(b));
            }

            if (ColorMod())
            {

                if (RollSheet.Roll.Rmod)// .Setting.Rmod != 0 && RollSheet.Setting.R != 0)
                {
                    r = ModCValue(RollSheet.Roll.R, RollSheet.Setting.Rswing, 0, r);
                }

                if (RollSheet.Roll.Gmod)//.Value != 0 && DrawConfig.G.Value != 0)
                {
                    g = ModCValue(RollSheet.Roll.G, RollSheet.Setting.Gswing, 0, g);//RollSheet.Setting.GRadial
                }

                if (RollSheet.Roll.Bmod)//.Value != 0 && DrawConfig.B.Value != 0)
                {
                    b = ModCValue(RollSheet.Roll.B, RollSheet.Setting.Bswing, 0, b);
                }
            }



            if (RollSheet.Setting.Blend)
            {
                return CobbleApp.Paint.AddTo(color, 255,
                    LakeshoreConfig.Tword(color.R, r, .2),
                   LakeshoreConfig.Tword(color.G, g, .2),
                   LakeshoreConfig.Tword(color.B, b, .2));
            }
            else
            {
                return Color.FromArgb(RollSheet.Color.A, ColorValue.Conform(r), ColorValue.Conform(g), ColorValue.Conform(b));
            }
        }

        //void DrawNextFrames(Cell c, int depth, Point point)
        //{
        //    ListMember item;
        //    if (depth < 0)
        //    {
        //        item = FrameItem?.Next ?? MovieFrames.Last;
        //        while (depth < 0 && item != null && c != null)
        //        {
        //            c = DrawFrameNext(c, item, point);
        //            item = item.Previous;
        //            depth++;
        //        }
        //        return;
        //    }
        //    //DrawTimer = DateTime.Now;
        //    item = FrameItem?.Next ?? MovieFrames.First;
        //    while (depth > 0 && item != null && c != null)
        //    {
        //        c = DrawFrameNext(c, item, point);
        //        item = item.Next;
        //        depth--;
        //    }
        //    //var ts = new TimeSpan(DateTime.Now.Ticks - DrawTimer.Ticks);
        //    //Status.Log("Frame Draw Clock " + ts.TotalMilliseconds);
        //}
        void DrawThisFrame(Bitmap Bitmap, int depth, Point point)
        {
            //DrawTimer = DateTime.Now;

            //Color c = Bitmap.GetPixel(point.X, point.Y);
            //c = ModColor(c);
            //Bitmap.SetPixel(point.X, point.Y, c);

            //var ts = new TimeSpan(DateTime.Now.Ticks - DrawTimer.Ticks);
            //Status.Log("SetPixel Clock " + ts.TotalMilliseconds);


            //DrawTimer = DateTime.Now;




            BitmapSurface.ModPixel(Bitmap, point, ModColor);

            //ts = new TimeSpan(DateTime.Now.Ticks - DrawTimer.Ticks);
            //Status.Log("ModPixel Clock " + ts.TotalMilliseconds);
        }

        public Point Motor(Point p)
        {
            if (DrawConfig.MotorH.Value == 0 && DrawConfig.MotorV.Value == 0)
            {
                return p;
            }
            int x = p.X + DrawConfig.MotorH.Value;
            int y = p.Y + DrawConfig.MotorV.Value;
            if (DrawConfig.MotorH.Value != 0 && RollSheet.Roll.Zig)
            {
                if (RollSheet.Roll.LurchX != 0)
                {
                    x += RollSheet.Roll.LurchX;
                    y += RollSheet.Roll.LurchX / 2;
                }
            }
            //            x += RandomAccess.Next(0, DrawConfig.MotorH.Value);

            if (DrawConfig.MotorV.Value != 0 && RollSheet.Roll.Zag)
            {
                y += RollSheet.Roll.LurchY;
                x += RollSheet.Roll.LurchX / 2;
            }

            return new Point(x, y);

        }
        int AddTo(int i, int more)
        {
            if (i >= 0)
            {
                return i + more;
            }

            return i - more;

        }
        Cell ModX(Cell cell, int x)
        {
            if (x > 0)
                return cell.NeighborE;
            else// if (x < 0)
                return cell.NeighborW;
            //else
            //    return cell;
        }
        Cell ModY(Cell cell, int y)
        {
            if (y > 0)
                return cell.NeighborS;
            else// if (x < 0)
                return cell.NeighborN;
            //else
            //    return cell;
        }
        Cell ModXRoll(Cell cell, int x)
        {
            var c = ModX(cell, x);

            if (c == null && RollSheet.Setting.Wrap)
            {
                if (x > 0)//cell.X == Rectangle.X)
                    c = ChildCell(new Point(Rectangle.X, cell.Y));
                else //if (cell.X == Rectangle.Right - 1)
                    c = ChildCell(new Point(Rectangle.Right - 1, cell.Y));
                // else
                //    c = ChildCell(new Point(Rectangle.Right, Rectangle.Y));

            }
            return c;
        }
        Cell ModYRoll(Cell cell, int y)
        {
            var c = ModY(cell, y);

            if (c == null && RollSheet.Setting.Wrap)
            {
                if (y > 0)
                    c = ChildCell(new Point(cell.X, Rectangle.Y));
                else //if (cell.X == Rectangle.Right - 1)
                    c = ChildCell(new Point(cell.X, Rectangle.Bottom - 1));
                //if (cell.Y == Rectangle.Y)
                //    c = ChildCell(new Point(Rectangle.X, Rectangle.Bottom));
                //else if (cell.Y == Rectangle.Bottom - 1)
                //    c = ChildCell(new Point(Rectangle.X, Rectangle.Y));

            }
            return c;
        }
        //Cell DrawFrameNext(Cell cell, ListMember item, Point point, int wobble = 0)
        //{
        //    var frame = (MovieFrame)item.Contains.Other;
        //    if (frame.Bitmap == null)
        //        return null;
        //    DrawFrramel = frame;

        //    RollSheet.ReUse();
        //    RollSheet.Next();

        //    var pointM = Motor(point);
        //    int xM = pointM.X - point.X;
        //    int yM = pointM.Y - point.Y;

        //    bool diag;

        //    Cell cellM = cell;
        //    Cell callTarget = null;
        //    while (xM != 0 && yM != 0 && cellM != null)
        //    {
        //        int xAbs = Math.Abs(xM);
        //        int yAbs = Math.Abs(yM);

        //        double xBit = xAbs * .1;
        //        double yBit = yAbs * .1;

        //        int diff = xAbs - yAbs;

        //        if (xM == 0 || yM == 0)
        //        {
        //            if (xM > 0)
        //            {
        //                callTarget = ModXRoll(cellM, xM);
        //                xM = AddTo(xM, -1);
        //            }
        //            else
        //            {
        //                callTarget = ModYRoll(cellM, yM);
        //                yM = AddTo(yM, -1);
        //            }
        //            continue;
        //        }

        //        if ((diff > 0 && RandomAccess.Percent(Math.Max(10, 100 - (diff * 10)))) || RandomAccess.Percent(100 - Math.Max(10, 100 - (diff * -10))))
        //        {
        //            callTarget = ModXRoll(cellM, xM);
        //            xM = AddTo(xM, -1);
        //        }
        //        else
        //        {
        //            callTarget = ModYRoll(cellM, yM);
        //            yM = AddTo(yM, -1);
        //        }

        //        cellM = callTarget;
        //    }
        //    if (cellM == null)
        //    {
        //        return null;
        //    }

        //    DrawThisFrame(frame.Bitmap, 0, point);

        //    // Color c = frame.Bitmap.GetPixel(point.X, point.Y);
        //    // c = ModColor(c);
        //    ////             BitmapSurface.ModPixel(frame.Bitmap, point, ModColor);
        //    // frame.Bitmap.SetPixel(point.X, point.Y, c);


        //    if (DrawConfig.SplitH.Value > 0 || DrawConfig.SplitV.Value > 0)
        //    {
        //        cellM.NeighborDo(DrawConfig.SplitH.Value, DrawConfig.SplitV.Value, NeighborPaintFrame, cell, RollSheet);
        //    }
        //    return cellM;
        //}

        //private void NeighborPaintFrame(NeighborZone zone, NeighborZone from)
        //{
        //    if (RandomAccess.Percent(DrawConfig.Decay.Value))
        //        return;

        //    var c = (Cell)zone;
        //    DrawThisFrame(DrawFrramel.Bitmap, 0, new Point(c.X - Surface.Rectangle.X, c.Y - Surface.Rectangle.Y));
        //    //DrawNextFrames(drill, new Point(c.X - Surface.Rectangle.X, c.Y - Surface.Rectangle.Y));
        //    //Paint(c, DrawConfig.SelectedPaint.Color, Distance(zone, from));
        //}

        protected bool ColorPush()
        {
            return DrawConfig.Push.On && (DrawConfig.RFlat.Value != 0 || DrawConfig.GFlat.Value != 0 || DrawConfig.BFlat.Value != 0);
        }
        protected bool ColorMod()
        {
            if (DrawConfig.R == null || DrawConfig.G == null || DrawConfig.B == null)
                return false;

            if (DrawConfig.Rmod.Value != 0 && DrawConfig.R.Value != 0)
                return true;

            if (DrawConfig.Gmod.Value != 0 && DrawConfig.G.Value != 0)
                return true;

            if (DrawConfig.Bmod.Value != 0 && DrawConfig.B.Value != 0)
                return true;

            return false;
        }
        protected int ModCValue(int change, int swing, int radial, int from)//, int distance)
        {
            int to = from;

            if (change > 0)
            {
                //int roll =  RandomAccess.Next(from, from + change);
                int roll = from + change;

                if (swing < 100)
                {
                    roll = from + LakeshoreConfig.Tword(from, roll, swing * .01);
                }

                if (roll > 255)
                {
                    if (RandomAccess.Percent(roll - 255))
                    {
                        roll = 255;

                    }
                    else
                    {
                        roll = 255 - RandomAccess.Next(0, 100 - (roll - 255));
                    }
                }
                to = roll;
            }
            else if (change < 0)
            {
                //int roll = RandomAccess.Next(from + change, from);
                int roll = from + change;

                if (swing < 100)
                {
                    roll = from + LakeshoreConfig.Tword(from, roll, swing * .01);
                }

                if (roll < 0)
                {
                    if (RandomAccess.Percent(-roll))
                    {
                        roll = 0;

                    }
                    else
                    {
                        roll = RandomAccess.Next(0, 100 + roll);
                    }
                }
                to = roll;
            }

            return to;
        }


        protected int ModCValue(DragSelect pct, DragSelect change, DragSelect swing, DragSelect radial, int from)//, int distance)
        {
            int to = from;
            if (RandomAccess.Percent(pct.Value))
            {
                if (change.Value > 0)
                {
                    int roll = RandomAccess.Next(from, from + change.Value);

                    if (swing.Value < 100)
                    {
                        roll = from + LakeshoreConfig.Tword(from, roll, swing.Value * .01);
                    }

                    if (roll > 255)
                    {
                        if (RandomAccess.Percent(roll - 255))
                        {
                            roll = 255;

                        }
                        else
                        {
                            roll = 255 - RandomAccess.Next(0, 100 - (roll - 255));
                        }
                    }
                    to = roll;
                }
                else if (change.Value < 0)
                {
                    int roll = RandomAccess.Next(from + change.Value, from);

                    if (swing.Value < 100)
                    {
                        roll = from + LakeshoreConfig.Tword(from, roll, swing.Value * .01);
                    }

                    if (roll < 0)
                    {
                        if (RandomAccess.Percent(-roll))
                        {
                            roll = 0;

                        }
                        else
                        {
                            roll = RandomAccess.Next(0, 100 + roll);
                        }
                    }
                    to = roll;
                }

            }
            return to;
        }

        private void NeighborPaint(NeighborZone zone, NeighborZone from)
        {
            RollSheet.Add();

            if (RollSheet.Roll.Decay)// RandomAccess.Percent(DrawConfig.Decay.Value))
                return;

            var c = (Cell)zone;

            Paint(c, DrawConfig.SelectedPaint.Color, Distance(zone, from));
        }

        public int Distance(NeighborZone zone, NeighborZone from)
        {
            if (from == null)
                return 0;
            return (Math.Abs(zone.X - from.X) + Math.Abs(zone.Y - from.Y)) / 2;
        }

        public virtual void Morph()
        {
            BankSet.Loop(MorphBank);
            Updates = true;
        }
        public virtual void MorphAll()
        {
            if (DrawConfig.MorphAll.On)
            {
                BankSet.LoopAll(MorphBank, null);
            }
            else
            {
                //RecentCells.Iterate(BankRef);
                foreach(var item in RecentCells)
                {
                    Morph((Cell)item.Value);
                }
            }

            Updates = true;
        }
        public void MorphBank(ListMember item)
        {
            ((ListHead)item.Object).Iterate(BankRef);
        }
        public void BankRef(ListMember item)
        {
            Morph((Cell)item.Object);
        }
        public void Morph(Cell cell)
        {
            if (cell.PP != null)
            {
                if (cell.PS.Origin == null)
                {
                    cell.PS.Origin = cell.PP;
                }

                LakeshoreConfig.PaintSpread(cell);
            }
        }
        public int ScaleChange(int target, int at)
        {
            int diff = target - at;
            int scale = Math.Abs(diff);
            if (scale > 20)
            {
                return at + (diff / 3);
            }
            else if (scale > 10)
            {
                return at + (diff / 2);
            }
            else
            {
               // Over = true;
                return at + diff + RandomAccess.Next(-scale, scale);
            }
        }
    }
    
}
