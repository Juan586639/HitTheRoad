using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManeuverManager : MonoBehaviour
{
    [SerializeField] private Maneuver maneuverToSend;
    public Section currMapSection;
    Maneuver road;
    public GameActionEvent OnActionEnded;
    public BikeController bike;
    public List<Maneuver> currManeuverChain = new List<Maneuver>();
    public List<ManeuverCombo> comboChain;
    public float currHealth;
    public float currEnergy;
    public SaveData data;
    int comboCount = 0;
    public GameData gameData;
    private GameObject ctrlHub;
    private controlHub outsideControls;
    public int test;

    // Start is called before the first frame update
    void Start()
    {
        ctrlHub = GameObject.Find("gameScenario");//link to GameObject with script "controlHub"
        outsideControls = ctrlHub.GetComponent<controlHub>();//to connect c# mobile control script to this one
        //data.SaveIntoJson();
        OnActionEnded.AddListener((IGameActions t, ManeuverVariables manVariables) =>
        {
            CreateManeuver(t, manVariables);
        });
        comboCount = gameData.numberOfCombos;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateManeuverCombo()
    {
        Debug.Log("aqui pasa algo raro");
        ManeuverCombo tempCombo = new ManeuverCombo(new List<Maneuver>(currManeuverChain), bike.bikeSpeed,currHealth,currEnergy,currMapSection);
        comboChain.Add(tempCombo);
        GameDataManager.main.gameData.maneuverCombos = comboChain;
        GameDataManager.main.UpdateGameData();
        currManeuverChain.Clear();
        
        comboCount++;
    }
    public void CreateManeuver(IGameActions gameAction, ManeuverVariables manVariables)
    {
        //Maneuver.tempAction = Test;
        road = new Maneuver(gameAction, manVariables.timePressed, manVariables.timeHeld);
        currManeuverChain.Add(road);
        Debug.Log("nueva maniobra");
        //SaveIntoJson();
    }


    public void SaveIntoJson()
    {
        maneuverToSend = road;
        string maneuverString = JsonUtility.ToJson(maneuverToSend);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PotionData.json", maneuverString);
    }
}
