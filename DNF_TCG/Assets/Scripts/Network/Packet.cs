using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum PacketType
{
    Ready = 0,
    Start = 1,
}

[Serializable]
public class TestPacket : Packet
{
    public int hi;
    public string hello;
}

[Serializable]
public class Packet
{
    public int ProtocolNo;
    public int PacketLength;

    public Packet()
    {
        ProtocolNo = 0;
        PacketLength = 0;
    }

    public static byte[] Serialize(object data)
    {
        try
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, data);

            return ms.ToArray();
        }
        catch
        {
            return null;
        }
    }

    public static object Deserialize(byte[] data)
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            ms.Write(data, 0, data.Length);
            //ms.Position = 0;

            return bf.Deserialize(ms);
        }
        catch
        {
            return null;
        }
    }
}