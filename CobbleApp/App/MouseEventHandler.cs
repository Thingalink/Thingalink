using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    
    public class MouseEventHandler : SysEventHandler
    {
        
    }

    public class MouseClickEventHandler : MouseEventHandler
    {

        public override void Sub()
        {
            //AppRoot.Form.MouseDown += Form_MouseDown;
            //AppRoot.Form.MouseUp += Form_MouseUp;
            AppRoot.Form.MouseClick += Form_MouseClick;
        }
        //protected virtual void Form_MouseDown(object sender, MouseEventArgs e)
        //{
        //}

        //protected virtual void Form_MouseUp(object sender, MouseEventArgs e)
        //{
        //}

        protected virtual void Form_MouseClick(object sender, MouseEventArgs e)
        {
            ContainerHost.Click(e);
        }
        
    }
    
    public class MouseMoveEventHandler : MouseEventHandler
    {
        public override void Sub()
        {
            AppRoot.Form.MouseMove += Form_MouseMove;
        }

        protected virtual void Form_MouseMove(object sender, MouseEventArgs e)
        {
            var LastMoveEvent = e;
            if (MouseEventList.AppThreadList.BlockMouse(LastMoveEvent))
            {
                return;
            }

            //only keeping events over move tracking zones
            if (ContainerHost.KeepMove(LastMoveEvent))
            {
                CollectMouseMove(LastMoveEvent);
            }
        }        


        public virtual void CollectMouseMove(MouseEventArgs e)
        {
            MouseEventList.AppThreadList.AddEvent(e);
        }
    }
}
