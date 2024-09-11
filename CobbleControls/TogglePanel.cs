using System;
using System.Drawing;
using CobbleApp;

namespace CobbleControls
{
    public class TogglePanelButton : ClickButton
    {
        Action OnShow;
        public ContainerZone Panel;

        new MenuBar Parent => (MenuBar)base.Parent;

        public TogglePanelButton(ContainerZone panel, string prompt, Action onShow, int x, int y, int w, int h, MenuBar parent, DrawSurface surface = null) : this(panel, prompt, onShow, new Rectangle(x, y, w, h), parent, surface)
        {
        }
        public TogglePanelButton(ContainerZone panel, string prompt, Action onShow, Rectangle rect, MenuBar parent, DrawSurface surface = null) : base(prompt, null, rect, parent, surface)
        {
            Panel = panel;
            OnShow = onShow;
            SetAction(Clicked);
            panel.ParentRemove();

        }
        public void Clicked()
        {
            Parent.PanelSelect(Panel);
            Panel.ParentAdd();
            Panel.ReSub();
            //            AppRoot.Instance.BackgroundImage.Save("C://SlabState//Back.bmp");
            Surface.DrawImage(AppRoot.Instance.BackgroundImage, Panel.Rectangle, Panel.Rectangle);// new Rectangle(0, Panel.Y, XoBase.Instance.SplashImage.Width, Panel.H));
            Panel.Draw();
            OnShow?.Invoke();
        }

    }


    public class TogglePanel : ContainerZone
    {
        public ToggleButton Panel1Button;
        public ToggleButton Panel2Button;
        public ContainerZone Panel1;
        public ContainerZone Panel2;

        Action Action;

        public TogglePanel(string prompt1, string prompt2, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, Action action = null) : base(rect, parent, surface)
        {
            if (rect.Width < 40 || rect.Height < ClickButton.DefaultHeight * 2)
            {
                Status.Log("TogglePanel has min size W40");
                return;
            }

            Action = action;
            int w = 0;// Math.Max(70, (int)(W * .2));
            if (rect.Width < 70)
            {
                w = rect.Width;
            }
            else if (prompt1.Length < 10 && prompt2.Length < 10)
            {
                w = 70;
            }
            else
                w = Math.Max(70, (int)(Width * .2));

            var r = new Rectangle(X, Y, w, (Height / 2) - 1);

            Panel1Button = new ToggleButton(prompt1, Go1, r, this);
            Panel1Button.On = true;

            r = new Rectangle(r.X, r.Bottom + 2, r.Width, r.Height);

            Panel2Button = new ToggleButton(prompt2, Go2, r, this);

            r = new Rectangle(Panel1Button.Rectangle.Right, Y, Rectangle.Right - Panel1Button.Rectangle.Right, Height);

            Panel1 = new ContainerZone(r, this);
            Panel2 = new ContainerZone(r, this);
            Panel2.ParentRemove();
        }
        public virtual void Go1()
        {
            Panel2Button.On = !Panel1Button.On;
            Go();
        }
        public virtual void Go2()
        {
            Panel1Button.On = !Panel2Button.On;
            Go();
        }
        public virtual void Go()
        {
            Panel1Button.Draw();
            Panel2Button.Draw();

            if (Panel1Button.On)
            {
                Panel2.ParentRemove();

                Panel1.ParentAdd();
                Panel1.ReSub();
                if (Panel1.ZoneCount == 0)
                {
                    Surface.FillRect(AppSingleton.DefaultBackColor, Panel1.Rectangle);
                }
                else
                    Panel1.Draw();
            }
            else
            {
                Panel1.ParentRemove();
                Panel2.ParentAdd();
                Panel2.ReSub();
                if (Panel2.ZoneCount == 0)
                {
                    Surface.FillRect(AppSingleton.DefaultBackColor, Panel2.Rectangle);
                }
                else
                    Panel2.Draw();
            }
            Action?.Invoke();
        }

        //public virtual void Activate1()
        //{ }
        //public virtual void Activate2()
        //{ }
    }
}
