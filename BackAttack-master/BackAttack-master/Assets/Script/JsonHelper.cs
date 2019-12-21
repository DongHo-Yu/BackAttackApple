using System;
using System.IO;


public static class JsonHelper
{
    private static string FilePath = "/Resources/";


    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;

        return  UnityEngine.JsonUtility.ToJson(wrapper);
    }

    public static void SaveJson<T>(T[] array, string SaveName)
    {
        string Data = ToJson<T>(array);
        Data = Data.Replace("{\"Items\":", "");
        Data = Data.Remove(Data.Length - 1, 1);

        string filePath;
#if UNITY_EDITOR
        filePath = UnityEngine.Application.dataPath + "/Json_Stage" + ".json";
#elif UNITY_ANDROID
        filePath = UnityEngine.Application.persistentDataPath + "/Json_Stage" + ".json";
#else
        filePath = UnityEngine.Application.dataPath + "/Json_Stage" + ".json";
#endif
        File.WriteAllText(filePath, Data);
        UnityEngine.Debug.Log("Json File Save ! ");
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}