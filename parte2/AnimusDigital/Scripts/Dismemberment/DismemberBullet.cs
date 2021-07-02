using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DismemberBullet : MonoBehaviour {
    public float Speed = 1;
    public float Lifetime = 3;
	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
	}
	
	// Update is called once per frame
	void Update () {
        Lifetime -= Time.deltaTime;
        if (Lifetime < 0) Destroy(gameObject);
	}
    private void OnCollisionEnter(Collision collision)
    {
        var Ragdoll = collision.gameObject.GetComponentInParent<RagdollSwitcher>();
        if (Ragdoll && !Ragdoll.isRagdoll) Ragdoll.EnablePhysics();;
        
        var Dismemberer = collision.gameObject.GetComponent<JointDismemberer>();
        if (Dismemberer == null) return;
        collision.gameObject.GetComponent<Joint>().breakForce = 0;
    }
}
