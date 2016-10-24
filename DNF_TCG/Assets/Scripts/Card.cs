using UnityEngine;
using System.Collections;
using System;

public class Card : MonoBehaviour
{
    public enum CardState
    {
        Deck,
        Hand,
        Field,
    }

    public UITexture CardFront, CardBack;
    public UIButton Button;

    public CardData data { get; set; }

    void Awake()
    {
        EventDelegate.Add(Button.onClick, onClickCard);
    }

    void Start()
    {
        data = CardData.temp;
    }

    private void onClickCard()
    {
        CardController.Instance.Show(this);
    }

    /// <summary>
    /// cast, levelup, set, equip...
    /// </summary>
    public virtual CardData Action1()
    {
        return data;
    }

    public virtual bool Start(CardData data)
    {
        return true;
    }

    public virtual bool Cast()
    {
        return true;
    }

    public virtual bool Activate()
    {
        return true;
    }

    public virtual bool Finish()
    {
        return true;
    }

    public virtual bool End()
    {
        return true;
    }


    /*
        Hand
        시전
        발동
        세트
        장착
        레벨업

        Field
        발동
    */
}