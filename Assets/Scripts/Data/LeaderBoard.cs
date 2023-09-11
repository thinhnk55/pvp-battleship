using System.Collections.Generic;

public class LeaderBoard
{
    public int Version;
    public long Start;
    public int Period;
    public int win;
    public int goldSpend;
    public int rewardWinPrevious;
    public int rewardGoldSpendPrevious;
    public List<int> winReward;
    public List<int> goldReward;
    public List<LeaderBoardWinInfo> winInfos;
    public List<LeaderBoardGoldInfo> goldInfos;
}
