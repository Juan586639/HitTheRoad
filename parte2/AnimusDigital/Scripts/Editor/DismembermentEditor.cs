using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Dismemberment))]
public class DismembermentEditor : Editor {
    public static bool ShowViewSettings = true;
    public static bool ShowFragmentsSettings = true;

    public static bool DrawBounds = true;
    public static bool DrawVertices = true;
    public static bool DrawEffects = true;
    public static float VertexSize = 0.007f;

    Bounds GetAutomaticBounds(Transform bone)
    {
        var collider = bone.GetComponent<Collider>();
        Bounds bounds = new Bounds();

        if (collider)
        {
            if (collider is BoxCollider)
            {
                var box = (BoxCollider)collider;
                bounds.center = box.center;
                bounds.size = box.size;
            }
            else if (collider is SphereCollider)
            {
                var sphere = (SphereCollider)collider;
                bounds.center = sphere.center;
                bounds.size = Vector3.one * sphere.radius * 2;
            }
            else if (collider is CapsuleCollider)
            {
                var capsule = (CapsuleCollider)collider;
                bounds.center = capsule.center;

                Vector3 Height = (capsule.direction == 0 ? Vector3.right : capsule.direction == 1 ? Vector3.up : Vector3.forward) * (capsule.height - capsule.radius * 2);
                bounds.size = Vector3.one * capsule.radius * 2 + Height;
            }
        }
        else
        {
            for (int n = 0; n < bone.childCount; n++) bounds.Encapsulate(bone.GetChild(n).localPosition);
        }
        return bounds;
    }
    PanelEvents DrawPanel(Rect rect, string Content, Color backgroundColor, bool DrawDeleteButton = false)
    {
        PanelEvents Result = PanelEvents.NoEvent;
        var OldColor = GUI.backgroundColor;
        backgroundColor.a = 0.3f;
        GUI.backgroundColor = backgroundColor;
        GUI.skin.button.alignment = TextAnchor.MiddleLeft;
        GUI.skin.button.fontStyle = FontStyle.Bold;

        GUI.Box(rect, "");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(Content)) Result = PanelEvents.FoldUnfold;
        GUI.skin.button.alignment = TextAnchor.MiddleCenter;
        GUI.skin.button.fontStyle = FontStyle.Normal;
        GUI.backgroundColor = OldColor;
        if (DrawDeleteButton && GUILayout.Button("X", GUILayout.Width(30))) Result = PanelEvents.Delete;
        EditorGUILayout.EndHorizontal();
        return Result;
    }

    void DrawViewSettings()
    {
        DrawBounds = EditorGUILayout.Toggle("Draw bounds", DrawBounds);
        DrawEffects = EditorGUILayout.Toggle("Draw effects", DrawEffects);
        DrawVertices = EditorGUILayout.Toggle("Draw vertices", DrawVertices);
        if (DrawVertices) VertexSize = EditorGUILayout.Slider("Vertex thickness", VertexSize, 0.001f, 0.01f);
        SceneView.RepaintAll();
    }

    void DrawFragmentDetails(SerializedProperty Fragment, bool isRoot=false)
    {
        EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("Name"));
        EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("color"));
        EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("bone"));
        if (isRoot && Fragment.FindPropertyRelative("bone").objectReferenceValue == null)
        {
            Fragment.FindPropertyRelative("bone").objectReferenceValue = ((Dismemberment)target).transform;
        }
        if (isRoot || Fragment.FindPropertyRelative("bone").objectReferenceValue == null) return;
        if (DrawPanel(EditorGUILayout.BeginVertical(), "Bounds", Color.gray) == PanelEvents.FoldUnfold)
        {
            Fragment.FindPropertyRelative("BoundsDetails").boolValue = !Fragment.FindPropertyRelative("BoundsDetails").boolValue;
        }
        if (Fragment.FindPropertyRelative("BoundsDetails").boolValue)
        {
            EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("Position"));
            EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("Rotation"));
            EditorGUILayout.PropertyField(Fragment.FindPropertyRelative("Size"));
            if (GUILayout.Button("Automatic bounds"))
            {
                var bounds = GetAutomaticBounds((Transform)Fragment.FindPropertyRelative("bone").objectReferenceValue);
                Fragment.FindPropertyRelative("Position").vector3Value = bounds.center;
                Fragment.FindPropertyRelative("Size").vector3Value = bounds.size;
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Separator();
        DrawFragmentEffectEditor(Fragment, "BoneEffect");
        DrawFragmentEffectEditor(Fragment, "BoneParentEffect");
    }
    void DrawFragmentEffectEditor(SerializedProperty Fragment, string Name)
    {
        if (DrawPanel(EditorGUILayout.BeginVertical(),Name,Color.gray)== PanelEvents.FoldUnfold)
        {
            Fragment.FindPropertyRelative(Name + "Details").boolValue = !Fragment.FindPropertyRelative(Name + "Details").boolValue;
        }
        if (Fragment.FindPropertyRelative(Name + "Details").boolValue) { 
        EditorGUILayout.PropertyField(Fragment.FindPropertyRelative(Name));
            if (Fragment.FindPropertyRelative(Name).objectReferenceValue != null)
            {

                EditorGUILayout.PropertyField(Fragment.FindPropertyRelative(Name + "Position"));
                EditorGUILayout.PropertyField(Fragment.FindPropertyRelative(Name + "Rotation"));
                EditorGUILayout.PropertyField(Fragment.FindPropertyRelative(Name + "Size"));
            }
        }
        EditorGUILayout.EndVertical();
    }
    void AddFragment(SerializedProperty Fragments)
    {
        Color[] colors = { Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow };
        Fragments.InsertArrayElementAtIndex(Fragments.arraySize);
        var NewFragment = Fragments.GetArrayElementAtIndex(Fragments.arraySize - 1);
        NewFragment.FindPropertyRelative("Name").stringValue = "Fragment" + Fragments.arraySize;
        NewFragment.FindPropertyRelative("color").colorValue = colors[Fragments.arraySize % colors.Length];
        NewFragment.FindPropertyRelative("bone").objectReferenceValue = null;
        NewFragment.FindPropertyRelative("SkinnedMeshes").ClearArray();
        NewFragment.FindPropertyRelative("BoneEffect").objectReferenceValue = null;
        NewFragment.FindPropertyRelative("BoneParentEffect").objectReferenceValue = null;
    } 
    void DrawFragmentsSettings()
    {
        serializedObject.Update();
        var Fragments = serializedObject.FindProperty("Fragments");
        int i = 0;
        while (i < Fragments.arraySize)
        {
            var Fragment = Fragments.GetArrayElementAtIndex(i);
            switch (DrawPanel(EditorGUILayout.BeginVertical(),Fragment.FindPropertyRelative("Name").stringValue,Fragment.FindPropertyRelative("color").colorValue,i>0))
            {
                case PanelEvents.NoEvent:
                    if (Fragment.FindPropertyRelative("ShowProperties").boolValue)
                    {
                        DrawFragmentDetails(Fragment, i == 0);
                        EditorGUILayout.Separator();
                    }
                    i++;
                    break;
                case PanelEvents.FoldUnfold:
                    Fragment.FindPropertyRelative("ShowProperties").boolValue = !Fragment.FindPropertyRelative("ShowProperties").boolValue;
                    break;
                case PanelEvents.Delete:
                    Fragments.DeleteArrayElementAtIndex(i);
                    break;
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Separator();
        }
        if (GUILayout.Button("Add fragment")) AddFragment(Fragments);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        if (DrawPanel(EditorGUILayout.BeginVertical(), "View settings",Color.white) == PanelEvents.FoldUnfold) ShowViewSettings = !ShowViewSettings;
        if (ShowViewSettings) DrawViewSettings();
        EditorGUILayout.EndVertical();

        if (DrawPanel(EditorGUILayout.BeginVertical(), "Fragments settings",Color.white) == PanelEvents.FoldUnfold) ShowFragmentsSettings = !ShowFragmentsSettings;
        if (ShowFragmentsSettings) DrawFragmentsSettings();
        EditorGUILayout.EndVertical();
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.Active)]
    static void DrawGizmos(Dismemberment Script, GizmoType gizmoType)
    {
        if (DrawBounds)
        {
            foreach (var fragment in Script.Fragments)
            {
                if (fragment.bone == null) continue;
                Gizmos.color = fragment.color;
                Gizmos.matrix = Matrix4x4.TRS(fragment.bone.position, fragment.bone.rotation, Vector3.one)*
                                Matrix4x4.TRS(fragment.Position,Quaternion.Euler(fragment.Rotation),fragment.Size);
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }

        if (DrawEffects)
        {
            foreach (var fragment in Script.Fragments)
            {
                Gizmos.color = fragment.color;
                if (fragment.BoneEffect)
                {
                    Gizmos.matrix =
                        Matrix4x4.TRS(fragment.bone.position, fragment.bone.rotation, fragment.bone.localScale) *
                        Matrix4x4.TRS(fragment.BoneEffectPosition, Quaternion.Euler(fragment.BoneEffectRotation), fragment.BoneEffectSize);
                    Gizmos.DrawMesh(fragment.BoneEffect.GetComponentInChildren<MeshFilter>().sharedMesh);
                }
                if (fragment.BoneParentEffect)
                {
                    Gizmos.matrix = Matrix4x4.identity;
                    if (fragment.bone.parent)
                        Gizmos.matrix *= Matrix4x4.TRS(fragment.bone.parent.position, fragment.bone.parent.rotation, fragment.bone.parent.localScale);     
                    Gizmos.matrix*=Matrix4x4.TRS(fragment.BoneParentEffectPosition, Quaternion.Euler(fragment.BoneParentEffectRotation), fragment.BoneParentEffectSize);
                    Gizmos.DrawMesh(fragment.BoneParentEffect.GetComponentInChildren<MeshFilter>().sharedMesh);
                }
            }
        }

        if (DrawVertices && Script.Fragments[0].bone)
        {
            Script.Fragments[0].SkinnedMeshes = Script.Fragments[0].bone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
            Gizmos.matrix = Matrix4x4.TRS(Script.transform.position, Script.transform.rotation, Script.transform.lossyScale);
            foreach (var SkinnedMesh in Script.Fragments[0].SkinnedMeshes)
            {
                var Mesh = new Mesh();
                SkinnedMesh.BakeMesh(Mesh);
                var Vertices = Mesh.vertices;
                var ColorMap = new Color[Mesh.vertices.Length];
                for (int i = 0; i < ColorMap.Length; i++) ColorMap[i] = Script.Fragments[0].color;

                var SortedFragments = Script.Fragments.OrderBy(f => f.bone.GetDepth()).ToList();
                foreach (var fragment in SortedFragments)
                {
                    foreach (var index in Dismemberment.GetSelection(SkinnedMesh,fragment))
                    {
                        ColorMap[index] = fragment.color;
                    }
                }

                for (int i=0; i<Vertices.Length;i++)
                {
                    Gizmos.color = ColorMap[i];
                    Gizmos.DrawCube(Vertices[i], Vector3.one * VertexSize);
                }
            }
        }
    }
    enum PanelEvents { NoEvent, FoldUnfold, Delete }
}
