using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class AsyncObject
{
    public byte[] Buffer;
    public Socket WorkingSocket;

    public AsyncObject(int bufferSize)
    {
        Buffer = new byte[bufferSize];
    }
}

public class Protocol
{
    public static Protocol Test1 = new Protocol(1, "TEST_1");
    public static Protocol Test2 = new Protocol(1, "TEST_2");

    public static Dictionary<int, ResultCallback> SendResultCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReceiveResultCallbackDic = new Dictionary<int, ResultCallback>();

    public Protocol(int no, string name)
    {
        this.no = no;
        this.name = name;
    }

    public delegate void ResultCallback(Packet packet);

    private int no;
    private string name;
    private Packet returnPacket;

    private Socket Socket { get { return NetworkManager.Instance.Socket; } }

    public void Send(Packet packet, ResultCallback callback)
    {
        packet.ProtocolNo = no;
        byte[] buffer = Packet.Serialize(packet);

        AsyncObject ao = new AsyncObject(buffer.Length);
        ao.Buffer = buffer;
        ao.WorkingSocket = Socket;

        SendResultCallbackDic[no] = callback;

        Socket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(sendCallback), ao);
    }

    private void sendCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(resultCallback), ao);

        //int receiveBytes = ao.WorkingSocket.EndReceive(ar);
        //if (receiveBytes > 0)
        //{
        //    TestPacket packet = (TestPacket)Packet.Deserialize(ao.Buffer);
        //    Debug.Log("sendCallback" + packet.hi.ToString());
        //    Debug.Log("sendCallback" + packet.hello);
        //}
    }

    private void resultCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (SendResultCallbackDic[no] == null)
            return;

        SendResultCallbackDic[no](packet);
    }

    public void Receive(Packet packet, ResultCallback callback)
    {
        returnPacket = packet;
        packet.ProtocolNo = no;

        Packet sendPacket = new Packet();
        sendPacket.ProtocolNo = no;
        byte[] buffer = Packet.Serialize(sendPacket);

        AsyncObject ao = new AsyncObject(buffer.Length);
        ao.Buffer = buffer;
        ao.WorkingSocket = Socket;

        ReceiveResultCallbackDic[no] = callback;

        Socket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), ao);
    }

    private void receiveCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Socket handler = ao.WorkingSocket;

        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (SendResultCallbackDic[no] == null)
            return;

        ReceiveResultCallbackDic[no](packet);

        byte[] buffer = Packet.Serialize(returnPacket);

        ao = new AsyncObject(buffer.Length);
        ao.Buffer = buffer;
        ao.WorkingSocket = handler;

        ao.WorkingSocket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(resultCallback), ao);
    }
}