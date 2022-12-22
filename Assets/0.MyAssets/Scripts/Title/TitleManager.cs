using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        BlackPannel blackPannel = BlackPannel.instance;
        StartCoroutine(blackPannel.FadeOut());
        StartCoroutine(Next());
    }
    public IEnumerator Next() {
        yield return new WaitForSeconds(2f);
        BlackPannel blackPannel = BlackPannel.instance;
        yield return StartCoroutine(blackPannel.FadeIn());
        blackPannel.NextScene("Lobby");
    }

}
