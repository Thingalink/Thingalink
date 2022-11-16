using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    public class ZoneList : ListHead
    {
        public delegate void ZoneMethod(Zone value);

        //public void Iterate(ZoneMethod action, ListMember first = null)
        //{
        //    var selection = first ?? First;
        //    while (selection != null)
        //    {
        //        action.Invoke(CastItem(selection));

        //        selection = selection.Next;
        //    }
        //}
        public Zone CastItem(ListMember item)
        {
            return (Zone)item.Object;
        }
        public override ListMember Add(object value, ListMember insertAfter = null)
        {
            return base.Add(value, insertAfter);
        }
        //protected MouseEvent ThisClick;
        public void Click(MouseEventArgs point)
        {
            //ThisClick = point;
            First?.Ifterminate(Hitit, IfHit, point);
        }
        public bool IfHit(ListMember item, object point)
        {
            return CastItem(item).IfHit((MouseEventArgs)point);
        }
        public void Hitit(ListMember item, object point)
        {
            //if (item.Object is ContainerZone)
            //    CastItem(item).Click((MouseEventArgs)point);
            //else//because debug
            CastItem(item).Click((MouseEventArgs)point);
        }

        public bool KeepMove(MouseEventArgs point)
        {
            var item = First;
            while (item != null)
            {
                if (IfMoveHit(item, point))
                    break;
                item = item.Next;
            }
            if (item == null)
                return false;
            return ((Zone)(item?.Object))?.KeepMove(point) ?? false;
        }

        public void Move(MouseEventArgs point)
        {
            First?.Ifterminate(Moveit, IfMoveHit, point);
        }
        public bool IfMoveHit(ListMember item, object point)
        {
            return CastItem(item).IfHit((MouseEventArgs)point);
        }
        public void Moveit(ListMember item, object point)
        {
            CastItem(item).Move((MouseEventArgs)point);
        }

        //public void Key(Keys key)
        //{
        //    First?.Ifterminate(Keyit, IfHit, key);
        //}
        //public void Keyit(ListMember item, object key)
        //{
        //    item.Contains.Zone.Key((Keys)key);
        //}

        public void Draw()
        {
            Iterate(Draw);
        }
        public void Draw(ListMember item)
        {
            CastItem(item).Draw();
        }
    }
}
