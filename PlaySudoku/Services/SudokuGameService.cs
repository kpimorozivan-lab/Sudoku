using System;
using System.Text;
using PlaySudoku.Constants;
using PlaySudoku.Data;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.Services
{
    public class SudokuGameService : IGameService
    {
        private readonly GameHistoryDatabase _historyDb;
        private readonly UserDatabase _userDb;
        private readonly ISudokuSolver _solver;
        private readonly ISudokuGenerator _generator;
        private readonly IRatingCalculator _ratingCalculator;

        private ISudokuBoard _currentBoard;
        private ISudokuBoard _solutionBoard;
        private ISudokuBoard _initialBoard;
        private User _currentUser;
        private DateTime _gameStartTime;
        private Difficulty _currentDifficulty;

        public SudokuGameService(
            GameHistoryDatabase historyDb,
            UserDatabase userDb,
            ISudokuSolver solver,
            ISudokuGenerator generator,
            IRatingCalculator ratingCalculator)
        {
            _historyDb = historyDb ?? throw new ArgumentNullException(nameof(historyDb));
            _userDb = userDb ?? throw new ArgumentNullException(nameof(userDb));
            _solver = solver ?? throw new ArgumentNullException(nameof(solver));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
            _ratingCalculator = ratingCalculator ?? throw new ArgumentNullException(nameof(ratingCalculator));
        }

        public void StartNewGame(User user, Difficulty difficulty)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _currentUser = user;
            _currentDifficulty = difficulty;
            _currentBoard = _generator.GeneratePuzzle(difficulty);
            _solutionBoard = new SudokuBoard(_currentBoard as SudokuBoard);
            _solver.Solve(_solutionBoard);
            _initialBoard = new SudokuBoard(_currentBoard as SudokuBoard);
            _gameStartTime = DateTime.Now;
        }

        public bool MakeMove(int row, int col, int value)
        {
            if (_currentBoard == null)
                throw new InvalidOperationException("The game has not started");

            if (row < 0 || row >= GameConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(row), "Incorrect line number");

            if (col < 0 || col >= GameConstants.BOARD_SIZE)
                throw new ArgumentOutOfRangeException(nameof(col), "Incorrect column number");

            if (value < GameConstants.MIN_NUMBER || value > GameConstants.MAX_NUMBER)
                throw new ArgumentOutOfRangeException(nameof(value), "The value must be between 0 and 9.");

            if (_currentBoard.GetCell(row, col).IsFixed)
                return false;

            _currentBoard.SetCell(row, col, value, false);
            return true;
        }

        public bool CheckSolution()
        {
            if (_currentBoard == null || _solutionBoard == null || _initialBoard == null)
                throw new InvalidOperationException("The game has not started");

            return _currentBoard.DiffersFrom(_initialBoard) &&
                   _currentBoard.Equals(_solutionBoard);
        }

        public void ShowHint()
        {
            if (_currentBoard == null)
                throw new InvalidOperationException("The game has not started");

            Console.WriteLine($"\nThere are empty cells left: {_currentBoard.CountEmptyCells()}");
            var missing = _currentBoard.CountMissingNumbers();
            Console.WriteLine("Missing numbers:");
            for (int num = 1; num <= GameConstants.MAX_NUMBER; num++)
            {
                if (missing[num] > 0)
                    Console.WriteLine($"{missing[num]}x {num}");
            }
        }

        public GameResult FinishGame(bool isWon)
        {
            if (_currentUser == null)
                throw new InvalidOperationException("User not specified");

            var duration = DateTime.Now - _gameStartTime;
            int ratingChange = _ratingCalculator.CalculateRatingChange(isWon, duration, _currentDifficulty);

            _currentUser.Rating += ratingChange;
            _userDb.Update(_currentUser);

            var history = new GameHistory
            {
                UserId = _currentUser.Id,
                Difficulty = _currentDifficulty,
                IsWon = isWon,
                Duration = duration,
                RatingChange = ratingChange
            };

            _historyDb.Add(history);

            return new GameResult
            {
                IsWon = isWon,
                Duration = duration,
                RatingChange = ratingChange
            };
        }

        public ISudokuBoard GetCurrentBoard() => _currentBoard;
        public ISudokuBoard GetSolutionBoard() => _solutionBoard;

        public string GetBoardDisplay(int cursorRow = -1, int cursorCol = -1)
        {
            if (_currentBoard == null)
                throw new InvalidOperationException("The game has not started");

            var sb = new StringBuilder();
            for (int row = 0; row < GameConstants.BOARD_SIZE; row++)
            {
                for (int col = 0; col < GameConstants.BOARD_SIZE; col++)
                {
                    var cell = _currentBoard.GetCell(row, col);
                    sb.Append(cell.Value != 0 ? cell.Value.ToString() : " ");
                    if (col < 8) sb.Append(" | ");
                }
                sb.AppendLine();
                if (row < 8)
                    sb.AppendLine("----------------------------------");
            }
            return sb.ToString();
        }
    }
}