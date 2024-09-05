using System;
using System.Drawing;
using Thingalink;

namespace CobbleApp
{
    public class MenuBar : ContainerZone
    {
        ListHead Buttons;

        ListMember ButtonItem;
        ClickButton Button => (ClickButton)ButtonItem?.Object;
        ContainerZone PanelHost;
        ContainerZone ActivePanel;

        int max;
        int width;
        public MenuBar(int width, Rectangle menuRectangle, ContainerZone panelHost, ContainerZone parent = null, DrawSurface surface = null) : base(menuRectangle, parent, surface)
        {
            Buttons = new ListHead();

            PanelHost = panelHost;
            this.width = width;
            max = menuRectangle.Width / width;
        }

        public override void Draw()
        {
            base.Draw();
            ActivePanel?.Draw();
        }
        public ContainerZone AddPanel(string buttonText)
        {
            if (Buttons.Count == max)
            {
                return null;
            }

            Rectangle r;
            if (Zones.Count == 0)
            {
                r = new Rectangle(X, Y, width - 2, Height);
            }
            else
            {
                r = Shaper.NextLeft((Rectangular)Zones.Last.Object, 2);
            }
            var btn = new TogglePanelButton(new ContainerZone(PanelHost.Rectangle, PanelHost), buttonText, null, r, this);
            ButtonItem = Buttons.Add(btn);

            btn.Panel.ParentRemove();

            //if (Zones.LastCount == 0)
            //{
            //    //                ActivePanel = btn.Panel;
            //    btn.Clicked();
            //}

            return btn.Panel;
        }
        public void PanelSelect(ContainerZone panel)
        {
            ActivePanel?.ParentRemove();
            ActivePanel = panel;
        }
        public void ReadySelect()
        {
            ((TogglePanelButton)Buttons.First.Object).ClickDo();
        }
    }
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
                    Surface.FillRect(AppRoot.ToolText.Backcolor, Panel1.Rectangle);
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
                    Surface.FillRect(AppRoot.ToolText.BackityBackcolor, Panel2.Rectangle);
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


    public class ToggleButton : ClickButton
    {
        public bool On;
        string AltPrompt;

        public ToggleButton(string prompt, Action action, int x, int y, int w, int h, ContainerZone parent = null, DrawSurface surface = null, string altprompt = null) : this(prompt, action, new Rectangle(x, y, w, h), parent, surface, altprompt)
        {
        }
        public ToggleButton(string prompt, Action action, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null, string altprompt = null) : base(prompt, action, rect, parent, surface)
        {
            AltPrompt = altprompt;
        }
        public override void Draw()
        {
            Surface.FillRect(Paint, Rectangle);
            if (On)
            {
                Surface.DrawText(Text.Font, Theme.Textcolor, Text.Text, Text.Point);
                Surface.Outline(Theme.Textcolor, Rectangle);
            }
            else
            {
                Surface.DrawText(Text.Font, Theme.Textcolor, AltPrompt ?? Text.Text, Text.Point);
            }
        }
        public virtual void Toggle()
        {
            On = !On;
        }

        public override void ClickDo()
        {
            Toggle();

            Draw();
            Action?.Invoke();

            block = false;
        }
    }
}
