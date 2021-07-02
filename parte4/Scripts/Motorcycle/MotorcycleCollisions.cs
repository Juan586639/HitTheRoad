using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorcycleCollisions : MonoBehaviour
{
    public LayerMask envCollisionLayer;
    public Animator bikerAnimator;

    [Header("UI")]
    public GameObject goRespawnPanel;
    public UnityEngine.UI.Text txtRespawn;

    MotorcycleMovement movement;
    MotorcyclePlayerManager manager;
    MotorcycleCamManager camManager;
    CharacterController controller;

    bool isOutOfRoad = false;
    public float outOfRoadResetTimer = 10.0f;
    float outOfRoadHelper = 0;

    // Start is called before the first frame update
    void Awake()
    {
        movement = GetComponent<MotorcycleMovement>();
        manager = GetComponent<MotorcyclePlayerManager>();
        camManager = GetComponent<MotorcycleCamManager>();
        controller = GetComponent<CharacterController>();
        //
        outOfRoadHelper = outOfRoadResetTimer;
    }

    private void Update()
    {
        if(isOutOfRoad)
        {
            outOfRoadHelper -= Time.deltaTime;

            if(outOfRoadHelper <= 0)
            {
                ResetPosition();
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if(collision.transform.tag == "Env" && movement.actualSpeed > 10)
        {
            bikerAnimator.SetTrigger("bump");
            camManager.ShakeCam();
            movement.BumpSlowDown();
        }

        if(collision.transform.tag == "Track")
        {
            outOfRoadHelper = outOfRoadResetTimer;
            isOutOfRoad = false;
            //goRespawnPanel.SetActive(false);
        }
        else
        {
            var resT = Mathf.Floor(outOfRoadHelper);
            if(resT < 5)
            {
                //goRespawnPanel.SetActive(true);
                txtRespawn.text = "ESTAS FUERA DE LA PISTA, REPOSICIÓN EN " + resT + "...";
            }
            isOutOfRoad = true;
        }
    }

    public void ResetPosition()
    {
        outOfRoadHelper = outOfRoadResetTimer;
        isOutOfRoad = false;
        //
        Transform newPos;
        if (manager.nextCheckpointIndex != 0)
        {
            newPos = GameManager.instance.checkpoints[manager.nextCheckpointIndex - 1].respawnPoint;
        }
        else
        {
            newPos = GameManager.instance.checkpoints[0].respawnPoint;
        }
        //
        controller.enabled = false;
        transform.position = new Vector3(newPos.position.x, newPos.position.y, newPos.position.z);
        transform.rotation = newPos.rotation;
        controller.enabled = true;
    }
}
