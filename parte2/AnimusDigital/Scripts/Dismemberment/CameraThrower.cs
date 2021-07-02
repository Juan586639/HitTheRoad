using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CameraThrower : MonoBehaviour {
    private new Camera camera;
    public GameObject ObjectToThrow;
	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && ObjectToThrow)
        {
            var Ray = camera.ScreenPointToRay(Input.mousePosition);
            var Instance = Instantiate(ObjectToThrow).transform;
            Instance.position = Ray.origin;
            Instance.forward = Ray.direction;
        }
	}
}
