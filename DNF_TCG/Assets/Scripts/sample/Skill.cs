using UnityEngine;
using System.Collections;

public class Skill : Card
{
    public Transform target;

    void Start()
    {
        text = "시전";
    }

    public override void Action()
    {
        UIGrid grid = transform.parent.GetComponent<UIGrid>();
        base.Action();
        Move(target);
        grid.Reposition();
        if (isEnemy)
        {
            Reverse();
        }
        else
        {
            Angle();
        }
    }
}