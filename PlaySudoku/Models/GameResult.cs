using System;

namespace PlaySudoku.Models
{
    public class GameResult
    {
        public bool IsWon { get; set; }
        public TimeSpan Duration { get; set; }
        public int RatingChange { get; set; }
    }
}