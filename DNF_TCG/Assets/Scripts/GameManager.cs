using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        CardDataManager instnace = CardDataManager.Instance;
    }

    void Start()
    {
        if (!NetworkManager.Instance.IsServer)
        {
            List<Card> deck = Info[0].InitDeckData(CardDataManager.Instance.GetDeck(1));

            Packet pack = new Packet();
            pack.SetObj(deck);

            Protocol.DeckInit.Send(pack, (packet) =>
            {
                Info[1].InitDeckData((List<CardData>)packet.GetObj(0));
                Debug.Log("SendResultCallback");
            });
        }
        else
        {
            List<Card> deck = Info[1].InitDeckData(CardDataManager.Instance.GetDeck(2));

            Protocol.DeckInit.Receive((packet) =>
            {
                Info[1].InitDeckData((List<CardData>)packet.GetObj(0));
                Debug.Log("ReciveResultCallback");
            }, (packet) =>
            {
                packet.SetObj(deck);
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