using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Gizmos00 : MonoBehaviour
{
    public Transform CubePlayerTransform;

    public Transform CubeSubTransform;

    [SerializeField]
    private GUIStyle gUIStyle;
    
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawLine( CubePlayerTransform.position , CubeSubTransform.position);
    }
#endif    
}
