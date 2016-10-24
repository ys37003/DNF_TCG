using UnityEngine;
using System.Collections;

public class CardSlot : MonoBehaviour
{
    public enum SlotType
    {
        Hand,
        Deck,
        Grave,
        LevelUp,
        Effect,
        Skill,
        Weapon,
        Armor,
        Monster,
    }

    public SlotType Type;
}