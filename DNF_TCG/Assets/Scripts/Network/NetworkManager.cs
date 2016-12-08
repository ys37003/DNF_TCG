using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager instance;
    public  static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(NetworkManager)) as NetworkManager;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("NetworkManager");
                instance = obj.AddComponent<NetworkManager>() as NetworkManager;
            }

            return instance;
        }
    }

    private ISocket socket = null;
    public Socket Socket
    {
        get
        {
            if (socket != null)
                return socket.Socket;

            return null;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine("ChangeScene");
    }

    public void CreateRoom(int port)
    {
        // 서버오픈
        socket = new Server();
        socket.Start(port);
    }

    public void DestroyRoom()
    {
        if (socket != null)
            socket.Close();
    }

    public void JoinRoom(int port)
    {
        // 클라연결
        socket = new Client();
        socket.Start(port);
    }

    IEnumerator ChangeScene()
    {
        while (Socket == null)
            yield return null;

        SceneManager.LoadScene("Play");
    }
}