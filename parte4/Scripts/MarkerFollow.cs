using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerFollow : MonoBehaviour
{
    public Transform tTarget;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tTarget)
        {
            Vector3 newPos = tTarget.position + (Vector3.up * 100);
            transform.position = newPos;
        }
    }
}
