using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNeuronNetworkWin.Model
{
    class JsonHelper
    {
        public static void Serialize<T>(T obj, string path) //записываем объект в файл
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(obj));
        }

        public static T Deserialize<T>(string path) //загрузка файла
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
    }
}
