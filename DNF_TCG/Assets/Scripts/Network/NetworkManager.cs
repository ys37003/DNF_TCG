using System.Collections;
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

    private Server server = null;
    private Client client = null;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateRoom(int port)
    {
        // 서버오픈
        server = new Server();
        server.Start(port);
        StartCoroutine("WaitConnect");
    }

    public void DestroyRoom()
    {
        if (server != null)
        {
            server.Close();
        }
    }

    public void JoinRoom(int port)
    {
        // 클라연결
        client = new Client();
        client.Start(port);
        StartCoroutine("WaitConnect");
    }

    IEnumerator WaitConnect()
    {
        while ((server != null && !server.IsReady) ||
               (client != null && !client.IsReady))
        {
            yield return null;
        }

        SceneManager.LoadScene("Play");
    }
}