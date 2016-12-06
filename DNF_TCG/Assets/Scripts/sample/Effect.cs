using UnityEngine;
using System.Collections;

public class Effect : Card
{
    public Transform target;

    void Start()
    {
        text = "세트";
    }

    public override void Action()
    {
        UIGrid grid = transform.parent.GetComponent<UIGrid>();
        base.Action();
        Move(target);
        grid.Reposition();
        if (isEnemy)
        {
            Angle();
        }
        else
        {
            Reverse();
        }
    }
}