using Application.Abstraction;
using Application.Abstraction.Repository;
using Application.Abstraction.UnitOfWork;
using Application.Common.Exceptions;
using Application.Implementation;
using Application.Request.User;
using Application.Response.User;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.Enums;

namespace Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserByIdAsync(GetUserByIdRequest request)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.Id);

            if (user == null)
                throw new CustomHttpException(System.Net.HttpStatusCode.NotFound, "user is not found");

            if (user.Status == (int)UserStatusEnum.Disabled)
                throw new CustomHttpException(System.Net.HttpStatusCode.BadRequest, "user should be activated");

            var mappedUser = _mapper.Map<User, UserDTO>(user);

            return mappedUser;
        }

        public async Task<UserDTO> GetUserByIdUsingGenericRepositoryAndUnitOfWorkAsync(GetUserByIdRequest request)
        {
            var user =await _unitOfWork.Repository<User>().FindByIdAsync(request.Id);

            if (user == null)
                throw new CustomHttpException(System.Net.HttpStatusCode.NotFound, "user is not found");

            if (user.Status == (int)UserStatusEnum.Disabled)
                throw new CustomHttpException(System.Net.HttpStatusCode.BadRequest, "user should be activated");

            var mappedUser = _mapper.Map<User, UserDTO>(user);

            return mappedUser;
        }
    }
}
