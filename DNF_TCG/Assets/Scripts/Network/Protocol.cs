using System;
using System.Net.Sockets;

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
    private Socket Socket { get { return NetworkManager.Instance.Socket; } }

    public delegate void ResultCallback(Packet packet);
    private ResultCallback resultCallback;

    public void Send(Packet packet, ResultCallback callback)
    {
        byte[] buffer = new byte[1024 * 4];
        Packet.Serialize(packet).CopyTo(buffer,0);
        AsyncObject ao = new AsyncObject(buffer.Length);
        ao.Buffer = buffer;
        ao.WorkingSocket = Socket;
        resultCallback = callback;

        Socket.BeginSend(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(sendCallback), ao);
    }

    private void sendCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;
        Socket handler = ao.WorkingSocket;

        int receiveBytes = ao.WorkingSocket.EndReceive(ar);

        if(receiveBytes > 0)
        {

        }

        handler.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), ao);
    }

    private void receiveCallback(IAsyncResult ar)
    {
        AsyncObject ao = (AsyncObject)ar.AsyncState;

        Packet packet = (Packet)Packet.Deserialize(ao.Buffer);

        resultCallback(packet);
    }

    public void Receive(Packet packet, ResultCallback callback)
    {
        byte[] buffer = Packet.Serialize(packet);
        AsyncObject ao = new AsyncObject(buffer.Length);
        resultCallback = callback;
        Socket.BeginReceive(ao.Buffer, 0, ao.Buffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), ao);
    }
}