using System;
using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    public class ContainerZone : Zone
    {
        public ZoneList Zones;
        ZoneList Moveable;
        ZoneList Keyable;
        public string Name;

        //public bool MoveQueued => Zones.Moveable.LastCount > 0;//{ get; internal set; }

        //public virtual bool MoveConsuming => Zones.Moveable.LastCount > 0;

        public ContainerZone(int x, int y, int w, int h, ContainerZone parent = null, DrawSurface surface = null, string name = null) : this(new Rectangle(x, y, w, h), parent, surface, name)
        {

        }
        public ContainerZone(Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, string name = null) : base(rect, parent)
        {
            Zones = new ZoneList();
            Moveable = new ZoneList();
            Keyable = new ZoneList();
            Name = name;
        }

        long ticks;

        public int ZoneCount => Zones.Count;

        public override void Draw()
        {
            if (ticks < DateTime.Now.Ticks - 500)
            {
                ticks = DateTime.Now.Ticks;
                Zones.Draw();
            }
            //base.Draw();
        }

        public override void Click(MouseEventArgs point)
        {
            Zones.Click(point);
        }

        public override bool KeepMove(MouseEventArgs point)
        {
            return Moveable.KeepMove(point);
        }

        public override void Move(MouseEventArgs point)
        {
            Moveable.Move(point);
        }
        public ListMember Add(Zone zone)
        {
            var item = Zones.Add(zone);
            return item;
        }
        public ListMember AddMovable(Zone zone)
        {
            return Moveable.Add(zone);
        }


        public ListMember AddKeyable(Zone zone)
        {
            return Keyable.Add(zone);
        }
        internal void Remove(ListMember item, bool movable = false, bool keyable = false)
        {
            Zones.DeleteItem(item);
            if (movable)
                Moveable.DeleteObject(item.Object);
            if (keyable)
                Keyable.DeleteObject(item.Object);
        }
        internal void RemoveMoveable(ListMember item)
        {
            Moveable.DeleteItem(item);
        }
        internal void RemoveKeyable(ListMember item)
        {
            Keyable.DeleteItem(item);
        }
        bool loopResult;

        public override bool MoveUnSub()
        {
            loopResult = false;
            Zones.Iterate(MoveUnSub);
            return loopResult;
        }
        protected void MoveUnSub(ListMember item)
        {
            if (((Zone)item.Object).MoveUnSub())
                loopResult = true;
        }
        public override void ReSub()
        {
            Zones.Iterate(ReSub);
        }
        protected void ReSub(ListMember item)
        {
            ((Zone)item.Object).ReSub();
        }
        //public override void Key(Keys key)
        //{
        //    Keyable.Key(key);
        //}

        public void ClearZones()
        {
            //Zones.Clear();
            //Moveable.Clear();
            //Keyable.Clear();

            Zones = new ZoneList();
            Moveable = new ZoneList();
            Keyable = new ZoneList();
        }
        public void UnsubMoveables()
        {
            Zones.Iterate(UnsubMoveable);
        }
        protected void UnsubMoveable(ListMember item)
        {
            ((Zone)item.Object).MoveUnSub();
        }
        //public bool CheckMove(MouseMove eventling)
        //{
        //    var b = Zones.IfMove(eventling);
        //    if (b)
        //        return true;
        //    return b;
        //}

    }
}
