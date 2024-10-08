﻿using CobbleApp;
using CobbleControls;
using System.Windows.Forms;
using CobblePaintBox;
using Thingalink;
using RandomAccess = Thingalink.RandomAccess;
using System.Drawing;
using System;
using System.IO;
using System.Collections.Generic;

namespace DrawAppTest
{
    public class UserDirectory
    {
        public string Path;

        public UserDirectory()
        {
            Path = Application.CommonAppDataPath;

        }
    }
    public class ProjectList
    {
        public static string SelectedProjectPath;

        public ListHead Projects;
        public ProjectList()
        { 
            Projects = new ListHead();
        }
         
    }
    class AppTest : StrokeCollectingApp
    {
        protected UserConfig UserConfig => AppSingleton.UserConfig;
        MenuBar MenuBar;
        protected ContainerZone Frames;
        protected ContainerZone Toggles;
        protected ContainerZone Tools;

        ContainerZone Brushes;
        ContainerZone Spread;
        ContainerZone Movement;
        ContainerZone ColorMods;
        ContainerZone Morphs;
        ColorPicker ColorPicker;

        TogglePanel TogglePanel;

        BrushSlotBar SlotBar;

        public AppTest(Form form) : base(form)//"C:\\SlabState\\Settings\\")
        {
        }
        protected override void InitUserConfig()
        {
            //installation folder does not need to be folder for output
            UserConfig config;

            var path = Directory.GetCurrentDirectory();
            string file = "\\Source.txt";
            if (!File.Exists(path + file))
            {
                string userfolder;

                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        userfolder = fbd.SelectedPath;
                    }
                    else
                    {
                        userfolder = path + "\\UserData";
                        if (!Directory.Exists(userfolder))
                        {
                            Directory.CreateDirectory(userfolder);
                        }
                    }


                }

                config = new UserConfig(userfolder, false);
                Storage.SaveConfig(path + file, config);
            }
            else
                config = Storage.OpenConfig(path + file);

            AppSingleton.LoadConfig(config);

            file = "\\Projects.txt";
            string selectedProject = "";
            if (File.Exists(AppSingleton.UserConfig.Path + file))
            {
                List<object> l = Storage.OpenFile<List<object>>(AppSingleton.UserConfig.Path + file);
                //todo leaving this for now as there is no project create or select it is placeholder
                if (l.Count > 0)
                    selectedProject = "\\" + l[0].ToString();
            }
            else
                selectedProject = "\\Default";

            path = AppSingleton.UserConfig.Path + "\\Projects";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ProjectList.SelectedProjectPath = path + selectedProject;

            SetStrokePath(ProjectList.SelectedProjectPath);

