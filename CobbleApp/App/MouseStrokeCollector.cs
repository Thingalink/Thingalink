using Thingalink;
using System.Windows.Forms;

namespace CobbleApp
{
    public class MouseStrokeCollector : MouseMoveEventHandler
    {
        bool Stroke;
        int StrokeID;
        bool StrokeDelay;
        public Rectangular StrokeZone;
        ListHead StrokeList;

        public delegate void ProcessStroke(ListHead strokeList, int strokeID);
        private ProcessStroke Action;

        public MouseStrokeCollector(ProcessStroke processStroke)
        {
            Action = processStroke;
        }

        public override void Sub()
        {
            AppRoot.Form.MouseClick += Form_MouseClick;
            AppRoot.Form.MouseDown += Form_MouseDown;
            AppRoot.Form.MouseUp += Form_MouseUp;
            AppRoot.Form.MouseMove += Form_MouseMove;
        }

        protected override void Form_MouseMove(object sender, MouseEventArgs e)
        {
            var LastMoveEvent = e;
            if (MouseEventList.AppThreadList.BlockMouse(LastMoveEvent) || StrokeDelay)
            {
                return;
            }

            CollectMouseMove(LastMoveEvent);
        }

        protected virtual void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (StrokeZone != null && StrokeZone.IfHit(e))
            {
                if (StrokeList != null)
                {
                    StrokeDelay = true;
                    Status.Log("Saving Stroke List");
                    return;
                }

                Stroke = true;
                StrokeList = new ListHead();
                StrokeList.Add(e);
            }
        }

        protected virtual void Form_MouseUp(object sender, MouseEventArgs e)
        {
            if (Stroke)
            {
                Stroke = false;

                Action.Invoke(StrokeList, StrokeID);

                StrokeID++;
                StrokeList = null;
                StrokeDelay = false;
            }
        }
        public override void CollectMouseMove(MouseEventArgs e)
        {
            if (Stroke)
            {
                if(ContainerHost.KeepMove(e))
                    StrokeList.Add(e);
            }
            else
                MouseEventList.AppThreadList.AddEvent(e);
        }

        protected virtual void Form_MouseClick(object sender, MouseEventArgs e)
        {
            if (!Stroke && !StrokeDelay)
                MouseEventList.AppThreadList.AddEvent(e);
        }

    }
}
