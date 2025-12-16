using System;
using PlaySudoku.Constants;
using PlaySudoku.Interfaces;
using PlaySudoku.Models;

namespace PlaySudoku.UI
{
    public class ConsoleUI
    {
        private readonly IAuthService _authService;
        private readonly IGameService _gameService;
        private readonly IStatisticsService _statisticsService;

        public ConsoleUI(IAuthService authService, IGameService gameService, IStatisticsService statisticsService)
        {
            _authService = authService;
            _gameService = gameService;
            _statisticsService = statisticsService;
        }

        public void Run()
        {
            while (true)
            {
                if (!_authService.IsLoggedIn())
                {
                    if (!ShowAuthMenu()) break;
                }
                else
                {
                    if (!ShowMainMenu()) break;
                }
            }
        }

        private bool ShowAuthMenu()
        {
            Console.Clear();
            Console.WriteLine("SUDOKU");
            Console.WriteLine();
            Console.WriteLine("1. Registration");
            Console.WriteLine("2. Login");
            Console.WriteLine("0. Exit");
            Console.Write("Choice: ");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1": Register(); break;
                case "2": Login(); break;
                case "0": return false;
            }
            return true;
        }

        private void Register()
        {
            Console.Clear();
            Console.Write("User: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            try
            {
                _authService.Register(username, password);
                Console.WriteLine("Registration successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Pause();
        }

        private void Login()
        {
            Console.Clear();
            Console.Write("User: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            try
            {
                _authService.Login(username, password);
                Console.WriteLine("Login completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            Pause();
        }

        private bool ShowMainMenu()
        {
            Console.Clear();
            var user = _authService.GetCurrentUser();
            Console.WriteLine($"User: {user.Username} | Rating: {user.Rating}");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Statistics");
            Console.WriteLine("3. Liderboard");
            Console.WriteLine("4. Log out of your account");
            Console.WriteLine("0. Exit the program");
            Console.Write("Choice: ");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1": StartGame(); break;
                case "2": ShowStatistics(); break;
                case "3": ShowLeaderboard(); break;
                case "4": _authService.Logout(); break;
                case "0": return false;
            }
            return true;
        }

        private void StartGame()
        {
            Console.Clear();
            Console.WriteLine("Difficulty:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.Write("Choice: ");

            string? diffInput = Console.ReadLine();
            Difficulty difficulty = diffInput switch
            {
                "1" => Difficulty.Easy,
                "2" => Difficulty.Medium,
                "3" => Difficulty.Hard,
                _ => Difficulty.Medium
            };

            _gameService.StartNewGame(_authService.GetCurrentUser(), difficulty);

            int cursorRow = 0;
            int cursorCol = 0;

            while (true)
            {
                Console.Clear();
                PrintBoardWithCursor(cursorRow, cursorCol);
                
                Console.WriteLine();
                Console.WriteLine("Controls:");
                Console.WriteLine("  Arrow Keys - Move cursor");
                Console.WriteLine("  1-9 - Enter number");
                Console.WriteLine("  0 - Clear cell");
                Console.WriteLine("  Enter - Check solution");
                Console.WriteLine("  H - Show hint");
                Console.WriteLine("  L - Show solution");
                Console.WriteLine("  R - Restart with new puzzle");
                Console.WriteLine("  Q - Quit to menu");

                var keyInfo = Console.ReadKey(true);
                char pressedChar = char.ToUpper(keyInfo.KeyChar);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (cursorRow > 0) cursorRow--;
                        break;

                    case ConsoleKey.DownArrow:
                        if (cursorRow < GameConstants.BOARD_SIZE - 1) cursorRow++;
                        break;

                    case ConsoleKey.LeftArrow:
                        if (cursorCol > 0) cursorCol--;
                        break;

                    case ConsoleKey.RightArrow:
                        if (cursorCol < GameConstants.BOARD_SIZE - 1) cursorCol++;
                        break;

                    case ConsoleKey.D0:
                    case ConsoleKey.NumPad0:
                        TrySetCell(cursorRow, cursorCol, 0);
                        break;

                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        TrySetCell(cursorRow, cursorCol, 1);
                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        TrySetCell(cursorRow, cursorCol, 2);
                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        TrySetCell(cursorRow, cursorCol, 3);
                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        TrySetCell(cursorRow, cursorCol, 4);
                        break;

                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        TrySetCell(cursorRow, cursorCol, 5);
                        break;

                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        TrySetCell(cursorRow, cursorCol, 6);
                        break;

                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        TrySetCell(cursorRow, cursorCol, 7);
                        break;

                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        TrySetCell(cursorRow, cursorCol, 8);
                        break;

                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        TrySetCell(cursorRow, cursorCol, 9);
                        break;

                    case ConsoleKey.Enter:
                        bool win = _gameService.CheckSolution();
                        var result = _gameService.FinishGame(win);
                        Console.Clear();
                        PrintBoardWithCursor(-1, -1);
                        Console.WriteLine();
                        Console.WriteLine(win ? "Congratulations! You won!" : "Solution is incorrect");
                        Console.WriteLine($"Rating change: {result.RatingChange}");
                        Pause();
                        return;
                }
                
                if (pressedChar == 'H')
                {
                    Console.Clear();
                    PrintBoardWithCursor(cursorRow, cursorCol);
                    Console.WriteLine();
                    _gameService.ShowHint();
                    Pause();
                }
                else if (pressedChar == 'L')
                {
                    Console.Clear();
                    Console.WriteLine("Solution:");
                    PrintSolutionBoard();
                    Pause();
                }
                else if (pressedChar == 'R')
                {
                    if (ConfirmRestart(ref difficulty))
                    {
                        _gameService.StartNewGame(_authService.GetCurrentUser(), difficulty);
                        cursorRow = 0;
                        cursorCol = 0;
                    }
                }
                else if (pressedChar == 'Q')
                {
                    return;
                }
            }
        }

        private bool ConfirmRestart(ref Difficulty currentDifficulty)
        {
            Console.Clear();
            Console.WriteLine("Select new difficulty:");
            Console.WriteLine("1. Easy");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Hard");
            Console.WriteLine("Press any other key to cancel");
            
            var key = Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    currentDifficulty = Difficulty.Easy;
                    return true;
                    
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    currentDifficulty = Difficulty.Medium;
                    return true;
                    
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    currentDifficulty = Difficulty.Hard;
                    return true;
                    
                default:
                    return false;
            }
        }

        private void TrySetCell(int row, int col, int value)
        {
            try
            {
                _gameService.MakeMove(row, col, value);
            }
            catch (Exception)
            {
                // Cell is fixed or invalid move - silently ignore
            }
        }

        private void PrintBoardWithCursor(int cursorRow, int cursorCol)
        {
            var board = _gameService.GetCurrentBoard();
            
            // Верхня рамка
            Console.WriteLine("╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");
            
            for (int row = 0; row < GameConstants.BOARD_SIZE; row++)
            {
                Console.Write("║");
                
                for (int col = 0; col < GameConstants.BOARD_SIZE; col++)
                {
                    var cell = board.GetCell(row, col);
                    
                    // Set colors
                    if (row == cursorRow && col == cursorCol)
                    {
                        // Курсор - жовтий фон
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        if (cell.IsFixed)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    }
                    else if (cell.IsFixed)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }
                    else if (cell.Value != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }

                    Console.Write(" " + (cell.Value != 0 ? cell.Value.ToString() : " ") + " ");
                    Console.ResetColor();

                    if (col == 2 || col == 5)
                        Console.Write("║");
                    else if (col < 8)
                        Console.Write("│");
                }
                
                Console.WriteLine("║");

                if (row == 2 || row == 5)
                {
                    Console.WriteLine("╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                }
                else if (row < 8)
                {
                    Console.WriteLine("╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");
                }
            }
            Console.WriteLine("╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
        }

        private void PrintSolutionBoard()
        {
            var board = _gameService.GetSolutionBoard();
            
            Console.WriteLine("╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");
            
            for (int row = 0; row < GameConstants.BOARD_SIZE; row++)
            {
                Console.Write("║");
                
                for (int col = 0; col < GameConstants.BOARD_SIZE; col++)
                {
                    var cell = board.GetCell(row, col);
                    Console.Write(" " + cell.Value + " ");

                    if (col == 2 || col == 5)
                        Console.Write("║");
                    else if (col < 8)
                        Console.Write("│");
                }
                
                Console.WriteLine("║");
                
                if (row == 2 || row == 5)
                {
                    Console.WriteLine("╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                }
                else if (row < 8)
                {
                    Console.WriteLine("╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");
                }
            }
            
            Console.WriteLine("╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
        }

        private void ShowStatistics()
        {
            Console.Clear();
            var user = _authService.GetCurrentUser();
            var history = _statisticsService.GetUserHistory(user.Id);

            Console.WriteLine("History");
            foreach (var g in history)
            {
                Console.WriteLine($"{g.PlayedAt:g} | {g.Difficulty} | {(g.IsWon ? "Win" : "Lose")} | {g.RatingChange}");
            }
            Pause();
        }

        private void ShowLeaderboard()
        {
            Console.Clear();
            Console.WriteLine("Liderboard");
            int rank = 1;
            foreach (var u in _statisticsService.GetLeaderboard())
            {
                Console.WriteLine($"{rank++}. {u.Username} --- {u.Rating}");
            }
            Pause();
        }

        private void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey(true);
        }
    }
}