using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorcyclePlayerManager : MonoBehaviour
{
    public PlayerIndex playerIndex = PlayerIndex.Player1;
    [HideInInspector]
    public MotorcycleMovement movement;

    [Header("Camera Settings")]
    public Camera playerCam;
    public Rect cameraRectSettings = new Rect();

    [Header("Race")]
    public int currentPosition = 1;
    public int currentLap = 1;
    public int nextCheckpointIndex = 0;

    [Header("UI")]
    public Text txtPosicion;
    public Text txtVelocidad;
    public Text txtVueltas;

    private void Awake()
    {
        movement = GetComponent<MotorcycleMovement>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(playerIndex == PlayerIndex.Player1)
        {

        }
        else
        {

        }
        //
        playerCam.rect = cameraRectSettings;
    }

    // Update is called once per frame
    void Update()
    {
        //txtPosicion.text = "POS " + currentPosition + "/2";
        //txtVelocidad.text = Mathf.FloorToInt(movement.actualSpeed).ToString();
        //txtVueltas.text = "VUELTA " + currentLap + "/" + GameManager.instance.numberOfLaps;
    }
}

public enum PlayerIndex
{
    Player1,
    Player2
}
