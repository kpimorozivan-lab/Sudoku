using System;
using PlaySudoku.Constants;

namespace PlaySudoku.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            Rating = GameConstants.INITIAL_RATING;
        }
    }
}