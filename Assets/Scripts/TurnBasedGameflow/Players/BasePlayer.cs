public abstract class BasePlayer : IPlayer
{
    // Базовый класс игрока

    protected string name;
    public string Name => name;

    protected int id;
    public int ID => id;

    protected float points;
    public float Points { get => points; set => points = value; }
    public float OverallScore { get; set; }
    public virtual bool IsAI { get; }


    protected GlobalFlags _globalFlags;
    protected TBS_TurnsManager _turnsManager;

    public virtual void Act()
    {
        // Заглушка
        //Debug.Log($"Player {Name} with ID {ID} is acting.");
    }

    public virtual void Init(GlobalFlags gf)
    {
        _globalFlags = gf;
    }

    public virtual void Init(GlobalFlags gf, TBS_TurnsManager turnsManager)
    {
        _globalFlags = gf;
        _turnsManager = turnsManager;
    }

    public BasePlayer() : this(0, "Player") { }

    public BasePlayer(int ID, string Name) : this(ID, Name, 0) { }

    public BasePlayer(int ID, string Name, int Points)
    {
        id = ID;
        name = Name;
        points = Points;
    }
}
