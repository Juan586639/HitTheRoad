using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ikTest : MonoBehaviour
{
    Animator myAnimator;

    // variables for turn IK link off for a time
    public int IK_rightWeight = 4;
    public int IK_leftWeight = 4;
    // variables for hand IK joint points
    public Transform IK_rightHandTarget;
    public Transform IK_leftHandTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnAnimatorIK(int layerIndex)
    {
        Debug.Log(layerIndex);
        if (IK_rightHandTarget != null)
        {
            Debug.Log(AvatarIKGoal.RightHand);
            myAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, (float)IK_rightWeight);
            myAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, (float)IK_rightWeight);
            myAnimator.SetIKPosition(AvatarIKGoal.RightHand, IK_rightHandTarget.position);
            myAnimator.SetIKRotation(AvatarIKGoal.RightHand, IK_rightHandTarget.rotation);
            //leftHand.LookAt(IK_leftHandTarget);
        }
        if (IK_leftHandTarget != null)
        {
            Debug.Log("aqui2");
            //Debug.Log(AvatarIKGoal.LeftHand);
            myAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, (float)IK_leftWeight);
            myAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, (float)IK_leftWeight);
            myAnimator.SetIKPosition(AvatarIKGoal.LeftHand, IK_leftHandTarget.position);
            myAnimator.SetIKRotation(AvatarIKGoal.LeftHand, IK_leftHandTarget.rotation);
            //RightHand.position = IK_leftHandTarget.position;
        }
        //same for a head
        //myAnimator.SetLookAtPosition(lookPoint.transform.position);
        //myAnimator.SetLookAtWeight(0.5f);//0.5f - means it rotates head 50% mixed with real animations 
    }
}
