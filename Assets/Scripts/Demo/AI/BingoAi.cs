public abstract class BingoAi : AIPlayer
{
    // Общий класс ИИ бинго
    protected BingoMap _map;
    protected TBS_Predictor _predictor;
    protected int _humanPlayerId = 0;

    public override void Init(GlobalFlags gf)
    {
        base.Init(gf);
        _map = TBS_BaseMap.Instance as BingoMap;
        _predictor = TBS_Predictor.Instance;
        _humanPlayerId = 1 - ID;
    }

    public override void Init(GlobalFlags gf, TBS_TurnsManager turnsManager)
    {
        base.Init(gf, turnsManager);
        _map = TBS_BaseMap.Instance as BingoMap;
        _predictor = TBS_Predictor.Instance;
        _humanPlayerId = 1 - ID;
    }

    protected void Put(int selectedColumnId)
    {
        _map.AddPiece(ID, selectedColumnId);
        _globalFlags.TriggerOnTurnEnded(_turnsManager.CurrentTurn, ID);
    }

    public BingoAi() : base() { }
    public BingoAi(int ID, string Name) : base(ID, Name) { }
    public BingoAi(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
