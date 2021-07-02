using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutedActions : MonoBehaviour
{
    public controlHub outControls;
    public float timeOfExecution;
    public float timeHeld;
    public Maneuver man;
    private float timePassed = 0;
    bool readyToRun = false;

    // Start is called before the first frame update
    public IEnumerator PreAction(controlHub gameobjectToCoroutine)
    {
        float timePassed = 0;
        Debug.Log("time of exec: "+timeOfExecution);
        while (timePassed <= timeOfExecution)
        {
            timePassed += Time.deltaTime;

            yield return null;
        }
        Debug.Log("pasaliughlkjg,kjg");
        //gameobjectToCoroutine.StartCoroutine(OnAction());
        readyToRun = true;
        gameobjectToCoroutine.StopCoroutine(PreAction(gameobjectToCoroutine));
    }
    private void Update()
    {
        if (readyToRun) { OnAction(); }
        
    }
    //public IEnumerator OnAction()
    //{
    //    Debug.Log("pasaaqui");
    //    float timePassed = 0;
    //    while (timePassed < timeHeld)
    //    {
    //        // Code to go left here
    //        man.action.OnAction(outControls);
    //        timePassed += Time.deltaTime;

    //        yield return null;
    //        Debug.Log("pasa");
    //    }
    //    while (timePassed > timeHeld)
    //    {
    //        // Code to go left here
    //        man.action.OffAction(outControls);
    //        //if (man.action.OnAction(outsideControls) <= 0)
    //        //    StopCoroutine(OnAction(timeHeld));
    //        yield return null;
    //    }
    //    //while (!doAction)
    //    //{
    //    //    outsideControls.Vertical = man.action.OffAction();
    //    //}
    //}
    public void OnAction()
    {
        Debug.Log("pasaaqui");
        timePassed += Time.deltaTime;
        if (timePassed < timeHeld)
        {
            man.action.OnAction(outControls);
        }
        else
        {
            man.action.OffAction(outControls);
        }
        // Code to go left here

        Debug.Log("pasa");
    }
    public void Init(controlHub outsideControls, float execTime, float heldTime, Maneuver _man)
    {
        outControls = outsideControls;
        timeOfExecution = execTime;
        timeHeld = heldTime;
        man = _man;
        outsideControls.StartCoroutine(PreAction(outsideControls));
    }
}
