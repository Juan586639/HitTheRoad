using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex = 0;
    public bool isFinishLine = false;
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        MotorcyclePlayerManager manager = other.GetComponent<MotorcyclePlayerManager>();

        //if (GameManager.instance.nextCheckpointIndex >= GameManager.instance.checkpoints.Count - 2 && isFinishLine)
        //{
        //    if(manager.playerIndex == PlayerIndex.Player1)
        //    {
        //        GameManager.instance.gameWonP1 = true;
        //    }
        //    else
        //    {
        //        GameManager.instance.gameWonP2 = true;
        //    }
        //    GameManager.instance.FinishRace();
        //}
        //else
        //{
        //    GameManager.instance.SetCheckpoint(GameManager.instance.checkpoints[checkpointIndex + 1], manager);
        //}
    }
}
