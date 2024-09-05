using System;
using System.Windows.Forms;

namespace CobbleApp
{
    public class SysEventHandler
    {
        public virtual void Sub()
        {

        }
    }
    public class FormRefreshHandler : SysEventHandler
    {
        public override void Sub()
        {
            AppRoot.Form.Move += Form_Refresh;
            AppRoot.Form.ResizeEnd += Form_ResizeEnd;
        }
        protected virtual void Form_Refresh(object sender, EventArgs e)
        {
            ContainerHost.Draw();
        }
        private void Form_ResizeEnd(object sender, EventArgs e)
        {
            ContainerHost.Resize();
        }

    }
}
