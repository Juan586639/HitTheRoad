using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MotorcycleMovement : MonoBehaviour
{
    [Header("Configuration")]
    CharacterController characterController;
    MotorcyclePlayerManager playerManager;
    public List<AxleInfo> axleInfos;
    public Transform bikerTransform;

    [Header("Movement")]
    float v = 0f, h = 0f;
    [HideInInspector]
    public bool isMovingV = false, isMovingH = false, isReversing = false, isTurnL = false, isTurnR = false, canThrottle = false, inputDisabled = true;
    [HideInInspector]
    public float actualImpulse = 0, actualSpeed = 0, actualReverseSpeed = 0, actualBrakeForce = 0, actualSteering = 0;

    [Header("Curves")]
    public AnimationCurve accelerationCurve;
    public AnimationCurve brakeCurve;
    public AnimationCurve dragCurve;
    [Space(10)]
    public float maxMotorTorque = 100f;
    public float maxSteeringAngle = 60f;
    public float frictionForce = 50f;
    public float brakingForce = 2000f;
    [Space(10)]
    public bool invertForwardAxis = true;

    private void Awake()
    {
        playerManager = GetComponent<MotorcyclePlayerManager>();
        characterController = GetComponent<CharacterController>();
        inputDisabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputDisabled)
        {
            if (playerManager.playerIndex == PlayerIndex.Player1)
            {
                v = Input.GetAxis("Vertical");
                h = Input.GetAxis("Horizontal");
            }
            else
            {
                v = Input.GetAxis("Vertical2");
                h = Input.GetAxis("Horizontal2");
            }
        }
        else
        {
            v = 0;
            h = 0;
            actualBrakeForce = 1000;
        }
        //
        if(v == 0)
        {
            isMovingV = false;
            isReversing = false;
            inputTimeHelperV = 0;
            if(actualSpeed < 0.1f)
            {
                inputInvertedTimeHelperV = 0;
            }
            actualImpulse = GetInvertedLinearInputV(actualImpulse);
        }
        else
        {
            if (v > 0)
            {
                isMovingV = true;
                isReversing = false;
                actualImpulse = Mathf.Lerp(actualImpulse, maxMotorTorque, GetLinearInput(v,4,actualImpulse));
            }
            else if (v < 0)
            {
                isReversing = true;
                isMovingV = false;
                if (actualImpulse > 0)
                {
                    actualImpulse -= Time.deltaTime * brakingForce;
                }
                else
                {
                    actualImpulse = v * .1f * maxMotorTorque;
                }
            }
            //
            inputInvertedTimeHelperV = 0;
        }
        if (h == 0)
        {
            isMovingH = false;
            inputTimeHelperH = 0;
        }
        else
        {
            isMovingH = true;
        }
        //
        if(actualSpeed < .1f && actualImpulse > 10)
        {
            actualImpulse -= Time.deltaTime * 2500;
        }
        //
        Vector3 movement;
        movement = transform.forward * actualImpulse * Time.deltaTime;
        characterController.SimpleMove(movement);
        //
        Vector3 rotation = Vector3.up * h * maxSteeringAngle * Time.deltaTime/2;
        transform.Rotate(rotation);
        CharacterFaceRelativeToSurface();
        //
        actualSpeed = transform.InverseTransformDirection(characterController.velocity).z;
        actualSteering = GetLinearInputH(h);
    }    
    
    #region Character Rotation and Input Management

    RaycastHit hit;
    Vector3 forwardRelativeToSurfaceNormal;

    private void CharacterFaceRelativeToSurface()
    {
        //For Detect The Base/Surface.
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10))
        {
            forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, hit.normal);
            Quaternion targetRotation = Quaternion.LookRotation(-forwardRelativeToSurfaceNormal, hit.normal); //check For target Rotation.
            bikerTransform.rotation = Quaternion.Lerp(bikerTransform.rotation, targetRotation, Time.deltaTime * 4); //Rotate Character accordingly.
        }
    }

    float inputTimeHelperV = 0;
    float inputInvertedTimeHelperV = 0;
    float inputTimeHelperH = 0;

    float GetLinearInput(float inputValue, float divider, float impulse)
    {
        if(impulse < maxMotorTorque / 3)
            divider *= 5;
        else if(impulse < (maxMotorTorque / 3)*2)
            divider *= 15;
        else
            divider *= 40;
        //
        inputTimeHelperV += Time.deltaTime / divider;
        inputTimeHelperV = Mathf.Clamp01(inputTimeHelperV);

        return Mathf.Lerp(0, inputValue, accelerationCurve.Evaluate(inputTimeHelperV));
    }
    float GetLinearInputV(float inputValue, float endValue)
    {
        inputTimeHelperV += Time.deltaTime * .125f;
        inputTimeHelperV = Mathf.Clamp01(inputTimeHelperV);
        
        return Mathf.Lerp(inputValue, endValue, accelerationCurve.Evaluate(inputTimeHelperV));
    }
    float GetInvertedLinearInputV(float inputValue)
    {
        inputInvertedTimeHelperV += Time.deltaTime / 10;
        inputInvertedTimeHelperV = Mathf.Clamp01(inputInvertedTimeHelperV);
        float value = Mathf.Lerp(inputValue, 0, dragCurve.Evaluate(inputInvertedTimeHelperV));
        
        if (value > 0.75f)
        {
            return value;
        }
        else
        {
            return 0;
        }
    }
    float GetLinearInputH(float inputValue)
    {
        inputTimeHelperH += Time.deltaTime * .5f;
        inputTimeHelperH = Mathf.Clamp01(inputTimeHelperH);
        
        return Mathf.Lerp(0, inputValue, inputTimeHelperH);
    }

    public void ResetTimeHelperV()
    {
        if(inputTimeHelperV > 0.2f)
            inputTimeHelperV = 0.2f;
    }
    #endregion

    #region Crash and Bump

    public void BumpSlowDown()
    {
        if (actualSpeed > 10)
            actualImpulse =  5;
        else if(actualSpeed > 1)
            actualImpulse = 1;
        else
            actualImpulse -= 50;
        //
        ResetTimeHelperV();
    }

    public void Crash()
    {
        actualImpulse = 0;
        inputTimeHelperV = 0;
    }

    public void DisableInput()
    {
        inputDisabled = true;
    }
    public void EnableInput()
    {
        inputDisabled = false;
    }
    #endregion
}

    [System.Serializable]
public class AxleInfo
{
    public WheelCollider wheel;
    public bool motor;
    public bool steering;
}
