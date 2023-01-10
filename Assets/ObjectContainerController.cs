
#region Match
using System;
using System.Collections.Generic;

[Serializable]
public class MatchStatus
{
    public bool HotGame;
    public int ID;
    public string Time;
    public string TeamA;
    public string TeamB;
    public int Type;
}
#endregion

#region MatchPools
[Serializable]
public class MatchPools
{
    public int MatchID;
    public string PoolCount;
    public Dictionary<string,Pools> Pools = new();
}
[Serializable]
public class Pools
{
    public int Entry;
    public int PoolID;
    public List<Prizevalues> PrizeList = new();
    public int PrizePool;
    public int SlotsFilled;
    public int TotalSlots;
    public string Type;
}
[Serializable]
public class Prizevalues
{
    public string Rank;
    public int Value;
}

#endregion

#region Players
public class Player
{
    public Dictionary<string,PlayerDetailsVal> Players;
    public string TeamName;
}
[Serializable]
public class PlayerDetailsVal
{
    public int FPoint;
    public string ID;
    public string Name;
    public int Type;
}
#endregion