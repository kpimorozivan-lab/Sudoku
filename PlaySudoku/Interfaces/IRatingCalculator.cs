using System;
using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface IRatingCalculator
    {
        int CalculateRatingChange(bool isWon, TimeSpan duration, Difficulty difficulty);
    }
}