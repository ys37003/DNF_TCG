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
        BtnNext.isEnabled = false;
        EventDelegate.Add(BtnNext.onClick, onClickNext);
    }

    public IEnumerator Timer()
    {
        BtnNext.isEnabled = true;
        TexTimer.gameObject.SetActive(true);
        TexTimer.fillAmount = 1;

        for(int i = 0; i< time; ++i)
        {
            TexTimer.fillAmount -= 1.0f / time;
            yield return new WaitForSeconds(1);
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