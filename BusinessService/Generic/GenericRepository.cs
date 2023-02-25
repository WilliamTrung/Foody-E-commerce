using ApplicationCore;
using BusinessService.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        public readonly FoodyContext _context;
        public readonly IUnitOfWork _unitOfWork;
        private DbSet<TEntity> _entities;
        public GenericRepository(FoodyContext context, IUnitOfWork unitOfWork)
        {
            _context= context;
            _entities = _context.Set<TEntity>();
            _unitOfWork= unitOfWork;
        }
        public virtual async Task Add(TEntity entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity entity)
        {
            if(entity is IsDelete)
            {
                ((IsDelete)entity).IsDeleted = true;
            } else
            {
                _entities.Remove(entity);
            }
            
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? expression = null, params string[] includeProperties)
        {
            var filter = _entities.AsNoTracking();
            foreach(var property in includeProperties)
            {
                filter.Include(property);
            }
            if(expression != null)
            {
                filter = filter.Where(expression);
            }
            return await filter.ToListAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
