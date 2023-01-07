﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using System.Linq;
using System;

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
    public GameObject PlayerVoteBtn;
    public TextMeshProUGUI CurrentVote;
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
    private string Value = "-100";
    private bool isVote = false;


    private bool[] PlayerVote;
    private string[] PlayerValues;
    private int[] count;

    private bool isOut = false;
    List<int> OutNumber;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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
        else if (State == 1)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                photonView.RPC("UpdateTime", RpcTarget.All, time);
            }
            else
            {
                State = 2;
                EndVote(PlayerValues);
                time = 10;
            }
        }
        else if (State == 2) {
            if (time > 0) { time -= Time.deltaTime; photonView.RPC("UpdateTime", RpcTarget.All, time); return; }
            photonView.RPC("EndResult", RpcTarget.All);
            State = 0;
            QS = true;
            time = 3;
        }
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        isSpy = false;
        Value = "-100";
        isVote = false;
        isOut = false;
        OutNumber = new List<int>();
        //게임 시작
        if (PhotonNetwork.IsMasterClient) {
            int SpyNumber = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            PlayerVote = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            PlayerValues = new string[PhotonNetwork.CurrentRoom.PlayerCount];
            count = new int[PhotonNetwork.CurrentRoom.PlayerCount];
            photonView.RPC("SetSpy", RpcTarget.All, SpyNumber);
            QS = true;
            time = 3f;
            State = 0;
            Debug.Log(SpyNumber);
        }
    }
    [PunRPC]
    public void SetSpy(int SpyNumber) {
        if (SpyNumber == PlayerID) { isSpy = true; }
    }
    public void QuestionRandom() {
        index = UnityEngine.Random.Range(0, Question.Length);
        for (int i = 0; i < PlayerVote.Length; i++) PlayerVote[i] = false;
        for (int i = 0; i < PlayerValues.Length; i++) PlayerValues[i] = "";
        photonView.RPC("QuestionOpen", RpcTarget.All, index);
    }
    [PunRPC]
    public void QuestionOpen(int index) {
        this.index = index;
        Value = "-100";
        isVote = false;
        CurrentValue.text = "현재 선택한 값 : ";
        for (int j = 0; j < 5; j++) Buttons[j].SetActive(false);
        SelectButtonGroup.SetActive(true);
        MainTextObj.SetActive(true);
        if (isSpy)
        {
            MainText.text = "당신은 스파이입니다. 신중하게 투표해주세요.";
        }
        else
            MainText.text = Question[index];

        if (isOut) { CurrentValue.text = ""; photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, "-1"); return; }
        switch (QuestionType[index])
        {
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
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if (OutNumber.Contains(i)) continue;
                    SelectPlayetName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    SelectPlayerButton[i].SetActive(true);
                }
                break;
        }
    }

    [PunRPC]
    public void SetVote(int index, string Values) {
        PlayerVote[index] = true;
        PlayerValues[index] = Values;
        for (int i = 0; i < PlayerVote.Length; i++) {
            if (PlayerVote[i] != true) { return; }
        }
        State = 1;
        photonView.RPC("EndQuestion", RpcTarget.All, PlayerValues);
    }
    [PunRPC]
    public void EndQuestion(string[] vs) {
        PlayerValue.text = "";
        SelectButtonGroup.SetActive(false);
        PlayerResult.SetActive(true);
        PlayerVoteBtn.SetActive(true);
        MainText.text = Question[index];
        CurrentVote.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PlayerVoteButton[i].SetActive(false);
            if (OutNumber.Contains(i)) continue;
            PlayerValue.text += PhotonNetwork.PlayerList[i].NickName + " : \t" + vs[i] + "\n\n";
            PlayetVoteName[i].text = PhotonNetwork.PlayerList[i].NickName;
            PlayerVoteButton[i].SetActive(true);
            if(PhotonNetwork.IsMasterClient)
                PlayerValues[i] = "";
        }
        if(isOut) { PlayerVoteBtn.SetActive(false); }
    }
    [PunRPC]
    public void PlayerVoteToAnother(int index, string Values) {
        PlayerValues[index] = Values;
    }
    [PunRPC]
    public void UpdateTime(float timer) {
        TimerText.text = "남은시간 : \n"+((int)timer).ToString() + " 초";
    }
    public void EndVote(string[] vs) {
        for (int i = 0; i < count.Length; i++) count[i] = 0;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++) {
            for (int j = 0; j < vs.Length; j++) {
                if (vs[j] == PhotonNetwork.PlayerList[i].NickName) {
                    count[i]++;
                }
            }
        }
        bool isit = false;
        int max = count.Max();
        int indexofMax = Array.IndexOf(count, max);
        for (int i = 0; i < count.Length; i++) { 
            if(count[i] == max && i != indexofMax) isit = true;
        }
        photonView.RPC("Result", RpcTarget.All, count, indexofMax,isit);
    }
    [PunRPC]
    public void Result(int[] c, int indexofMax, bool isit) {
        PlayerValue.text = "";
        PlayerVoteBtn.SetActive(false);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (OutNumber.Contains(i)) continue;
            PlayerValue.text += PhotonNetwork.PlayerList[i].NickName + " : \t" + c[i] + "표\n\n";
            if (PhotonNetwork.IsMasterClient)
                PlayerValues[i] = "";
        }
        if (isit){ 
            PlayerValue.text += "\n\n" + "아무도 탈락하지 않았습니다."; return;
        }
        PlayerValue.text += "\n\n"+PhotonNetwork.PlayerList[indexofMax].NickName + "님 탈락";
        //탈락 이벤트
        if (indexofMax == PlayerID) isOut = true;
        OutNumber.Add(indexofMax);
        
    }
    [PunRPC]
    public void EndResult() {
        PlayerResult.SetActive(false);
        MainTextObj.SetActive(false);
    }
    public void Value_Zero() {
        isVote = true;
        Value = "0";
        CurrentValue.text = "현재 선택한 값 : " + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_One() {
        isVote = true;
        Value = "1";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[0].NickName; }
        Debug.Log(Value);
        CurrentValue.text = "현재 선택한 값 : " + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Two()
    {
        isVote = true;
        Value = "2";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[1].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Three()
    {
        isVote = true;
        Value = "3";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[2].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Four()
    {
        isVote = true;
        Value = "4";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[3].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Five()
    {
        isVote = true;
        Value = "5";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[4].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Six()
    {
        isVote = true;
        Value = "6";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[5].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Seven()
    {
        isVote = true;
        Value = "7";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[6].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Eight()
    {
        isVote = true;
        Value = "8";
        if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[7].NickName; }
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Nine()
    {
        isVote = true;
        Value = "9";
        CurrentValue.text = "현재 선택한 값 : \t" + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_1() {
        Value = PhotonNetwork.PlayerList[0].NickName; 
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);

    }
    public void Vote_2()
    {
        Value = PhotonNetwork.PlayerList[1].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_3()
    {
        Value = PhotonNetwork.PlayerList[2].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_4()
    {
        Value = PhotonNetwork.PlayerList[3].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_5()
    {
        Value = PhotonNetwork.PlayerList[4].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_6()
    {
        Value = PhotonNetwork.PlayerList[5].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_7()
    {
        Value = PhotonNetwork.PlayerList[6].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Vote_8()
    {
        Value = PhotonNetwork.PlayerList[7].NickName;
        CurrentVote.text = "투표한 플레이어 : " + Value.ToString();
        photonView.RPC("PlayerVoteToAnother", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void GameStop()
    {
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
    }
}
