using Application.Request.User;
using Application.Response.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IUserService 
    {
        Task<UserDTO> GetUserByIdAsync(GetUserByIdRequest request);
        Task<UserDTO> GetUserByIdUsingGenericRepositoryAndUnitOfWorkAsync(GetUserByIdRequest request);
    }
}
