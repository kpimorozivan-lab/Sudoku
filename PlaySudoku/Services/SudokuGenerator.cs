using System;
using PlaySudoku.Constants;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class SudokuGenerator : ISudokuGenerator
    {
        private readonly ISudokuSolver _solver;
        private readonly Random _random;

        private const int EASY_MIN = 20;
        private const int EASY_MAX = 31;
        private const int MEDIUM_MIN = 40;
        private const int MEDIUM_MAX = 51;
        private const int HARD_MIN = 60;
        private const int HARD_MAX = 71;

        public SudokuGenerator(ISudokuSolver solver)
        {
            _solver = solver ?? throw new ArgumentNullException(nameof(solver));
            _random = new Random();
        }

        public ISudokuBoard GeneratePuzzle(Difficulty difficulty)
        {
            var board = new SudokuBoard();
            _solver.Solve(board);

            int numbersToRemove = difficulty switch
            {
                Difficulty.Easy => _random.Next(EASY_MIN, EASY_MAX),
                Difficulty.Medium => _random.Next(MEDIUM_MIN, MEDIUM_MAX),
                Difficulty.Hard => _random.Next(HARD_MIN, HARD_MAX),
                _ => _random.Next(MEDIUM_MIN, MEDIUM_MAX)
            };

            for (int i = 0; i < numbersToRemove; i++)
            {
                int row, col;
                do
                {
                    row = _random.Next(GameConstants.BOARD_SIZE);
                    col = _random.Next(GameConstants.BOARD_SIZE);
                } while (board.GetCell(row, col).Value == 0);

                board.SetCell(row, col, 0, false);
            }

            for (int row = 0; row < GameConstants.BOARD_SIZE; row++)
            for (int col = 0; col < GameConstants.BOARD_SIZE; col++)
                if (board.GetCell(row, col).Value != 0)
                    board.GetCell(row, col).IsFixed = true;

            return board;
        }
    }
}