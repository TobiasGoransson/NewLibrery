using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Interfaces.RepoInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetByCredentialsAsync(string username, string password, CancellationToken cancellationToken);
    }

}
