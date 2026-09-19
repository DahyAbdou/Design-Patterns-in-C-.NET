using Application.Abstraction.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _db.User.FirstOrDefaultAsync(x => x.Id.Equals(id));

            return user;
        }
    }
}
