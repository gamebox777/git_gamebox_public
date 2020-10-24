using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オフライン時の経過時間テスト
/// </summary>
public class OfflineTimeMnager01 : MonoBehaviour
{
    /// <summary> PlayerPrefsのセーブキー </summary>
    const string SaveKeyString = "OfflineTimeMnager01SaveKey";
    
    /// <summary> 使用するクラス </summary>
    [HideInInspector]
    public ClassSaveData classSaveData;

    /// <summary> 前回辞めた時間(セーブしていた時間) </summary>
    [HideInInspector]
    public DateTime oldDateTime;
    
    private void Awake()
    {
        SaveDataLoad();    //セーブデータをロードする
    }

    // Start is called before the first frame update
    void Start()
    {
        CalculateOfflineEarnings();    // オフライン時の経過時間を計算
    }


    /// <summary>
    /// オフライン時の経過時間を計算
    /// </summary>
    public void CalculateOfflineEarnings()
    {
        DateTime currentDateTime = DateTime.Now;
        if (oldDateTime > currentDateTime) {
            //データ不正：セーブデータの時間の方が今の時間よりも進んでいたので今の時間を入れる
            oldDateTime = DateTime.Now;
        }
        
        //アプリを停止していた時間を計算
        TimeSpan timeElasped = currentDateTime - oldDateTime;    //経過した時間
        float elapsedTimeInSeconds = (int)Math.Round(timeElasped.TotalSeconds, 0, MidpointRounding.ToEven);
        Debug.Log($"オフラインでの経過時間:{elapsedTimeInSeconds}秒");
    }
    
    private void OnApplicationQuit()
    {
        Debug.Log("アプリが中断されたのでセーブ");
        SaveDataSave();
    }

    /// <summary>
    /// セーブデータ：ロード
    /// </summary>
    public void SaveDataLoad()
    {
        //セーブデータ存在チェック
        if (PlayerPrefsJsonUtil.ExistData(SaveKeyString)) {
            //セーブデータ有り
            classSaveData = new ClassSaveData();
            classSaveData = PlayerPrefsJsonUtil.GetObject<ClassSaveData>( SaveKeyString );
            oldDateTime = classSaveData.GetDateTime();
            string str = oldDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            Debug.Log($"アプリ開始時：セーブされていた時間:{str}");
            
            str = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            Debug.Log($"今の時間:{str}");
        }
        else {
            //セーブデータ無し
            Debug.Log("セーブデータがないのでセーブデータ作成");
            classSaveData = CreateSaveData();
            oldDateTime = DateTime.Now;
        }
    }

    /// <summary>
    /// セーブデータ：セーブ
    /// </summary>
    public void SaveDataSave()
    {
        string str = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        Debug.Log($"アプリ終了時：セーブ時の時間:{str}");

        //DateTime型をString型に変換
        classSaveData.dateTimeString = DateTime.Now.ToBinary().ToString();
        PlayerPrefsJsonUtil.SetObject<ClassSaveData>(SaveKeyString,classSaveData,true);
    }
    
    
    #region SaveDataClass

    /// <summary>
    /// セーブデータクラス
    /// </summary>
    [Serializable]
    public struct ClassSaveData
    {
        /// <summary> DateTime型はシリアライズ出来ないので文字列で保存 </summary>
        public string dateTimeString;

        public DateTime GetDateTime()
        {
            DateTime datetime = System.DateTime.FromBinary (System.Convert.ToInt64 (dateTimeString));
            return datetime;
        }
    }

    /// <summary>
    /// セーブデータ作成
    /// </summary>
    /// <returns></returns>
    public ClassSaveData CreateSaveData()
    {
        ClassSaveData saveData = new ClassSaveData
        {
            dateTimeString = DateTime.Now.ToBinary().ToString(),
        };
        return saveData;
    }
    
    #endregion    //SaveDataClass
}

