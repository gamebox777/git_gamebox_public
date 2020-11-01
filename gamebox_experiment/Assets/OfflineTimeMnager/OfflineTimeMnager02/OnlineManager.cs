using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class OnlineManager : MonoBehaviour
{
    [Serializable]
    public class _BaseSettings {
        /// <summary>
        /// 貯まる資源の最大数
        /// </summary>
        //[Tooltip("Maximum number of lives by default for all players.")]
        [CustomLabel("資源最大数")]
        public int MaxResource = 999;

        /// <summary>
        /// Time to recover one life in minutes.
        /// 1つの生命を回復する時間
        /// </summary>
        [CustomLabel("資源を1獲得する時間(秒)")]
        public double SecondsToAcquired = 5d;
    }
    public _BaseSettings BaseSettings = new _BaseSettings();

    public Text TextResource;
    public Text TextTimer;
    
    /// <summary>
    /// 現在の資源数
    /// </summary>
    private int mCurrentResources;
    
    /// <summary>
    /// 次の資源を獲得するまでのタイマー(加算)
    /// </summary>
    private double mSecondsToNextResourceTimer;
    
    /// <summary>
    /// todo:OfflineTimeManager02.csのawakeでオフライン経過時間を出してiいる
    /// </summary>
    private void Start()
    {
        mCurrentResources = 0;
        mSecondsToNextResourceTimer = 0d;
        SaveDataLoad();    //セーブデータをロードする
        CalculateOfflineEarnings();    //オフライン経過分を計算
    }

    /// <summary>
    /// オフライン時の経過時間から獲得資源を計算
    /// </summary>
    private void CalculateOfflineEarnings()
    {
        //オフライン時の経過時間取得(秒)
        double elapsedTime = OfflineTimeManager02.instance.ElapsedTimeInSeconds;
        Debug.Log($"OnlineManager経過時間:{elapsedTime}");

        //資源獲得数
        int GetResourceNum = (int)(elapsedTime / BaseSettings.SecondsToAcquired);
        Debug.Log($"オフライン獲得資源数:{GetResourceNum}");

        mCurrentResources = classSaveData.SaveResources + GetResourceNum;
        
        //獲得数に達さなかった時間を経過時間に足す
        mSecondsToNextResourceTimer = elapsedTime - (BaseSettings.SecondsToAcquired * GetResourceNum );
        Debug.Log($"資源獲得に達さなくて余った時間:{mSecondsToNextResourceTimer:F3}");

        if ((mSecondsToNextResourceTimer + classSaveData.SecondsToNextResourceTimer) > BaseSettings.SecondsToAcquired) {
            mCurrentResources += 1;
            mSecondsToNextResourceTimer -= BaseSettings.SecondsToAcquired;
        }
    }

    private void Update()
    {
        mSecondsToNextResourceTimer += Time.unscaledDeltaTime;
        TextTimer.text = "資源が増えるtimer:" + mSecondsToNextResourceTimer.ToString();
        TextResource.text = "資源数:" + mCurrentResources.ToString();
        if (mSecondsToNextResourceTimer > BaseSettings.SecondsToAcquired) {
            mCurrentResources += 1;
            mSecondsToNextResourceTimer = 0;
        }

        //セーブデータの値更新 ※保存はしていない
        classSaveData.SaveResources = mCurrentResources;
        classSaveData.SecondsToNextResourceTimer = mSecondsToNextResourceTimer;
    }
    
    //--------------------------------------------------------------------------
    //
    // セーブデータ部分
    //
    //--------------------------------------------------------------------------
    
    /// <summary> PlayerPrefsのセーブキー </summary>
    const string SaveKeyString = "OnlineManager00";
    
    /// <summary> 使用するクラス </summary>
    [HideInInspector]
    public ClassSaveData classSaveData;

    /// <summary>
    /// アプリケーションが終了された時に呼ばれる
    /// </summary>
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
            classSaveData = PlayerPrefsJsonUtil.GetObject<ClassSaveData>( SaveKeyString );
        }
        else {
            //セーブデータ無し
            Debug.Log("セーブデータがないのでセーブデータ作成");
            classSaveData = CreateSaveData();
            classSaveData.SaveResources = 0;
            classSaveData.SecondsToNextResourceTimer = 0d;
        }
    }

    /// <summary>
    /// セーブデータ：セーブ
    /// </summary>
    public void SaveDataSave()
    {
        PlayerPrefsJsonUtil.SetObject<ClassSaveData>(SaveKeyString,classSaveData,true);
    }
    
    /// <summary>
    /// セーブデータ作成
    /// </summary>
    /// <returns></returns>
    public ClassSaveData CreateSaveData()
    {
        ClassSaveData saveData = new ClassSaveData
        {
            SaveResources = 0,
            SecondsToNextResourceTimer = 0d,
        };
        return saveData;
    }
    
    /// <summary>
    /// セーブデータクラス
    /// </summary>
    [Serializable]
    public struct ClassSaveData
    {
        public int SaveResources;
        public double SecondsToNextResourceTimer;
    }
}
