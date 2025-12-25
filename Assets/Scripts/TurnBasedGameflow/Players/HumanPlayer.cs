using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : BasePlayer
{
    public override bool IsAI => false;

    public HumanPlayer() : base() { }
    public HumanPlayer(int ID, string Name) : base(ID, Name) { }
    public HumanPlayer(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
