using System.Collections.Generic;
using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface IStatisticsService
    {
        List<GameHistory> GetUserHistory(int userId);
        int GetUserRating(int userId);
        List<User> GetLeaderboard(int top = 10);
    }
}