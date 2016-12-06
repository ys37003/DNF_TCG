using UnityEngine;
using System.Collections;

public class Weapon : Card
{
    public Transform target;

    void Start()
    {
        text = "장착";
    }

    public override void Action()
    {
        UIGrid grid = transform.parent.GetComponent<UIGrid>();
        base.Action();
        Move(target);
        grid.Reposition();
    }
}
