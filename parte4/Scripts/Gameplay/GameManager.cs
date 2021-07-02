using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Gameplay")]
    public int numberOfLaps = 1;
    public List<Checkpoint> checkpoints = new List<Checkpoint>();
    Checkpoint furtherCheckpoint = null;
    [HideInInspector]
    public int nextCheckpointIndex = 0;
    //
    [HideInInspector]
    public bool raceStarted = false, isMultiplayer = false, gameWonP1 = false, gameWonP2 = false;

    [Header("Players")]
    PlayerIndex playerPlaying;
    public MotorcyclePlayerManager managerPlayer1;
    public MotorcyclePlayerManager managerPlayer2;

    [Header("UI")]
    public GameObject StartPanelP1;
    public GameObject StartPanelP2;

    #region Singleton

    public static GameManager instance = null;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        //
        instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        furtherCheckpoint = checkpoints[0];
        nextCheckpointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePositions();
        //
        if(Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RestartLevel();
        }
    }

    #region Race

    public void StartRace(bool isMultiplayerEnable)
    {
        isMultiplayer = isMultiplayerEnable;

        if (isMultiplayer)
        {
            managerPlayer1.movement.EnableInput();
            managerPlayer2.movement.EnableInput();
        }
        else
        {
            managerPlayer1.movement.EnableInput();
        }

        raceStarted = true;
    }

    public void StartRace(PlayerIndex singlePlayerIndex)
    {
        if (singlePlayerIndex == PlayerIndex.Player1)
        {
            managerPlayer1.movement.EnableInput();
        }
        else
        {
            managerPlayer2.movement.EnableInput();
        }

        playerPlaying = singlePlayerIndex;
        raceStarted = true;
    }

    public void FinishRace()
    {
        managerPlayer1.movement.DisableInput();
        managerPlayer2.movement.DisableInput();
        //
        if (isMultiplayer)
        {
            if(gameWonP1)
            {
                GameStartManager.instance.DeclareWinner(PlayerIndex.Player1);
            }
            else if(gameWonP2)
            {
                GameStartManager.instance.DeclareWinner(PlayerIndex.Player2);
            }
        }
        else
        {
            GameStartManager.instance.DeclareWinner(playerPlaying);
        }
        //
        raceStarted = false;
        StartCoroutine(DelayedRestart());
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator DelayedRestart()
    {
        yield return new WaitForSecondsRealtime(10);
        RestartLevel();
    }

    public void SetCheckpoint(Checkpoint checkpoint, MotorcyclePlayerManager player)
    {
        player.nextCheckpointIndex = checkpoints.IndexOf(checkpoint);
        SetNextCheckpoint(checkpoint);
    }

    public void SetNextCheckpoint(Checkpoint checkpoint)
    {
        if(checkpoints.IndexOf(checkpoint) > nextCheckpointIndex)
        {
            nextCheckpointIndex = checkpoints.IndexOf(checkpoint);
            furtherCheckpoint = checkpoints[nextCheckpointIndex];
        }
    }

    #endregion

    #region Calculations

    public void CalculatePositions()
    {
        float distanceToNextCheckpointP1 = Vector3.Distance(managerPlayer1.transform.position, furtherCheckpoint.transform.position);
        float distanceToNextCheckpointP2 = Vector3.Distance(managerPlayer2.transform.position, furtherCheckpoint.transform.position);

        if(distanceToNextCheckpointP1 < distanceToNextCheckpointP2)
        {
            managerPlayer1.currentPosition = 1;
            managerPlayer2.currentPosition = 2;
        }
        else
        {
            managerPlayer1.currentPosition = 2;
            managerPlayer2.currentPosition = 1;
        }
    }

    #endregion
}
