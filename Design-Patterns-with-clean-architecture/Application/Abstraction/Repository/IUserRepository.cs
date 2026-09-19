using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.Repository
{
    public interface IUserRepository
    {
        // add uintofwork for new functions needed to this entity
        Task<User> GetUserByIdAsync(int id);
    }
}
