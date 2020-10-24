using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クラスをJSON形式で保存
/// </summary>
public static class PlayerPrefsJsonUtil
{
    /// <summary>
    /// 指定データがあるか
    /// </summary>
    /// <param name="key">データを識別するためのキー</param>
    public static bool ExistData(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    /// <summary>
    /// 指定されたオブジェクトの情報を保存します
    /// </summary>
    public static void SetObject<T>(string key, T obj,bool isSave)
    {
        var json = JsonUtility.ToJson(obj);
        PlayerPrefs.SetString(key, json);
        if ( isSave ) {
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// 指定されたオブジェクトの情報を読み込みます
    /// </summary>
    public static T GetObject<T>(string key)
    {
        var json = PlayerPrefs.GetString(key);
        var obj = JsonUtility.FromJson<T>(json);
        return obj;
    }
}