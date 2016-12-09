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

    public PlayerInfo[] Info = null;

    public enum Phase
    {
        Ready,
        Start,
        Cast,
        Activate,
        Finish,
        End,
        Wait,
    }

    public List<Step> StepList = new List<Step>();
    public Phase phase { get; private set; }

    void Awake()
    {
        phase = Phase.Ready;
        CardDataManager instnace = CardDataManager.Instance;
    }

    void Start()
    {
        Info[0].isEnemy = false;
        Info[1].isEnemy = true;

        StepList[0].time = 60;
        StepList[1].time = 120;
        StepList[2].time = 120;
        StepList[3].time = 60;
        StepList[4].time = 60;

        EventDelegate.Add(StepList[0].BtnNext.onClick, () => { StopCoroutine("IStart"); StartCoroutine("Cast"); });
        EventDelegate.Add(StepList[1].BtnNext.onClick, () => { StopCoroutine("Cast"); StartCoroutine("Activate"); });
        EventDelegate.Add(StepList[2].BtnNext.onClick, () => { StopCoroutine("Activate"); StartCoroutine("Finish"); });
        EventDelegate.Add(StepList[3].BtnNext.onClick, () => { StopCoroutine("Finish"); StartCoroutine("End"); });
        EventDelegate.Add(StepList[4].BtnNext.onClick, () => { StopCoroutine("End"); NextPhase(); StartCoroutine("Wait"); });

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
                phase++;
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
            }, (packet) => { phase++; });
        }

        StartCoroutine("Ready");
        StartCoroutine("MoveLoop");
    }

    IEnumerator MoveLoop()
    {
        bool Call = true;
        int cardNumber = -1;
        int to = 0;
        while (true)
        {
            if (cardNumber != -1)
            {
                Info[1].Move(cardNumber, to);
            }

            if (Call)
            {
                Call = false;
                Protocol.Move.Receive((packet) =>
                {
                    cardNumber = packet.GetInt(0);
                    to = packet.GetInt(1);
                    Call = true;
                }, null, null);
            }
        }
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
                Info[0].StartCoroutine(Info[0].IDraw(6));
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
        Phase now = Phase.Ready;
        Phase before = now;

        yield return new WaitForSeconds(0.5f);

        Protocol.TURN.Receive((packet) =>
        {
            now = (Phase)packet.GetInt(0);
        }, null, null);

        while (now != Phase.Wait)
        {
            if(before != now)
            {
                if ((int)now - 2 >= 0)
                {
                    StepList[(int)now - 2].onClickNext();
                }
                StartCoroutine(StepList[(int)now - 1].Timer());
                before = now;

                Protocol.TURN.Receive((packet) =>
                {
                    now = (Phase)packet.GetInt(0);
                }, null, null);
            }

            yield return null;
        }

        StepList[(int)Phase.End - 1].onClickNext();
        yield return IStart();
    }

    IEnumerator IStart()
    {
        phase = Phase.Start;
        NextPhase();

        yield return new WaitForSeconds(StepList[0].time);
        StepList[0].onClickNext();
        yield return Cast();
    }

    IEnumerator Cast()
    {
        NextPhase();

        yield return new WaitForSeconds(StepList[1].time);
        StepList[1].onClickNext();
        yield return Activate();
    }

    IEnumerator Activate()
    {
        NextPhase();

        yield return new WaitForSeconds(StepList[2].time);
        StepList[2].onClickNext();
        yield return Finish();
    }

    IEnumerator Finish()
    {
        NextPhase();

        yield return new WaitForSeconds(StepList[3].time);
        StepList[3].onClickNext();
        yield return End();
    }

    IEnumerator End()
    {
        NextPhase();

        yield return new WaitForSeconds(StepList[4].time);
        StepList[4].onClickNext();

        NextPhase();
        yield return Wait();
    }

    private void NextPhase()
    {
        if ((int)phase - 2 >= 0)
        {
            StepList[(int)phase - 2].onClickNext();
        }

        if (phase != Phase.Wait)
        {
            StartCoroutine(StepList[(int)phase - 1].Timer());
        }

        Packet pck = new Packet();
        pck.SetInt((int)phase++);

        Protocol.TURN.Send(pck, null);
    }
}