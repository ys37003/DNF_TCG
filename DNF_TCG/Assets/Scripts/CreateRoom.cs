using UnityEngine;
using System.Collections;
using System;
using System.Net;

public class CreateRoom : MonoBehaviour
{
    public UIInput InputRoomName;
    public UIButton btnCreateRoom, BtnClose;

    public string IP { get; set; }
    public int Port { get; set; }

    [SerializeField]
    private GameObject goWait = null;

    void Awake()
    {
        EventDelegate.Add(btnCreateRoom.onClick, onClickCreateRoom);
        EventDelegate.Add(BtnClose.onClick, onClickClose);
    }

    private void onClickCreateRoom()
    {
        string roomName = InputRoomName.value;
        Port = UnityEngine.Random.Range(1000, 9999);

        IPAddress[] address = Dns.GetHostAddresses(Dns.GetHostName());
        for(int i = 0; i<address.Length; ++i)
        {
            if (address[i].ToString().Split('.').Length == 3)
            {
                IP = address[i].ToString();
            }
        }

        NetworkManager.Instance.CreateRoom(IP, UnityEngine.Random.Range(1000, 9999));
        StartCoroutine(ICreateRoom(IP, Port, roomName));
        onClickClose();
        goWait.SetActive(true);
    }

    private void onClickClose()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ICreateRoom(string ip, int port, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("ip", ip);
        form.AddField("port", port);
        form.AddField("name", name);

        WWW www = new WWW("10.0.1.6:8080/room", form);

        yield return www;
    }
}