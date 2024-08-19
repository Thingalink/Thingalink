using CobbleApp;

namespace LibraryTest;

public class Splash : BitmapSplash
{
    //ToggleButton Sound;
    bool dosound = false;

    public Splash(Action action, bool sound = false) : base("C:\\SlabState\\Gut\\Gravel\\GradiasSplit10.bmp", DrawScreen.Instance.Rectangle, action)
    {
        dosound = sound;
        //Sound = new ToggleButton("Sound is " + (sound ? "On" :"Off"), Toggle, new Rectangle(20, 100, 100, 40));
        //Sound = new ToggleButton("Sound is On", Toggle, new Rectangle(20, 100, 140, 40), this, altprompt: "Sound is Off");
        //if (sound)
        //{
        //    Sound.On = sound;
        //}
    }
    public override void Draw()
    {
        //semi transparent splash still paint a back
        DrawScreen.Instance.FillBack(Color.Purple);
        base.Draw();


        PlayGreeting();
        //Sound.Draw();
    }

    //public override void Click(MouseEventArgs point)
    //{
    //    //if (Sound.IfHit(point))
    //    //{
    //    //    Sound.Click(point);
    //    //    return;
    //    //}
    //    base.Click(point);
    //}

    //void Toggle()
    //{
    //    UserConfig.AppConfig.Sound = Sound.On;
    //    Storage.SaveConfig("C:\\SlabState\\Gut\\Source.txt", UserConfig.AppConfig);

    //    //if (Sound.On)
    //    //{
    //    //    var simpleSound = new OSAudio("C:\\SlabState\\User\\spells and coin\\", "phase shift.wav");
    //    //    simpleSound.Play();
    //    //}
    //    //else
    //    //{
    //    //    var simpleSound = new OSAudio("C:\\SlabState\\User\\spells and coin\\", "teleport.wav");
    //    //    simpleSound.Play();
    //    //}
    //}
    private void PlayGreeting()
    {
        if (dosound)// Sound.On)
        {
            var simpleSound = new OSAudio("C:\\SlabState\\User\\spells and coin\\", "bless.wav");
            simpleSound.Play();
        }
    }
    private void PlayExit()
    {
        if (dosound)//Sound.On)
        {
            var simpleSound = new OSAudio("C:\\SlabState\\User\\spells and coin\\", "disintegration.wav");
            simpleSound.Play();
        }
    }
    public override void Escape()
    {
        PlayExit();
        base.Escape();
    }
}
