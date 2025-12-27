using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TBS_BaseOrderRule : ScriptableObject
{
    public abstract List<int> GetTurnOrder(IReadOnlyCollection<IPlayer> players);
}
