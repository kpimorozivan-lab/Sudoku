using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface ISudokuGenerator
    {
        ISudokuBoard GeneratePuzzle(Difficulty difficulty);
    }
}