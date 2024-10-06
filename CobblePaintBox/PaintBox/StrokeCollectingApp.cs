using CobbleApp;
using Thingalink;
using System.IO;
using System.Windows.Forms;
using CobbleStorageJSON;

namespace CobblePaintBox
{

    /// <summary>
    /// AppRoot considers the click standard equipment. not all apps need track movement
    /// the paint app wants the full path while button held and the setting state as well
    /// </summary>
    public class StrokeCollectingApp : AppRoot
    {
        //bool Stroke;
        //bool StrokeDelay;
        //int StrokeID;
        //public Rectangular StrokeZone;
        //ListHead StrokeList;

        public string Path;
        public string PathStroke;
        public int ConfigFile;
        public bool ConfigChange = true;
        public MouseStrokeCollector MouseStroke;
        public DrawConfigSetting Settings;

        public StrokeCollectingApp(Form form) : base(form)
        {
            Storage.Use(new StorageJSON());
        }

        public void SetStrokePath(string path)
        {
            Path = path;
        }
        protected override void InitMouseHandler()
        {
            MouseStroke = new MouseStrokeCollector(ProcessStroke);
            SysEventHandlers.Add(MouseStroke);
        }

        void ProcessStroke(ListHead strokeList, int strokeID)
        {
            if (ConfigChange)
            {
                var setting = new DrawConfigSetting();
                setting.Save();

                PathStroke = Path + "\\" + ConfigFile.ToString("D4") + "_" + DrawConfig.Drill.Value.ToString("D3");
                Directory.CreateDirectory(PathStroke);

                Storage.SaveFile<DrawConfigSetting>(PathStroke + "\\DrawConfig.txt", setting);
                //keep a current 
                Storage.SaveFile<DrawConfigSetting>(Path + "\\DrawConfig.txt", setting);

                ConfigFile++;
                ConfigChange = false;
            }

            var stroke = new BrushStroke(ConfigFile, strokeList);
            Storage.SaveFile<BrushStroke>(PathStroke + "\\Stroke" + strokeID.ToString("D4") + ".txt", stroke);


            MouseEventList.AppThreadList.AppendEvents(strokeList);
        }
        
        public DrawConfigSetting Load(int fileId, int depth = 0)
        {
            PathStroke = Path + fileId.ToString("D4") + "_" + depth.ToString("D3");

            return Storage.OpenFile<DrawConfigSetting>(PathStroke + "\\DrawConfig.txt");
        }
        public void FetchNewest()
        {
            Settings = Storage.OpenFile<DrawConfigSetting>(Path + "\\DrawConfig.txt");
        }
        public DrawConfigSetting FetchDrawConfig(string path)
        {
            return Storage.OpenFile<DrawConfigSetting>(path + "\\DrawConfig.txt");
        }
        public BrushStroke FetchStroke(string path)
        {
            return Storage.OpenFile<BrushStroke>(path);
        }

        protected virtual void ConfiChange()
        {
            if (!ConfigChange)
            {
                ConfigChange = true;
            }
        }
    }
}
