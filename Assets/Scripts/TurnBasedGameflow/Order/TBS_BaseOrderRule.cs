using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TBS_BaseOrderRule : MonoBehaviour
{
    public static TBS_BaseOrderRule Instance { get ; private set ; }
    private void Awake()
    {
        Instance = this;
    }
    public abstract List<int> GetTurnOrder(IReadOnlyCollection<IPlayer> players);
}
