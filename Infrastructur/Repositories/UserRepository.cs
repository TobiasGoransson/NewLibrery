using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructur.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;
        private readonly DbSet<User> _userSet;

        public UserRepository(DbContext context)
        {
            _context = context;
            _userSet = context.Set<User>();
        }

        public async Task<User> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken)
        {
            return await _userSet
                .FirstOrDefaultAsync(u => u.UserName == username && u.Password == password, cancellationToken);
        }
    }

}
