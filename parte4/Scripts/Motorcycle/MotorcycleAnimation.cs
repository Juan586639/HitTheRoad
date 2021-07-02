using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleAnimation : MonoBehaviour
{
    [Header("Animator")]
    public Animator animBiker;
    MotorcycleMovement movement;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<MotorcycleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        animBiker.SetBool("driving_forward", movement.isMovingV);
        animBiker.SetBool("driving_backward", movement.isReversing);
        //
        animBiker.SetFloat("forward_speed", movement.actualSpeed);
        animBiker.SetFloat("steering", movement.actualSteering);
    }
}
