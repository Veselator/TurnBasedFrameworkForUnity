public abstract class BingoAi : AIPlayer
{
    // Общий класс ИИ бинго
    protected BingoMap _map;

    public override void Init(GlobalFlags gf)
    {
        base.Init(gf);
        _map = TBS_BaseMap.Instance as BingoMap;
    }

    public override void Init(GlobalFlags gf, TBS_TurnsManager turnsManager)
    {
        base.Init(gf, turnsManager);
        _map = TBS_BaseMap.Instance as BingoMap;
    }

    public BingoAi() : base() { }
    public BingoAi(int ID, string Name) : base(ID, Name) { }
    public BingoAi(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
