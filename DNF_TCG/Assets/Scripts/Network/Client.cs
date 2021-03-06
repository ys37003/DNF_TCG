﻿using UnityEngine;
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

    public void Start(string ip, int port)
    {
        if (client != null)
            return;

        IPAddress ipAddress = Dns.GetHostAddresses(ip)[0];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

        Socket tClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        tClient.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), tClient);
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            client = (Socket)ar.AsyncState;
            client.EndConnect(ar);

            Debug.Log(string.Format("Socket connected to {0}", client.RemoteEndPoint.ToString()));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}