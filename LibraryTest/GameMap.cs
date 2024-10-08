using CobbleApp;
using Thingalink;

using RandomAccess = Thingalink.RandomAccess;

namespace LibraryTest;

public class GameClass
{
    public AppRoot App => AppRoot.Instance;

    public DrawScreen Screen => AppRoot.Screen;
   // public TextTheme Font => AppRoot.ToolText;

    public static GameMap Map = new GameMap();
}

public class Region : GameClass
{
    public ListHead Neighbors = new ListHead();
    public Zone Focal;
    public Paint Paint;

    public Region(int divisorLow, int divisorHight)
    {
        Paint = new Paint(RandomAccess.RandomColor());
        int w = RandomAccess.Next(0, Screen.Width / divisorHight);
        int h = RandomAccess.Next(Screen.Height / divisorLow, Screen.Height / divisorHight);
        Focal = new Zone(RandomAccess.Next(w / 2, Screen.Width / divisorHight),
            RandomAccess.Next(Screen.Height / divisorLow, Screen.Height / divisorHight), w, h);
        
        //else border from
    }
}

public class GameMap : GameClass
{
    public Region Region;

    public ListHead Regions;

    public GameMap()
    {
        GameClass.Map = this;
        Regions = new ListHead();
        Region = new Region(7, 5);
    }

    public void SetRegion(Region region)
    {
        //if(.Map == this)
        {
            Region = region;
        }
    }
}