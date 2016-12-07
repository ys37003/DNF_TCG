using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client
{
    private Socket client = null;
    public bool IsReady { get; private set; }

    public void Close()
    {
        if (client == null)
            return;

        client.Shutdown(SocketShutdown.Both);
        client.Close();
        IsReady = false;
    }

    public void Start(int port)
    {
        if (client != null)
            return;

        // Establish the remote endpoint for the socket.
        // The name of the 
        // remote device is "host.contoso.com".
        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = Dns.GetHostAddresses("10.0.1.6")[0]; //ipHostInfo.AddressList[1];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

        // Create a TCP/IP socket.
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // Connect to the remote endpoint.
        client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
        IsReady = false;
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            Debug.Log(string.Format("Socket connected to {0}", client.RemoteEndPoint.ToString()));
            IsReady = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}