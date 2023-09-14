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
    public int rankWinPrevious;
    public int rankGoldSpendPrevious;
    public bool rankWinPreviousAvailable;
    public bool rankGoldSpendPreviousAvailable;
    public List<int> winReward;
    public List<int> goldReward;
    public List<LeaderBoardWinInfo> winInfos;
    public List<LeaderBoardGoldInfo> goldInfos;

    public static int GetIconReward(int rank)
    {
        if (rank < 1)
        {
            return 0;
        }
        else if (rank < 2)
        {
            return 1;
        }
        else if (rank < 3)
        {
            return 2;
        }
        else if (rank < 10)
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }
}
