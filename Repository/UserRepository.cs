using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    using BusinessObject;
    using Microsoft.EntityFrameworkCore;

    public class UserRepository : IUserRepository
    {
        private readonly HotelBookingDbContext _context;

        public UserRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        // 🟢 Lấy user theo email
        public async Task<User?> GetByEmail(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        // 🟢 Lấy user theo token verify
        public async Task<User?> GetByVerifyToken(string token)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.VerifyToken == token);
        }

        // 🟢 Thêm user
        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // 🟢 Update user
        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
