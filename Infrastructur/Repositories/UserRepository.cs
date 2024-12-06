using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using Infrastructur.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructur.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Realdatabase _context;
       

        public UserRepository(Realdatabase context)
        {
            _context = context;
         
        }

        public async Task<User> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken)
        {
            return await _context.User
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password, cancellationToken);
        }
    }
}

