using System;
using System.Collections.Generic;
using PlaySudoku.Constants;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class SudokuBoard : ISudokuBoard
    {
        private readonly SudokuCell[,] board;

        public SudokuBoard()
        {
            board = new SudokuCell[GameConstants.BOARD_SIZE, GameConstants.BOARD_SIZE];
            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                    board[i, j] = new SudokuCell(0, false);
        }

        public SudokuBoard(SudokuBoard other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            board = new SudokuCell[GameConstants.BOARD_SIZE, GameConstants.BOARD_SIZE];
            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                {
                    var cell = other.GetCell(i, j);
                    board[i, j] = new SudokuCell(cell.Value, cell.IsFixed);
                }
        }

        public SudokuCell GetCell(int row, int col)
        {
            ValidateCoordinates(row, col);
            return board[row, col];
        }

        public void SetCell(int row, int col, int value, bool isFixed)
        {
            ValidateCoordinates(row, col);
            if (value < GameConstants.MIN_NUMBER || value > GameConstants.MAX_NUMBER)
                throw new ArgumentOutOfRangeException(nameof(value));

            board[row, col].Value = value;
            board[row, col].IsFixed = isFixed;
        }

        public bool DiffersFrom(ISudokuBoard other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                    if (GetCell(i, j).Value != other.GetCell(i, j).Value)
                        return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ISudokuBoard other)) return false;

            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                    if (GetCell(i, j).Value != other.GetCell(i, j).Value)
                        return false;
            return true;
        }

        public int CountEmptyCells()
        {
            int count = 0;
            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                    if (GetCell(i, j).Value == 0)
                        count++;
            return count;
        }

        public Dictionary<int, int> CountMissingNumbers()
        {
            var counts = new Dictionary<int, int>();
            for (int i = 1; i <= GameConstants.MAX_NUMBER; i++)
                counts[i] = 0;

            for (int i = 0; i < GameConstants.BOARD_SIZE; i++)
                for (int j = 0; j < GameConstants.BOARD_SIZE; j++)
                {
                    int val = GetCell(i, j).Value;
                    if (val != 0) counts[val]++;
                }

            for (int i = 1; i <= GameConstants.MAX_NUMBER; i++)
                counts[i] = GameConstants.BOARD_SIZE - counts[i];

            return counts;
        }

        public override int GetHashCode() => base.GetHashCode();

        private void ValidateCoordinates(int row, int col)
        {
            if (row < 0 || row >= GameConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(row));
            if (col < 0 || col >= GameConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(col));
        }
    }
}