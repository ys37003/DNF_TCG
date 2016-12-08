using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Packet
{
    public int ProtocolNo;
    public List<object> Data = new List<object>();

    public Packet()
    {
        ProtocolNo = 0;
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
            ms.Position = 0;

            return bf.Deserialize(ms);
        }
        catch
        {
            return null;
        }
    }

    #region get/set
    
    public bool GetBool(int no)
    {
        return (bool)Data[no];
    }

    public int GetInt(int no)
    {
        return (int)Data[no];
    }

    public float GetFloat(int no)
    {
        return (float)Data[no];
    }

    public string GetString(int no)
    {
        return (string)Data[no];
    }

    public void SetBool(bool b)
    {
        Data.Add(b);
    }

    public void SetInt(int i)
    {
        Data.Add(i);
    }

    public void SetFloat(float f)
    {
        Data.Add(f);
    }

    public void SetString(string str)
    {
        Data.Add(str);
    }

    #endregion
}