using System.Drawing;
using System.Windows.Forms;
using Thingalink;

namespace CobbleApp
{
    //extend per user or need
    public class UserConfig
    {
        public static UserConfig AppConfig;

        public string Path;
        public bool Sound;

        public UserConfig(string path, bool sound)
        {
            if (AppConfig == null)
                AppConfig = this;

            Path = path;
            Sound = sound;
        }
    }

    public abstract class StorageMethod
    {
        public abstract void SaveConfig(string file, UserConfig config);
        public abstract UserConfig OpenConfig(string file);

        public abstract void SaveFile<T>(string file, T classT);
        public abstract T OpenFile<T>(string file);
    }

    public class Storage
    {
        protected static StorageMethod StorageMethod;

        public static void Use(StorageMethod storageMethod)
        {
            StorageMethod = storageMethod;
        }

        public static void SaveConfig(string file, UserConfig config)
        {
            StorageMethod?.SaveConfig(file, config);
        }
        public static UserConfig OpenConfig(string file, StorageMethod storageMethod = null)
        {
            if (storageMethod != null)
                Use(storageMethod);

            return StorageMethod?.OpenConfig(file);
        }

        public static void SaveFile<T>(string file, T config)
        {
            StorageMethod?.SaveFile<T>(file, config);
        }
        public static T OpenFile<T>(string file, StorageMethod storageMethod = null)
        {
            if (storageMethod != null)
                Use(storageMethod);

            return StorageMethod.OpenFile<T>(file);
        }
    }
}
