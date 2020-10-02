using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Serialization;

public class Gizmos00 : MonoBehaviour
{
    public Transform CubeRedTransform;
    public Transform CubeBlueTransform;

    [SerializeField]
    private GUIStyle gUIStyle;
    
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawLine( CubeRedTransform.position , CubeBlueTransform.position);

        //赤いキューブの座標
        Handles.Label(CubeRedTransform.position + Vector3.back * 1.5f, $"座標 <color=#ffffff> {CubeRedTransform.position}</color>", gUIStyle);
        
        //青いキューブの座標
        Handles.Label(CubeBlueTransform.position + Vector3.back * 1.5f, $"座標 <color=#ffffff> {CubeBlueTransform.position}</color>", gUIStyle);
    }
#endif    
}
