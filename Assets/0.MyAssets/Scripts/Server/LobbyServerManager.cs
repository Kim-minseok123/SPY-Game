using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class LobbyServerManager : MonoBehaviourPunCallbacks
{
    public Animator Logo;
    public TextMeshProUGUI StatusText;
    public TMP_InputField NickNameInput;
    public GameObject AfterObject;
    public GameObject BeforeObject;
    public TMP_InputField RoomNameInput;
    public TextMeshProUGUI PlayerList;
    public TextMeshProUGUI RoomInfo;
    public GameObject GameStartBtn;

    int RoomName = 10000000;

    void Awake() { 
        Screen.SetResolution(540, 960, false);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString() + RoomName.ToString();

    public void Connect() { 
        if (NickNameInput.text.Equals("")) { Debug.Log("닉네임을 입력해주세요"); return; } 
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public void Disconnect() { 
        PhotonNetwork.Disconnect();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
   UnityEngine.Application.Quit();
#endif
    }
    public override void OnDisconnected(DisconnectCause cause) => print("서버 연결 끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() {
        print("서버 접속 성공");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        Logo.SetTrigger("Start");
        AfterObject.SetActive(true);
        BeforeObject.SetActive(false);
    }
    public void CreateRoom() {
        RoomName = Random.Range(10000000, 100000000);
        PhotonNetwork.CreateRoom(RoomName.ToString(), new RoomOptions { MaxPlayers = 8 }); 

    }

    public void JoinRoom() {
        PhotonNetwork.JoinRoom(RoomNameInput.text); 
    }

    public void LeaveRoom() { 
        PhotonNetwork.LeaveRoom();
        LobbyClientManager.instanse.RoomUIOff();
        AfterObject.SetActive(true);
    }

    public override void OnCreatedRoom()
    {
        AfterObject.SetActive(false);
        LobbyClientManager.instanse.RoomUIOn();
        print("방만들기완료");
    }

    public override void OnJoinedRoom() {
        AfterObject.SetActive(false);
        LobbyClientManager.instanse.JoinRoomUIOff();
        LobbyClientManager.instanse.RoomUIOn();
        GameStartBtn.SetActive(false);
        Info();
        print("방참가완료"); 
    }
    public override void OnPlayerEnteredRoom(Player newPlayer) => Info();
    public override void OnPlayerLeftRoom(Player otherPlayer) => Info();
    public override void OnCreateRoomFailed(short returnCode, string message) => CreateRoom();
    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            PlayerList.text = "";
            RoomInfo.text = "방 이름 : \n" + PhotonNetwork.CurrentRoom.Name + "\n\n\n\n\n\n\n\n\n"
                + PhotonNetwork.CurrentRoom.PlayerCount + "명 / 최대 "
                + PhotonNetwork.CurrentRoom.MaxPlayers + "명";

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                PlayerList.text += PhotonNetwork.PlayerList[i].NickName + "\n";
            if (PhotonNetwork.IsMasterClient) {
                GameStartBtn.SetActive(true);
            }
        }
    }

    public void GameStartButton() {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 4 || !PhotonNetwork.IsMasterClient) { print("인원이 너무 적습니다.");  return; }
        photonView.RPC("GameStart", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void GameStart() {
        StartCoroutine(GameMapStart());
    }
    
    public IEnumerator GameMapStart()
    {
        BlackPannel blackPannel = BlackPannel.instance;
        yield return StartCoroutine(blackPannel.FadeIn());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(blackPannel.FadeOut());
    }
}
