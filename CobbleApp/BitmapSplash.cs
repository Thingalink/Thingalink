using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CobbleApp
{
    public class BitmapSplash : ContainerZone
    {
        public Bitmap Bitmap;
        public Action Action;

        public BitmapSplash(string path, Rectangle rect, Action action) : this(new Bitmap(path), rect, action)
        {
        }

        public BitmapSplash(Bitmap bitmap, Rectangle rect, Action action) : base(rect)
        {
            Bitmap = bitmap;
            Action = action;

            // AppHost.Instance.UIService.KeyService.EscapeAction(Escape);
        }

        //public override void ParentAdd()
        //{
        //    //tobe container host but too soon
        //    //Item = Parent.Add(this, false, true);
        //}
        //public override void ParentRemove()
        //{
        //    //Parent.Remove(Item, false, true);
        //}

        public virtual void Escape()
        {
            Parent.ClearZones();
            Action.Invoke();
        }

        public override void Key(Keys key)
        {
            //  AppHost.Instance.UIService.KeyService.DropEscape(Escape);
            Escape();
        }
        public override void Draw()
        {
            Surface.DrawImage(Bitmap, DrawScreen.Instance.Rectangle);

            Status.Instance.Back.LoadClip(Bitmap);
            Status.Log("Click or Any Key");
            //Status.Instance.Tick();
        }

        public override void Click(MouseEventArgs point)
        {
            //Parent.
            // AppHost.Instance.UIService.KeyService.DropEscape(Escape);
            Escape();
        }

    }
}
