using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;

namespace Framework
{
    public static class DataHelper
    {
        public static void Save<T>(T data, string filePath)
        {
            //byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            var json = JsonConvert.SerializeObject(data);
            var path = Path.Combine(Application.persistentDataPath, filePath);
            var bytes = Encoding.UTF8.GetBytes(json);
            File.WriteAllBytes(path, bytes);
        }

        public static T Load<T>(string filePath) where T : class
        {
            try
            {
                string path = Path.Combine(Application.persistentDataPath, filePath);
                if (!File.Exists(path))
                    return null;

                byte[] bytes = File.ReadAllBytes(path);

                //return SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
                return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
            }
            catch (Exception e)
            {
                PDebug.Log("Something wrong when load data: {0}", e);
                return null;
            }
        }

        public static void Delete(string filePath)
        {
            string path = Path.Combine(Application.persistentDataPath, filePath);
            if (!File.Exists(path))
                return;

            File.Delete(path);
        }
    }
}