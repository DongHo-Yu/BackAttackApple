using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public int StageID;
    public int IsClear;
}


public static class JsonContainer
{
    private static string FilePath = "/Resources/";
    public static void JsonLoad(out Stage[] StageMeta)
    {
        StageMeta = null;

        try
        {
            if (PlayerPrefs.GetInt("DefaultLoad", 1) == 1)
            {
                TextAsset tAsset = Resources.Load("Json_Stage") as TextAsset;
                StageMeta = JsonHelper.FromJson<Stage>(fixJson(tAsset.text));
                JsonHelper.SaveJson(StageMeta, "Json_Stage");
                PlayerPrefs.SetInt("DefaultLoad", 0);
                PlayerPrefs.Save();
            }

            string filePath;

#if UNITY_EDITOR
            filePath = UnityEngine.Application.dataPath + "/Json_Stage" + ".json";
#elif UNITY_ANDROID
            filePath = UnityEngine.Application.persistentDataPath + "/Json_Stage" + ".json";
                        Debug.Log("And");
#else
            filePath = UnityEngine.Application.dataPath + "/Json_Stage" + ".json";
#endif
            StageMeta = JsonHelper.FromJson<Stage>(fixJson(System.IO.File.ReadAllText(filePath)));
        }
        catch (System.Exception e)
        {
            Debug.Log("Error in Json :" + e.Message);
        }
    }

    private static string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
}

 