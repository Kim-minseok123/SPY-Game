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
            if(time > 0) { time -= Time.deltaTime; return; }
            QuestionRandom();
            photonView.RPC("QuestionOpen", RpcTarget.All, index);
            QS = false;
            time = 60;
        } 
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        //게임 시작
        if (PhotonNetwork.IsMasterClient) {
            Hashtable cp;
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) {
                cp = PhotonNetwork.PlayerList[i].CustomProperties;
                cp["IsSpy"] = "0";
                cp["IsVote"] = "0";
                cp["Value"] = "-100";
            }
            int SpyNumber = Random.Range(0, PhotonNetwork.CountOfPlayers);
            Debug.Log(SpyNumber);
            cp = PhotonNetwork.PlayerList[SpyNumber].CustomProperties;
            cp["IsSpy"] = "1";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                cp = PhotonNetwork.PlayerList[i].CustomProperties;
                Debug.Log(cp["IsSpy"].ToString());
                Debug.Log(cp["IsVote"].ToString());
                Debug.Log(cp["Value"].ToString());
            }
            QS = true;
            State = 0;
        }
    }
    public void QuestionRandom() {
        index = Random.Range(0, 5);
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
        Hashtable cp;
        cp = PhotonNetwork.LocalPlayer.CustomProperties;
        if (cp["IsSpy"].ToString().Equals("1")) {
            MainText.text = "당신은 스파이입니다. 신중하게 투표해주세요.";
        }
        else
            MainText.text = Question[index];
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
        if (PhotonNetwork.IsMasterClient) {
            if (State == 0)
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Hashtable cp;
                    cp = PhotonNetwork.PlayerList[i].CustomProperties;
                    if (cp["IsVote"].ToString().Equals("0")) {
                        Debug.Log("아직 투표 덜함");
                        return;
                    }
                }
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Hashtable cp;
                    cp = PhotonNetwork.PlayerList[i].CustomProperties;
                    cp["IsVote"] = "0";
                }
                State = 1;
                Debug.Log("투표 함");
            }
        }
    }
    public void Value_Zero() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "0";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_One() {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "1";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Two()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "2";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Three()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "3";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Four()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "4";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Five()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "5";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Six()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "6";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Seven()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "7";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Eight()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "8";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void Value_Nine()
    {
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        cp["Value"] = "9";
        cp["IsVote"] = "1";
        CurrentValue.text = "현재 선택한 값 : " + cp["Value"].ToString();
    }
    public void GameStop()
    {
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
    }
}
