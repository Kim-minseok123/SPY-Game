using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;

public class GameServerManager : MonoBehaviourPunCallbacks
{
    //스파이 게임 메인 질문
    string[] Question;
    int[] QuestionType;
    //싱글톤
    public static GameServerManager instance;
    public GameObject GamePannel;
    //질문
    public GameObject MainTextObj;
    public TextMeshProUGUI MainText;
    //선택할 버튼
    public GameObject SelectButtonGroup;
    public GameObject[] Buttons;
    public GameObject[] SelectPlayerButton;
    public TextMeshProUGUI[] SelectPlayetName;
    //현재 누른 값
    public TextMeshProUGUI CurrentValue;
    //플레이어 값
    public GameObject PlayerResult;
    public TextMeshProUGUI PlayerValue;
    public GameObject[] PlayerVoteButton;
    public TextMeshProUGUI[] PlayetVoteName;
    //타이머
    public TextMeshProUGUI TimerText;
    private int Timer;
    //0 : 질문 나옴, 1 : 투표
    private int index = 0;
    //플레이어 프로퍼티
    public int State = -1;
    bool QS = false;
    private float time;

    public int PlayerID = -1;

    private bool isSpy = false;
    private int Value = -100;
    private bool isVote = false;
    private bool[] PlayerVote;
    private int[] PlayerValues;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        time = 3f;
        Question = new string[5];
        QuestionType = new int[5];
        Question[0] = "당신은 이번 달에 영화를 몇편을 봤습니까?";
        QuestionType[0] = 1;
        Question[1] = "차가운 피자를 좋아하시나요?";
        QuestionType[1] = 2;
        Question[2] = "당신은 연애하는 것보다 독신으로 사는게 낫다고 생각하시나요?";
        QuestionType[2] = 3;
        Question[3] = "당신은 얼마나 과감한 사람인가요?";
        QuestionType[3] = 4;
        Question[4] = "가장 도박을 잘할거 같은 사람은 누구인가요?";
        QuestionType[4] = 5;
    }
    // Update is called once per frame
    void Update()
    {
        //타이머   
        if (State == 0 && QS)
        {
            if (time > 0) { time -= Time.deltaTime; return; }
            QuestionRandom();
            QS = false;
            time = 60;
        }
        else if (State == 1) {
            if (time > 0) {
                time -= Time.deltaTime;
                photonView.RPC("EndQuestion", RpcTarget.All);
                photonView.RPC("UpdateTime", RpcTarget.All, time);
            }
        }
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        //게임 시작
        if (PhotonNetwork.IsMasterClient) {
            int SpyNumber = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            photonView.RPC("SetSpy", RpcTarget.All, SpyNumber);
            PlayerVote = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            PlayerValues = new int[PhotonNetwork.CurrentRoom.PlayerCount];
            for (int i = 0; i < PlayerVote.Length; i++) PlayerVote[i] = false;
            QS = true;
            State = 0;
        }
    }
    [PunRPC]
    public void SetSpy(int SpyNumber) {
        if (SpyNumber == PlayerID) { isSpy = true; }
    }
    public void QuestionRandom() {
        index = Random.Range(0, 5);
        photonView.RPC("QuestionOpen", RpcTarget.All, index);
    }
    [PunRPC]
    public void QuestionOpen(int index) {
        for (int j = 0; j < 5; j++) Buttons[j].SetActive(false);
        MainTextObj.SetActive(true);
        SelectButtonGroup.SetActive(true);
        switch (QuestionType[index]) {
            case 1:
                Buttons[0].SetActive(true);
                break;
            case 2:
                Buttons[1].SetActive(true);
                break;
            case 3:
                Buttons[2].SetActive(true);
                break;
            case 4:
                Buttons[3].SetActive(true);
                break;
            case 5:
                Buttons[4].SetActive(true);
                break;
        }
        if (isSpy)
        {
            MainText.text = "당신은 스파이입니다. 신중하게 투표해주세요.";
        }
        else
            MainText.text = Question[index];
    }

    [PunRPC]
    public void SetVote(int index, int Values) {
        PlayerVote[index] = true;
        PlayerValues[index] = Values;
        for (int i = 0; i < PlayerVote.Length; i++) {
            if (PlayerVote[i] != true) { return; }
        }
        State = 1;
    }
    [PunRPC]
    public void EndQuestion() {
        MainTextObj.SetActive(false);
        SelectButtonGroup.SetActive(false);
        PlayerResult.SetActive(true);
    }
    [PunRPC]
    public void UpdateTime(float timer) {
        TimerText.text = "남은시간 : \n"+((int)timer).ToString() + " 초";
    }
    public void Value_Zero() {
        isVote = true;
        Value = 0;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_One() {
        isVote = true;
        Value = 1;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Two()
    {
        isVote = true;
        Value = 2;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Three()
    {
        isVote = true;
        Value = 3;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Four()
    {
        isVote = true;
        Value = 4;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Five()
    {
        isVote = true;
        Value = 5;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Six()
    {
        isVote = true;
        Value = 6;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Seven()
    {
        isVote = true;
        Value = 7;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Eight()
    {
        isVote = true;
        Value = 8;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void Value_Nine()
    {
        isVote = true;
        Value = 9;
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
    }
    public void GameStop()
    {
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
    }
}
