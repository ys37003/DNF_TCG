using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : ISocket
{
    private Socket client = null;

    public Socket Socket { get { return client; } }

    public void Close()
    {
        if (client == null)
            return;

        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

    public void Start(int port)
    {
        if (client != null)
            return;

        IPAddress ipAddress = Dns.GetHostAddresses("10.0.1.6")[0];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);

            Debug.Log(string.Format("Socket connected to {0}", client.RemoteEndPoint.ToString()));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}