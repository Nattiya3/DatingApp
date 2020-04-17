
using System.Threading.Tasks;
using DatingApp.API.vscode.Models;

namespace DatingApp.API.vscode.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);

        Task<User> Login(string name, string password);

        Task<bool> UserExists(string name);
    }
}