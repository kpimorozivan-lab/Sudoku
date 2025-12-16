using System.Collections.Generic;
using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface ISudokuBoard
    {
        SudokuCell GetCell(int row, int col);
        void SetCell(int row, int col, int value, bool isFixed);
        bool DiffersFrom(ISudokuBoard other);
        int CountEmptyCells();
        Dictionary<int, int> CountMissingNumbers();
    }
}