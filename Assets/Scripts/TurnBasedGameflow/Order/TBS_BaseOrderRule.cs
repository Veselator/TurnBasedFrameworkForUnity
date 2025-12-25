using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TBS_BaseOrderRule : MonoBehaviour
{
    public abstract List<int> GetTurnOrder(IReadOnlyCollection<IPlayer> players);
}
