using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using LitJson;
using System.Collections.Generic;

public class Robby : MonoBehaviour
{
    public UIButton BtnShop, BtnEdit, BtnCreateRoom, BtnRefresh, BtnExit;

    [SerializeField]
    private GameObject goCreateRoom;

    [SerializeField]
    private List<Room> roomList = new List<Room>();

    private List<RoomData> roomDataList = new List<RoomData>();

    void Awake()
    {
        EventDelegate.Add(BtnRefresh.onClick, onClickRefresh);
        EventDelegate.Add(BtnCreateRoom.onClick, onClickCreateRoom);
        EventDelegate.Add(BtnExit.onClick, onClickExit);
    }

    private void onClickRefresh()
    {
        StartCoroutine("GetRoomList");
    }

    private void onClickCreateRoom()
    {
        gameObject.SetActive(true);
    }

    private void onClickExit()
    {
        SceneManager.LoadScene("Title");
    }

    IEnumerator GetRoomList()
    {
        WWW www = new WWW("10.0.1.6:8080/room");

        yield return www;

        JsonData data = JsonMapper.ToObject(www.text);

        roomDataList.Clear();

        for (int i = 0; i < data.Count; ++i)
        {
            RoomData rd = new RoomData
            (
                data[i]["ip"].ToString(),
                (int)data[i]["port"],
                data[i]["name"].ToString()
            );

            roomDataList.Add(rd);
        }

        for (int i = 0; i < roomList.Count; ++i)
        {
            if (i < roomDataList.Count)
            {
                roomList[i].SetData(roomDataList[i]);
                roomList[i].SetActive(true);
            }
            else
            {
                roomList[i].SetData(null);
                roomList[i].SetActive(false);
            }
        }
    }
}