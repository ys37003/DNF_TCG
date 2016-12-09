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
            });
        }
    }

    public void NextPhase()
    {
        phase++;
    }

    //IEnumerator Start()
    //{
    //    yield return Cast();
    //}

    //IEnumerator Cast()
    //{
    //    yield return Activate();
    //}

    //IEnumerator Activate()
    //{
    //    yield return Finish();
    //}

    //IEnumerator Finish()
    //{
    //    yield return End();
    //}

    //IEnumerator End()
    //{
    //    yield return Start();
    //}
}