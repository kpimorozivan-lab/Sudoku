using System;

namespace PlaySudoku.Models
{
    public class GameHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Difficulty Difficulty { get; set; }
        public bool IsWon { get; set; }
        public TimeSpan Duration { get; set; }
        public int RatingChange { get; set; }
        public DateTime PlayedAt { get; set; }

        public GameHistory()
        {
            PlayedAt = DateTime.Now;
        }
    }
}