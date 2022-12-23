using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyClientManager : MonoBehaviour
{
    public GameObject JoinRoomUI;
    public GameObject RoomUI;
    public static LobbyClientManager instanse;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instanse != null) {
            Destroy(gameObject);
            return;
        }
        instanse = this;
    }
    void Start()
    {
        BlackPannel blackPannel = BlackPannel.instance;
        StartCoroutine(blackPannel.FadeOut());
    }

    public void JoinRoomUIOn() {
        JoinRoomUI.SetActive(true);
    }
    public void JoinRoomUIOff() {
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
