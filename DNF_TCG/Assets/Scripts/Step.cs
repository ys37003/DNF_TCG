using UnityEngine;
using System.Collections;
using System;

public class Step : MonoBehaviour
{
    public GameManager.Phase phase;
    public UIButton BtnNext;
    public UITexture TexTimer;
    public int time;

    void Awake()
    {
        EventDelegate.Add(BtnNext.onClick, onClickNext);
    }

    public IEnumerator Timer()
    {
        BtnNext.isEnabled = true;
        TexTimer.gameObject.SetActive(true);
        TexTimer.fillAmount = 1;

        float startTime = Time.time;
        float playTime = startTime;
        while (Time.time < time + startTime)
        {
            playTime = Time.time;
            TexTimer.fillAmount -= (Time.time - playTime) / time;
            yield return null;
        }

        BtnNext.isEnabled = false;
        TexTimer.gameObject.SetActive(false);
    }

    public void onClickNext()
    {
        BtnNext.isEnabled = false;
        TexTimer.gameObject.SetActive(false);
        StopCoroutine("TImer");
    }
}