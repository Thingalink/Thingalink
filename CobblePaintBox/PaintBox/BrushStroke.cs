using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobblePaintBox
{
    public class BrushStroke
    {
        public int Config;
        public int Drill;
        public List<Point> Points;

        public BrushStroke(int config, ListHead list)
        {
            Config = config;
            Drill = DrawConfig.Drill.Value;
            Points = new List<Point>();
            list.Iterate(Add);

        }

        protected void Add(ListMember item)
        {
            var mouse = (MouseEventArgs)item.Object;
            Points.Add(new Point(mouse.X, mouse.Y));
        }
    }
}
