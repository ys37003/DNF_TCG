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

    public SpriteRenderer CardFront, CardBack;
    public UIButton Button;
    public Animation ani;

    public bool IsEnemy = false;
    public CardState State = CardState.Deck;
    public CardData data { get; set; }

    public string text;
    public int number;

    void Awake()
    {
        EventDelegate.Add(Button.onClick, onClickCard);

        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        while (data == null)
            yield return null;

        Button.isEnabled = false;
        string str = string.Format("CardList/Act1/Act1_{0}", data.act_no.ToString("000"));
        Texture2D t = (Texture2D)Resources.Load(str);
        CardFront.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector3.one * 0.5f);
        State = CardState.Hand;

        switch (data.card_type)
        {
            case CardType.LevelUp:
                text = "레벨업";
                break;
            case CardType.Skill:
                text = "시전";
                break;
            case CardType.Effect:
                text = "세트";
                break;
            case CardType.Weapon:
                text = "장착";
                break;
            case CardType.Armor:
                text = "장착";
                break;
            case CardType.Monster:
                text = "소환";
                break;
            default:
                break;
        }
    }

    public void Open()
    {
        ani.Play("CardOpen");
    }

    public void Reverse()
    {
        ani.Play("CardReverse");
    }

    public void Move(Transform target)
    {
        transform.parent = target.transform;
        StartCoroutine("Move_");
    }

    IEnumerator Move_()
    {
        if (transform.parent.GetComponent<CardSlot>().Type != CardSlot.SlotType.Hand)
        {
            transform.rotation = Quaternion.EulerRotation(-30, 0, 0);
            TweenRotation.Begin(gameObject, 0.1f, Quaternion.EulerRotation(0,0,0));
        }
        TweenPosition.Begin(gameObject, 0.5f, Vector3.zero);
        TweenScale.Begin(gameObject, 0.5f, Vector3.one);
        yield return new WaitForSeconds(0.5f);
    }

    public virtual void Action()
    {
        if (IsEnemy)
            return;

        int to = 0;
        switch (data.card_type)
        {
            case CardType.LevelUp:
                to = 3;
                break;
            case CardType.Skill:
                to = 5;
                break;
            case CardType.Effect:
                to = 4;
                break;
            case CardType.Weapon:
                to = 6;
                break;
            case CardType.Armor:
                to = 7;
                break;
            case CardType.Monster:
                to = 8;
                break;
            default:
                break;
        }

        GameManager.Instance.Info[0].Move(number, to);

        Packet pck = new Packet();
        pck.SetInt(number);
        pck.SetInt(to);

        Protocol.Move.Send(pck, null);
    }

    /// <summary>
    /// cast, levelup, set, equip...
    /// </summary>
    public virtual CardData Action1()
    {
        return data;
    }

    //public virtual bool Start(CardData data)
    //{
    //    return true;
    //}

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

    private void onClickCard()
    {
        if (IsEnemy)
            return;

        if (GameManager.Instance.phase == GameManager.Phase.Wait)
            return;

        CardController.Instance.Show(this);
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