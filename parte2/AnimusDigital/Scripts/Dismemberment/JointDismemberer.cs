using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointDismemberer : MonoBehaviour {
    private Dismemberment dismemberment;

    private void Awake()
    {
        dismemberment = GetComponentInParent<Dismemberment>();
    }
    private void OnJointBreak(float breakForce)
    {
        dismemberment.Dismember(name);
        Destroy(this);
    }
}
