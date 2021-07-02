#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AssignSplatMap))]
public class AssignSplatMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AssignSplatMap myScript = (AssignSplatMap)target;
        if (GUILayout.Button("Assign Splatmap"))
        {
            myScript.AssignMap();
        }
    }
}
#endif