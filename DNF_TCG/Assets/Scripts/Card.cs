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
        StartCoroutine("Reverse_");
    }

    public void Move(Transform target)
    {
        transform.parent = target.transform;
        StartCoroutine("Move_");
    }

    public void Angle()
    {
        StartCoroutine("Angle_");
    }

    bool isReverse = false;
    IEnumerator Reverse_()
    {
        if (isReverse == true)
            yield break;

        isReverse = true;

        TweenRotation.Begin(gameObject, 0.5f, Quaternion.Euler(-0.1f, transform.eulerAngles.y + 180, 0));
        yield return new WaitForSeconds(0.25f);

        CardBack.depth *= -1;
        yield return new WaitForSeconds(0.25f);

        isReverse = false;
    }

    bool isMove = false;
    IEnumerator Move_()
    {
        if (isMove == true)
            yield break;

        isMove = true;

        TweenPosition.Begin(gameObject, 0.5f, Vector3.zero);
        TweenScale.Begin(gameObject, 0.5f, Vector3.one);
        yield return new WaitForSeconds(0.5f);

        isMove = false;
    }

    IEnumerator Angle_()
    {
        if (isReverse == true)
            yield break;

        isReverse = true;

        TweenRotation.Begin(gameObject, 0.5f, Quaternion.Euler(-0.1f, transform.eulerAngles.y, 0));
        yield return new WaitForSeconds(0.25f);

        isReverse = false;
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
        if(isEnemy)
        {
            Action();
        }
        else
        {
            CardController.Instance.Show(this);
        }
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