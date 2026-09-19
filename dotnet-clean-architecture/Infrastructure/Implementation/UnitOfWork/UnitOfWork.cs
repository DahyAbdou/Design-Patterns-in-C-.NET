using Application.Abstraction.Repository;
using Application.Abstraction.UnitOfWork;
using Infrastructure.Implementation.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        private readonly ILogger<UnitOfWork> _logger;
        private Lazy<IUserRepository> _userRepository;
        private Lazy<IDoctorJobTitleRepository> _doctorJobTitleRepository;
        public IUserRepository UserRepository => _userRepository.Value;
        public IDoctorJobTitleRepository  DoctorJobTitleRepository => _doctorJobTitleRepository.Value;
        public UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
            _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_context));
            _doctorJobTitleRepository=new Lazy<IDoctorJobTitleRepository>(() => new DoctorJobTitleRepository(_context));

        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        private Lazy<IGenericRepository<TEntity>> _genericRepository<TEntity>()
          where TEntity : class
        {
            return new Lazy<IGenericRepository<TEntity>>(() => new GenericRepository<TEntity>(_context));
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return _genericRepository<TEntity>().Value;
        }

        public async Task<int> SaveTransactionAsync(Action dml)//dml Data Manipulation Language
        {
            int result;
            try
            {
                await BeginTransactionAsync();
                dml();
                result = await SaveChangesAsync();
                await CommitAsync();
                return result;
            }
            catch (Exception e)
            {
                _logger.LogWarning($"message : {e.Message} {Environment.NewLine}  stack trace :{e.StackTrace}");
                await RollbackTransactionAsync();
                return 0;
            }
        }

      
    }
}
