using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardDataManager : MonoBehaviour
{
    private static CardDataManager instance;
    public  static CardDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(CardDataManager)) as CardDataManager;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("CardDataManager");
                instance = obj.AddComponent<CardDataManager>() as CardDataManager;
            }

            return instance;
        }
    }

    public List<CardData> Deck1 = new List<CardData>();
    public List<CardData> Deck2 = new List<CardData>();

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        InitDeck1();
        InitDeck2();
    }

    public List<CardData> GetDeck(int no)
    {
        switch (no)
        {
            case 1: return Deck1;
            case 2: return Deck2;
        }

        return null;
    }

    void InitDeck1()
    {
        for (int i = 0; i < 10; ++i) // 10
        {
            Deck1.Add(LevelUpCardData.FIghterLevelUp);
        }

        for (int i = 0; i < 4; ++i) // 16
        {
            Deck1.Add(SkillCardData.FighetAttack1);
            Deck1.Add(SkillCardData.FighetAttack2);
            Deck1.Add(SkillCardData.FighetAttack3);
            Deck1.Add(SkillCardData.NormalAttack0);
        }

        // 4
        Deck1.Add(EquipCardData.FighterWeapon1);
        Deck1.Add(EquipCardData.FighterWeapon2);
        Deck1.Add(EquipCardData.FighterWeapon3);
        Deck1.Add(EquipCardData.FighterWeapon4);

        // 3
        Deck1.Add(EquipCardData.CommonArmor1);
        Deck1.Add(EquipCardData.CommonArmor2);
        Deck1.Add(EquipCardData.CommonArmor3);
    }

    void InitDeck2()
    {
        for (int i = 0; i < 10; ++i) // 10
        {
            Deck1.Add(LevelUpCardData.GunnerLevelUp);
        }

        for (int i = 0; i < 4; ++i) // 16
        {
            Deck1.Add(SkillCardData.GunnerAttack1);
            Deck1.Add(SkillCardData.GunnerAttack2);
            Deck1.Add(SkillCardData.GunnerAttack3);
            Deck1.Add(SkillCardData.NormalAttack0);
        }

        // 4
        Deck1.Add(EquipCardData.GunnerWeapon1);
        Deck1.Add(EquipCardData.GunnerWeapon2);
        Deck1.Add(EquipCardData.GunnerWeapon3);
        Deck1.Add(EquipCardData.GunnerWeapon4);

        // 3
        Deck1.Add(EquipCardData.CommonArmor1);
        Deck1.Add(EquipCardData.CommonArmor2);
        Deck1.Add(EquipCardData.CommonArmor3);
    }
}