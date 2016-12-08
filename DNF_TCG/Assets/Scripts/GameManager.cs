using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public  static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                instance = obj.AddComponent<GameManager>() as GameManager;
            }

            return instance;
        }
    }

    private PlayerInfo[] Info = new PlayerInfo[2];

    public enum Phase
    {
        Start,
        Cast,
        Activate,
        Finish,
        End,
    }

    private Phase phase;

    void Awake()
    {
        if(NetworkManager.Instance.IsServer)
        {
            Packet pack = new Packet();
            pack.SetInt(1);
            pack.SetInt(2);

            Protocol.DeckInit.Send(pack, (packet) =>
            {
                Debug.Log("SendResultCallback");
                Info[0].InitDeckData(CardDataManager.Instance.GetDeck(1));
                Info[1].InitDeckData(CardDataManager.Instance.GetDeck(2));
            });
        }
        else
        {
            Protocol.DeckInit.Receive((packet) =>
            {
                Debug.Log("ReciveResultCallback");
                Info[0].InitDeckData(CardDataManager.Instance.GetDeck(packet.GetInt(1)));
                Info[1].InitDeckData(CardDataManager.Instance.GetDeck(packet.GetInt(0)));
            }, (packet) =>
            {
                Debug.Log("ReciveReturnCallback");
            });
        }
    }

    public void NextPhase()
    {
        phase++;
    }

    IEnumerator Start()
    {
        yield return Cast();
    }

    IEnumerator Cast()
    {
        yield return Activate();
    }

    IEnumerator Activate()
    {
        yield return Finish();
    }

    IEnumerator Finish()
    {
        yield return End();
    }

    IEnumerator End()
    {
        yield return Start();
    }
}