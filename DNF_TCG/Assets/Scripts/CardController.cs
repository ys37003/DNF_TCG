using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardController : MonoBehaviour
{
    private static CardController instance;
    public  static CardController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(CardController)) as CardController;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("CardController");
                instance = obj.AddComponent<CardController>() as CardController;
            }

            return instance;
        }
    }

    private List<UIButton> btnList;
    private UIGrid uiGrid;

    private Card card;

    //임시
    public Transform tf;

    void Awake()
    {
        btnList = new List<UIButton>(GetComponentsInChildren<UIButton>());
        uiGrid = GetComponent<UIGrid>();

        EventDelegate.Add(btnList[0].onClick, () =>
        {
            card.Move(tf);
            card.Reverse();
            gameObject.SetActive(false);
        });
    }

    public void Show(Card card)
    {
        this.card = card;

        transform.position = UICamera.lastWorldPosition;
        transform.localPosition = new Vector3(transform.localPosition.x + 100, transform.localPosition.y - 150, 0);

        gameObject.SetActive(true);
    }
}