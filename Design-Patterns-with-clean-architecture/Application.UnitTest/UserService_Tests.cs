using Application.Abstraction.Repository;
using Application.Abstraction.UnitOfWork;
using Application.Implementation;
using Application.Response.User;
using AutoMapper;
using Domain.Entities;
using Application.Common.Exceptions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Domain.Enums.Enums;

namespace Application.UnitTest
{
    public class UserService_Tests
    {
        [Test]
        public void InValid_USerNullFromDb_GetUserByIdAsync()
        {
            int Id = 2;

            User user = null;

            var userRepoistory = new Mock<IUserRepository>();
            var UnitOfWork = new Mock<IUnitOfWork>();

            userRepoistory.Setup(u => u.GetUserByIdAsync(Id)).Returns(Task.FromResult(user));

            var map = new Mock<IMapper>();

            map.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(new UserDTO());

            var userService = new UserService(userRepoistory.Object, map.Object, UnitOfWork.Object);

            var request = new Request.User.GetUserByIdRequest() { Id = Id };

            Assert.ThrowsAsync<CustomHttpException>(async () => await userService.GetUserByIdAsync(request));
        }

        [Test]
        public void InValid_USerDisabled_GetUserByIdAsync()
        {
            int Id = 2;

            var user = new User() { Status = (int)UserStatusEnum.Disabled };

            var userRepoistory = new Mock<IUserRepository>();
            var UnitOfWork = new Mock<IUnitOfWork>();
            userRepoistory.Setup(u => u.GetUserByIdAsync(Id)).Returns(Task.FromResult(user));

            var map = new Mock<IMapper>();

            map.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(() => new UserDTO());

            var userService = new UserService(userRepoistory.Object, map.Object, UnitOfWork.Object);

            var request = new Request.User.GetUserByIdRequest() { Id = Id };

            Assert.ThrowsAsync(typeof(CustomHttpException), async () => await userService.GetUserByIdAsync(request));
        }

        [Test]
        public void Valid_GetUserByIdAsync()
        {
            int Id = 2;

            var user = new User() { Status = (int)UserStatusEnum.Active };

            var userRepoistory = new Mock<IUserRepository>();
            var UnitOfWork = new Mock<IUnitOfWork>();
            userRepoistory.Setup(u => u.GetUserByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new User()));

            var map = new Mock<IMapper>();

            map.Setup(m => m.Map<User, UserDTO>(It.IsAny<User>())).Returns(() => new UserDTO());

            var userService = new UserService(userRepoistory.Object, map.Object, UnitOfWork.Object);

            var request = new Request.User.GetUserByIdRequest() { Id = Id };

            Assert.DoesNotThrowAsync(async () => await userService.GetUserByIdAsync(request));
        }
    }
}