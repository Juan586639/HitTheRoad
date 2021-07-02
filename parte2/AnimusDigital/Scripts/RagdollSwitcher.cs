using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSwitcher : MonoBehaviour {
    public bool isRagdoll { get; private set; }

    private void Start()
    {
        DisablePhysics();
    }

    public void EnablePhysics()
    {
        GetComponent<Animator>().enabled = false;
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }
    public void DisablePhysics()
    {
        GetComponent<Animator>().enabled = true;
        foreach (var rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }
}
