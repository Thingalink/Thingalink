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

        public StrokeCollectingApp(Form form, string path ) : base(form)
        {
            Path = path;
            Storage.Use(new StorageJSON());
        }

        protected override void InitSysHandlers()
        {
            SysEventHandlers = new ListHead();
            SysEventHandlers.Add(new FormRefreshHandler());
            MouseStroke = new MouseStrokeCollector(ProcessStroke);
            SysEventHandlers.Add(MouseStroke);
        }

        void ProcessStroke(ListHead strokeList, int strokeID)
        {
            if (ConfigChange)
            {
                var setting = new DrawConfigSetting();
                setting.Save();

                PathStroke = Path + ConfigFile.ToString("D4") + "_" + DrawConfig.Drill.Value.ToString("D3");
                Directory.CreateDirectory(PathStroke);

                Storage.SaveConfig<DrawConfigSetting>(PathStroke + "\\DrawConfig.txt", setting);

                ConfigFile++;
                ConfigChange = false;
            }

            var stroke = new BrushStroke(ConfigFile, strokeList);
            Storage.SaveConfig<BrushStroke>(PathStroke + "\\Stroke" + strokeID.ToString("D4") + ".txt", stroke);


            MouseEventList.AppThreadList.AppendEvents(strokeList);
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
