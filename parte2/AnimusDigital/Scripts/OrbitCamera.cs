using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {
    [Range(0.3f, 3)]
    public float MaxDistance = 0.3f;
    public Transform[] Targets;
    private float Yaw;
    private float Pitch;
    private Camera cam;
    private int Target = -1;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
        cam.nearClipPlane = 0.01f;
        if (Targets.Length > 0) Target = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && Targets.Length > 0) Target = (Target + 1) % Targets.Length;
        cam.transform.localPosition = Vector3.back * MaxDistance;
        MaxDistance = Mathf.Clamp(MaxDistance - Input.GetAxis("Mouse ScrollWheel"), 0.3f, 3);
        Yaw = (Yaw + Input.GetAxis("Mouse X")) % 360;
        Pitch = Mathf.Clamp(Pitch - Input.GetAxis("Mouse Y"), -90, 90);
        transform.rotation = Quaternion.Euler(Pitch, Yaw, 0);
        if (Target>-1) transform.position = Vector3.Lerp(transform.position, Targets[Target].position, 0.1f);
	}
}
