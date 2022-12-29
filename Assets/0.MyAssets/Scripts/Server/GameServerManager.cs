using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class GameServerManager : MonoBehaviourPunCallbacks
{
    public static GameServerManager instance;
    public GameObject GamePannel;
    public TextMeshProUGUI MainText;
    public GameObject[] Button;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerValue;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        //타이머   
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        //게임 시작
    }
    public void GameStop() {
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
    }

    public void Value_Zero() { 
    
    }
    public void Value_One() { 
    }
    public void Value_Two()
    {
    }
    public void Value_Three()
    {
    }
    public void Value_Four()
    {
    }
    public void Value_Five()
    {
    }
    public void Value_Six()
    {
    }
    public void Value_Seven()
    {
    }
    public void Value_Eight()
    {
    }
    public void Value_Nine()
    {
    }
}
