using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dismemberment : MonoBehaviour {
    public List<BodyFragment> Fragments = new List<BodyFragment>() { new BodyFragment() { color= Color.black, Name="ROOT" } };
    private bool isInitialized = false;
    private List<TransformPose> PoseTransforms = new List<TransformPose>();

    private void Awake()
    {
        var Transforms = Fragments[0].bone.GetComponentsInChildren<Transform>();
        foreach (var t in Transforms)
        {
            PoseTransforms.Add(new TransformPose()
            {
                transform = t,
                InitialLocalPosition = t.localPosition,
                InitialLocalRotation = t.localRotation,
                InitialLocalScale = t.localScale
            });
        }
    }
    void SetPose(Pose pose)
    {
        int i = 0;
        while (i<PoseTransforms.Count)
        {
            if (PoseTransforms[i].transform /*&& PoseTransforms[i].transform.IsChildOf(Fragments[0].bone)*/) i++;
            else PoseTransforms.RemoveAt(i);
        }
        foreach (var PoseTransform in PoseTransforms)
        {
            if (pose== Pose.Initial)
            {
                PoseTransform.LocalPosition = PoseTransform.transform.localPosition;
                PoseTransform.LocalRotation = PoseTransform.transform.localRotation;
                PoseTransform.LocalScale = PoseTransform.transform.localScale;
            }
            PoseTransform.transform.localPosition = pose == Pose.Current ? PoseTransform.LocalPosition : PoseTransform.InitialLocalPosition;
            PoseTransform.transform.localRotation = pose == Pose.Current ? PoseTransform.LocalRotation : PoseTransform.InitialLocalRotation;
            PoseTransform.transform.localScale = pose == Pose.Current ? PoseTransform.LocalScale : PoseTransform.InitialLocalScale;
        }
    }

    void Init()
    {
        if (Fragments[0].bone == null) Fragments[0].bone = transform;
        Fragments[0].SkinnedMeshes = Fragments[0].bone.GetComponentsInChildren<SkinnedMeshRenderer>().ToList();
        foreach (var skinnedMesh in Fragments[0].SkinnedMeshes) skinnedMesh.sharedMesh = skinnedMesh.sharedMesh.Copy();
    }
    public void Dismember(string FragmentName)
    {
        var fragment = Fragments.Find(f => f.Name == FragmentName);
        if (fragment != null)
        {
            if (!isInitialized)
            {
                Init();
                isInitialized = true;
            }
            Dismember(fragment);
        }
        else Debug.LogError("Fragment with name " + FragmentName + " not found!");
    }
    void Dismember(BodyFragment fragment)
    {
        //1. Find parent fragment
        var Parents = Fragments.FindAll(f => fragment.bone.IsChildOf(f.bone) && f.SkinnedMeshes != null && f.SkinnedMeshes.Count > 0);
        if (Parents == null || Parents.Count == 0) return;
        var Parent = fragment.GetNearestParent(Parents.ToArray());

        //2. Set initial pose
        SetPose(Pose.Initial);
        //3. Create Dummies
        var DummyForParent = CreateDummy(fragment.bone, fragment.bone.parent);
        var Dummy = CreateDummy(fragment.bone.parent, fragment.bone);
        //4. Cut all parent skinned meshes
        int i = 0;
        while (i < Parent.SkinnedMeshes.Count)
        {
            if (CutSkinnedMesh(Parent.SkinnedMeshes[i], fragment, Dummy, DummyForParent)) Parent.SkinnedMeshes.RemoveAt(i);
            else i++;
        }
        //5. Return pose
        SetPose(Pose.Current);
        //6. Instantiate effects
        if (fragment.BoneEffect)
        {
            var BoneEffect = Instantiate(fragment.BoneEffect, fragment.bone).transform;
            BoneEffect.localPosition = fragment.BoneEffectPosition;
            BoneEffect.localRotation = Quaternion.Euler(fragment.BoneEffectRotation);
            BoneEffect.localScale = fragment.BoneEffectSize;
        }
        if (fragment.BoneParentEffect)
        {
            var BoneParentEffect = Instantiate(fragment.BoneParentEffect, fragment.bone.parent).transform;
            BoneParentEffect.localPosition = fragment.BoneParentEffectPosition;
            BoneParentEffect.localRotation = Quaternion.Euler(fragment.BoneParentEffectRotation);
            BoneParentEffect.localScale = fragment.BoneParentEffectSize;
        }
        //7. Create LOD group if needed
        var LODGroup = GetComponentInChildren<LODGroup>();
        if (LODGroup != null) CloneLODGroupForFragment(fragment, LODGroup);
        //8. Disconnect fragment
        fragment.bone.SetParent(null);
    }

    void CloneLODGroupForFragment(BodyFragment fragment, LODGroup original)
    {
        var LODGroup = fragment.bone.gameObject.AddComponent<LODGroup>();
        var LODs = original.GetLODs();
        for (int i=0; i<LODs.Length; i++)
        {
            LODs[i].renderers = fragment.SkinnedMeshes.Where(m => m.name[m.name.Length - 1].ToString() == i.ToString()).ToArray();
        }
        LODGroup.SetLODs(LODs);
    }
    public static List<int> GetSelection(SkinnedMeshRenderer skinnedMesh, BodyFragment fragment)
    {
        List<int> Selection = new List<int>();
        if (fragment.bone == null) return Selection;
        var Mesh = new Mesh();
        skinnedMesh.BakeMesh(Mesh);
        var Vertices = Mesh.vertices;
        var Bounds = new Bounds(Vector3.zero,Vector3.one);
        var M = Matrix4x4.TRS(fragment.bone.position, fragment.bone.rotation, fragment.bone.lossyScale) *
                Matrix4x4.TRS(fragment.Position, Quaternion.Euler(fragment.Rotation), fragment.Size);
        M = Matrix4x4.Inverse(M);
        for (int i=0; i<Vertices.Length;i++)
        {
            if (Bounds.Contains(M.MultiplyPoint(skinnedMesh.transform.TransformPoint(Vertices[i])))) {
                Selection.Add(i);
            }
        }
        return Selection;
    }
    bool CutSkinnedMesh(SkinnedMeshRenderer skinnedMesh, BodyFragment fragment, Transform Dummy, Transform DummyForParent)
    {
        //1. Get all child fragments
        var ChildFragments = Fragments.FindAll(f => f.bone.IsChildOf(fragment.bone));
        //2. Get vertex selection of all child fragments
        List<int> Selection = new List<int>();
        foreach (var f in ChildFragments) Selection.AddRange(GetSelection(skinnedMesh, f));
        //3. If selection is empty, then don't do anything
        if (Selection.Count == 0) return false;
        //4. Copy skinned mesh
        fragment.SkinnedMeshes.Add(skinnedMesh.Copy(fragment.bone.gameObject));
        fragment.SkinnedMeshes[fragment.SkinnedMeshes.Count - 1].rootBone = fragment.bone;
        //5. If selection covers entire mesh, then destroy parent skinned mesh
        Selection = Selection.Distinct().ToList();
        if (Selection.Count == skinnedMesh.sharedMesh.vertices.Length)
        {
            Destroy(skinnedMesh);
            ReplaceBonesOnDummy(fragment.SkinnedMeshes[fragment.SkinnedMeshes.Count - 1], Dummy, fragment.bone, DummyReplacementMode.ReplaceParents);
            return true;
        }
        //6. Separate selection into fragment shared mesh
        fragment.SkinnedMeshes[fragment.SkinnedMeshes.Count - 1].sharedMesh = skinnedMesh.sharedMesh.SeparateVerts(Selection.ToArray());
        //7. Set dummy for parent
        ReplaceBonesOnDummy(skinnedMesh, DummyForParent, fragment.bone, DummyReplacementMode.ReplaceChildren);
        //8. Set dummy for fragment
        ReplaceBonesOnDummy(fragment.SkinnedMeshes[fragment.SkinnedMeshes.Count - 1], Dummy, fragment.bone, DummyReplacementMode.ReplaceParents);
        return false;
    }

    Transform CreateDummy(Transform t, Transform Parent)
    {
        var Dummy = new GameObject("Dummy").transform;
        Dummy.SetPositionAndRotation(t.position, t.rotation);
        Dummy.SetParent(Parent);
        return Dummy;
    }
    void ReplaceBonesOnDummy(SkinnedMeshRenderer skinnedMesh, Transform Dummy, Transform CrackedBone, DummyReplacementMode mode)
    {
        var bones = skinnedMesh.bones;
        for (int i = 0; i < bones.Length; i++)
        {
            switch (mode)
            {
                case DummyReplacementMode.ReplaceChildren:
                    if (bones[i].IsChildOf(CrackedBone)) bones[i] = Dummy;
                    break;
                case DummyReplacementMode.ReplaceParents:
                    if (!bones[i].IsChildOf(CrackedBone)) bones[i] = Dummy;
                    break;
            }
        }
        skinnedMesh.bones = bones;
    }
    enum DummyReplacementMode { ReplaceParents, ReplaceChildren }
    enum Pose { Initial, Current }

    class TransformPose
    {
        public Transform transform;
        public Vector3 InitialLocalPosition;
        public Quaternion InitialLocalRotation;
        public Vector3 InitialLocalScale;

        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;
    }
}
[System.Serializable]
public class BodyFragment
{
    public bool ShowProperties;
    public string Name;
    public Transform bone;

    public bool BoundsDetails;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Size;

    public GameObject BoneEffect;
    public bool BoneEffectDetails;
    public Vector3 BoneEffectPosition;
    public Vector3 BoneEffectRotation;
    public Vector3 BoneEffectSize;

    public GameObject BoneParentEffect;
    public bool BoneParentEffectDetails;
    public Vector3 BoneParentEffectPosition;
    public Vector3 BoneParentEffectRotation;
    public Vector3 BoneParentEffectSize;

    public Color color;
    public List<SkinnedMeshRenderer> SkinnedMeshes;

    public BodyFragment GetNearestParent(params BodyFragment[] Parents)
    {
        var Parent = bone.parent;
        while (Parent != null)
        {
            for (int i = 0; i < Parents.Length; i++) if (Parent == Parents[i].bone) return Parents[i];
            Parent = Parent.parent;
        }
        return null;
    }
}
