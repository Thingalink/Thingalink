using CobbleApp;
using CobbleControls;
using Thingalink;

namespace CobblePaintBox
{
    public class ScreenSelect
    {
        public static Paint Root;
        public static Paint RootElse;
    }

    public static class DrawConfig
    {
        public static ListHead ModCells = new ListHead();
        public static Paint SelectedPaint;
      //  public static ColorPicker OverwriteMin;
      //  public static ColorPicker OverwriteMax;
        public static ToggleButton Overwrite;
        public static ToggleButton Racer;
        public static ToggleButton Clude;
        public static ToggleButton Wrap;
        public static ToggleButton Morph;
        public static ToggleButton MorphAll;

        public static ToggleButton SnapMorphs;

        //public static ToggleButton More;
        //public static ToggleButton Time;
        //public static ToggleButton Start;
        //public static ToggleButton Frame;

        public static BrushedMatrix Matrix;


        public static Oscillator Oscillator;
        public static DragSelect Drill;

        public static DragSelect ClickBlock;

        public static DragSelect SplitH;
        public static DragSelect SplitV;

        public static DragSelect MotorH;
        public static DragSelect MotorV;
        public static DragSelect MotorZig;
        public static DragSelect MotorZag;
        public static DragSelect MotorLurch;
        public static DragSelect MotorLurchX;

        public static DragSelect HBump;
        public static DragSelect VBump;

        public static DragSelect BumpDouble;
        public static DragSelect Decay;

        //public static DragSelect Lap;
        public static DragSelect Sway;
        public static DragSelect Swing;


        public static DragSelect Rmod;
        public static DragSelect Gmod;
        public static DragSelect Bmod;


        public static ToggleButton Blend;
        public static ToggleButton Push;
        public static ToggleButton ModPush;

        public static DragSelect R;
        public static DragSelect G;
        public static DragSelect B;
        public static DragSelect RFlat;
        public static DragSelect GFlat;
        public static DragSelect BFlat;

        public static DragSelect CModReRoll;
        public static DragSelect SpreadReRoll;
        public static DragSelect Shmear;
        public static DragSelect DShmear;





        public static DragSelect Rswing;
        public static DragSelect Gswing;
        public static DragSelect Bswing;
        public static ToggleButton Solid;
        public static DragSelect Jump;
        public static DragSelect RRadial;
        public static DragSelect GRadial;
        public static DragSelect BRadial;
    }

    public class DrawConfigSetting
    {
        public bool Blend;
        public bool Solid;
        public int SplitH;
        public int MotorH;
        public int ClickBlock;
        public int Drill;
        public int SpreadReRoll;
        public int SplitV;
        public int Decay;
        public int HBump;
        public int VBump;
        public int BumpDouble;
        public int Sway;
        public int Swing;
        public int Shmear;
        public int DShmear;
        public int MotorV;
        public int MotorZig;
        public int MotorZag;
        public int MotorLurch;
        public int MotorLurchX;
        public bool Wrap;

        public int ColorA;
        public int ColorR;
        public int ColorG;
        public int ColorB;
        //public Bitmap ImageBrush;
        public bool Push;
        public bool ModPush;


        public bool OcilR;
        public int RUnder;
        public int ROver;
        public bool OcilG;
        public int GUnder;
        public int GOver;
        public bool OcilB;
        public int BUnder;
        public int BOver;

        public int Rmod;
        public int Gmod;
        public int Bmod;
        public int R;
        public int G;
        public int B;
        public int RFlat;
        public int GFlat;
        public int BFlat;
        public int Rswing;
        public int Gswing;
        public int Bswing;
        //public int RRadial;
        //public int GRadial;
        //public int BRadial;
        public int CModReRoll;

