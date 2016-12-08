public class CardData
{
    public readonly int no;
    public readonly int act;
    public readonly int act_no;

    public readonly string name;
    public readonly Job job;
    public readonly CardType card_type;
    public readonly Rarity rarity;
    public readonly string contents;

    public CardData(int no, int act, int act_no, string name, Job job, CardType card_type, Rarity rarity, string contents)
    {
        this.no = no;
        this.act = act;
        this.act_no = act_no;
        this.name = name;
        this.job = job;
        this.card_type = card_type;
        this.rarity = rarity;
        this.contents = contents;
    }
}

public enum Job
{
    Common = 0,
    FIghter = 0 << 1,
    Gunner = 0 << 2,
    GhostNight = 0 << 3,
    Magician = 0 << 4,
}

public enum CardType
{
    LevelUp,
    Skill,
    Effect,
    Weapon,
    Armor,
    Monster,
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Unique,
    Epic,
}