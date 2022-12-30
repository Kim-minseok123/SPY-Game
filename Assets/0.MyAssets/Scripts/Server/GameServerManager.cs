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
    public GameObject[] Button;
    public GameObject[] SelectPlayerButton;
    public TextMeshProUGUI[] SelectPlayetName;
    //현재 누른 값
    public TextMeshProUGUI CurrentValue;
    //플레이어 값
    public TextMeshProUGUI PlayerValue;
    public GameObject[] PlayerVoteButton;
    public TextMeshProUGUI[] PlayetVoteName;
    //타이머
    public TextMeshProUGUI TimerText;
    private int Timer;
    //플레이어 프로퍼티
    
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
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        //게임 시작
        if (PhotonNetwork.IsMasterClient) {
            Hashtable cp;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                cp = PhotonNetwork.PlayerList[i].CustomProperties;
                cp["IsSpy"] = false;
                cp["IsVote"] = false;
                cp["Value"] = -100;
            }
            int SpyNumber = Random.Range(0, PhotonNetwork.CountOfPlayers);
            cp = PhotonNetwork.PlayerList[SpyNumber].CustomProperties;
            cp["IsSpy"] = true;
        }
    }
    
    public void GameStop() {
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
    }
    public void Value_Zero() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 0;
        cp["IsVote"] = true;
        CurrentValue.text = "0";
    }
    public void Value_One() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 1;
        cp["IsVote"] = true;
        CurrentValue.text = "1";
    }
    public void Value_Two()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 2;
        cp["IsVote"] = true;
        CurrentValue.text = "2";
    }
    public void Value_Three()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 3;
        cp["IsVote"] = true;
        CurrentValue.text = "3";
    }
    public void Value_Four()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 4;
        cp["IsVote"] = true;
        CurrentValue.text = "4";
    }
    public void Value_Five()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 5;
        cp["IsVote"] = true;
        CurrentValue.text = "5";
    }
    public void Value_Six()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 6;
        cp["IsVote"] = true;
        CurrentValue.text = "6";
    }
    public void Value_Seven()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 7;
        cp["IsVote"] = true;
        CurrentValue.text = "7";
    }
    public void Value_Eight()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 8;
        cp["IsVote"] = true;
        CurrentValue.text = "8";
    }
    public void Value_Nine()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = 9;
        cp["IsVote"] = true;
        CurrentValue.text = "9";
    }
}
