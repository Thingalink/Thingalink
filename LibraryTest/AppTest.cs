using CobbleApp;
using Microsoft.VisualBasic.Devices;
using Thingalink;
using RandomAccess = Thingalink.RandomAccess;

namespace LibraryTest;

class AppTest : AppRoot
{ 
    //defaults do nothing but exit on escape key

    ListHead Bodies;
    GameMap Map => GameMap.Map;

    public AppTest(Form form) : base(form)
    {

        Bodies = new ListHead();

    }

    protected override void LoadControls()
    {
        GameMap.Map = new GameMap();
        Bodies.Add(Map);
        //ShowControls();
    }
    protected override BitmapSplash InitSplash()
    {
        return new Splash(SplashReturn);
    }


}
