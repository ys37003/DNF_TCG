public class RoomData
{
    public readonly string ip;
    public readonly int port;
    public readonly string name;

    public RoomData(string ip, int port, string name)
    {
        this.ip = ip;
        this.port = port;
        this.name = name;
    }
}