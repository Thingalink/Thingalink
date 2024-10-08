using CobbleApp;
using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    class MorphConfig
    {

        public int Pct0;
        public int Pct1;
        public int Pct2;
        public int Pct3;
        public int Pct4;


        public delegate void PaintMod(Paint paint);
        public delegate bool CellMod(Cell cell);
        public delegate bool CellPaintMod(CellPaintSetting cell);

        public bool Mod(Cell cell)
        {
            if (ModRoll != null)
            {
                return ModRoll(cell);
            }
            if (!Exclude(cell))
            {
                return false;
            }
            if (RandomAccess.Percent(Pct0))
            {
                return false;
            }
            if (RandomAccess.Percent(Pct1))
            {
                return Mod1(cell);
            }
            else if (RandomAccess.Percent(Pct2))
            {
                return Mod2(cell);
            }
            else if (Mod3 != null && RandomAccess.Percent(Pct3))
            {
                return Mod3(cell);
            }
            else if (Mod4 != null && RandomAccess.Percent(Pct4))
            {
                return Mod4(cell);
            }
            else
            {
                return ModElse(cell.Paint.Back);
            }
        }

        public CellMod Exclude;
        public CellMod Select;
        public CellMod ModRoll;
        public CellMod Mod1;
        public CellMod Mod2;
        public CellMod Mod3 = null;
        public CellMod Mod4 = null;
        public CellPaintMod ModElse;

        //public PaintMod Whiter;
        //public PaintMod Blacker;
        //public PaintMod Reder;
        //public PaintMod Greener;
        //public PaintMod Bluer;
        //public PaintMod Purpler;
        //public PaintMod Oranger;
        //public PaintMod Yellower;
    }

    class LakeshoreConfig
    {
        static MorphConfig Config;
        //static PaintBox PaintBox;
        public static void SetConfig()//PaintBox paintBox)
        {
            //PaintBox = paintBox;
            Config = new MorphConfig();


            Config.ModRoll = Spread;

            Config.Exclude = Cellection;
            Config.Select = CellAction;
            Config.Pct0 = 0;
            Config.Pct1 = 20;
            Config.Pct2 = 14;
            Config.Pct3 = 1;
            Config.Pct4 = 5;
            // Config.ModRoll = CellRoll;
            Config.Mod1 = FountainSpread;
            Config.Mod2 = MoreSpread;
            Config.ModElse = Morph;
        }

        public static int Tword(int at, int to, double chunk = .2)
        {
            if (at == to)
            {
                return 0;
            }
            else if (at > to)
            {
                return System.Math.Min((int)((to - at) * chunk), -1);
            }
            else
            {

                return System.Math.Max((int)((to - at) * chunk), 1);
            }
        }
        public static bool TickAt(Cell cell)
        {
            if (!Config.Exclude(cell))
            {
                return false;
            }

            if (Config.Select(cell))
            {
                return true;
            }

            return Config.Mod(cell);

            // return TickAtol(cell);
        }
        public static bool Cellection(Cell cell)
        {
            if (cell.Paint.Back.Target == null)
                return false;

            return true;
        }
        public static bool CellRoll(Cell cell)
        {
            if (RandomAccess.Percent(Config.Pct1))
                return Config.Mod1(cell);// FountainSpread(cell);
            else if (RandomAccess.Percent(Config.Pct2))
                return Config.Mod2(cell);//return MoreSpread(cell);
            else if (RandomAccess.Percent(Config.Pct3))
            {//sparkle temp change CellAction returns to target 
                cell.Paint.Back.Paint.Set(cell.Paint.Back.Origin?.Color ?? DrawConfig.SelectedPaint.Color);
                return true;
            }
            else if (RandomAccess.Percent(Config.Pct4))
            {
                return Config.ModElse(cell.Paint.Back); // Morph(cell);
            }
            return false;
        }
        public static bool CellAction(Cell cell)
        {
            if (cell.Paint.Back.Target.Color != cell.Paint.Back.Paint.Color)
            {
                cell.Paint.Back.Paint.SetAdd(Tword(cell.Paint.Back.Paint.Color.R, cell.Paint.Back.Target.Color.R, .2),
                    Tword(cell.Paint.Back.Paint.Color.G, cell.Paint.Back.Target.Color.G, .2),
                    Tword(cell.Paint.Back.Paint.Color.B, cell.Paint.Back.Target.Color.B, .2));
                return true;
            }

            return false;
        }
        public static bool CellOrgin(Cell cell)
        {
            if (cell.Paint.Back.Origin != null)
            {
                if (RandomAccess.Percent(60))
                    FountainSpread(cell);
                else if (RandomAccess.Percent(80))
                    MoreSpread(cell);
                else if (RandomAccess.Percent(1))
                {
                    cell.Paint.Back.Paint.Set(cell.Paint.Back.Origin.Color);
                }
                else
                {
                    Morph(cell.Paint.Back);
                }
                return true;
            }
            return false;
        }
        public static bool TickAtol(Cell cell)
        {
            if (!Cellection(cell))
            {
                return false;
            }

            if (CellAction(cell))
            {
                return true;
            }

            return CellOrgin(cell);

            //if (cell.Origin != null) //cell.Target != null)
            //{

            //    //    FountainSpread(cell);
            //    //}
            //    if (RandomAccess.Percent(60))
            //        FountainSpread(cell);
            //    else if (RandomAccess.Percent(80))
            //        MoreSpread(cell);
            //    else if (RandomAccess.Percent(1))
            //    {
            //        cell.Paint.Set(cell.Origin.Color);
            //    }
            //    else
            //    {
            //        Morph(cell);
            //    }

            //    return true;
            //}
        }



        public static bool Spread(Cell from)
        {
            //Cell cell = from.NeighborAny();
            //if (cell == null || cell.Target != null)
            //    return FountainSpread(from);// Mod(from, from.Target, from.OcilROn, from.OcilGOn, from.OcilBOn);
            if (from.Paint.Back.OcilROn || from.Paint.Back.OcilGOn || from.Paint.Back.OcilBOn)
                return Mod(from, from.Paint.Back.Target, from.Paint.Back.OcilROn, from.Paint.Back.OcilGOn, from.Paint.Back.OcilBOn);
            //return Mod(cell, from.Target);

            Cell cell = (Cell)from.NeighborAny();
            if (cell != null)
            {
                if (cell.Paint.Back.Target != null && RandomAccess.Percent(20))
                    return FountainTarget(cell, from.Paint.Back.Target);
                else if (RandomAccess.Percent(50))
                    return FountainTarget(cell, from.Paint.Back.Target);
                else if (RandomAccess.Percent(50))
                    return MoreSpread(from);//will neighbor again ok
                else
                    return Morph(cell.Paint.Back);
            }
            return false;
        }
        public static int OcilMuch()
        {
           // return 2;
            return RandomAccess.Next(4, 5);
        }

        public static bool Ocil(Cell cell)
        {
            bool did = cell.PS.OcilROn || cell.PS.OcilGOn || cell.PS.OcilBOn;
            int much;
            if (DrawConfig.Oscillator.OcilROn && cell.PS.OcilROn)
            {
                if (cell.Paint.Back.OcilR == 1)
                {
                    much = cell.PP.Color.R + OcilMuch();

                    if (much >= cell.Paint.Back.OcilRHigh)
                    {
                        cell.PP.SetR(cell.Paint.Back.OcilRHigh);
                        cell.Paint.Back.OcilR = 0;
                    }
                    else
                        cell.PP.SetR(much);
                }
                else
                {
                    much = cell.PP.Color.R - OcilMuch();

                    if (much <= cell.Paint.Back.OcilRLow)
                    {
                        cell.PP.SetR(cell.Paint.Back.OcilRLow);
                        cell.Paint.Back.OcilR = 1;
                    }
                    else
                        cell.PP.SetR(much);
                }
                //did = true;
            }

            if (DrawConfig.Oscillator.OcilGOn && cell.PS.OcilGOn)
            {
                if (cell.Paint.Back.OcilG == 1)
                {
                    much = cell.PP.Color.G + OcilMuch();

                    if (much >= cell.Paint.Back.OcilGHigh)
                    {
                        cell.PP.SetG(cell.Paint.Back.OcilGHigh);
                        cell.Paint.Back.OcilG = 0;
                    }
                    else
                        cell.PP.SetG(much);
                }
                else
                {
                    much = cell.PP.Color.G - OcilMuch();

                    if (much <= cell.Paint.Back.OcilGLow)
                    {
                        cell.PP.SetG(cell.Paint.Back.OcilGLow);
                        cell.Paint.Back.OcilG = 1;
                    }
                    else
                        cell.PP.SetG(much);
                }
                //did = true;
            }
            if (DrawConfig.Oscillator.OcilBOn && cell.PS.OcilBOn)
            {
                if (cell.Paint.Back.OcilB == 1)
                {
                    much = cell.PP.Color.B + OcilMuch();

                    if (much >= cell.Paint.Back.OcilBHigh)
                    {
                        cell.PP.SetB(cell.Paint.Back.OcilBHigh);
                        cell.Paint.Back.OcilB = 0;
                    }
                    else
                        cell.PP.SetB(much);
                }
                else
                {
                    much = cell.PP.Color.B - OcilMuch();

                    if (much <= cell.Paint.Back.OcilBLow)
                    {
                        cell.PP.SetB(cell.Paint.Back.OcilBLow);
                        cell.Paint.Back.OcilB = 1;
                    }
                    else
                        cell.PP.SetB(much);
                }
                //did = true;
            }
            return did;
        }
        public static bool Mod(Cell cell, Paint target, bool rOn, bool gOn, bool bOn)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            //Ocil(cell, target, rOn, gOn, bool);
            if (Oscillator.Instance.OcilROn && rOn)//cell.OcilROn)
            {
                if (target.Color.R > 254)// && RandomAccess.Fifty50)
                {
                    r = RandomAccess.Next(-10, -1);
                    cell.Paint.Back.OcilR = 1;
                }
                else if (target.Color.R == 0)//&& RandomAccess.Fifty50)
                {
                    r = RandomAccess.Next(1, 10);
                    cell.Paint.Back.OcilR = 0;
                }
                else if (cell.Paint.Back.OcilR == 1)
                {
                    r = RandomAccess.Next(-5, -1);
                }
                else
                {
                    r = RandomAccess.Next(1, 5);
                }
            }

            if (Oscillator.Instance.OcilGOn && gOn)//cell.OcilGOn)
            {
                if (target.Color.G > 254 && RandomAccess.Fifty50)
                {
                    g = RandomAccess.Next(-10, -1);
                    cell.Paint.Back.OcilG = 1;
                }
                else if (target.Color.G == 0 && RandomAccess.Fifty50)
                {
                    g = RandomAccess.Next(1, 10);
                    cell.Paint.Back.OcilG = 0;
                }
                else if (cell.Paint.Back.OcilG == 1)
                {
                    g = RandomAccess.Next(-5, -1);
                }
                else
                {
                    g = RandomAccess.Next(1, 5);
                }
            }
            if (Oscillator.Instance.OcilBOn && bOn)// cell.OcilBOn)
            {
                if (target.Color.B > 254 && RandomAccess.Fifty50)
                {
                    b = RandomAccess.Next(-10, -1);
                    cell.Paint.Back.OcilB = 1;
                }
                if (target.Color.B == 0 && RandomAccess.Fifty50)
                {
                    b = RandomAccess.Next(1, 10);
                    cell.Paint.Back.OcilB = 0;
                }
                else if (cell.Paint.Back.OcilB == 1)
                {
                    b = RandomAccess.Next(-5, -1);
                }
                else
                {
                    b = RandomAccess.Next(1, 5);
                }
            }

            if (cell.Paint.Back.Target == null)
            {

                cell.Paint.Back.Target = new Paint(target.Color.A, target.Color.R + r, target.Color.G + g, target.Color.B + b);
                //if (cell.Origin == null)
                //{
                //    cell.Origin = new Paint(from.Target.Color.A, r, g, b);
                //}
            }
            else
            {
                cell.Paint.Back.Target.SetAdd(r, g, b);
            }

            cell.Paint.Back.Paint.Set(cell.Paint.Back.Target.Color);

            if (cell.Paint.Back.Origin == null)
            {
                cell.Paint.Back.Origin = target;
            }
            DrawConfig.ModCells.Add(cell);
            //Fountains.Add(cell);
            return true;
        }
        public static bool FountainTarget(Cell cell, Paint paint)
        {
            if (cell.Paint.Back.Target == null)
            {
                int r = 1;
                int g = 1;
                int b = 1;
                if (paint.Color.R > 254 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(-10, -1);

                if (paint.Color.G > 254 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(-10, -1);

                if (paint.Color.B > 254 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(-10, -1);

                if (paint.Color.R == 0 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(1, 10);

                if (paint.Color.G == 0 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(1, 10);

                if (paint.Color.B == 0 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(1, 10);

                cell.Paint.Back.Target = new Paint(paint.Color.A, r, g, b);
                if (cell.Paint.Back.Origin == null)
                {
                    cell.Paint.Back.Origin = new Paint(paint.Color.A, r, g, b);
                }
            }
            else
            {
                cell.Paint.Back.Target.SetAdd(Tword(cell.Paint.Back.Target.Color.R, paint.Color.R, 1 / RandomAccess.Next(2, 5)),
                    Tword(cell.Paint.Back.Target.Color.G, paint.Color.G, 1 / RandomAccess.Next(2, 5)),
                    Tword(cell.Paint.Back.Target.Color.B, paint.Color.B, 1 / RandomAccess.Next(2, 5)));

                // cell.Target.SetAdd(RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10));
            }

            DrawConfig.ModCells.Add(cell);
            return true;
        }
        public static bool PaintSpread(Cell from)
        {
            if(Ocil(from))
            {
                DrawConfig.Matrix.DrawCell(from);
                return true;
            }

            if (from.PP.Color.R + from.PP.Color.G + from.PP.Color.B > 700)
            {

                from.PP.SetAdd(RandomAccess.Next(-100, -10), RandomAccess.Next(-100, -10), RandomAccess.Next(-100, -10));
            }
            Cell cell = (Cell)from.NeighborAny();
            if (cell == null)
                return false;
           // if (cell.PP == null)
            {
                int r = from.PP.Color.R;
                int g = from.PP.Color.G;
                int b = from.PP.Color.B;
                if (from.PP.Color.R > 254 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(-10, -1);

                if (from.PP.Color.G > 254 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(-10, -1);

                if (from.PP.Color.B > 254 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(-10, -1);

                if (from.PP.Color.R == 0 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(1, 10);

                if (from.PP.Color.G == 0 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(1, 10);

                if (from.PP.Color.B == 0 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(1, 10);

                cell.PS.Paint = new Paint(from.PP.Color.A, r, g, b);
                cell.PP.SetAdd(RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10));
                //if (cell.Paint.Back.Origin == null)
                //{
                //    cell.Paint.Back.Origin = new Paint(from.PP.Color.A, r, g, b);
                //}
            }
            //else
            //{
            //    cell.PP.SetAdd(RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10));
            //}

            if (cell.Paint.Back.Origin == null)
            {
                cell.Paint.Back.Origin = from.Paint.Back.Origin;
            }
//            DrawConfig.ModCells.Add(cell);
            DrawConfig.Matrix.DrawCell(cell);
            //Fountains.Add(cell);
            return true;
        }

        public static bool FountainSpread(Cell from)
        {
            if (from.Paint.Back.Target.Color.R + from.Paint.Back.Target.Color.G + from.Paint.Back.Target.Color.B > 700)
            {

                from.Paint.Back.Target.SetAdd(RandomAccess.Next(-100, -10), RandomAccess.Next(-100, -10), RandomAccess.Next(-100, -10));
            }
            Cell cell = (Cell)from.NeighborAny();
            if (cell == null)
                return false;
            if (cell.Paint.Back.Target == null)
            {
                int r = 1;
                int g = 1;
                int b = 1;
                if (from.Paint.Back.Target.Color.R > 254 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(-10, -1);

                if (from.Paint.Back.Target.Color.G > 254 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(-10, -1);

                if (from.Paint.Back.Target.Color.B > 254 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(-10, -1);

                if (from.Paint.Back.Target.Color.R == 0 && RandomAccess.Fifty50)
                    r = RandomAccess.Next(1, 10);

                if (from.Paint.Back.Target.Color.G == 0 && RandomAccess.Fifty50)
                    g = RandomAccess.Next(1, 10);

                if (from.Paint.Back.Target.Color.B == 0 && RandomAccess.Fifty50)
                    b = RandomAccess.Next(1, 10);

                cell.Paint.Back.Target = new Paint(from.Paint.Back.Target.Color.A, r, g, b);
                if (cell.Paint.Back.Origin == null)
                {
                    cell.Paint.Back.Origin = new Paint(from.Paint.Back.Target.Color.A, r, g, b);
                }
            }
            else
            {
                cell.Paint.Back.Target.SetAdd(RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10), RandomAccess.Next(-10, 10));
            }

            if (cell.Paint.Back.Origin == null)
            {
                cell.Paint.Back.Origin = from.Paint.Back.Origin;
            }
            DrawConfig.ModCells.Add(cell);
            //Fountains.Add(cell);
            return true;
        }
        public static int MorphMuch()
        {
            return RandomAccess.Next(-20, 20);
        }
        public static int MorphSome()
        {
            return RandomAccess.Next(-10, 10);
        }
        public static int MorphDarker()
        {
            return RandomAccess.Next(-100, -25);
        }
        public static int MorphLighter()
        {
            return RandomAccess.Next(25, 100);
        }
        public static bool Morph(CellPaintSetting cell)
        {
            int r = RandomAccess.Next(0, 3);
            switch (r)
            {
                case 0:
                    cell.Paint.SetR(cell.Paint.Color.R + MorphMuch());
                    break;
                case 1:
                    cell.Paint.SetG(cell.Paint.Color.G + MorphMuch());
                    break;
                case 2:
                    cell.Paint.SetB(cell.Paint.Color.B + MorphMuch());
                    break;
            }
            return true;
        }
        public static bool MorphSome(Cell cell, Cell from)
        {
            if (cell.PP == null)
            {
                cell.PS.Paint = new Paint(from.PP.Color);
            }

            int r = RandomAccess.Next(0, 3);
            switch (r)
            {
                case 0:
                    cell.PP.SetR(from.PP.Color.R + MorphSome());
                    break;
                case 1:
                    cell.PP.SetG(from.PP.Color.G + MorphSome());
                    break;
                case 2:
                    cell.PP.SetB(from.PP.Color.B + MorphSome());
                    break;
            }
            return true;
        }
        public static void Morph(Cell cell, Cell source)
        {
            if (cell.PP == null)
            {
                cell.PS.Paint = new Paint(source.PP.Color);
            }

            int r = RandomAccess.Next(0, 3);
            switch (r)
            {
                case 0:
                    cell.PP.SetR(source.PP.Color.R + MorphMuch());
                    break;
                case 1:
                    cell.PP.SetG(source.PP.Color.G + MorphMuch());
                    break;
                case 2:
                    cell.PP.SetB(source.PP.Color.B + MorphMuch());
                    break;
            }
        }
        public static void MorphTword(Cell cell, Cell from, Cell to, double factor = .2)
        {
            if (cell.PP == null)
            {
                cell.PS.Paint = new Paint(from.PP.Color);
            }

            cell.PP.SetAdd(
                        LakeshoreConfig.Tword(from.PP.Color.R, to.PP.Color.R, factor),
                       LakeshoreConfig.Tword(from.PP.Color.G, to.PP.Color.G, factor),
                       LakeshoreConfig.Tword(from.PP.Color.B, to.PP.Color.B, factor));

        }

        //public static void Blend(Cell cell, Cell source1, Cell source2)
        //{
        //    cell.PS.Paint = Blend(source1.PP.Color, source2.PP.Color);
        //}
        public static Paint Blend(Color source1, Color source2)
        {
            return new Paint(source1.A + Tword(source1.A, source2.A, .5),
                source1.R + Tword(source1.R, source2.R, .5),
                source1.G + Tword(source1.G, source2.G, .5),
                source1.B + Tword(source1.B, source2.B, .5));
        }

        public static void Lighter(CellPaintSetting cell)
        {
            int r = RandomAccess.Next(0, 3);
            switch (r)
            {
                case 0:
                    cell.Target.SetR(cell.Target.Color.R + MorphLighter());
                    break;
                case 1:
                    cell.Target.SetG(cell.Target.Color.G + MorphLighter());
                    break;
                case 2:
                    cell.Target.SetB(cell.Target.Color.B + MorphLighter());
                    break;
            }
        }
        public static void Darker(CellPaintSetting cell)
        {
            int r = RandomAccess.Next(0, 3);
            switch (r)
            {
                case 0:
                    cell.Target.SetR(cell.Target.Color.R + MorphDarker());
                    break;
                case 1:
                    cell.Target.SetG(cell.Target.Color.G + MorphDarker());
                    break;
                case 2:
                    cell.Target.SetB(cell.Target.Color.B + MorphDarker());
                    break;
            }
        }

        public static bool MoreSpread(Cell from)
        {
            int sum = from.Paint.Back.Target.Sum();
            if (sum > 700)
            {
                Darker(from.Paint.Back);
                return true;
            }

            if (sum < 40)
            {
                Lighter(from.Paint.Back);
                return true;
            }

            Cell cell = (Cell)from.NeighborAny();
            if (cell == null)
                return false;

            //if (cell.TargetR != null && cell.TargetR.Int + cell.TargetG.Int + cell.TargetB.Int > sum)
            //    return;
            if (cell.Paint.Back.Target == null)
            {
                cell.Paint.Back.Target = new Paint(from.Paint.Back.Target.Color);
            }
            else if (cell.Paint.Back.Origin == null && RandomAccess.Percent(10))
            {
                cell.Paint.Back.Origin = new Paint(from.Paint.Back.Target.Color);
            }

            if (RandomAccess.Percent(33))
            {
                cell.Paint.Back.Target.Set(from.Paint.Back.Target.Color.R - 30, from.Paint.Back.Target.Color.G, from.Paint.Back.Target.Color.B);
            }
            else if (RandomAccess.Percent(33))
            {
                cell.Paint.Back.Target.Set(from.Paint.Back.Target.Color.R, from.Paint.Back.Target.Color.G - 30, from.Paint.Back.Target.Color.B);
            }
            else
            {
                cell.Paint.Back.Target.Set(from.Paint.Back.Target.Color.R, from.Paint.Back.Target.Color.G, from.Paint.Back.Target.Color.B - 30);
            }

            DrawConfig.ModCells.Add(cell);
            //Fountains.Add(cell);
            return true;
        }
    }
}
