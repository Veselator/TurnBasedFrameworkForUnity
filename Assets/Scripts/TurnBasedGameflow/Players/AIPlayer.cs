using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : BasePlayer
{
    public override bool IsAI => true;
    public AIPlayer() : base() { }
    public AIPlayer(int ID, string Name) : base(ID, Name) { }
    public AIPlayer(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
