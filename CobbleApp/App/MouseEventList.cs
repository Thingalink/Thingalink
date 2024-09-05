using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    public class MouseEventList
    {
        protected ListHead EventList;
        private ListHead ChugList;

        public static MouseEventList AppThreadList;

        public MouseEventList()
        {
            EventList = new ListHead();
        }

        public bool HasEvent => EventList.Count > 0;
        public bool Overload => EventList.Count > 3500;

        private bool busy;
        public bool Busy => busy;

        public bool CheckOverload()
        {
            if (!Busy && MouseEventList.AppThreadList.Overload)
            {
                busy = true;

                Status.Log("UI Overloaded");
            }
            return Busy;
        }
        public void ClearOverload()
        {
            if (Busy)
            {
                busy = false;
                Status.Log("UI Unlock");
            }
        }

        public void Lock()
        {
            busy = true;
        }
        public void Unlock()
        {
            busy = false;
        }

        public virtual bool BlockMouse(MouseEventArgs e)
        {
            if (Busy)
                return true;

            return false;
        }

        public ReadyMule StartTask()
        {
            ChugList = EventList;
            EventList = new ListHead();
            return new ReadyMule(ChugEvents);
        }

        public void AddEvent(MouseEventArgs value)
        {
            EventList.Add(value);
        }
        public void AppendEvents(ListHead list)
        {
            EventList.Append(list);
        }
        protected virtual void ChugEvents()
        {
            ChugList.IterateUIBreather(ChugEvent, Application.DoEvents);
        }

        protected virtual void ChugEvent(ListMember item)
        {
            ChugEvent((MouseEventArgs)item.Object);
        }

        protected virtual void ChugEvent(MouseEventArgs mouse)
        {
            if (mouse.Clicks > 0)
                ContainerHost.Click(mouse);
            else
                ContainerHost.Move(mouse);
        }
    }
}
