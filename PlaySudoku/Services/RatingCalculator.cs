using System;
using PlaySudoku.Constants;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class RatingCalculator : IRatingCalculator
    {
        private const int BASE_POINTS_EASY = 30;
        private const int BASE_POINTS_MEDIUM = 50;
        private const int BASE_POINTS_HARD = 70;
        private const int SPEED_BONUS_FAST = 30;
        private const int SPEED_BONUS_MEDIUM = 20;
        private const int SPEED_BONUS_SLOW = 10;

        public int CalculateRatingChange(bool isWon, TimeSpan duration, Difficulty difficulty)
        {
            if (!isWon) return GameConstants.LOSS_PENALTY;

            int basePoints = difficulty switch
            {
                Difficulty.Easy => BASE_POINTS_EASY,
                Difficulty.Medium => BASE_POINTS_MEDIUM,
                Difficulty.Hard => BASE_POINTS_HARD,
                _ => BASE_POINTS_MEDIUM
            };

            if (duration.TotalMinutes < 5) basePoints += SPEED_BONUS_FAST;
            else if (duration.TotalMinutes < 10) basePoints += SPEED_BONUS_MEDIUM;
            else if (duration.TotalMinutes < 15) basePoints += SPEED_BONUS_SLOW;

            return basePoints;
        }
    }
}