using PlaySudoku.Models;

namespace PlaySudoku.Interfaces
{
    public interface IAuthService
    {
        User Register(string username, string password);
        User Login(string username, string password);
        void Logout();
        User GetCurrentUser();
        bool IsLoggedIn();
    }
}