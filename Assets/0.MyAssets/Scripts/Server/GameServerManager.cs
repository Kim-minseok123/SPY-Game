using System.Collections;
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
    List<int> QuestionIndex;
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
    public GameObject FadePannel;
    private int Timer;
    //0 : 질문 나옴, 1 : 투표
    private int index = 0;
    //플레이어 프로퍼티
    public int State = -1;
    bool QS = false;
    private float time;

    public int PlayerID = -1;

    private bool isSpy = false;
    private int NumberOfSpy;
    private string Value = "-100";
    private bool isVote = false;
    private bool isFade = false;

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
        Question = new string[30];
        QuestionType = new int[30];
        //type 1 : 0~9, type 2: 예 아니오, type 3: 전혀, 때때로, 자주, type 4 :1 ~ 5 ,type 5 : 플레이어
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
        Question[5] = "얼마나 자주 부모님께 진실을 말합니까?";
        QuestionType[5] = 3;
        Question[6] = "당신의 키는 보통인가요?";
        QuestionType[6] = 2;
        Question[7] = "당신은 썸남/녀를 만나기위해서 절친과의 약속을 버릴 수 있나요?";
        QuestionType[7] = 2;
        Question[8] = "고양이를 좋아하시나요?";
        QuestionType[8] = 2;
        Question[9] = "당신은 스시를 얼마나 자주 먹습니까?";
        QuestionType[9] = 4;
        Question[10] = "학교를 땡땡이 쳐본적이 있나요?";
        QuestionType[10] = 2;
        Question[11] = "얼마나 자주 모자를 쓰나요?";
        QuestionType[11] = 3;
        Question[12] = "내가 싸워서 이길거 같은 사람은 누구인가요?";
        QuestionType[12] = 5;
        Question[13] = "당신은 얼마나 평범한 사람인가요?";
        QuestionType[13] = 4;
        Question[14] = "당신이 카지노에서 돈을 쓸 가능성은 어느정도 되나요?";
        QuestionType[14] = 1;
        Question[15] = "가장 터프한 사람은 누구인가요?";
        QuestionType[15] = 5;
        Question[16] = "플러팅을 자주하는 사람은 누구인가요?";
        QuestionType[16] = 5;
        Question[17] = "파인애플 피자에 대한 당신의 의견은 어느정도 입니까?";
        QuestionType[17] = 4;
        Question[18] = "얼마나 자주 핸드폰으로 통화를 하나요?";
        QuestionType[18] = 4;
        Question[19] = "당신은 얼마나 논리적인 사람인가요?";
        QuestionType[19] = 4;
        Question[20] = "여태까지 결혼식에 몇번정도 가봤나요?";
        QuestionType[20] = 1;
        Question[21] = "현대 미술은 과대평가를 받고 계신다고 생각합니까?";
        QuestionType[21] = 2;
        Question[22] = "민트초코에 대한 당신의 의견은 어느정도 입니까?";
        QuestionType[22] = 4;
        Question[23] = "위기상황에 빠졌을 때, 가장 잘 대처할 것 같은 사람은 누구입니까?";
        QuestionType[23] = 5;
        Question[24] = "당신은 얼마나 '배운 사람'인가요?";
        QuestionType[24] = 4;
        Question[25] = "연애를 가장 많이 할 것 같은 사람은 누구입니까?";
        QuestionType[25] = 5;
        Question[26] = "가장 빨리 결혼을 할 것 같은 사람은 누구입니까?";
        QuestionType[26] = 5;
        Question[27] = "얼마나 자주 채소를 먹나요?";
        QuestionType[27] = 3;
        Question[28] = "영화관을 얼마나 즐기십니까?";
        QuestionType[28] = 4;
        Question[29] = "평소에 물을 자주 마시는 편인가요?";
        QuestionType[29] = 4;
    }
    // Update is called once per frame
    void Update()
    {
        //타이머   
        if (State == 0 && QS)
        {
            if (time > 0) {
                if (!isFade) photonView.RPC("GameSetting", RpcTarget.All);
                isFade = true;
                time -= Time.deltaTime;
                return;
            }
            photonView.RPC("SetSpy", RpcTarget.All, NumberOfSpy);
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
        else if (State == 2)
        {
            if (time > 0) { time -= Time.deltaTime; photonView.RPC("UpdateTime", RpcTarget.All, time); return; }
            photonView.RPC("EndResult", RpcTarget.All);
            State = 0;
            QS = true;
            time = 3;
            isFade = false;
        }
        else if (State == 3)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                TimerText.text = "남은시간 : \n" + ((int)time).ToString() + " 초";
                return;
            }
            State = -1;
            isFade = false;
            GameStop();
        }
    }

    public void GameStart() {
        GamePannel.SetActive(true);
        isSpy = false;
        Value = "-100";
        isVote = false;
        isOut = false;
        State = -1;
        QS = false;
        NumberOfSpy = -1;
        OutNumber = new List<int>();
        QuestionIndex = new List<int>();
        //게임 시작
        if (PhotonNetwork.IsMasterClient) {
            int SpyNumber = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            PlayerVote = new bool[PhotonNetwork.CurrentRoom.PlayerCount];
            PlayerValues = new string[PhotonNetwork.CurrentRoom.PlayerCount];
            count = new int[PhotonNetwork.CurrentRoom.PlayerCount];
            NumberOfSpy = SpyNumber;
            QS = true;
            time = 3f;
            State = 0;
            Debug.Log(NumberOfSpy);
        }
    }
    [PunRPC]
    public void SetSpy(int SpyNumber) {
        if (SpyNumber == PlayerID) { isSpy = true; }
        Debug.Log(isSpy);
        NumberOfSpy = SpyNumber;
    }
    public void QuestionRandom() {
        while (true) {
            index = UnityEngine.Random.Range(0, Question.Length);
            if (!QuestionIndex.Contains(index)) { 
                QuestionIndex.Add(index);
                break;
            }
        }
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
        PlayerValue.text = "\n";
        SelectButtonGroup.SetActive(false);
        PlayerResult.SetActive(true);
        PlayerVoteBtn.SetActive(true);
        MainText.text = Question[index];
        CurrentVote.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PlayerVoteButton[i].SetActive(false);
            if (OutNumber.Contains(i)) continue;
            PlayerValue.text += PhotonNetwork.PlayerList[i].NickName + " : \t" + vs[i] + "\n";
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
            PlayerValue.text += PhotonNetwork.PlayerList[i].NickName + " : \t" + c[i] + "표\n";
            if (PhotonNetwork.IsMasterClient)
                PlayerValues[i] = "";
        }
        if (isit){ 
            PlayerValue.text += "\n\n" + "아무도 탈락하지 않았습니다."; return;
        }
        PlayerValue.text += "\n\n"+PhotonNetwork.PlayerList[indexofMax].NickName + "님 탈락\n";
        //탈락 이벤트
        if (indexofMax == PlayerID) isOut = true;
        OutNumber.Add(indexofMax);
        if (NumberOfSpy == indexofMax)
        {
            PlayerValue.text += PhotonNetwork.PlayerList[indexofMax].NickName + "님은 '스파이' 입니다.\n 시민 승리!!";
            State = 3;
            time = 10;
        }
        else {
            PlayerValue.text += PhotonNetwork.PlayerList[indexofMax].NickName + "님은 '스파이'가 아닙니다.";
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 5 &&PhotonNetwork.CurrentRoom.PlayerCount - OutNumber.Count <= 3) {
                PlayerValue.text += "\n 스파이 승리!!";
                State = 3;
                time = 10;
            }
            else if(PhotonNetwork.CurrentRoom.PlayerCount < 5 && PhotonNetwork.CurrentRoom.PlayerCount - OutNumber.Count <= 2)
            {
                PlayerValue.text += "\n 스파이 승리!!";
                State = 3;
                time = 10;
            }
        }
    }
    [PunRPC]
    public void EndResult() {
        PlayerResult.SetActive(false);
        MainTextObj.SetActive(false);
    }
    public void Value_Zero() {
        isVote = true;
        Value = "0";
        if (QuestionType[index] == 2) Value = "아니오";
        else if (QuestionType[index] == 3) Value = "전혀";
        CurrentValue.text = "현재 선택한 값 : " + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_One() {
        isVote = true;
        Value = "1";
        if (QuestionType[index] == 2) Value = "예";
        else if (QuestionType[index] == 3) Value = "때때로";
        else if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[0].NickName; }
        CurrentValue.text = "현재 선택한 값 : " + Value.ToString();
        photonView.RPC("SetVote", RpcTarget.MasterClient, PlayerID, Value);
    }
    public void Value_Two()
    {
        isVote = true;
        Value = "2";
        if(QuestionType[index] == 3) { Value = "자주"; }
        else if (QuestionType[index] == 5) { Value = PhotonNetwork.PlayerList[1].NickName; }
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
        StartCoroutine(GameEnd());  
    }
    public IEnumerator GameEnd()
    {
        BlackPannel blackPannel = BlackPannel.instance;
        yield return StartCoroutine(blackPannel.FadeIn());
        yield return new WaitForSeconds(1f);
        PlayerResult.SetActive(false);
        MainTextObj.SetActive(false);
        GamePannel.SetActive(false);
        LobbyClientManager.instance.RoomUIOn();
        LobbyClientManager.instance.GameStartNoticeOn();
        yield return StartCoroutine(blackPannel.FadeOut());
    }
    [PunRPC]
    public void GameSetting() {
        StartCoroutine(FadeInOut());
    }

    //페이드 인
    public IEnumerator FadeInOut()
    {
        Color c = FadePannel.GetComponent<Image>().color;
        FadePannel.SetActive(true);
        for (float f = 0f; f < 1; f += 0.02f)
        {
            c.a = f;
            FadePannel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
        FadePannel.SetActive(true);
        for (float f = 1f; f > 0; f -= 0.02f)
        {
            c.a = f;
            FadePannel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1);
        FadePannel.SetActive(false);
    }
}
