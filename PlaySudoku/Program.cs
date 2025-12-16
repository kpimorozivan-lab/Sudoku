using PlaySudoku.Data;
using PlaySudoku.Interfaces;
using PlaySudoku.Services;
using PlaySudoku.UI;

namespace PlaySudoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileStorage = new FileUserStorage("users.json");
            var userDb = new UserDatabase(fileStorage);
            var historyDb = new GameHistoryDatabase();

            ISudokuSolver solver = new SudokuSolver();
            ISudokuGenerator generator = new SudokuGenerator(solver);
            IRatingCalculator ratingCalculator = new RatingCalculator();

            IAuthService authService = new AuthService(userDb);
            IGameService gameService = new SudokuGameService(
                historyDb,
                userDb,
                solver,
                generator,
                ratingCalculator);
            IStatisticsService statisticsService = new StatisticsService(historyDb, userDb);

            var ui = new ConsoleUI(authService, gameService, statisticsService);
            ui.Run();
        }
    }
}