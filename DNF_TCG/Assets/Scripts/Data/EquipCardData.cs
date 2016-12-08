public class EquipCardData : CardData
{
    public readonly int level;
    public readonly int atk;
    public readonly EquipType equip_type;

    public EquipCardData(int no, int act, int act_no, string name, Job job, CardType card_type, Rarity rarity, string contents,
        int level, int atk, EquipType equip_type) : base(no, act, act_no, name, job, card_type, rarity, contents)
    {
        this.level = level;
        this.atk = atk;
        this.equip_type = equip_type;
    }

    public static EquipCardData FighterWeapon1 = new EquipCardData(45, 1, 45, "청동통파", Job.FIghter, CardType.Weapon, Rarity.Common, "", 10, 200, EquipType.Weapon);
    public static EquipCardData FighterWeapon2 = new EquipCardData(46, 1, 46, "주문식통파", Job.FIghter, CardType.Weapon, Rarity.Common, "", 20, 200, EquipType.Weapon);
    public static EquipCardData FighterWeapon3 = new EquipCardData(47, 1, 47, "더블히터", Job.FIghter, CardType.Weapon, Rarity.Common, "", 30, 300, EquipType.Weapon);
    public static EquipCardData FighterWeapon4 = new EquipCardData(48, 1, 48, "페이옌", Job.FIghter, CardType.Weapon, Rarity.Common, "", 40, 300, EquipType.Weapon);

    public static EquipCardData GunnerWeapon1 = new EquipCardData(93, 1, 93, "포켓리볼버", Job.Gunner, CardType.Weapon, Rarity.Common, "", 10, 200, EquipType.Weapon);
    public static EquipCardData GunnerWeapon2 = new EquipCardData(94, 1, 94, "노스피스리볼버", Job.Gunner, CardType.Weapon, Rarity.Common, "", 20, 300, EquipType.Weapon);
    public static EquipCardData GunnerWeapon3 = new EquipCardData(95, 1, 95, "이튼리볼버", Job.Gunner, CardType.Weapon, Rarity.Common, "", 30, 400, EquipType.Weapon);
    public static EquipCardData GunnerWeapon4 = new EquipCardData(96, 1, 96, "제스가텐", Job.Gunner, CardType.Weapon, Rarity.Common, "", 40, 400, EquipType.Weapon);

    public static EquipCardData CommonArmor1 = new EquipCardData(115, 1, 115, "스야흉갑", Job.Common, CardType.Armor, Rarity.Common, "", 10, 200, EquipType.Armor);
    public static EquipCardData CommonArmor2 = new EquipCardData(116, 1, 116, "스야메일", Job.Common, CardType.Armor, Rarity.Common, "", 20, 300, EquipType.Armor);
    public static EquipCardData CommonArmor3 = new EquipCardData(117, 1, 117, "본아머", Job.Common, CardType.Armor, Rarity.Common, "", 30, 400, EquipType.Armor);
}

public enum EquipType
{
    Weapon,
    Armor,
}