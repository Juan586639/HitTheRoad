using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameStartManager : MonoBehaviour
{
    public GameObject goPanelStartP1, goPanelStartP2;
    bool p1Ready = false, p2Ready = false, canGetReady = true;

    [Header("UI")]
    public GameObject imgStartP1;
    public GameObject imgWinP1, imgLoseP1, imgNotReadyP1;
    [Space(10)]
    public GameObject imgStartP2;
    public GameObject imgWinP2, imgLoseP2, imgNotReadyP2;
    [Space(10)]
    public VideoPlayer videoplayerP1;
    public VideoPlayer videoplayerP2;

    #region Singleton

    public static GameStartManager instance = null;
    private void Awake()
    {
        if (instance)
            return;
        //
        instance = this;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if(canGetReady)
        {
            if(Input.GetAxis("Vertical") != 0 && !p1Ready)
            {
                ReadyPlayer1();
            }
            //
            if (Input.GetAxis("Vertical2") != 0 && !p2Ready)
            {
                ReadyPlayer2();
            }
        }
    }

    public void ReadyPlayer1()
    {
        imgStartP1.SetActive(false);
        goPanelStartP1.SetActive(false);
        videoplayerP1.Stop();
        videoplayerP1.gameObject.SetActive(false);
        p1Ready = true;
        if(!p2Ready)
            StartCoroutine(DisableReadyCoroutine());
    }
    public void ReadyPlayer2()
    {
        imgStartP2.SetActive(false);
        goPanelStartP2.SetActive(false);
        videoplayerP2.Stop();
        videoplayerP2.gameObject.SetActive(false);
        p2Ready = true;
        if(!p1Ready)
            StartCoroutine(DisableReadyCoroutine());
    }

    public IEnumerator DisableReadyCoroutine()
    {
        yield return new WaitForSecondsRealtime(5);
        canGetReady = false;

        if(p1Ready && p2Ready)
        {
            GameManager.instance.StartRace(true);
        }
        else
        {
            if (p1Ready)
            {
                GameManager.instance.StartRace(PlayerIndex.Player1);
                imgStartP2.SetActive(false);
                imgNotReadyP2.SetActive(true);
            }
            //
            if(p2Ready)
            {
                GameManager.instance.StartRace(PlayerIndex.Player2);
                imgStartP1.SetActive(false);
                imgNotReadyP1.SetActive(true);
            }
        }
    }

    public void DeclareWinner(PlayerIndex winnerPlayer)
    {
        goPanelStartP1.SetActive(true);
        goPanelStartP2.SetActive(true);
        //
        switch (winnerPlayer)
        {
            case PlayerIndex.Player1:
                imgWinP1.SetActive(true);
                if (GameManager.instance.isMultiplayer)
                    imgLoseP2.SetActive(true);
                break;
            case PlayerIndex.Player2:
                imgWinP2.SetActive(true);
                if(GameManager.instance.isMultiplayer)
                    imgLoseP1.SetActive(true);
                break;
        }
    }
}
