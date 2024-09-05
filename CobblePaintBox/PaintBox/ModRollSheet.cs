using System.Drawing;
using Thingalink;

namespace CobblePaintBox
{
    public class ModRollSheet
    {
        public RollSeries Rolls;
        public ModRoll Roll;

        public Point ClickAt;
        public int Frame;

        public Color Color;// Paint;
        public Bitmap ImageBrush;
        public DrawConfigSetting Setting;

        public ModRollSheet(Point click, int frame)
        {
            ClickAt = click;
            Frame = frame;

            Color = DrawConfig.SelectedPaint.Color;
            Rolls = new RollSeries();
            Setting = new DrawConfigSetting();
            Setting.Save();
        }
        public void Clear()
        {
            Rolls.Reuse(false);
            Rolls = new RollSeries();
        }
        public void Add()
        {
            Roll = new ModRoll(this);
            Rolls.Add(Roll);
        }
        protected bool ReUseRolls;
        public void ReUse()
        {
            if (Setting.CModReRoll > 0 || Setting.SpreadReRoll > 0)
                Rolls.Reuse(true);
            else
                Clear();

        }

        public void Next()
        {
            if (Rolls.ReUseRolls)
            {
                Roll = Rolls.Next();

                if (Setting.SpreadReRoll > 0)
                {
                    Roll.RollSpread();
                }
                if (Setting.CModReRoll > 0)
                {
                    Roll.RollColor();
                }
            }
            else
                Add();
        }
    }
    public class ModRoll
    {
        public ModRollSheet Sheet;

        public ModRoll(ModRollSheet sheet)
        {
            Sheet = sheet;
            Decay = RandomAccess.Percent(sheet.Setting.Decay);
            //if(Decay)
            //{
            //    return;
            //}

            HBump = RandomAccess.Percent(sheet.Setting.HBump);
            VBump = RandomAccess.Percent(sheet.Setting.VBump);
            BumpDouble = RandomAccess.Percent(sheet.Setting.BumpDouble);
            HBump = RandomAccess.Percent(sheet.Setting.HBump);
            HBump = RandomAccess.Percent(sheet.Setting.HBump);
            Rmod = RandomAccess.Percent(sheet.Setting.Rmod);
            Gmod = RandomAccess.Percent(sheet.Setting.Gmod);
            Bmod = RandomAccess.Percent(sheet.Setting.Bmod);
            Swing = RandomAccess.Percent(sheet.Setting.Swing);
            SwingDir = RandomAccess.Percent(50);
            Shmear = RandomAccess.Percent(sheet.Setting.Shmear);
            DShmear = RandomAccess.Percent(sheet.Setting.DShmear);

            if (Rmod)
            {
                if (sheet.Setting.R > 0)
                    R = RandomAccess.Next(0, sheet.Setting.R);
                else
                    R = RandomAccess.Next(sheet.Setting.R, 1);
            }
            if (Gmod)
            {
                if (sheet.Setting.G > 0)
                    G = RandomAccess.Next(0, sheet.Setting.G);
                else
                    G = RandomAccess.Next(sheet.Setting.G, 1);
            }
            if (Bmod)
            {
                if (sheet.Setting.B > 0)
                    B = RandomAccess.Next(0, sheet.Setting.B);
                else
                    B = RandomAccess.Next(sheet.Setting.B, 1);
            }
            Zig = RandomAccess.Percent(sheet.Setting.MotorZig);
            Zag = RandomAccess.Percent(sheet.Setting.MotorZag);

            if (sheet.Setting.MotorLurchX > 1)
            {
                if (sheet.Setting.MotorH > 0)
                    LurchX = RandomAccess.Next(-sheet.Setting.MotorH * (sheet.Setting.MotorLurchX - 1), sheet.Setting.MotorH * sheet.Setting.MotorLurchX);
                else
                    LurchX = RandomAccess.Next(sheet.Setting.MotorH * sheet.Setting.MotorLurchX, -sheet.Setting.MotorH * (sheet.Setting.MotorLurchX - 1));

                if (sheet.Setting.MotorV > 0)
                    LurchY = RandomAccess.Next(-sheet.Setting.MotorV * (sheet.Setting.MotorLurchX - 1), sheet.Setting.MotorV * sheet.Setting.MotorLurchX);
                else
                    LurchY = RandomAccess.Next(sheet.Setting.MotorV * sheet.Setting.MotorLurchX, -sheet.Setting.MotorV * (sheet.Setting.MotorLurchX - 1));

            }
            else
            {
                if (sheet.Setting.MotorH > 0)
                    LurchX = RandomAccess.Next(-sheet.Setting.MotorH, sheet.Setting.MotorH * sheet.Setting.MotorLurchX);
                else
                    LurchX = RandomAccess.Next(sheet.Setting.MotorH * sheet.Setting.MotorLurchX, -sheet.Setting.MotorH);

                if (sheet.Setting.MotorV > 0)
                    LurchY = RandomAccess.Next(-sheet.Setting.MotorV, sheet.Setting.MotorV * sheet.Setting.MotorLurchX);
                else
                    LurchY = RandomAccess.Next(sheet.Setting.MotorV * sheet.Setting.MotorLurchX, -sheet.Setting.MotorV);

            }
        }

