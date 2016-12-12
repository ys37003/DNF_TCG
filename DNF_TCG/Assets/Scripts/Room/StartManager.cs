using UnityEngine;

public class StartManager : MonoBehaviour
{
    public UIInput InputIP, InputRoomNumber;
    public UIButton BtnCreateRoom, BtnJoinRoom, BtnExit;

    private UILabel labelCreateRoom;

    void Awake()
    {
        labelCreateRoom = BtnCreateRoom.GetComponentInChildren<UILabel>();

        EventDelegate.Add(BtnCreateRoom.onClick, onClickCreateRoom);
        EventDelegate.Add(BtnJoinRoom.onClick, onClickJoinRoom);
        EventDelegate.Add(BtnExit.onClick, onClickExit);
    }

    bool isWait = false;
    private void onClickCreateRoom()
    {
        if(isWait == false && InputRoomNumber.value != "")
        {
            isWait = true;
            BtnJoinRoom.isEnabled = false;
            labelCreateRoom.text = "대기중/취소";
            NetworkManager.Instance.CreateRoom(InputIP.value, int.Parse(InputRoomNumber.value));
        }
        else
        {
            isWait = false;
            BtnJoinRoom.isEnabled = false;
            labelCreateRoom.text = "방만들기";
            NetworkManager.Instance.DestroyRoom();
        }
    }

    private void onClickJoinRoom()
    {
        if(InputRoomNumber.value != "")
        {
            NetworkManager.Instance.JoinRoom(InputIP.value, int.Parse(InputRoomNumber.value));
        }
    }

    private void onClickExit()
    {
        Application.Quit();
    }
}