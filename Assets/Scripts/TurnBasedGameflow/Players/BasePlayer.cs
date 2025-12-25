using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayer : IPlayer
{
    // Базовый класс игрока

    protected string name;
    public string Name => name;

    protected int id;
    public int ID => id;

    protected int points;
    public int Points => points;
    public virtual bool IsAI { get; }

    public BasePlayer() : this(0, "Player") { }

    public BasePlayer(int ID, string Name) : this(ID, Name, 0) { }

    public BasePlayer(int ID, string Name, int Points)
    {
        id = ID;
        name = Name;
        points = Points;
    }
}
