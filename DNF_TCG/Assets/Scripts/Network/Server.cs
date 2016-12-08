using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Server : ISocket
{
    private TcpListener lit_Listener = null;

    private Socket clientSock;
    public Socket Socket { get { return clientSock; } }

    public void Close()
    {
        if (lit_Listener == null)
            return;

        lit_Listener.Stop();
    }

    public void Start(int port)
    {
        if (lit_Listener != null)
            return;

        lit_Listener = new TcpListener(Dns.GetHostAddresses("10.0.1.6")[0], port);
        lit_Listener.Start();
        lit_Listener.BeginAcceptSocket(new AsyncCallback(AcceptCallback), lit_Listener);
    }

    void AcceptCallback(IAsyncResult ar)
    {
        try
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            clientSock = listener.EndAcceptSocket(ar);

            Debug.Log(string.Format("Socket connected to {0}", clientSock.RemoteEndPoint.ToString()));
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}