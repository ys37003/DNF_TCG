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

    [SerializeField]
    private PlayerInfo[] Info = null;

    public enum Phase
    {
        Ready,
        Wait,
        Start,
        Cast,
        Activate,
        Finish,
        End,
    }

    private Phase phase = Phase.Ready;

    void Awake()
    {
        CardDataManager instnace = CardDataManager.Instance;
    }

    void Start()
    {
        if (!NetworkManager.Instance.IsServer)
        {
            List<Card> deck = Info[0].InitDeckData(CardDataManager.Instance.GetDeck(1));
            List<int> cardNoList = new List<int>();
            foreach(Card c in deck)
            {
                cardNoList.Add(c.data.no);
            }

            Packet pack = new Packet();
            pack.SetObj(cardNoList);

            List<CardData> deck2 = CardDataManager.Instance.Deck2;

            Protocol.DECK_INIT.Send(pack, (packet) =>
            {
                List<int> noList = (List<int>)packet.GetObj(0);
                List<CardData> dataList = new List<CardData>();
                foreach(int no in noList)
                {
                    dataList.Add(deck2.Find((card) => { return card.no == no; }));
                }

                Info[1].SetDeckData(dataList);
                Debug.Log("SendResultCallback");
                NextPhase();
            });
        }
        else
        {
            List<Card> deck = Info[0].InitDeckData(CardDataManager.Instance.GetDeck(2));
            List<int> cardNoList = new List<int>();
            foreach (Card c in deck)
            {
                cardNoList.Add(c.data.no);
            }

            List<CardData> deck1 = CardDataManager.Instance.Deck1;

            Protocol.DECK_INIT.Receive((packet) =>
            {
                List<int> noList = (List<int>)packet.GetObj(0);
                List<CardData> dataList = new List<CardData>();
                foreach (int no in noList)
                {
                    dataList.Add(deck1.Find((card) => { return card.no == no; }));
                }

                Info[1].SetDeckData(dataList);
                Debug.Log("ReciveResultCallback");
            }, (packet) =>
            {
                packet.SetObj(cardNoList);
                Debug.Log("ReciveReturnCallback");
            }, (packet) => { NextPhase(); });
        }

        StartCoroutine("Ready");
    }

    public void NextPhase()
    {
        phase++;
    }

    IEnumerator Ready()
    {
        while (phase == Phase.Ready)
            yield return null;

        int result = 0;
        if(NetworkManager.Instance.IsServer)
        {
            Packet pck = new Packet();
            pck.SetInt(5);

            Protocol.DRAW.Send(pck, (packet) =>
            {
                result = packet.GetInt(0);
            });

            phase = Phase.Start;
        }
        else
        {
            Packet pck = new Packet();
            pck.SetInt(6);

            Protocol.DRAW.Send(pck, (packet) =>
            {
                result = packet.GetInt(0);
            });

            phase = Phase.Wait;
        }

        while (result == 0)
            yield return null;

        switch (phase)
        {
            case Phase.Wait:
                Info[0].StartCoroutine(Info[0].IDraw(5));
                Info[1].StartCoroutine(Info[1].IDraw(result));
                yield return Wait();
                break;
            case Phase.Start:
                Info[0].StartCoroutine(Info[0].IDraw(5));
                Info[1].StartCoroutine(Info[1].IDraw(result));
                yield return IStart();
                break;
        }
    }

    IEnumerator Wait()
    {
        yield return IStart();
    }

    IEnumerator IStart()
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
        yield return Wait();
    }
}