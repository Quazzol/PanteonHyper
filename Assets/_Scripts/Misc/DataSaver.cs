using System;
using UnityEngine;

namespace Misc
{
    public class JsonDataSaver
    {
        public static void Save<T>(string key, T dataToSave)
        {
            string jsonData = JsonUtility.ToJson(dataToSave);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public static T Load<T>(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return default(T);

            var jsonData = PlayerPrefs.GetString(key);
            object resultValue = JsonUtility.FromJson<T>(jsonData);
            return (T)Convert.ChangeType(resultValue, typeof(T));
        }
    }
}