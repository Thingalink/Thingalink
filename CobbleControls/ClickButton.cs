using System;
using System.Drawing;
using System.Windows.Forms;
using CobbleApp;
using Thingalink;

namespace CobbleControls
{
    public class ClickButton : Zone
    {
        public static int DefaultHeight = 24;

        protected MeasuredText Text;
        protected Paint Paint;
        protected Action Action;

        protected bool block;

        public ClickButton(string prompt, Action action, int x, int y, int w, int h, ContainerZone parent = null, DrawSurface surface = null) : this(prompt, action, new Rectangle(x, y, w, h), parent, surface)
        {
        }
        public ClickButton(string prompt, Action action, Rectangle rect, ContainerZone parent = null, DrawSurface surface = null) : base(rect, parent, surface)
        {
            Action = action;
            Text = InitFont(prompt);
            Paint = InitPaint();
        }
        public void SetAction(Action action)
        {
            Action = action;
        }
        public virtual MeasuredText InitFont(string text)
        {
            var t = GetFont();
            t.Text = text;
            return t;
        }
        protected virtual MeasuredText GetFont()
        {
            var t = new MeasuredText();
            t.Font = AppSingleton.FontsList.SelectedFont;
            t.Point = new Point(Rectangle.X, Rectangle.Y);
            return t;
        }
        public virtual Paint InitPaint()
        {
            return GetPaint();
        }
        protected virtual Paint GetPaint()
        {
            return AppSingleton.DefaultBackColor;
        }

        public override void Draw()
        {
            //var t = Text ?? GetFont();
            //var p = Paint ?? GetPaint();

            Surface.FillRect(Paint, Rectangle);
            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, Text.Text, Text.Point);
            Surface.Outline(AppSingleton.DefaultTextColor, Rectangle);
        }

        public override void Click(MouseEventArgs point)
        {
            if (block)
                return;

            block = true;

            ClickDo();
            //AppRoot.Instance.QueAction(ClickDo);
        }
        public virtual void ClickDo()
        {
            Surface.FillRect(AppSingleton.DefaultBackColor, Rectangle);
            Surface.DrawText(Text.Font, AppSingleton.DefaultTextColor, Text.Text, Text.Point);

            Action?.Invoke();

            Draw();

            block = false;
        }

        public void Set(string v)
        {
            Text.Text = v;
            //ChugMule.Que(Draw);
            Draw();
        }
    }
}
