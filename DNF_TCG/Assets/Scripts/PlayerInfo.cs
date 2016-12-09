using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlotType = CardSlot.SlotType;

public class PlayerInfo : MonoBehaviour
{
    private Dictionary<SlotType, List<Card>> CardDic = new Dictionary<SlotType, List<Card>>();
    private Dictionary<SlotType, CardSlot> CardSlotDic = new Dictionary<SlotType, CardSlot>();

    [SerializeField]
    private GameObject goField = null, goHand = null;

    public bool isEnemy = false;
    public int LifePoint { get; private set; }
    public int DeckSize { get { return CardDic[SlotType.Deck].Count - 1; } }

    void Awake()
    {
        for (SlotType type = SlotType.Hand; type <= SlotType.Monster; ++type)
        {
            CardDic.Add(type, new List<Card>());
        }

        List<CardSlot> slotList = new List<CardSlot>(goField.GetComponentsInChildren<CardSlot>());
        foreach (CardSlot slot in slotList)
        {
            CardSlotDic[slot.Type] = slot;
        }
        CardSlotDic[SlotType.Hand] = goHand.GetComponent<CardSlot>();

        InitDeck(new List<Card>(CardSlotDic[SlotType.Deck].transform.GetComponentsInChildren<Card>()));
    }

    public IEnumerator IDraw(int count)
    {
        yield return new WaitForSeconds(4);

        for (int i = 0; i < count; ++i)
        {
            Draw(1);
            yield return new WaitForSeconds(0.5f);
            CardSlotDic[SlotType.Hand].GetComponent<UIGrid>().Reposition();
        }
    }

    public void InitDeck(List<Card> cardList)
    {
        while (0 != cardList.Count)
        {
            int index = 0;
            cardList[index].IsEnemy = isEnemy;
            CardDic[SlotType.Deck].Add(cardList[index]);
            cardList.RemoveAt(index);
        }
    }

    public List<Card> InitDeckData(List<CardData> cardDataList)
    {
        int i = 0;
        while (0 != cardDataList.Count)
        {
            int index = Random.Range(0, cardDataList.Count - 1);
            Card card = CardDic[SlotType.Deck][i++];
            card.number = index;
            card.data = cardDataList[index];
            cardDataList.RemoveAt(index);
        }

        return CardDic[SlotType.Deck];
    }

    public void SetDeckData(List<CardData> cardDataList)
    {
        int i = 0;
        while (0 != cardDataList.Count)
        {
            int index = 0;
            Card card = CardDic[SlotType.Deck][i++];
            card.number = index;
            card.data = cardDataList[index];
            cardDataList.RemoveAt(index);
        }
    }

    public void Draw(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Card card = CardDic[SlotType.Deck][DeckSize - i];
            CardDic[SlotType.Hand].Add(card);
            card.Move(CardSlotDic[SlotType.Hand].transform);
            card.Button.isEnabled = true;
            if (!isEnemy)
                card.Open();
        }

        CardDic[SlotType.Deck].RemoveRange(DeckSize - count + 1, count);
    }

    public void Move(int number, int to)
    {
        for(SlotType t = SlotType.Hand; t <= SlotType.Monster; ++t)
        {
            foreach (Card c in CardDic[t])
            {
                if (c.number == number)
                {
                    c.Move(CardSlotDic[(SlotType)to].transform);
                    return;
                }
            }
        }
    }
}