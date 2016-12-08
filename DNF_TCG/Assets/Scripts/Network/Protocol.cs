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
    public static Protocol DeckInit = new Protocol(1, "DECK_INIT");

    public static Dictionary<int, ResultCallback> SendResultCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReceiveResultCallbackDic = new Dictionary<int, ResultCallback>();
    public static Dictionary<int, ResultCallback> ReceiveReturnCallbackDic = new Dictionary<int, ResultCallback>();

    public Protocol(int no, string name)
    {
        this.no = no;
        this.name = name;
    }

    public delegate void ResultCallback(Packet packet);

    private int no;
    private string name;

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
    }

    private void resultCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        if (SendResultCallbackDic[packet.ProtocolNo] == null)
            return;

        SendResultCallbackDic[packet.ProtocolNo](packet);
    }

    public void Receive(ResultCallback receiveCallback, ResultCallback returnCallback)
    {
        AsyncObject ao = new AsyncObject(1024 * 4);
        ao.Buffer = new byte[1024 * 4];
        ao.WorkingSocket = Socket;

        ReceiveResultCallbackDic[no] = receiveCallback;
        ReceiveReturnCallbackDic[no] = returnCallback;

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

        if (ReceiveReturnCallbackDic[packet.ProtocolNo] != null)
            ReceiveReturnCallbackDic[packet.ProtocolNo](packet);

        byte[] buffer = Packet.Serialize(rPacket);
        AsyncObject returnAo = new AsyncObject(buffer.Length);
        returnAo.Buffer = buffer;
        returnAo.WorkingSocket = ao.WorkingSocket;

        returnAo.WorkingSocket.BeginSend(returnAo.Buffer, 0, returnAo.Buffer.Length, SocketFlags.None, null, returnAo);
    }
}