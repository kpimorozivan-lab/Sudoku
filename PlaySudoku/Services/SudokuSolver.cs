using System;
using System.Linq;
using PlaySudoku.Constants;
using PlaySudoku.Interfaces;

namespace PlaySudoku.Services
{
    public class SudokuSolver : ISudokuSolver
    {
        private readonly Random _random;

        public SudokuSolver()
        {
            _random = new Random();
        }

        public bool Solve(ISudokuBoard board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));

            for (int row = 0; row < GameConstants.BOARD_SIZE; row++)
            {
                for (int col = 0; col < GameConstants.BOARD_SIZE; col++)
                {
                    if (board.GetCell(row, col).Value == 0)
                    {
                        var numbers = Enumerable.Range(1, GameConstants.MAX_NUMBER)
                            .OrderBy(x => _random.Next())
                            .ToArray();
                        
                        foreach (int num in numbers)
                        {
                            if (IsValid(board, row, col, num))
                            {
                                board.SetCell(row, col, num, false);
                                if (Solve(board)) return true;
                                board.SetCell(row, col, 0, false);
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValid(ISudokuBoard board, int row, int col, int num)
        {
            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
            {
                if (board.GetCell(row, i).Value == num || board.GetCell(i, col).Value == num)
                    return false;
            }
            
            int startRow = row - row % GameConstants.BOX_SIZE;
            int startCol = col - col % GameConstants.BOX_SIZE;

            for (int i = 0; i < GameConstants.BOX_SIZE; i++)
            {
                for (int j = 0; j < GameConstants.BOX_SIZE; j++)
                {
                    if (board.GetCell(i + startRow, j + startCol).Value == num)
                        return false;
                }
            }

            return true;
        }
    }
}