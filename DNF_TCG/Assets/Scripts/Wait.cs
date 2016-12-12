using UnityEngine;
using System.Collections;
using System;

public class Wait : MonoBehaviour
{
    public UILabel LabelWait;
    public UIButton BtnCancel;

    [SerializeField]
    private CreateRoom cr = null;

    void Awake()
    {
        EventDelegate.Add(BtnCancel.onClick, onClickCancel);
    }

    private void onClickCancel()
    {
        NetworkManager.Instance.DestroyRoom();
        StartCoroutine("DestroyRoom");
        gameObject.SetActive(false);
    }

    IEnumerator Loop()
    {
        while(true)
        {
            LabelWait.text = "상대방을 기다리는 중\n.";
            yield return new WaitForSeconds(0.5f);

            LabelWait.text = "상대방을 기다리는 중\n..";
            yield return new WaitForSeconds(0.5f);

            LabelWait.text = "상대방을 기다리는 중\n...";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator DestroyRoom()
    {
        WWW www = new WWW(string.Format("10.0.1.6:8080/room/{0}", cr.Port));

        yield return www;
    }
}