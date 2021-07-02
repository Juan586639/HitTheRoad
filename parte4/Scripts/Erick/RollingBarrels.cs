using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrels : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entro al Collider!");
    }
}
