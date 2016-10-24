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

    void Awake()
    {
        btnList = new List<UIButton>(GetComponentsInChildren<UIButton>());
        uiGrid = GetComponent<UIGrid>();
    }

    public void Show(Card card)
    {
        transform.position = UICamera.lastWorldPosition;
        transform.localPosition = new Vector3(transform.localPosition.x - 3, transform.localPosition.y + 3, 0);

        gameObject.SetActive(true);
    }
}