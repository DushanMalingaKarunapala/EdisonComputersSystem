using Core.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }


        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();
            Console.WriteLine($"Rows affected: {result}");
        }

        public async Task Delete(int id)
        {
            var item = await GetById(id);

            if(item != null)
            {
                _dbSet.Remove(item);
                await _dbContext.SaveChangesAsync();
            }


        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        

        public async Task Update(T entity)
        {

            var keyproperties = _dbContext.Model?.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault(); // Get the primary key property of the entity type

            if (keyproperties == null)
            {
                throw new InvalidOperationException($"Entity type {typeof(T).Name} does not have a primary key defined.");
            }

            var keyValue = keyproperties.PropertyInfo?.GetValue(entity); // Get the value of the primary key property from the entity


            var existingEntity = await _dbSet.FindAsync(keyValue); // Find the existing entity by its primary key value


            if (existingEntity == null)
            {
                throw new InvalidOperationException($"Entity with primary key {keyValue} not found in the database.");
            }

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity); // Update the existing entity with the new values from the provided entity
            await _dbContext.SaveChangesAsync();
        }
    }
}
