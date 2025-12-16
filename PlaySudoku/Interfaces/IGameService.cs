using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface IGameService
    {
        void StartNewGame(User user, Difficulty difficulty);
        bool MakeMove(int row, int col, int value);
        bool CheckSolution();
        void ShowHint();
        GameResult FinishGame(bool isWon);
        ISudokuBoard GetCurrentBoard();
        ISudokuBoard GetSolutionBoard();
        string GetBoardDisplay(int cursorRow = -1, int cursorCol = -1);
    }
}