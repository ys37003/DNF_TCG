using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SlotType = CardSlot.SlotType;

public class PlayerInfo : MonoBehaviour
{
    private Dictionary<SlotType, List<Card>> CardDic = new Dictionary<SlotType, List<Card>>();
    private Dictionary<SlotType, CardSlot> CardSlotDic = new Dictionary<SlotType, CardSlot>();

    public GameObject goField, goHand;

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
    }

    void Start()
    {
        // 임시
        InitDeck(new List<Card>(CardSlotDic[SlotType.Deck].GetComponentsInChildren<Card>()));
        StartCoroutine("DumyDraw");
    }

    IEnumerator DumyDraw()
    {
        yield return new WaitForSeconds(4);

        for (int i = 0; i < 6; ++i)
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
            int index = 0;//Random.Range(0, cardList.Count - 1);
            CardDic[SlotType.Deck].Add(cardList[index]);
            cardList.RemoveAt(index);
        }
    }

    public void Draw(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            Card card = CardDic[SlotType.Deck][DeckSize- i];
            CardDic[SlotType.Hand].Add(card);
            card.Move(CardSlotDic[SlotType.Hand].transform);
            card.Reverse();
        }

        CardDic[SlotType.Deck].RemoveRange(DeckSize - count + 1, count);
    }
}