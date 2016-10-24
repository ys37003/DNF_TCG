public class CardData
{
    public readonly int no;
    public readonly int act;
    public readonly int act_no;

    public readonly string name;
    public readonly Job job;
    public readonly int type;
    public readonly int rarity;
    public readonly string contents;

    public CardData(int no, int act, int act_no, string name, Job job, int type, int rarity, string contents)
    {
        this.no = no;
        this.act = act;
        this.act_no = act_no;
        this.name = name;
        this.job = job;
        this.type = type;
        this.rarity = rarity;
        this.contents = contents;
    }

    public static CardData temp = new CardData(0, 0, 0, "123", Job.FIghter, 0, 0, "123");
}

public enum Job
{
    FIghter = 0,
    Gunner = 0 << 1,
    GhostNight = 0 << 2,
    Magician = 0 << 3,
}