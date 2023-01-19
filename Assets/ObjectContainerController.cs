using System;
using System.Collections.Generic;
using UnityEngine;

#region Match


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
    public Dictionary<string, Pools> Pools = new();
}
[Serializable]
public class Pools
{
    public int Entry;
    public int PoolID;
    public Dictionary<string,Prizevalues> PrizeList = new();
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
    public Dictionary<string, PlayerDetailsVal> Players;
    public string TeamName;
}
[Serializable]
public class PlayerDetailsVal
{
    public int FPoint;
    public string ID;
    public string Name;
    public int Type;
    public string URL;
}
#endregion

#region SettingTeamToRealDb

[Serializable]
public class SelectedTeam
{
    public string TeamID;
    public SelectedPlayers Players;
}

[Serializable]
public class SelectedPlayers
{
    public string Captain;
    public string ViceCaptian;
    public SelectedTeamPlayers TeamA;
    public SelectedTeamPlayers TeamB;
}

[Serializable]
public class SelectedTeamPlayers
{
    public string TeamName;
    public List<string> players = new List<string>();
}

[Serializable]
public class SelectedPoolID
{
    public string PoolID;
    public string TeamID;
}

[Serializable]
public class MyMatchDetails
{
    public Dictionary<string, SelectedPoolID> SelectedPools = new Dictionary<string, SelectedPoolID>();
    public Dictionary<string, SelectedTeam> SelectedTeam = new Dictionary<string, SelectedTeam>();
}
#endregion

#region PlayerSelectionForMatch
[Serializable]
public class PlayerSelectedForMatch
{
    public string playerName;
    public string PlayerID;
    public string points;
    public string countryName;
    public int type;
    public bool isCaptain;
    public bool isViceCaptain;
    public Sprite playerPic;
}

#endregion

#region Team

[Serializable]
public class Team
{
    public string LogoURL;
    public TeamType PlayerDetails = new();
    public string TeamName;
}

[Serializable]
public class TeamType
{
    public string T20;
    public string ODI;
    public string TEST;
}

#endregion

#region PlayerSelectedMatchDetails

public class MatchID
{
    public Dictionary<string, SelectdPoolID> SelectedPools = new();
    public Dictionary<string, SelectedTeamID> SelectedTeam = new();

}

public class SelectdPoolID
{ 
    public string PoolID;
    public string TeamID;
}

public class SelectedTeamID 
{
    public PlayersSelected Players;
}

public class PlayersSelected
{
    public string Captain;
    public TeamSelected TeamA;
    public TeamSelected TeamB;
    public string ViceCaptian;
}

public class TeamSelected
{
    public string TeamName;
    public List<string> players = new();
}
#endregion

#region ScoreCard

public class LiveMatchScoreCard
{
    public Dictionary<string, InningsDetails> MatchDetails = new();
    public string TeamA;
    public string TeamB;
    public string TossWon;
}

public class InningsDetails 
{ 
    public ScoreLive Batting = new();
    public Dictionary<string, TeamDeatilsBowling> Bowling = new();   
    public int InningsOvers;
    public int InningsRuns;
    public int InningsWickets;
}

public class ScoreLive
{
    public Dictionary<string,TeamDeatilsBatting> Score = new();
}

public class TeamDeatilsBatting 
{
    public string Balls;
    public string Four;
    public string Score;
    public string Six;
    public string Status;
    public string StatusDetails;

}
public class BowlingLive
{
    public Dictionary<string, TeamDeatilsBowling> Team = new();
}

public class TeamDeatilsBowling
{
    public string Extra;
    public string Mainden;
    public string Over;
    public string Runs;
    public string Wicket;
}
#endregion

#region EnumCollections
public enum MatchTypeStatus 
{
    Upcoming,
    Live,
    Complete
}

#endregion