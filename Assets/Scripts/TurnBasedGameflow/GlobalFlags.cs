using System.Collections.Generic;
using UnityEngine.Events;

public class GlobalFlags
{
    public class Flag
    {
        public UnityEvent<bool> onFlagChange;
        public FlagType flagType;
        public bool State
        {
            get => state;
            set
            {
                if (state != value)
                {
                    state = value;
                    onFlagChange?.Invoke(state);
                }
            }
        }

        private bool state;

        public Flag(FlagType flagType) : this(flagType, false) { }

        public Flag(FlagType flagType, bool state)
        {
            this.flagType = flagType;
            this.state = state;
            onFlagChange = new UnityEvent<bool>();
        }
    }

    private HashSet<Flag> _activeFlags = new HashSet<Flag>();
    
    public void SetFlag(FlagType flag, bool newState = true)
    {
        foreach (var activeFlag in _activeFlags)
        {
            if (activeFlag.flagType == flag)
            {
                activeFlag.State = newState;
                return;
            }
        }

        _activeFlags.Add(new Flag(flag, newState));
    }

    public Flag GetFlag(FlagType flag)
    {
        foreach (var activeFlag in _activeFlags)
        {
            if (activeFlag.flagType == flag)
                return activeFlag;
        }

        return null;
    }

    public void ToggleFlag(FlagType flag)
    {
        foreach (var activeFlag in _activeFlags)
        {
            if (activeFlag.flagType == flag)
            {
                activeFlag.State = !activeFlag.State;
                return;
            }
        }

        _activeFlags.Add(new Flag(flag, true));
    }

    public void ResetAllFlags()
    {
        _activeFlags.Clear();
    }

    public IReadOnlyCollection<Flag> GetActiveFlags()
    {
        return _activeFlags;
    }
}

public enum FlagType
{
    // General
    PlayerCanMove,
    GameStarted,
    LevelCompleted,
    InputLocked,
    GameOver,
    GameWin,

    // Runner specific
    RunnerStage1Passed,
    RunnerStage2Passed,
    RunnerStage3Passed,
    RunnerIsRotating,
    BlockPlayerMoving,
    CarTurning,

    // ShootEmUp
    ShootEmUpWaveEnded,
    ShootEmUpStartWave,
    ShootEmUpEnemyDied,
    
    // BoxPuzzle
    IsReadyToShowMiniCamera
}