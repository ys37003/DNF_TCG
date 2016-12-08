using System.Net.Sockets;

public interface ISocket
{
    Socket Socket { get;}

    void Start(int port);
    void Close();
}