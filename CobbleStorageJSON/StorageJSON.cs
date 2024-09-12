using CobbleApp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CobbleStorageJSON
{
    //put this in exe level project (and Newtonsoft Nuget) - or design another StorageMethod type
    public class StorageJSON : StorageMethod
    {
        //private static JsonSerializer Serializer;

        public StorageJSON()
        {
            Storage.Use(this);
        }

        public override void SaveConfig(string file, UserConfig config)
        {
            var s = JsonHelper.FromClass<UserConfig>(config);

            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.WriteLine(s);
            }
        }
        public override UserConfig OpenConfig(string file)
        {
            using (StreamReader outputFile = new StreamReader(file))
            {

                var s = outputFile.ReadToEnd();
                var c = JsonHelper.ToClass<UserConfig>(s);
                return c;
            }
        }

        public override void SaveFile<T>(string file, T classT)
        {
            var s = JsonHelper.FromClass<T>(classT);

            using (StreamWriter outputFile = new StreamWriter(file))
            {
                outputFile.WriteLine(s);
            }
        }
        public override T OpenFile<T>(string file)
        {
            using (StreamReader outputFile = new StreamReader(file))
            {

                var s = outputFile.ReadToEnd();
                var c = JsonHelper.ToClass<T>(s);
                return c;
            }
        }

        //}
        ////https://www.codegrepper.com/code-examples/javascript/newtonsoft+json+c%23+code+project+example
        public static class JsonHelper
        {
            public static string FromClass<T>(T data, bool isEmptyToNull = false,
                             JsonSerializerSettings jsonSettings = null)
            {
                string response = string.Empty;

                if (!EqualityComparer<T>.Default.Equals(data, default(T)))
                    response = JsonConvert.SerializeObject(data, jsonSettings);

                return isEmptyToNull ? (response == "{}" ? "null" : response) : response;
            }

            public static T ToClass<T>(string data, JsonSerializerSettings jsonSettings = null)
            {
                var response = default(T);

                if (!string.IsNullOrEmpty(data))
                    response = jsonSettings == null
                      ? JsonConvert.DeserializeObject<T>(data)
                      : JsonConvert.DeserializeObject<T>(data, jsonSettings);

                return response;
            }
        }
    }
}
