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

    public CardState State = CardState.Deck;
    public CardData data { get; set; }

    public bool isEnemy;
    public string text;

    void Awake()
    {
        EventDelegate.Add(Button.onClick, onClickCard);
    }

    void Start()
    {
        data = CardData.temp;
    }

    public virtual void Action()
    {
    }

    public void Reverse()
    {
    }

    public void Move(Transform target)
    {
        transform.parent = target.transform;
        StartCoroutine("Move_");
    }

    IEnumerator Move_()
    {
        TweenPosition.Begin(gameObject, 0.5f, Vector3.zero);
        TweenScale.Begin(gameObject, 0.5f, Vector3.one);
        yield return new WaitForSeconds(0.5f);
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

    private void onClickCard()
    {
        CardController.Instance.Show(this);
    }

    public void TestSend()
    {
        TestPacket tp = new TestPacket();
        tp.hi = 5;
        tp.hello = "hello";

        Protocol p = new Protocol();
        p.Send(tp, (packet) =>
        {
            Debug.Log((packet as TestPacket).hi);
            Debug.Log((packet as TestPacket).hello);
        });
    }

    public void TestReceive()
    {
        Protocol p = new Protocol();
        p.Receive(new TestPacket(), (packet) =>
        {
            Debug.Log((packet as TestPacket).hi);
            Debug.Log((packet as TestPacket).hello);
        });
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