        public void Save()
        {
            Blend = DrawConfig.Blend.On;
            Solid = DrawConfig.Solid.On;
            SplitH = DrawConfig.SplitH.Value;
            MotorH = DrawConfig.MotorH.Value;
            ClickBlock = DrawConfig.ClickBlock.Value;
            Drill = DrawConfig.Drill.Value;
            SpreadReRoll = DrawConfig.SpreadReRoll.Value;
            SplitV = DrawConfig.SplitV.Value;
            Decay = DrawConfig.Decay.Value;
            HBump = DrawConfig.HBump.Value;
            VBump = DrawConfig.VBump.Value;
            BumpDouble = DrawConfig.BumpDouble.Value;
            Sway = DrawConfig.Sway.Value;
            Swing = DrawConfig.Swing.Value;
            Shmear = DrawConfig.Shmear.Value;
            DShmear = DrawConfig.DShmear.Value;
            MotorV = DrawConfig.MotorV.Value;
            MotorZig = DrawConfig.MotorZig.Value;
            MotorZag = DrawConfig.MotorZag.Value;
            MotorLurch = DrawConfig.MotorLurch.Value;
            MotorLurchX = DrawConfig.MotorLurchX.Value;
            Wrap = DrawConfig.Wrap.On;


            ColorA = DrawConfig.SelectedPaint.Color.A;
            ColorR = DrawConfig.SelectedPaint.Color.R;
            ColorG = DrawConfig.SelectedPaint.Color.G;
            ColorB = DrawConfig.SelectedPaint.Color.B;
            Push = DrawConfig.Push.On;
            ModPush = DrawConfig.ModPush.On;

            OcilR = DrawConfig.Oscillator.OcilROn;
            RUnder = DrawConfig.Oscillator.RUnder.Value;
            ROver = DrawConfig.Oscillator.ROver.Value;
            OcilG = DrawConfig.Oscillator.OcilGOn;
            GUnder = DrawConfig.Oscillator.GUnder.Value;
            GOver = DrawConfig.Oscillator.GUnder.Value;
            OcilB = DrawConfig.Oscillator.OcilBOn;
            BUnder = DrawConfig.Oscillator.BUnder.Value;
            BOver = DrawConfig.Oscillator.BUnder.Value;

            Rmod = DrawConfig.Rmod.Value;
            Gmod = DrawConfig.Gmod.Value;
            Bmod = DrawConfig.Bmod.Value;
            R = DrawConfig.R.Value;
            G = DrawConfig.G.Value;
            B = DrawConfig.B.Value;
            RFlat = DrawConfig.RFlat.Value;
            GFlat = DrawConfig.GFlat.Value;
            BFlat = DrawConfig.BFlat.Value;
            Rswing = DrawConfig.Rswing.Value;
            Gswing = DrawConfig.Gswing.Value;
            Bswing = DrawConfig.Bswing.Value;
            CModReRoll = DrawConfig.CModReRoll.Value;
        }

        public void Load()
        {
            DrawConfig.Blend.On = Blend;
            DrawConfig.Solid.On = Solid;
            DrawConfig.SplitH.Set(SplitH);
            DrawConfig.MotorH.Set(MotorH);
            DrawConfig.ClickBlock.Set(ClickBlock);
            DrawConfig.Drill.Set(Drill);
            DrawConfig.SpreadReRoll.Set(SpreadReRoll);
            DrawConfig.SplitV.Set(SplitV);
            DrawConfig.Decay.Set(Decay);
            DrawConfig.HBump.Set(HBump);
            DrawConfig.VBump.Set(VBump);
            DrawConfig.BumpDouble.Set(BumpDouble);
            DrawConfig.Sway.Set(Sway);
            DrawConfig.Swing.Set(Swing);
            DrawConfig.Shmear.Set(Shmear);
            DrawConfig.DShmear.Set(DShmear);
            DrawConfig.MotorV.Set(MotorV);
            DrawConfig.MotorZig.Set(MotorZig);
            DrawConfig.MotorZag.Set(MotorZag);
            DrawConfig.MotorLurch.Set(MotorLurch);
            DrawConfig.MotorLurchX.Set(MotorLurchX);
            DrawConfig.Wrap.On = Wrap;

            DrawConfig.SelectedPaint = new Paint(ColorA, ColorR, ColorG, ColorB);
            DrawConfig.Push.On = Push;
            DrawConfig.ModPush.On = ModPush;

            DrawConfig.Oscillator.OcilR.On = OcilR;
            DrawConfig.Oscillator.RUnder.Set(RUnder);
            DrawConfig.Oscillator.ROver.Set(ROver);
            DrawConfig.Oscillator.OcilG.On = OcilG;
            DrawConfig.Oscillator.GUnder.Set(GUnder);
            DrawConfig.Oscillator.GUnder.Set(GUnder);
            DrawConfig.Oscillator.OcilB.On = OcilB;
            DrawConfig.Oscillator.BUnder.Set(BUnder);
            DrawConfig.Oscillator.BUnder.Set(BUnder);

            DrawConfig.Rmod.Set(Rmod);
            DrawConfig.Gmod.Set(Gmod);
            DrawConfig.Bmod.Set(Bmod);
            DrawConfig.R.Set(R);
            DrawConfig.G.Set(G);
            DrawConfig.B.Set(B);
            DrawConfig.RFlat.Set(RFlat);
            DrawConfig.GFlat.Set(GFlat);
            DrawConfig.BFlat.Set(BFlat);
            DrawConfig.Rswing.Set(Rswing);
            DrawConfig.Gswing.Set(Gswing);
            DrawConfig.Bswing.Set(Bswing);
            DrawConfig.CModReRoll.Set(CModReRoll);

        }
    }
}
