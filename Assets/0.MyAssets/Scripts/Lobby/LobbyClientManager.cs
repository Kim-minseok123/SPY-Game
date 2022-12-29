using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LobbyClientManager : MonoBehaviour
{
    public GameObject JoinRoomUI;
    public GameObject RoomUI;
    public TMP_InputField Roomname;
    public static LobbyClientManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        BlackPannel blackPannel = BlackPannel.instance;
        StartCoroutine(blackPannel.FadeOut());
    }

    public void JoinRoomUIOn() {
        Roomname.text = "";
        JoinRoomUI.SetActive(true);
    }
    public void JoinRoomUIOff() {
        Roomname.text = "";
        JoinRoomUI.SetActive(false);
    }
    public void RoomUIOn()
    {
        RoomUI.SetActive(true);
    }
    public void RoomUIOff()
    {
        RoomUI.SetActive(false);
    }
}
