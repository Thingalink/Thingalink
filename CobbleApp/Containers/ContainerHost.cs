using System.Windows.Forms;

namespace CobbleApp
{
    public static class ContainerHost
    {
        public static ContainerZone Zone;

        private static ZoneList UpdateZones;

        public static void SetHost(ContainerZone hostZone)
        {
            Zone = hostZone;
            UpdateZones = new ZoneList();
        }

        public static void Click(MouseEventArgs point)
        {
            Zone.Click(point);
        }

        public static void Move(MouseEventArgs point)
        {
            Zone.Move(point);
        }
        public static bool KeepMove(MouseEventArgs point)
        {
            return Zone.KeepMove(point);
        }
        public static void Key(Keys key)
        {
            Zone.Key(key);
        }

        public static void Draw()
        {
            Zone.Draw();
        }

        public static void Resize()
        {
            Zone.Draw();
        }
        public static void QueUpdate(Zone zone)
        {
            UpdateZones.Add(zone);
        }
        public static bool DrawUpdates()
        {
            if (UpdateZones.Count == 0)
                return false;
            var zones = UpdateZones;
            UpdateZones = new ZoneList();
            zones.Draw();
            return true;
        }
    }
}
