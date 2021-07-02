using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : Singleton<GameDataManager>
{
    public GameData gameData;
    private float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        gameData = Serializer.Load<GameData>("GameData.cs");
        if (gameData == null)
        {
            gameData = new GameData();
            UpdateGameData();
        }
    }
    private void Update()
    {
        timePassed += Time.deltaTime;
        //Debug.Log("Update: "+timePassed);
        if (Input.GetKeyDown(KeyCode.J))
        {
            UpdateGameData();
        }
    }
    // Update is called once per frame
    public void UpdateGameData()
    {
        Debug.Log("UPDATED");
        Serializer.Save<GameData>("GameData.cs", gameData);
    }

}
