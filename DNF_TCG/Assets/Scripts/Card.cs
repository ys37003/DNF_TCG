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

    void Awake()
    {
        EventDelegate.Add(Button.onClick, onClickCard);

        StartCoroutine("Init");
    }

    IEnumerator Init()
    {
        while (data == null)
            yield return null;

        string str = string.Format("CardList/Act1/Act1_{0}", data.act_no.ToString("000"));
        Texture2D t = (Texture2D)Resources.Load(str);
        CardFront.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector3.one * 0.5f);
    }

    public void InitCard()
    {
    }

    public virtual void Action()
    {
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
        CardController.Instance.Show(this);
        //TestReceive();
    }

    public void TestSend()
    {
        //Packet pck = new Packet();
        //pck.SetString("testString");
        //pck.SetFloat(1.4f);

        //Protocol.Test1.Send(new Packet(), (packet) =>
        //{
        //    Debug.Log("receiveResultCallback No.0, " + packet.GetString(0));
        //    Debug.Log("receiveResultCallback No.1, " + packet.GetFloat(1));
        //});
    }

    public void TestReceive()
    {
        //Protocol.Test2.Receive((packet) =>
        //{
        //    Debug.Log("receiveResultCallback No.0, " + packet.GetString(0));
        //    Debug.Log("receiveResultCallback No.1, " + packet.GetFloat(1));
        //});
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