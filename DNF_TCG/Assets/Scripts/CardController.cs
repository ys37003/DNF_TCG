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

    void Awake()
    {
        btnList = new List<UIButton>(GetComponentsInChildren<UIButton>());
        uiGrid = GetComponent<UIGrid>();

        EventDelegate.Add(btnList[0].onClick, () =>
        {
            card.Action();
            gameObject.SetActive(false);
        });
    }

    public void Show(Card card)
    {
        this.card = card;

        transform.localPosition = UICamera.lastEventPosition - new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        btnList[0].GetComponentInChildren<UILabel>().text = card.text;

        gameObject.SetActive(true);
    }
}