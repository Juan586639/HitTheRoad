using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;

[System.Serializable]
public class Maneuver
{
    [SerializeField]
    public IGameActions action;
    [SerializeField]
    public float timeOfExecution;
    [SerializeField]
    public float timeHeld;
    [SerializeField]
    public Maneuver dbAsset;
    // Start is called before the first frame update
    void Start()
    {
        //dbAsset = (Maneuver)AssetDatabase.LoadAssetAtPath("Assets / ListOfBase2.asset", typeof(Maneuver));
    }
    public Maneuver(IGameActions _maneuverAction, float _timeOfExecution, float _timeHeld)
    {
        action = _maneuverAction;
        timeOfExecution = _timeOfExecution;
        timeHeld = _timeHeld;
        DebugData();
        //CreateAsset();
    }
    void DebugData()
    {
        //Debug.Log("execution section: "  /*executionSection*/ + "executionTime" + timeOfExecution + " timeHeld: " + timeHeld);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void CreateAsset()
    {
        //var listOfBase = ScriptableObject.CreateInstance<Maneuver>();
        //listOfBase.maneuverAction = maneuverAction;
        //listOfBase.timeOfExecution = timeOfExecution;
        //AssetDatabase.CreateAsset(listOfBase, "Assets/ListOfBase2.asset");
        //AssetDatabase.SaveAssets();
    }
}
