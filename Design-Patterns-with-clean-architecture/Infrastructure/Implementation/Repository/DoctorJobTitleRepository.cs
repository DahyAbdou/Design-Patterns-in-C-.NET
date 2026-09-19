using Application.Abstraction.Repository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Infrastructure.Implementation.Repository
{
    public class DoctorJobTitleRepository :IDoctorJobTitleRepository
    {
        private readonly ApplicationDbContext _db;
        public DoctorJobTitleRepository(ApplicationDbContext db)
        {

            _db = db;

        }
        public async Task<DoctorJobTitle> GetDoctorJobTitleByNameAsync(string name)
        {
            var doctorJobTitle = await _db.DoctorJobTitle.FirstOrDefaultAsync(x => x.NameAr.Contains(name)||x.NameEn.Contains(name));

            return doctorJobTitle;
        }


        public async Task<DoctorJobTitle> GetByIdAsync(object id)
        {
            return await _db.DoctorJobTitle.FirstOrDefaultAsync(ent => ent.Id.Equals(id));
        }

        public async Task InsertAsync(DoctorJobTitle entity)
        {
            await _db.DoctorJobTitle.AddAsync(entity);
        }



        public IQueryable<DoctorJobTitle> Find(Expression<Func<DoctorJobTitle, bool>> filter = null, Func<IQueryable<DoctorJobTitle>, IOrderedQueryable<DoctorJobTitle>> orderBy = null, params Expression<Func<DoctorJobTitle, object>>[] includeProperties)
        {
            IQueryable<DoctorJobTitle> query = _db.DoctorJobTitle;

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

        public async Task<DoctorJobTitle> FindByIdAsync(object id)
        {
            return await _db.DoctorJobTitle.FirstOrDefaultAsync(ent => ent.Id.Equals(id));
        }

        public async Task UpdateAsync(DoctorJobTitle entity)
        {
            await Update(entity);
        }

        public async Task DeletetAsync(object id)
        {
            _db.DoctorJobTitle.FromSqlRaw($"delete from DoctorJobTitle where Id=@id", id);
        }


        private async Task Update(DoctorJobTitle entityToUpdate)
        {
            if (_db.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _db.DoctorJobTitle.Attach(entityToUpdate);
            }

            _db.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
