using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class Server
{
    private TcpListener lit_Listener = null;
    public bool IsReady { get; private set; }

    public void Close()
    {
        if (lit_Listener == null)
            return;

        lit_Listener.Stop();
        IsReady = false;
    }

    public void Start(int port)
    {
        if (lit_Listener != null)
            return;

        lit_Listener = new TcpListener(Dns.GetHostAddresses("10.0.1.6")[0], port);
        lit_Listener.Start();
        lit_Listener.BeginAcceptSocket(new AsyncCallback(AcceptCallback), lit_Listener);
        IsReady = false;
    }

    void AcceptCallback(IAsyncResult ar)
    {
        try
        {
            // Get the listener that handles the client request.
            TcpListener listener = (TcpListener)ar.AsyncState;

            // End the operation and display the received data on the
            //console.
            Socket handler = listener.EndAcceptSocket(ar);

            Debug.Log(string.Format("Socket connected to {0}", handler.RemoteEndPoint.ToString()));
            IsReady = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}