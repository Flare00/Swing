using System.Collections.Generic;

public interface ILeaderboardChange
{
    public void OnLeaderboardReceive(Leaderboard.ScorePlayer[] list);
    public void OnTop10Receive(Leaderboard.ScorePlayer[] list);
    public void OnAroundReceive(Leaderboard.ScorePlayer[] list);

}
