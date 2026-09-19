using Application.Abstraction.Repository;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Repository
{
    public class UserDapperRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";

        public UserDapperRepository(IConfiguration config)
        {
            _config = config;
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            
            var user = await db.QueryFirstOrDefaultAsync<User>($"select * from user where id=@p0",id);

            return user;
        }
    }
}
