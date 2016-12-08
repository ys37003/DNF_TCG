public class SkillCardData : CardData
{
    public readonly int level;
    public readonly int atk;

    public SkillCardData(int no, int act, int act_no, string name, Job job, CardType card_type, Rarity rarity, string contents,
        int level, int atk) : base(no, act, act_no, name, job, card_type, rarity, contents)
    {
        this.level = level;
        this.atk = atk;
    }

    public static SkillCardData FighetAttack1 = new SkillCardData(2, 1, 2, "해머킥", Job.FIghter, CardType.Skill, Rarity.Common, "", 10, 200);
    public static SkillCardData FighetAttack2 = new SkillCardData(8, 1, 8, "공중밟기", Job.FIghter, CardType.Skill, Rarity.Uncommon, "", 20, 200);
    public static SkillCardData FighetAttack3 = new SkillCardData(18, 1, 18, "그랩캐넌", Job.FIghter, CardType.Skill, Rarity.Common, "", 30, 500);
    public static SkillCardData FighetAttack4 = new SkillCardData(32, 1, 32, "마운트", Job.FIghter, CardType.Skill, Rarity.Common, "", 40, 900);

    public static SkillCardData GunnerAttack1 = new SkillCardData(52, 1, 52, "슬라이딩", Job.Gunner, CardType.Skill, Rarity.Common, "", 10, 300);
    public static SkillCardData GunnerAttack2 = new SkillCardData(56, 1, 56, "공중사격", Job.Gunner, CardType.Skill, Rarity.Common, "", 20, 400);
    public static SkillCardData GunnerAttack3 = new SkillCardData(64, 1, 64, "슈타이어중저격총", Job.Gunner, CardType.Skill, Rarity.Common, "", 30, 500);
    public static SkillCardData GunnerAttack4 = new SkillCardData(79, 1, 79, "웨스턴파이어", Job.Gunner, CardType.Skill, Rarity.Common, "", 40, 500);

    public static SkillCardData NormalAttack1 = new SkillCardData(98, 1, 98, "단련된주먹", Job.Common, CardType.Skill, Rarity.Common, "", 10, 100);
    public static SkillCardData NormalAttack2 = new SkillCardData(99, 1, 99, "약점공격", Job.Common, CardType.Skill, Rarity.Common, "", 20, 200);
    public static SkillCardData NormalAttack3 = new SkillCardData(100, 1, 100, "강철무장의힘", Job.Common, CardType.Skill, Rarity.Common, "", 30, 400);
    public static SkillCardData NormalAttack4 = new SkillCardData(101, 1, 101, "치명공격", Job.Common, CardType.Skill, Rarity.Common, "", 30, 600);
    public static SkillCardData NormalAttack0 = new SkillCardData(102, 1, 102, "기습공격", Job.Common, CardType.Skill, Rarity.Common, "", 0, 300);
}