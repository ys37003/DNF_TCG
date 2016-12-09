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
    public static Protocol DECK_INIT = new Protocol(1, "DECK_INIT");
    public static Protocol DRAW = new Protocol(2, "DRAW");
    public static Protocol TURN = new Protocol(3, "TURN");

    public static Dictionary<int, ResultCallback> SendResultCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReceiveResultCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReceiveReturnCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReturnResultCallbackDic = new Dictionary<int, ResultCallback>();

    public Protocol(int no, string name)
    {
        this.no = no;
        this.name = name;
    }

    public delegate void ResultCallback(Packet packet);

    private int no;
    private string name;

    private Socket Socket { get { return NetworkManager.Instance.Socket; } }

    public void Send(Packet packet, ResultCallback sendCallback)
    {
        packet.ProtocolNo = no;
        byte[] buffer = Packet.Serialize(packet);

        AsyncObject ao = new AsyncObject(buffer.Length);
        ao.Buffer = buffer;
        ao.WorkingSocket = Socket;

        SendResultCallbackDic[no] = sendCallback;

        Socket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallback), ao);
    }

    private void sendCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        ao.WorkingSocket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(sendResultCallback), ao);
    }

    private void sendResultCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (SendResultCallbackDic[packet.ProtocolNo] == null)
            return;

        SendResultCallbackDic[packet.ProtocolNo](packet);
    }

    public void Receive(ResultCallback receiveCallback, ResultCallback returnCallback, ResultCallback resultCallback)
    {
        AsyncObject ao = new AsyncObject(1024 * 4);
        ao.Buffer = new byte[1024 * 4];
        ao.WorkingSocket = Socket;

        ReceiveResultCallbackDic[no] = receiveCallback;
        ReceiveReturnCallbackDic[no] = returnCallback;
        ReturnResultCallbackDic[no] = resultCallback;

        Socket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(this.receiveCallback), ao);
    }

    private void receiveCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (ReceiveResultCallbackDic[packet.ProtocolNo] != null)
            ReceiveResultCallbackDic[packet.ProtocolNo](packet);

        Packet rPacket = new Packet();
        rPacket.ProtocolNo = no;

        if (ReceiveReturnCallbackDic[rPacket.ProtocolNo] != null)
            ReceiveReturnCallbackDic[rPacket.ProtocolNo](rPacket);

        byte[] buffer = Packet.Serialize(rPacket);
        AsyncObject returnAo = new AsyncObject(buffer.Length);
        returnAo.Buffer = buffer;
        returnAo.WorkingSocket = ao.WorkingSocket;

        returnAo.WorkingSocket.BeginSend(returnAo.Buffer, 0, returnAo.Buffer.Length, SocketFlags.None, new AsyncCallback(receiveResultCallback), returnAo);
    }

    private void receiveResultCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (ReturnResultCallbackDic[packet.ProtocolNo] != null)
            ReturnResultCallbackDic[packet.ProtocolNo](packet);
    }
}