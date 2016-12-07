using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum PacketType
{
    Ready = 0,
    Start = 1,
}

public class TestPacket : Packet
{
    public int hi;
    public string hello;
}

public class Packet
{
    public int packet_Type;
    public int packet_Length;

    public Packet()
    {
        packet_Type = 0;
        packet_Length = 0;
    }

    public static byte[] Serialize(object data)
    {
        try
        {
            MemoryStream ms = new MemoryStream(1024 * 4); // packet size will be maximum 4k
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
            MemoryStream ms = new MemoryStream(1024 * 4);
            ms.Write(data, 0, data.Length);

            ms.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(ms);
            ms.Close();
            return obj;
        }
        catch
        {
            return null;
        }
    }
}