using UnityEngine;
using System.Collections;

public class LevelUp : Card
{
    public Transform target;

    void Start()
    {
        text = "레벨업";
    }

    public override void Action()
    {
        UIGrid grid = transform.parent.GetComponent<UIGrid>();
        base.Action();
        Move(target);
        grid.Reposition();
    }
}