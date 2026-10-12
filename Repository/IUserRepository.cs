using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;

namespace Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByEmail(string email);
        Task<User?> GetByVerifyToken(string token);
        Task Add(User user);
        Task Update(User user);
    }
}
