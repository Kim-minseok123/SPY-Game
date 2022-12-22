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
    
    void Awake()=> Screen.SetResolution(1080, 1920, false);

    void Update() => StatusText.text = PhotonNetwork.NetworkClientState.ToString();

    public void Connect() { 
        if (NickNameInput.text.Equals("")) { Debug.Log("닉네임을 입력해주세요"); return; } 
        PhotonNetwork.ConnectUsingSettings(); 
    }

    public override void OnConnectedToMaster() {
        print("서버 접속 성공");
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        Logo.SetTrigger("Start");
        AfterObject.SetActive(true);
        BeforeObject.SetActive(false);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause) => print("서버 연결 끊김");

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => print("로비 접속 성공");
    //public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });

    //public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);

    //public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => print("방만들기완료");

    public override void OnJoinedRoom() => print("방참가완료");

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방만들기실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방참가실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("방랜덤참가실패");

    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 있는지? : " + PhotonNetwork.InLobby);
            print("연결됐는지? : " + PhotonNetwork.IsConnected);
        }
    }
}
