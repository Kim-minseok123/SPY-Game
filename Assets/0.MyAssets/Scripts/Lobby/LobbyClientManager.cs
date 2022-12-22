using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyClientManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BlackPannel blackPannel = BlackPannel.instance;
        StartCoroutine(blackPannel.FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
