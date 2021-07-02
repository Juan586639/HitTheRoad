using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SecondPlayerController : MonoBehaviour
{
    public List<ManeuverCombo> maneuverCombo;
    public float currMinExecutionTime = 0;
    public float value = 0;
    public float value2 = 0;
    public GameObject ctrlHub;
    public controlHub outsideControls;
    Maneuver man;
    public delegate void ActionDelegate(); // This defines what type of method you're going to call.
    public ActionDelegate m_methodToCall; // This is the variable holding the method you're going to call.
    private float timePassed = 0;
    public GameObject maneuversHolder;
    public int comboLenght;
    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        ctrlHub = GameObject.Find("gameScenario");//link to GameObject with script "controlHub"
        outsideControls = ctrlHub.GetComponent<controlHub>();//to connect c# mobile control script to this one
        maneuverCombo = GameDataManager.main.gameData.maneuverCombos;
        SectionManager.main.OnSectionEnded.AddListener((MapSection ms) =>
        {
            ResetManeuvers();
            ExecuteCombos(ms.sectionId);
        });
        ExecuteCombos(-1);
    }
    public void ResetManeuvers()
    {
        Debug.Log("reseting");
        List<ExecutedActions> executedActions = maneuversHolder.GetComponents<ExecutedActions>().ToList();
        executedActions.ForEach(e => Destroy(e));
        inputManager.ResetTime(Time.time);
    }
    public void ExecuteCombos(int sectionId)
    {
        sectionId++;
        Debug.Log("executing: "+sectionId);
        comboLenght = ComboLenght(sectionId);
        for (int i = 0; i < comboLenght ; i++)
        {
            man = CurrManeuver(sectionId);
            Debug.Log("iterator + "+man.timeOfExecution);
            maneuversHolder.AddComponent<ExecutedActions>();
            maneuversHolder.GetComponents<ExecutedActions>()[i].Init(outsideControls, man.timeOfExecution, man.timeHeld, man);
            //ExecutedActions currAction = new ExecutedActions(outsideControls, man.timeOfExecution, man.timeHeld, man);
        }
    }
    //public IEnumerator OnAction(float timeHeld)
    //{
    //    Debug.Log("pasaaqui");
    //    float timePassed = 0;
    //    while (timePassed < timeHeld)
    //    {
    //        // Code to go left here
    //        man.action.OnAction(outsideControls);
    //        timePassed += Time.deltaTime;

    //        yield return null;
    //        Debug.Log("pasa");
    //    }
    //    while (timePassed > timeHeld)
    //    {
    //        // Code to go left here
    //        man.action.OnAction(outsideControls);
    //        timePassed += Time.deltaTime;
    //        //if (man.action.OnAction(outsideControls) <= 0)
    //        //    StopCoroutine(OnAction(timeHeld));
    //        yield return null;
    //    }
    //    //while (!doAction)
    //    //{
    //    //    outsideControls.Vertical = man.action.OffAction();
    //    //}
    //}
    // Update is called once per frame
    void Update()
    {
        //test(man.timeHeld, man);
        Debug.Log(Input.GetAxis("Horizontal"));
    }
    public void test(float timeHeld, Maneuver man)
    {

        Debug.Log("pasaaqui");
        timePassed += Time.deltaTime;
        if (timePassed < timeHeld)
        {
            man.action.OnAction(outsideControls);
        }
        else
        {
            man.action.OffAction(outsideControls);
        }
        // Code to go left here

        Debug.Log("pasa");
    }
    void Accelerate()
    {

    }
    void turn()
    {

    }
    public Maneuver CurrManeuver(int sectionID)
    {
        ManeuverCombo manCombo = maneuverCombo.Where(mc => (mc.mapSection.sectionID == sectionID)).ToArray()[0];
        List<Maneuver> tempManeuverChain = manCombo.maneuverChain.Where(mc => mc.timeOfExecution > currMinExecutionTime).ToList();
        currMinExecutionTime = tempManeuverChain.Min(mc => mc.timeOfExecution);
        Maneuver currManeuver = manCombo.maneuverChain.Where(mc => mc.timeOfExecution == currMinExecutionTime).ToArray()[0];
        return currManeuver;
    }
    public int ComboLenght(int sectionID)
    {
        ManeuverCombo manCombo = maneuverCombo.Where(mc => (mc.mapSection.sectionID == sectionID)).ToArray()[0];
        List<Maneuver> tempManeuverChain = manCombo.maneuverChain.Where(mc => mc.timeOfExecution > currMinExecutionTime).ToList();
        return tempManeuverChain.Count();
    }
}
