using Application.Abstraction.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.UnitOfWork
{
    public interface IUnitOfWork
    {
       Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task<int> SaveChangesAsync();
        Task RollbackTransactionAsync();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        public Task<int> SaveTransactionAsync(Action action);
        IUserRepository UserRepository { get; }
        IDoctorJobTitleRepository  DoctorJobTitleRepository { get; }
    }
}