        public bool HBump;
        public bool VBump;
        public bool BumpDouble;
        public bool Decay;
        public bool Rmod;
        public bool Gmod;
        public bool Bmod;
        public bool Swing;
        public bool Sway;
        public bool SwingDir;
        public bool Shmear;
        public bool DShmear;
        public int R;
        public int G;
        public int B;
        public bool Zig;
        public bool Zag;
        public int LurchX;
        public int LurchY;

        internal void RollSpread()//ModRollSheet sheet)
        {
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                HBump = RandomAccess.Percent(Sheet.Setting.HBump);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                VBump = RandomAccess.Percent(Sheet.Setting.VBump);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                BumpDouble = RandomAccess.Percent(Sheet.Setting.BumpDouble);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Swing = RandomAccess.Percent(Sheet.Setting.Swing);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Sway = RandomAccess.Percent(Sheet.Setting.Sway);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                SwingDir = RandomAccess.Percent(50);
            }
            if (RandomAccess.Percent(Sheet.Setting.Shmear))
            {
                Shmear = RandomAccess.Percent(Sheet.Setting.Shmear);
            }
            if (RandomAccess.Percent(Sheet.Setting.DShmear))
            {
                DShmear = RandomAccess.Percent(Sheet.Setting.DShmear);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Zig = RandomAccess.Percent(Sheet.Setting.MotorZig);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Zag = RandomAccess.Percent(Sheet.Setting.MotorZag);
            }
        }
        internal void RollColor()
        {
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Rmod = RandomAccess.Percent(Sheet.Setting.Rmod);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Gmod = RandomAccess.Percent(Sheet.Setting.Gmod);
            }
            if (RandomAccess.Percent(Sheet.Setting.SpreadReRoll))
            {
                Bmod = RandomAccess.Percent(Sheet.Setting.Bmod);
            }

            if (Rmod)
            {
                if (Sheet.Setting.R > 0)
                    R = RandomAccess.Next(0, Sheet.Setting.R);
                else
                    R = RandomAccess.Next(Sheet.Setting.R, 1);
            }
            if (Gmod)
            {
                if (Sheet.Setting.G > 0)
                    G = RandomAccess.Next(0, Sheet.Setting.G);
                else
                    G = RandomAccess.Next(Sheet.Setting.G, 1);
            }
            if (Bmod)
            {
                if (Sheet.Setting.B > 0)
                    B = RandomAccess.Next(0, Sheet.Setting.B);
                else
                    B = RandomAccess.Next(Sheet.Setting.B, 1);
            }
        }
        //public bool Any()
        //{
        //    if (Decay)
        //        return false;

        //    return HBump || VBump || 
        //}
    }
    public class RollSeries
    {
        public bool ReUseRolls;

        public RollSeries()
        {
            Rolls = new ListHead();

        }
        public void Add(ModRoll roll)
        {
            Rolls.Add(roll);
            //return roll.Any();
        }
        public void Reuse(bool reuse)
        {
            ReUseRolls = reuse;
            Item = null;
        }
        public ModRoll First()
        {
            Item = Rolls.First;
            Roll = (ModRoll)Item.Object;
            return Roll;
        }
        ListMember Item;
        ModRoll Roll;
        public ModRoll Next()
        {
            if (Item == null)//|| Item.Next == null)
                return First();

            if (Item.Next == null)
            {
                return null;
            }
            Item = Item.Next;
            Roll = (ModRoll)Item.Object;


            return Roll;
        }
        public ListHead Rolls;
    }
}
