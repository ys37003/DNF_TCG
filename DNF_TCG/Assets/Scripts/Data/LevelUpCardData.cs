public class LevelUpCardData : CardData
{
    public readonly int level;

    public LevelUpCardData(int no, int act, int act_no, string name, Job job, CardType card_type, Rarity rarity, string contents,
        int level) : base(no, act, act_no, name, job, card_type, rarity, contents)
    {
        this.level = level;
    }

    public static LevelUpCardData FIghterLevelUp = new LevelUpCardData(49, 1, 49, "레벨업", Job.FIghter, CardType.LevelUp, Rarity.Common, "", 10);
    public static LevelUpCardData GunnerLevelUp = new LevelUpCardData(97, 1, 97, "레벨업", Job.FIghter, CardType.LevelUp, Rarity.Common, "", 10);
}
