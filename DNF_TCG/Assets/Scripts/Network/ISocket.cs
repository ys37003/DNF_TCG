using System.Net.Sockets;

public interface ISocket
{
    Socket Socket { get;}

    void Start(string ip, int port);
    void Close();
}