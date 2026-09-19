using Application.Abstraction.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Repository
{
    public  class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<TEntity> _dbSet;
        public GenericRepository(ApplicationDbContext db)
        {
            this._db = db;
            this._dbSet = this._db.Set<TEntity>();
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }
            else
            {
                return query.AsQueryable();
            }

        }

        public async Task<TEntity> FindByIdAsync(object id)
        {
            return await this._dbSet.FindAsync(id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await this._dbSet.AddAsync(entity);
        }

      

        public async Task InsertRangeAsync(List<TEntity> entities)
        {
            await this._dbSet.AddRangeAsync(entities);
        }

      
        public async Task UpdateAsync(TEntity entity)
        {
            await Update(entity);
        }
    

        public async Task DeletetAsync(object id)
        {
            TEntity entityToDelete = await this._dbSet.FindAsync(id);
             await Delete(entityToDelete);
        }

        private async Task Delete(TEntity entityToDelete)
        {
            if (this._db.Entry(entityToDelete).State == EntityState.Detached)
            {
                this._dbSet.Attach(entityToDelete);
            }

            this._dbSet.Remove(entityToDelete);
        }
        private async Task Update(TEntity entityToUpdate)
        {
            if (this._db.Entry(entityToUpdate).State == EntityState.Detached)
            {
                this._dbSet.Attach(entityToUpdate);
            }

            this._db.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
