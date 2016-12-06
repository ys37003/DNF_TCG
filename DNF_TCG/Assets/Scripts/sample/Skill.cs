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
    }
}