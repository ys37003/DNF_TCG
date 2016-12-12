using UnityEngine;
using System.Collections;
using System;

public class Room : MonoBehaviour
{
    public UILabel LabelRoomName;
    public UIButton BtnJoin;

    public RoomData RoomData { get; set; }

    void Awake()
    {
        EventDelegate.Add(BtnJoin.onClick, onClickJoin);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetData(RoomData data)
    {
        LabelRoomName.text = data.name;
    }

    private void onClickJoin()
    {
        NetworkManager.Instance.JoinRoom(RoomData.ip, RoomData.port);
    }
}