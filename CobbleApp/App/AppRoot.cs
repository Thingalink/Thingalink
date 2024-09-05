using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Thingalink;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

namespace CobbleApp
{

    public class AppRoot //: ContainerHost //: Form
    {
        public static AppRoot Instance;

        public static Form Form;
        public static DrawScreen Screen;

        public static TextTheme ToolText;

        public Bitmap BackgroundImage;


        private TaskMule TaskMule;
        protected WaitMule Loader;
        public static ChugMule ChugMule;
        public Action ChugOnce;

        protected ListHead SysEventHandlers;

        //protected ListHead EventList;
        //private ListHead ChugList;

        //ServiceList KeyEvents;


        public AppRoot(Form form)
        {
            Form = form;
            Instance = this;

            FormPreload();

            Form.Load += Loaded;
        }

        protected virtual void FormPreload()
        {
            Form.WindowState = FormWindowState.Maximized;
        }
        private void Loaded(object sender, EventArgs e)
        {
            Screen = new DrawScreen(Form);

            Form.FormClosing += Form_FormClosing;

            MouseEventList.AppThreadList = new MouseEventList();
            InitSysHandlers();


            ContainerHost.SetHost(InitContainer());

            ContainerHost.Draw();
            //DrawControls();
            InitAppLoader();
        }

        protected virtual void InitSysHandlers()
        {
            SysEventHandlers = new ListHead();            
            SysEventHandlers.Add(new FormRefreshHandler());
            SysEventHandlers.Add(new MouseClickEventHandler());
        }

        public virtual void Sub()
        {
            SysEventHandlers.Iterate(HandlerSub);
        }
        private void HandlerSub(ListMember item)
        {
            ((SysEventHandler)item.Object).Sub();
        }

        public void OldSub()
        {
            //Form.Move += Form_Refresh;
            //Form.ResizeEnd += Form_ResizeEnd;
            //Form.MouseDown += Form_MouseDown;
            //Form.MouseUp += Form_MouseUp;
            //Form.MouseClick += Form_MouseClick;
            //Form.MouseMove += Form_MouseMove;

            //KeyEvents = new ServiceList();
            //Form.KeyPress += Form_KeyPress;
            Form.KeyUp += Form_KeyUp;
        }

        protected virtual void InitFonts()
        {
            ToolText = new TextTheme(Screen.SizeFont("TestTextHowLongandsomemore", "Arial", 300, 30), Color.Black, Color.GhostWhite);
            new Status(new Rectangle(Screen.Rectangle.X, Screen.Rectangle.Y, 250, Screen.Rectangle.Height), ToolText, 30, 100, 500, true);
        }

        private ContainerZone InitContainer()
        {
            InitFonts();

            var splash = InitSplash();
            if (splash != null)
                return splash;

            return InitAppSpace();
        }
        protected virtual BitmapSplash InitSplash()
        {
            return null;
        }
        protected virtual void SplashReturn()
        {
            ContainerHost.SetHost(InitAppSpace());
            ShowControls();
            //ContainerHost.Draw();
        }

        protected virtual ContainerZone InitAppSpace()
        {
           // var surf = new BitmapSurface(Screen.Rectangle);
           // surf.FillRect(new Paint(Color.LimeGreen), surf.Rectangle);
           // BackgroundImage = surf.Bitmap;

            //check clear of splash content
            Screen.FillBack(Color.LimeGreen);
            Status.Log("splash close.  default app doing nothing.");
            var r = Status.Instance.Rectangle;
            return new ContainerZone(r.Right, r.Y, Screen.Rectangle.Width - r.Width, r.Height);
        }
        protected virtual void InitAppLoader()
        {
            Loader = new WaitMule(LoadControls);
            StartChugThread();
            Sub();
        }

        protected virtual void LoadControls()
        {
        }

        protected virtual void ShowControls()
        {
            if (!Loader.Done)
            {
                Status.Log("Loading...");
            }
            Loader.Ready(DrawControls);
        }

        protected virtual void DrawControls()
        {
            ContainerHost.Draw();
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CanClose())
            {
                Close();
            }
            else if (e != null)
                e.Cancel = true;
        }

        public virtual bool CanClose()
        {
            return true;
        }

        public virtual void Close()
        {
            ChugMule?.End();
            Application.Exit();
        }

        protected void StartChugThread()
        {
            ChugMule = new ChugMule(ChugThread);
            ChugMule.Start();
        }

        protected void ChugThread()
        {
            if (ChugOnce != null)
            {
                ChugOnce.Invoke();
                ChugOnce = null;
            }

            if (TaskMule == null)
            {
                if (MouseEventList.AppThreadList.HasEvent)
                {
                    TaskMule = MouseEventList.AppThreadList.StartTask();
                }
                else
                    ChugIdle();
            }
            else if (TaskMule.Done)
            {
                MouseEventList.AppThreadList.ClearOverload();

                ContainerHost.DrawUpdates();
                ChugIdle();
                TaskMule = null;
            }
            else 
                MouseEventList.AppThreadList.CheckOverload();

        }
        protected virtual void ChugIdle()
        {
            Status.Instance.Tick();
        }
        
        /// <summary>
        /// if not drawing direct to the screen
        /// </summary>
        protected virtual void SurfaceRefresh()
        {
            ContainerHost.DrawUpdates();
        }

        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (ContainerHost.Zone == null)
                {
                    Form_FormClosing(null, null);
                    return;
                }
                else if (EscapeConsume())
                    return;

                Form_FormClosing(null, null);
            }
        }
        public virtual bool EscapeConsume()
        {
            return false;
        }
    }
}