            if (!Directory.Exists(ProjectList.SelectedProjectPath))
            {
                InitProject(ProjectList.SelectedProjectPath);
            }
            else
            {
                LoadProject();
            }
        }
        protected void InitProject(string name)
        {
            Directory.CreateDirectory(name);
            Settings = new DrawConfigSetting();
            DrawConfig.SelectedPaint = new Paint(RandomAccess.RandomColor());
        }

        protected void LoadProject()
        {
            FetchNewest();
            DrawConfig.SelectedPaint = new Paint(Settings.ColorA, Settings.ColorR, Settings.ColorG, Settings.ColorB);

        }
        protected void ProjectRedraw()
        {
            string lastfolder = null;
            string lastfile = null;

            foreach (var folder in Directory.EnumerateDirectories(ProjectList.SelectedProjectPath))
            {
                var settings = FetchDrawConfig(folder);
                settings?.Load();

                foreach (var file in Directory.EnumerateFiles(folder))
                {
                    if(file.EndsWith("DrawConfig.txt"))
                    {
                        continue;
                    }

                    var stroke = FetchStroke(file);
                    foreach(var point in stroke.Points)
                    {
                        DrawConfig.Matrix.Click(point);
                    }

                    lastfile = file;
                }

                lastfolder = folder;
            }

            if (lastfile != null)
            {
                MouseStroke.SetID(int.Parse(lastfile.Substring(lastfile.Length - 8, 4)));
                ConfigFile = int.Parse(lastfolder.Substring(lastfolder.Length - 8, 4));
            }

        }

        //protected override BitmapSplash InitSplash()
        //{
        //    return new Splash(SplashReturn);//, true);
        //}
        protected override BitmapSplash InitSplash()
        {
            var splash = new Splash(SplashReturn, UserConfig.Sound);
            BackgroundImage = splash.Bitmap;
            return splash;
        }

        protected override void LoadControls()
        {
            Status.Log("Test Run");
            //Status.Update();

            var r = Shaper.RightOf(Status.Instance.Rectangle);
            Frames = new ContainerZone(new Rectangle(r.X, r.Y, Screen.Rectangle.Width - Status.Instance.Rectangle.Width, r.Height), ContainerHost.Zone, name: "Frames");
            Toggles = new ContainerZone(Shaper.NewRegular(Frames.X, Frames.Y + ClickButton.DefaultHeight + 2, Frames.Width, (Frames.Height / 6) - ClickButton.DefaultHeight - 2), Frames, name: "Toggles");
            MenuBar = new MenuBar(120, Shaper.NewRegular(Frames.X, Frames.Y, Frames.Width, ClickButton.DefaultHeight), Toggles, Frames);

            Tools = new ContainerZone(new Rectangle(Toggles.X, Toggles.Rectangle.Bottom + 2, Toggles.Width, 160), Frames, name: "Tools");


            InitToggles();

            Settings.Load();
            ProjectRedraw();

        }
        protected void InitToggles()
        {
            Brushes = MenuBar.AddPanel("Brushes");
            Spread = MenuBar.AddPanel("Spread");
            Movement = MenuBar.AddPanel("Movement");
            ColorMods = MenuBar.AddPanel("Color Mod");
            Morphs = MenuBar.AddPanel("Morphs");

            var r = new Rectangle(Toggles.X + 2, Toggles.Y + 2, 50, Toggles.Height - 4);

            DrawConfig.Blend = new ToggleButton("Blend", ConfiChange, r, Brushes, null, "Paint");


            // DrawConfig.SplitSource = new IntValue();
            // DrawConfig.SplitSource.Set(1);
            DrawConfig.SplitH = new DragSelect(AppSingleton.DefaultBackColor, "SplitH", 2, ConfiChange, r, Spread);//, null, DrawConfig.SplitSource);
            DrawConfig.SplitH.Range = 50;
            DrawConfig.SplitH.RangeLow = 0;

            DrawConfig.MotorH = new DragSelect(AppSingleton.DefaultBackColor, "MotorH", 2, ConfiChange, r, Movement);
            DrawConfig.MotorH.Range = 30;
            DrawConfig.MotorH.RangeLow = -30;

            r = Shaper.NewRegular(r.X, r.Y, 100, 30);
            DrawConfig.Morph = new ToggleButton("Morph", ToggleMorph, r, Morphs);


            r = Shaper.NextLeft(DrawConfig.Blend, 2);
            //r = new Rectangle(DrawConfig.Blend.Rectangle.Right + 2, Toggles.Y + 2, 60, (Toggles.H / 2) - 4);
            DrawConfig.ClickBlock = new DragSelect(AppSingleton.DefaultBackColor, "Block", 3, ConfiChange, r, Brushes);//, mod: DrawZone.PeatMod);
            DrawConfig.ClickBlock.Range = 200;
            DrawConfig.ClickBlock.Set(0);

            r = Shaper.NextLeft(DrawConfig.ClickBlock, 2);
            DrawConfig.Drill = new DragSelect(AppSingleton.DefaultBackColor, "Drill", 2, ConfiChange, r, Brushes);//, null, DrawConfig.DeepSource);
            DrawConfig.Drill.Range = 400;
            DrawConfig.Drill.RangeLow = -400;

            r = Shaper.NextLeft(DrawConfig.Drill, 2);
            DrawConfig.SpreadReRoll = new DragSelect(AppSingleton.DefaultBackColor, "ReRoll", 2, ConfiChange, r, Brushes);
            DrawConfig.SpreadReRoll.Range = 100;
            DrawConfig.SpreadReRoll.RangeLow = 0;

            //

            r = Shaper.NextLeft(DrawConfig.SplitH, 2);
            DrawConfig.SplitV = new DragSelect(AppSingleton.DefaultBackColor, "SplitV", 2, ConfiChange, r, Spread);
            DrawConfig.SplitV.Range = 50;
            DrawConfig.SplitV.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.SplitV, 2);
            DrawConfig.Decay = new DragSelect(AppSingleton.DefaultBackColor, "Decay", 2, ConfiChange, r, Spread);
            DrawConfig.Decay.Range = 90;
            DrawConfig.Decay.RangeLow = 0;
            DrawConfig.Decay.Set(0);

            r = Shaper.NextLeft(DrawConfig.Decay, 2);
            DrawConfig.HBump = new DragSelect(AppSingleton.DefaultBackColor, "HBump", 2, ConfiChange, r, Spread);
            DrawConfig.HBump.Range = 100;
            DrawConfig.HBump.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.HBump, 2);
            DrawConfig.VBump = new DragSelect(AppSingleton.DefaultBackColor, "VBump", 2, ConfiChange, r, Spread);
            DrawConfig.VBump.Range = 100;
            DrawConfig.VBump.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.VBump, 2);
            DrawConfig.BumpDouble = new DragSelect(AppSingleton.DefaultBackColor, "BumpX%", 2, ConfiChange, r, Spread);
            DrawConfig.BumpDouble.Range = 100;
            DrawConfig.BumpDouble.RangeLow = 0;


            r = Shaper.NextLeft(DrawConfig.BumpDouble, 2);

            DrawConfig.Sway = new DragSelect(AppSingleton.DefaultBackColor, "Sway", 2, ConfiChange, r, Spread);
            DrawConfig.Sway.Range = 100;
            DrawConfig.Sway.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.Sway, 2);

            //r = Shaper.NextLeft(DrawConfig.Swing, 2);
            //DrawConfig.Drill = new DragSelect(FineText.Backcolor, "Drill", 2, GoSwing, r, Toggles);//, null, DrawConfig.DeepSource);
            //DrawConfig.Drill.Range = 100;
            //DrawConfig.Drill.RangeLow = 0;
            DrawConfig.Swing = new DragSelect(AppSingleton.DefaultBackColor, "Swing", 2, ConfiChange, r, Spread);
            DrawConfig.Swing.Range = 100;
            DrawConfig.Swing.RangeLow = 0;



            r = Shaper.NextLeft(DrawConfig.Swing, 2);
            DrawConfig.Shmear = new DragSelect(AppSingleton.DefaultBackColor, "Shmear", 2, ConfiChange, r, Spread);
            DrawConfig.Shmear.Range = 100;
            DrawConfig.Shmear.RangeLow = 0;


            r = Shaper.NextLeft(DrawConfig.Shmear, 2);
            DrawConfig.DShmear = new DragSelect(AppSingleton.DefaultBackColor, "DShmear", 2, ConfiChange, r, Spread);
            DrawConfig.DShmear.Range = 100;
            DrawConfig.DShmear.RangeLow = 0;

            // r = Shaper.NextLeft(DrawConfig.Shmear, 2);


            r = Shaper.NextLeft(DrawConfig.MotorH, 2);
            DrawConfig.MotorV = new DragSelect(AppSingleton.DefaultBackColor, "MotorV", 2, ConfiChange, r, Movement);
            DrawConfig.MotorV.Range = 30;
            DrawConfig.MotorV.RangeLow = -30;

            r = Shaper.NextLeft(DrawConfig.MotorV, 2);
            DrawConfig.MotorZig = new DragSelect(AppSingleton.DefaultBackColor, "Zig%", 2, ConfiChange, r, Movement);
            DrawConfig.MotorZig.Range = 100;
            DrawConfig.MotorZig.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.MotorZig, 2);
            DrawConfig.MotorZag = new DragSelect(AppSingleton.DefaultBackColor, "Zag%", 2, ConfiChange, r, Movement);
            DrawConfig.MotorZag.Range = 100;
            DrawConfig.MotorZag.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.MotorZag, 2);
            DrawConfig.MotorLurch = new DragSelect(AppSingleton.DefaultBackColor, "Lurch", 2, ConfiChange, r, Movement);
            DrawConfig.MotorLurch.Range = 100;
            DrawConfig.MotorLurch.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.MotorLurch, 2);
            DrawConfig.MotorLurchX = new DragSelect(AppSingleton.DefaultBackColor, "LurchX", 2, ConfiChange, r, Movement);
            DrawConfig.MotorLurchX.Range = 7;
            DrawConfig.MotorLurchX.RangeLow = 0;

            r = Shaper.NextLeft(DrawConfig.MotorLurchX, 2);
            DrawConfig.Wrap = new ToggleButton("Wrap", ConfiChange, r, Movement);

            int xImageBrush = ((DragSelect)Brushes.Zones.Last.Object).Right + 5;
            r = new Rectangle(xImageBrush, Toggles.Y, Toggles.Width - xImageBrush - 5, Toggles.Height);
            TogglePanel = new TogglePanel("Solid", "Image", r, Brushes, action: ConfiChange);

            //var r = new Rectangle(Toggles.X + 2, Toggles.Y + 2, 50, (Toggles.H / 2) - 4);
            DrawConfig.Solid = TogglePanel.Panel1Button;// new ToggleButton("Solid", GoSolid, r, Toggles, null, "Image");

            InitToolbar();

            // var btn = new ClickButton("SaveSettings", SaveConfig, Shaper.NextUnder(DrawConfig.SpreadReRoll), Frames);
            // new ClickButton("LoadSettings", LoadConfig, Shaper.NextLeft(btn), Frames);

        }

        protected void InitToolbar()
        {
            //DrawConfig.SelectedPaint = new Paint(RandomAccess.RandomColor());
            ColorPicker = new ColorPicker(ref DrawConfig.SelectedPaint, new Rectangle(Tools.X + 2, Tools.Y + 2, 250, 120), Tools, action: ConfiChange);

            //ColorPicker.Draw();

            //var r = Shaper.NextLeft(ColorPicker, 1);
            //DrawConfig.OverwriteMin = new ColorPicker(Color.FromArgb(255, 30, 30, 30), r, Frames);

            //r = Shaper.NextLeft(DrawConfig.OverwriteMin, 1);
            //DrawConfig.OverwriteMax = new ColorPicker(Color.FromArgb(255, 255, 255, 255), r, Frames);

            var r = new Rectangle(Tools.X + 2, ColorPicker.Rectangle.Bottom + 2, 60, Tools.Height - ColorPicker.Height - 2);

            DrawConfig.Push = new ToggleButton("Push", ConfiChange, r, Tools);

            r = Shaper.NextLeft(DrawConfig.Push, 2);

            DrawConfig.ModPush = new ToggleButton("Mod", ConfiChange, r, Tools);

            r = Shaper.NextLeft(DrawConfig.ModPush, 2);

            DrawConfig.RFlat = new DragSelect(AppSingleton.DefaultBackColor, "Push R", 2, ConfiChange, r, Tools);
            DrawConfig.RFlat.Range = 5;
            DrawConfig.RFlat.RangeLow = -5;
            DrawConfig.RFlat.Set(0);

            r = Shaper.NextLeft(DrawConfig.RFlat, 2);
            DrawConfig.GFlat = new DragSelect(AppSingleton.DefaultBackColor, "Push G", 2, ConfiChange, r, Tools);
            DrawConfig.GFlat.Range = 5;
            DrawConfig.GFlat.RangeLow = -5;
            DrawConfig.GFlat.Set(0);

            r = Shaper.NextLeft(DrawConfig.GFlat, 2);
            DrawConfig.BFlat = new DragSelect(AppSingleton.DefaultBackColor, "Push B", 2, ConfiChange, r, Tools);
            DrawConfig.BFlat.Range = 5;
            DrawConfig.BFlat.RangeLow = -5;
            DrawConfig.BFlat.Set(0);


            r = Shaper.NextLeft(DrawConfig.BFlat, 2);

            DrawConfig.Overwrite = new ToggleButton("Overwrite", null, r, Tools);


            r = Shaper.NextLeft(DrawConfig.Overwrite, 2);
            DrawConfig.Clude = new ToggleButton("Clude", null, r, Tools);
            r = Shaper.NextLeft(DrawConfig.Clude, 2);
            DrawConfig.Racer = new ToggleButton("Racer", null, r, Tools);

            //var r = new Rectangle(ColorPicker.Rectangle.Right + 20, ColorPicker.Y + 10, Frames.Rectangle.Width - ColorPicker.Rectangle.Right - 20, 60 + (ClickButton.DefaultHeight * 3));
            //    // Brush = new DrawZone(r, this, Peat);

            SlotBar = new BrushSlotBar(TogglePanel.Panel2.Rectangle, TogglePanel.Panel2);

            SlotBar.Bitmap.MovePreSub();

            r = DrawConfig.Blend.Rectangle; // new Rectangle(DrawConfig.OverwriteMax.Rectangle.Right + 20, ColorPicker.Y + 10, 60, Tools.H - 4);

            DrawConfig.Rmod = new DragSelect(AppSingleton.DefaultBackColor, "Mod%", 3, ConfiChange, r, ColorMods);
            DrawConfig.Rmod.Range = 100;
            DrawConfig.Rmod.RangeLow = 0;
            DrawConfig.Rmod.Set(0);

            r = Shaper.NextLeft(DrawConfig.Rmod, 2);
            DrawConfig.R = new DragSelect(AppSingleton.DefaultBackColor, "Red", 3, ConfiChange, r, ColorMods);
            DrawConfig.R.Range = 100;
            DrawConfig.R.RangeLow = -100;
            DrawConfig.R.Set(0);

            r = Shaper.NextLeft(DrawConfig.R, 2);
            DrawConfig.Rswing = new DragSelect(AppSingleton.DefaultBackColor, "Swing", 3, ConfiChange, r, ColorMods);
            DrawConfig.Rswing.Range = 100;
            DrawConfig.Rswing.RangeLow = 0;
            DrawConfig.Rswing.Set(100);


            r = Shaper.NextLeft(DrawConfig.Rswing, 2);
            DrawConfig.Gmod = new DragSelect(AppSingleton.DefaultBackColor, "Mod%", 3, ConfiChange, r, ColorMods);
            DrawConfig.Gmod.Range = 100;
            DrawConfig.Gmod.RangeLow = 0;
            DrawConfig.Gmod.Set(0);

            r = Shaper.NextLeft(DrawConfig.Gmod, 2);
            DrawConfig.G = new DragSelect(AppSingleton.DefaultBackColor, "Green", 3, ConfiChange, r, ColorMods);
            DrawConfig.G.Range = 100;
            DrawConfig.G.RangeLow = -100;
            DrawConfig.G.Set(0);

            r = Shaper.NextLeft(DrawConfig.G, 2);
            DrawConfig.Gswing = new DragSelect(AppSingleton.DefaultBackColor, "Swing", 3, ConfiChange, r, ColorMods);
            DrawConfig.Gswing.Range = 100;
            DrawConfig.Gswing.RangeLow = 0;
            DrawConfig.Gswing.Set(100);

            r = Shaper.NextLeft(DrawConfig.Gswing, 2);
            DrawConfig.Bmod = new DragSelect(AppSingleton.DefaultBackColor, "Mod%", 3, ConfiChange, r, ColorMods);
            DrawConfig.Bmod.Range = 100;
            DrawConfig.Bmod.RangeLow = 0;
            DrawConfig.Bmod.Set(0);

            r = Shaper.NextLeft(DrawConfig.Bmod, 2);
            DrawConfig.B = new DragSelect(AppSingleton.DefaultBackColor, "Blue", 3, ConfiChange, r, ColorMods);
            DrawConfig.B.Range = 100;
            DrawConfig.B.RangeLow = -100;
            DrawConfig.B.Set(0);

            r = Shaper.NextLeft(DrawConfig.B, 2);
            DrawConfig.Bswing = new DragSelect(AppSingleton.DefaultBackColor, "Swing", 3, ConfiChange, r, ColorMods);
            DrawConfig.Bswing.Range = 100;
            DrawConfig.Bswing.RangeLow = 0;
            DrawConfig.Bswing.Set(100);

            r = Shaper.NextLeft(DrawConfig.Bswing, 2);
            DrawConfig.CModReRoll = new DragSelect(AppSingleton.DefaultBackColor, "ReRoll", 3, ConfiChange, r, ColorMods);
            DrawConfig.CModReRoll.Range = 100;
            DrawConfig.CModReRoll.RangeLow = 0;
            DrawConfig.CModReRoll.Set(0);

            DrawConfig.Oscillator = new Oscillator(Shaper.NewRegular(r.Right + 4, r.Y, 150, r.Height), ColorMods, ConfiChange);

            //r = Shaper.NextLeft(DrawConfig.Bswing, 2);
            //DrawConfig.RRadial = new DragSelect(FineText.Backcolor, "Radial", 3, GoRRadial, r, Tools);
            //DrawConfig.RRadial.Range = 100;
            //DrawConfig.RRadial.RangeLow = -100;
            //DrawConfig.RRadial.Set(0);

            //r = Shaper.NextLeft(DrawConfig.RRadial, 2);
            //DrawConfig.GRadial = new DragSelect(FineText.Backcolor, "Radial", 3, GoGRadial, r, Tools);
            //DrawConfig.GRadial.Range = 100;
            //DrawConfig.GRadial.RangeLow = -100;
            //DrawConfig.GRadial.Set(0);

            //r = Shaper.NextLeft(DrawConfig.GRadial, 2);
            //DrawConfig.BRadial = new DragSelect(FineText.Backcolor, "Radial", 3, GoBRadial, r, Tools);
            //DrawConfig.BRadial.Range = 100;
            //DrawConfig.BRadial.RangeLow = -100; 
            //DrawConfig.BRadial.Set(0);

            r = Shaper.NewRegular(DrawConfig.Racer.Right + 2, DrawConfig.Racer.Y, 100, DrawConfig.Racer.Height);
            var btn = new ClickButton("Snapshot", TakeSnap, r, Tools);


            InitMatrix();
        }

        protected override void DrawControls()
        {
            ContainerHost.SetHost(Frames);

            Tools.ReSub();
            //            ColorPicker.MoveSub();
            //          DrawConfig.RFlat.MoveSub();
            //        DrawConfig.GFlat.MoveSub();
            //      DrawConfig.BFlat.MoveSub();

            //DrawConfig.Matrix.Refresh();
            MouseStroke.StrokeZone = DrawConfig.Matrix;
            DrawConfig.Matrix.MoveSub();//.MovePreSub();
            SlotBar.SetTarget(DrawConfig.Matrix);

            Status.Instance.Clear();
            Status.Log("Welcome");

            ChugOnce = DrawInit;
        }
        protected void DrawInit()
        {
            MenuBar.ReadySelect();
            ContainerHost.Draw();
        }
        
        void TakeSnap()
        {
            DrawConfig.Matrix.Surface.Bitmap.Save(UserConfig.Path + "\\Snap" + DateTime.Now.Ticks + ".bmp");
        }
        void ToggleMorph()
        {
        }

        protected void InitMatrix()
        {
            DrawConfig.Matrix = new BrushedMatrix(null, ref SlotBar.Bitmap, Shaper.NewRegular(Tools.X, Tools.Bottom + 2, 960, 600), Frames, true);
            DrawConfig.Matrix.InitGuts();

            var r = Shaper.NextUnder(DrawConfig.Morph, 2);
            var btn = new ClickButton("Clear Recent", DrawConfig.Matrix.InitRecentCells, r, Morphs);

            r = Shaper.NextUnder(btn, 2);
            btn = new ClickButton("Clear Old", DrawConfig.Matrix.LopOldCells, r, Morphs);

            r = Shaper.NextUnder(btn, 2);
            DrawConfig.MorphAll = new ToggleButton("Morph Recent", null, r, Morphs, altprompt: "Morph All");

            r = Shaper.NextLeft(DrawConfig.MorphAll, 2);
            DrawConfig.SnapMorphs = new ToggleButton("Snap Per Morph", null, r, Morphs);
            Status.Log("Loader Ready");
        }

        protected void LoadConfig()
        {
            DrawConfigSetting settings;
            //using (StreamReader outputFile = new StreamReader("C:\\SlabState\\Settings\\DrawConfig.txt"))
            //{
            //    var s = outputFile.ReadToEnd();
            //    settings = JsonHelper.ToClass<DrawConfigSetting>(s);
            //}



            settings = Storage.OpenFile<DrawConfigSetting>("C:\\SlabState\\Settings\\DrawConfig.txt");

            settings.Load();
            MenuBar.Draw();
        }
       
        protected override void SurfaceRefresh()
        {
            if (Loader.Done && ContainerHost.Zone == Frames)
            {
                SlotBar.Bitmap.Draw();
                DrawConfig.Matrix.Draw();
            }
            //base.SurfaceRefresh();
        }
        protected override void ChugIdle()
        {
            if (DrawConfig.Morph != null && DrawConfig.Morph.On)
            {
                DrawConfig.Matrix.MorphAll();
                if (DrawConfig.SnapMorphs.On)
                    TakeSnap();
            }

            SurfaceRefresh();

            base.ChugIdle();
        }
    }

}