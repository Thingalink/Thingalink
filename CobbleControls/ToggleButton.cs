using System;
using System.Drawing;
using CobbleApp;
using Thingalink;

namespace CobbleControls
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
                Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, Text.Text, Text.Point);
                Surface.Outline(AppSingleton.DefaultTextColor, Rectangle);
            }
            else
            {
                Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, AltPrompt ?? Text.Text, Text.Point);
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
