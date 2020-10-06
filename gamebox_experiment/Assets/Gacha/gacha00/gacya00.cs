using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

/// <summary>
/// 一番基本のガチャプログラム
/// 重みでガチャを引く
/// </summary>
public class gacya00 : MonoBehaviour
{
    /// <summary> スクリプタブルオブジェクト </summary>
    public Entity_Gacya00 SoData;

    /// <summary> エクセルの内容表示用text </summary>
    public Text TextElementList;

    /// <summary> ガチャ結果表示用タイトルtext </summary>
    public Text TextResultTitle;

    /// <summary> ガチャ結果表示用text </summary>
    public Text TextResultList;

    private List<Entity_Gacya00.Param> mGacyaParam = new List<Entity_Gacya00.Param>();

    /// <summary> ガチャ回数 </summary>
    private int mGacyaCount = 1;
        
    // Start is called before the first frame update
    void Start()
    {
        string tStr = String.Empty;
        //元のエクセルデータを表示
        foreach (var t in SoData.sheets[0].list) {
            float probability = (float)t.Probability;
            tStr += $"ID:{t.ID}  {t.Type} 重み：{t.Weight} 確率：{probability:p2} \n";
        }
        TextElementList.text = tStr;
    }

    /// <summary>
    /// インプットフィールド：ガチャの回数
    /// </summary>
    /// <param name="num"></param>
    public void OnEndEdit(string num)
    {
        mGacyaCount = int.Parse(num);
        Debug.Log($"ガチャ回数{mGacyaCount}");
    }

    /// <summary>
    /// ガチャ回す
    /// </summary>
    public void OnButtonGacya()
    {
        //ガチャ結果保存用Dictionary初期化 (ID:出た回数)
        Dictionary<int,int> GacyaResult =  new Dictionary<int, int>();

        //ガチャ結果保存用Dictionary初期化
        foreach (var t in SoData.sheets[0].list) {
            GacyaResult.Add(t.ID,0);
        }

        //ガチャ回数分回す
        for (int i = 0; i < mGacyaCount; i++) {
            int id = Lottery();
            if (GacyaResult.ContainsKey(id)) {
                GacyaResult[id] += 1;
            }
        }
        
        //ガチャ結果
        string tStr = String.Empty;
        foreach (var t in GacyaResult) {
            Debug.Log($"ガチャ：ID:{t.Key} 出た回数:{t.Value}");
            Entity_Gacya00.Param param = GetIdToElement(t.Key);
            if (param != null) {
                float probability = (float)t.Value / (float)mGacyaCount;
                tStr += $"{param.Type} 回数:{t.Value} 確率:{probability:p2} \n";
            }
            TextResultList.text = tStr;
        }

        TextResultTitle.text = $"【結果】{mGacyaCount}回";

    }
    
    
    /// <summary>
    /// 抽選
    /// </summary>
    /// <returns></returns>
    int Lottery()
    {
        int lotnum = -1;
        //List<int> RangeList = new List<int>();
        Dictionary<int,int> RangeList = new Dictionary<int, int>();
        
        //範囲リストを作る
        int rangeadd = 0;
        foreach (var t in SoData.sheets[0].list) {
            rangeadd += t.Weight;
            RangeList.Add( t.ID,rangeadd );
        }
        Debug.Log($"重みの合計{rangeadd}");
        
        //抽選
        int randnum = UnityEngine.Random.Range(0, rangeadd);
        int num = 0;
        foreach (var t in RangeList) {
            if (randnum < t.Value) {
                return t.Key;
            }
        }

        return lotnum;    //-1が変える
    }

    /// <summary>
    /// IDからデータを取り出す
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Entity_Gacya00.Param GetIdToElement(int id)
    {
        foreach (var t in SoData.sheets[0].list) {
            if (t.ID == id) {
                return t;
            }
        }

        return null;
    }
        
}
