using System;
using System.Drawing;

namespace Thingalink
{
    public static class RandomAccess
    {
        public static Random Seed = new Random();

        public static Color RandomColor()
        {
            return Color.FromArgb(Seed.Next(255),
                                                  Seed.Next(255),
                                                  Seed.Next(255));
        }

        public static Pen RandomColorPen(int thick, bool addToPool = false)// = true)
        {
            Color randomColor = RandomColor();

            if (thick == -1)
                thick = Seed.Next(40);

            var pen = new Pen(randomColor, thick);

            //if (addToPool)
            //    instance.Pens.Add(pen);

            return pen;
        }

        public static int Next(int min, int max)
        {
            return Seed.Next(min, max);

        }

        public static ListMember Item(ListHead list)
        {
            return list.At(Next(0, list.Count));

        }

        public static bool Fifty50 => RandomAccess.Seed.Next(0, 2) < 1 ? true : false;
        public static bool Percent(int pct)
        {
            return Seed.Next(0, 100) < pct;

        }
    }
}
