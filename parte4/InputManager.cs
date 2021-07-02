using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public float startTime =0f;
    ManeuverVariables maneuverVariables;
    ManeuverVariables upVariables, leftVariables, rightVariables, downVariables;
    private Maneuver dbAsset;
    ManeuverManager maneuverManager;
    ManeuverCombo test;
    public int debug;
    public float valueToReturn;
    public float getaxisvar;
    public float multiplier;
    float currSectionTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        //dbAsset = (Maneuver)AssetDatabase.LoadAssetAtPath("Assets/ListOfBase2.asset", typeof(Maneuver));
        maneuverManager = FindObjectOfType<ManeuverManager>();
        maneuverVariables = new ManeuverVariables();
        //maneuverManager.maneuverVariables = maneuverVariables; //asignación por referencia
    }

    public void ResetTime(float _currSectionTime)
    {
        currSectionTime = _currSectionTime;
    }
    // Update is called once per frame
    void Update()
    {
        getaxisvar = Input.GetAxis("Horizontal");
        //Debug.Log(test.maneuverChain.ToArray()[debug].timeOfExecution);
        //test.maneuverChain.ToArray()[debug].action.Process();
        if (Input.GetKeyDown(KeyCode.W))
        {
            upVariables = new ManeuverVariables();
            upVariables.timePressed = Time.time - currSectionTime;
            //maneuverVariables.timePressed = Time.time;
            Debug.Log((Time.time - startTime).ToString("00:00.00"));
        }
        //if (Input.GetKey(KeyCode.D))
        //{

        //    valueToReturn += Time.deltaTime * multiplier;
        //    if (valueToReturn > 1)
        //        valueToReturn = 1;

        //}
        //else
        //{
        //    valueToReturn -= Time.deltaTime * multiplier;
        //    if (valueToReturn < 0)
        //        valueToReturn = 0;
        //}

        if (Input.GetKeyUp(KeyCode.W))
        {
            upVariables.timeHeld = Time.time - upVariables.timePressed;
            //Debug.Log("Pressed for : " + timePressed + " Seconds");
            Debug.Log("newup");
            maneuverManager.OnActionEnded.Invoke(new Up(),upVariables);
        }
        //Debug.Log(test.maneuverChain.ToArray()[debug].timeOfExecution);
        //test.maneuverChain.ToArray()[debug].action.Process();
        if (Input.GetKeyDown(KeyCode.S))
        {
            downVariables = new ManeuverVariables();
            downVariables.timePressed = Time.time - currSectionTime;
            //maneuverVariables.timePressed = Time.time;
            Debug.Log((Time.time - startTime).ToString("00:00.00"));
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            downVariables.timeHeld = Time.time - downVariables.timePressed;
            //Debug.Log("Pressed for : " + timePressed + " Seconds");
            maneuverManager.OnActionEnded.Invoke(new Down(), downVariables);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightVariables = new ManeuverVariables();
            rightVariables.timePressed = Time.time - currSectionTime;
            //maneuverVariables.timePressed = Time.time;
            //Debug.Log((Time.time - startTime).ToString("00:00.00"));
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            rightVariables.timeHeld = Time.time - rightVariables.timePressed;
            //Debug.Log("Pressed for : " + timePressed + " Seconds");
            maneuverManager.OnActionEnded.Invoke(new Right(),rightVariables);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftVariables = new ManeuverVariables();
            leftVariables.timePressed = Time.time - currSectionTime;
            //maneuverVariables.timePressed = Time.time;
            //Debug.Log((Time.time - startTime).ToString("00:00.00"));
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            leftVariables.timeHeld = Time.time - leftVariables.timePressed;
            //Debug.Log("Pressed for : " + timePressed + " Seconds");
            maneuverManager.OnActionEnded.Invoke(new Left(),leftVariables);
        }
        //Debug.Log(Input.GetAxis("Horizontal"));
    }
}
