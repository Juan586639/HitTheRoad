using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keysTest : MonoBehaviour
{
    float c, startTime, timePressed;
    public Maneuver road;
    public ManeuverManager manManager;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TouchButton(5f));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("w"))
        //{
        //    timePressed = Time.time;
        //    Debug.Log((Time.time - startTime).ToString("00:00.00"));
        //}
        //if (Input.GetKey("w") && Time.time - startTime < 1f)
        //{
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    timehe = Time.time;
        //}

        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    timePressed = Time.time - timePressed;
        //    //Debug.Log("Pressed for : " + timePressed + " Seconds");
        //    manManager.CreateManeuver(Agacharse(), timePressed, c);
        //}

    }
    //public Maneuver.GameAction Agacharse()
    //{
    //    Debug.Log("acelerar");
    //    return new Maneuver.GameAction(0);
    //}
    IEnumerator TouchButton(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
