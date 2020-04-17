using System;
using System.Threading.Tasks;
using DatingApp.API.vscode.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.vscode.Data
{
    public class AuthRepository : IAuthRepository
    {
        public DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string name, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == name);

            if(user == null)
                return null;
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                byte [] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string name)
        {
            if(await _context.Users.AnyAsync(x => x.Name == name))
                return true;

            return false;
        }
    }
}